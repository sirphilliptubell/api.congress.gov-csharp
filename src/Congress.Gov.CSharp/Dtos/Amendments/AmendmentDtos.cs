using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Congress.Gov.CSharp.Dtos.Common;

namespace Congress.Gov.CSharp.Dtos.Amendments
{
    /// <summary>
    /// Root page for a list of amendments (GET /amendment, /amendment/{congress}, /amendment/{congress}/{amendmentType}).
    /// </summary>
    public sealed class AmendmentsListPage
    {
        /// <summary>
        /// Gets or sets the list of amendment items.
        /// </summary>
        public List<AmendmentListItem> Amendments { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// A list item representing an amendment in list endpoints.
    /// </summary>
    public sealed class AmendmentListItem
    {
        /// <summary>
        /// Gets or sets the congress number.
        /// </summary>
        public int Congress { get; set; }

        /// <summary>
        /// Gets or sets the latest action.
        /// </summary>
        public LatestAction LatestAction { get; set; } = new();

        /// <summary>
        /// Gets or sets the amendment number as a string (e.g., "2137").
        /// </summary>
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the purpose text when present.
        /// </summary>
        public string? Purpose { get; set; }

        /// <summary>
        /// Gets or sets the amendment type (e.g., SAMDT, HAMDT).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the update timestamp when present.
        /// </summary>
        public DateTimeOffset? UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the API URL to the amendment detail.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Root page for amendment detail (GET /amendment/{congress}/{amendmentType}/{amendmentNumber}).
    /// </summary>
    public sealed class AmendmentDetailPage
    {
        /// <summary>
        /// Gets or sets the amendment detail.
        /// </summary>
        public AmendmentDetail Amendment { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Detailed amendment information.
    /// </summary>
    public sealed class AmendmentDetail
    {
        /// <summary>
        /// Gets or sets the actions collection reference (count + url).
        /// </summary>
        public CountUrlRef? Actions { get; set; }

        /// <summary>
        /// Gets or sets the "amendments to amendment" collection reference (count + url).
        /// </summary>
        public CountUrlRef? AmendmentsToAmendment { get; set; }

        /// <summary>
        /// Gets or sets the amended bill reference.
        /// </summary>
        public AmendedBillRef? AmendedBill { get; set; }

        /// <summary>
        /// Gets or sets the amendment chamber (e.g., Senate).
        /// </summary>
        public string Chamber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the congress number.
        /// </summary>
        public int Congress { get; set; }

        /// <summary>
        /// Gets or sets the cosponsors collection reference (count + url).
        /// </summary>
        public CountUrlRef? Cosponsors { get; set; }

        /// <summary>
        /// Gets or sets the latest action.
        /// </summary>
        public LatestAction LatestAction { get; set; } = new();

        /// <summary>
        /// Gets or sets the amendment number as a string.
        /// </summary>
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the proposed date/time when present.
        /// </summary>
        public DateTimeOffset? ProposedDate { get; set; }

        /// <summary>
        /// Gets or sets the amendment purpose when present.
        /// </summary>
        public string? Purpose { get; set; }

        /// <summary>
        /// Gets or sets the list of sponsors.
        /// </summary>
        public List<MemberRef>? Sponsors { get; set; }

        /// <summary>
        /// Gets or sets the submitted date/time when present.
        /// </summary>
        public DateTimeOffset? SubmittedDate { get; set; }

        /// <summary>
        /// Gets or sets the amendment type (e.g., SAMDT).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the update timestamp when present.
        /// </summary>
        public DateTimeOffset? UpdateDate { get; set; }

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Amendment action entry.
    /// </summary>
    public sealed class AmendmentAction
    {
        /// <summary>
        /// Gets or sets the action date (YYYY-MM-DD).
        /// </summary>
        public DateTime ActionDate { get; set; }

        /// <summary>
        /// Gets or sets the recorded votes, when present.
        /// </summary>
        public List<RecordedVoteRef>? RecordedVotes { get; set; }

        /// <summary>
        /// Gets or sets the source system.
        /// </summary>
        public SourceSystemRef? SourceSystem { get; set; }

        /// <summary>
        /// Gets or sets the action text.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the action type (e.g., Floor).
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    // -------- Subresource page wrappers (for pagination) --------

    /// <summary>
    /// Page wrapper for amendment actions.
    /// </summary>
    public sealed class AmendmentActionsPage
    {
        /// <summary>
        /// Gets or sets the list of actions.
        /// </summary>
        public List<AmendmentAction> Actions { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Page wrapper for amendment cosponsors.
    /// </summary>
    public sealed class AmendmentCosponsorsPage
    {
        /// <summary>
        /// Gets or sets the list of cosponsors.
        /// </summary>
        public List<Cosponsor> Cosponsors { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Page wrapper for "amendments to amendment".
    /// </summary>
    public sealed class AmendmentsToAmendmentPage
    {
        /// <summary>
        /// Gets or sets the list of amendments.
        /// </summary>
        public List<AmendmentListItem> Amendments { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Page wrapper for amendment text versions.
    /// </summary>
    public sealed class AmendmentTextVersionsPage
    {
        /// <summary>
        /// Gets or sets the list of text versions.
        /// </summary>
        public List<TextVersion> TextVersions { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }
}