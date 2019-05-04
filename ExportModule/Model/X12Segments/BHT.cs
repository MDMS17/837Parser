using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class BHT : X12SegmentBase
    {
        public string HirarchicalStructureCode { get; set; }
        public string TransactionSetPurposeCode { get; set; }
        public string TransactionID { get; set; } //batch control number
        public string TransactionDate { get; set; }
        public string TransactionTime { get; set; }
        public string TransactionTypeCode { get; set; }
        public BHT()
        {
            SegmentCode = "BHT";
            LoopName = "Header";
            HirarchicalStructureCode = "0019";
            TransactionSetPurposeCode = "00";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(TransactionID) && !string.IsNullOrEmpty(TransactionTypeCode);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("BHT*" + HirarchicalStructureCode + "*" + TransactionSetPurposeCode + "*" + TransactionID + "*" + TransactionDate + "*" + TransactionTime + "*" + TransactionTypeCode + "~");
            return sb.ToString();
        }
    }
}
