using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class HI_I : X12SegmentBase
    {
        public List<HIItem> His { get; set; }
        public int HiCounts { get; set; }
        public HI_I()
        {
            SegmentCode = "HI";
            LoopName = "2300";
            His = new List<HIItem>();
            HiCounts = 0;
        }
        public override bool Valid()
        {
            if (His.Count(x => x.HIQual == "ABK" || x.HIQual == "BK") == 0) return false;
            foreach (HIItem item in His) if (string.IsNullOrEmpty(item.HIQual) || string.IsNullOrEmpty(item.HICode)) return false;
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            HIItem primaryDiagnosis = His.Where(x => x.HIQual == "ABK" || x.HIQual == "BK").First();
            sb.Append("HI*" + primaryDiagnosis.HIQual + ":" + primaryDiagnosis.HICode);
            if (!string.IsNullOrEmpty(primaryDiagnosis.PresentOnAdmissionIndicator)) sb.Append(":::::::" + primaryDiagnosis.PresentOnAdmissionIndicator);
            sb.Append("~");
            HiCounts++;
            HIItem admittingDiagnosis = His.Where(x => x.HIQual == "ABJ" || x.HIQual == "BJ").FirstOrDefault();
            if (admittingDiagnosis != null)
            {
                sb.Append("HI*" + admittingDiagnosis.HIQual + ":" + admittingDiagnosis.HICode + "~");
                HiCounts++;
            }
            List<HIItem> patientReasonForVisitDiagnosis = His.Where(x => x.HIQual == "APR" || x.HIQual == "PR").ToList();
            if (patientReasonForVisitDiagnosis.Count > 0)
            {
                sb.Append("HI");
                for (int i = 0; i < patientReasonForVisitDiagnosis.Count; i++)
                {
                    sb.Append("*" + patientReasonForVisitDiagnosis[i].HIQual + ":" + patientReasonForVisitDiagnosis[i].HICode);
                    if (i == 2) break;
                }
                sb.Append("~");
                HiCounts++;
            }
            List<HIItem> externalCauseofInjuryDiagnosis = His.Where(x => x.HIQual == "ABN" || x.HIQual == "BN").ToList();
            if (externalCauseofInjuryDiagnosis.Count > 0)
            {
                sb.Append("HI");
                for (int i = 0; i < externalCauseofInjuryDiagnosis.Count; i++)
                {
                    sb.Append("*" + externalCauseofInjuryDiagnosis[i].HIQual + ":" + externalCauseofInjuryDiagnosis[i].HICode);
                    if (!string.IsNullOrEmpty(externalCauseofInjuryDiagnosis[i].PresentOnAdmissionIndicator)) sb.Append(":::::::" + externalCauseofInjuryDiagnosis[i].PresentOnAdmissionIndicator);
                    if (i == 11) break;
                }
                sb.Append("~");
                HiCounts++;
            }
            HIItem DRGDiagnosis = His.Where(x => x.HIQual == "DR").FirstOrDefault();
            if (DRGDiagnosis != null)
            {
                sb.Append("HI*" + DRGDiagnosis.HIQual + ":" + DRGDiagnosis.HICode + "~");
                HiCounts++;
            }
            List<HIItem> otherDiagnosis = His.Where(x => x.HIQual == "ABF" || x.HIQual == "BF").ToList();
            if (otherDiagnosis.Count > 0)
            {
                sb.Append("HI");
                for (int i = 0; i < otherDiagnosis.Count; i++)
                {
                    if (i == 12)
                    {
                        sb.Append("~HI");
                        HiCounts++;
                    }
                    sb.Append("*" + otherDiagnosis[i].HIQual + ":" + otherDiagnosis[i].HICode);
                    if (!string.IsNullOrEmpty(otherDiagnosis[i].PresentOnAdmissionIndicator)) sb.Append(":::::::" + otherDiagnosis[i].PresentOnAdmissionIndicator);
                    if (i == 23) break;
                }
                sb.Append("~");
                HiCounts++;
            }
            HIItem principleProcedureDiagnosis = His.Where(x => x.HIQual == "BBR" || x.HIQual == "BR" || x.HIQual == "CAH").FirstOrDefault();
            if (principleProcedureDiagnosis != null)
            {
                sb.Append("HI*" + principleProcedureDiagnosis.HIQual + ":" + principleProcedureDiagnosis.HICode + ":D8:" + principleProcedureDiagnosis.HIFromDate + "~");
                HiCounts++;
                List<HIItem> otherProcedureDiagnosis = His.Where(x => x.HIQual == "BBQ" || x.HIQual == "BQ").ToList();
                if (otherProcedureDiagnosis.Count > 0)
                {
                    sb.Append("HI");
                    for (int i = 0; i < otherProcedureDiagnosis.Count; i++)
                    {
                        if (i == 12)
                        {
                            sb.Append("~HI");
                            HiCounts++;
                        }
                        sb.Append("*" + otherProcedureDiagnosis[i].HIQual + ":" + otherProcedureDiagnosis[i].HICode + ":D8:" + otherProcedureDiagnosis[i].HIFromDate);
                        if (i == 23) break;
                    }
                    sb.Append("~");
                    HiCounts++;
                }
            }
            List<HIItem> spanDiagnosis = His.Where(x => x.HIQual == "BI").ToList();
            if (spanDiagnosis.Count > 0)
            {
                sb.Append("HI");
                for (int i = 0; i < spanDiagnosis.Count; i++)
                {
                    if (i == 12)
                    {
                        sb.Append("~HI");
                        HiCounts++;
                    }
                    sb.Append("*" + spanDiagnosis[i].HIQual + ":" + spanDiagnosis[i].HICode + ":RD8:" + spanDiagnosis[i].HIFromDate + "-" + spanDiagnosis[i].HIToDate);
                    if (i == 23) break;
                }
                sb.Append("~");
                HiCounts++;
            }
            List<HIItem> occurrenceDiagnosis = His.Where(x => x.HIQual == "BH").ToList();
            if (occurrenceDiagnosis.Count > 0)
            {
                sb.Append("HI");
                for (int i = 0; i < occurrenceDiagnosis.Count; i++)
                {
                    if (i == 12)
                    {
                        sb.Append("~HI");
                        HiCounts++;
                    }
                    sb.Append("*" + occurrenceDiagnosis[i].HIQual + ":" + occurrenceDiagnosis[i].HICode + ":D8:" + occurrenceDiagnosis[i].HIFromDate);
                    if (i == 23) break;
                }
                sb.Append("~");
                HiCounts++;
            }
            List<HIItem> valueDiagnosis = His.Where(x => x.HIQual == "BE").ToList();
            if (valueDiagnosis.Count > 0)
            {
                sb.Append("HI");
                for (int i = 0; i < valueDiagnosis.Count; i++)
                {
                    if (i == 12)
                    {
                        sb.Append("~HI");
                        HiCounts++;
                    }
                    if (!string.IsNullOrEmpty(valueDiagnosis[i].HIAmount))
                    {
                        sb.Append("*" + valueDiagnosis[i].HIQual + ":" + valueDiagnosis[i].HICode + ":::" + valueDiagnosis[i].HIAmount);
                    }
                    else
                    {
                        sb.Append("*" + valueDiagnosis[i].HIQual + ":" + valueDiagnosis[i].HICode);
                    }
                    if (i == 23) break;
                }
                sb.Append("~");
                HiCounts++;
            }
            List<HIItem> conditionDiagnosis = His.Where(x => x.HIQual == "BG").ToList();
            if (conditionDiagnosis.Count > 0)
            {
                sb.Append("HI");
                for (int i = 0; i < conditionDiagnosis.Count; i++)
                {
                    if (i == 12)
                    {
                        sb.Append("~HI");
                        HiCounts++;
                    }
                    sb.Append("*" + conditionDiagnosis[i].HIQual + ":" + conditionDiagnosis[i].HICode);
                    if (i == 23) break;
                }
                sb.Append("~");
                HiCounts++;
            }
            List<HIItem> treatmentDiagnosis = His.Where(x => x.HIQual == "TC").ToList();
            if (treatmentDiagnosis.Count > 0)
            {
                sb.Append("HI");
                for (int i = 0; i < treatmentDiagnosis.Count; i++)
                {
                    if (i == 12)
                    {
                        sb.Append("~HI");
                        HiCounts++;
                    }
                    sb.Append("*" + treatmentDiagnosis[i].HIQual + ":" + treatmentDiagnosis[i].HICode);
                    if (i == 23) break;
                }
                sb.Append("~");
                HiCounts++;
            }
            return sb.ToString();
        }
    }
}
