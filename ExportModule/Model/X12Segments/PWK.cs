using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class PWKItem
    {
        public string ReportTypeCode { get; set; }
        public string ReportTransmissionCode { get; set; }
        public string AttachmentControlNumber { get; set; }
    }
    public class PWK : X12SegmentBase
    {
        public List<PWKItem> Pwks { get; set; }
        public PWK()
        {
            SegmentCode = "PWK";
            Pwks = new List<PWKItem>();
        }
        public override bool Valid()
        {
            foreach (PWKItem pwk in Pwks) if (string.IsNullOrEmpty(pwk.ReportTypeCode) || string.IsNullOrEmpty(pwk.ReportTransmissionCode)) return false;
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            foreach (PWKItem pwk in Pwks)
            {
                sb.Append("PWK*" + pwk.ReportTypeCode + "*" + pwk.ReportTransmissionCode);
                if (!string.IsNullOrEmpty(pwk.AttachmentControlNumber)) sb.Append("***AC*" + pwk.AttachmentControlNumber);
                sb.Append("~");
            }
            return sb.ToString();
        }
    }
}
