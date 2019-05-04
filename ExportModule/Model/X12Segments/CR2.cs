using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class CR2 : X12SegmentBase
    {
        public string PatientConditionCode { get; set; }
        public string PatientConditionDescription1 { get; set; }
        public string PatientConditionDescription2 { get; set; }
        public CR2()
        {
            SegmentCode = "CR2";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(PatientConditionCode);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CR2********" + PatientConditionCode);
            if (!string.IsNullOrEmpty(PatientConditionDescription2))
            {
                sb.Append("**");
                sb.Append(string.IsNullOrEmpty(PatientConditionDescription1) ? "" : PatientConditionDescription1);
                sb.Append("*" + PatientConditionDescription2);
            }
            else if (!string.IsNullOrEmpty(PatientConditionDescription1))
            {
                sb.Append("**" + PatientConditionDescription1);
            }
            sb.Append("~");
            return sb.ToString();
        }
    }
}
