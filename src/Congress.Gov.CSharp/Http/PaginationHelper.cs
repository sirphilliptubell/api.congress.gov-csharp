using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Congress.Gov.CSharp.Http
{
    /// <summary>
    /// Offset-based auto-pagination utilities for Congress.gov endpoints.
    /// Provides typed pagination over pages and items.
    /// </summary>
    public static class PaginationHelper
    {
        /// <summary>
        /// Enumerates typed pages by repeatedly invoking the provided fetch delegate, incrementing the offset by <paramref name="limit"/> each time,
        /// until the count of items in a page is less than <paramref name="limit"/> (or zero).
        /// </summary>
        /// <typeparam name="TPage">The DTO type representing a page payload.</typeparam>
        /// <param name="fetchPage">
        /// A delegate that fetches a single page given (offset, limit, ct) and returns a TPage.
        /// The caller is responsible for including offset/limit in the outbound request.
        /// </param>
        /// <param name="startOffset">The starting offset (defaults to 0).</param>
        /// <param name="limit">The page size (defaults to 250 — the API maximum).</param>
        /// <param name="countSelector">
        /// Delegate that returns the number of items present in the fetched page (required to determine continuation).
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>An async sequence of typed pages.</returns>
        public static async IAsyncEnumerable<TPage> AutoPaginatePagesAsync<TPage>(
            Func<int, int, CancellationToken, Task<TPage>> fetchPage,
            int startOffset,
            int limit,
            Func<TPage, int> countSelector,
            [EnumeratorCancellation] CancellationToken ct = default)
        {
            if (fetchPage is null) throw new ArgumentNullException(nameof(fetchPage));
            if (countSelector is null) throw new ArgumentNullException(nameof(countSelector));
            if (limit <= 0) throw new ArgumentOutOfRangeException(nameof(limit), "limit must be greater than zero.");

            var offset = Math.Max(0, startOffset);

            while (true)
            {
                ct.ThrowIfCancellationRequested();

                var page = await fetchPage(offset, limit, ct).ConfigureAwait(false);
                var count = countSelector(page);

                yield return page;

                if (count <= 0 || count < limit)
                {
                    yield break;
                }

                offset += limit;
            }
        }

        /// <summary>
        /// Enumerates items across pages by repeatedly invoking the provided fetch delegate and projecting items from each page,
        /// incrementing the offset by <paramref name="limit"/> each time until the page item count is less than <paramref name="limit"/>.
        /// </summary>
        /// <typeparam name="TPage">The DTO type representing a page payload.</typeparam>
        /// <typeparam name="TItem">The item type extracted from the page.</typeparam>
        /// <param name="fetchPage">Delegate that fetches a page.</param>
        /// <param name="itemsSelector">Projects the items list from a page.</param>
        /// <param name="startOffset">Starting offset (defaults to 0).</param>
        /// <param name="limit">Page size (defaults to 250).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>IAsyncEnumerable of items.</returns>
        public static async IAsyncEnumerable<TItem> AutoPaginateItemsAsync<TPage, TItem>(
            Func<int, int, CancellationToken, Task<TPage>> fetchPage,
            Func<TPage, IReadOnlyList<TItem>> itemsSelector,
            int startOffset,
            int limit,
            [EnumeratorCancellation] CancellationToken ct = default)
        {
            if (fetchPage is null) throw new ArgumentNullException(nameof(fetchPage));
            if (itemsSelector is null) throw new ArgumentNullException(nameof(itemsSelector));
            if (limit <= 0) throw new ArgumentOutOfRangeException(nameof(limit), "limit must be greater than zero.");

            var offset = Math.Max(0, startOffset);

            while (true)
            {
                ct.ThrowIfCancellationRequested();

                var page = await fetchPage(offset, limit, ct).ConfigureAwait(false);
                var items = itemsSelector(page) ?? Array.Empty<TItem>();
                var count = items.Count;

                for (var i = 0; i < count; i++)
                {
                    yield return items[i];
                }

                if (count <= 0 || count < limit)
                {
                    yield break;
                }

                offset += limit;
            }
        }
    }
}