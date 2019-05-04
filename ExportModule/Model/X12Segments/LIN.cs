using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class LIN : X12SegmentBase
    {
        public string LINQualifier { get; set; }
        public string NationalDrugCode { get; set; }
        public LIN()
        {
            SegmentCode = "LIN";
            LoopName = "2410";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(LINQualifier) && !string.IsNullOrEmpty(NationalDrugCode);
        }
        public override string ToX12String()
        {
            return "LIN**" + LINQualifier + "*" + NationalDrugCode + "~";
        }
    }
}
