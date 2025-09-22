using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Congress.Gov.CSharp.Dtos.Amendments;
using Congress.Gov.CSharp.Dtos.Common;
using Congress.Gov.CSharp.Filters.Amendments;
using Congress.Gov.CSharp.Http;
using Congress.Gov.CSharp.Internal.Routing;
using Congress.Gov.CSharp.Options;

namespace Congress.Gov.CSharp.Clients.Amendments
{
    /// <summary>
    /// Contract for interacting with Congress.gov amendment endpoints (/amendment).
    /// </summary>
    public interface IAmendmentsClient
    {
        /// <summary>
        /// Enumerates amendments sorted by date of latest action.
        /// </summary>
        /// <param name="filters">Optional filters (from/to).</param>
        /// <param name="limit">Optional page size; defaults to client options DefaultLimit.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>IAsyncEnumerable of amendment list items.</returns>
        IAsyncEnumerable<AmendmentListItem> ListAsync(AmendmentListFilters? filters = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates amendments filtered by congress, sorted by latest action.
        /// </summary>
        IAsyncEnumerable<AmendmentListItem> ListByCongressAsync(int congress, AmendmentListFilters? filters = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates amendments filtered by congress and amendment type (hamdt, samdt, suamdt), sorted by latest action.
        /// </summary>
        IAsyncEnumerable<AmendmentListItem> ListByCongressAndTypeAsync(int congress, string amendmentType, AmendmentListFilters? filters = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Returns detailed information for a specified amendment.
        /// </summary>
        Task<AmendmentDetail> GetAsync(int congress, string amendmentType, int amendmentNumber, CancellationToken ct = default);

        /// <summary>
        /// Enumerates actions for a specified amendment.
        /// </summary>
        IAsyncEnumerable<AmendmentAction> GetActionsAsync(int congress, string amendmentType, int amendmentNumber, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates cosponsors for a specified amendment.
        /// </summary>
        IAsyncEnumerable<Cosponsor> GetCosponsorsAsync(int congress, string amendmentType, int amendmentNumber, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates amendments to a specified amendment.
        /// </summary>
        IAsyncEnumerable<AmendmentListItem> GetAmendmentsToAmendmentAsync(int congress, string amendmentType, int amendmentNumber, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates text versions for a specified amendment.
        /// </summary>
        IAsyncEnumerable<TextVersion> GetTextVersionsAsync(int congress, string amendmentType, int amendmentNumber, int? limit = null, CancellationToken ct = default);
    }

    /// <summary>
    /// Default implementation of <see cref="IAmendmentsClient"/>.
    /// </summary>
    public sealed class AmendmentsClient : IAmendmentsClient
    {
        private readonly IRequestExecutor _executor;
        private readonly CongressClientOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmendmentsClient"/> class.
        /// </summary>
        /// <param name="executor">Low-level HTTP executor configured by the root client.</param>
        /// <param name="options">Immutable client options (base URL, retry policy, defaults).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="executor"/> or <paramref name="options"/> is null.</exception>
        public AmendmentsClient(IRequestExecutor executor, CongressClientOptions options)
        {
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc />
        public IAsyncEnumerable<AmendmentListItem> ListAsync(AmendmentListFilters? filters = null, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            return PaginationHelper.AutoPaginateItemsAsync<AmendmentsListPage, AmendmentListItem>(
                fetchPage: (offset, lmt, token) => FetchAmendmentsListAsync(PathBuilder.AmendmentList(), filters, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<AmendmentListItem>)page.Amendments,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<AmendmentListItem> ListByCongressAsync(int congress, AmendmentListFilters? filters = null, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.AmendmentByCongress(congress);
            return PaginationHelper.AutoPaginateItemsAsync<AmendmentsListPage, AmendmentListItem>(
                fetchPage: (offset, lmt, token) => FetchAmendmentsListAsync(path, filters, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<AmendmentListItem>)page.Amendments,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<AmendmentListItem> ListByCongressAndTypeAsync(int congress, string amendmentType, AmendmentListFilters? filters = null, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.AmendmentByCongressAndType(congress, amendmentType);
            return PaginationHelper.AutoPaginateItemsAsync<AmendmentsListPage, AmendmentListItem>(
                fetchPage: (offset, lmt, token) => FetchAmendmentsListAsync(path, filters, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<AmendmentListItem>)page.Amendments,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public async Task<AmendmentDetail> GetAsync(int congress, string amendmentType, int amendmentNumber, CancellationToken ct = default)
        {
            var path = PathBuilder.AmendmentDetail(congress, amendmentType, amendmentNumber);
            var page = await _executor.GetFromJsonAsync<AmendmentDetailPage>(path, query: null, ct).ConfigureAwait(false);
            return page.Amendment;
        }

        /// <inheritdoc />
        public IAsyncEnumerable<AmendmentAction> GetActionsAsync(int congress, string amendmentType, int amendmentNumber, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.AmendmentActions(congress, amendmentType, amendmentNumber);
            return PaginationHelper.AutoPaginateItemsAsync<AmendmentActionsPage, AmendmentAction>(
                fetchPage: (offset, lmt, token) => FetchPageAsync<AmendmentActionsPage>(path, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<AmendmentAction>)page.Actions,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<Cosponsor> GetCosponsorsAsync(int congress, string amendmentType, int amendmentNumber, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.AmendmentCosponsors(congress, amendmentType, amendmentNumber);
            return PaginationHelper.AutoPaginateItemsAsync<AmendmentCosponsorsPage, Cosponsor>(
                fetchPage: (offset, lmt, token) => FetchPageAsync<AmendmentCosponsorsPage>(path, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<Cosponsor>)page.Cosponsors,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<AmendmentListItem> GetAmendmentsToAmendmentAsync(int congress, string amendmentType, int amendmentNumber, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.AmendmentsToAmendment(congress, amendmentType, amendmentNumber);
            return PaginationHelper.AutoPaginateItemsAsync<AmendmentsToAmendmentPage, AmendmentListItem>(
                fetchPage: (offset, lmt, token) => FetchPageAsync<AmendmentsToAmendmentPage>(path, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<AmendmentListItem>)page.Amendments,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<TextVersion> GetTextVersionsAsync(int congress, string amendmentType, int amendmentNumber, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.AmendmentTextVersions(congress, amendmentType, amendmentNumber);
            return PaginationHelper.AutoPaginateItemsAsync<AmendmentTextVersionsPage, TextVersion>(
                fetchPage: (offset, lmt, token) => FetchPageAsync<AmendmentTextVersionsPage>(path, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<TextVersion>)page.TextVersions,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        private async Task<AmendmentsListPage> FetchAmendmentsListAsync(string path, AmendmentListFilters? filters, int offset, int limit, CancellationToken ct)
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

            return await _executor.GetFromJsonAsync<AmendmentsListPage>(path, query, ct).ConfigureAwait(false);
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