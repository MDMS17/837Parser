using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class SVD : X12SegmentBase
    {
        public string OtherPayerPrimaryIdentifier { get; set; }
        public string ServiceLinePaidAmount { get; set; }
        public string ServiceQualifier { get; set; }
        public string ProcedureCode { get; set; }
        public string ProcedureModifier1 { get; set; }
        public string ProcedureModifier2 { get; set; }
        public string ProcedureModifier3 { get; set; }
        public string ProcedureModifier4 { get; set; }
        public string ProcedureDescription { get; set; }
        public string PaidServiceUnitCount { get; set; }
        public string BundledLineNumber { get; set; }
        public string ServiceLineRevenueCode { get; set; }
        public SVD()
        {
            SegmentCode = "SVD";
            LoopName = "2430";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SVD*" + OtherPayerPrimaryIdentifier);
            sb.Append("*" + ServiceLinePaidAmount);
            sb.Append("*");
            if (!string.IsNullOrEmpty(ServiceQualifier))
            {
                sb.Append(ServiceQualifier);
                if (!string.IsNullOrEmpty(ProcedureCode)) sb.Append(":" + ProcedureCode);
                if (!string.IsNullOrEmpty(ProcedureDescription))
                {
                    sb.Append(":");
                    if (!string.IsNullOrEmpty(ProcedureModifier1)) sb.Append(ProcedureModifier1);
                    sb.Append(":");
                    if (!string.IsNullOrEmpty(ProcedureModifier2)) sb.Append(ProcedureModifier2);
                    sb.Append(":");
                    if (!string.IsNullOrEmpty(ProcedureModifier3)) sb.Append(ProcedureModifier3);
                    sb.Append(":");
                    if (!string.IsNullOrEmpty(ProcedureModifier4)) sb.Append(ProcedureModifier4);
                    sb.Append(":" + ProcedureDescription);
                }
                else
                {
                    if (!string.IsNullOrEmpty(ProcedureModifier1)) sb.Append(":" + ProcedureModifier1);
                    if (!string.IsNullOrEmpty(ProcedureModifier2)) sb.Append(":" + ProcedureModifier2);
                    if (!string.IsNullOrEmpty(ProcedureModifier3)) sb.Append(":" + ProcedureModifier3);
                    if (!string.IsNullOrEmpty(ProcedureModifier4)) sb.Append(":" + ProcedureModifier4);
                }
            }
            sb.Append("*");
            if (!string.IsNullOrEmpty(ServiceLineRevenueCode)) sb.Append(ServiceLineRevenueCode);
            sb.Append("*" + PaidServiceUnitCount);
            if (!string.IsNullOrEmpty(BundledLineNumber))
            {
                sb.Append("*");
                sb.Append(BundledLineNumber);
            }
            sb.Append("~");
            return sb.ToString();
        }
    }
}
