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
        public static void Export837D(string CountyCode)
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
                List<ClaimHeader> headers = context.ClaimHeaders.Where(x => x.ClaimID.StartsWith(CountyCode)).OrderBy(x => x.ID).Skip(i * 5000).Take(5000).ToList();
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
                    List<ToothStatus> toothStatuses = context.Database.SqlQuery<ToothStatus>("select * from Sub_History.ToothStatus where ClaimID in (select ClaimID from Sub_History.ClaimTempHost)").ToList();
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
                        List<ToothStatus> tooStatuses = toothStatuses.Where(x => x.ClaimID == header.ClaimID).ToList();

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
                        if (!string.IsNullOrEmpty(header.AccidentDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "439";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = header.AccidentDate;
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.AppliancePlacementDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "452";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = header.AppliancePlacementDate;
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                        }
                        if (!string.IsNullOrEmpty(header.ServiceFromDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "472";
                            dtp.DateQualifier = string.IsNullOrEmpty(header.ServiceToDate) ? "D8" : "RD8";
                            dtp.StartDate = header.ServiceFromDate;
                            dtp.EndDate = header.ServiceToDate;
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
                        DN1 dn1 = new DN1();
                        dn1.OrthoMonthTotal = header.OrthoMonthTotal;
                        dn1.OrthoMonthRemaining = header.OrthoMonthRemaining;
                        sb.Append(dn1.ToX12String());
                        SegmentCount++;
                        if (tooStatuses.Count(x => x.ServiceLineNumber == "0") > 0)
                        {
                            DN2 dn2 = new DN2();
                            foreach (ToothStatus too in tooStatuses.Where(x => x.ServiceLineNumber == "0"))
                            {
                                DN2Item item = new DN2Item();
                                item.ToothNumber = too.ToothNumber;
                                item.StatusCode = too.StatusCode;
                                dn2.DN2Items.Add(item);
                            }
                            sb.Append(dn2.ToX12String());
                            SegmentCount += dn2.DN2Items.Count;
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
                        if (claimsis.Count(x => x.LoopName == "2300") > 0)
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
                        if (claimproviders.Count(x => x.LoopName == "2310A") > 0)
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
                            if (claimsis.Count(x => x.LoopName == "2310B") > 0)
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
                            if (claimsis.Count(x => x.LoopName == "2310C") > 0)
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
                            if (!string.IsNullOrEmpty(claimprovider.ProviderTaxonomyCode))
                            {
                                PRV prv = new PRV();
                                prv.ProviderQualifier = "PE";
                                prv.ProviderTaxonomyCode = claimprovider.ProviderTaxonomyCode;
                                sb.Append(prv.ToX12String());
                                SegmentCount++;
                            }
                            if (claimsis.Count(x => x.LoopName == "2310D") > 0)
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
                            nm1.NameType = string.IsNullOrEmpty(claimprovider.ProviderFirstName) ? "2" : "1";
                            nm1.LastName = claimprovider.ProviderLastName;
                            nm1.FirstName = claimprovider.ProviderFirstName;
                            nm1.MiddleName = claimprovider.ProviderMiddle;
                            nm1.Suffix = claimprovider.ProviderSuffix;
                            nm1.IDQualifer = claimprovider.ProviderIDQualifier;
                            nm1.IDCode = claimprovider.ProviderID;
                            sb.Append(nm1.ToX12String());
                            SegmentCount++;
                            if (claimsis.Count(x => x.LoopName == "2310E") > 0)
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
                                claimprovider = claimproviders.Where(x => x.LoopName == "2330H" && x.RepeatSequence == sbr1.SubscriberSequenceNumber).FirstOrDefault();
                                if (claimprovider != null)
                                {
                                    nm1 = new NM1();
                                    nm1.NameQualifier = claimprovider.ProviderQualifier;
                                    nm1.NameType = "2";
                                    sb.Append(nm1.ToX12String());
                                    SegmentCount++;
                                    if (claimsis.Count(x => x.LoopName == "2330H" && x.RepeatSequence == sbr1.SubscriberSequenceNumber) > 0)
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
                            }
                        }
                        foreach (ServiceLine line in claimlines.OrderBy(x => int.Parse(x.ServiceLineNumber)))
                        {
                            LX lx = new LX();
                            lx.ServiceLineNumber = line.ServiceLineNumber;
                            sb.Append(lx.ToX12String());
                            SegmentCount++;
                            SV3 sv3 = new SV3();
                            sv3.ServiceIDQualifier = line.ServiceIDQualifier;
                            sv3.ProcedureCode = line.ProcedureCode;
                            sv3.ProcedureModifier1 = line.ProcedureModifier1;
                            sv3.ProcedureModifier2 = line.ProcedureModifier2;
                            sv3.ProcedureModifier3 = line.ProcedureModifier3;
                            sv3.ProcedureModifier4 = line.ProcedureModifier4;
                            sv3.ProcedureDescription = line.ProcedureDescription;
                            sv3.LineItemChargeAmount = line.LineItemChargeAmount;
                            sv3.ServiceUnitQuantity = line.ServiceUnitQuantity;
                            sv3.LineItemPOS = line.LineItemPOS;
                            sv3.DiagPointer1 = line.DiagPointer1;
                            sv3.DiagPointer2 = line.DiagPointer2;
                            sv3.DiagPointer3 = line.DiagPointer3;
                            sv3.DiagPointer4 = line.DiagPointer4;
                            sv3.OralCavityDesignationCode1 = line.OralCavityDesignationCode1;
                            sv3.OralCavityDesignationCode2 = line.OralCavityDesignationCode2;
                            sv3.OralCavityDesignationCode3 = line.OralCavityDesignationCode3;
                            sv3.OralCavityDesignationCode4 = line.OralCavityDesignationCode4;
                            sv3.OralCavityDesignationCode5 = line.OralCavityDesignationCode5;
                            sv3.ProsthesisCrownOrInlayCode = line.ProsthesisCrownOrInlayCode;

                            sb.Append(sv3.ToX12String());
                            SegmentCount++;
                            if (tooStatuses.Count(x => x.ServiceLineNumber == line.ServiceLineNumber) > 0)
                            {
                                TOO too = new TOO();
                                foreach (ToothStatus tooStatus in tooStatuses)
                                {
                                    TooItem item = new TooItem();
                                    item.ToothNumber = tooStatus.ToothNumber;
                                    item.StatusCode = tooStatus.StatusCode;
                                    item.SurfaceCode2 = tooStatus.SurfaceCode2;
                                    item.SurfaceCode3 = tooStatus.SurfaceCode3;
                                    item.SurfaceCode4 = tooStatus.SurfaceCode4;
                                    item.SurfaceCode5 = tooStatus.SurfaceCode5;
                                    too.TooItems.Add(item);
                                }
                                sb.Append(too.ToX12String());
                                SegmentCount += too.TooItems.Count;
                            }
                            DTP dtp = new DTP();
                            dtp.DateCode = "472";
                            dtp.DateQualifier = "RD8";
                            dtp.StartDate = line.ServiceFromDate;
                            dtp.EndDate = line.ServiceToDate;
                            sb.Append(dtp.ToX12String());
                            SegmentCount++;
                            if (!string.IsNullOrEmpty(line.PriorPlacementDate))
                            {
                                dtp = new DTP();
                                dtp.DateCode = "441";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = line.PriorPlacementDate;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(line.AppliancePlacementDate))
                            {
                                dtp = new DTP();
                                dtp.DateCode = "452";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = line.AppliancePlacementDate;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(line.ReplacementDate))
                            {
                                dtp = new DTP();
                                dtp.DateCode = "446";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = line.ReplacementDate;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(line.TreatmentStartDate))
                            {
                                dtp = new DTP();
                                dtp.DateCode = "196";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = line.TreatmentStartDate;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
                            }
                            if (!string.IsNullOrEmpty(line.TreatmentCompletionDate))
                            {
                                dtp = new DTP();
                                dtp.DateCode = "198";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = line.TreatmentCompletionDate;
                                sb.Append(dtp.ToX12String());
                                SegmentCount++;
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
                            if (claimsvds.Count(x => x.ServiceLineNumber == line.ServiceLineNumber) > 0)
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
