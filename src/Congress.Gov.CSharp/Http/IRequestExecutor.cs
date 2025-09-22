using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Congress.Gov.CSharp.Options;

namespace Congress.Gov.CSharp.Http
{
    /// <summary>
    /// Abstraction over outbound HTTP to api.congress.gov providing api_key and format injection,
    /// retry/backoff handling, and typed JSON deserialization.
    /// </summary>
    public interface IRequestExecutor
    {
        /// <summary>
        /// Gets the API key used for requests.
        /// </summary>
        string ApiKey { get; }

        /// <summary>
        /// Gets the client options applied to all requests.
        /// </summary>
        CongressClientOptions Options { get; }

        /// <summary>
        /// Sends a prepared HTTP request with resilient retry behavior.
        /// </summary>
        /// <param name="request">The HTTP request message.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The HTTP response message.</returns>
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct = default);

        /// <summary>
        /// Issues a GET request to the given relative path, injecting api_key and format as needed,
        /// and deserializes the JSON response body into the specified type.
        /// </summary>
        /// <typeparam name="TResponse">The response DTO type to deserialize into.</typeparam>
        /// <param name="relativePath">Relative path under the client's BaseAddress (e.g., "/bill").</param>
        /// <param name="query">Optional query key/values to add to the request.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The typed response DTO.</returns>
        Task<TResponse> GetFromJsonAsync<TResponse>(string relativePath, IDictionary<string, string?>? query = null, CancellationToken ct = default);
    }
}