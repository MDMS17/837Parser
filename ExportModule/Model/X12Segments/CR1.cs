using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class CR1 : X12SegmentBase
    {
        public string AmbulanceWeight { get; set; }
        public string AmbulanceReasonCode { get; set; }
        public string AmbulanceQuantity { get; set; }
        public string AmbulanceRoundTripDescription { get; set; }
        public string AmbulanceStretcherDescription { get; set; }
        public CR1()
        {
            SegmentCode = "CR1";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(AmbulanceReasonCode) && !string.IsNullOrEmpty(AmbulanceQuantity);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CR1");
            if (string.IsNullOrEmpty(AmbulanceWeight)) sb.Append("**");
            else sb.Append("*LB*" + AmbulanceWeight);
            sb.Append("**" + AmbulanceReasonCode + "*DH*" + AmbulanceQuantity);
            if (!string.IsNullOrEmpty(AmbulanceStretcherDescription))
            {
                sb.Append("***");
                sb.Append(string.IsNullOrEmpty(AmbulanceRoundTripDescription) ? "" : AmbulanceRoundTripDescription);
                sb.Append("*" + AmbulanceStretcherDescription);
            }
            else if (!string.IsNullOrEmpty(AmbulanceRoundTripDescription))
            {
                sb.Append("***" + AmbulanceRoundTripDescription);
            }
            sb.Append("~");
            return sb.ToString();
        }
    }
}
