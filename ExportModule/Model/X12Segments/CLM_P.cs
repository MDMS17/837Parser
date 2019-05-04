using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class CLM_P : X12SegmentBase
    {
        public string ClaimID { get; set; }
        public string ClaimAmount { get; set; }
        public string ClaimPOS { get; set; }
        public string ClaimType { get; set; }
        public string ClaimFrequencyCode { get; set; }
        public string ClaimProviderSignature { get; set; }
        public string ClaimProviderAssignment { get; set; }
        public string ClaimBenefitAssignment { get; set; }
        public string ClaimReleaseofInformationCode { get; set; }
        public string ClaimPatientSignatureSourceCode { get; set; }
        public string ClaimRelatedCausesQualifier { get; set; }
        public string ClaimRelatedCausesCode { get; set; }
        public string ClaimRelatedStateCode { get; set; }
        public string ClaimRelatedCountryCode { get; set; }
        public string ClaimSpecialProgramCode { get; set; }
        public string ClaimDelayReasonCode { get; set; }
        public CLM_P()
        {
            SegmentCode = "CLM";
            ClaimType = "B";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(ClaimID) && !string.IsNullOrEmpty(ClaimAmount) && !string.IsNullOrEmpty(ClaimPOS) && !string.IsNullOrEmpty(ClaimFrequencyCode) && !string.IsNullOrEmpty(ClaimProviderSignature) && !string.IsNullOrEmpty(ClaimProviderAssignment) && !string.IsNullOrEmpty(ClaimBenefitAssignment) && !string.IsNullOrEmpty(ClaimReleaseofInformationCode);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CLM*" + ClaimID + "*" + ClaimAmount + "***" + ClaimPOS + ":B:" + ClaimFrequencyCode + "*" + ClaimProviderSignature + "*" + ClaimProviderAssignment + "*" + ClaimBenefitAssignment + "*" + ClaimReleaseofInformationCode);
            if (!string.IsNullOrEmpty(ClaimDelayReasonCode))
            {
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ClaimPatientSignatureSourceCode) ? "" : ClaimPatientSignatureSourceCode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimRelatedCausesCode))
                {
                    sb.Append(ClaimRelatedCausesQualifier + ":" + ClaimRelatedCausesCode);
                    if (!string.IsNullOrEmpty(ClaimRelatedStateCode)) sb.Append("::" + ClaimRelatedStateCode);
                    if (!string.IsNullOrEmpty(ClaimRelatedCountryCode)) sb.Append(":" + ClaimRelatedCountryCode);
                }
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ClaimSpecialProgramCode) ? "" : ClaimSpecialProgramCode);
                sb.Append("********" + ClaimDelayReasonCode);
            }
            else if (!string.IsNullOrEmpty(ClaimSpecialProgramCode))
            {
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ClaimPatientSignatureSourceCode) ? "" : ClaimPatientSignatureSourceCode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimRelatedCausesCode))
                {
                    sb.Append(ClaimRelatedCausesQualifier + ":" + ClaimRelatedCausesCode);
                    if (!string.IsNullOrEmpty(ClaimRelatedStateCode)) sb.Append("::" + ClaimRelatedStateCode);
                    if (!string.IsNullOrEmpty(ClaimRelatedCountryCode)) sb.Append(":" + ClaimRelatedCountryCode);
                }
                sb.Append("*" + ClaimSpecialProgramCode);
            }
            else if (!string.IsNullOrEmpty(ClaimRelatedCausesCode))
            {
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ClaimPatientSignatureSourceCode) ? "" : ClaimPatientSignatureSourceCode);
                sb.Append("*");
                sb.Append(ClaimRelatedCausesQualifier + ":" + ClaimRelatedCausesCode);
                if (!string.IsNullOrEmpty(ClaimRelatedStateCode)) sb.Append("::" + ClaimRelatedStateCode);
                if (!string.IsNullOrEmpty(ClaimRelatedCountryCode)) sb.Append(":" + ClaimRelatedCountryCode);
            }
            else if (!string.IsNullOrEmpty(ClaimPatientSignatureSourceCode))
            {
                sb.Append("*" + ClaimPatientSignatureSourceCode);
            }
            sb.Append("~");
            return sb.ToString();
        }
    }
}
