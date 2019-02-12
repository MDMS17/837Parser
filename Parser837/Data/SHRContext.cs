using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Parser837
{
    public class SHRContext : DbContext
    {
        public SHRContext() : base("name=VEGADEVEDI")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Sub_History");
            modelBuilder.Entity<ClaimTempHost>().ToTable("ClaimTempHost");
        }
        public virtual DbSet<ClaimTempHost> ClaimTempHost { get; set; }
        public virtual DbSet<ClaimHeader> ClaimHeaders { get; set; }
        public virtual DbSet<ServiceLine> ServiceLines { get; set; }
        public virtual DbSet<SubmissionLog> SubmissionLogs { get; set; }
        public virtual DbSet<ClaimCAS> ClaimCAS { get; set; }
        public virtual DbSet<ClaimCRC> ClaimCRCs { get; set; }
        public virtual DbSet<ClaimHI> ClaimHIs { get; set; }
        public virtual DbSet<ClaimK3> ClaimK3s { get; set; }
        public virtual DbSet<ClaimLineAuth> ClaimLineAuths { get; set; }
        public virtual DbSet<ClaimLineFRM> ClaimLineFRMs { get; set; }
        public virtual DbSet<ClaimLineLQ> ClaimLineLQs { get; set; }
        public virtual DbSet<ClaimLineMEA> ClaimLineMEAs { get; set; }
        public virtual DbSet<ClaimLineSVD> ClaimLineSVDs { get; set; }
        public virtual DbSet<ClaimNte> ClaimNtes { get; set; }
        public virtual DbSet<ClaimProvider> ClaimProviders { get; set; }
        public virtual DbSet<ClaimPWK> ClaimPWKs { get; set; }
        public virtual DbSet<ClaimSBR> ClaimSBRs { get; set; }
        public virtual DbSet<ClaimPatient> ClaimPatients { get; set; }
        public virtual DbSet<ClaimSecondaryIdentification> ClaimSecondaryIdentifications { get; set; }
        public virtual DbSet<ProviderContact> ProviderContacts { get; set; }

    }
}
