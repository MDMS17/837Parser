using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class PRV : X12SegmentBase
    {
        public string ProviderQualifier { get; set; }
        public string ProviderTaxonomyCode { get; set; }
        public PRV()
        {
            SegmentCode = "PRV";
            //if (ProviderQualifier == "85") ProviderQualifier = "BI";
            //if (ProviderQualifier == "82") ProviderQualifier = "PE";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(ProviderTaxonomyCode);
        }
        public override string ToX12String()
        {
            return "PRV*" + ProviderQualifier + "*PXC*" + ProviderTaxonomyCode + "~";
        }
    }
}
