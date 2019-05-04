using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class PAT : X12SegmentBase
    {
        public string PatientRelatedCode { get; set; }
        public string PatientRelatedDeathDate { get; set; }
        public string PatientRelatedUnit { get; set; }
        public string PatientRelatedWeight { get; set; }
        public string PatientRelatedPregnancyIndicator { get; set; }
        public PAT()
        {
            SegmentCode = "PAT";
        }
        public override bool Valid()
        {
            return LoopName == "2000C" ? !string.IsNullOrEmpty(PatientRelatedCode) : true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("PAT");
            if (!string.IsNullOrEmpty(PatientRelatedPregnancyIndicator))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(PatientRelatedCode)) sb.Append(PatientRelatedCode);
                sb.Append("****");
                if (!string.IsNullOrEmpty(PatientRelatedDeathDate)) sb.Append("D8*" + PatientRelatedDeathDate);
                else sb.Append("*");
                sb.Append("*");
                if (!string.IsNullOrEmpty(PatientRelatedUnit)) sb.Append(PatientRelatedUnit);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PatientRelatedWeight)) sb.Append(PatientRelatedWeight);
                sb.Append("*" + PatientRelatedPregnancyIndicator);
            }
            else if (!string.IsNullOrEmpty(PatientRelatedWeight))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(PatientRelatedCode)) sb.Append(PatientRelatedCode);
                sb.Append("****");
                if (!string.IsNullOrEmpty(PatientRelatedDeathDate)) sb.Append("D8*" + PatientRelatedDeathDate);
                else sb.Append("*");
                sb.Append("*");
                if (!string.IsNullOrEmpty(PatientRelatedUnit)) sb.Append(PatientRelatedUnit);
                sb.Append("*" + PatientRelatedWeight);
            }
            else if (!string.IsNullOrEmpty(PatientRelatedUnit))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(PatientRelatedCode)) sb.Append(PatientRelatedCode);
                sb.Append("****");
                if (!string.IsNullOrEmpty(PatientRelatedDeathDate)) sb.Append("D8*" + PatientRelatedDeathDate);
                else sb.Append("*");
                sb.Append("*" + PatientRelatedUnit);
            }
            else if (!string.IsNullOrEmpty(PatientRelatedDeathDate))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(PatientRelatedCode)) sb.Append(PatientRelatedCode);
                sb.Append("****D8*" + PatientRelatedDeathDate);
            }
            else if (!string.IsNullOrEmpty(PatientRelatedCode))
            {
                sb.Append("*" + PatientRelatedCode);
            }
            sb.Append("~");
            return sb.ToString();
        }
    }
}
