using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class ISA : X12SegmentBase
    {
        public string InterchangeSenderID { get; set; }
        public string InterchangeReceiverID { get; set; }
        public string InterchangeDate { get; set; }
        public string InterchangeTime { get; set; }
        public string InterchangeControlNumber { get; set; }
        public string ProductionFlag { get; set; }
        public ISA()
        {
            SegmentCode = "ISA";
            LoopName = "Header";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ISA*00*          *00*          *ZZ*");
            sb.Append(InterchangeSenderID.PadRight(15, ' '));
            sb.Append("*ZZ*" + InterchangeReceiverID.PadRight(15, ' '));
            sb.Append("*" + InterchangeDate.Substring(2, 6) + "*" + InterchangeTime + "*^*00501*" + InterchangeControlNumber + "*1*" + ProductionFlag + "*:~");
            return sb.ToString();
        }
    }
}
