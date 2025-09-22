using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Congress.Gov.CSharp.Dtos.Summaries
{
    /// <summary>
    /// Root page for /summaries list endpoints.
    /// </summary>
    public sealed class SummariesListPage
    {
        /// <summary>
        /// Gets or sets the list of summary feed items.
        /// </summary>
        public List<SummaryFeedItem> Summaries { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// A summary entry in the summaries feeds.
    /// </summary>
    public sealed class SummaryFeedItem
    {
        /// <summary>
        /// Gets or sets the action date (YYYY-MM-DD).
        /// </summary>
        public DateTime ActionDate { get; set; }

        /// <summary>
        /// Gets or sets the action description (e.g., Introduced in Senate, Public Law).
        /// </summary>
        public string ActionDesc { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a compact bill reference object.
        /// </summary>
        public SummaryBillRef Bill { get; set; } = new();

        /// <summary>
        /// Gets or sets the current chamber name.
        /// </summary>
        public string? CurrentChamber { get; set; }

        /// <summary>
        /// Gets or sets the current chamber code (e.g., H, S).
        /// </summary>
        public string? CurrentChamberCode { get; set; }

        /// <summary>
        /// Gets or sets the last summary update timestamp.
        /// </summary>
        public DateTimeOffset? LastSummaryUpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the HTML text of the summary.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the update timestamp for this entry.
        /// </summary>
        public DateTimeOffset UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the version code string.
        /// </summary>
        public string VersionCode { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// A compact reference to a bill shown within a summary feed item.
    /// </summary>
    public sealed class SummaryBillRef
    {
        /// <summary>
        /// Gets or sets the congress number.
        /// </summary>
        public int Congress { get; set; }

        /// <summary>
        /// Gets or sets the bill number as a string.
        /// </summary>
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the origin chamber name (e.g., Senate).
        /// </summary>
        public string OriginChamber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the origin chamber code (e.g., S).
        /// </summary>
        public string OriginChamberCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the bill title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the bill type (e.g., S).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the update date including text (if present) as a timestamp.
        /// </summary>
        public DateTimeOffset? UpdateDateIncludingText { get; set; }

        /// <summary>
        /// Gets or sets the API URL to the bill detail resource.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }
}