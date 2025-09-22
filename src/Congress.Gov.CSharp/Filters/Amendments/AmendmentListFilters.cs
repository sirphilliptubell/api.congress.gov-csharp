using System;

namespace Congress.Gov.CSharp.Filters.Amendments
{
    /// <summary>
    /// Query parameters for /amendment list endpoints.
    /// </summary>
    public sealed class AmendmentListFilters
    {
        /// <summary>
        /// Gets or sets an optional from date/time to filter by update date (YYYY-MM-DDThh:mm:ssZ).
        /// </summary>
        public DateTimeOffset? FromDateTime { get; set; }

        /// <summary>
        /// Gets or sets an optional to date/time to filter by update date (YYYY-MM-DDThh:mm:ssZ).
        /// </summary>
        public DateTimeOffset? ToDateTime { get; set; }
    }
}