using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class SV1 : X12SegmentBase
    {
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
        public string LineItemPOS { get; set; }
        public string DiagPointer1 { get; set; }
        public string DiagPointer2 { get; set; }
        public string DiagPointer3 { get; set; }
        public string DiagPointer4 { get; set; }
        public string EmergencyIndicator { get; set; }
        public string EPSDTIndicator { get; set; }
        public string FamilyPlanningIndicator { get; set; }
        public string CopayStatusCode { get; set; }
        public SV1()
        {
            SegmentCode = "SV1";
            LoopName = "2400";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(ServiceIDQualifier) && !string.IsNullOrEmpty(ProcedureCode) && !string.IsNullOrEmpty(LineItemChargeAmount) && !string.IsNullOrEmpty(LineItemUnit) && !string.IsNullOrEmpty(ServiceUnitQuantity) && !string.IsNullOrEmpty(DiagPointer1);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SV1*" + ServiceIDQualifier + ":" + ProcedureCode);
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
            sb.Append("*");
            if (!string.IsNullOrEmpty(LineItemPOS)) sb.Append(LineItemPOS);
            sb.Append("**" + DiagPointer1);
            if (!string.IsNullOrEmpty(DiagPointer2)) sb.Append(":" + DiagPointer2);
            if (!string.IsNullOrEmpty(DiagPointer3)) sb.Append(":" + DiagPointer3);
            if (!string.IsNullOrEmpty(DiagPointer4)) sb.Append(":" + DiagPointer4);
            if (!string.IsNullOrEmpty(CopayStatusCode))
            {
                sb.Append("**");
                if (!string.IsNullOrEmpty(EmergencyIndicator)) sb.Append(EmergencyIndicator);
                sb.Append("**");
                if (!string.IsNullOrEmpty(EPSDTIndicator)) sb.Append(EPSDTIndicator);
                sb.Append("*");
                if (!string.IsNullOrEmpty(FamilyPlanningIndicator)) sb.Append(FamilyPlanningIndicator);
                sb.Append("***" + CopayStatusCode);
            }
            else if (!string.IsNullOrEmpty(FamilyPlanningIndicator))
            {
                sb.Append("**");
                if (!string.IsNullOrEmpty(EmergencyIndicator)) sb.Append(EmergencyIndicator);
                sb.Append("**");
                if (!string.IsNullOrEmpty(EPSDTIndicator)) sb.Append(EPSDTIndicator);
                sb.Append("*" + FamilyPlanningIndicator);
            }
            else if (!string.IsNullOrEmpty(EPSDTIndicator))
            {
                sb.Append("**");
                if (!string.IsNullOrEmpty(EmergencyIndicator)) sb.Append(EmergencyIndicator);
                sb.Append("**" + EPSDTIndicator);
            }
            else if (!string.IsNullOrEmpty(EmergencyIndicator))
            {
                sb.Append("**" + EmergencyIndicator);
            }
            sb.Append("~");
            return sb.ToString();
        }
    }
}
