using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class HCP : X12SegmentBase
    {
        public string PricingMethodology { get; set; }
        public string RepricedAllowedAmount { get; set; }
        public string RepricedSavingAmount { get; set; }
        public string RepricingOrganizationID { get; set; }
        public string RepricingRate { get; set; }
        public string RepricedGroupCode { get; set; }
        public string RepricedGroupAmount { get; set; }
        public string RepricingRevenueCode { get; set; }
        public string RepricingUnit { get; set; }
        public string RepricingQuantity { get; set; }
        public string RejectReasonCode { get; set; }
        public string PolicyComplianceCode { get; set; }
        public string ExceptionCode { get; set; }
        public HCP()
        {
            SegmentCode = "HCP";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(PricingMethodology) && !string.IsNullOrEmpty(RepricedAllowedAmount);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("HCP*" + PricingMethodology + "*" + RepricedAllowedAmount);
            if (!string.IsNullOrEmpty(ExceptionCode))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedSavingAmount)) sb.Append(RepricedSavingAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingOrganizationID)) sb.Append(RepricingOrganizationID);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingRate)) sb.Append(RepricingRate);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedGroupCode)) sb.Append(RepricedGroupCode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedGroupAmount)) sb.Append(RepricedGroupAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingRevenueCode)) sb.Append(RepricingRevenueCode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingUnit)) sb.Append(RepricingUnit);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingQuantity)) sb.Append(RepricingQuantity);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RejectReasonCode)) sb.Append(RejectReasonCode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PolicyComplianceCode)) sb.Append(PolicyComplianceCode);
                sb.Append("*" + ExceptionCode);
            }
            else if (!string.IsNullOrEmpty(PolicyComplianceCode))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedSavingAmount)) sb.Append(RepricedSavingAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingOrganizationID)) sb.Append(RepricingOrganizationID);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingRate)) sb.Append(RepricingRate);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedGroupCode)) sb.Append(RepricedGroupCode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedGroupAmount)) sb.Append(RepricedGroupAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingRevenueCode)) sb.Append(RepricingRevenueCode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingUnit)) sb.Append(RepricingUnit);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingQuantity)) sb.Append(RepricingQuantity);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RejectReasonCode)) sb.Append(RejectReasonCode);
                sb.Append("*" + PolicyComplianceCode);
            }
            else if (!string.IsNullOrEmpty(RejectReasonCode))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedSavingAmount)) sb.Append(RepricedSavingAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingOrganizationID)) sb.Append(RepricingOrganizationID);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingRate)) sb.Append(RepricingRate);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedGroupCode)) sb.Append(RepricedGroupCode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedGroupAmount)) sb.Append(RepricedGroupAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingRevenueCode)) sb.Append(RepricingRevenueCode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingUnit)) sb.Append(RepricingUnit);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingQuantity)) sb.Append(RepricingQuantity);
                sb.Append("*" + RejectReasonCode);
            }
            else if (!string.IsNullOrEmpty(RepricingQuantity))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedSavingAmount)) sb.Append(RepricedSavingAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingOrganizationID)) sb.Append(RepricingOrganizationID);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingRate)) sb.Append(RepricingRate);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedGroupCode)) sb.Append(RepricedGroupCode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedGroupAmount)) sb.Append(RepricedGroupAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingRevenueCode)) sb.Append(RepricingRevenueCode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingUnit)) sb.Append(RepricingUnit);
                sb.Append("*" + RepricingQuantity);
            }
            else if (!string.IsNullOrEmpty(RepricingUnit))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedSavingAmount)) sb.Append(RepricedSavingAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingOrganizationID)) sb.Append(RepricingOrganizationID);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingRate)) sb.Append(RepricingRate);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedGroupCode)) sb.Append(RepricedGroupCode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedGroupAmount)) sb.Append(RepricedGroupAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingRevenueCode)) sb.Append(RepricingRevenueCode);
                sb.Append("*" + RepricingUnit);
            }
            else if (!string.IsNullOrEmpty(RepricingRevenueCode))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedSavingAmount)) sb.Append(RepricedSavingAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingOrganizationID)) sb.Append(RepricingOrganizationID);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingRate)) sb.Append(RepricingRate);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedGroupCode)) sb.Append(RepricedGroupCode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedGroupAmount)) sb.Append(RepricedGroupAmount);
                sb.Append("*" + RepricingRevenueCode);
            }
            else if (!string.IsNullOrEmpty(RepricedGroupAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedSavingAmount)) sb.Append(RepricedSavingAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingOrganizationID)) sb.Append(RepricingOrganizationID);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingRate)) sb.Append(RepricingRate);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedGroupCode)) sb.Append(RepricedGroupCode);
                sb.Append("*" + RepricedGroupAmount);
            }
            else if (!string.IsNullOrEmpty(RepricedGroupCode))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedSavingAmount)) sb.Append(RepricedSavingAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingOrganizationID)) sb.Append(RepricingOrganizationID);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingRate)) sb.Append(RepricingRate);
                sb.Append("*" + RepricedGroupCode);
            }
            else if (!string.IsNullOrEmpty(RepricingRate))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedSavingAmount)) sb.Append(RepricedSavingAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricingOrganizationID)) sb.Append(RepricingOrganizationID);
                sb.Append("*" + RepricingRate);
            }
            else if (!string.IsNullOrEmpty(RepricingOrganizationID))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(RepricedSavingAmount)) sb.Append(RepricedSavingAmount);
                sb.Append("*" + RepricingOrganizationID);
            }
            else if (!string.IsNullOrEmpty(RepricedSavingAmount))
            {
                sb.Append("*" + RepricedSavingAmount);
            }
            sb.Append("~");
            return sb.ToString();
        }
    }
}
