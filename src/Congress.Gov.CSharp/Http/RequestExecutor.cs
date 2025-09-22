
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Congress.Gov.CSharp.Options;

namespace Congress.Gov.CSharp.Http
{
    /// <summary>
    /// Default implementation of <see cref="IRequestExecutor"/> that injects required query parameters
    /// (api_key and optionally format=json), applies resilient retries (including Retry-After), and
    /// provides convenience JSON helpers.
    /// </summary>
    public sealed class RequestExecutor : IRequestExecutor
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestExecutor"/> class.
        /// </summary>
        /// <param name="httpClient">The underlying HTTP client. Its BaseAddress must be set.</param>
        /// <param name="apiKey">The API key to inject into requests.</param>
        /// <param name="options">Client options for base URL, retries, defaults.</param>
        public RequestExecutor(HttpClient httpClient, string apiKey, CongressClientOptions options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            if (_httpClient.BaseAddress == null)
            {
                throw new ArgumentException("HttpClient.BaseAddress must be set.", nameof(httpClient));
            }

            ApiKey = string.IsNullOrWhiteSpace(apiKey)
                ? throw new ArgumentException("API key cannot be null or whitespace.", nameof(apiKey))
                : apiKey;

            Options = options ?? throw new ArgumentNullException(nameof(options));

            // Apply optional timeout to the HttpClient if provided.
            if (Options.RequestTimeout.HasValue)
            {
                _httpClient.Timeout = Options.RequestTimeout.Value;
            }

            // Default headers (prefer JSON)
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Optional User-Agent suffix
            if (!string.IsNullOrWhiteSpace(Options.UserAgentSuffix))
            {
                var product = new ProductInfoHeaderValue("Congress.Gov.CSharp", "1.0");
                var comment = new ProductInfoHeaderValue($"(+{Options.UserAgentSuffix})");
                _httpClient.DefaultRequestHeaders.UserAgent.Add(product);
                _httpClient.DefaultRequestHeaders.UserAgent.Add(comment);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Congress.Gov.CSharp", "1.0"));
            }
        }

        /// <inheritdoc />
        public string ApiKey { get; }

        /// <inheritdoc />
        public CongressClientOptions Options { get; }

