using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportModule.Model.Interfaces
{
    public abstract class X12SegmentBase : IX12Segments
    {
        public string LoopName { get; set; }
        public string SegmentCode { get; set; }
        public abstract bool Valid();
        public abstract string ToX12String();
    }
}
