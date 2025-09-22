using System;

namespace Congress.Gov.CSharp.Options
{
    /// <summary>
    /// Configurable options for the Congress.gov client including base URL, retry policy, defaults, and formatting.
    /// </summary>
    public class CongressClientOptions
    {
        /// <summary>
        /// Gets or sets the base URL used for all API requests. Defaults to https://api.congress.gov/v3.
        /// Must end with a trailing slash (/) so the version doesn't get lost when combined with relative paths.
        /// </summary>
        public string? BaseUrl { get; set; } = "https://api.congress.gov/v3/";

        /// <summary>
        /// Gets or sets the retry policy used for transient failures (e.g., HTTP 429 / 5xx).
        /// </summary>
        public RetryOptions Retry { get; set; } = new RetryOptions();

        /// <summary>
        /// Gets or sets a value indicating whether the client enforces format=json for all calls by default.
        /// </summary>
        public bool ForceJsonFormat { get; set; } = true;

        /// <summary>
        /// Gets or sets the default page size (limit) used by auto-pagination helpers. The API maximum is 250.
        /// </summary>
        public int DefaultLimit { get; set; } = 250;

        /// <summary>
        /// Gets or sets an optional request timeout applied to the underlying HttpClient, if provided.
        /// </summary>
        public TimeSpan? RequestTimeout { get; set; }

        /// <summary>
        /// Gets or sets a value to append to the User-Agent header for outbound requests, if specified.
        /// </summary>
        public string? UserAgentSuffix { get; set; }
    }

    /// <summary>
    /// Retry policy configuration for handling transient HTTP failures.
    /// </summary>
    public class RetryOptions
    {
        /// <summary>
        /// Gets or sets the maximum number of retry attempts for transient errors (in addition to the initial try).
        /// </summary>
        public int MaxRetries { get; set; } = 3;

        /// <summary>
        /// Gets or sets the base delay used for exponential backoff between retries.
        /// </summary>
        public TimeSpan BaseDelay { get; set; } = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Gets or sets a jitter factor (0.0 - 1.0) applied to the computed backoff to reduce thundering herd.
        /// </summary>
        public double JitterFactor { get; set; } = 0.2;

        /// <summary>
        /// Gets or sets a value indicating whether the client should honor the Retry-After header when present.
        /// </summary>
        public bool RespectRetryAfter { get; set; } = true;
    }
}