using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class CUR : X12SegmentBase
    {
        public string ProviderCurrencyCode { get; set; }
        public CUR()
        {
            SegmentCode = "CUR";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(ProviderCurrencyCode);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CUR*85*" + ProviderCurrencyCode);
            sb.Append("~");
            return sb.ToString();
        }
    }
}
