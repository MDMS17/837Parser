using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class SV5 : X12SegmentBase
    {
        public string DMEQualifier { get; set; }
        public string DMEProcedureCode { get; set; }
        public string DMEDays { get; set; }
        public string DMERentalPrice { get; set; }
        public string DMEPurchasePrice { get; set; }
        public string DMEFrequencyCode { get; set; }
        public SV5()
        {
            SegmentCode = "SV5";
            LoopName = "2400";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(DMEQualifier) && !string.IsNullOrEmpty(DMEProcedureCode) && !string.IsNullOrEmpty(DMEDays) && !string.IsNullOrEmpty(DMERentalPrice) && !string.IsNullOrEmpty(DMEPurchasePrice) && !string.IsNullOrEmpty(DMEFrequencyCode);
        }
        public override string ToX12String()
        {
            return "SV5*" + DMEQualifier + ":" + DMEProcedureCode + "*DA*" + DMEDays + "*" + DMERentalPrice + "*" + DMEPurchasePrice + "*" + DMEFrequencyCode + "~";
        }
    }
}
