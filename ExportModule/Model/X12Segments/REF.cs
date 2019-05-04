using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class REFItem
    {
        public string ProviderQualifier { get; set; }
        public string ProviderID { get; set; }
        public string OtherPayerPrimaryIDentification { get; set; } //only for referral number, prior authorization
    }
    public class REF : X12SegmentBase
    {
        public List<REFItem> Refs { get; set; }
        public REF()
        {
            SegmentCode = "REF";
            Refs = new List<REFItem>();
        }
        public override bool Valid()
        {
            foreach (REFItem Ref in Refs) if (string.IsNullOrEmpty(Ref.ProviderQualifier) || string.IsNullOrEmpty(Ref.ProviderID)) return false;
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            foreach (REFItem Ref in Refs)
            {
                sb.Append("REF*" + Ref.ProviderQualifier + "*" + Ref.ProviderID);
                if (!string.IsNullOrEmpty(Ref.OtherPayerPrimaryIDentification)) sb.Append("**2U:" + Ref.OtherPayerPrimaryIDentification);
                sb.Append("~");
            }
            return sb.ToString();
        }
    }
}
