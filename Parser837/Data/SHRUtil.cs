using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser837
{
    public static class SHRUtil
    {
        public static void SaveClaims(ref List<Claim> claims)
        {
            using (var context = new SHRContext())
            {
                context.ClaimHeaders.AddRange(claims.Select(x => x.Header).ToList());
                context.ClaimCAS.AddRange(claims.SelectMany(x => x.Cases).ToList());
                context.ClaimCRCs.AddRange(claims.SelectMany(x => x.CRCs).ToList());
                context.ClaimHIs.AddRange(claims.SelectMany(x => x.His).ToList());
                context.ClaimK3s.AddRange(claims.SelectMany(x => x.K3s).ToList());
                context.ClaimLineFRMs.AddRange(claims.SelectMany(x => x.FRMs).ToList());
                context.ClaimLineLQs.AddRange(claims.SelectMany(x => x.LQs).ToList());
                context.ClaimLineMEAs.AddRange(claims.SelectMany(x => x.Meas).ToList());
                context.ClaimLineSVDs.AddRange(claims.SelectMany(x => x.SVDs).ToList());
                context.ClaimNtes.AddRange(claims.SelectMany(x => x.Notes).ToList());
                context.ClaimPatients.AddRange(claims.SelectMany(x => x.Patients).ToList());
                context.ClaimProviders.AddRange(claims.SelectMany(x => x.Providers).ToList());
                context.ClaimPWKs.AddRange(claims.SelectMany(x => x.PWKs).ToList());
                context.ClaimSBRs.AddRange(claims.SelectMany(x => x.Subscribers).ToList());
                context.ClaimSecondaryIdentifications.AddRange(claims.SelectMany(x => x.SecondaryIdentifications).ToList());
                context.ProviderContacts.AddRange(claims.SelectMany(x => x.ProviderContacts).ToList());
                context.ServiceLines.AddRange(claims.SelectMany(x => x.Lines).ToList());
                context.ToothStatus.AddRange(claims.SelectMany(x => x.ToothStatuses).ToList());
                context.SaveChanges();
            }
        }

        public static void SaveLines(ref List<ServiceLine> claimlines, ref List<ClaimLineSVD> claimlinesvds, ref List<ClaimCAS> claimcases)
        {
            using (var context = new SHRContext())
            {
                context.ServiceLines.AddRange(claimlines);
                context.ClaimLineSVDs.AddRange(claimlinesvds);
                context.ClaimCAS.AddRange(claimcases);
                context.SaveChanges();
            }
        }
    }
}