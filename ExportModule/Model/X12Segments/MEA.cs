using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class MEA : X12SegmentBase
    {
        public string TestCode { get; set; }
        public string MeasurementQualifier { get; set; }
        public string TestResult { get; set; }
        public MEA()
        {
            SegmentCode = "MEA";
            LoopName = "2400";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(TestCode) && !string.IsNullOrEmpty(MeasurementQualifier) && !string.IsNullOrEmpty(TestResult);
        }
        public override string ToX12String()
        {
            return "MEA*" + TestCode + "*" + MeasurementQualifier + "*" + TestResult + "~";
        }
    }
}