        /// <summary>
        /// Default JSON serializer options for Congress.gov responses.
        /// </summary>
        private static readonly System.Text.Json.JsonSerializerOptions s_jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
        };

        /// <inheritdoc />
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            // Ensure api_key and format=json (if configured) on the request URI.
            request.RequestUri = EnsureAuthAndFormatOnUri(request.RequestUri);

            var attempt = 0;
            HttpResponseMessage? response = null;

            while (true)
            {
                ct.ThrowIfCancellationRequested();
                attempt++;

                try
                {
                    response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }

                    if (!IsTransient(response.StatusCode))
                    {
                        await ThrowHttpErrorAsync(request, response).ConfigureAwait(false);
                    }

                    if (attempt > Options.Retry.MaxRetries)
                    {
                        await ThrowHttpErrorAsync(request, response).ConfigureAwait(false);
                    }

                    var delay = GetRetryDelay(response, attempt);
                    if (delay > TimeSpan.Zero)
                    {
                        await Task.Delay(delay, ct).ConfigureAwait(false);
                    }

                    // Dispose the previous response before retry
                    response.Dispose();
                    continue;
                }
                catch (OperationCanceledException)
                {
                    response?.Dispose();
                    throw;
                }
                catch (HttpRequestException hrex)
                {
                    if (attempt > Options.Retry.MaxRetries)
                    {
                        response?.Dispose();
                        throw;
                    }

                    var delay = ComputeBackoff(attempt);
                    await Task.Delay(delay, ct).ConfigureAwait(false);
                    response?.Dispose();
                }
            }
        }

        /// <summary>
        /// Issues a GET request to the given relative path, injecting api_key and format as needed,
        /// and deserializes the JSON response body into the specified type.
        /// </summary>
        /// <typeparam name="TResponse">The response DTO type to deserialize into.</typeparam>
        /// <param name="relativePath">Relative path under the client's BaseAddress (e.g., "/bill").</param>
        /// <param name="query">Optional query key/values to add to the request.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The typed response DTO.</returns>
        public async Task<TResponse> GetFromJsonAsync<TResponse>(string relativePath, IDictionary<string, string?>? query = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) throw new ArgumentException("Relative path is required.", nameof(relativePath));

            var uri = BuildRelativeUri(relativePath, query);
            using var request = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await SendAsync(request, ct).ConfigureAwait(false);
            await using var stream = await response.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);

            var result = await System.Text.Json.JsonSerializer.DeserializeAsync<TResponse>(stream, s_jsonOptions, ct).ConfigureAwait(false);
            if (result is null)
            {
                throw new System.Text.Json.JsonException($"Failed to deserialize response body for {request.Method} {request.RequestUri} into {typeof(TResponse).FullName}.");
            }

            return result;
        }

        private static bool IsTransient(HttpStatusCode statusCode)
        {
            var code = (int)statusCode;
            return statusCode == HttpStatusCode.TooManyRequests // 429
                   || (code >= 500 && code <= 599);
        }

        private TimeSpan GetRetryDelay(HttpResponseMessage response, int attempt)
        {
            if (Options.Retry.RespectRetryAfter)
            {
                if (response.Headers.RetryAfter != null)
                {
                    if (response.Headers.RetryAfter.Delta.HasValue)
                    {
                        return response.Headers.RetryAfter.Delta.Value;
                    }

                    if (response.Headers.RetryAfter.Date.HasValue)
                    {
                        var delta = response.Headers.RetryAfter.Date.Value - DateTimeOffset.UtcNow;
                        if (delta > TimeSpan.Zero)
                        {
                            return delta;
                        }
                    }
                }
            }

            return ComputeBackoff(attempt);
        }

        private TimeSpan ComputeBackoff(int attempt)
        {
            // Exponential backoff with jitter
            var baseDelay = Options.Retry.BaseDelay;
            var exponent = Math.Pow(2, Math.Max(0, attempt - 1));
            var delay = TimeSpan.FromMilliseconds(baseDelay.TotalMilliseconds * exponent);

            var jitterFactor = Math.Clamp(Options.Retry.JitterFactor, 0.0, 1.0);
            var jitterMs = delay.TotalMilliseconds * jitterFactor * Random.Shared.NextDouble();
            var withJitter = delay + TimeSpan.FromMilliseconds(jitterMs);

            return withJitter;
        }

        private Uri BuildRelativeUri(string relativePath, IDictionary<string, string?>? query)
        {
            // Start with base + relative
            var uri = new Uri(_httpClient.BaseAddress!, relativePath.TrimStart('/'));

            // Merge existing query with provided query and defaults (api_key/format)
            var merged = ParseQuery(uri.Query);
            if (query != null)
            {
                foreach (var kvp in query)
                {
                    if (kvp.Key is null) continue;
                    merged[kvp.Key] = kvp.Value;
                }
            }

            // Ensure auth and format
            merged["api_key"] = ApiKey;
            if (Options.ForceJsonFormat)
            {
                merged["format"] = "json";
            }

            var qs = BuildQueryString(merged);
            var builder = new UriBuilder(uri)
            {
                Query = qs
            };
            return builder.Uri;
        }

        private Uri EnsureAuthAndFormatOnUri(Uri? input)
        {
            if (input == null)
            {
                // If no URI on request, use base with just the defaults.
                return BuildRelativeUri("/", null);
            }

            // Only recompose when it's relative to BaseAddress
            Uri uri = input.IsAbsoluteUri ? input : new Uri(_httpClient.BaseAddress!, input.ToString());

            var merged = ParseQuery(uri.Query);
            merged["api_key"] = ApiKey;
            if (Options.ForceJsonFormat)
            {
                merged["format"] = "json";
            }

            var qs = BuildQueryString(merged);
            var builder = new UriBuilder(uri)
            {
                Query = qs
            };
            return builder.Uri;
        }

        private static Dictionary<string, string?> ParseQuery(string queryString)
        {
            var dict = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrEmpty(queryString))
            {
                return dict;
            }

            var q = queryString;
            if (q.StartsWith("?", StringComparison.Ordinal)) q = q.Substring(1);

            var segments = q.Split('&', StringSplitOptions.RemoveEmptyEntries);
            foreach (var seg in segments)
            {
                var idx = seg.IndexOf('=', StringComparison.Ordinal);
                if (idx < 0)
                {
                    dict[Uri.UnescapeDataString(seg)] = null;
                    continue;
                }

                var key = Uri.UnescapeDataString(seg.Substring(0, idx));
                var val = Uri.UnescapeDataString(seg.Substring(idx + 1));
                dict[key] = val;
            }

            return dict;
        }

        private static string BuildQueryString(Dictionary<string, string?> values)
        {
            var sb = new StringBuilder();
            var first = true;
            foreach (var kvp in values)
            {
                if (string.IsNullOrWhiteSpace(kvp.Key)) continue;

                if (!first) sb.Append('&');
                first = false;

                sb.Append(Uri.EscapeDataString(kvp.Key));
                if (kvp.Value != null)
                {
                    sb.Append('=');
                    sb.Append(Uri.EscapeDataString(kvp.Value));
                }
            }

            return sb.ToString();
        }

        private static async Task ThrowHttpErrorAsync(HttpRequestMessage request, HttpResponseMessage response)
        {
            string? requestId = null;
            if (response.Headers.TryGetValues("x-request-id", out var ids))
            {
                foreach (var id in ids)
                {
                    requestId = id;
                    break;
                }
            }

            string body = string.Empty;
            if (response.Content != null)
            {
                try
                {
                    body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                catch
                {
                    // ignore body read failures
                }
            }

            var message = $"HTTP {(int)response.StatusCode} {response.ReasonPhrase} for {request.Method} {request.RequestUri}. " +
                          $"Request-Id: {(requestId ?? "n/a")}. " +
                          $"Body: {Truncate(body, 2000)}";

            throw new HttpRequestException(message, null, response.StatusCode);
        }

        private static string Truncate(string value, int max)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Length <= max ? value : value.Substring(0, max) + "...";
        }
    }
}
