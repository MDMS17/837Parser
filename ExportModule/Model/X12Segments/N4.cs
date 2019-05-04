using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class N4 : X12SegmentBase
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public string CountrySubCode { get; set; }
        public N4()
        {
            SegmentCode = "N4";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(City);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("N4*" + City);
            if (!string.IsNullOrEmpty(CountrySubCode))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(State)) sb.Append(State);
                sb.Append("*");
                if (!string.IsNullOrEmpty(Zipcode)) sb.Append(Zipcode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(Country)) sb.Append(Country);
                sb.Append("***" + CountrySubCode);
            }
            else if (!string.IsNullOrEmpty(Country))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(State)) sb.Append(State);
                sb.Append("*");
                if (!string.IsNullOrEmpty(Zipcode)) sb.Append(Zipcode);
                sb.Append("*" + Country);
            }
            else if (!string.IsNullOrEmpty(Zipcode))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(State)) sb.Append(State);
                sb.Append("*" + Zipcode);
            }
            else if (!string.IsNullOrEmpty(State))
            {
                sb.Append("*" + State);
            }
            sb.Append("~");
            return sb.ToString();
        }
    }
}
