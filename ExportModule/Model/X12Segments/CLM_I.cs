using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class CLM_I : X12SegmentBase
    {
        public string ClaimID { get; set; }
        public string ClaimAmount { get; set; }
        public string ClaimPOS { get; set; }
        public string ClaimType { get; set; }
        public string ClaimFrequencyCode { get; set; }
        public string ClaimProviderAssignment { get; set; }
        public string ClaimBenefitAssignment { get; set; }
        public string ClaimReleaseofInformationCode { get; set; }
        public string ClaimDelayReasonCode { get; set; }
        public CLM_I()
        {
            SegmentCode = "CLM";
            ClaimType = "A";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(ClaimID) && !string.IsNullOrEmpty(ClaimAmount) && !string.IsNullOrEmpty(ClaimPOS) && !string.IsNullOrEmpty(ClaimFrequencyCode) && !string.IsNullOrEmpty(ClaimProviderAssignment) && !string.IsNullOrEmpty(ClaimBenefitAssignment) && !string.IsNullOrEmpty(ClaimReleaseofInformationCode);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CLM*" + ClaimID + "*" + ClaimAmount + "***" + ClaimPOS + ":A:" + ClaimFrequencyCode + "**" + ClaimProviderAssignment + "*" + ClaimBenefitAssignment + "*" + ClaimReleaseofInformationCode);
            if (!string.IsNullOrEmpty(ClaimDelayReasonCode)) sb.Append("***********" + ClaimDelayReasonCode);
            sb.Append("~");
            return sb.ToString();
        }
    }
}
