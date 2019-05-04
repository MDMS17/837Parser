using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class CAS : X12SegmentBase
    {
        public string AdjGroupCode { get; set; }
        public string AdjReasonCode1 { get; set; }
        public string AdjAmount1 { get; set; }
        public string AdjQuantity1 { get; set; }
        public string AdjReasonCode2 { get; set; }
        public string AdjAmount2 { get; set; }
        public string AdjQuantity2 { get; set; }
        public string AdjReasonCode3 { get; set; }
        public string AdjAmount3 { get; set; }
        public string AdjQuantity3 { get; set; }
        public string AdjReasonCode4 { get; set; }
        public string AdjAmount4 { get; set; }
        public string AdjQuantity4 { get; set; }
        public string AdjReasonCode5 { get; set; }
        public string AdjAmount5 { get; set; }
        public string AdjQuantity5 { get; set; }
        public string AdjReasonCode6 { get; set; }
        public string AdjAmount6 { get; set; }
        public string AdjQuantity6 { get; set; }
        public CAS()
        {
            SegmentCode = "CAS";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(AdjGroupCode) && !string.IsNullOrEmpty(AdjReasonCode1) && !string.IsNullOrEmpty(AdjAmount1);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CAS*" + AdjGroupCode + "*" + AdjReasonCode1 + "*" + AdjAmount1);
            if (!string.IsNullOrEmpty(AdjReasonCode2) && !string.IsNullOrEmpty(AdjAmount2))
            {
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(AdjQuantity1) ? "" : AdjQuantity1);
                sb.Append("*" + AdjReasonCode2 + "*" + AdjAmount2);
            }
            if (!string.IsNullOrEmpty(AdjReasonCode3) && !string.IsNullOrEmpty(AdjAmount3))
            {
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(AdjQuantity2) ? "" : AdjQuantity2);
                sb.Append("*" + AdjReasonCode3 + "*" + AdjAmount3);
            }
            if (!string.IsNullOrEmpty(AdjReasonCode4) && !string.IsNullOrEmpty(AdjAmount4))
            {
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(AdjQuantity3) ? "" : AdjQuantity3);
                sb.Append("*" + AdjReasonCode4 + "*" + AdjAmount4);
            }
            if (!string.IsNullOrEmpty(AdjReasonCode5) && !string.IsNullOrEmpty(AdjAmount5))
            {
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(AdjQuantity4) ? "" : AdjQuantity4);
                sb.Append("*" + AdjReasonCode5 + "*" + AdjAmount5);
            }
            if (!string.IsNullOrEmpty(AdjReasonCode6) && !string.IsNullOrEmpty(AdjAmount6))
            {
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(AdjQuantity5) ? "" : AdjQuantity5);
                sb.Append("*" + AdjReasonCode6 + "*" + AdjAmount6);
            }
            if (!string.IsNullOrEmpty(AdjQuantity6)) sb.Append("*" + AdjQuantity6);
            sb.Append("~");
            return sb.ToString();
        }
    }
}
