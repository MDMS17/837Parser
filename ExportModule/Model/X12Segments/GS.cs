using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class GS : X12SegmentBase
    {
        public string FunctionalIDCode { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string TransactionDate { get; set; }
        public string TransactrionTime { get; set; }
        public string GroupControlNumber { get; set; }
        public string ResponsibleAgencyCode { get; set; }
        public string VersionID { get; set; }
        public GS()
        {
            SegmentCode = "GS";
            LoopName = "Header";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(FunctionalIDCode) && !string.IsNullOrEmpty(SenderID) && !string.IsNullOrEmpty(ReceiverID) && !string.IsNullOrEmpty(TransactionDate) && !string.IsNullOrEmpty(TransactrionTime) && !string.IsNullOrEmpty(GroupControlNumber) && !string.IsNullOrEmpty(ResponsibleAgencyCode) && !string.IsNullOrEmpty(VersionID);
        }
        public override string ToX12String()
        {
            return "GS*" + FunctionalIDCode + "*" + SenderID + "*" + ReceiverID + "*" + TransactionDate + "*" + TransactrionTime + "*" + GroupControlNumber + "*" + ResponsibleAgencyCode + "*" + VersionID + "~";
        }
    }
}
