using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class DTP : X12SegmentBase
    {
        public string DateCode { get; set; }
        public string DateQualifier { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DTP()
        {
            SegmentCode = "DTP";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(DateCode) && ((DateQualifier == "D8" && !string.IsNullOrEmpty(StartDate)) || (DateQualifier == "RD8" && !string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate)));
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DTP*" + DateCode + "*" + DateQualifier + "*" + StartDate);
            if (DateQualifier == "RD8" && !string.IsNullOrEmpty(EndDate)) sb.Append("-" + EndDate);
            sb.Append("~");
            return sb.ToString();
        }
    }
}
