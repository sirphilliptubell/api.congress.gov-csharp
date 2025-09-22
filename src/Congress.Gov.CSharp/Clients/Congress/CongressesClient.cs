using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Congress.Gov.CSharp.Dtos.Congress;
using Congress.Gov.CSharp.Http;
using Congress.Gov.CSharp.Internal.Routing;
using Congress.Gov.CSharp.Options;

namespace Congress.Gov.CSharp.Clients.Congress
{
    /// <summary>
    /// Contract for interacting with Congress.gov congress endpoints (/congress).
    /// </summary>
    public interface ICongressesClient
    {
        /// <summary>
        /// Enumerates the list of congresses and congressional sessions.
        /// </summary>
        /// <param name="limit">Optional page size; defaults to client options DefaultLimit.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>IAsyncEnumerable of congress entries.</returns>
        IAsyncEnumerable<CongressEntry> ListAsync(int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Returns detailed information for a specified congress.
        /// </summary>
        /// <param name="congress">The congress number (e.g., 116).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The congress entry.</returns>
        Task<CongressEntry> GetAsync(int congress, CancellationToken ct = default);

        /// <summary>
        /// Returns detailed information for the current congress.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The current congress entry.</returns>
        Task<CongressEntry> GetCurrentAsync(CancellationToken ct = default);
    }

    /// <summary>
    /// Default implementation of <see cref="ICongressesClient"/> that uses the shared request executor for HTTP calls.
    /// </summary>
    public sealed class CongressesClient : ICongressesClient
    {
        private readonly IRequestExecutor _executor;
        private readonly CongressClientOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="CongressesClient"/> class.
        /// </summary>
        /// <param name="executor">Low-level HTTP executor configured by the root client.</param>
        /// <param name="options">Immutable client options (base URL, retry policy, defaults).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="executor"/> or <paramref name="options"/> is null.</exception>
        public CongressesClient(IRequestExecutor executor, CongressClientOptions options)
        {
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc />
        public IAsyncEnumerable<CongressEntry> ListAsync(int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.CongressList();
            return PaginationHelper.AutoPaginateItemsAsync<CongressesListPage, CongressEntry>(
                fetchPage: (offset, lmt, token) => FetchCongressListAsync(path, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<CongressEntry>)page.Congresses,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public async Task<CongressEntry> GetAsync(int congress, CancellationToken ct = default)
        {
            var path = PathBuilder.CongressByNumber(congress);
            var page = await _executor.GetFromJsonAsync<CongressDetailPage>(path, query: null, ct).ConfigureAwait(false);
            return page.Congress;
        }

        /// <inheritdoc />
        public async Task<CongressEntry> GetCurrentAsync(CancellationToken ct = default)
        {
            var path = PathBuilder.CongressCurrent();
            var page = await _executor.GetFromJsonAsync<CongressDetailPage>(path, query: null, ct).ConfigureAwait(false);
            return page.Congress;
        }

        private async Task<CongressesListPage> FetchCongressListAsync(string path, int offset, int limit, CancellationToken ct)
        {
            var query = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
            {
                ["offset"] = offset.ToString(System.Globalization.CultureInfo.InvariantCulture),
                ["limit"] = limit.ToString(System.Globalization.CultureInfo.InvariantCulture)
            };

            return await _executor.GetFromJsonAsync<CongressesListPage>(path, query, ct).ConfigureAwait(false);
        }
    }
}