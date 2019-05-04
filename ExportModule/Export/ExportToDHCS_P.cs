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
    public static partial class ExportToDHCS
    {
        public static void Export837P(string CountyCode)
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
                List<ClaimHeader> headers = context.ClaimHeaders.Where(x => x.ClaimID.StartsWith(CountyCode) && x.ClaimType == "B").OrderBy(x => x.ID).Skip(i * 5000).Take(5000).ToList();
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
                    isa.InterchangeSenderID = "330704304" + CountyCode;
                    isa.InterchangeReceiverID = "CALIFORNIA-DHCS";
                    isa.InterchangeDate = transactionDate;
                    isa.InterchangeTime = transactionTime;
                    isa.InterchangeControlNumber = icn;
                    isa.ProductionFlag = "P";
                    sb.Append(isa.ToX12String());
                    GS gs = new GS();
                    gs.FunctionalIDCode = "HC";
                    gs.SenderID = "330704304";
                    gs.ReceiverID = "CALIFORNIA-DHCS";
                    gs.TransactionDate = transactionDate;
                    gs.TransactrionTime = transactionTime;
                    gs.GroupControlNumber = "1";
                    gs.ResponsibleAgencyCode = "X";
                    gs.VersionID = "005010X222A1";
                    sb.Append(gs.ToX12String());
                    ST st = new ST();
                    st.TransactionControlNumber = "0000001";
                    st.VersionNumber = "005010X222A1";
                    sb.Append(st.ToX12String());
                    SegmentCount++;
                    BHT bht = new BHT();
                    bht.TransactionID = transactionDate + transactionTime;
                    bht.TransactionDate = transactionDate;
                    bht.TransactionTime = transactionTime;
                    bht.TransactionTypeCode = "RP";
                    sb.Append(bht.ToX12String());
                    SegmentCount++;
                    NM1 nm1 = new NM1();
                    nm1.NameQualifier = "41";
                    nm1.NameType = "2";
                    nm1.LastName = "IEHP";
                    nm1.IDQualifer = "46";
                    nm1.IDCode = "330704304";
                    sb.Append(nm1.ToX12String());
                    SegmentCount++;
                    PER per = new PER();
                    PERItem peritem = new PERItem();
                    peritem.ContactName = "ARNOLD ALLENDE";
                    peritem.Phone = "9098902025";
                    per.Pers.Add(peritem);
                    sb.Append(per.ToX12String());
                    SegmentCount++;
                    nm1 = new NM1();
                    nm1.NameQualifier = "40";
                    nm1.NameType = "2";
                    nm1.LastName = "CALIFORNIA-DHCS";
                    nm1.IDQualifer = "46";
                    nm1.IDCode = "610442";
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
                        nm1.NameType = string.IsNullOrEmpty(claimprovider.ProviderFirstName) ? "2" : "1";
                        nm1.LastName = claimprovider.ProviderLastName;
                        nm1.FirstName = claimprovider.ProviderFirstName;
                        nm1.MiddleName = claimprovider.ProviderMiddle;
                        nm1.Suffix = claimprovider.ProviderSuffix;
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
                            nm1.NameType = "1";
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
                        sbr.InsuredTypeCode = claimsbr.InsuredTypeCode;
                        sbr.ClaimFilingCode = claimsbr.ClaimFilingCode;
                        sb.Append(sbr.ToX12String());
                        SegmentCount++;
                        if (!string.IsNullOrEmpty(claimsbr.DeathDate) || !string.IsNullOrEmpty(claimsbr.Weight) || !string.IsNullOrEmpty(claimsbr.PregnancyIndicator))
                        {
                            PAT pat = new PAT();
                            pat.PatientRelatedDeathDate = claimsbr.DeathDate;
                            pat.PatientRelatedUnit = claimsbr.Unit;
                            pat.PatientRelatedWeight = claimsbr.Weight;
                            pat.PatientRelatedPregnancyIndicator = claimsbr.PregnancyIndicator;
                            sb.Append(pat.ToX12String());
                            SegmentCount++;
                        }
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
                        if (!string.IsNullOrEmpty(claimsbr.SubscriberAddress))
                        {
                            n3 = new N3();
                            n3.Address = claimsbr.SubscriberAddress;
                            n3.Address2 = claimsbr.SubscriberAddress2;
                            sb.Append(n3.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(claimsbr.SubscriberCity))
                        {
                            n4 = new N4();
                            n4.City = claimsbr.SubscriberCity;
                            n4.State = claimsbr.SubscriberState;
                            n4.Zipcode = claimsbr.SubscriberZip;
                            n4.Country = claimsbr.SubscriberCountry;
                            n4.CountrySubCode = claimsbr.SubscriberCountrySubCode;
                            sb.Append(n4.ToX12String());
                            SegmentCount++;
                        }
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
                            nm1.NameType = "1";
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
                                ProviderContact claimpc = providercontacts.Where(x => x.ClaimID == header.ClaimID && x.LoopName == "2010CA").FirstOrDefault();
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
                        }
                        CLM_P clm = new CLM_P();
                        clm.ClaimID = header.ClaimID;
                        clm.ClaimAmount = header.ClaimAmount;
                        clm.ClaimPOS = header.ClaimPOS;
                        clm.ClaimType = header.ClaimType;
                        clm.ClaimFrequencyCode = header.ClaimFrequencyCode;
                        clm.ClaimProviderSignature = header.ClaimProviderSignature;
                        clm.ClaimProviderAssignment = header.ClaimProviderAssignment;
                        clm.ClaimBenefitAssignment = header.ClaimBenefitAssignment;
                        clm.ClaimReleaseofInformationCode = header.ClaimReleaseofInformationCode;
                        clm.ClaimPatientSignatureSourceCode = header.ClaimPatientSignatureSourceCode;
                        clm.ClaimRelatedCausesQualifier = header.ClaimRelatedCausesQualifier;
                        clm.ClaimRelatedCausesCode = header.ClaimRelatedCausesCode;
                        clm.ClaimRelatedStateCode = header.ClaimRelatedStateCode;
                        clm.ClaimRelatedCountryCode = header.ClaimRelatedCountryCode;
                        clm.ClaimSpecialProgramCode = header.ClaimSpecialProgramCode;
                        clm.ClaimDelayReasonCode = header.ClaimDelayReasonCode;
                        sb.Append(clm.ToX12String());
                        SegmentCount++;
                        if (!string.IsNullOrEmpty(header.CurrentIllnessDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "431";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = header.CurrentIllnessDate;
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.InitialTreatmentDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "454";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = header.InitialTreatmentDate;
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.LastSeenDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "304";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = header.LastSeenDate;
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.AcuteManifestestationDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "453";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = header.AcuteManifestestationDate;
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.AccidentDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "439";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = header.AccidentDate;
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.LastMenstrualPeriodDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "484";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = header.LastMenstrualPeriodDate;
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.LastXrayDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "455";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = header.LastXrayDate;
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.PrescriptionDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "471";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = header.PrescriptionDate;
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.DisabilityStartDate) || !string.IsNullOrEmpty(header.DisabilityEndDate))
                        {
                            DTP dtp = new DTP();
                            if (!string.IsNullOrEmpty(header.DisabilityStartDate) && !string.IsNullOrEmpty(header.DisabilityEndDate))
                            {
                                dtp.DateCode = "314";
                                dtp.DateQualifier = "RD8";
                                dtp.StartDate = header.DisabilityStartDate;
                                dtp.EndDate = header.DisabilityEndDate;
                            }
                            else if (!string.IsNullOrEmpty(header.DisabilityStartDate))
                            {
                                dtp.DateCode = "360";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = header.DisabilityStartDate;
                            }
                            else if (!string.IsNullOrEmpty(header.DisabilityEndDate))
                            {
                                dtp.DateCode = "361";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = header.DisabilityEndDate;
                            }
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.LastWorkedDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "297";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = header.LastWorkedDate;
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.AuthorizedReturnToWorkDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "296";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = header.AuthorizedReturnToWorkDate;
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.AdmissionDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "435";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = header.AdmissionDate;
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.DischargeDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "096";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = header.DischargeDate;
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.AssumedStartDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "090";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = header.AssumedStartDate;
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.AssumedEndDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "091";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = header.AssumedEndDate;
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.FirstContactDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "444";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = header.FirstContactDate;
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
                            NTE nte = new NTE();
                            nte.NoteCode = claimnotes.Where(x => x.ServiceLineNumber == "0").FirstOrDefault().NoteCode;
                            nte.NoteText = claimnotes.Where(x => x.ServiceLineNumber == "0").FirstOrDefault().NoteText;
                            sb.Append(nte.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.AmbulanceReasonCode) && !string.IsNullOrEmpty(header.AmbulanceQuantity))
                        {
                            CR1 cr1 = new CR1();
                            cr1.AmbulanceWeight = header.AmbulanceWeight;
                            cr1.AmbulanceReasonCode = header.AmbulanceReasonCode;
                            cr1.AmbulanceQuantity = header.AmbulanceQuantity;
                            cr1.AmbulanceRoundTripDescription = header.AmbulanceRoundTripDescription;
                            cr1.AmbulanceStretcherDescription = header.AmbulanceStretcherDescription;
                            sb.Append(cr1.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.PatientConditionCode))
                        {
                            CR2 cr2 = new CR2();
                            cr2.PatientConditionCode = header.PatientConditionCode;
                            cr2.PatientConditionDescription1 = header.PatientConditionDescription1;
                            cr2.PatientConditionDescription2 = header.PatientConditionDescription2;
                            sb.Append(cr2.ToX12String());
                            SegmentCount++;
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
                        HI_P hip = new HI_P();
                        foreach (ClaimHI claimhi in claimhis)
                        {
                            HIItem hiitem = new HIItem();
                            hiitem.HIQual = claimhi.HIQual;
                            hiitem.HICode = claimhi.HICode;
                            hip.His.Add(hiitem);
                        }
                        sb.Append(hip.ToX12String());
                        SegmentCount += hip.HiCount;
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
                        if (claimproviders.Where(x => x.LoopName == "2310A").Count() > 0)
                        {
                            foreach (ClaimProvider provider in claimproviders.Where(x => x.LoopName == "2310A"))
                            {
                                nm1 = new NM1();
                                nm1.NameQualifier = provider.ProviderQualifier;
                                nm1.NameType = "1";
                                nm1.LastName = provider.ProviderLastName;
                                nm1.FirstName = provider.ProviderFirstName;
                                nm1.MiddleName = provider.ProviderMiddle;
                                nm1.Suffix = provider.ProviderSuffix;
                                nm1.IDQualifer = provider.ProviderIDQualifier;
                                nm1.IDCode = provider.ProviderID;
                                sb.Append(nm1.ToX12String());
                                SegmentCount++;
                                if (claimsis.Where(x => x.LoopName == "2310A" && x.RepeatSequence == claimprovider.RepeatSequence).Count() > 0)
                                {
                                    rref = new REF();
                                    foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2310A" && x.RepeatSequence == claimprovider.RepeatSequence))
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
                        claimprovider = claimproviders.Where(x => x.LoopName == "2310B").FirstOrDefault();
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
                            if (!string.IsNullOrEmpty(claimprovider.ProviderTaxonomyCode))
                            {
                                PRV prv = new PRV();
                                prv.ProviderQualifier = "PE";
                                prv.ProviderTaxonomyCode = claimprovider.ProviderTaxonomyCode;
                                sb.Append(prv.ToX12String());
                                SegmentCount++;
                            }
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
                            ProviderContact claimpc = claimpcs.Where(x => x.LoopName == "2310C").FirstOrDefault();
                            if (claimpc != null)
                            {
                                per = new PER();
                                peritem = new PERItem();
                                peritem.ContactName = claimpc.ContactName;
                                peritem.Phone = claimpc.Phone;
                                peritem.Email = claimpc.Email;
                                peritem.Fax = claimpc.Fax;
                                peritem.PhoneEx = claimpc.PhoneEx;
                                per.Pers.Add(peritem);
                                sb.Append(per.ToX12String());
                                SegmentCount += per.Pers.Count;
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
                        claimprovider = claimproviders.Where(x => x.LoopName == "2310F").FirstOrDefault();
                        if (claimprovider != null)
                        {
                            nm1 = new NM1();
                            nm1.NameQualifier = claimprovider.ProviderQualifier;
                            nm1.NameType = "2";
                            nm1.LastName = claimprovider.ProviderLastName;
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
                                if (!string.IsNullOrEmpty(sbr1.COBNonCoveredAmount))
                                {
                                    AMT amt = new AMT();
                                    amt.AmountQualifier = "A8";
                                    amt.Amount = sbr1.COBNonCoveredAmount;
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
                                OI oi = new OI();
                                oi.BenefitsAssignmentCertificationIndicator = sbr1.BenefitsAssignmentCertificationIndicator;
                                oi.PatientSignatureSourceCode = sbr1.PatientSignatureSourceCode;
                                oi.ReleaseOfInformationCode = sbr1.ReleaseOfInformationCode;
                                sb.Append(oi.ToX12String());
                                SegmentCount++;
                                if (!string.IsNullOrEmpty(sbr1.ReimbursementRate) || !string.IsNullOrEmpty(sbr1.HCPCSPayableAmount) || !string.IsNullOrEmpty(sbr1.MOA_ClaimPaymentRemarkCode1) || !string.IsNullOrEmpty(sbr1.EndStageRenalDiseasePaymentAmount) || !string.IsNullOrEmpty(sbr1.MOA_NonPayableProfessionalComponentBilledAmount))
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
                                claimsi = claimsis.Where(x => x.LoopName == "2330A" && x.RepeatSequence == sbr1.SubscriberSequenceNumber && x.ProviderQualifier == "SY").FirstOrDefault();
                                if (claimsi != null)
                                {
                                    rref = new REF();
                                    refitem = new REFItem();
                                    refitem.ProviderQualifier = claimsi.ProviderQualifier;
                                    refitem.ProviderID = claimsi.ProviderID;
                                    rref.Refs.Add(refitem);
                                    sb.Append(rref.ToX12String());
                                    SegmentCount++;
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
                                if (claimproviders.Where(x => x.LoopName == "2330C" && x.RepeatSequence.Split(':')[0] == sbr1.SubscriberSequenceNumber).Count() > 0)
                                {
                                    foreach (ClaimProvider prv in claimproviders.Where(x => x.LoopName == "2330C" && x.RepeatSequence.Split(':')[0] == sbr1.SubscriberSequenceNumber))
                                    {
                                        nm1 = new NM1();
                                        nm1.NameQualifier = prv.ProviderQualifier;
                                        nm1.NameType = "1";
                                        sb.Append(nm1.ToX12String());
                                        SegmentCount++;
                                        if (claimsis.Where(x => x.LoopName == "2330C" && x.RepeatSequence == prv.RepeatSequence).Count() > 0)
                                        {
                                            rref = new REF();
                                            foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2330C" && x.RepeatSequence == prv.RepeatSequence))
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
                                    nm1.NameType = "2";
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
                                    nm1.NameType = "1";
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
                                    nm1.NameType = "2";
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
                            }
                        }
                        foreach (ServiceLine line in claimlines.OrderBy(x => int.Parse(x.ServiceLineNumber)))
                        {
                            LX lx = new LX();
                            lx.ServiceLineNumber = line.ServiceLineNumber;
                            sb.Append(lx.ToX12String());
                            SegmentCount++;
                            SV1 sv1 = new SV1();
                            sv1.ServiceIDQualifier = line.ServiceIDQualifier;
                            sv1.ProcedureCode = line.ProcedureCode;
                            sv1.ProcedureModifier1 = line.ProcedureModifier1;
                            sv1.ProcedureModifier2 = line.ProcedureModifier2;
                            sv1.ProcedureModifier3 = line.ProcedureModifier3;
                            sv1.ProcedureModifier4 = line.ProcedureModifier4;
                            sv1.ProcedureDescription = line.ProcedureDescription;
                            sv1.LineItemChargeAmount = line.LineItemChargeAmount;
                            sv1.LineItemUnit = line.LineItemUnit;
                            sv1.ServiceUnitQuantity = line.ServiceUnitQuantity;
                            sv1.LineItemPOS = line.LineItemPOS;
                            sv1.DiagPointer1 = line.DiagPointer1;
                            sv1.DiagPointer2 = line.DiagPointer2;
                            sv1.DiagPointer3 = line.DiagPointer3;
                            sv1.DiagPointer4 = line.DiagPointer4;
                            sv1.EmergencyIndicator = line.EmergencyIndicator;
                            sv1.EPSDTIndicator = line.EPSDTIndicator;
                            sv1.FamilyPlanningIndicator = line.FamilyPlanningIndicator;
                            sv1.CopayStatusCode = line.CopayStatusCode;
                            sb.Append(sv1.ToX12String());
                            SegmentCount++;
                            if (!string.IsNullOrEmpty(line.DMEDays) && !string.IsNullOrEmpty(line.DMERentalPrice) && !string.IsNullOrEmpty(line.DMEPurchasePrice) && !string.IsNullOrEmpty(line.DMEFrequencyCode))
                            {
                                SV5 sv5 = new SV5();
                                sv5.DMEQualifier = line.DMEQualifier;
                                sv5.DMEProcedureCode = line.DMEProcedureCode;
                                sv5.DMEDays = line.DMEDays;
                                sv5.DMERentalPrice = line.DMERentalPrice;
                                sv5.DMEPurchasePrice = line.DMEPurchasePrice;
                                sv5.DMEFrequencyCode = line.DMEFrequencyCode;
                                sb.Append(sv5.ToX12String());
                                SegmentCount++;
                            }
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
                            if (!string.IsNullOrEmpty(line.AmbulanceTransportReasonCode) && !string.IsNullOrEmpty(line.TransportDistance))
                            {
                                CR1 cr1 = new CR1();
                                cr1.AmbulanceWeight = line.PatientWeight;
                                cr1.AmbulanceReasonCode = line.AmbulanceTransportReasonCode;
                                cr1.AmbulanceQuantity = line.TransportDistance;
                                cr1.AmbulanceRoundTripDescription = line.RoundTripPurposeDescription;
                                cr1.AmbulanceStretcherDescription = line.StretcherPueposeDescription;
                                sb.Append(cr1.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(line.CertificationTypeCode) && !string.IsNullOrEmpty(line.DMEDuration))
                            {
                                CR3 cr3 = new CR3();
                                cr3.CertificationTypeCode = line.CertificationTypeCode;
                                cr3.DMEDuration = line.DMEDuration;
                                sb.Append(cr3.ToX12String());
                                SegmentCount++;
                            }
                            if (claimcrcs.Where(x => x.ServiceLineNumber == line.ServiceLineNumber).Count() > 0)
                            {
                                foreach (ClaimCRC claimcrc in claimcrcs.Where(x => x.ServiceLineNumber == line.ServiceLineNumber))
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
                            DTP dtp = new DTP();
                            dtp.DateCode = "472";
                            dtp.DateQualifier = string.IsNullOrEmpty(line.ServiceToDate) ? "D8" : "RD8";
                            dtp.StartDate = line.ServiceFromDate;
                            dtp.EndDate = line.ServiceToDate;
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                            if (!string.IsNullOrEmpty(line.PrescriptionDate))
                            {
                                dtp = new DTP();
                                dtp.DateCode = "471";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = line.PrescriptionDate;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(line.CertificationDate))
                            {
                                dtp = new DTP();
                                dtp.DateCode = "607";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = line.CertificationDate;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(line.BeginTherapyDate))
                            {
                                dtp = new DTP();
                                dtp.DateCode = "463";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = line.BeginTherapyDate;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(line.LastCertificationDate))
                            {
                                dtp = new DTP();
                                dtp.DateCode = "461";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = line.LastCertificationDate;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(line.LastSeenDate))
                            {
                                dtp = new DTP();
                                dtp.DateCode = "304";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = line.LastSeenDate;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(line.TestDateHemo))
                            {
                                dtp = new DTP();
                                dtp.DateCode = "738";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = line.TestDateHemo;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(line.TestDateSerum))
                            {
                                dtp = new DTP();
                                dtp.DateCode = "739";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = line.TestDateSerum;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(line.ShippedDate))
                            {
                                dtp = new DTP();
                                dtp.DateCode = "011";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = line.ShippedDate;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(line.LastXrayDate))
                            {
                                dtp = new DTP();
                                dtp.DateCode = "455";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = line.LastXrayDate;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(line.InitialTreatmentDate))
                            {
                                dtp = new DTP();
                                dtp.DateCode = "454";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = line.InitialTreatmentDate;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(line.AmbulancePatientCount))
                            {
                                QTY qty = new QTY();
                                qty.AmbulancePatientCount = line.AmbulancePatientCount;
                                sb.Append(qty.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(line.ObstetricAdditionalUnits))
                            {
                                QTY qty = new QTY();
                                qty.ObstetricAdditionalUnits = line.ObstetricAdditionalUnits;
                                sb.Append(qty.ToX12String());
                                SegmentCount++;
                            }
                            if (claimmeas.Where(x => x.ServiceLineNumber == line.ServiceLineNumber).Count() > 0)
                            {
                                foreach (ClaimLineMEA claimlinemea in claimmeas.Where(x => x.ServiceLineNumber == line.ServiceLineNumber))
                                {
                                    MEA mea = new MEA();
                                    mea.TestCode = claimlinemea.TestCode;
                                    mea.MeasurementQualifier = claimlinemea.MeasurementQualifier;
                                    mea.TestResult = claimlinemea.TestResult;
                                    sb.Append(mea.ToX12String());
                                    SegmentCount++;
                                }
                            }
                            if (!string.IsNullOrEmpty(line.ContractTypeCode))
                            {
                                CN1 cn1 = new CN1();
                                cn1.ContractTypeCode = line.ContractTypeCode;
                                cn1.ContractAmount = line.ContractAmount;
                                cn1.ContractPercentage = line.ContractPercentage;
                                cn1.ContractCode = line.ContractCode;
                                cn1.ContractTermsDiscountPercentage = line.TermsDiscountPercentage;
                                cn1.ContractVersionIdentifier = line.ContractVersionIdentifier;
                                sb.Append(cn1.ToX12String());
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
                            if (!string.IsNullOrEmpty(line.SalesTaxAmount))
                            {
                                AMT amt = new AMT();
                                amt.AmountQualifier = "T";
                                amt.Amount = line.SalesTaxAmount;
                                sb.Append(amt.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(line.PostageClaimedAmount))
                            {
                                AMT amt = new AMT();
                                amt.AmountQualifier = "F4";
                                amt.Amount = line.PostageClaimedAmount;
                                sb.Append(amt.ToX12String());
                                SegmentCount++;
                            }
                            if (claimk3s.Where(x => x.ServiceLineNumber == line.ServiceLineNumber).Count() > 0)
                            {
                                K3 k3 = new K3();
                                foreach (ClaimK3 claimk3 in claimk3s.Where(x => x.ServiceLineNumber == line.ServiceLineNumber))
                                {
                                    k3.K3s.Add(claimk3.K3);
                                }
                                sb.Append(k3.ToX12String());
                                SegmentCount += k3.K3s.Count;
                            }
                            if (claimnotes.Where(x => x.ServiceLineNumber == line.ServiceLineNumber).Count() > 0)
                            {
                                foreach (ClaimNte nte in claimnotes.Where(x => x.ServiceLineNumber == line.ServiceLineNumber))
                                {
                                    NTE note = new NTE();
                                    note.NoteCode = nte.NoteCode;
                                    note.NoteText = nte.NoteText;
                                    sb.Append(note.ToX12String());
                                    SegmentCount++;
                                }
                            }
                            if (!string.IsNullOrEmpty(line.PurchasedServiceProviderIdentifier) && !string.IsNullOrEmpty(line.PurchasedServiceChargeAmount))
                            {
                                PS1 ps1 = new PS1();
                                ps1.PurchasedServiceProviderIdentifier = line.PurchasedServiceProviderIdentifier;
                                ps1.PurchasedServiceChargeAmount = line.PurchasedServiceChargeAmount;
                                sb.Append(ps1.ToX12String());
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
                                if (!string.IsNullOrEmpty(claimprovider.ProviderTaxonomyCode))
                                {
                                    PRV prv = new PRV();
                                    prv.ProviderQualifier = "PE";
                                    prv.ProviderTaxonomyCode = claimprovider.ProviderTaxonomyCode;
                                    sb.Append(prv.ToX12String());
                                    SegmentCount++;
                                }
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
                                nm1.NameType = "2";
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
                            claimprovider = claimproviders.Where(x => x.LoopName == "2420E" && x.ServiceLineNumber == line.ServiceLineNumber).FirstOrDefault();
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
                                if (claimsis.Where(x => x.LoopName == "2420E" && x.ServiceLineNumber == line.ServiceLineNumber).Count() > 0)
                                {
                                    rref = new REF();
                                    foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2420E" && x.ServiceLineNumber == line.ServiceLineNumber))
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
                                ProviderContact claimpc = claimpcs.Where(x => x.LoopName == "2420E").FirstOrDefault();
                                if (claimpc != null)
                                {
                                    per = new PER();
                                    peritem = new PERItem();
                                    peritem.ContactName = claimpc.ContactName;
                                    peritem.Phone = claimpc.Phone;
                                    peritem.Email = claimpc.Email;
                                    peritem.Fax = claimpc.Fax;
                                    peritem.PhoneEx = claimpc.PhoneEx;
                                    per.Pers.Add(peritem);
                                    sb.Append(per.ToX12String());
                                    SegmentCount += per.Pers.Count;
                                }
                            }
                            List<ClaimProvider> loopproviders = claimproviders.Where(x => x.LoopName == "2420F" && x.ServiceLineNumber == line.ServiceLineNumber).Take(2).ToList();
                            if (loopproviders.Count > 0)
                            {
                                foreach (ClaimProvider loopprovider in loopproviders)
                                {
                                    nm1 = new NM1();
                                    nm1.NameQualifier = loopprovider.ProviderQualifier;
                                    nm1.NameType = string.IsNullOrEmpty(loopprovider.ProviderFirstName) ? "2" : "1";
                                    nm1.LastName = loopprovider.ProviderLastName;
                                    nm1.FirstName = loopprovider.ProviderFirstName;
                                    nm1.MiddleName = loopprovider.ProviderMiddle;
                                    nm1.Suffix = loopprovider.ProviderSuffix;
                                    nm1.IDQualifer = loopprovider.ProviderIDQualifier;
                                    nm1.IDCode = loopprovider.ProviderID;
                                    sb.Append(nm1.ToX12String());
                                    SegmentCount++;
                                    if (claimsis.Where(x => x.LoopName == "2420F" && x.ServiceLineNumber == line.ServiceLineNumber && x.RepeatSequence == claimprovider.RepeatSequence).Count() > 0)
                                    {
                                        rref = new REF();
                                        foreach (ClaimSecondaryIdentification si in claimsis.Where(x => x.LoopName == "2420F" && x.ServiceLineNumber == line.ServiceLineNumber && x.RepeatSequence == claimprovider.RepeatSequence))
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
                            }
                            claimprovider = claimproviders.Where(x => x.LoopName == "2420G" && x.ServiceLineNumber == line.ServiceLineNumber).FirstOrDefault();
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
                                n4 = new N4();
                                n4.City = claimprovider.ProviderCity;
                                n4.State = claimprovider.ProviderState;
                                n4.Zipcode = claimprovider.ProviderZip;
                                n4.Country = claimprovider.ProviderCountry;
                                n4.CountrySubCode = claimprovider.ProviderCountrySubCode;
                                sb.Append(n4.ToX12String());
                                SegmentCount++;
                            }
                            claimprovider = claimproviders.Where(x => x.LoopName == "2420H" && x.ServiceLineNumber == line.ServiceLineNumber).FirstOrDefault();
                            if (claimprovider != null)
                            {
                                nm1 = new NM1();
                                nm1.NameQualifier = claimprovider.ProviderQualifier;
                                nm1.NameType = "2";
                                nm1.LastName = claimprovider.ProviderLastName;
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
                                    dtp = new DTP();
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
                            if (claimlqs.Where(x => x.ServiceLineNumber == line.ServiceLineNumber).Count() > 0)
                            {
                                LQ lq = new LQ();
                                foreach (ClaimLineLQ claimlq in claimlqs.Where(x => x.ServiceLineNumber == line.ServiceLineNumber))
                                {
                                    LQItem lqitem = new LQItem();
                                    lqitem.FormQualifier = claimlq.FormQualifier;
                                    lqitem.IndustryCode = claimlq.IndustryCode;
                                    lqitem.LQSequence = claimlq.LQSequence;
                                    lq.LQs.Add(lqitem);
                                }
                                foreach (ClaimLineFRM claimfrm in claimfrms.Where(x => x.ServiceLineNumber == line.ServiceLineNumber))
                                {
                                    FRMItem frmitem = new FRMItem();
                                    frmitem.LQSequence = claimfrm.LQSequence;
                                    frmitem.QuestionNumber = claimfrm.QuestionNumber;
                                    frmitem.QuestionResponseIndicator = claimfrm.QuestionResponseIndicator;
                                    frmitem.QuestionResponse = claimfrm.QuestionResponse;
                                    frmitem.QuestionResponseDate = claimfrm.QuestionResponseDate;
                                    frmitem.AllowedChargePercentage = claimfrm.AllowedChargePercentage;
                                    lq.FRMs.Add(frmitem);
                                }
                                sb.Append(lq.ToX12String());
                                SegmentCount += lq.LQs.Count + lq.FRMs.Count;
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
                    File.WriteAllText(Path.Combine(DestinationFolder, "IEHP_" + CountyCode + "_837P_MCE_" + DateTime.Today.ToString("yyyyMMdd") + "_" + (i + 1).ToString().PadLeft(5, '1') + ".dat"), sb.ToString());
                }
            }
            context.Dispose();
        }
    }
}
