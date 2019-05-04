using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class CRC : X12SegmentBase
    {
        public string CodeCategory { get; set; }
        public string ConditionIndicator { get; set; }
        public string ConditionCode { get; set; }
        public string ConditionCode2 { get; set; }
        public string ConditionCode3 { get; set; }
        public string ConditionCode4 { get; set; }
        public string ConditionCode5 { get; set; }
        public CRC()
        {
            SegmentCode = "CRC";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(CodeCategory) && !string.IsNullOrEmpty(ConditionIndicator) && !string.IsNullOrEmpty(ConditionCode);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CRC*" + CodeCategory + "*" + ConditionIndicator + "*" + ConditionCode);
            if (!string.IsNullOrEmpty(ConditionCode5))
            {
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ConditionCode2) ? "" : ConditionCode2);
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ConditionCode3) ? "" : ConditionCode3);
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ConditionCode4) ? "" : ConditionCode4);
                sb.Append("*" + ConditionCode5);
            }
            else if (!string.IsNullOrEmpty(ConditionCode4))
            {
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ConditionCode2) ? "" : ConditionCode2);
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ConditionCode3) ? "" : ConditionCode3);
                sb.Append("*" + ConditionCode4);
            }
            else if (!string.IsNullOrEmpty(ConditionCode3))
            {
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ConditionCode2) ? "" : ConditionCode2);
                sb.Append("*" + ConditionCode3);
            }
            else if (!string.IsNullOrEmpty(ConditionCode2))
            {
                sb.Append("*" + ConditionCode2);
            }
            sb.Append("~");
            return sb.ToString();
        }
    }
}
