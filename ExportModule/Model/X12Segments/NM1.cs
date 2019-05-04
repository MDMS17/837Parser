using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class NM1 : X12SegmentBase
    {
        public string NameQualifier { get; set; }
        public string NameType { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string IDQualifer { get; set; }
        public string IDCode { get; set; }
        public NM1()
        {
            SegmentCode = "NM1";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(NameQualifier) && !string.IsNullOrEmpty(NameType) && !string.IsNullOrEmpty(LastName);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("NM1*" + NameQualifier + "*" + NameType);
            if (!string.IsNullOrEmpty(IDCode))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LastName)) sb.Append(LastName);
                sb.Append("*");
                if (!string.IsNullOrEmpty(FirstName)) sb.Append(FirstName);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MiddleName)) sb.Append(MiddleName);
                sb.Append("**");
                if (!string.IsNullOrEmpty(Suffix)) sb.Append(Suffix);
                sb.Append("*");
                if (!string.IsNullOrEmpty(IDQualifer)) sb.Append(IDQualifer);
                sb.Append("*" + IDCode);
            }
            else if (!string.IsNullOrEmpty(Suffix))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LastName)) sb.Append(LastName);
                sb.Append("*");
                if (!string.IsNullOrEmpty(FirstName)) sb.Append(FirstName);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MiddleName)) sb.Append(MiddleName);
                sb.Append("**" + Suffix);
            }
            else if (!string.IsNullOrEmpty(MiddleName))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LastName)) sb.Append(LastName);
                sb.Append("*");
                if (!string.IsNullOrEmpty(FirstName)) sb.Append(FirstName);
                sb.Append("*" + MiddleName);
            }
            else if (!string.IsNullOrEmpty(FirstName))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LastName)) sb.Append(LastName);
                sb.Append("*" + FirstName);
            }
            else if (!string.IsNullOrEmpty(LastName))
            {
                sb.Append("*" + LastName);
            }
            sb.Append("~");
            return sb.ToString();
        }
    }
}
