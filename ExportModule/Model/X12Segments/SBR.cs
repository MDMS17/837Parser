using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class SBR : X12SegmentBase
    {
        public string SubscriberSequenceNumber { get; set; }
        public string SubscriberRelationshipCode { get; set; }
        public string InsuredGroupNumber { get; set; }
        public string OtherInsuredGroupName { get; set; }
        public string InsuredTypeCode { get; set; }
        public string ClaimFilingCode { get; set; }
        public SBR()
        {
            SegmentCode = "SBR";
        }
        public override bool Valid()
        {
            return LoopName == "2000B" ? !string.IsNullOrEmpty(SubscriberSequenceNumber) : !string.IsNullOrEmpty(SubscriberSequenceNumber) && !string.IsNullOrEmpty(SubscriberRelationshipCode);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SBR*" + SubscriberSequenceNumber);
            if (!string.IsNullOrEmpty(ClaimFilingCode))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(SubscriberRelationshipCode)) sb.Append(SubscriberRelationshipCode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(InsuredGroupNumber)) sb.Append(InsuredGroupNumber);
                sb.Append("*");
                if (!string.IsNullOrEmpty(OtherInsuredGroupName)) sb.Append(OtherInsuredGroupName);
                sb.Append("*");
                if (!string.IsNullOrEmpty(InsuredTypeCode)) sb.Append(InsuredTypeCode);
                sb.Append("****" + ClaimFilingCode);
            }
            else if (!string.IsNullOrEmpty(InsuredTypeCode))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(SubscriberRelationshipCode)) sb.Append(SubscriberRelationshipCode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(InsuredGroupNumber)) sb.Append(InsuredGroupNumber);
                sb.Append("*");
                if (!string.IsNullOrEmpty(OtherInsuredGroupName)) sb.Append(OtherInsuredGroupName);
                sb.Append("*" + InsuredTypeCode);
            }
            else if (!string.IsNullOrEmpty(OtherInsuredGroupName))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(SubscriberRelationshipCode)) sb.Append(SubscriberRelationshipCode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(InsuredGroupNumber)) sb.Append(InsuredGroupNumber);
                sb.Append("*" + OtherInsuredGroupName);
            }
            else if (!string.IsNullOrEmpty(OtherInsuredGroupName))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(SubscriberRelationshipCode)) sb.Append(SubscriberRelationshipCode);
                sb.Append("*" + OtherInsuredGroupName);
            }
            else if (!string.IsNullOrEmpty(SubscriberRelationshipCode))
            {
                sb.Append("*" + SubscriberRelationshipCode);
            }
            sb.Append("~");
            return sb.ToString();
        }
    }
}
