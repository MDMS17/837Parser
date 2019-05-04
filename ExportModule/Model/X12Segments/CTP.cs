using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class CTP : X12SegmentBase
    {
        public string DrugQuantity { get; set; }
        public string DrugQualifier { get; set; }
        public CTP()
        {
            SegmentCode = "CTP";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(DrugQuantity) && !string.IsNullOrEmpty(DrugQualifier);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CTP****" + DrugQuantity + "*" + DrugQualifier);
            sb.Append("~");
            return sb.ToString();
        }
    }
}
