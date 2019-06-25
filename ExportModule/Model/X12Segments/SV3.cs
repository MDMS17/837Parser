using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class SV3 : X12SegmentBase
    {
        public string ServiceIDQualifier { get; set; }
        public string ProcedureCode { get; set; }
        public string ProcedureModifier1 { get; set; }
        public string ProcedureModifier2 { get; set; }
        public string ProcedureModifier3 { get; set; }
        public string ProcedureModifier4 { get; set; }
        public string ProcedureDescription { get; set; }
        public string LineItemChargeAmount { get; set; }
        public string LineItemPOS { get; set; }
        public string ServiceUnitQuantity { get; set; }
        public string OralCavityDesignationCode1 { get; set; }
        public string OralCavityDesignationCode2 { get; set; }
        public string OralCavityDesignationCode3 { get; set; }
        public string OralCavityDesignationCode4 { get; set; }
        public string OralCavityDesignationCode5 { get; set; }
        public string ProsthesisCrownOrInlayCode { get; set; }
        public string DiagPointer1 { get; set; }
        public string DiagPointer2 { get; set; }
        public string DiagPointer3 { get; set; }
        public string DiagPointer4 { get; set; }
        public SV3()
        {
            SegmentCode = "SV3";
            LoopName = "2400";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SV3*" + ServiceIDQualifier + "*" + ProcedureCode);
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
            if (!string.IsNullOrEmpty(DiagPointer1))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LineItemPOS)) sb.Append(LineItemPOS);
                sb.Append("*");
                if (!string.IsNullOrEmpty(OralCavityDesignationCode1))
                {
                    sb.Append(OralCavityDesignationCode1);
                    if (!string.IsNullOrEmpty(OralCavityDesignationCode2)) sb.Append(":" + OralCavityDesignationCode2);
                    if (!string.IsNullOrEmpty(OralCavityDesignationCode3)) sb.Append(":" + OralCavityDesignationCode3);
                    if (!string.IsNullOrEmpty(OralCavityDesignationCode4)) sb.Append(":" + OralCavityDesignationCode4);
                    if (!string.IsNullOrEmpty(OralCavityDesignationCode5)) sb.Append(":" + OralCavityDesignationCode5);
                }
                sb.Append("*");
                if (!string.IsNullOrEmpty(ProsthesisCrownOrInlayCode)) sb.Append(ProsthesisCrownOrInlayCode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ServiceUnitQuantity)) sb.Append(ServiceUnitQuantity);
                sb.Append("*****" + DiagPointer1);
                if (!string.IsNullOrEmpty(DiagPointer2)) sb.Append(":" + DiagPointer2);
                if (!string.IsNullOrEmpty(DiagPointer3)) sb.Append(":" + DiagPointer3);
                if (!string.IsNullOrEmpty(DiagPointer4)) sb.Append(":" + DiagPointer4);
            }
            else if (!string.IsNullOrEmpty(ServiceUnitQuantity))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LineItemPOS)) sb.Append(LineItemPOS);
                sb.Append("*");
                if (!string.IsNullOrEmpty(OralCavityDesignationCode1))
                {
                    sb.Append(OralCavityDesignationCode1);
                    if (!string.IsNullOrEmpty(OralCavityDesignationCode2)) sb.Append(":" + OralCavityDesignationCode2);
                    if (!string.IsNullOrEmpty(OralCavityDesignationCode3)) sb.Append(":" + OralCavityDesignationCode3);
                    if (!string.IsNullOrEmpty(OralCavityDesignationCode4)) sb.Append(":" + OralCavityDesignationCode4);
                    if (!string.IsNullOrEmpty(OralCavityDesignationCode5)) sb.Append(":" + OralCavityDesignationCode5);
                }
                sb.Append("*");
                if (!string.IsNullOrEmpty(ProsthesisCrownOrInlayCode)) sb.Append(ProsthesisCrownOrInlayCode);
                sb.Append("*" + ServiceUnitQuantity);
            }
            else if (!string.IsNullOrEmpty(ProsthesisCrownOrInlayCode))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LineItemPOS)) sb.Append(LineItemPOS);
                sb.Append("*");
                if (!string.IsNullOrEmpty(OralCavityDesignationCode1))
                {
                    sb.Append(OralCavityDesignationCode1);
                    if (!string.IsNullOrEmpty(OralCavityDesignationCode2)) sb.Append(":" + OralCavityDesignationCode2);
                    if (!string.IsNullOrEmpty(OralCavityDesignationCode3)) sb.Append(":" + OralCavityDesignationCode3);
                    if (!string.IsNullOrEmpty(OralCavityDesignationCode4)) sb.Append(":" + OralCavityDesignationCode4);
                    if (!string.IsNullOrEmpty(OralCavityDesignationCode5)) sb.Append(":" + OralCavityDesignationCode5);
                }
                sb.Append("*" + ProsthesisCrownOrInlayCode);
            }
            else if (!string.IsNullOrEmpty(OralCavityDesignationCode1))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LineItemPOS)) sb.Append(LineItemPOS);
                sb.Append("*" + OralCavityDesignationCode1);
                if (!string.IsNullOrEmpty(OralCavityDesignationCode2)) sb.Append(":" + OralCavityDesignationCode2);
                if (!string.IsNullOrEmpty(OralCavityDesignationCode3)) sb.Append(":" + OralCavityDesignationCode3);
                if (!string.IsNullOrEmpty(OralCavityDesignationCode4)) sb.Append(":" + OralCavityDesignationCode4);
                if (!string.IsNullOrEmpty(OralCavityDesignationCode5)) sb.Append(":" + OralCavityDesignationCode5);
            }
            else if (!string.IsNullOrEmpty(LineItemPOS))
            {
                sb.Append("*" + LineItemPOS);
            }
            sb.Append("~");
            return sb.ToString();
        }
    }
}
