using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class MOA : X12SegmentBase
    {
        public string ReimbursementRate { get; set; }
        public string HCPCSPayableAmount { get; set; }
        public string MOA_ClaimPaymentRemarkCode1 { get; set; }
        public string MOA_ClaimPaymentRemarkCode2 { get; set; }
        public string MOA_ClaimPaymentRemarkCode3 { get; set; }
        public string MOA_ClaimPaymentRemarkCode4 { get; set; }
        public string MOA_ClaimPaymentRemarkCode5 { get; set; }
        public string EndStageRenalDiseasePaymentAmount { get; set; }
        public string MOA_NonPayableProfessionalComponentBilledAmount { get; set; }
        public MOA()
        {
            SegmentCode = "MOA";
            LoopName = "2320";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("MOA");
            if (!string.IsNullOrEmpty(MOA_NonPayableProfessionalComponentBilledAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(ReimbursementRate)) sb.Append(ReimbursementRate);
                sb.Append("*");
                if (!string.IsNullOrEmpty(HCPCSPayableAmount)) sb.Append(HCPCSPayableAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode1)) sb.Append(MOA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode2)) sb.Append(MOA_ClaimPaymentRemarkCode2);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode3)) sb.Append(MOA_ClaimPaymentRemarkCode3);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode4)) sb.Append(MOA_ClaimPaymentRemarkCode4);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode5)) sb.Append(MOA_ClaimPaymentRemarkCode5);
                sb.Append("*");
                if (!string.IsNullOrEmpty(EndStageRenalDiseasePaymentAmount)) sb.Append(EndStageRenalDiseasePaymentAmount);
                sb.Append("*" + MOA_NonPayableProfessionalComponentBilledAmount);
            }
            else if (!string.IsNullOrEmpty(EndStageRenalDiseasePaymentAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(ReimbursementRate)) sb.Append(ReimbursementRate);
                sb.Append("*");
                if (!string.IsNullOrEmpty(HCPCSPayableAmount)) sb.Append(HCPCSPayableAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode1)) sb.Append(MOA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode2)) sb.Append(MOA_ClaimPaymentRemarkCode2);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode3)) sb.Append(MOA_ClaimPaymentRemarkCode3);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode4)) sb.Append(MOA_ClaimPaymentRemarkCode4);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode5)) sb.Append(MOA_ClaimPaymentRemarkCode5);
                sb.Append("*" + EndStageRenalDiseasePaymentAmount);
            }
            else if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode5))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(ReimbursementRate)) sb.Append(ReimbursementRate);
                sb.Append("*");
                if (!string.IsNullOrEmpty(HCPCSPayableAmount)) sb.Append(HCPCSPayableAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode1)) sb.Append(MOA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode2)) sb.Append(MOA_ClaimPaymentRemarkCode2);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode3)) sb.Append(MOA_ClaimPaymentRemarkCode3);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode4)) sb.Append(MOA_ClaimPaymentRemarkCode4);
                sb.Append("*" + MOA_ClaimPaymentRemarkCode5);
            }
            else if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode4))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(ReimbursementRate)) sb.Append(ReimbursementRate);
                sb.Append("*");
                if (!string.IsNullOrEmpty(HCPCSPayableAmount)) sb.Append(HCPCSPayableAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode1)) sb.Append(MOA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode2)) sb.Append(MOA_ClaimPaymentRemarkCode2);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode3)) sb.Append(MOA_ClaimPaymentRemarkCode3);
                sb.Append("*" + MOA_ClaimPaymentRemarkCode4);
            }
            else if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode3))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(ReimbursementRate)) sb.Append(ReimbursementRate);
                sb.Append("*");
                if (!string.IsNullOrEmpty(HCPCSPayableAmount)) sb.Append(HCPCSPayableAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode1)) sb.Append(MOA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode2)) sb.Append(MOA_ClaimPaymentRemarkCode2);
                sb.Append("*" + MOA_ClaimPaymentRemarkCode3);
            }
            else if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode2))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(ReimbursementRate)) sb.Append(ReimbursementRate);
                sb.Append("*");
                if (!string.IsNullOrEmpty(HCPCSPayableAmount)) sb.Append(HCPCSPayableAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode1)) sb.Append(MOA_ClaimPaymentRemarkCode1);
                sb.Append("*" + MOA_ClaimPaymentRemarkCode2);
            }
            else if (!string.IsNullOrEmpty(MOA_ClaimPaymentRemarkCode1))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(ReimbursementRate)) sb.Append(ReimbursementRate);
                sb.Append("*");
                if (!string.IsNullOrEmpty(HCPCSPayableAmount)) sb.Append(HCPCSPayableAmount);
                sb.Append("*" + MOA_ClaimPaymentRemarkCode1);
            }
            else if (!string.IsNullOrEmpty(HCPCSPayableAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(ReimbursementRate)) sb.Append(ReimbursementRate);
                sb.Append("*" + HCPCSPayableAmount);
            }
            else if (!string.IsNullOrEmpty(ReimbursementRate))
            {
                sb.Append("*" + ReimbursementRate);
            }
            sb.Append("~");
            return sb.ToString();
        }
    }
}
