using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Congress.Gov.CSharp.Dtos.Members;
using Congress.Gov.CSharp.Filters.Members;
using Congress.Gov.CSharp.Http;
using Congress.Gov.CSharp.Internal.Routing;
using Congress.Gov.CSharp.Options;

namespace Congress.Gov.CSharp.Clients.Members
{
    /// <summary>
    /// Contract for interacting with Congress.gov member endpoints (/member).
    /// </summary>
    public interface IMembersClient
    {
        /// <summary>
        /// Enumerates members.
        /// </summary>
        /// <param name="filters">Optional filters (from/to update window, currentMember flag).</param>
        /// <param name="limit">Optional page size; defaults to client options DefaultLimit.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>IAsyncEnumerable of member list items.</returns>
        IAsyncEnumerable<MemberListItem> ListAsync(MemberListFilters? filters = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates members filtered by the specified congress.
        /// </summary>
        /// <param name="congress">The congress number (e.g., 118).</param>
        /// <param name="filters">Optional filters (from/to update window, currentMember flag).</param>
        /// <param name="limit">Optional page size; defaults to client options DefaultLimit.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>IAsyncEnumerable of member list items.</returns>
        IAsyncEnumerable<MemberListItem> ListByCongressAsync(int congress, MemberListFilters? filters = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates members filtered by the specified state.
        /// </summary>
        /// <param name="stateCode">Two-letter state code (e.g., MI).</param>
        /// <param name="currentMember">Optional current member flag.</param>
        /// <param name="limit">Optional page size; defaults to client options DefaultLimit.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>IAsyncEnumerable of member list items.</returns>
        IAsyncEnumerable<MemberListItem> ListByStateAsync(string stateCode, bool? currentMember = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates members filtered by the specified state and district.
        /// </summary>
        /// <param name="stateCode">Two-letter state code (e.g., MI).</param>
        /// <param name="district">District number (e.g., 10).</param>
        /// <param name="currentMember">Optional current member flag.</param>
        /// <param name="limit">Optional page size; defaults to client options DefaultLimit.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>IAsyncEnumerable of member list items.</returns>
        IAsyncEnumerable<MemberListItem> ListByStateAndDistrictAsync(string stateCode, int district, bool? currentMember = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates members filtered by congress, state, and district.
        /// </summary>
        /// <param name="congress">The congress number.</param>
        /// <param name="stateCode">Two-letter state code.</param>
        /// <param name="district">District number.</param>
        /// <param name="currentMember">Optional current member flag.</param>
        /// <param name="limit">Optional page size; defaults to client options DefaultLimit.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>IAsyncEnumerable of member list items.</returns>
        IAsyncEnumerable<MemberListItem> ListByCongressStateAndDistrictAsync(int congress, string stateCode, int district, bool? currentMember = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Returns detailed information for a specified member.
        /// </summary>
        /// <param name="bioguideId">The member's bioguide identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Member detail.</returns>
        Task<MemberDetail> GetAsync(string bioguideId, CancellationToken ct = default);

        /// <summary>
        /// Enumerates the list of legislation sponsored by the specified member.
        /// </summary>
        /// <param name="bioguideId">The member's bioguide identifier.</param>
        /// <param name="limit">Optional page size; defaults to client options DefaultLimit.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>IAsyncEnumerable of member legislation items.</returns>
        IAsyncEnumerable<MemberLegislationItem> GetSponsoredLegislationAsync(string bioguideId, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Enumerates the list of legislation cosponsored by the specified member.
        /// </summary>
        /// <param name="bioguideId">The member's bioguide identifier.</param>
        /// <param name="limit">Optional page size; defaults to client options DefaultLimit.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>IAsyncEnumerable of member legislation items.</returns>
        IAsyncEnumerable<MemberLegislationItem> GetCosponsoredLegislationAsync(string bioguideId, int? limit = null, CancellationToken ct = default);
    }

    /// <summary>
    /// Default implementation of <see cref="IMembersClient"/> that uses the shared request executor for HTTP calls.
    /// </summary>
    public sealed class MembersClient : IMembersClient
    {
        private readonly IRequestExecutor _executor;
        private readonly CongressClientOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembersClient"/> class.
        /// </summary>
        /// <param name="executor">Low-level HTTP executor configured by the root client.</param>
        /// <param name="options">Immutable client options (base URL, retry policy, defaults).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="executor"/> or <paramref name="options"/> is null.</exception>
        public MembersClient(IRequestExecutor executor, CongressClientOptions options)
        {
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc />
        public IAsyncEnumerable<MemberListItem> ListAsync(MemberListFilters? filters = null, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.MemberList();
            return PaginationHelper.AutoPaginateItemsAsync<MembersListPage, MemberListItem>(
                fetchPage: (offset, lmt, token) => FetchMembersListAsync(path, filters, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<MemberListItem>)page.Members,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<MemberListItem> ListByCongressAsync(int congress, MemberListFilters? filters = null, int? limit = null, CancellationToken ct = default)
        {
            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.MemberByCongress(congress);
            return PaginationHelper.AutoPaginateItemsAsync<MembersListPage, MemberListItem>(
                fetchPage: (offset, lmt, token) => FetchMembersListAsync(path, filters, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<MemberListItem>)page.Members,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<MemberListItem> ListByStateAsync(string stateCode, bool? currentMember = null, int? limit = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(stateCode)) throw new ArgumentException("State code is required.", nameof(stateCode));

            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.MemberByState(stateCode);
            return PaginationHelper.AutoPaginateItemsAsync<MembersListPage, MemberListItem>(
                fetchPage: (offset, lmt, token) => FetchMembersByStateAsync(path, currentMember, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<MemberListItem>)page.Members,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<MemberListItem> ListByStateAndDistrictAsync(string stateCode, int district, bool? currentMember = null, int? limit = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(stateCode)) throw new ArgumentException("State code is required.", nameof(stateCode));
            if (district <= 0) throw new ArgumentOutOfRangeException(nameof(district), "District must be greater than zero.");

            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.MemberByStateAndDistrict(stateCode, district);
            return PaginationHelper.AutoPaginateItemsAsync<MembersListPage, MemberListItem>(
                fetchPage: (offset, lmt, token) => FetchMembersByStateAsync(path, currentMember, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<MemberListItem>)page.Members,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<MemberListItem> ListByCongressStateAndDistrictAsync(int congress, string stateCode, int district, bool? currentMember = null, int? limit = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(stateCode)) throw new ArgumentException("State code is required.", nameof(stateCode));
            if (district <= 0) throw new ArgumentOutOfRangeException(nameof(district), "District must be greater than zero.");

            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.MemberByCongressStateAndDistrict(congress, stateCode, district);
            return PaginationHelper.AutoPaginateItemsAsync<MembersListPage, MemberListItem>(
                fetchPage: (offset, lmt, token) => FetchMembersByStateAsync(path, currentMember, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<MemberListItem>)page.Members,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public async Task<MemberDetail> GetAsync(string bioguideId, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(bioguideId)) throw new ArgumentException("BioguideId is required.", nameof(bioguideId));

            var path = PathBuilder.MemberDetail(bioguideId);
            var page = await _executor.GetFromJsonAsync<MemberDetailPage>(path, query: null, ct).ConfigureAwait(false);
            return page.Member;
        }

        /// <inheritdoc />
        public IAsyncEnumerable<MemberLegislationItem> GetSponsoredLegislationAsync(string bioguideId, int? limit = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(bioguideId)) throw new ArgumentException("BioguideId is required.", nameof(bioguideId));

            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.MemberSponsoredLegislation(bioguideId);
            return PaginationHelper.AutoPaginateItemsAsync<MemberSponsoredLegislationPage, MemberLegislationItem>(
                fetchPage: (offset, lmt, token) => FetchPageAsync<MemberSponsoredLegislationPage>(path, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<MemberLegislationItem>)page.SponsoredLegislation,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        /// <inheritdoc />
        public IAsyncEnumerable<MemberLegislationItem> GetCosponsoredLegislationAsync(string bioguideId, int? limit = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(bioguideId)) throw new ArgumentException("BioguideId is required.", nameof(bioguideId));

            var pageSize = limit.GetValueOrDefault(_options.DefaultLimit);
            var path = PathBuilder.MemberCosponsoredLegislation(bioguideId);
            return PaginationHelper.AutoPaginateItemsAsync<MemberCosponsoredLegislationPage, MemberLegislationItem>(
                fetchPage: (offset, lmt, token) => FetchPageAsync<MemberCosponsoredLegislationPage>(path, offset, lmt, token),
                itemsSelector: page => (IReadOnlyList<MemberLegislationItem>)page.CosponsoredLegislation,
                startOffset: 0,
                limit: pageSize,
                ct: ct
            );
        }

        private async Task<MembersListPage> FetchMembersListAsync(string path, MemberListFilters? filters, int offset, int limit, CancellationToken ct)
        {
            var query = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
            {
                ["offset"] = offset.ToString(CultureInfo.InvariantCulture),
                ["limit"] = limit.ToString(CultureInfo.InvariantCulture)
            };

            if (filters?.FromDateTime is DateTimeOffset from)
            {
                query["fromDateTime"] = ToZulu(from);
            }
            if (filters?.ToDateTime is DateTimeOffset to)
            {
                query["toDateTime"] = ToZulu(to);
            }
            if (filters?.CurrentMember is bool current)
            {
                query["currentMember"] = current ? "true" : "false";
            }

            return await _executor.GetFromJsonAsync<MembersListPage>(path, query, ct).ConfigureAwait(false);
        }

        private async Task<MembersListPage> FetchMembersByStateAsync(string path, bool? currentMember, int offset, int limit, CancellationToken ct)
        {
            var query = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
            {
                ["offset"] = offset.ToString(CultureInfo.InvariantCulture),
                ["limit"] = limit.ToString(CultureInfo.InvariantCulture)
            };

            if (currentMember is bool cm)
            {
                query["currentMember"] = cm ? "true" : "false";
            }

            return await _executor.GetFromJsonAsync<MembersListPage>(path, query, ct).ConfigureAwait(false);
        }

        private async Task<TPage> FetchPageAsync<TPage>(string path, int offset, int limit, CancellationToken ct)
        {
            var query = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
            {
                ["offset"] = offset.ToString(CultureInfo.InvariantCulture),
                ["limit"] = limit.ToString(CultureInfo.InvariantCulture)
            };

            return await _executor.GetFromJsonAsync<TPage>(path, query, ct).ConfigureAwait(false);
        }

        private static string ToZulu(DateTimeOffset dto) => dto.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
    }
}