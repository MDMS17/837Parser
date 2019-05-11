using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportModule.Model
{
    public class BillingProviderAdditionalDataElements
    {
        public string EIN { get; set; }
        public string ProviderAddress { get; set; }
        public string ProviderCity { get; set; }
        public string ProviderState { get; set; }
        public string ProviderZip { get; set; }
        public string ProviderCountry { get; set; }
    }
}
