using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class AMT : X12SegmentBase
    {
        public string AmountQualifier { get; set; }
        public string Amount { get; set; }
        public AMT()
        {
            SegmentCode = "AMT";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(AmountQualifier) && !string.IsNullOrEmpty(Amount);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("AMT*" + AmountQualifier + "*" + Amount);
            sb.Append("~");
            return sb.ToString();
        }
    }
}
