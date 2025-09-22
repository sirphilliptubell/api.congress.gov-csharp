using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Congress.Gov.CSharp.Dtos.Common
{
    /// <summary>
    /// Represents a "latestAction" element with date and text.
    /// </summary>
    public sealed class LatestAction
    {
        /// <summary>
        /// Gets or sets the action date (YYYY-MM-DD).
        /// </summary>
        public DateTime ActionDate { get; set; }

        /// <summary>
        /// Gets or sets the action description text.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Gets or sets the source system, when present.
        /// </summary>
        public SourceSystemRef? SourceSystem { get; set; }

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Represents a "sourceSystem" reference with code and name.
    /// </summary>
    public sealed class SourceSystemRef
    {
        /// <summary>
        /// Gets or sets the source system code.
        /// </summary>
        public int? Code { get; set; }

        /// <summary>
        /// Gets or sets the source system name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Represents a hyperlink to a resource and its type.
    /// </summary>
    public sealed class TextFormat
    {
        /// <summary>
        /// Gets or sets the format type (e.g., PDF, Formatted Text, Formatted XML).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the absolute URL to the formatted resource.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Optional flag on some endpoints (e.g., committee report text) that indicates errata.
        /// </summary>
        public string? IsErrata { get; set; }

        /// <summary>
        /// Optional "part" identifier present on some endpoints.
        /// </summary>
        public string? Part { get; set; }

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Represents a single text version entry.
    /// </summary>
    public sealed class TextVersion
    {
        /// <summary>
        /// Gets or sets the publication date/time for the text version (when present).
        /// </summary>
        public DateTimeOffset? Date { get; set; }

        /// <summary>
        /// Gets or sets the list of available formats for this version.
        /// </summary>
        public List<TextFormat> Formats { get; set; } = new();

        /// <summary>
        /// Gets or sets the version type (e.g., Enrolled Bill, Placed on Calendar Senate).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Represents a title entry for a bill or related resource.
    /// </summary>
    public sealed class TitleEntry
    {
        /// <summary>
        /// Gets or sets an optional bill text version code.
        /// </summary>
        public string? BillTextVersionCode { get; set; }

        /// <summary>
        /// Gets or sets an optional bill text version name.
        /// </summary>
        public string? BillTextVersionName { get; set; }

        /// <summary>
        /// Gets or sets an optional chamber code (e.g., H, S).
        /// </summary>
        public string? ChamberCode { get; set; }

        /// <summary>
        /// Gets or sets an optional chamber name.
        /// </summary>
        public string? ChamberName { get; set; }

        /// <summary>
        /// Gets or sets the title text.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the title type text.
        /// </summary>
        public string TitleType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets an optional title type code when present.
        /// </summary>
        public int? TitleTypeCode { get; set; }

        /// <summary>
        /// Gets or sets an optional update timestamp when present.
        /// </summary>
        public DateTimeOffset? UpdateDate { get; set; }

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Represents a single committee activity entry with a date and name.
    /// </summary>
    public sealed class CommitteeActivity
    {
        /// <summary>
        /// Gets or sets the activity date/time.
        /// </summary>
        public DateTimeOffset? Date { get; set; }

        /// <summary>
        /// Gets or sets the activity name (e.g., Referred to, Markup by, Reported by).
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Represents a committee reference with activities, chamber and type.
    /// </summary>
    public sealed class CommitteeRef
    {
        /// <summary>
        /// Gets or sets the list of committee activities.
        /// </summary>
        public List<CommitteeActivity>? Activities { get; set; }

        /// <summary>
        /// Gets or sets the chamber (e.g., House, Senate).
        /// </summary>
        public string? Chamber { get; set; }

        /// <summary>
        /// Gets or sets the committee name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the committee system code (e.g., hsgo00).
        /// </summary>
        public string SystemCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the committee type (e.g., Standing).
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the API URL for this committee reference.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Represents a law entry with number and type.
    /// </summary>
    public sealed class LawEntry
    {
        /// <summary>
        /// Gets or sets the law number (e.g., 117-108).
        /// </summary>
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the law type (e.g., Public Law).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Represents a relationship detail entry for related bills.
    /// </summary>
    public sealed class RelationshipDetail
    {
        /// <summary>
        /// Gets or sets the identifier of the actor identifying this relationship (e.g., CRS, House).
        /// </summary>
        public string IdentifiedBy { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the relationship type (e.g., Related bill, Procedurally-related).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Represents a named policy area for a bill.
    /// </summary>
    public sealed class PolicyArea
    {
        /// <summary>
        /// Gets or sets the policy area name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Represents a legislative subject entry.
    /// </summary>
    public sealed class LegislativeSubject
    {
        /// <summary>
        /// Gets or sets the subject name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets optional update timestamp.
        /// </summary>
        public DateTimeOffset? UpdateDate { get; set; }

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Represents a sponsor or member reference.
    /// </summary>
    public sealed class Sponsor
    {
        /// <summary>
        /// Gets or sets the member bioguide id.
        /// </summary>
        public string BioguideId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the member district when present.
        /// </summary>
        public int? District { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets full display name (e.g., Rep. Maloney, Carolyn B. [D-NY-12]).
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets middle name when present.
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the party code (e.g., D, R).
        /// </summary>
        public string Party { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the state code when present (e.g., NY).
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// Gets or sets the "isByRequest" flag as a string when present.
        /// </summary>
        public string? IsByRequest { get; set; }

        /// <summary>
        /// Gets or sets the API URL to the member resource.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Represents a cosponsor entry.
    /// </summary>
    public sealed class Cosponsor
    {
        /// <summary>
        /// Gets or sets the member bioguide id.
        /// </summary>
        public string BioguideId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the member district when present.
        /// </summary>
        public int? District { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets full display name (e.g., Sen. Portman, Rob [R-OH]).
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this is an original cosponsor.
        /// </summary>
        public bool IsOriginalCosponsor { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets middle name when present.
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the party code (e.g., D, R).
        /// </summary>
        public string Party { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the sponsorship date (YYYY-MM-DD).
        /// </summary>
        public DateTime SponsorshipDate { get; set; }

        /// <summary>
        /// Gets or sets the state code (e.g., WV) when present.
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// Gets or sets the API URL to the member resource.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Represents a reference to a bill that is amended by an amendment.
    /// </summary>
    public sealed class AmendedBillRef
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
        /// Gets or sets the origin chamber name.
        /// </summary>
        public string? OriginChamber { get; set; }

        /// <summary>
        /// Gets or sets the origin chamber code (e.g., H or S).
        /// </summary>
        public string? OriginChamberCode { get; set; }

        /// <summary>
        /// Gets or sets the bill title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the bill type (e.g., HR, S).
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

    /// <summary>
    /// Represents a reference to a member (used in amendment sponsors).
    /// </summary>
    public sealed class MemberRef
    {
        /// <summary>
        /// Gets or sets the bioguide id.
        /// </summary>
        public string BioguideId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the member first name.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the member last name.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the member full display name.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

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
    /// Represents a recorded vote reference entry on an amendment action.
    /// </summary>
    public sealed class RecordedVoteRef
    {
        /// <summary>
        /// Gets or sets the chamber name.
        /// </summary>
        public string Chamber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the congress number.
        /// </summary>
        public int Congress { get; set; }

        /// <summary>
        /// Gets or sets the date/time of the vote.
        /// </summary>
        public DateTimeOffset Date { get; set; }

        /// <summary>
        /// Gets or sets the roll number.
        /// </summary>
        public int RollNumber { get; set; }

        /// <summary>
        /// Gets or sets the session number.
        /// </summary>
        public int SessionNumber { get; set; }

        /// <summary>
        /// Gets or sets the absolute URL to the vote resource (often external).
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Represents a common "count/url" structure used by bill detail for subresources.
    /// </summary>
    public sealed class CountUrlRef
    {
        /// <summary>
        /// Gets or sets the item count.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the absolute URL to the subresource collection.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Extension data for forward compatibility.
        /// </summary>
        [JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }
    }
}