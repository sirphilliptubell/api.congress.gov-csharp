using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Congress.Gov.CSharp.Dtos.Amendments;
using Congress.Gov.CSharp.Dtos.Bills;
using Congress.Gov.CSharp.Dtos.Common;
using Congress.Gov.CSharp.Filters.Bills;
using Congress.Gov.CSharp.Http;
using Congress.Gov.CSharp.Internal.Routing;
using Congress.Gov.CSharp.Options;

namespace Congress.Gov.CSharp.Clients.Bills
{
    /// <summary>
    /// Contract for interacting with Congress.gov bill endpoints (/bill).
    /// </summary>
    public interface IBillsClient
    {
        /// <summary>
        /// Enumerates bills sorted by date of latest action.
        /// </summary>
        /// <param name="filters">Optional filters (from/to/sort).</param>
        /// <param name="limit">Optional page size; defaults to client options DefaultLimit.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>IAsyncEnumerable of bill list items.</returns>
        IAsyncEnumerable<BillListItem> ListAsync(BillListFilters? filters = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates bills filtered by the specified congress, sorted by date of latest action.
        /// </summary>
        /// <param name="congress">The congress number (e.g., 117).</param>
        /// <param name="filters">Optional filters (from/to/sort).</param>
        /// <param name="limit">Optional page size; defaults to client options DefaultLimit.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>IAsyncEnumerable of bill list items.</returns>
        IAsyncEnumerable<BillListItem> ListByCongressAsync(int congress, BillListFilters? filters = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates bills filtered by the specified congress and bill type, sorted by date of latest action.
        /// </summary>
        /// <param name="congress">The congress number (e.g., 117).</param>
        /// <param name="billType">The bill type (hr, s, hjres, sjres, hconres, sconres, hres, sres).</param>
        /// <param name="filters">Optional filters (from/to/sort).</param>
        /// <param name="limit">Optional page size; defaults to client options DefaultLimit.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>IAsyncEnumerable of bill list items.</returns>
        IAsyncEnumerable<BillListItem> ListByCongressAndTypeAsync(int congress, string billType, BillListFilters? filters = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Returns detailed information for a specified bill.
        /// </summary>
        /// <param name="congress">The congress number.</param>
        /// <param name="billType">The bill type.</param>
        /// <param name="billNumber">The bill's assigned number.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The bill detail.</returns>
        Task<BillDetail> GetAsync(int congress, string billType, int billNumber, CancellationToken ct = default);

        /// <summary>
        /// Enumerates the list of actions on a specified bill.
        /// </summary>
        IAsyncEnumerable<BillAction> GetActionsAsync(int congress, string billType, int billNumber, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates the list of amendments to a specified bill.
        /// </summary>
        IAsyncEnumerable<AmendmentListItem> GetAmendmentsAsync(int congress, string billType, int billNumber, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates the list of committees associated with a specified bill.
        /// </summary>
        IAsyncEnumerable<CommitteeRef> GetCommitteesAsync(int congress, string billType, int billNumber, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates the list of cosponsors on a specified bill.
        /// </summary>
        IAsyncEnumerable<Cosponsor> GetCosponsorsAsync(int congress, string billType, int billNumber, BillCosponsorFilters? filters = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates the list of related bills to a specified bill.
        /// </summary>
        IAsyncEnumerable<RelatedBill> GetRelatedBillsAsync(int congress, string billType, int billNumber, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the legislative subjects for a specified bill.
        /// </summary>
        Task<BillSubjects> GetSubjectsAsync(int congress, string billType, int billNumber, CancellationToken ct = default);

        /// <summary>
        /// Enumerates the list of summaries for a specified bill.
        /// </summary>
        IAsyncEnumerable<Summary> GetSummariesAsync(int congress, string billType, int billNumber, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates the list of text versions for a specified bill.
        /// </summary>
        IAsyncEnumerable<TextVersion> GetTextVersionsAsync(int congress, string billType, int billNumber, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates the list of titles for a specified bill.
        /// </summary>
        IAsyncEnumerable<TitleEntry> GetTitlesAsync(int congress, string billType, int billNumber, int? limit = null, CancellationToken ct = default);
    }

    /// <summary>
    /// Default implementation of <see cref="IBillsClient"/> that uses the shared request executor for HTTP calls.
    /// </summary>
    public sealed class BillsClient : IBillsClient
    {
        private readonly IRequestExecutor _executor;
        private readonly CongressClientOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="BillsClient"/> class.
        /// </summary>
        /// <param name="executor">Low-level HTTP executor configured by the root client.</param>
        /// <param name="options">Immutable client options (base URL, retry policy, defaults).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="executor"/> or <paramref name="options"/> is null.</exception>
        public BillsClient(IRequestExecutor executor, CongressClientOptions options)
        {
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc />
        public IAsyncEnumerable<BillListItem> ListAsync(BillListFilters? filters = null, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            return PaginationHelper.AutoPaginateItemsAsync<BillsListPage, BillListItem>(
                fetchPage: (offset, lmt, token) => FetchBillsListAsync(PathBuilder.BillList(), filters, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<BillListItem>)page.Bills,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<BillListItem> ListByCongressAsync(int congress, BillListFilters? filters = null, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.BillByCongress(congress);
            return PaginationHelper.AutoPaginateItemsAsync<BillsListPage, BillListItem>(
                fetchPage: (offset, lmt, token) => FetchBillsListAsync(path, filters, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<BillListItem>)page.Bills,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<BillListItem> ListByCongressAndTypeAsync(int congress, string billType, BillListFilters? filters = null, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.BillByCongressAndType(congress, billType);
            return PaginationHelper.AutoPaginateItemsAsync<BillsListPage, BillListItem>(
                fetchPage: (offset, lmt, token) => FetchBillsListAsync(path, filters, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<BillListItem>)page.Bills,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public async Task<BillDetail> GetAsync(int congress, string billType, int billNumber, CancellationToken ct = default)
        {
            var path = PathBuilder.BillDetail(congress, billType, billNumber);
            var page = await _executor.GetFromJsonAsync<BillDetailPage>(path, query: null, ct).ConfigureAwait(false);
            return page.Bill;
        }

        /// <inheritdoc />
        public IAsyncEnumerable<BillAction> GetActionsAsync(int congress, string billType, int billNumber, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.BillActions(congress, billType, billNumber);
            return PaginationHelper.AutoPaginateItemsAsync<BillActionsPage, BillAction>(
                fetchPage: (offset, lmt, token) => FetchPageAsync<BillActionsPage>(path, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<BillAction>)page.Actions,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<AmendmentListItem> GetAmendmentsAsync(int congress, string billType, int billNumber, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.BillAmendments(congress, billType, billNumber);
            return PaginationHelper.AutoPaginateItemsAsync<BillAmendmentsPage, AmendmentListItem>(
                fetchPage: (offset, lmt, token) => FetchPageAsync<BillAmendmentsPage>(path, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<AmendmentListItem>)page.Amendments,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<CommitteeRef> GetCommitteesAsync(int congress, string billType, int billNumber, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.BillCommittees(congress, billType, billNumber);
            return PaginationHelper.AutoPaginateItemsAsync<BillCommitteesPage, CommitteeRef>(
                fetchPage: (offset, lmt, token) => FetchPageAsync<BillCommitteesPage>(path, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<CommitteeRef>)page.Committees,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<Cosponsor> GetCosponsorsAsync(int congress, string billType, int billNumber, BillCosponsorFilters? filters = null, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.BillCosponsors(congress, billType, billNumber);
            return PaginationHelper.AutoPaginateItemsAsync<BillCosponsorsPage, Cosponsor>(
                fetchPage: (offset, lmt, token) => FetchBillCosponsorsAsync(path, filters, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<Cosponsor>)page.Cosponsors,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<RelatedBill> GetRelatedBillsAsync(int congress, string billType, int billNumber, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.BillRelatedBills(congress, billType, billNumber);
            return PaginationHelper.AutoPaginateItemsAsync<BillRelatedBillsPage, RelatedBill>(
                fetchPage: (offset, lmt, token) => FetchPageAsync<BillRelatedBillsPage>(path, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<RelatedBill>)page.RelatedBills,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public async Task<BillSubjects> GetSubjectsAsync(int congress, string billType, int billNumber, CancellationToken ct = default)
        {
            var path = PathBuilder.BillSubjects(congress, billType, billNumber);
            var page = await _executor.GetFromJsonAsync<BillSubjectsPage>(path, query: null, ct).ConfigureAwait(false);
            return page.Subjects;
        }

        /// <inheritdoc />
        public IAsyncEnumerable<Summary> GetSummariesAsync(int congress, string billType, int billNumber, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.BillSummaries(congress, billType, billNumber);
            return PaginationHelper.AutoPaginateItemsAsync<BillSummariesPage, Summary>(
                fetchPage: (offset, lmt, token) => FetchPageAsync<BillSummariesPage>(path, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<Summary>)page.Summaries,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<TextVersion> GetTextVersionsAsync(int congress, string billType, int billNumber, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.BillTextVersions(congress, billType, billNumber);
            return PaginationHelper.AutoPaginateItemsAsync<BillTextVersionsPage, TextVersion>(
                fetchPage: (offset, lmt, token) => FetchPageAsync<BillTextVersionsPage>(path, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<TextVersion>)page.TextVersions,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<TitleEntry> GetTitlesAsync(int congress, string billType, int billNumber, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.BillTitles(congress, billType, billNumber);
            return PaginationHelper.AutoPaginateItemsAsync<BillTitlesPage, TitleEntry>(
                fetchPage: (offset, lmt, token) => FetchPageAsync<BillTitlesPage>(path, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<TitleEntry>)page.Titles,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        private async Task<BillsListPage> FetchBillsListAsync(string path, BillListFilters? filters, int offset, int limit, CancellationToken ct)
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

            return await _executor.GetFromJsonAsync<BillsListPage>(path, query, ct).ConfigureAwait(false);
        }

        private async Task<BillCosponsorsPage> FetchBillCosponsorsAsync(string path, BillCosponsorFilters? filters, int offset, int limit, CancellationToken ct)
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

            return await _executor.GetFromJsonAsync<BillCosponsorsPage>(path, query, ct).ConfigureAwait(false);
        }

        private async Task<TPage> FetchPageAsync<TPage>(string path, int offset, int limit, CancellationToken ct)
        {
            var query = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
            {
                ["offset"] = offset.ToString(System.Globalization.CultureInfo.InvariantCulture),
                ["limit"] = limit.ToString(System.Globalization.CultureInfo.InvariantCulture)
            };

            return await _executor.GetFromJsonAsync<TPage>(path, query, ct).ConfigureAwait(false);
        }

        private static string ToZulu(DateTimeOffset dto) => dto.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
    }
}