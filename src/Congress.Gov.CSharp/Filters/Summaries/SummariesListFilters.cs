using System;

namespace Congress.Gov.CSharp.Filters.Summaries
{
    /// <summary>
    /// Query parameters for /summaries list endpoints.
    /// </summary>
    public sealed class SummariesListFilters
    {
        /// <summary>
        /// Gets or sets an optional from date/time to filter by update date (YYYY-MM-DDThh:mm:ssZ).
        /// </summary>
        public DateTimeOffset? FromDateTime { get; set; }

        /// <summary>
        /// Gets or sets an optional to date/time to filter by update date (YYYY-MM-DDThh:mm:ssZ).
        /// </summary>
        public DateTimeOffset? ToDateTime { get; set; }

        /// <summary>
        /// Gets or sets an optional sort expression. Allowed values: "updateDate+asc" or "updateDate+desc".
        /// </summary>
        public string? Sort { get; set; }
    }
}