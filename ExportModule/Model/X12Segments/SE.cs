using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class SE : X12SegmentBase
    {
        public string SegmentCount { get; set; }
        public string TransactionControlNumber { get; set; }
        public SE()
        {
            SegmentCode = "SE";
            LoopName = "Trailer";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            return "SE*" + SegmentCount + "*" + TransactionControlNumber + "~";
        }
    }
}
