using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class PS1 : X12SegmentBase
    {
        public string PurchasedServiceProviderIdentifier { get; set; }
        public string PurchasedServiceChargeAmount { get; set; }
        public PS1()
        {
            SegmentCode = "PS1";
            LoopName = "2400";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(PurchasedServiceProviderIdentifier) && !string.IsNullOrEmpty(PurchasedServiceChargeAmount);
        }
        public override string ToX12String()
        {
            return "PS1*" + PurchasedServiceProviderIdentifier + "*" + PurchasedServiceChargeAmount + "~";
        }
    }
}
