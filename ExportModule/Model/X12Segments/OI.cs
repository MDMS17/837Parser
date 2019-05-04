using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class OI : X12SegmentBase
    {
        public string BenefitsAssignmentCertificationIndicator { get; set; }
        public string PatientSignatureSourceCode { get; set; }
        public string ReleaseOfInformationCode { get; set; }
        public OI()
        {
            SegmentCode = "OI";
            LoopName = "2320";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(BenefitsAssignmentCertificationIndicator) && !string.IsNullOrEmpty(ReleaseOfInformationCode);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("OI***" + BenefitsAssignmentCertificationIndicator + "*");
            if (!string.IsNullOrEmpty(PatientSignatureSourceCode)) sb.Append(PatientSignatureSourceCode);
            sb.Append("**" + ReleaseOfInformationCode + "~");
            return sb.ToString();
        }
    }
}
