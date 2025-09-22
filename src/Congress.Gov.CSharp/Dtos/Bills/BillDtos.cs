using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Congress.Gov.CSharp.Dtos.Common;
using Congress.Gov.CSharp.Dtos.Amendments;

namespace Congress.Gov.CSharp.Dtos.Bills
{
    /// <summary>
    /// Root page for a list of bills (GET /bill, /bill/{congress}, /bill/{congress}/{billType}).
    /// </summary>
    public sealed class BillsListPage
    {
        /// <summary>
        /// Gets or sets the list of bill items.
        /// </summary>
        public List<BillListItem> Bills { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// A list item representing a bill in list endpoints.
    /// </summary>
    public sealed class BillListItem
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
        /// Gets or sets the bill number as a string (e.g., "3076").
        /// </summary>
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the origin chamber (e.g., House).
        /// </summary>
        public string OriginChamber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the origin chamber code (e.g., H).
        /// </summary>
        public string OriginChamberCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the primary title of the bill.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the bill type (e.g., HR, S).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the update date (date-only where presented in examples).
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the update date when including text updates (date/time where presented in examples).
        /// </summary>
        public DateTimeOffset? UpdateDateIncludingText { get; set; }

        /// <summary>
        /// Gets or sets the API URL to the bill detail.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Root page for bill detail (GET /bill/{congress}/{billType}/{billNumber}).
    /// </summary>
    public sealed class BillDetailPage
    {
        /// <summary>
        /// Gets or sets the bill.
        /// </summary>
        public BillDetail Bill { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Detailed bill information.
    /// </summary>
    public sealed class BillDetail
    {
        /// <summary>
        /// Gets or sets the action collection reference (count + url).
        /// </summary>
        public CountUrlRef? Actions { get; set; }

        /// <summary>
        /// Gets or sets the amendment collection reference (count + url).
        /// </summary>
        public CountUrlRef? Amendments { get; set; }

        /// <summary>
        /// Gets or sets the CBO cost estimates for the bill.
        /// </summary>
        public List<CboCostEstimate>? CboCostEstimates { get; set; }

        /// <summary>
        /// Gets or sets the committee reports references (citation + url).
        /// </summary>
        public List<CommitteeReportRef>? CommitteeReports { get; set; }

        /// <summary>
        /// Gets or sets the committees collection reference (count + url).
        /// </summary>
        public CountUrlRef? Committees { get; set; }

        /// <summary>
        /// Gets or sets the congress number.
        /// </summary>
        public int Congress { get; set; }

        /// <summary>
        /// Gets or sets the constitutional authority statement text when present (HTML).
        /// </summary>
        public string? ConstitutionalAuthorityStatementText { get; set; }

        /// <summary>
        /// Gets or sets the cosponsors collection reference (count + url).
        /// </summary>
        public CountUrlRef? Cosponsors { get; set; }

        /// <summary>
        /// Gets or sets the introduced date (YYYY-MM-DD).
        /// </summary>
        public DateTime? IntroducedDate { get; set; }

        /// <summary>
        /// Gets or sets the latest action.
        /// </summary>
        public LatestAction LatestAction { get; set; } = new();

        /// <summary>
        /// Gets or sets the laws created by this bill (when enacted).
        /// </summary>
        public List<LawEntry>? Laws { get; set; }

        /// <summary>
        /// Gets or sets the public legislation URL on congress.gov.
        /// </summary>
        public string? LegislationUrl { get; set; }

        /// <summary>
        /// Gets or sets the bill number (as a string).
        /// </summary>
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the origin chamber (e.g., House).
        /// </summary>
        public string OriginChamber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the policy area.
        /// </summary>
        public PolicyArea? PolicyArea { get; set; }

        /// <summary>
        /// Gets or sets the related bills collection reference (count + url).
        /// </summary>
        public CountUrlRef? RelatedBills { get; set; }

        /// <summary>
        /// Gets or sets the sponsors on the bill.
        /// </summary>
        public List<Sponsor>? Sponsors { get; set; }

        /// <summary>
        /// Gets or sets the subjects collection reference (count + url).
        /// </summary>
        public CountUrlRef? Subjects { get; set; }

        /// <summary>
        /// Gets or sets the summaries collection reference (count + url).
        /// </summary>
        public CountUrlRef? Summaries { get; set; }

        /// <summary>
        /// Gets or sets the text versions collection reference (count + url).
        /// </summary>
        public CountUrlRef? TextVersions { get; set; }

        /// <summary>
        /// Gets or sets the bill's title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the titles collection reference (count + url).
        /// </summary>
        public CountUrlRef? Titles { get; set; }

        /// <summary>
        /// Gets or sets the bill type (e.g., HR, S).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the update date/time (includes text updates).
        /// </summary>
        public DateTimeOffset UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the update date/time including text updates.
        /// </summary>
        public DateTimeOffset? UpdateDateIncludingText { get; set; }

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// CBO cost estimate entry.
    /// </summary>
    public sealed class CboCostEstimate
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the publication date/time.
        /// </summary>
        public DateTimeOffset PubDate { get; set; }

        /// <summary>
        /// Gets or sets the title of the estimate.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL to the estimate.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Committee report reference entry on a bill detail.
    /// </summary>
    public sealed class CommitteeReportRef
    {
        /// <summary>
        /// Gets or sets the report citation (e.g., H. Rept. 117-89,Part 1).
        /// </summary>
        public string Citation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the API URL to the committee report.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Bill action entry.
    /// </summary>
    public sealed class BillAction
    {
        /// <summary>
        /// Gets or sets the action code (when present).
        /// </summary>
        public string? ActionCode { get; set; }

        /// <summary>
        /// Gets or sets the action date (YYYY-MM-DD).
        /// </summary>
        public DateTime ActionDate { get; set; }

        /// <summary>
        /// Gets or sets the source system.
        /// </summary>
        public SourceSystemRef? SourceSystem { get; set; }

        /// <summary>
        /// Gets or sets the description text.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the action type (e.g., BecameLaw, President, IntroReferral).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Related bill entry.
    /// </summary>
    public sealed class RelatedBill
    {
        /// <summary>
        /// Gets or sets the congress number.
        /// </summary>
        public int Congress { get; set; }

        /// <summary>
        /// Gets or sets the latest action.
        /// </summary>
        public LatestAction? LatestAction { get; set; }

        /// <summary>
        /// Gets or sets the bill number (as a number in examples).
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the relationship details.
        /// </summary>
        public List<RelationshipDetail>? RelationshipDetails { get; set; }

        /// <summary>
        /// Gets or sets the title (when present).
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the bill type (e.g., HRES, S).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the API URL to the related bill detail.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Subjects object on a bill (policy area + legislative subjects).
    /// </summary>
    public sealed class BillSubjects
    {
        /// <summary>
        /// Gets or sets the list of legislative subjects.
        /// </summary>
        public List<LegislativeSubject> LegislativeSubjects { get; set; } = new();

        /// <summary>
        /// Gets or sets the policy area.
        /// </summary>
        public PolicyArea PolicyArea { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Bill summary entry.
    /// </summary>
    public sealed class Summary
    {
        /// <summary>
        /// Gets or sets the summary action date (YYYY-MM-DD).
        /// </summary>
        public DateTime ActionDate { get; set; }

        /// <summary>
        /// Gets or sets the action description (e.g., Public Law).
        /// </summary>
        public string ActionDesc { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the HTML text of the summary.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the update timestamp.
        /// </summary>
        public DateTimeOffset UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the version code (e.g., 49).
        /// </summary>
        public string VersionCode { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    // -------- Subresource page wrappers (for pagination) --------

    /// <summary>
    /// Page wrapper for bill actions.
    /// </summary>
    public sealed class BillActionsPage
    {
        /// <summary>
        /// Gets or sets the list of actions.
        /// </summary>
        public List<BillAction> Actions { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Page wrapper for bill amendments.
    /// </summary>
    public sealed class BillAmendmentsPage
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
    /// Page wrapper for bill committees.
    /// </summary>
    public sealed class BillCommitteesPage
    {
        /// <summary>
        /// Gets or sets the list of committees.
        /// </summary>
        public List<CommitteeRef> Committees { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Page wrapper for bill cosponsors.
    /// </summary>
    public sealed class BillCosponsorsPage
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
    /// Page wrapper for related bills.
    /// </summary>
    public sealed class BillRelatedBillsPage
    {
        /// <summary>
        /// Gets or sets the list of related bills.
        /// </summary>
        public List<RelatedBill> RelatedBills { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Page wrapper for bill subjects.
    /// </summary>
    public sealed class BillSubjectsPage
    {
        /// <summary>
        /// Gets or sets the subjects object.
        /// </summary>
        public BillSubjects Subjects { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Page wrapper for bill summaries.
    /// </summary>
    public sealed class BillSummariesPage
    {
        /// <summary>
        /// Gets or sets the list of summaries.
        /// </summary>
        public List<Summary> Summaries { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Page wrapper for bill text versions.
    /// </summary>
    public sealed class BillTextVersionsPage
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

    /// <summary>
    /// Page wrapper for bill titles.
    /// </summary>
    public sealed class BillTitlesPage
    {
        /// <summary>
        /// Gets or sets the list of titles.
        /// </summary>
        public List<TitleEntry> Titles { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }
}