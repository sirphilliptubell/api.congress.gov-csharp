using System;
using System.Globalization;

namespace Congress.Gov.CSharp.Internal.Routing
{
    /// <summary>
    /// Centralized route composition for Congress.gov endpoints.
    /// </summary>
    internal static class PathBuilder
    {
        // Bills
        public static string BillList() => "bill";

        public static string BillByCongress(int congress) => $"bill/{congress.ToString(CultureInfo.InvariantCulture)}";

        public static string BillByCongressAndType(int congress, string billType)
            => $"bill/{congress.ToString(CultureInfo.InvariantCulture)}/{NormalizeType(billType)}";

        public static string BillDetail(int congress, string billType, int billNumber)
            => $"bill/{congress.ToString(CultureInfo.InvariantCulture)}/{NormalizeType(billType)}/{billNumber.ToString(CultureInfo.InvariantCulture)}";

        public static string BillActions(int congress, string billType, int billNumber)
            => $"{BillDetail(congress, billType, billNumber)}/actions";

        public static string BillAmendments(int congress, string billType, int billNumber)
            => $"{BillDetail(congress, billType, billNumber)}/amendments";

        public static string BillCommittees(int congress, string billType, int billNumber)
            => $"{BillDetail(congress, billType, billNumber)}/committees";

        public static string BillCosponsors(int congress, string billType, int billNumber)
            => $"{BillDetail(congress, billType, billNumber)}/cosponsors";

        public static string BillRelatedBills(int congress, string billType, int billNumber)
            => $"{BillDetail(congress, billType, billNumber)}/relatedbills";

        public static string BillSubjects(int congress, string billType, int billNumber)
            => $"{BillDetail(congress, billType, billNumber)}/subjects";

        public static string BillSummaries(int congress, string billType, int billNumber)
            => $"{BillDetail(congress, billType, billNumber)}/summaries";

        public static string BillTextVersions(int congress, string billType, int billNumber)
            => $"{BillDetail(congress, billType, billNumber)}/text";

        public static string BillTitles(int congress, string billType, int billNumber)
            => $"{BillDetail(congress, billType, billNumber)}/titles";

        // Summaries
        public static string SummariesList() => "summaries";

        public static string SummariesByCongress(int congress)
            => $"summaries/{congress.ToString(CultureInfo.InvariantCulture)}";

        public static string SummariesByCongressAndBillType(int congress, string billType)
            => $"summaries/{congress.ToString(CultureInfo.InvariantCulture)}/{NormalizeType(billType)}";

        // Congress
        public static string CongressList() => "congress";

        public static string CongressByNumber(int congress)
            => $"congress/{congress.ToString(CultureInfo.InvariantCulture)}";

        public static string CongressCurrent()
            => "congress/current";

        // Amendments
        public static string AmendmentList() => "amendment";

        public static string AmendmentByCongress(int congress) => $"amendment/{congress.ToString(CultureInfo.InvariantCulture)}";

        public static string AmendmentByCongressAndType(int congress, string amendmentType)
            => $"amendment/{congress.ToString(CultureInfo.InvariantCulture)}/{NormalizeType(amendmentType)}";

        public static string AmendmentDetail(int congress, string amendmentType, int amendmentNumber)
            => $"amendment/{congress.ToString(CultureInfo.InvariantCulture)}/{NormalizeType(amendmentType)}/{amendmentNumber.ToString(CultureInfo.InvariantCulture)}";

        public static string AmendmentActions(int congress, string amendmentType, int amendmentNumber)
            => $"{AmendmentDetail(congress, amendmentType, amendmentNumber)}/actions";

        public static string AmendmentCosponsors(int congress, string amendmentType, int amendmentNumber)
            => $"{AmendmentDetail(congress, amendmentType, amendmentNumber)}/cosponsors";

        public static string AmendmentsToAmendment(int congress, string amendmentType, int amendmentNumber)
            => $"{AmendmentDetail(congress, amendmentType, amendmentNumber)}/amendments";

        public static string AmendmentTextVersions(int congress, string amendmentType, int amendmentNumber)
            => $"{AmendmentDetail(congress, amendmentType, amendmentNumber)}/text";

        // Members
        public static string MemberList() => "member";

        public static string MemberDetail(string bioguideId)
            => $"member/{bioguideId}";

        public static string MemberSponsoredLegislation(string bioguideId)
            => $"member/{bioguideId}/sponsored-legislation";

        public static string MemberCosponsoredLegislation(string bioguideId)
            => $"member/{bioguideId}/cosponsored-legislation";

        public static string MemberByCongress(int congress)
            => $"member/congress/{congress.ToString(CultureInfo.InvariantCulture)}";

        public static string MemberByState(string stateCode)
            => $"member/{stateCode}";

        public static string MemberByStateAndDistrict(string stateCode, int district)
            => $"member/{stateCode}/{district.ToString(CultureInfo.InvariantCulture)}";

        public static string MemberByCongressStateAndDistrict(int congress, string stateCode, int district)
            => $"member/congress/{congress.ToString(CultureInfo.InvariantCulture)}/{stateCode}/{district.ToString(CultureInfo.InvariantCulture)}";

        private static string NormalizeType(string value)
            => string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim().ToLowerInvariant();
    }
}