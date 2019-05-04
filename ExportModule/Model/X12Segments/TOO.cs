using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class TooItem
    {
        public string ToothNumber { get; set; }
        public string StatusCode { get; set; }
        public string SurfaceCode2 { get; set; }
        public string SurfaceCode3 { get; set; }
        public string SurfaceCode4 { get; set; }
        public string SurfaceCode5 { get; set; }
    }
    public class TOO : X12SegmentBase
    {
        public List<TooItem> TooItems { get; set; }
        public TOO()
        {
            SegmentCode = "TOO";
            LoopName = "2400";
            TooItems = new List<TooItem>();
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            foreach (TooItem item in TooItems)
            {
                sb.Append("TOO*JP*" + item.ToothNumber);
                if (!string.IsNullOrEmpty(item.StatusCode))
                {
                    sb.Append("*" + item.StatusCode);
                    if (!string.IsNullOrEmpty(item.SurfaceCode2)) sb.Append(":" + item.SurfaceCode2);
                    if (!string.IsNullOrEmpty(item.SurfaceCode3)) sb.Append(":" + item.SurfaceCode3);
                    if (!string.IsNullOrEmpty(item.SurfaceCode4)) sb.Append(":" + item.SurfaceCode4);
                    if (!string.IsNullOrEmpty(item.SurfaceCode5)) sb.Append(":" + item.SurfaceCode5);
                }
                sb.Append("~");
            }
            return sb.ToString();
        }
    }
}
