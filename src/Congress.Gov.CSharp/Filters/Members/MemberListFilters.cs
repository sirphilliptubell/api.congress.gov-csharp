using System;

namespace Congress.Gov.CSharp.Filters.Members
{
    /// <summary>
    /// Filters for member list endpoints that support update-date windowing and current membership flag.
    /// </summary>
    public sealed class MemberListFilters
    {
        /// <summary>
        /// Gets or sets the starting timestamp to filter by update date.
        /// When provided, it will be formatted as Zulu: YYYY-MM-DDTHH:mm:ssZ.
        /// </summary>
        public DateTimeOffset? FromDateTime { get; set; }

        /// <summary>
        /// Gets or sets the ending timestamp to filter by update date.
        /// When provided, it will be formatted as Zulu: YYYY-MM-DDTHH:mm:ssZ.
        /// </summary>
        public DateTimeOffset? ToDateTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to filter for current members only (true) or not (false).
        /// When set, the query parameter currentMember will be included with a lowercase boolean value.
        /// </summary>
        public bool? CurrentMember { get; set; }
    }
}