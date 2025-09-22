using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Congress.Gov.CSharp.Dtos.Congress
{
    /// <summary>
    /// Root page for /congress list endpoint.
    /// </summary>
    public sealed class CongressesListPage
    {
        /// <summary>
        /// Gets or sets the list of congress entries.
        /// </summary>
        public List<CongressEntry> Congresses { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Root page for /congress/:congress and /congress/current endpoints.
    /// </summary>
    public sealed class CongressDetailPage
    {
        /// <summary>
        /// Gets or sets the congress entry.
        /// </summary>
        public CongressEntry Congress { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Represents a Congress with its sessions.
    /// </summary>
    public sealed class CongressEntry
    {
        /// <summary>
        /// Gets or sets the end year as a string (per API examples).
        /// </summary>
        public string? EndYear { get; set; }

        /// <summary>
        /// Gets or sets the display name (e.g., "117th Congress").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the numeric congress number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the sessions for this congress.
        /// </summary>
        public List<CongressSession> Sessions { get; set; } = new();

        /// <summary>
        /// Gets or sets the start year as a string (per API examples).
        /// </summary>
        public string StartYear { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the update timestamp for this congress (when present).
        /// </summary>
        public DateTimeOffset? UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets a canonical API URL for this congress.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Represents a session belonging to a Congress.
    /// </summary>
    public sealed class CongressSession
    {
        /// <summary>
        /// Gets or sets the chamber (e.g., "House of Representatives" or "Senate").
        /// </summary>
        public string Chamber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the end date (date only).
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the session number (1 or 2).
        /// </summary>
        public int? Number { get; set; }

        /// <summary>
        /// Gets or sets the start date (date only).
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the session type (e.g., "R").
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }
}