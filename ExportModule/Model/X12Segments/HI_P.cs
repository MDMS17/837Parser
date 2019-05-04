using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class HIItem
    {
        public string HIQual { get; set; }
        public string HICode { get; set; }
        public string PresentOnAdmissionIndicator { get; set; }
        public string HIFromDate { get; set; }
        public string HIToDate { get; set; }
        public string HIAmount { get; set; }
    }
    public class HI_P : X12SegmentBase
    {
        public List<HIItem> His { get; set; }
        public int HiCount { get; set; }
        public HI_P()
        {
            SegmentCode = "HI";
            LoopName = "2300";
            His = new List<HIItem>();
            HiCount = 0;
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
            if (His.Count(x => x.HIQual == "ABK" || x.HIQual == "BK") > 0)
            {
                HIItem primaryDiagnosis = His.Where(x => x.HIQual == "ABK" || x.HIQual == "BK").First();
                sb.Append("HI*" + primaryDiagnosis.HIQual + ":" + primaryDiagnosis.HICode);
                List<HIItem> additionalDiagnosis = His.Where(x => x.HIQual == "ABF" || x.HIQual == "BF").ToList();
                if (additionalDiagnosis.Count > 0)
                {
                    foreach (HIItem item in additionalDiagnosis) sb.Append("*" + item.HIQual + ":" + item.HICode);
                }
                sb.Append("~");
                HiCount++;
            }
            if (His.Count(x => x.HIQual == "BP") > 0)
            {
                HIItem anesthesiaDiagnosis = His.Where(x => x.HIQual == "BP").First();
                sb.Append("HI*" + anesthesiaDiagnosis.HIQual + ":" + anesthesiaDiagnosis.HICode);
                if (His.Count(x => x.HIQual == "BO") > 0)
                {
                    HIItem anesthesiaAdditional = His.Where(x => x.HIQual == "BO").First();
                    sb.Append("*BO:" + anesthesiaAdditional.HICode);
                }
                sb.Append("~");
                HiCount++;
            }
            if (His.Count(x => x.HIQual == "BG") > 0)
            {
                sb.Append("HI");
                List<HIItem> conditionCodes = His.Where(x => x.HIQual == "BG").ToList();
                for (int i = 0; i < conditionCodes.Count; i++)
                {
                    if (i == 12) { sb.Append("~HI"); }
                    sb.Append("*BG:" + conditionCodes[i].HICode);
                    if (i == 23) break;
                }
                sb.Append("~");
                HiCount++;
            }
            return sb.ToString();
        }
    }
}
