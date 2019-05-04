using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class K3 : X12SegmentBase
    {
        public List<string> K3s { get; set; }
        public K3()
        {
            SegmentCode = "K3";
            K3s = new List<string>();
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < K3s.Count; i++)
            {
                sb.Append("K3*" + K3s[i] + "~");
                if (i == 9) break;
            }
            return sb.ToString();
        }
    }
}
