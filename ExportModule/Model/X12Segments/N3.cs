using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class N3 : X12SegmentBase
    {
        public string Address { get; set; }
        public string Address2 { get; set; }
        public N3()
        {
            SegmentCode = "N3";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(Address);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("N3*" + Address);
            if (!string.IsNullOrEmpty(Address2)) sb.Append("*" + Address2);
            sb.Append("~");
            return sb.ToString();
        }
    }
}
