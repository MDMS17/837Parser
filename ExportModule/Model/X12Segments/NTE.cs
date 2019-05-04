using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class NTE : X12SegmentBase
    {
        public string NoteCode { get; set; }
        public string NoteText { get; set; }
        public NTE()
        {
            SegmentCode = "NTE";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(NoteCode) && !string.IsNullOrEmpty(NoteText);
        }
        public override string ToX12String()
        {
            return "NTE*" + NoteCode + "*" + NoteText + "~";
        }
    }
}
