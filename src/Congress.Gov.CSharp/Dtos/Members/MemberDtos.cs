using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Congress.Gov.CSharp.Dtos.Common;

namespace Congress.Gov.CSharp.Dtos.Members
{
    /// <summary>
    /// Root page for a list of members (GET /member and related list endpoints).
    /// </summary>
    public sealed class MembersListPage
    {
        /// <summary>
        /// Gets or sets the list of member items.
        /// </summary>
        public List<MemberListItem> Members { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// A list item representing a congressional member in list endpoints.
    /// </summary>
    public sealed class MemberListItem
    {
        /// <summary>
        /// Gets or sets the member bioguide identifier.
        /// </summary>
        public string BioguideId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the depiction information (image and attribution), when present.
        /// </summary>
        public DepictionRef? Depiction { get; set; }

        /// <summary>
        /// Gets or sets the district number, when present.
        /// </summary>
        public int? District { get; set; }

        /// <summary>
        /// Gets or sets the display name (e.g., "Leahy, Patrick J.").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the spelled-out party name (e.g., "Democratic"), when present.
        /// </summary>
        public string? PartyName { get; set; }

        /// <summary>
        /// Gets or sets the state name (e.g., "Vermont" or "Arizona"), when present.
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// Gets or sets the terms for the member (normalized as a flat list).
        /// </summary>
        public List<MemberTerm> Terms { get; set; } = new();

        /// <summary>
        /// Gets or sets the last update timestamp, when present.
        /// </summary>
        public DateTimeOffset? UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the API URL to the member detail resource.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Depiction reference (image URL and attribution).
    /// </summary>
    public sealed class DepictionRef
    {
        /// <summary>
        /// Gets or sets the attribution HTML.
        /// </summary>
        public string? Attribution { get; set; }

        /// <summary>
        /// Gets or sets the image URL (absolute).
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Normalized member term entry.
    /// </summary>
    public sealed class MemberTerm
    {
        /// <summary>
        /// Gets or sets the chamber (e.g., "House of Representatives", "Senate").
        /// </summary>
        public string? Chamber { get; set; }

        /// <summary>
        /// Gets or sets the congress number for this term, when present.
        /// </summary>
        public int? Congress { get; set; }

        /// <summary>
        /// Gets or sets the start year for the term (when provided as a year).
        /// </summary>
        public int? StartYear { get; set; }

        /// <summary>
        /// Gets or sets the end year for the term (when provided as a year).
        /// </summary>
        public int? EndYear { get; set; }

        /// <summary>
        /// Gets or sets a member type label (e.g., "Senator"), when present.
        /// </summary>
        public string? MemberType { get; set; }

        /// <summary>
        /// Gets or sets the state two-letter code for the term, when present.
        /// </summary>
        public string? StateCode { get; set; }

        /// <summary>
        /// Gets or sets the state name for the term, when present.
        /// </summary>
        public string? StateName { get; set; }

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Root page for member detail (GET /member/{bioguideId}).
    /// </summary>
    public sealed class MemberDetailPage
    {
        /// <summary>
        /// Gets or sets the member detail.
        /// </summary>
        public MemberDetail Member { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Detailed congressional member information.
    /// </summary>
    public sealed class MemberDetail
    {
        /// <summary>
        /// Gets or sets the member bioguide identifier.
        /// </summary>
        public string BioguideId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the birth year as a string (examples show string).
        /// </summary>
        public string? BirthYear { get; set; }

        /// <summary>
        /// Gets or sets the cosponsored legislation collection reference (count + url), when present.
        /// </summary>
        public CountUrlRef? CosponsoredLegislation { get; set; }

        /// <summary>
        /// Gets or sets the depiction information (image and attribution), when present.
        /// </summary>
        public DepictionRef? Depiction { get; set; }

        /// <summary>
        /// Gets or sets the direct-order name (e.g., "Patrick J. Leahy").
        /// </summary>
        public string? DirectOrderName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the honorific name (e.g., "Mr.").
        /// </summary>
        public string? HonorificName { get; set; }

        /// <summary>
        /// Gets or sets the inverted-order name (e.g., "Leahy, Patrick J.").
        /// </summary>
        public string? InvertedOrderName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the leadership positions held, when present.
        /// </summary>
        public List<LeadershipEntry>? Leadership { get; set; }

        /// <summary>
        /// Gets or sets the party history entries, when present.
        /// </summary>
        public List<PartyHistoryEntry>? PartyHistory { get; set; }

        /// <summary>
        /// Gets or sets the sponsored legislation collection reference (count + url), when present.
        /// </summary>
        public CountUrlRef? SponsoredLegislation { get; set; }

        /// <summary>
        /// Gets or sets the state name.
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// Gets or sets the normalized terms list.
        /// </summary>
        public List<MemberTerm>? Terms { get; set; }

        /// <summary>
        /// Gets or sets the last update timestamp.
        /// </summary>
        public DateTimeOffset? UpdateDate { get; set; }

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Leadership role entry.
    /// </summary>
    public sealed class LeadershipEntry
    {
        /// <summary>
        /// Gets or sets the congress number.
        /// </summary>
        public int Congress { get; set; }

        /// <summary>
        /// Gets or sets the leadership type (e.g., "President Pro Tempore").
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Party history entry.
    /// </summary>
    public sealed class PartyHistoryEntry
    {
        /// <summary>
        /// Gets or sets the party abbreviation (e.g., "D").
        /// </summary>
        public string PartyAbbreviation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the party name (e.g., "Democrat").
        /// </summary>
        public string PartyName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start year for party affiliation.
        /// </summary>
        public int StartYear { get; set; }

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    // ---------- Sponsored / Cosponsored legislation pages ----------

    /// <summary>
    /// Page wrapper for member sponsored legislation.
    /// </summary>
    public sealed class MemberSponsoredLegislationPage
    {
        /// <summary>
        /// Gets or sets the list of sponsored legislation items.
        /// </summary>
        public List<MemberLegislationItem> SponsoredLegislation { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Page wrapper for member cosponsored legislation.
    /// </summary>
    public sealed class MemberCosponsoredLegislationPage
    {
        /// <summary>
        /// Gets or sets the list of cosponsored legislation items.
        /// </summary>
        public List<MemberLegislationItem> CosponsoredLegislation { get; set; } = new();

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// A minimal bill-like listing item used by member sponsored/cosponsored lists.
    /// </summary>
    public sealed class MemberLegislationItem
    {
        /// <summary>
        /// Gets or sets the congress number.
        /// </summary>
        public int Congress { get; set; }

        /// <summary>
        /// Gets or sets the introduced date (YYYY-MM-DD).
        /// </summary>
        public DateTime IntroducedDate { get; set; }

        /// <summary>
        /// Gets or sets the latest action entry.
        /// </summary>
        public LatestAction LatestAction { get; set; } = new();

        /// <summary>
        /// Gets or sets the bill number (as a string).
        /// </summary>
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the policy area, when present.
        /// </summary>
        public PolicyArea? PolicyArea { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the bill type (e.g., S, HR).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the API URL to the bill detail.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }
}