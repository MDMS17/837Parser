using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class SV2 : X12SegmentBase
    {
        public string RevenueCode { get; set; }
        public string ServiceIDQualifier { get; set; }
        public string ProcedureCode { get; set; }
        public string ProcedureModifier1 { get; set; }
        public string ProcedureModifier2 { get; set; }
        public string ProcedureModifier3 { get; set; }
        public string ProcedureModifier4 { get; set; }
        public string ProcedureDescription { get; set; }
        public string LineItemChargeAmount { get; set; }
        public string LineItemUnit { get; set; }
        public string ServiceUnitQuantity { get; set; }
        public string LineItemDeniedChargeAmount { get; set; }
        public SV2()
        {
            SegmentCode = "SV2";
            LoopName = "2400";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(RevenueCode) && !string.IsNullOrEmpty(LineItemChargeAmount) && !string.IsNullOrEmpty(LineItemUnit) && !string.IsNullOrEmpty(ServiceUnitQuantity);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SV2*" + RevenueCode);
            sb.Append("*");
            if (!string.IsNullOrEmpty(ServiceIDQualifier)) sb.Append(ServiceIDQualifier);
            if (!string.IsNullOrEmpty(ProcedureCode)) sb.Append(":" + ProcedureCode);
            if (!string.IsNullOrEmpty(ProcedureDescription))
            {
                int delimiterrepeat = 0;
                if (!string.IsNullOrEmpty(ProcedureModifier1)) { sb.Append(":" + ProcedureModifier1); delimiterrepeat++; }
                if (!string.IsNullOrEmpty(ProcedureModifier2)) { sb.Append(":" + ProcedureModifier2); delimiterrepeat++; }
                if (!string.IsNullOrEmpty(ProcedureModifier3)) { sb.Append(":" + ProcedureModifier3); delimiterrepeat++; }
                if (!string.IsNullOrEmpty(ProcedureModifier4)) { sb.Append(":" + ProcedureModifier4); delimiterrepeat++; }
                sb.Append(new String(':', 5 - delimiterrepeat) + ProcedureDescription);
            }
            else
            {
                if (!string.IsNullOrEmpty(ProcedureModifier1)) sb.Append(":" + ProcedureModifier1);
                if (!string.IsNullOrEmpty(ProcedureModifier2)) sb.Append(":" + ProcedureModifier2);
                if (!string.IsNullOrEmpty(ProcedureModifier3)) sb.Append(":" + ProcedureModifier3);
                if (!string.IsNullOrEmpty(ProcedureModifier4)) sb.Append(":" + ProcedureModifier4);
            }
            sb.Append("*" + LineItemChargeAmount);
            sb.Append("*" + LineItemUnit);
            sb.Append("*" + ServiceUnitQuantity);
            if (!string.IsNullOrEmpty(LineItemDeniedChargeAmount)) sb.Append("**" + LineItemDeniedChargeAmount);
            sb.Append("~");
            return sb.ToString();
        }
    }
}
