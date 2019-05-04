using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class QTY : X12SegmentBase
    {
        public string AmbulancePatientCount { get; set; }
        public string ObstetricAdditionalUnits { get; set; }
        public QTY()
        {
            SegmentCode = "QTY";
            LoopName = "2400";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(AmbulancePatientCount)) sb.Append("QTY*PT*" + AmbulancePatientCount + "~");
            if (!string.IsNullOrEmpty(ObstetricAdditionalUnits)) sb.Append("QTY*FL" + ObstetricAdditionalUnits + "~");
            return sb.ToString();
        }
    }
}
