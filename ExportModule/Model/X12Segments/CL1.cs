using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class CL1 : X12SegmentBase
    {
        public string AdmissionTypeCode { get; set; }
        public string AdmissionSourceCode { get; set; }
        public string PatientStatusCode { get; set; }
        public CL1()
        {
            SegmentCode = "CL1";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(AdmissionTypeCode) && !string.IsNullOrEmpty(PatientStatusCode);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CL1*" + AdmissionTypeCode + "*");
            sb.Append(string.IsNullOrEmpty(AdmissionSourceCode) ? "" : AdmissionSourceCode);
            sb.Append("*" + PatientStatusCode + "~");
            return sb.ToString();
        }
    }
}
