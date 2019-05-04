using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class LQItem
    {
        public string LQSequence { get; set; }
        public string FormQualifier { get; set; }
        public string IndustryCode { get; set; }
    }
    public class FRMItem
    {
        public string LQSequence { get; set; }
        public string QuestionNumber { get; set; }
        public string QuestionResponseIndicator { get; set; }
        public string QuestionResponse { get; set; }
        public string QuestionResponseDate { get; set; }
        public string AllowedChargePercentage { get; set; }
    }
    public class LQ : X12SegmentBase
    {
        public List<LQItem> LQs { get; set; }
        public List<FRMItem> FRMs { get; set; }
        public LQ()
        {
            SegmentCode = "LQ";
            LoopName = "2440";
            LQs = new List<LQItem>();
            FRMs = new List<FRMItem>();
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            foreach (LQItem item in LQs)
            {
                sb.Append("LQ*" + item.FormQualifier + "*" + item.IndustryCode + "~");
                List<FRMItem> frmitems = FRMs.Where(x => x.LQSequence == item.LQSequence).ToList();
                if (frmitems.Count > 0)
                {
                    foreach (FRMItem frmitem in frmitems)
                    {
                        sb.Append("FRM*" + frmitem.QuestionNumber);
                        if (!string.IsNullOrEmpty(frmitem.AllowedChargePercentage))
                        {
                            sb.Append("*");
                            if (!string.IsNullOrEmpty(frmitem.QuestionResponseIndicator)) sb.Append(frmitem.QuestionResponseIndicator);
                            sb.Append("*");
                            if (!string.IsNullOrEmpty(frmitem.QuestionResponse)) sb.Append(frmitem.QuestionResponse);
                            sb.Append("*");
                            if (!string.IsNullOrEmpty(frmitem.QuestionResponseDate)) sb.Append(frmitem.QuestionResponseDate);
                            sb.Append("*" + frmitem.AllowedChargePercentage);
                        }
                        else if (!string.IsNullOrEmpty(frmitem.QuestionResponseDate))
                        {
                            sb.Append("*");
                            if (!string.IsNullOrEmpty(frmitem.QuestionResponseIndicator)) sb.Append(frmitem.QuestionResponseIndicator);
                            sb.Append("*");
                            if (!string.IsNullOrEmpty(frmitem.QuestionResponse)) sb.Append(frmitem.QuestionResponse);
                            sb.Append("*" + frmitem.QuestionResponseDate);
                        }
                        else if (!string.IsNullOrEmpty(frmitem.QuestionResponse))
                        {
                            sb.Append("*");
                            if (!string.IsNullOrEmpty(frmitem.QuestionResponseIndicator)) sb.Append(frmitem.QuestionResponseIndicator);
                            sb.Append("*" + frmitem.QuestionResponse);
                        }
                        else if (!string.IsNullOrEmpty(frmitem.QuestionResponseIndicator))
                        {
                            sb.Append("*" + frmitem.QuestionResponseIndicator);
                        }
                        sb.Append("~");
                    }
                }
            }
            return sb.ToString();
        }
    }
}
