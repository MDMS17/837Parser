using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class HL : X12SegmentBase
    {
        public string HLID { get; set; }
        public string ParentID { get; set; }
        public string LevelCode { get; set; }
        public string ChildCode { get; set; }
        public HL()
        {
            SegmentCode = "HL";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            if (LoopName == "2000A") sb.Append("HL*" + HLID + "**20*1~");
            else if (LoopName == "2000B") sb.Append("HL*" + HLID + "*" + ParentID + "*22*" + ChildCode + "~");
            else if (LoopName == "200C") sb.Append("HL*" + HLID + "*" + ParentID + "23*0~");
            return sb.ToString();
        }
    }
}
