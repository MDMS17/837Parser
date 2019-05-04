using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ExportModule
{
    public class SubHistContext : DbContext
    {
        public SubHistContext() : base("name=Titan") { }

    }
}
