using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class DN2Item
    {
        public string ToothNumber { get; set; }
        public string StatusCode { get; set; }
    }
    public class DN2 : X12SegmentBase
    {
        public List<DN2Item> DN2Items { get; set; }
        public DN2()
        {
            SegmentCode = "DN2";
            LoopName = "2300"; DN2Items = new List<DN2Item>();
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DN2Item item in DN2Items)
            {
                sb.Append("DN2*" + item.ToothNumber + "*" + item.StatusCode + "****JP~");
            }
            return sb.ToString();
        }
    }
}
