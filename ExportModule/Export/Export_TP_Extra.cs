using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.X12Segments;
using System.Configuration;
using System.Data.Entity;
using Parser837;
using System.IO;

namespace ExportModule
{
    public static class Export_TP_Extra
    {
        public static void Export_TP_Extra_I()
        {

            {
                string DestinationFolder = ConfigurationManager.AppSettings["DestinationFolder"];
                SHRContext context = new SHRContext();
                int pages = context.ClaimHeaders.Count() / 5000;
                StringBuilder sb = new StringBuilder();
                int SegmentCount = 0;
                int HLID = 0;
                int HL_Subscriber_Parent_ID = 0;
                int HL_Patient_Parent_ID = 0;
                for (int i = 0; i <= pages; i++)
                {
                    sb.Clear();
                    SegmentCount = 0;
                    HLID = 1;
                    context.Database.ExecuteSqlCommand("truncate table Sub_History.ClaimTempHost");
                    List<ClaimHeader> headers = context.ClaimHeaders.Where(x => x.ClaimType == "A").OrderBy(x => x.ID).Skip(i * 5000).Take(5000).ToList();
                    if (headers.Count > 0)
                    {
                        context.Database.ExecuteSqlCommand("truncate table Sub_History.ClaimTempHost");
                        List<ClaimTempHost> tempclaims = new List<ClaimTempHost>();
                        foreach (var header in headers) tempclaims.Add(new ClaimTempHost { ClaimID = header.ClaimID });
                        context.ClaimTempHost.AddRange(tempclaims.Distinct());
                        context.SaveChanges();
                        List<ClaimCAS> cases = context.Database.SqlQuery<ClaimCAS>("select * from Sub_History.ClaimCAS where ClaimID in (select ClaimID from Sub_history.ClaimTempHost)").ToList();
                        List<ClaimCRC> crcs = context.Database.SqlQuery<ClaimCRC>("select * from Sub_history.ClaimCRCs where ClaimID in (select ClaimID from Sub_History.ClaimTempHost)").ToList();
                        List<ClaimHI> his = context.Database.SqlQuery<ClaimHI>("select * from Sub_History.ClaimHIs where ClaimID in (select ClaimID from Sub_History.ClaimTempHost)").ToList();
                        List<ClaimK3> k3s = context.Database.SqlQuery<ClaimK3>("select * from Sub_History.ClaimK3s where ClaimID in (select ClaimID from Sub_History.ClaimTempHost)").ToList();
                        List<ClaimLineFRM> frms = context.Database.SqlQuery<ClaimLineFRM>("select * from Sub_History.ClaimLineFRMs where ClaimID in (select ClaimID from Sub_History.ClaimTempHost)").ToList();
                        List<ClaimLineLQ> lqs = context.Database.SqlQuery<ClaimLineLQ>("select * from Sub_History.ClaimLineLQs where ClaimID in (select ClaimID from Sub_History.ClaimTempHost)").ToList();
                        List<ClaimLineMEA> meas = context.Database.SqlQuery<ClaimLineMEA>("select * from Sub_History.ClaimLineMEAs where ClaimID in (select ClaimID from Sub_History.ClaimTempHost)").ToList();
                        List<ClaimLineSVD> svds = context.Database.SqlQuery<ClaimLineSVD>("select * from Sub_History.ClaimLineSVDs where ClaimID in (select ClaimID from Sub_History.ClaimTempHost)").ToList();
                        List<ClaimNte> notes = context.Database.SqlQuery<ClaimNte>("select * from Sub_History.ClaimNtes where ClaimID in (select ClaimID from Sub_History.ClaimTempHost)").ToList();
                        List<ClaimPatient> patients = context.Database.SqlQuery<ClaimPatient>("select * from Sub_History.ClaimPatients where ClaimID in (select ClaimID from Sub_History.ClaimTempHost)").ToList();
                        List<ClaimProvider> providers = context.Database.SqlQuery<ClaimProvider>("select * from Sub_History.ClaimProviders where ClaimID in (select ClaimID from Sub_History.ClaimTempHost)").ToList();
                        List<ClaimPWK> pwks = context.Database.SqlQuery<ClaimPWK>("select * from Sub_History.ClaimPWKs where ClaimID in (select ClaimID from Sub_History.ClaimTempHost)").ToList();
                        List<ClaimSBR> sbrs = context.Database.SqlQuery<ClaimSBR>("select * from Sub_History.ClaimSBRs where ClaimID in (select ClaimID from Sub_History.ClaimTempHost)").ToList();
                        List<ClaimSecondaryIdentification> secondaryidentifications = context.Database.SqlQuery<ClaimSecondaryIdentification>("select * from Sub_History.ClaimSecondaryIdentifications where ClaimID in (select ClaimID from Sub_History.ClaimTempHost)").ToList();
                        List<ProviderContact> providercontacts = context.Database.SqlQuery<ProviderContact>("select * from Sub_History.ProviderContacts where ClaimID in (select ClaimID from Sub_History.ClaimTempHost)").ToList();
                        List<ServiceLine> servicelines = context.Database.SqlQuery<ServiceLine>("select * from Sub_History.ServiceLines where ClaimID in (select ClaimID from Sub_History.ClaimTempHost)").ToList();

                        string transactionDate = DateTime.Today.ToString("yyyyMMdd");
                        string transactionTime = DateTime.Now.ToString("HHmm");
                        string icn = (DateTime.Today.DayOfYear + 100).ToString() + DateTime.Now.ToString("HHmmssfff").Substring(1, 6);
                        ISA isa = new ISA();
                        isa.InterchangeSenderID = "IMG";
                        isa.InterchangeReceiverID = "00303";
                        isa.InterchangeDate = transactionDate;
                        isa.InterchangeTime = transactionTime;
                        isa.InterchangeControlNumber = icn;
                        isa.ProductionFlag = "P";
                        sb.Append(isa.ToX12String());
                        GS gs = new GS();
                        gs.FunctionalIDCode = "HC";
                        gs.SenderID = "IMG";
                        gs.ReceiverID = "00303";
                        gs.TransactionDate = transactionDate;
                        gs.TransactrionTime = transactionTime;
                        gs.GroupControlNumber = "1";
                        gs.ResponsibleAgencyCode = "X";
                        gs.VersionID = "005010X223A2";
                        sb.Append(gs.ToX12String());
                        ST st = new ST();
                        st.TransactionControlNumber = "0000001";
                        st.VersionNumber = "005010X223A2";
                        sb.Append(st.ToX12String());
                        SegmentCount++;
                        BHT bht = new BHT();
                        bht.TransactionID = transactionDate + transactionTime;
                        bht.TransactionDate = transactionDate;
                        bht.TransactionTime = transactionTime;
                        bht.TransactionTypeCode = "CH";
                        sb.Append(bht.ToX12String());
                        SegmentCount++;
                        NM1 nm1 = new NM1();
                        nm1.NameQualifier = "41";
                        nm1.NameType = "2";
                        nm1.LastName = "IMAGENET";
                        nm1.IDQualifer = "46";
                        nm1.IDCode = "IMG";
                        sb.Append(nm1.ToX12String());
                        SegmentCount++;
                        PER per = new PER();
                        PERItem peritem = new PERItem();
                        peritem.ContactName = "Steve Strawn";
                        peritem.Phone = "2533950110";
                        per.Pers.Add(peritem);
                        sb.Append(per.ToX12String());
                        SegmentCount++;
                        nm1 = new NM1();
                        nm1.NameQualifier = "40";
                        nm1.NameType = "2";
                        nm1.LastName = "IEHP";
                        nm1.IDQualifer = "46";
                        nm1.IDCode = "00303";
                        sb.Append(nm1.ToX12String());
                        SegmentCount++;

                        foreach (ClaimHeader header in headers)
                        {
                            List<ClaimCAS> claimcases = cases.Where(x => x.ClaimID == header.ClaimID).ToList();
                            List<ClaimCRC> claimcrcs = crcs.Where(x => x.ClaimID == header.ClaimID).ToList();
                            List<ClaimHI> claimhis = his.Where(x => x.ClaimID == header.ClaimID).ToList();
                            List<ClaimK3> claimk3s = k3s.Where(x => x.ClaimID == header.ClaimID).ToList();
                            List<ClaimLineFRM> claimfrms = frms.Where(x => x.ClaimID == header.ClaimID).ToList();
                            List<ClaimLineLQ> claimlqs = lqs.Where(x => x.ClaimID == header.ClaimID).ToList();
                            List<ClaimLineMEA> claimmeas = meas.Where(x => x.ClaimID == header.ClaimID).ToList();
                            List<ClaimLineSVD> claimsvds = svds.Where(x => x.ClaimID == header.ClaimID).ToList();
                            List<ClaimNte> claimnotes = notes.Where(x => x.ClaimID == header.ClaimID).ToList();
                            List<ClaimPatient> claimpatients = patients.Where(x => x.ClaimID == header.ClaimID).ToList();
                            List<ClaimProvider> claimproviders = providers.Where(x => x.ClaimID == header.ClaimID).ToList();
                            List<ClaimPWK> claimpwks = pwks.Where(x => x.ClaimID == header.ClaimID).ToList();
                            List<ClaimSBR> claimsbrs = sbrs.Where(x => x.ClaimID == header.ClaimID).ToList();
                            List<ClaimSecondaryIdentification> claimsis = secondaryidentifications.Where(x => x.ClaimID == header.ClaimID).ToList();
                            List<ProviderContact> claimpcs = providercontacts.Where(x => x.ClaimID == header.ClaimID).ToList();
                            List<ServiceLine> claimlines = servicelines.Where(x => x.ClaimID == header.ClaimID).ToList();

                            ClaimProvider claimprovider = claimproviders.Where(x => x.LoopName == "2000A").FirstOrDefault();

                            HL hl = new HL();
                            hl.LoopName = "2000A";
                            hl.HLID = HLID.ToString();
                            HL_Subscriber_Parent_ID = HLID;
                            HLID++;
                            hl.LevelCode = "20";
                            hl.ChildCode = "1";
                            sb.Append(hl.ToX12String());
                            SegmentCount++;
                            if (!string.IsNullOrEmpty(claimprovider.ProviderTaxonomyCode))
                            {
                                PRV prv = new PRV();
                                prv.ProviderQualifier = "BI";
                                prv.ProviderTaxonomyCode = claimprovider.ProviderTaxonomyCode;
                                sb.Append(prv.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(claimprovider.ProviderCurrencyCode))
                            {
                                CUR cur = new CUR();
                                cur.ProviderCurrencyCode = claimprovider.ProviderCurrencyCode;
                                sb.Append(cur.ToX12String());
                                SegmentCount++;
                            }
                            nm1 = new NM1();
                            nm1.NameQualifier = "85";
                            nm1.NameType = "2";
                            nm1.LastName = claimprovider.ProviderLastName;
                            nm1.IDQualifer = claimprovider.ProviderIDQualifier;
                            nm1.IDCode = claimprovider.ProviderID;
                            sb.Append(nm1.ToX12String());
                            SegmentCount++;
                            N3 n3 = new N3();
                            n3.Address = claimprovider.ProviderAddress;
                            n3.Address2 = claimprovider.ProviderAddress2;
                            sb.Append(n3.ToX12String());
                            SegmentCount++;
                            N4 n4 = new N4();
                            n4.City = claimprovider.ProviderCity;
                            n4.State = claimprovider.ProviderState;
                            n4.Zipcode = claimprovider.ProviderZip;
                            n4.Country = claimprovider.ProviderCountry;
                            n4.CountrySubCode = claimprovider.ProviderCountrySubCode;
                            sb.Append(n4.ToX12String());
                            SegmentCount++;
                            ClaimSecondaryIdentification claimsi = claimsis.Where(x => x.LoopName == "2010AA" && x.ProviderQualifier == "EI").FirstOrDefault();
                            REF rref = new REF();
                            REFItem refitem = new REFItem();
                            refitem.ProviderQualifier = claimsi.ProviderQualifier;
                            refitem.ProviderID = claimsi.ProviderID;
                            rref.Refs.Add(refitem);
                            foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2010AA" && x.ProviderQualifier != "EI"))
                            {
                                refitem = new REFItem();
                                refitem.ProviderQualifier = si.ProviderQualifier;
                                refitem.ProviderID = si.ProviderID;
                                rref.Refs.Add(refitem);
                            }
                            sb.Append(rref.ToX12String());
                            SegmentCount += rref.Refs.Count;
                            if (claimpcs.Where(x => x.LoopName == "2010AA").Count() > 0)
                            {
                                per = new PER();
                                foreach (ProviderContact pc in claimpcs.Where(x => x.LoopName == "2010AA"))
                                {
                                    peritem = new PERItem();
                                    peritem.ContactName = pc.ContactName;
                                    peritem.Phone = pc.Phone;
                                    peritem.Email = pc.Email;
                                    peritem.Fax = pc.Fax;
                                    peritem.PhoneEx = pc.PhoneEx;
                                    per.Pers.Add(peritem);
                                }
                                sb.Append(per.ToX12String());
                                SegmentCount += per.Pers.Count;
                            }
                            claimprovider = claimproviders.Where(x => x.LoopName == "2010AB").FirstOrDefault();
                            if (claimprovider != null)
                            {
                                nm1 = new NM1();
                                nm1.NameQualifier = claimprovider.ProviderQualifier;
                                nm1.NameType = "2";
                                sb.Append(nm1.ToX12String());
                                SegmentCount++;
                                n3 = new N3();
                                n3.Address = claimprovider.ProviderAddress;
                                n3.Address2 = claimprovider.ProviderAddress2;
                                sb.Append(n3.ToX12String());
                                SegmentCount++;
                                n4 = new N4();
                                n4.City = claimprovider.ProviderCity;
                                n4.State = claimprovider.ProviderState;
                                n4.Zipcode = claimprovider.ProviderZip;
                                n4.Country = claimprovider.ProviderCountry;
                                n4.CountrySubCode = claimprovider.ProviderCountrySubCode;
                                sb.Append(n4.ToX12String());
                                SegmentCount++;
                            }
                            claimprovider = claimproviders.Where(x => x.LoopName == "2010AC").FirstOrDefault();
                            if (claimprovider != null)
                            {
                                nm1 = new NM1();
                                nm1.NameQualifier = claimprovider.ProviderIDQualifier;
                                nm1.NameType = "2";
                                nm1.LastName = claimprovider.ProviderLastName;
                                nm1.IDQualifer = claimprovider.ProviderIDQualifier;
                                nm1.IDCode = claimprovider.ProviderID;
                                sb.Append(nm1.ToX12String());
                                SegmentCount++;
                                n3 = new N3();
                                n3.Address = claimprovider.ProviderAddress;
                                n3.Address2 = claimprovider.ProviderAddress2;
                                sb.Append(n3.ToX12String());
                                SegmentCount++;
                                n4 = new N4();
                                n4.City = claimprovider.ProviderCity;
                                n4.State = claimprovider.ProviderState;
                                n4.Zipcode = claimprovider.ProviderZip;
                                n4.Country = claimprovider.ProviderCountry;
                                n4.CountrySubCode = claimprovider.ProviderCountrySubCode;
                                sb.Append(n4.ToX12String());
                                SegmentCount++;
                                claimsi = claimsis.Where(x => x.LoopName == "2010AC" && x.ProviderQualifier == "EI").FirstOrDefault();
                                rref = new REF();
                                refitem = new REFItem();
                                refitem.ProviderQualifier = claimsi.ProviderQualifier;
                                refitem.ProviderID = claimsi.ProviderID;
                                rref.Refs.Add(refitem);
                                foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2010AC" && x.ProviderQualifier != "EI"))
                                {
                                    refitem = new REFItem();
                                    refitem.ProviderQualifier = si.ProviderQualifier;
                                    refitem.ProviderID = si.ProviderID;
                                    rref.Refs.Add(refitem);
                                }
                                sb.Append(rref.ToX12String());
                                SegmentCount += rref.Refs.Count;
                            }
                            ClaimSBR claimsbr = claimsbrs.Where(x => x.LoopName == "2000B").FirstOrDefault();
                            hl = new HL();
                            hl.LoopName = "2000B";
                            hl.HLID = HLID.ToString();
                            HL_Patient_Parent_ID = HLID;
                            HLID++;
                            hl.ParentID = HL_Subscriber_Parent_ID.ToString();
                            hl.LevelCode = "22";
                            hl.ChildCode = claimpatients.Count > 0 ? "1" : "0";
                            sb.Append(hl.ToX12String());
                            SegmentCount++;
                            SBR sbr = new SBR();
                            sbr.SubscriberSequenceNumber = claimsbr.SubscriberSequenceNumber;
                            sbr.SubscriberRelationshipCode = claimsbr.SubscriberRelationshipCode;
                            sbr.InsuredGroupNumber = claimsbr.InsuredGroupNumber;
                            sbr.OtherInsuredGroupName = claimsbr.OtherInsuredGroupName;
                            sbr.ClaimFilingCode = claimsbr.ClaimFilingCode;
                            sb.Append(sbr.ToX12String());
                            SegmentCount++;
                            nm1 = new NM1();
                            nm1.NameQualifier = "IL";
                            nm1.NameType = string.IsNullOrEmpty(claimsbr.FirstName) ? "2" : "1";
                            nm1.LastName = claimsbr.LastName;
                            nm1.FirstName = claimsbr.FirstName;
                            nm1.MiddleName = claimsbr.MidddleName;
                            nm1.Suffix = claimsbr.NameSuffix;
                            nm1.IDQualifer = claimsbr.IDQualifier;
                            nm1.IDCode = claimsbr.IDCode;
                            sb.Append(nm1.ToX12String());
                            SegmentCount++;
                            n3 = new N3();
                            n3.Address = claimsbr.SubscriberAddress;
                            n3.Address2 = claimsbr.SubscriberAddress2;
                            sb.Append(n3.ToX12String());
                            SegmentCount++;
                            n4 = new N4();
                            n4.City = claimsbr.SubscriberCity;
                            n4.State = claimsbr.SubscriberState;
                            n4.Zipcode = claimsbr.SubscriberZip;
                            n4.Country = claimsbr.SubscriberCountry;
                            n4.CountrySubCode = claimsbr.SubscriberCountrySubCode;
                            sb.Append(n4.ToX12String());
                            SegmentCount++;
                            if (!string.IsNullOrEmpty(claimsbr.SubscriberBirthDate) && !string.IsNullOrEmpty(claimsbr.SubscriberGender))
                            {
                                DMG dmg = new DMG();
                                dmg.BirthDate = claimsbr.SubscriberBirthDate;
                                dmg.Gender = claimsbr.SubscriberGender;
                                sb.Append(dmg.ToX12String());
                                SegmentCount++;
                            }
                            claimsi = claimsis.Where(x => x.LoopName == "2010BA" && x.ProviderQualifier == "SY").FirstOrDefault();
                            if (claimsi != null)
                            {
                                rref = new REF();
                                refitem = new REFItem();
                                refitem.ProviderQualifier = claimsi.ProviderQualifier;
                                refitem.ProviderID = claimsi.ProviderID;
                                rref.Refs.Add(refitem);
                                sb.Append(rref.ToX12String());
                                SegmentCount += rref.Refs.Count;
                            }
                            claimsi = claimsis.Where(x => x.LoopName == "2010BA" && x.ProviderQualifier == "Y4").FirstOrDefault();
                            if (claimsi != null)
                            {
                                rref = new REF();
                                refitem = new REFItem();
                                refitem.ProviderQualifier = claimsi.ProviderQualifier;
                                refitem.ProviderID = claimsi.ProviderID;
                                rref.Refs.Add(refitem);
                                sb.Append(rref.ToX12String());
                                SegmentCount += rref.Refs.Count;
                                ProviderContact claimpc = claimpcs.Where(x => x.LoopName == "2010BA").FirstOrDefault();
                                if (claimpc != null)
                                {
                                    peritem = new PERItem();
                                    peritem.ContactName = claimpc.ContactName;
                                    peritem.Email = claimpc.Email;
                                    peritem.Fax = claimpc.Fax;
                                    peritem.Phone = claimpc.Phone;
                                    peritem.PhoneEx = claimpc.PhoneEx;
                                    per = new PER();
                                    per.Pers.Add(peritem);
                                    sb.Append(per.ToX12String());
                                    SegmentCount++;
                                }
                            }
                            claimprovider = claimproviders.Where(x => x.LoopName == "2010BB").FirstOrDefault();
                            nm1 = new NM1();
                            nm1.NameQualifier = claimprovider.ProviderQualifier;
                            nm1.NameType = "2";
                            nm1.LastName = claimprovider.ProviderLastName;
                            nm1.IDQualifer = claimprovider.ProviderIDQualifier;
                            nm1.IDCode = claimprovider.ProviderID;
                            sb.Append(nm1.ToX12String());
                            SegmentCount++;
                            if (!string.IsNullOrEmpty(claimprovider.ProviderAddress))
                            {
                                n3 = new N3();
                                n3.Address = claimprovider.ProviderAddress;
                                n3.Address2 = claimprovider.ProviderAddress2;
                                sb.Append(n3.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(claimprovider.ProviderCity))
                            {
                                n4 = new N4();
                                n4.City = claimprovider.ProviderCity;
                                n4.State = claimprovider.ProviderState;
                                n4.Zipcode = claimprovider.ProviderZip;
                                n4.Country = claimprovider.ProviderCountry;
                                n4.CountrySubCode = claimprovider.ProviderCountrySubCode;
                                sb.Append(n4.ToX12String());
                                SegmentCount++;
                            }
                            if (claimsis.Where(x => x.LoopName == "2010BB").Count() > 0)
                            {
                                rref = new REF();
                                foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2010BB"))
                                {
                                    refitem = new REFItem();
                                    refitem.ProviderQualifier = si.ProviderQualifier;
                                    refitem.ProviderID = si.ProviderID;
                                    rref.Refs.Add(refitem);
                                }
                                sb.Append(rref.ToX12String());
                                SegmentCount += rref.Refs.Count;
                            }

                            if (claimpatients.Count > 0)
                            {
                                hl = new HL();
                                hl.HLID = HLID.ToString();
                                HLID++;
                                hl.ParentID = HL_Patient_Parent_ID.ToString();
                                hl.LevelCode = "23";
                                hl.ChildCode = "0";
                                sb.Append(hl.ToX12String());
                                SegmentCount++;
                                PAT pat = new PAT();
                                pat.PatientRelatedCode = patients.FirstOrDefault().PatientRelatedCode;
                                pat.PatientRelatedDeathDate = patients.FirstOrDefault().PatientRelatedDeathDate;
                                pat.PatientRelatedUnit = patients.FirstOrDefault().PatientRelatedUnit;
                                pat.PatientRelatedWeight = patients.FirstOrDefault().PatientRelatedWeight;
                                pat.PatientRelatedPregnancyIndicator = patients.FirstOrDefault().PatientRelatedPregnancyIndicator;
                                sb.Append(pat.ToX12String());
                                SegmentCount++;
                                nm1 = new NM1();
                                nm1.NameQualifier = "QC";
                                nm1.NameType = string.IsNullOrEmpty(patients.FirstOrDefault().PatientFirstName) ? "2" : "1";
                                nm1.LastName = patients.FirstOrDefault().PatientLastName;
                                nm1.FirstName = patients.FirstOrDefault().PatientFirstName;
                                nm1.MiddleName = patients.FirstOrDefault().PatientMiddle;
                                nm1.Suffix = patients.FirstOrDefault().PatientSuffix;
                                sb.Append(nm1.ToX12String());
                                SegmentCount++;
                                n3 = new N3();
                                n3.Address = patients.FirstOrDefault().PatientAddress;
                                n3.Address2 = patients.FirstOrDefault().PatientAddress2;
                                sb.Append(n3.ToString());
                                SegmentCount++;
                                n4 = new N4();
                                n4.City = patients.FirstOrDefault().PatientCity;
                                n4.State = patients.FirstOrDefault().PatientState;
                                n4.Zipcode = patients.FirstOrDefault().PatientZip;
                                n4.Country = patients.FirstOrDefault().PatientCountry;
                                n4.CountrySubCode = patients.FirstOrDefault().PatientCountrySubCode;
                                sb.Append(n4.ToX12String());
                                SegmentCount++;
                                DMG dmg = new DMG();
                                dmg.BirthDate = patients.FirstOrDefault().PatientBirthDate;
                                dmg.Gender = patients.FirstOrDefault().PatientGender;
                                sb.Append(dmg.ToX12String());
                                SegmentCount++;
                                claimsi = claimsis.Where(x => x.LoopName == "2010CA" && x.ProviderQualifier == "Y4").FirstOrDefault();
                                if (claimsi != null)
                                {
                                    rref = new REF();
                                    refitem = new REFItem();
                                    refitem.ProviderQualifier = claimsi.ProviderQualifier;
                                    refitem.ProviderID = claimsi.ProviderID;
                                    rref.Refs.Add(refitem);
                                    sb.Append(rref.ToX12String());
                                    SegmentCount += rref.Refs.Count;
                                }
                                claimsi = claimsis.Where(x => x.LoopName == "2010CA" && (x.ProviderQualifier == "1W" || x.ProviderQualifier == "SY")).FirstOrDefault();
                                if (claimsi != null)
                                {
                                    rref = new REF();
                                    refitem = new REFItem();
                                    refitem.ProviderQualifier = claimsi.ProviderQualifier;
                                    refitem.ProviderID = claimsi.ProviderID;
                                    rref.Refs.Add(refitem);
                                    sb.Append(rref.ToX12String());
                                    SegmentCount += rref.Refs.Count;
                                }
                                ProviderContact claimpc = claimpcs.Where(x => x.LoopName == "2010CA").FirstOrDefault();
                                if (claimpc != null)
                                {
                                    peritem = new PERItem();
                                    peritem.ContactName = claimpc.ContactName;
                                    peritem.Email = claimpc.Email;
                                    peritem.Fax = claimpc.Fax;
                                    peritem.Phone = claimpc.Phone;
                                    peritem.PhoneEx = claimpc.PhoneEx;
                                    per = new PER();
                                    per.Pers.Add(peritem);
                                    sb.Append(per.ToX12String());
                                    SegmentCount++;
                                }
                            }
                            CLM_I clm = new CLM_I();
                            clm.ClaimID = header.ClaimID;
                            clm.ClaimAmount = header.ClaimAmount;
                            clm.ClaimPOS = header.ClaimPOS;
                            clm.ClaimType = header.ClaimType;
                            clm.ClaimFrequencyCode = header.ClaimFrequencyCode;
                            clm.ClaimProviderAssignment = header.ClaimProviderAssignment;
                            clm.ClaimBenefitAssignment = header.ClaimBenefitAssignment;
                            clm.ClaimReleaseofInformationCode = header.ClaimReleaseofInformationCode;
                            clm.ClaimDelayReasonCode = header.ClaimDelayReasonCode;
                            sb.Append(clm.ToX12String());
                            SegmentCount++;
                            if (!string.IsNullOrEmpty(header.DischargeDate))
                            {
                                DTP dtp = new DTP();
                                dtp.DateCode = "096";
                                dtp.DateQualifier = "TM";
                                dtp.StartDate = header.DischargeDate;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(header.StatementFromDate) && !string.IsNullOrEmpty(header.StatementToDate))
                            {
                                DTP dtp = new DTP();
                                dtp.DateCode = "434";
                                dtp.DateQualifier = "RD8";
                                dtp.StartDate = header.StatementFromDate;
                                dtp.EndDate = header.StatementToDate;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(header.AdmissionDate))
                            {
                                DTP dtp = new DTP();
                                dtp.DateCode = "435";
                                dtp.DateQualifier = header.AdmissionDate.Length == 8 ? "D8" : "DT";
                                dtp.StartDate = header.AdmissionDate;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(header.RepricerReceivedDate))
                            {
                                DTP dtp = new DTP();
                                dtp.DateCode = "050";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = header.RepricerReceivedDate;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(header.AdmissionTypeCode) && !string.IsNullOrEmpty(header.PatientStatusCode))
                            {
                                CL1 cl1 = new CL1();
                                cl1.AdmissionTypeCode = header.AdmissionTypeCode;
                                cl1.AdmissionSourceCode = header.AdmissionSourceCode;
                                cl1.PatientStatusCode = header.PatientStatusCode;
                                sb.Append(cl1.ToX12String());
                                SegmentCount++;
                            }
                            if (claimpwks.Count > 0)
                            {
                                PWK pwk = new PWK();
                                foreach (ClaimPWK claimpwk in claimpwks)
                                {
                                    PWKItem pwkitem = new PWKItem();
                                    pwkitem.ReportTypeCode = claimpwk.ReportTypeCode;
                                    pwkitem.ReportTransmissionCode = claimpwk.ReportTransmissionCode;
                                    pwkitem.AttachmentControlNumber = claimpwk.AttachmentControlNumber;
                                    pwk.Pwks.Add(pwkitem);
                                }
                                sb.Append(pwk.ToX12String());
                                SegmentCount += pwk.Pwks.Count;
                            }
                            if (!string.IsNullOrEmpty(header.ContractTypeCode))
                            {
                                CN1 cn1 = new CN1();
                                cn1.ContractTypeCode = header.ContractTypeCode;
                                cn1.ContractAmount = header.ContractAmount;
                                cn1.ContractPercentage = header.ContractPercentage;
                                cn1.ContractCode = header.ContractCode;
                                cn1.ContractTermsDiscountPercentage = header.ContractTermsDiscountPercentage;
                                cn1.ContractVersionIdentifier = header.ContractVersionIdentifier;
                                sb.Append(cn1.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(header.PatientPaidAmount))
                            {
                                AMT amt = new AMT();
                                amt.AmountQualifier = "F5";
                                amt.Amount = header.PatientPaidAmount;
                                sb.Append(amt.ToX12String());
                                SegmentCount++;
                            }
                            if (claimsis.Where(x => x.LoopName == "2300").Count() > 0)
                            {
                                rref = new REF();
                                foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2300"))
                                {
                                    refitem = new REFItem();
                                    refitem.ProviderQualifier = si.ProviderQualifier;
                                    refitem.ProviderID = si.ProviderID;
                                    rref.Refs.Add(refitem);
                                }
                                sb.Append(rref.ToX12String());
                                SegmentCount += rref.Refs.Count;
                            }
                            if (claimk3s.Count > 0)
                            {
                                K3 k3 = new K3();
                                foreach (ClaimK3 claimk3 in claimk3s)
                                {
                                    k3.K3s.Add(claimk3.K3);
                                }
                                sb.Append(k3.ToX12String());
                                SegmentCount += k3.K3s.Count;
                            }
                            if (claimnotes.Where(x => x.ServiceLineNumber == "0").Count() > 0)
                            {
                                foreach (ClaimNte claimnote in claimnotes.Where(x => x.ServiceLineNumber == "0"))
                                {
                                    NTE nte = new NTE();
                                    nte.NoteCode = claimnote.NoteCode;
                                    nte.NoteText = claimnote.NoteText;
                                    sb.Append(nte.ToX12String());
                                    SegmentCount++;
                                }
                            }
                            if (claimcrcs.Where(x => x.ServiceLineNumber == "0").Count() > 0)
                            {
                                foreach (ClaimCRC claimcrc in claimcrcs.Where(x => x.ServiceLineNumber == "0"))
                                {
                                    CRC crc = new CRC();
                                    crc.CodeCategory = claimcrc.CodeCategory;
                                    crc.ConditionIndicator = claimcrc.ConditionIndicator;
                                    crc.ConditionCode = claimcrc.ConditionCode;
                                    crc.ConditionCode2 = claimcrc.ConditionCode2;
                                    crc.ConditionCode3 = claimcrc.ConditionCode3;
                                    crc.ConditionCode4 = claimcrc.ConditionCode4;
                                    crc.ConditionCode5 = claimcrc.ConditionCode5;
                                    sb.Append(crc.ToX12String());
                                    SegmentCount++;
                                }
                            }
                            HI_I hii = new HI_I();
                            foreach (ClaimHI claimhi in claimhis)
                            {
                                HIItem hiitem = new HIItem();
                                hiitem.HIQual = claimhi.HIQual;
                                hiitem.HICode = claimhi.HICode;
                                hiitem.PresentOnAdmissionIndicator = claimhi.PresentOnAdmissionIndicator;
                                hiitem.HIFromDate = claimhi.HIFromDate;
                                hiitem.HIToDate = claimhi.HIToDate;
                                hiitem.HIAmount = claimhi.HIAmount;
                                hii.His.Add(hiitem);
                            }
                            sb.Append(hii.ToX12String());
                            SegmentCount += hii.HiCounts;
                            if (!string.IsNullOrEmpty(header.PricingMethodology) && !string.IsNullOrEmpty(header.RepricedAllowedAmount))
                            {
                                HCP hcp = new HCP();
                                hcp.PricingMethodology = header.PricingMethodology;
                                hcp.RepricedAllowedAmount = header.RepricedAllowedAmount;
                                hcp.RepricedSavingAmount = header.RepricedSavingAmount;
                                hcp.RepricingOrganizationID = header.RepricingOrganizationID;
                                hcp.RepricingRate = header.RepricingRate;
                                hcp.RepricedGroupCode = header.RepricedGroupCode;
                                hcp.RepricedGroupAmount = header.RepricedGroupAmount;
                                hcp.RejectReasonCode = header.RejectReasonCode;
                                hcp.PolicyComplianceCode = header.PolicyComplianceCode;
                                hcp.ExceptionCode = header.ExceptionCode;
                                sb.Append(hcp.ToX12String());
                                SegmentCount++;
                            }
                            claimprovider = claimproviders.Where(x => x.LoopName == "2310A").FirstOrDefault();
                            if (claimprovider != null)
                            {
                                nm1 = new NM1();
                                nm1.NameQualifier = claimprovider.ProviderQualifier;
                                nm1.NameType = "1";
                                nm1.LastName = claimprovider.ProviderLastName;
                                nm1.FirstName = claimprovider.ProviderFirstName;
                                nm1.MiddleName = claimprovider.ProviderMiddle;
                                nm1.Suffix = claimprovider.ProviderSuffix;
                                nm1.IDQualifer = claimprovider.ProviderIDQualifier;
                                nm1.IDCode = claimprovider.ProviderID;
                                sb.Append(nm1.ToX12String());
                                SegmentCount++;
                                if (!string.IsNullOrEmpty(claimprovider.ProviderTaxonomyCode))
                                {
                                    PRV prv = new PRV();
                                    prv.ProviderQualifier = "AT";
                                    prv.ProviderTaxonomyCode = claimprovider.ProviderTaxonomyCode;
                                    sb.Append(prv.ToX12String());
                                    SegmentCount++;
                                }
                                if (claimsis.Where(x => x.LoopName == "2310A").Count() > 0)
                                {
                                    rref = new REF();
                                    foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2310A"))
                                    {
                                        refitem = new REFItem();
                                        refitem.ProviderQualifier = si.ProviderQualifier;
                                        refitem.ProviderID = si.ProviderID;
                                        rref.Refs.Add(refitem);
                                    }
                                    sb.Append(rref.ToX12String());
                                    SegmentCount += rref.Refs.Count;
                                }
                            }
                            claimprovider = claimproviders.Where(x => x.LoopName == "2310B").FirstOrDefault();
                            if (claimprovider != null)
                            {
                                nm1 = new NM1();
                                nm1.NameQualifier = claimprovider.ProviderQualifier;
                                nm1.NameType = "1";
                                nm1.LastName = claimprovider.ProviderLastName;
                                nm1.FirstName = claimprovider.ProviderFirstName;
                                nm1.MiddleName = claimprovider.ProviderMiddle;
                                nm1.Suffix = claimprovider.ProviderSuffix;
                                nm1.IDQualifer = claimprovider.ProviderIDQualifier;
                                nm1.IDCode = claimprovider.ProviderID;
                                sb.Append(nm1.ToX12String());
                                SegmentCount++;
                                if (claimsis.Where(x => x.LoopName == "2310B").Count() > 0)
                                {
                                    rref = new REF();
                                    foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2310B"))
                                    {
                                        refitem = new REFItem();
                                        refitem.ProviderQualifier = si.ProviderQualifier;
                                        refitem.ProviderID = si.ProviderID;
                                        rref.Refs.Add(refitem);
                                    }
                                    sb.Append(rref.ToX12String());
                                    SegmentCount += rref.Refs.Count;
                                }
                            }
                            claimprovider = claimproviders.Where(x => x.LoopName == "2310C").FirstOrDefault();
                            if (claimprovider != null)
                            {
                                nm1 = new NM1();
                                nm1.NameQualifier = claimprovider.ProviderQualifier;
                                nm1.NameType = "1";
                                nm1.LastName = claimprovider.ProviderLastName;
                                nm1.FirstName = claimprovider.ProviderFirstName;
                                nm1.MiddleName = claimprovider.ProviderMiddle;
                                nm1.Suffix = claimprovider.ProviderSuffix;
                                nm1.IDQualifer = claimprovider.ProviderIDQualifier;
                                nm1.IDCode = claimprovider.ProviderID;
                                sb.Append(nm1.ToX12String());
                                SegmentCount++;
                                if (claimsis.Where(x => x.LoopName == "2310C").Count() > 0)
                                {
                                    rref = new REF();
                                    foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2310C"))
                                    {
                                        refitem = new REFItem();
                                        refitem.ProviderQualifier = si.ProviderQualifier;
                                        refitem.ProviderID = si.ProviderID;
                                        rref.Refs.Add(refitem);
                                    }
                                    sb.Append(rref.ToX12String());
                                    SegmentCount += rref.Refs.Count;
                                }
                            }
                            claimprovider = claimproviders.Where(x => x.LoopName == "2310D").FirstOrDefault();
                            if (claimprovider != null)
                            {
                                nm1 = new NM1();
                                nm1.NameQualifier = claimprovider.ProviderQualifier;
                                nm1.NameType = string.IsNullOrEmpty(claimprovider.ProviderFirstName) ? "2" : "1";
                                nm1.LastName = claimprovider.ProviderLastName;
                                nm1.FirstName = claimprovider.ProviderFirstName;
                                nm1.MiddleName = claimprovider.ProviderMiddle;
                                nm1.Suffix = claimprovider.ProviderSuffix;
                                nm1.IDQualifer = claimprovider.ProviderIDQualifier;
                                nm1.IDCode = claimprovider.ProviderID;
                                sb.Append(nm1.ToX12String());
                                SegmentCount++;
                                if (claimsis.Where(x => x.LoopName == "2310D").Count() > 0)
                                {
                                    rref = new REF();
                                    foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2310D"))
                                    {
                                        refitem = new REFItem();
                                        refitem.ProviderQualifier = si.ProviderQualifier;
                                        refitem.ProviderID = si.ProviderID;
                                        rref.Refs.Add(refitem);
                                    }
                                    sb.Append(rref.ToX12String());
                                    SegmentCount += rref.Refs.Count;
                                }
                            }
                            claimprovider = claimproviders.Where(x => x.LoopName == "2310E").FirstOrDefault();
                            if (claimprovider != null)
                            {
                                nm1 = new NM1();
                                nm1.NameQualifier = claimprovider.ProviderQualifier;
                                nm1.NameType = "2";
                                nm1.LastName = claimprovider.ProviderLastName;
                                nm1.IDQualifer = claimprovider.ProviderIDQualifier;
                                nm1.IDCode = claimprovider.ProviderID;
                                sb.Append(nm1.ToX12String());
                                SegmentCount++;
                                n3 = new N3();
                                n3.Address = claimprovider.ProviderAddress;
                                n3.Address2 = claimprovider.ProviderAddress2;
                                sb.Append(n3.ToX12String());
                                SegmentCount++;
                                n4 = new N4();
                                n4.City = claimprovider.ProviderCity;
                                n4.State = claimprovider.ProviderState;
                                n4.Zipcode = claimprovider.ProviderZip;
                                n4.Country = claimprovider.ProviderCountry;
                                n4.CountrySubCode = claimprovider.ProviderCountrySubCode;
                                sb.Append(n4.ToX12String());
                                SegmentCount++;
                                if (claimsis.Where(x => x.LoopName == "2310E").Count() > 0)
                                {
                                    rref = new REF();
                                    foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2310E"))
                                    {
                                        refitem = new REFItem();
                                        refitem.ProviderQualifier = si.ProviderQualifier;
                                        refitem.ProviderID = si.ProviderID;
                                        rref.Refs.Add(refitem);
                                    }
                                    sb.Append(rref.ToX12String());
                                    SegmentCount += rref.Refs.Count;
                                }
                            }
                            claimprovider = claimproviders.Where(x => x.LoopName == "2310F").FirstOrDefault();
                            if (claimprovider != null)
                            {
                                nm1 = new NM1();
                                nm1.NameQualifier = claimprovider.ProviderQualifier;
                                nm1.NameType = string.IsNullOrEmpty(claimprovider.ProviderFirstName) ? "2" : "1";
                                nm1.LastName = claimprovider.ProviderLastName;
                                nm1.FirstName = claimprovider.ProviderFirstName;
                                nm1.MiddleName = claimprovider.ProviderMiddle;
                                nm1.Suffix = claimprovider.ProviderSuffix;
                                nm1.IDQualifer = claimprovider.ProviderIDQualifier;
                                nm1.IDCode = claimprovider.ProviderID;
                                sb.Append(nm1.ToX12String());
                                SegmentCount++;
                                if (claimsis.Where(x => x.LoopName == "2310F").Count() > 0)
                                {
                                    rref = new REF();
                                    foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2310F"))
                                    {
                                        refitem = new REFItem();
                                        refitem.ProviderQualifier = si.ProviderQualifier;
                                        refitem.ProviderID = si.ProviderID;
                                        rref.Refs.Add(refitem);
                                    }
                                    sb.Append(rref.ToX12String());
                                    SegmentCount += rref.Refs.Count;
                                }
                            }
                            if (claimsbrs.Where(x => x.LoopName == "2320").Count() > 0)
                            {
                                foreach (ClaimSBR sbr1 in claimsbrs.Where(x => x.LoopName == "2320"))
                                {
                                    sbr = new SBR();
                                    sbr.SubscriberSequenceNumber = sbr1.SubscriberSequenceNumber;
                                    sbr.SubscriberRelationshipCode = sbr1.SubscriberRelationshipCode;
                                    sbr.InsuredGroupNumber = sbr1.InsuredGroupNumber;
                                    sbr.OtherInsuredGroupName = sbr1.OtherInsuredGroupName;
                                    sbr.InsuredTypeCode = sbr1.InsuredTypeCode;
                                    sbr.ClaimFilingCode = sbr1.ClaimFilingCode;
                                    sb.Append(sbr.ToX12String());
                                    SegmentCount++;
                                    List<ClaimCAS> loopcases = claimcases.Where(x => x.SubscriberSequenceNumber == sbr1.SubscriberSequenceNumber && x.ServiceLineNumber == "0").ToList();
                                    if (loopcases.Count > 0)
                                    {
                                        int caspages = (int)(Math.Ceiling((decimal)loopcases.Count / 6));
                                        for (int icas = 0; icas < caspages; icas++)
                                        {
                                            CAS cas = new CAS();
                                            cas.AdjGroupCode = loopcases[icas * 6].GroupCode;
                                            cas.AdjReasonCode1 = loopcases[icas * 6].ReasonCode;
                                            cas.AdjAmount1 = loopcases[icas * 6].AdjustmentAmount;
                                            cas.AdjQuantity1 = loopcases[icas * 6].AdjustmentQuantity;
                                            if (loopcases.Count > icas * 6 + 1)
                                            {
                                                cas.AdjReasonCode2 = loopcases[icas * 6 + 1].ReasonCode;
                                                cas.AdjAmount2 = loopcases[icas * 6 + 1].AdjustmentAmount;
                                                cas.AdjQuantity2 = loopcases[icas * 6 + 1].AdjustmentQuantity;
                                            }
                                            if (loopcases.Count > icas * 6 + 2)
                                            {
                                                cas.AdjReasonCode3 = loopcases[icas * 6 + 2].ReasonCode;
                                                cas.AdjAmount3 = loopcases[icas * 6 + 2].AdjustmentAmount;
                                                cas.AdjQuantity3 = loopcases[icas * 6 + 2].AdjustmentQuantity;
                                            }
                                            if (loopcases.Count > icas * 6 + 3)
                                            {
                                                cas.AdjReasonCode4 = loopcases[icas * 6 + 3].ReasonCode;
                                                cas.AdjAmount4 = loopcases[icas * 6 + 3].AdjustmentAmount;
                                                cas.AdjQuantity4 = loopcases[icas * 6 + 3].AdjustmentQuantity;
                                            }
                                            if (loopcases.Count > icas * 6 + 4)
                                            {
                                                cas.AdjReasonCode5 = loopcases[icas * 6 + 4].ReasonCode;
                                                cas.AdjAmount5 = loopcases[icas * 6 + 4].AdjustmentAmount;
                                                cas.AdjQuantity5 = loopcases[icas * 6 + 4].AdjustmentQuantity;
                                            }
                                            if (loopcases.Count > icas * 6 + 5)
                                            {
                                                cas.AdjReasonCode6 = loopcases[icas * 6 + 5].ReasonCode;
                                                cas.AdjAmount6 = loopcases[icas * 6 + 5].AdjustmentAmount;
                                                cas.AdjQuantity6 = loopcases[icas * 6 + 5].AdjustmentQuantity;
                                            }
                                            sb.Append(cas.ToX12String());
                                            SegmentCount++;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(sbr1.COBPayerPaidAmount))
                                    {
                                        AMT amt = new AMT();
                                        amt.AmountQualifier = "D";
                                        amt.Amount = sbr1.COBPayerPaidAmount;
                                        sb.Append(amt.ToX12String());
                                        SegmentCount++;
                                    }
                                    if (!string.IsNullOrEmpty(sbr1.COBRemainingPatientLiabilityAmount))
                                    {
                                        AMT amt = new AMT();
                                        amt.AmountQualifier = "EAF";
                                        amt.Amount = sbr1.COBRemainingPatientLiabilityAmount;
                                        sb.Append(amt.ToX12String());
                                        SegmentCount++;
                                    }
                                    if (!string.IsNullOrEmpty(sbr1.COBNonCoveredAmount))
                                    {
                                        AMT amt = new AMT();
                                        amt.AmountQualifier = "A8";
                                        amt.Amount = sbr1.COBNonCoveredAmount;
                                        sb.Append(amt.ToX12String());
                                        SegmentCount++;
                                    }
                                    OI oi = new OI();
                                    oi.BenefitsAssignmentCertificationIndicator = sbr1.BenefitsAssignmentCertificationIndicator;
                                    oi.PatientSignatureSourceCode = sbr1.PatientSignatureSourceCode;
                                    oi.ReleaseOfInformationCode = sbr1.ReleaseOfInformationCode;
                                    sb.Append(oi.ToX12String());
                                    SegmentCount++;
                                    if (!string.IsNullOrEmpty(sbr1.CoveredDays))
                                    {
                                        MIA mia = new MIA();
                                        mia.CoveredDays = sbr1.CoveredDays;
                                        mia.LifetimePsychiatricDays = sbr1.LifetimePsychiatricDays;
                                        mia.ClaimDRGAmount = sbr1.ClaimDRGAmount;
                                        mia.MIA_ClaimPaymentRemarkCode1 = sbr1.MIA_ClaimPaymentRemarkCode1;
                                        mia.ClaimDisproportionateShareAmount = sbr1.ClaimDisproportionateShareAmount;
                                        mia.ClaimMSPPassThroughAmount = sbr1.ClaimMSPPassThroughAmount;
                                        mia.ClaimPPSCapitalAmount = sbr1.ClaimPPSCapitalAmount;
                                        mia.PPSCapitalFSPDRGAmount = sbr1.PPSCapitalFSPDRGAmount;
                                        mia.PPSCapitalHSPDRGAmount = sbr1.PPSCapitalHSPDRGAmount;
                                        mia.PPSCapitalDSHDRGAmount = sbr1.PPSCapitalDSHDRGAmount;
                                        mia.OldCapitalAmount = sbr1.OldCapitalAmount;
                                        mia.PPSCapitalIMEAmount = sbr1.PPSCapitalIMEAmount;
                                        mia.PPSOperatingHospitalSpecificDRGAmount = sbr1.PPSOperatingHospitalSpecificDRGAmount;
                                        mia.CostReportDayCount = sbr1.CostReportDayCount;
                                        mia.PPSOperatingFederalSpecificDRGAmount = sbr1.PPSOperatingFederalSpecificDRGAmount;
                                        mia.ClaimPPSCapitalOutlierAmount = sbr1.ClaimPPSCapitalOutlierAmount;
                                        mia.ClaimIndirectTeachingAmount = sbr1.ClaimIndirectTeachingAmount;
                                        mia.MIA_NonPayableProfessionalComponentBilledAmount = sbr1.MIA_NonPayableProfessionalComponentBilledAmount;
                                        mia.MIA_ClaimPaymentRemarkCode2 = sbr1.MOA_ClaimPaymentRemarkCode2;
                                        mia.MIA_ClaimPaymentRemarkCode3 = sbr1.MOA_ClaimPaymentRemarkCode3;
                                        mia.MIA_ClaimPaymentRemarkCode4 = sbr1.MOA_ClaimPaymentRemarkCode4;
                                        mia.MIA_ClaimPaymentRemarkCode5 = sbr1.MOA_ClaimPaymentRemarkCode5;
                                        mia.PPSCapitalExceptionAmount = sbr1.PPSCapitalExceptionAmount;
                                        sb.Append(mia.ToX12String());
                                        SegmentCount++;
                                    }
                                    if (!string.IsNullOrEmpty(sbr1.ReimbursementRate) || !string.IsNullOrEmpty(sbr1.HCPCSPayableAmount) || !string.IsNullOrEmpty(sbr1.MIA_ClaimPaymentRemarkCode1))
                                    {
                                        MOA moa = new MOA();
                                        moa.ReimbursementRate = sbr1.ReimbursementRate;
                                        moa.HCPCSPayableAmount = sbr1.HCPCSPayableAmount;
                                        moa.MOA_ClaimPaymentRemarkCode1 = sbr1.MOA_ClaimPaymentRemarkCode1;
                                        moa.MOA_ClaimPaymentRemarkCode2 = sbr1.MOA_ClaimPaymentRemarkCode2;
                                        moa.MOA_ClaimPaymentRemarkCode3 = sbr1.MOA_ClaimPaymentRemarkCode3;
                                        moa.MOA_ClaimPaymentRemarkCode4 = sbr1.MOA_ClaimPaymentRemarkCode4;
                                        moa.MOA_ClaimPaymentRemarkCode5 = sbr1.MOA_ClaimPaymentRemarkCode5;
                                        moa.EndStageRenalDiseasePaymentAmount = sbr1.EndStageRenalDiseasePaymentAmount;
                                        moa.MOA_NonPayableProfessionalComponentBilledAmount = sbr1.MOA_NonPayableProfessionalComponentBilledAmount;
                                        sb.Append(moa.ToX12String());
                                        SegmentCount++;
                                    }
                                    nm1 = new NM1();
                                    nm1.NameQualifier = "IL";
                                    nm1.NameType = string.IsNullOrEmpty(sbr1.FirstName) ? "2" : "1";
                                    nm1.LastName = sbr1.LastName;
                                    nm1.FirstName = sbr1.FirstName;
                                    nm1.MiddleName = sbr1.MidddleName;
                                    nm1.Suffix = sbr1.NameSuffix;
                                    nm1.IDQualifer = sbr1.IDQualifier;
                                    nm1.IDCode = sbr1.IDCode;
                                    sb.Append(nm1.ToX12String());
                                    SegmentCount++;
                                    if (!string.IsNullOrEmpty(sbr1.SubscriberAddress))
                                    {
                                        n3 = new N3();
                                        n3.Address = sbr1.SubscriberAddress;
                                        n3.Address2 = sbr1.SubscriberAddress2;
                                        sb.Append(n3.ToX12String());
                                        SegmentCount++;
                                        if (!string.IsNullOrEmpty(sbr1.SubscriberCity))
                                        {
                                            n4 = new N4();
                                            n4.City = sbr1.SubscriberCity;
                                            n4.State = sbr1.SubscriberState;
                                            n4.Zipcode = sbr1.SubscriberZip;
                                            n4.Country = sbr1.SubscriberCountry;
                                            n4.CountrySubCode = sbr1.SubscriberCountrySubCode;
                                            sb.Append(n4.ToX12String());
                                            SegmentCount++;
                                        }
                                    }
                                    if (claimsis.Where(x => x.LoopName == "2330A" && x.RepeatSequence == sbr1.SubscriberSequenceNumber).Count() > 0)
                                    {
                                        rref = new REF();
                                        foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2330A" && x.RepeatSequence == sbr1.SubscriberSequenceNumber))
                                        {
                                            refitem = new REFItem();
                                            refitem.ProviderQualifier = si.ProviderQualifier;
                                            refitem.ProviderID = si.ProviderID;
                                            rref.Refs.Add(refitem);
                                        }
                                        sb.Append(rref.ToX12String());
                                        SegmentCount += rref.Refs.Count;
                                    }
                                    claimprovider = claimproviders.Where(x => x.LoopName == "2330B" && x.RepeatSequence == sbr1.SubscriberSequenceNumber).FirstOrDefault();
                                    if (claimprovider != null)
                                    {
                                        nm1 = new NM1();
                                        nm1.NameQualifier = claimprovider.ProviderQualifier;
                                        nm1.NameType = "2";
                                        nm1.LastName = claimprovider.ProviderLastName;
                                        nm1.IDQualifer = claimprovider.ProviderIDQualifier;
                                        nm1.IDCode = claimprovider.ProviderID;
                                        sb.Append(nm1.ToX12String());
                                        SegmentCount++;
                                        if (!string.IsNullOrEmpty(claimprovider.ProviderAddress))
                                        {
                                            n3 = new N3();
                                            n3.Address = claimprovider.ProviderAddress;
                                            n3.Address2 = claimprovider.ProviderAddress2;
                                            sb.Append(n3.ToX12String());
                                            SegmentCount++;
                                            if (!string.IsNullOrEmpty(claimprovider.ProviderCity))
                                            {
                                                n4 = new N4();
                                                n4.City = claimprovider.ProviderCity;
                                                n4.State = claimprovider.ProviderState;
                                                n4.Zipcode = claimprovider.ProviderZip;
                                                n4.Country = claimprovider.ProviderCountry;
                                                n4.CountrySubCode = claimprovider.ProviderCountrySubCode;
                                                sb.Append(n4.ToX12String());
                                                SegmentCount++;
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(sbr1.PaymentDate))
                                    {
                                        DTP dtp = new DTP();
                                        dtp.DateCode = "573";
                                        dtp.DateQualifier = "D8";
                                        dtp.StartDate = sbr1.PaymentDate;
                                        sb.Append(dtp.ToX12String());
                                        SegmentCount++;
                                    }
                                    if (claimsis.Where(x => x.LoopName == "2330B" && x.RepeatSequence == sbr1.SubscriberSequenceNumber).Count() > 0)
                                    {
                                        rref = new REF();
                                        foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2330B" && x.RepeatSequence == sbr1.SubscriberSequenceNumber))
                                        {
                                            refitem = new REFItem();
                                            refitem.ProviderQualifier = si.ProviderQualifier;
                                            refitem.ProviderID = si.ProviderID;
                                            rref.Refs.Add(refitem);
                                        }
                                        sb.Append(rref.ToX12String());
                                        SegmentCount += rref.Refs.Count;
                                    }
                                    claimprovider = claimproviders.Where(x => x.LoopName == "2330C" && x.RepeatSequence == sbr1.SubscriberSequenceNumber).FirstOrDefault();
                                    if (claimprovider != null)
                                    {
                                        nm1 = new NM1();
                                        nm1.NameQualifier = claimprovider.ProviderQualifier;
                                        nm1.NameType = "1";
                                        sb.Append(nm1.ToX12String());
                                        SegmentCount++;
                                        if (claimsis.Where(x => x.LoopName == "2330C" && x.RepeatSequence == claimprovider.RepeatSequence).Count() > 0)
                                        {
                                            rref = new REF();
                                            foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2330C" && x.RepeatSequence == claimprovider.RepeatSequence))
                                            {
                                                refitem = new REFItem();
                                                refitem.ProviderQualifier = si.ProviderQualifier;
                                                refitem.ProviderID = si.ProviderID;
                                                rref.Refs.Add(refitem);
                                            }
                                            sb.Append(rref.ToX12String());
                                            SegmentCount += rref.Refs.Count;
                                        }
                                    }
                                    claimprovider = claimproviders.Where(x => x.LoopName == "2330D" && x.RepeatSequence == sbr1.SubscriberSequenceNumber).FirstOrDefault();
                                    if (claimprovider != null)
                                    {
                                        nm1 = new NM1();
                                        nm1.NameQualifier = claimprovider.ProviderQualifier;
                                        nm1.NameType = "1";
                                        sb.Append(nm1.ToX12String());
                                        SegmentCount++;
                                        if (claimsis.Where(x => x.LoopName == "2330D" && x.RepeatSequence == sbr1.SubscriberSequenceNumber).Count() > 0)
                                        {
                                            rref = new REF();
                                            foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2330D" && x.RepeatSequence == sbr1.SubscriberSequenceNumber))
                                            {
                                                refitem = new REFItem();
                                                refitem.ProviderQualifier = si.ProviderQualifier;
                                                refitem.ProviderID = si.ProviderID;
                                                rref.Refs.Add(refitem);
                                            }
                                            sb.Append(rref.ToX12String());
                                            SegmentCount += rref.Refs.Count;
                                        }
                                    }
                                    claimprovider = claimproviders.Where(x => x.LoopName == "2330E" && x.RepeatSequence == sbr1.SubscriberSequenceNumber).FirstOrDefault();
                                    if (claimprovider != null)
                                    {
                                        nm1 = new NM1();
                                        nm1.NameQualifier = claimprovider.ProviderQualifier;
                                        nm1.NameType = "1";
                                        sb.Append(nm1.ToX12String());
                                        SegmentCount++;
                                        if (claimsis.Where(x => x.LoopName == "2330E" && x.RepeatSequence == sbr1.SubscriberSequenceNumber).Count() > 0)
                                        {
                                            rref = new REF();
                                            foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2330E" && x.RepeatSequence == sbr1.SubscriberSequenceNumber))
                                            {
                                                refitem = new REFItem();
                                                refitem.ProviderQualifier = si.ProviderQualifier;
                                                refitem.ProviderID = si.ProviderID;
                                                rref.Refs.Add(refitem);
                                            }
                                            sb.Append(rref.ToX12String());
                                            SegmentCount += rref.Refs.Count;
                                        }
                                    }
                                    claimprovider = claimproviders.Where(x => x.LoopName == "2330F" && x.RepeatSequence == sbr1.SubscriberSequenceNumber).FirstOrDefault();
                                    if (claimprovider != null)
                                    {
                                        nm1 = new NM1();
                                        nm1.NameQualifier = claimprovider.ProviderQualifier;
                                        nm1.NameType = "2";
                                        sb.Append(nm1.ToX12String());
                                        SegmentCount++;
                                        if (claimsis.Where(x => x.LoopName == "2330F" && x.RepeatSequence == sbr1.SubscriberSequenceNumber).Count() > 0)
                                        {
                                            rref = new REF();
                                            foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2330D" && x.RepeatSequence == sbr1.SubscriberSequenceNumber))
                                            {
                                                refitem = new REFItem();
                                                refitem.ProviderQualifier = si.ProviderQualifier;
                                                refitem.ProviderID = si.ProviderID;
                                                rref.Refs.Add(refitem);
                                            }
                                            sb.Append(rref.ToX12String());
                                            SegmentCount += rref.Refs.Count;
                                        }
                                    }
                                    claimprovider = claimproviders.Where(x => x.LoopName == "2330G" && x.RepeatSequence == sbr1.SubscriberSequenceNumber).FirstOrDefault();
                                    if (claimprovider != null)
                                    {
                                        nm1 = new NM1();
                                        nm1.NameQualifier = claimprovider.ProviderQualifier;
                                        nm1.NameType = "1";
                                        sb.Append(nm1.ToX12String());
                                        SegmentCount++;
                                        if (claimsis.Where(x => x.LoopName == "2330G" && x.RepeatSequence == sbr1.SubscriberSequenceNumber).Count() > 0)
                                        {
                                            rref = new REF();
                                            foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2330G" && x.RepeatSequence == sbr1.SubscriberSequenceNumber))
                                            {
                                                refitem = new REFItem();
                                                refitem.ProviderQualifier = si.ProviderQualifier;
                                                refitem.ProviderID = si.ProviderID;
                                                rref.Refs.Add(refitem);
                                            }
                                            sb.Append(rref.ToX12String());
                                            SegmentCount += rref.Refs.Count;
                                        }
                                    }
                                    claimprovider = claimproviders.Where(x => x.LoopName == "2330H" && x.RepeatSequence == sbr1.SubscriberSequenceNumber).FirstOrDefault();
                                    if (claimprovider != null)
                                    {
                                        nm1 = new NM1();
                                        nm1.NameQualifier = claimprovider.ProviderQualifier;
                                        nm1.NameType = "1";
                                        sb.Append(nm1.ToX12String());
                                        SegmentCount++;
                                        if (claimsis.Where(x => x.LoopName == "2330H" && x.RepeatSequence == sbr1.SubscriberSequenceNumber).Count() > 0)
                                        {
                                            rref = new REF();
                                            foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2330H" && x.RepeatSequence == sbr1.SubscriberSequenceNumber))
                                            {
                                                refitem = new REFItem();
                                                refitem.ProviderQualifier = si.ProviderQualifier;
                                                refitem.ProviderID = si.ProviderID;
                                                rref.Refs.Add(refitem);
                                            }
                                            sb.Append(rref.ToX12String());
                                            SegmentCount += rref.Refs.Count;
                                        }
                                    }
                                    claimprovider = claimproviders.Where(x => x.LoopName == "2330I" && x.RepeatSequence == sbr1.SubscriberSequenceNumber).FirstOrDefault();
                                    if (claimprovider != null)
                                    {
                                        nm1 = new NM1();
                                        nm1.NameQualifier = claimprovider.ProviderQualifier;
                                        nm1.NameType = "2";
                                        sb.Append(nm1.ToX12String());
                                        SegmentCount++;
                                        if (claimsis.Where(x => x.LoopName == "2330I" && x.RepeatSequence == sbr1.SubscriberSequenceNumber).Count() > 0)
                                        {
                                            rref = new REF();
                                            foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2330I" && x.RepeatSequence == sbr1.SubscriberSequenceNumber))
                                            {
                                                refitem = new REFItem();
                                                refitem.ProviderQualifier = si.ProviderQualifier;
                                                refitem.ProviderID = si.ProviderID;
                                                rref.Refs.Add(refitem);
                                            }
                                            sb.Append(rref.ToX12String());
                                            SegmentCount += rref.Refs.Count;
                                        }
                                    }
                                }
                            }
                            foreach (ServiceLine line in claimlines.OrderBy(x => int.Parse(x.ServiceLineNumber)))
                            {
                                LX lx = new LX();
                                lx.ServiceLineNumber = line.ServiceLineNumber;
                                sb.Append(lx.ToX12String());
                                SegmentCount++;
                                SV2 sv2 = new SV2();
                                sv2.RevenueCode = line.RevenueCode;
                                sv2.ServiceIDQualifier = line.ServiceIDQualifier;
                                sv2.ProcedureCode = line.ProcedureCode;
                                sv2.ProcedureModifier1 = line.ProcedureModifier1;
                                sv2.ProcedureModifier2 = line.ProcedureModifier2;
                                sv2.ProcedureModifier3 = line.ProcedureModifier3;
                                sv2.ProcedureModifier4 = line.ProcedureModifier4;
                                sv2.ProcedureDescription = line.ProcedureDescription;
                                sv2.LineItemChargeAmount = line.LineItemChargeAmount;
                                sv2.LineItemUnit = line.LineItemUnit;
                                sv2.ServiceUnitQuantity = line.ServiceUnitQuantity;
                                sv2.LineItemDeniedChargeAmount = line.LineItemDeniedChargeAmount;
                                sb.Append(sv2.ToX12String());
                                SegmentCount++;
                                if (claimpwks.Where(x => x.ServiceLineNumber == line.ServiceLineNumber).Count() > 0)
                                {
                                    PWK pwk = new PWK();
                                    foreach (ClaimPWK claimpwk in claimpwks.Where(x => x.ServiceLineNumber == line.ServiceLineNumber))
                                    {
                                        PWKItem pwkitem = new PWKItem();
                                        pwkitem.ReportTypeCode = claimpwk.ReportTypeCode;
                                        pwkitem.ReportTransmissionCode = claimpwk.ReportTransmissionCode;
                                        pwkitem.AttachmentControlNumber = claimpwk.AttachmentControlNumber;
                                        pwk.Pwks.Add(pwkitem);
                                    }
                                    sb.Append(pwk.ToX12String());
                                    SegmentCount += pwk.Pwks.Count;
                                }
                                if (!string.IsNullOrEmpty(line.ServiceFromDate) && !string.IsNullOrEmpty(line.ServiceToDate))
                                {
                                    DTP dtp = new DTP();
                                    dtp.DateCode = "472";
                                    dtp.DateQualifier = string.IsNullOrEmpty(line.ServiceToDate) ? "D8" : "RD8";
                                    dtp.StartDate = line.ServiceFromDate;
                                    dtp.EndDate = line.ServiceToDate;
                                    sb.Append(dtp.ToX12String());
                                    SegmentCount++;
                                }
                                if (claimsis.Where(x => x.LoopName == "2400" && x.ServiceLineNumber == line.ServiceLineNumber).Count() > 0)
                                {
                                    rref = new REF();
                                    foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2400" && x.ServiceLineNumber == line.ServiceLineNumber))
                                    {
                                        refitem = new REFItem();
                                        refitem.ProviderQualifier = si.ProviderQualifier;
                                        refitem.ProviderID = si.ProviderID;
                                        refitem.OtherPayerPrimaryIDentification = si.OtherPayerPrimaryIDentification;
                                        rref.Refs.Add(refitem);
                                    }
                                    sb.Append(rref.ToX12String());
                                    SegmentCount += rref.Refs.Count;
                                }
                                if (!string.IsNullOrEmpty(line.ServiceTaxAmount))
                                {
                                    AMT amt = new AMT();
                                    amt.AmountQualifier = "GT";
                                    amt.Amount = line.ServiceTaxAmount;
                                    sb.Append(amt.ToX12String());
                                    SegmentCount++;
                                }
                                if (!string.IsNullOrEmpty(line.FacilityTaxAmount))
                                {
                                    AMT amt = new AMT();
                                    amt.AmountQualifier = "N8";
                                    amt.Amount = line.FacilityTaxAmount;
                                    sb.Append(amt.ToX12String());
                                    SegmentCount++;
                                }
                                if (claimnotes.Where(x => x.ServiceLineNumber == line.ServiceLineNumber).Count() > 0)
                                {
                                    NTE note = new NTE();
                                    note.NoteCode = claimnotes.Where(x => x.ServiceLineNumber == line.ServiceLineNumber).First().NoteCode;
                                    note.NoteText = claimnotes.Where(x => x.ServiceLineNumber == line.ServiceLineNumber).First().NoteText;
                                    sb.Append(note.ToX12String());
                                    SegmentCount++;
                                }
                                if (!string.IsNullOrEmpty(line.PricingMethodology) && !string.IsNullOrEmpty(line.RepricedAllowedAmount))
                                {
                                    HCP hcp = new HCP();
                                    hcp.PricingMethodology = line.PricingMethodology;
                                    hcp.RepricedAllowedAmount = line.RepricedAllowedAmount;
                                    hcp.RepricedSavingAmount = line.RepricedSavingAmount;
                                    hcp.RepricingOrganizationID = line.RepricingOrganizationIdentifier;
                                    hcp.RepricingRate = line.RepricingRate;
                                    hcp.RepricedGroupCode = line.RepricedAmbulatoryPatientGroupCode;
                                    hcp.RepricedGroupAmount = line.RepricedAmbulatoryPatientGroupAmount;
                                    hcp.RepricingUnit = line.RepricingUnit;
                                    hcp.RepricingQuantity = line.RepricingQuantity;
                                    hcp.RejectReasonCode = line.RejectReasonCode;
                                    hcp.PolicyComplianceCode = line.PolicyComplianceCode;
                                    hcp.ExceptionCode = line.ExceptionCode;
                                    sb.Append(hcp.ToX12String());
                                    SegmentCount++;
                                }
                                if (!string.IsNullOrEmpty(line.LINQualifier) && !string.IsNullOrEmpty(line.NationalDrugCode))
                                {
                                    LIN lin = new LIN();
                                    lin.LINQualifier = line.LINQualifier;
                                    lin.NationalDrugCode = line.NationalDrugCode;
                                    sb.Append(lin.ToX12String());
                                    SegmentCount++;
                                }
                                if (!string.IsNullOrEmpty(line.DrugQuantity) && !string.IsNullOrEmpty(line.DrugQualifier))
                                {
                                    CTP ctp = new CTP();
                                    ctp.DrugQuantity = line.DrugQuantity;
                                    ctp.DrugQualifier = line.DrugQualifier;
                                    sb.Append(ctp.ToX12String());
                                    SegmentCount++;
                                }
                                if (claimsis.Where(x => x.LoopName == "2410" && x.ServiceLineNumber == line.ServiceLineNumber).Count() > 0)
                                {
                                    claimsi = claimsis.Where(x => x.LoopName == "2410" && x.ServiceLineNumber == line.ServiceLineNumber).FirstOrDefault();
                                    rref = new REF();
                                    refitem = new REFItem();
                                    refitem.ProviderQualifier = claimsi.ProviderQualifier;
                                    refitem.ProviderID = claimsi.ProviderID;
                                    rref.Refs.Add(refitem);
                                    sb.Append(rref.ToX12String());
                                    SegmentCount++;
                                }
                                claimprovider = claimproviders.Where(x => x.LoopName == "2420A" && x.ServiceLineNumber == line.ServiceLineNumber).FirstOrDefault();
                                if (claimprovider != null)
                                {
                                    nm1 = new NM1();
                                    nm1.NameQualifier = claimprovider.ProviderQualifier;
                                    nm1.NameType = string.IsNullOrEmpty(claimprovider.ProviderFirstName) ? "2" : "1";
                                    nm1.LastName = claimprovider.ProviderLastName;
                                    nm1.FirstName = claimprovider.ProviderFirstName;
                                    nm1.MiddleName = claimprovider.ProviderMiddle;
                                    nm1.Suffix = claimprovider.ProviderSuffix;
                                    nm1.IDQualifer = claimprovider.ProviderIDQualifier;
                                    nm1.IDCode = claimprovider.ProviderID;
                                    sb.Append(nm1.ToX12String());
                                    SegmentCount++;
                                    if (claimsis.Where(x => x.LoopName == "2420A" && x.ServiceLineNumber == line.ServiceLineNumber).Count() > 0)
                                    {
                                        rref = new REF();
                                        foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2420A" && x.ServiceLineNumber == line.ServiceLineNumber))
                                        {
                                            refitem = new REFItem();
                                            refitem.ProviderQualifier = si.ProviderQualifier;
                                            refitem.ProviderID = si.ProviderID;
                                            refitem.OtherPayerPrimaryIDentification = si.OtherPayerPrimaryIDentification;
                                            rref.Refs.Add(refitem);
                                        }
                                        sb.Append(rref.ToX12String());
                                        SegmentCount += rref.Refs.Count;
                                    }
                                }
                                claimprovider = claimproviders.Where(x => x.LoopName == "2420B" && x.ServiceLineNumber == line.ServiceLineNumber).FirstOrDefault();
                                if (claimprovider != null)
                                {
                                    nm1 = new NM1();
                                    nm1.NameQualifier = claimprovider.ProviderQualifier;
                                    nm1.NameType = string.IsNullOrEmpty(claimprovider.ProviderFirstName) ? "2" : "1";
                                    nm1.LastName = claimprovider.ProviderLastName;
                                    nm1.FirstName = claimprovider.ProviderFirstName;
                                    nm1.MiddleName = claimprovider.ProviderMiddle;
                                    nm1.Suffix = claimprovider.ProviderSuffix;
                                    nm1.IDQualifer = claimprovider.ProviderIDQualifier;
                                    nm1.IDCode = claimprovider.ProviderID;
                                    sb.Append(nm1.ToX12String());
                                    SegmentCount++;
                                    if (claimsis.Where(x => x.LoopName == "2420B" && x.ServiceLineNumber == line.ServiceLineNumber).Count() > 0)
                                    {
                                        rref = new REF();
                                        foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2420B" && x.ServiceLineNumber == line.ServiceLineNumber))
                                        {
                                            refitem = new REFItem();
                                            refitem.ProviderQualifier = si.ProviderQualifier;
                                            refitem.ProviderID = si.ProviderID;
                                            refitem.OtherPayerPrimaryIDentification = si.OtherPayerPrimaryIDentification;
                                            rref.Refs.Add(refitem);
                                        }
                                        sb.Append(rref.ToX12String());
                                        SegmentCount += rref.Refs.Count;
                                    }
                                }
                                claimprovider = claimproviders.Where(x => x.LoopName == "2420C" && x.ServiceLineNumber == line.ServiceLineNumber).FirstOrDefault();
                                if (claimprovider != null)
                                {
                                    nm1 = new NM1();
                                    nm1.NameQualifier = claimprovider.ProviderQualifier;
                                    nm1.NameType = string.IsNullOrEmpty(claimprovider.ProviderFirstName) ? "2" : "1";
                                    nm1.LastName = claimprovider.ProviderLastName;
                                    nm1.FirstName = claimprovider.ProviderFirstName;
                                    nm1.MiddleName = claimprovider.ProviderMiddle;
                                    nm1.Suffix = claimprovider.ProviderSuffix;
                                    nm1.IDQualifer = claimprovider.ProviderIDQualifier;
                                    nm1.IDCode = claimprovider.ProviderID;
                                    sb.Append(nm1.ToX12String());
                                    SegmentCount++;
                                    if (claimsis.Where(x => x.LoopName == "2420C" && x.ServiceLineNumber == line.ServiceLineNumber).Count() > 0)
                                    {
                                        rref = new REF();
                                        foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2420C" && x.ServiceLineNumber == line.ServiceLineNumber))
                                        {
                                            refitem = new REFItem();
                                            refitem.ProviderQualifier = si.ProviderQualifier;
                                            refitem.ProviderID = si.ProviderID;
                                            refitem.OtherPayerPrimaryIDentification = si.OtherPayerPrimaryIDentification;
                                            rref.Refs.Add(refitem);
                                        }
                                        sb.Append(rref.ToX12String());
                                        SegmentCount += rref.Refs.Count;
                                    }
                                }
                                claimprovider = claimproviders.Where(x => x.LoopName == "2420D" && x.ServiceLineNumber == line.ServiceLineNumber).FirstOrDefault();
                                if (claimprovider != null)
                                {
                                    nm1 = new NM1();
                                    nm1.NameQualifier = claimprovider.ProviderQualifier;
                                    nm1.NameType = string.IsNullOrEmpty(claimprovider.ProviderFirstName) ? "2" : "1";
                                    nm1.LastName = claimprovider.ProviderLastName;
                                    nm1.FirstName = claimprovider.ProviderFirstName;
                                    nm1.MiddleName = claimprovider.ProviderMiddle;
                                    nm1.Suffix = claimprovider.ProviderSuffix;
                                    nm1.IDQualifer = claimprovider.ProviderIDQualifier;
                                    nm1.IDCode = claimprovider.ProviderID;
                                    sb.Append(nm1.ToX12String());
                                    SegmentCount++;
                                    if (claimsis.Where(x => x.LoopName == "2420D" && x.ServiceLineNumber == line.ServiceLineNumber).Count() > 0)
                                    {
                                        rref = new REF();
                                        foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2420D" && x.ServiceLineNumber == line.ServiceLineNumber))
                                        {
                                            refitem = new REFItem();
                                            refitem.ProviderQualifier = si.ProviderQualifier;
                                            refitem.ProviderID = si.ProviderID;
                                            refitem.OtherPayerPrimaryIDentification = si.OtherPayerPrimaryIDentification;
                                            rref.Refs.Add(refitem);
                                        }
                                        sb.Append(rref.ToX12String());
                                        SegmentCount += rref.Refs.Count;
                                    }
                                }
                                if (claimsvds.Where(x => x.ServiceLineNumber == line.ServiceLineNumber).Count() > 0)
                                {
                                    foreach (ClaimLineSVD claimlinesvd in claimsvds.Where(x => x.ServiceLineNumber == line.ServiceLineNumber))
                                    {
                                        SVD svd = new SVD();
                                        svd.OtherPayerPrimaryIdentifier = claimlinesvd.OtherPayerPrimaryIdentifier;
                                        svd.ServiceLinePaidAmount = claimlinesvd.ServiceLinePaidAmount;
                                        svd.ServiceQualifier = claimlinesvd.ServiceQualifier;
                                        svd.ProcedureCode = claimlinesvd.ProcedureCode;
                                        svd.ProcedureModifier1 = claimlinesvd.ProcedureModifier1;
                                        svd.ProcedureModifier2 = claimlinesvd.ProcedureModifier2;
                                        svd.ProcedureModifier3 = claimlinesvd.ProcedureModifier3;
                                        svd.ProcedureModifier4 = claimlinesvd.ProcedureModifier4;
                                        svd.ProcedureDescription = claimlinesvd.ProcedureDescription;
                                        svd.PaidServiceUnitCount = claimlinesvd.PaidServiceUnitCount;
                                        svd.BundledLineNumber = claimlinesvd.BundledLineNumber;
                                        svd.ServiceLineRevenueCode = claimlinesvd.ServiceLineRevenueCode;
                                        sb.Append(svd.ToX12String());
                                        SegmentCount++;
                                        List<ClaimCAS> loopcases = claimcases.Where(x => x.ServiceLineNumber == line.ServiceLineNumber && x.SubscriberSequenceNumber == claimlinesvd.RepeatSequence).ToList();
                                        if (loopcases.Count > 0)
                                        {
                                            foreach (string groupCode in loopcases.Select(x => x.GroupCode).Distinct())
                                            {
                                                List<ClaimCAS> groupcases = loopcases.Where(x => x.GroupCode == groupCode).ToList();
                                                int caspages = (int)(Math.Ceiling((decimal)groupcases.Count / 6));
                                                for (int icas = 0; icas < caspages; icas++)
                                                {
                                                    CAS cas = new CAS();
                                                    cas.AdjGroupCode = groupcases[icas * 6].GroupCode;
                                                    cas.AdjReasonCode1 = groupcases[icas * 6].ReasonCode;
                                                    cas.AdjAmount1 = groupcases[icas * 6].AdjustmentAmount;
                                                    cas.AdjQuantity1 = groupcases[icas * 6].AdjustmentQuantity;
                                                    if (groupcases.Count > icas * 6 + 1)
                                                    {
                                                        cas.AdjReasonCode2 = groupcases[icas * 6 + 1].ReasonCode;
                                                        cas.AdjAmount2 = groupcases[icas * 6 + 1].AdjustmentAmount;
                                                        cas.AdjQuantity2 = groupcases[icas * 6 + 1].AdjustmentQuantity;
                                                    }
                                                    if (groupcases.Count > icas * 6 + 2)
                                                    {
                                                        cas.AdjReasonCode3 = groupcases[icas * 6 + 2].ReasonCode;
                                                        cas.AdjAmount3 = groupcases[icas * 6 + 2].AdjustmentAmount;
                                                        cas.AdjQuantity3 = groupcases[icas * 6 + 2].AdjustmentQuantity;
                                                    }
                                                    if (groupcases.Count > icas * 6 + 3)
                                                    {
                                                        cas.AdjReasonCode4 = groupcases[icas * 6 + 3].ReasonCode;
                                                        cas.AdjAmount4 = groupcases[icas * 6 + 3].AdjustmentAmount;
                                                        cas.AdjQuantity4 = groupcases[icas * 6 + 3].AdjustmentQuantity;
                                                    }
                                                    if (groupcases.Count > icas * 6 + 4)
                                                    {
                                                        cas.AdjReasonCode5 = groupcases[icas * 6 + 4].ReasonCode;
                                                        cas.AdjAmount5 = groupcases[icas * 6 + 4].AdjustmentAmount;
                                                        cas.AdjQuantity5 = groupcases[icas * 6 + 4].AdjustmentQuantity;
                                                    }
                                                    if (groupcases.Count > icas * 6 + 5)
                                                    {
                                                        cas.AdjReasonCode6 = groupcases[icas * 6 + 5].ReasonCode;
                                                        cas.AdjAmount6 = groupcases[icas * 6 + 5].AdjustmentAmount;
                                                        cas.AdjQuantity6 = groupcases[icas * 6 + 5].AdjustmentQuantity;
                                                    }
                                                    sb.Append(cas.ToX12String());
                                                    SegmentCount++;
                                                }
                                            }
                                        }
                                        DTP dtp = new DTP();
                                        dtp.DateCode = "573";
                                        dtp.DateQualifier = "D8";
                                        dtp.StartDate = claimlinesvd.AdjudicationDate;
                                        sb.Append(dtp.ToX12String());
                                        SegmentCount++;
                                        if (!string.IsNullOrEmpty(claimlinesvd.ReaminingPatientLiabilityAmount))
                                        {
                                            AMT amt = new AMT();
                                            amt.AmountQualifier = "EAF";
                                            amt.Amount = claimlinesvd.ReaminingPatientLiabilityAmount;
                                            sb.Append(amt.ToX12String());
                                            SegmentCount++;
                                        }
                                    }
                                }
                            }
                        }
                        SE se = new SE();
                        SegmentCount++;
                        se.SegmentCount = SegmentCount.ToString();
                        se.TransactionControlNumber = "0000001";
                        sb.Append(se.ToX12String());
                        GE ge = new GE();
                        ge.GroupControlNumber = "1";
                        ge.NumberofTransactionSets = "1";
                        sb.Append(ge.ToX12String());
                        IEA iea = new IEA();
                        iea.NumberofFunctionalGroups = "1";
                        iea.InterchangeControlNumber = icn;
                        sb.Append(iea.ToX12String());
                        File.WriteAllText(Path.Combine(DestinationFolder, "IEHP_" + "_837I_MCE_" + DateTime.Today.ToString("yyyyMMdd") + "_" + (i + 1).ToString().PadLeft(5, '1') + ".dat"), sb.ToString());
                    }
                }
                context.Dispose();
            }
        }
    }
}
