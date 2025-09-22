using System;
using System.Net.Http;
using Congress.Gov.CSharp.Clients.Amendments;
using Congress.Gov.CSharp.Clients.Bills;
using Congress.Gov.CSharp.Clients.Summaries;
using Congress.Gov.CSharp.Clients.Congress;
using Congress.Gov.CSharp.Clients.Members;
using Congress.Gov.CSharp.Http;
using Congress.Gov.CSharp.Options;

namespace Congress.Gov.CSharp
{
	/// <summary>
	/// Contract for the Congress.gov API client. This client aggregates all resource sub-clients
	/// and provides shared HTTP plumbing (API key injection, format enforcement, retries).
	/// </summary>
	public interface ICongressClient
	{
		/// <summary>
		/// Gets the API key used to authenticate with api.congress.gov, sourced from constructor or environment.
		/// </summary>
		string ApiKey { get; }

		/// <summary>
		/// Gets the immutable client options applied to all requests (base URL, retry policy, defaults).
		/// </summary>
		CongressClientOptions Options { get; }

		/// <summary>
		/// Gets the base address used for outbound API calls (defaults to https://api.congress.gov/v3).
		/// </summary>
		Uri BaseAddress { get; }

		/// <summary>
		/// Provides access to bill endpoints (/bill).
		/// </summary>
		IBillsClient Bills { get; }

		/// <summary>
		/// Provides access to amendment endpoints (/amendment).
		/// </summary>
		IAmendmentsClient Amendments { get; }

		/// <summary>
		/// Provides access to member endpoints (/member).
		/// </summary>
		IMembersClient Members { get; }
		
		/// <summary>
		/// Provides access to summaries endpoints (/summaries).
		/// </summary>
		ISummariesClient Summaries { get; }
		
		/// <summary>
		/// Provides access to congress endpoints (/congress).
		/// </summary>
		ICongressesClient Congresses { get; }
	}

	/// <summary>
	/// Default implementation of <see cref="ICongressClient"/> that wires baseline HTTP plumbing:
	/// api_key and format=json query injection, configurable base URL, and resilient retry behavior.
	/// </summary>
	public class CongressClient : ICongressClient
	{
		private const string DefaultBaseUrl = "https://api.congress.gov/v3";

		private readonly HttpClient _httpClient;
		private readonly IRequestExecutor _executor;

		/// <summary>
		/// Creates a client using the specified API key or environment variables (CONGRESS_GOV_API_KEY)
		/// and default options. This overload is maintained for compatibility.
		/// </summary>
		/// <param name="apiKey">API key; if null or whitespace, environment variables are consulted.</param>
		public CongressClient(string? apiKey = null)
			: this(apiKey, new CongressClientOptions(), httpClient: null)
		{
		}

		/// <summary>
		/// Creates a client with configurable options and optional <see cref="HttpClient"/>.
		/// </summary>
		/// <param name="apiKey">API key; if null or whitespace, environment variables are consulted.</param>
		/// <param name="options">Client options (base URL, retries, defaults). If null, defaults are used.</param>
		/// <param name="httpClient">
		/// Optional externally managed <see cref="HttpClient"/>. When null, an internal instance is created.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// Thrown when API key is not provided and cannot be resolved from environment.
		/// </exception>
		public CongressClient(string? apiKey, CongressClientOptions? options, HttpClient? httpClient)
		{
			if (string.IsNullOrWhiteSpace(apiKey)) {
				apiKey = Environment.GetEnvironmentVariable("CONGRESS_GOV_API_KEY", EnvironmentVariableTarget.Process)
					?? Environment.GetEnvironmentVariable("CONGRESS_GOV_API_KEY", EnvironmentVariableTarget.User)
					?? Environment.GetEnvironmentVariable("CONGRESS_GOV_API_KEY", EnvironmentVariableTarget.Machine)
					?? throw new InvalidOperationException("Api key is required. It may be specified in a CONGRESS_GOV_API_KEY environment variable.");
			}

			ApiKey = apiKey;
			Options = options ?? new CongressClientOptions();

			var baseUrl = string.IsNullOrWhiteSpace(Options.BaseUrl) ? DefaultBaseUrl : Options.BaseUrl!;
			var baseAddress = new Uri(baseUrl);

			_httpClient = httpClient ?? new HttpClient { BaseAddress = baseAddress };
			if (_httpClient.BaseAddress == null) {
				_httpClient.BaseAddress = baseAddress;
			}

			_executor = new RequestExecutor(_httpClient, ApiKey, Options);

			// Sub-clients
			Bills = new BillsClient(_executor, Options);
			Amendments = new AmendmentsClient(_executor, Options);
			Members = new MembersClient(_executor, Options);
			Summaries = new SummariesClient(_executor, Options);
			Congresses = new CongressesClient(_executor, Options);
		}

		/// <inheritdoc />
		public string ApiKey { get; }

		/// <inheritdoc />
		public CongressClientOptions Options { get; }

		/// <inheritdoc />
		public Uri BaseAddress => _httpClient.BaseAddress ?? new Uri(DefaultBaseUrl, UriKind.Absolute);

		/// <inheritdoc />
		public IBillsClient Bills { get; }
		
		/// <inheritdoc />
		public IAmendmentsClient Amendments { get; }

		/// <inheritdoc />
		public IMembersClient Members { get; }
		
		/// <inheritdoc />
		public ISummariesClient Summaries { get; }
		
		/// <inheritdoc />
		public ICongressesClient Congresses { get; }

		#region Internal plumbing accessors (for future sub-clients)

		/// <summary>
		/// Provides internal access to the low-level request executor for sub-clients.
		/// </summary>
		internal IRequestExecutor Executor => _executor;

		#endregion
	}
}
