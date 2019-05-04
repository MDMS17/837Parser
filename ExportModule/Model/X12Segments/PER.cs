using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class PERItem
    {
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Phone { get; set; }
        public string PhoneEx { get; set; }
    }
    public class PER : X12SegmentBase
    {
        public List<PERItem> Pers { get; set; }
        public PER()
        {
            SegmentCode = "PER";
            Pers = new List<PERItem>();
        }
        public override bool Valid()
        {
            foreach (PERItem per in Pers)
            {
                if (string.IsNullOrEmpty(per.Email) && string.IsNullOrEmpty(per.Fax) && string.IsNullOrEmpty(per.Phone)) return false;
            }
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            foreach (PERItem per in Pers)
            {
                sb.Append("PER*IC*");
                if (!string.IsNullOrEmpty(per.ContactName)) sb.Append(per.ContactName);
                if (!string.IsNullOrEmpty(per.Email)) sb.Append("*EM*" + per.Email);
                if (!string.IsNullOrEmpty(per.Fax)) sb.Append("*FX*" + per.Fax);
                if (!string.IsNullOrEmpty(per.Phone)) sb.Append("*TE*" + per.Phone);
                if (!string.IsNullOrEmpty(per.PhoneEx)) sb.Append("*EX*" + per.PhoneEx);
                sb.Append("~");
            }
            return sb.ToString();
        }
    }
}
