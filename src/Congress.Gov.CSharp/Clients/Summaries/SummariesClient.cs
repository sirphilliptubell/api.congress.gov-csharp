using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Congress.Gov.CSharp.Dtos.Summaries;
using Congress.Gov.CSharp.Filters.Summaries;
using Congress.Gov.CSharp.Http;
using Congress.Gov.CSharp.Internal.Routing;
using Congress.Gov.CSharp.Options;

namespace Congress.Gov.CSharp.Clients.Summaries
{
    /// <summary>
    /// Contract for interacting with Congress.gov summaries endpoints (/summaries).
    /// </summary>
    public interface ISummariesClient
    {
        /// <summary>
        /// Enumerates summaries sorted by date of last update.
        /// </summary>
        /// <param name="filters">Optional filters (from/to/sort).</param>
        /// <param name="limit">Optional page size; defaults to client options DefaultLimit.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>IAsyncEnumerable of summary feed items.</returns>
        IAsyncEnumerable<SummaryFeedItem> ListAsync(SummariesListFilters? filters = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates summaries filtered by the specified congress, sorted by date of last update.
        /// </summary>
        /// <param name="congress">The congress number (e.g., 117).</param>
        /// <param name="filters">Optional filters (from/to/sort).</param>
        /// <param name="limit">Optional page size; defaults to client options DefaultLimit.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>IAsyncEnumerable of summary feed items.</returns>
        IAsyncEnumerable<SummaryFeedItem> ListByCongressAsync(int congress, SummariesListFilters? filters = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates summaries filtered by the specified congress and bill type, sorted by date of last update.
        /// </summary>
        /// <param name="congress">The congress number (e.g., 117).</param>
        /// <param name="billType">The bill type (hr, s, hjres, sjres, hconres, sconres, hres, sres).</param>
        /// <param name="filters">Optional filters (from/to/sort).</param>
        /// <param name="limit">Optional page size; defaults to client options DefaultLimit.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>IAsyncEnumerable of summary feed items.</returns>
        IAsyncEnumerable<SummaryFeedItem> ListByCongressAndBillTypeAsync(int congress, string billType, SummariesListFilters? filters = null, int? limit = null, CancellationToken ct = default);
    }

    /// <summary>
    /// Default implementation of <see cref="ISummariesClient"/> that uses the shared request executor for HTTP calls.
    /// </summary>
    public sealed class SummariesClient : ISummariesClient
    {
        private readonly IRequestExecutor _executor;
        private readonly CongressClientOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SummariesClient"/> class.
        /// </summary>
        /// <param name="executor">Low-level HTTP executor configured by the root client.</param>
        /// <param name="options">Immutable client options (base URL, retry policy, defaults).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="executor"/> or <paramref name="options"/> is null.</exception>
        public SummariesClient(IRequestExecutor executor, CongressClientOptions options)
        {
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc />
        public IAsyncEnumerable<SummaryFeedItem> ListAsync(SummariesListFilters? filters = null, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.SummariesList();
            return PaginationHelper.AutoPaginateItemsAsync<SummariesListPage, SummaryFeedItem>(
                fetchPage: (offset, lmt, token) => FetchSummariesListAsync(path, filters, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<SummaryFeedItem>)page.Summaries,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<SummaryFeedItem> ListByCongressAsync(int congress, SummariesListFilters? filters = null, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.SummariesByCongress(congress);
            return PaginationHelper.AutoPaginateItemsAsync<SummariesListPage, SummaryFeedItem>(
                fetchPage: (offset, lmt, token) => FetchSummariesListAsync(path, filters, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<SummaryFeedItem>)page.Summaries,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<SummaryFeedItem> ListByCongressAndBillTypeAsync(int congress, string billType, SummariesListFilters? filters = null, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.SummariesByCongressAndBillType(congress, billType);
            return PaginationHelper.AutoPaginateItemsAsync<SummariesListPage, SummaryFeedItem>(
                fetchPage: (offset, lmt, token) => FetchSummariesListAsync(path, filters, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<SummaryFeedItem>)page.Summaries,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        private async Task<SummariesListPage> FetchSummariesListAsync(string path, SummariesListFilters? filters, int offset, int limit, CancellationToken ct)
        {
            var query = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
            {
                ["offset"] = offset.ToString(System.Globalization.CultureInfo.InvariantCulture),
                ["limit"] = limit.ToString(System.Globalization.CultureInfo.InvariantCulture)
            };

            if (filters?.FromDateTime is DateTimeOffset from)
            {
                query["fromDateTime"] = ToZulu(from);
            }
            if (filters?.ToDateTime is DateTimeOffset to)
            {
                query["toDateTime"] = ToZulu(to);
            }
            if (!string.IsNullOrWhiteSpace(filters?.Sort))
            {
                query["sort"] = filters!.Sort!;
            }

            return await _executor.GetFromJsonAsync<SummariesListPage>(path, query, ct).ConfigureAwait(false);
        }

        private static string ToZulu(DateTimeOffset dto) => dto.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
    }
}