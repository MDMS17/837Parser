using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class DN1 : X12SegmentBase
    {
        public string OrthoMonthTotal { get; set; }
        public string OrthoMonthRemaining { get; set; }
        public DN1()
        {
            SegmentCode = "DN1";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DN1");
            if (!string.IsNullOrEmpty(OrthoMonthTotal)) sb.Append("*" + OrthoMonthTotal);
            if (!string.IsNullOrEmpty(OrthoMonthRemaining))
            {
                if (string.IsNullOrEmpty(OrthoMonthTotal)) sb.Append("***");
                else sb.Append("*");
                sb.Append(OrthoMonthRemaining);
            }
            if (string.IsNullOrEmpty(OrthoMonthTotal) && string.IsNullOrEmpty(OrthoMonthRemaining)) sb.Append("****Y");
            sb.Append("~");
            return sb.ToString();
        }
    }
}
