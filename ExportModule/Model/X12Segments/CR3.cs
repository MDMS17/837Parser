using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class CR3 : X12SegmentBase
    {
        public string CertificationTypeCode { get; set; }
        public string DMEDuration { get; set; }
        public CR3()
        {
            SegmentCode = "CR3";
            LoopName = "2400";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(CertificationTypeCode) && !string.IsNullOrEmpty(DMEDuration);
        }
        public override string ToX12String()
        {
            return "CR3" + "*" + CertificationTypeCode + "*MO*" + DMEDuration + "~";
        }
    }
}
