using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class CN1 : X12SegmentBase
    {
        public string ContractTypeCode { get; set; }
        public string ContractAmount { get; set; }
        public string ContractPercentage { get; set; }
        public string ContractCode { get; set; }
        public string ContractTermsDiscountPercentage { get; set; }
        public string ContractVersionIdentifier { get; set; }
        public CN1()
        {
            SegmentCode = "CN1";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(ContractTypeCode);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CN1*" + ContractTypeCode);
            if (!string.IsNullOrEmpty(ContractVersionIdentifier))
            {
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ContractAmount) ? "" : ContractAmount);
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ContractPercentage) ? "" : ContractPercentage);
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ContractCode) ? "" : ContractCode);
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ContractTermsDiscountPercentage) ? "" : ContractTermsDiscountPercentage);
                sb.Append("*" + ContractVersionIdentifier);
            }
            else if (!string.IsNullOrEmpty(ContractTermsDiscountPercentage))
            {
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ContractAmount) ? "" : ContractAmount);
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ContractPercentage) ? "" : ContractPercentage);
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ContractCode) ? "" : ContractCode);
                sb.Append("*" + ContractTermsDiscountPercentage);
            }
            else if (!string.IsNullOrEmpty(ContractCode))
            {
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ContractAmount) ? "" : ContractAmount);
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ContractPercentage) ? "" : ContractPercentage);
                sb.Append("*" + ContractCode);
            }
            else if (!string.IsNullOrEmpty(ContractPercentage))
            {
                sb.Append("*");
                sb.Append(string.IsNullOrEmpty(ContractAmount) ? "" : ContractAmount);
                sb.Append("*" + ContractPercentage);
            }
            else if (!string.IsNullOrEmpty(ContractAmount))
            {
                sb.Append("*" + ContractAmount);
            }
            sb.Append("~");
            return sb.ToString();
        }
    }
}
