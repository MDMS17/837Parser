using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser837;
using ExportModule.Model.DataSource;
using System.Xml.Linq;

namespace ExportModule
{
    public static partial class LoadData
    {
        private static void Recursive_Search_Value(XElement root, ref StringBuilder sb)
        {
            foreach (XElement ele in root.Descendants())
            {
                if (ele.Descendants().Count() == 0 && ele.Value.Length > 50) sb.AppendLine(ele.Name.ToString() + "------" + ele.Value + "------" + ele.Value.Length.ToString());
                else Recursive_Search_Value(ele, ref sb);
            }

        }
        private static void Recursive_Search_Name(XElement root, ref HashSet<string> hs)
        {
            XNamespace ns = "http://schemas.microsoft.com/BizTalk/EDI/X12/2006";
            foreach (XElement ele in root.Descendants())
            {
                if (ele.Name.Namespace == ns && !hs.Contains(ele.Name.ToString())) hs.Add(ele.Name.ToString());
                if (ele.Descendants().Count() > 0)
                {
                    Recursive_Search_Name(ele, ref hs);
                }
            }
        }
        public static void SubHist_P305()
        {
            Claim claim;
            List<Claim> claims = new List<Claim>();
            var context = new SHRContext();
            //StringBuilder sb = new StringBuilder();
            foreach (Hipaa_XML hipaaclaim in context.Hipaa_XML.Where(x => x.ClaimType == "P"))
            {

                XDocument xdoc = XDocument.Parse(hipaaclaim.ClaimHipaaXML);
                //Recursive_Search_Value(xdoc.Root, ref sb);


                claim = new Claim();
                claim.Header.ClaimID = hipaaclaim.ClaimID;
                claim.Header.FileID = 0;
                XNamespace ns = "http://schemas.microsoft.com/BizTalk/EDI/X12/2006";
                //XElement st = xdoc.Descendants("ST").FirstOrDefault();
                //XElement bht = xdoc.Descendants(ns + "BHT_BeginningofHierarchicalTransaction").FirstOrDefault();
                //XElement loop1000A = xdoc.Descendants(ns + "NM1_SubLoop").FirstOrDefault().Descendants(ns + "TS837_1000A_Loop").FirstOrDefault();
                //XElement loop1000B = xdoc.Descendants(ns + "NM1_SubLoop").FirstOrDefault().Descendants(ns + "TS837_1000B_Loop").FirstOrDefault();
                ClaimProvider provider = new ClaimProvider();
                provider.ClaimID = hipaaclaim.ClaimID;
                provider.FileID = 0;
                provider.ServiceLineNumber = "0";
                provider.LoopName = "2000A";
                XElement loop2000A = xdoc.Descendants(ns + "TS837_2000A_Loop").FirstOrDefault();
                XElement BillProvPRV = loop2000A.Descendants(ns + "PRV_BillingProviderSpecialtyInformation").FirstOrDefault();
                if (BillProvPRV != null)
                {
                    foreach (XElement ele in BillProvPRV.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("PRV03"))
                        {
                            provider.ProviderTaxonomyCode = ele.Value;
                        }
                    }
                }
                XElement loop2010AA = loop2000A.Descendants(ns + "NM1_SubLoop_2").FirstOrDefault().Descendants(ns + "TS837_2010AA_Loop").FirstOrDefault();
                XElement BillProvNM1 = loop2010AA.Descendants(ns + "NM1_BillingProviderName").FirstOrDefault();
                foreach (XElement ele in BillProvNM1.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM104")) provider.ProviderFirstName = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM105")) provider.ProviderMiddle = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM107")) provider.ProviderSuffix = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                }
                XElement BillProvN3 = loop2010AA.Descendants(ns + "N3_BillingProviderAddress").FirstOrDefault();
                foreach (XElement ele in BillProvN3.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("N301")) provider.ProviderAddress = ele.Value;
                    if (ele.Name.ToString().StartsWith("N302")) provider.ProviderAddress2 = ele.Value;
                }
                XElement BillProvN4 = loop2010AA.Descendants(ns + "N4_BillingProviderCity_State_ZIPCode").FirstOrDefault();
                foreach (XElement ele in BillProvN4.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("N401")) provider.ProviderCity = ele.Value;
                    if (ele.Name.ToString().StartsWith("N402")) provider.ProviderState = ele.Value;
                    if (ele.Name.ToString().StartsWith("N403")) provider.ProviderZip = ele.Value;
                    if (ele.Name.ToString().StartsWith("N404")) provider.ProviderCountry = ele.Value;
                    if (ele.Name.ToString().StartsWith("N407")) provider.ProviderCountrySubCode = ele.Value;
                }
                XElement BillProvSecondaryIdentifications = loop2010AA.Descendants(ns + "REF_SubLoop").FirstOrDefault();
                claim.Providers.Add(provider);
                foreach (XElement ele in BillProvSecondaryIdentifications.Nodes())
                {
                    ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                    si.ClaimID = hipaaclaim.ClaimID;
                    si.FileID = 0;
                    si.ServiceLineNumber = "0";
                    si.LoopName = "2010AA";
                    foreach (XElement child_ele in ele.Descendants())
                    {
                        if (child_ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = child_ele.Value;
                        if (child_ele.Name.ToString().StartsWith("REF02")) si.ProviderID = child_ele.Value;
                    }
                    claim.SecondaryIdentifications.Add(si);
                }
                foreach (XElement BillProvPER in loop2010AA.Descendants(ns + "PER_BillingProviderContactInformation"))
                {
                    ProviderContact pc = new ProviderContact();
                    pc.ClaimID = hipaaclaim.ClaimID;
                    pc.FileID = 0;
                    pc.ServiceLineNumber = "0";
                    pc.LoopName = "2000A";
                    foreach (XElement ele in BillProvPER.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("PER02")) pc.ContactName = ele.Value;
                        if (ele.Name.ToString().StartsWith("PER04"))
                        {
                            switch (((XElement)ele.PreviousNode).Value)
                            {
                                case "TE":
                                    pc.Phone = ele.Value;
                                    break;
                                case "EM":
                                    pc.Email = ele.Value;
                                    break;
                                case "FX":
                                    pc.Fax = ele.Value;
                                    break;
                            }
                        }
                        if (ele.Name.ToString().StartsWith("PER06"))
                        {
                            switch (((XElement)ele.PreviousNode).Value)
                            {
                                case "TE":
                                    pc.Phone = ele.Value;
                                    break;
                                case "EM":
                                    pc.Email = ele.Value;
                                    break;
                                case "FX":
                                    pc.Fax = ele.Value;
                                    break;
                                case "EX":
                                    pc.PhoneEx = ele.Value;
                                    break;
                            }
                        }
                        if (ele.Name.ToString().StartsWith("PER08"))
                        {
                            switch (((XElement)ele.PreviousNode).Value)
                            {
                                case "TE":
                                    pc.Phone = ele.Value;
                                    break;
                                case "EM":
                                    pc.Email = ele.Value;
                                    break;
                                case "FX":
                                    pc.Fax = ele.Value;
                                    break;
                                case "EX":
                                    pc.PhoneEx = ele.Value;
                                    break;
                            }
                        }
                    }
                    claim.ProviderContacts.Add(pc);
                }
                XElement loop2010AB = loop2000A.Descendants(ns + "NM1_SubLoop_2").FirstOrDefault().Descendants(ns + "TS837_2010AB_Loop").FirstOrDefault();
                if (loop2010AB != null)
                {
                    provider = new ClaimProvider();
                    provider.ClaimID = hipaaclaim.ClaimID;
                    provider.FileID = 0;
                    provider.ServiceLineNumber = "0";
                    provider.LoopName = "2010AB";
                    XElement loop2010ABNM1 = loop2010AB.Descendants(ns + "NM1_Pay_toAddressName").FirstOrDefault();
                    if (loop2010ABNM1 != null)
                    {
                        foreach (XElement ele in loop2010ABNM1.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM104")) provider.ProviderFirstName = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM105")) provider.ProviderMiddle = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM107")) provider.ProviderSuffix = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                        }
                    }
                    XElement loop2010ABN3 = loop2010AB.Descendants(ns + "N3_Pay_ToAddress_ADDRESS").FirstOrDefault();
                    if (loop2010ABN3 != null)
                    {
                        foreach (XElement ele in loop2010ABN3.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N301")) provider.ProviderAddress = ele.Value;
                            if (ele.Name.ToString().StartsWith("N302")) provider.ProviderAddress2 = ele.Value;
                        }
                    }
                    XElement loop2010ABN4 = loop2010AB.Descendants(ns + "N4_Pay_toAddressCity_State_ZIPCode").FirstOrDefault();
                    if (loop2010ABN4 != null)
                    {
                        foreach (XElement ele in loop2010ABN4.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N401")) provider.ProviderCity = ele.Value;
                            if (ele.Name.ToString().StartsWith("N402")) provider.ProviderState = ele.Value;
                            if (ele.Name.ToString().StartsWith("N403")) provider.ProviderZip = ele.Value;
                            if (ele.Name.ToString().StartsWith("N404")) provider.ProviderCountry = ele.Value;
                            if (ele.Name.ToString().StartsWith("N407")) provider.ProviderCountrySubCode = ele.Value;
                        }
                    }
                    claim.Providers.Add(provider);
                }
                ClaimSBR subscriber = new ClaimSBR();
                subscriber.ClaimID = hipaaclaim.ClaimID;
                subscriber.FileID = 0;
                subscriber.LoopName = "2000B";
                XElement loop2000B = loop2000A.Descendants(ns + "TS837_2000B_Loop").FirstOrDefault();
                XElement Subscriber = loop2000B.Descendants(ns + "SBR_SubscriberInformation").FirstOrDefault();
                foreach (XElement ele in Subscriber.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("SBR01")) subscriber.SubscriberSequenceNumber = ele.Value;
                    if (ele.Name.ToString().StartsWith("SBR02")) subscriber.SubscriberRelationshipCode = ele.Value;
                    if (ele.Name.ToString().StartsWith("SBR03")) subscriber.InsuredGroupNumber = ele.Value;
                    if (ele.Name.ToString().StartsWith("SBR04")) subscriber.OtherInsuredGroupName = ele.Value;
                    if (ele.Name.ToString().StartsWith("SBR05")) subscriber.InsuredTypeCode = ele.Value;
                    if (ele.Name.ToString().StartsWith("SBR09")) subscriber.ClaimFilingCode = ele.Value;
                }
                XElement subscriberPAT = loop2000B.Descendants(ns + "PAT_PatientInformation").FirstOrDefault();
                if (subscriberPAT != null)
                {
                    foreach (XElement ele in subscriberPAT.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("PAT06")) subscriber.DeathDate = ele.Value;
                        if (ele.Name.ToString().StartsWith("PAT08")) subscriber.Weight = ele.Value;
                        if (ele.Name.ToString().StartsWith("PAT09")) subscriber.PregnancyIndicator = ele.Value;
                    }
                }
                XElement loop2010BA = loop2000A.Descendants(ns + "NM1_SubLoop_3").FirstOrDefault().Descendants(ns + "TS837_2010BA_Loop").FirstOrDefault();
                XElement subscriberNM1 = loop2010BA.Descendants(ns + "NM1_SubscriberName").FirstOrDefault();
                foreach (XElement ele in subscriberNM1.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("NM103")) subscriber.LastName = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM104")) subscriber.FirstName = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM105")) subscriber.MidddleName = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM107")) subscriber.NameSuffix = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM108")) subscriber.IDQualifier = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM109")) subscriber.IDCode = ele.Value;
                }
                XElement subscriberN3 = loop2010BA.Descendants(ns + "N3_SubscriberAddress").FirstOrDefault();
                if (subscriberN3 != null)
                {
                    foreach (XElement ele in subscriberN3.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("N301")) subscriber.SubscriberAddress = ele.Value;
                        if (ele.Name.ToString().StartsWith("N302")) subscriber.SubscriberAddress2 = ele.Value;
                    }
                }
                XElement subscriberN4 = loop2010BA.Descendants(ns + "N4_SubscriberCity_State_ZIPCode").FirstOrDefault();
                if (subscriberN4 != null)
                {
                    foreach (XElement ele in subscriberN4.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("N401")) subscriber.SubscriberCity = ele.Value;
                        if (ele.Name.ToString().StartsWith("N402")) subscriber.SubscriberState = ele.Value;
                        if (ele.Name.ToString().StartsWith("N403")) subscriber.SubscriberZip = ele.Value;
                        if (ele.Name.ToString().StartsWith("N404")) subscriber.SubscriberCountry = ele.Value;
                        if (ele.Name.ToString().StartsWith("N407")) subscriber.SubscriberCountrySubCode = ele.Value;
                    }
                }
                XElement subscriberDMG = loop2010BA.Descendants(ns + "DMG_SubscriberDemographicInformation").FirstOrDefault();
                if (subscriberDMG != null)
                {
                    foreach (XElement ele in subscriberDMG.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("DMG02")) subscriber.SubscriberBirthDate = ele.Value;
                        if (ele.Name.ToString().StartsWith("DMG03")) subscriber.SubscriberGender = ele.Value;
                    }
                }
                XElement subscriberREF = loop2010BA.Descendants(ns + "REF_SubLoop_3").FirstOrDefault();
                if (subscriberREF != null)
                {
                    foreach (XElement ele in subscriberREF.Nodes())
                    {
                        ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                        si.ClaimID = hipaaclaim.ClaimID;
                        si.FileID = 0;
                        si.ServiceLineNumber = "0";
                        si.LoopName = "2010BA";
                        foreach (XElement child_ele in ele.Descendants())
                        {
                            if (child_ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = child_ele.Value;
                            if (child_ele.Name.ToString().StartsWith("REF02")) si.ProviderID = child_ele.Value;
                        }
                        claim.SecondaryIdentifications.Add(si);
                    }
                }
                claim.Subscribers.Add(subscriber);
                provider = new ClaimProvider();
                provider.ClaimID = hipaaclaim.ClaimID;
                provider.FileID = 0;
                provider.ServiceLineNumber = "0";
                provider.LoopName = "2010BB";
                XElement loop2010BB = loop2000A.Descendants(ns + "NM1_SubLoop_3").FirstOrDefault().Descendants(ns + "TS837_2010BB_Loop").FirstOrDefault();
                XElement subscriberPayerNM1 = loop2010BB.Descendants(ns + "NM1_PayerName").FirstOrDefault();
                foreach (XElement ele in subscriberPayerNM1.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                }
                XElement subscriberPayerN3 = loop2010BB.Descendants(ns + "N3_PayerAddress").FirstOrDefault();
                if (subscriberPayerN3 != null)
                {
                    foreach (XElement ele in subscriberPayerN3.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("N301")) provider.ProviderAddress = ele.Value;
                        if (ele.Name.ToString().StartsWith("N302")) provider.ProviderAddress2 = ele.Value;
                    }
                }
                XElement subscriberPayerN4 = loop2010BB.Descendants(ns + "N4_PayerCity_StatE_ZIPCode").FirstOrDefault();
                if (subscriberPayerN4 != null)
                {
                    foreach (XElement ele in subscriberPayerN4.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("N401")) provider.ProviderCity = ele.Value;
                        if (ele.Name.ToString().StartsWith("N402")) provider.ProviderState = ele.Value;
                        if (ele.Name.ToString().StartsWith("N403")) provider.ProviderZip = ele.Value;
                        if (ele.Name.ToString().StartsWith("N404")) provider.ProviderCountry = ele.Value;
                        if (ele.Name.ToString().StartsWith("N407")) provider.ProviderCountrySubCode = ele.Value;
                    }
                }
                claim.Providers.Add(provider);

                XElement Clm = loop2000B.Descendants(ns + "CLM_ClaimInformation").FirstOrDefault();
                XElement ClmPOS = Clm.Descendants(ns + "C023_HealthCareServiceLocationInformation").FirstOrDefault();
                foreach (XElement ele in Clm.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("CLM02")) claim.Header.ClaimAmount = ele.Value;
                    if (ele.Name.ToString().StartsWith("CLM06")) claim.Header.ClaimProviderSignature = ele.Value;
                    if (ele.Name.ToString().StartsWith("CLM07")) claim.Header.ClaimProviderAssignment = ele.Value;
                    if (ele.Name.ToString().StartsWith("CLM08")) claim.Header.ClaimBenefitAssignment = ele.Value;
                    if (ele.Name.ToString().StartsWith("CLM09")) claim.Header.ClaimReleaseofInformationCode = ele.Value;
                }
                foreach (XElement ele in ClmPOS.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("C02301")) claim.Header.ClaimPOS = ele.Value;
                    if (ele.Name.ToString().StartsWith("C02302")) claim.Header.ClaimType = ele.Value;
                    if (ele.Name.ToString().StartsWith("C02303")) claim.Header.ClaimFrequencyCode = ele.Value;
                }
                XElement CN1 = loop2000B.Descendants(ns + "CN1_ContractInformation").FirstOrDefault();
                if (CN1 != null)
                {
                    foreach (XElement ele in CN1.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("CN101")) claim.Header.ContractTypeCode = ele.Value;
                        if (ele.Name.ToString().StartsWith("CN102")) claim.Header.ContractAmount = ele.Value;
                        if (ele.Name.ToString().StartsWith("CN104")) claim.Header.ContractCode = ele.Value;
                    }
                }
                XElement ClaimSecondaryIdentifications = loop2000B.Descendants(ns + "REF_SubLoop_5").FirstOrDefault();
                if (ClaimSecondaryIdentifications != null)
                {
                    foreach (XElement ele in ClaimSecondaryIdentifications.Nodes())
                    {
                        ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                        si.ClaimID = hipaaclaim.ClaimID;
                        si.FileID = 0;
                        si.ServiceLineNumber = "0";
                        si.LoopName = "2300";
                        foreach (XElement child_ele in ele.Descendants())
                        {
                            if (child_ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = child_ele.Value;
                            if (child_ele.Name.ToString().StartsWith("REF02")) si.ProviderID = child_ele.Value;
                        }
                        claim.SecondaryIdentifications.Add(si);
                    }
                }
                if (claim.SecondaryIdentifications.Where(x => x.LoopName == "2300" && x.ProviderQualifier == "F8").FirstOrDefault() == null)
                {
                    ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                    si.ClaimID = hipaaclaim.ClaimID;
                    si.FileID = 0;
                    si.LoopName = "2300";
                    si.ServiceLineNumber = "0";
                    si.ProviderQualifier = "F8";
                    si.ProviderID = hipaaclaim.ClaimID;
                    claim.SecondaryIdentifications.Add(si);
                }
                XElement note = loop2000B.Descendants(ns + "NTE_ClaimNote").FirstOrDefault();
                if (note != null)
                {
                    ClaimNte nte = new ClaimNte();
                    nte.ClaimID = hipaaclaim.ClaimID;
                    nte.FileID = 0;
                    nte.ServiceLineNumber = "0";
                    foreach (XElement ele in note.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("NTE01")) nte.NoteCode = ele.Value;
                        if (ele.Name.ToString().StartsWith("NTE02")) nte.NoteText = ele.Value;
                    }
                    claim.Notes.Add(nte);
                }
                XElement claimCR1 = loop2000B.Descendants(ns + "CR1_AmbulanceTransportInformation").FirstOrDefault();
                if (claimCR1 != null)
                {
                    foreach (XElement ele in claimCR1.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("CR102")) claim.Header.AmbulanceWeight = ele.Value;
                        if (ele.Name.ToString().StartsWith("CR104")) claim.Header.AmbulanceReasonCode = ele.Value;
                        if (ele.Name.ToString().StartsWith("CR106")) claim.Header.AmbulanceQuantity = ele.Value;
                        if (ele.Name.ToString().StartsWith("CR109")) claim.Header.AmbulanceRoundTripDescription = ele.Value;
                        if (ele.Name.ToString().StartsWith("CR110")) claim.Header.AmbulanceStretcherDescription = ele.Value;
                    }
                }
                XElement claimlevelCRC = loop2000B.Descendants(ns + "CRC_SubLoop").FirstOrDefault();
                if (claimlevelCRC != null)
                {
                    foreach (XElement ele in claimlevelCRC.Nodes())
                    {
                        ClaimCRC claimCRC = new ClaimCRC();
                        claimCRC.ClaimID = hipaaclaim.ClaimID;
                        claimCRC.FileID = 0;
                        claimCRC.ServiceLineNumber = "0";
                        foreach (XElement child_ele in ele.Descendants())
                        {
                            if (child_ele.Name.ToString().StartsWith("CRC01")) claimCRC.CodeCategory = child_ele.Value;
                            if (child_ele.Name.ToString().StartsWith("CRC02")) claimCRC.ConditionIndicator = child_ele.Value;
                            if (child_ele.Name.ToString().StartsWith("CRC03")) claimCRC.ConditionCode = child_ele.Value;
                            if (child_ele.Name.ToString().StartsWith("CRC04")) claimCRC.ConditionCode2 = child_ele.Value;
                            if (child_ele.Name.ToString().StartsWith("CRC05")) claimCRC.ConditionCode3 = child_ele.Value;
                            if (child_ele.Name.ToString().StartsWith("CRC06")) claimCRC.ConditionCode4 = child_ele.Value;
                            if (child_ele.Name.ToString().StartsWith("CRC07")) claimCRC.ConditionCode5 = child_ele.Value;
                        }
                        claim.CRCs.Add(claimCRC);
                    }
                }
                XElement his = loop2000B.Descendants(ns + "HI_SubLoop").FirstOrDefault();
                foreach (XElement ele in his.Nodes())
                {
                    foreach (XElement child_ele in ele.Nodes())
                    {
                        ClaimHI hi = new ClaimHI();
                        hi.ClaimID = hipaaclaim.ClaimID;
                        hi.FileID = 0;
                        foreach (XElement grand_ele in child_ele.Descendants())
                        {
                            if (grand_ele.Name.ToString().StartsWith("C02201")) hi.HIQual = grand_ele.Value;
                            if (grand_ele.Name.ToString().StartsWith("C02202")) hi.HICode = grand_ele.Value;
                        }
                        claim.His.Add(hi);
                    }
                }
                XElement claimDTP = loop2000B.Descendants(ns + "DTP_SubLoop").FirstOrDefault();
                if (claimDTP != null)
                {
                    foreach (XElement ele in claimDTP.Nodes())
                    {
                        foreach (XElement child_ele in ele.Descendants())
                        {
                            if (child_ele.Name.ToString().StartsWith("DTP01"))
                            {
                                switch (child_ele.Value)
                                {
                                    case "304":
                                        claim.Header.LastSeenDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                        break;
                                    case "431":
                                        claim.Header.CurrentIllnessDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                        break;
                                    case "435":
                                        claim.Header.AdmissionDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                        break;
                                    case "439":
                                        claim.Header.AccidentDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                        break;
                                    case "454":
                                        claim.Header.InitialTreatmentDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                        break;
                                    case "471":
                                        claim.Header.PrescriptionDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                        break;
                                    case "484":
                                        claim.Header.LastMenstrualPeriodDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                        break;
                                    case "096":
                                        //institutional discharge hour
                                        claim.Header.DischargeDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                        break;
                                    case "434":
                                        //institutional statement date
                                        claim.Header.StatementFromDate = ((XElement)child_ele.NextNode.NextNode).Value.Split('-')[0];
                                        claim.Header.StatementToDate = ((XElement)child_ele.NextNode.NextNode).Value.Split('-')[1];
                                        break;
                                }
                                break;
                            }
                        }
                    }
                }
                XElement claimAMT = loop2000B.Descendants(ns + "AMT_PatientAmountPaid").FirstOrDefault();
                if (claimAMT != null)
                {
                    foreach (XElement ele in claimAMT.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("AMT02")) claim.Header.PatientPaidAmount = ele.Value;
                    }
                }
                XElement claimlevelPWK = loop2000B.Descendants(ns + "PWK_ClaimSupplementalInformation").FirstOrDefault();
                if (claimlevelPWK != null)
                {
                    ClaimPWK pwk = new ClaimPWK();
                    pwk.ClaimID = hipaaclaim.ClaimID;
                    pwk.FileID = 0;
                    pwk.ServiceLineNumber = "0";
                    foreach (XElement ele in claimlevelPWK.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("PWK01")) pwk.ReportTypeCode = ele.Value;
                        if (ele.Name.ToString().StartsWith("PWK02")) pwk.ReportTransmissionCode = ele.Value;
                    }
                    claim.PWKs.Add(pwk);
                }
                if (loop2000B.Descendants(ns + "NM1_SubLoop_4").Descendants(ns + "TS837_2310A_Loop").Count() > 0)
                {
                    foreach (XElement referProv in loop2000B.Descendants(ns + "NM1_SubLoop_4").Descendants(ns + "TS837_2310A_Loop"))
                    {
                        provider = new ClaimProvider();
                        provider.ClaimID = hipaaclaim.ClaimID;
                        provider.FileID = 0;
                        provider.ServiceLineNumber = "0";
                        provider.LoopName = "2310A";
                        provider.RepeatSequence = (claim.Providers.Count + 1).ToString();
                        foreach (XElement ele in referProv.Descendants(ns + "NM1_ReferringProviderName").Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM104")) provider.ProviderFirstName = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM105")) provider.ProviderMiddle = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM107")) provider.ProviderSuffix = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                        }
                        if (referProv.Descendants(ns + "REF_ReferringProviderSecondaryIdentification").Count() > 0)
                        {
                            foreach (XElement ele in referProv.Descendants(ns + "REF_ReferringProviderSecondaryIdentification"))
                            {
                                ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                                si.ClaimID = hipaaclaim.ClaimID;
                                si.FileID = 0;
                                si.ServiceLineNumber = "0";
                                si.LoopName = "2310A";
                                foreach (XElement child_ele in ele.Descendants())
                                {
                                    if (child_ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = child_ele.Value;
                                    if (child_ele.Name.ToString().StartsWith("REF02")) si.ProviderID = child_ele.Value;
                                }
                                claim.SecondaryIdentifications.Add(si);
                            }
                        }
                        claim.Providers.Add(provider);
                    }
                }

                XElement rendProv = loop2000B.Descendants(ns + "NM1_SubLoop_4").Descendants(ns + "TS837_2310B_Loop").FirstOrDefault();
                if (rendProv != null)
                {
                    provider = new ClaimProvider();
                    provider.ClaimID = hipaaclaim.ClaimID;
                    provider.FileID = 0;
                    provider.ServiceLineNumber = "0";
                    provider.LoopName = "2310B";
                    XElement rendProvNM1 = rendProv.Descendants(ns + "NM1_RenderingProviderName").FirstOrDefault();
                    if (rendProvNM1 != null)
                    {
                        foreach (XElement ele in rendProvNM1.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM104")) provider.ProviderFirstName = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM105")) provider.ProviderMiddle = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM107")) provider.ProviderSuffix = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                        }
                    }
                    XElement rendProvPRV = rendProv.Descendants(ns + "PRV_RenderingProviderSpecialtyInformation").FirstOrDefault();
                    if (rendProvPRV != null)
                    {
                        foreach (XElement ele in rendProvPRV.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("PRV03")) provider.ProviderTaxonomyCode = ele.Value;
                        }
                    }
                    claim.Providers.Add(provider);
                    if (rendProv.Descendants(ns + "REF_RenderingProviderSecondaryIdentification").Count() > 0)
                    {
                        foreach (XElement rendProvSI in rendProv.Descendants(ns + "REF_RenderingProviderSecondaryIdentification"))
                        {
                            ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                            si.ClaimID = hipaaclaim.ClaimID;
                            si.FileID = 0;
                            si.ServiceLineNumber = "0";
                            si.LoopName = "2310B";
                            foreach (XElement ele in rendProvSI.Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = ele.Value;
                                if (ele.Name.ToString().StartsWith("REF02")) si.ProviderID = ele.Value;
                            }
                            claim.SecondaryIdentifications.Add(si);
                        }
                    }
                }
                XElement servFacility = loop2000B.Descendants(ns + "NM1_SubLoop_4").Descendants(ns + "TS837_2310C_Loop").FirstOrDefault();
                if (servFacility != null)
                {
                    provider = new ClaimProvider();
                    provider.ClaimID = hipaaclaim.ClaimID;
                    provider.FileID = 0;
                    provider.ServiceLineNumber = "0";
                    provider.LoopName = "2310C";
                    XElement servFacilityNM1 = servFacility.Descendants(ns + "NM1_ServiceFacilityLocationName").FirstOrDefault();
                    if (servFacilityNM1 != null)
                    {
                        foreach (XElement ele in servFacilityNM1.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM104")) provider.ProviderFirstName = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM105")) provider.ProviderMiddle = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM107")) provider.ProviderSuffix = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                        }
                    }
                    XElement servFacilityN3 = servFacility.Descendants(ns + "N3_ServiceFacilityLocationAddress").FirstOrDefault();
                    if (servFacilityN3 != null)
                    {
                        foreach (XElement ele in servFacilityN3.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N301")) provider.ProviderAddress = ele.Value;
                            if (ele.Name.ToString().StartsWith("N302")) provider.ProviderAddress2 = ele.Value;
                        }
                    }
                    XElement servFacilityN4 = servFacility.Descendants(ns + "N4_ServiceFacilityLocationCity_State_ZIPCode").FirstOrDefault();
                    if (servFacilityN4 != null)
                    {
                        foreach (XElement ele in servFacilityN4.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N401")) provider.ProviderCity = ele.Value;
                            if (ele.Name.ToString().StartsWith("N402")) provider.ProviderState = ele.Value;
                            if (ele.Name.ToString().StartsWith("N403")) provider.ProviderZip = ele.Value;
                            if (ele.Name.ToString().StartsWith("N404")) provider.ProviderCountry = ele.Value;
                            if (ele.Name.ToString().StartsWith("N407")) provider.ProviderCountrySubCode = ele.Value;
                        }
                    }
                    claim.Providers.Add(provider);
                }
                XElement SuperProv = loop2000B.Descendants(ns + "NM1_SubLoop_4").Descendants(ns + "TS837_2310D_Loop").FirstOrDefault();
                if (SuperProv != null)
                {
                    provider = new ClaimProvider();
                    provider.ClaimID = hipaaclaim.ClaimID;
                    provider.FileID = 0;
                    provider.ServiceLineNumber = "0";
                    provider.LoopName = "2310D";
                    XElement superProvNM1 = SuperProv.Descendants(ns + "NM1_SupervisingProviderName").FirstOrDefault();
                    if (superProvNM1 != null)
                    {
                        foreach (XElement ele in superProvNM1.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N401")) provider.ProviderCity = ele.Value;
                            if (ele.Name.ToString().StartsWith("N402")) provider.ProviderState = ele.Value;
                            if (ele.Name.ToString().StartsWith("N403")) provider.ProviderZip = ele.Value;
                            if (ele.Name.ToString().StartsWith("N404")) provider.ProviderCountry = ele.Value;
                            if (ele.Name.ToString().StartsWith("N407")) provider.ProviderCountrySubCode = ele.Value;
                        }
                    }
                }
                XElement pickup = loop2000B.Descendants(ns + "NM1_SubLoop_4").Descendants(ns + "TS837_2310E_Loop").FirstOrDefault();
                if (pickup != null)
                {
                    provider = new ClaimProvider();
                    provider.ClaimID = hipaaclaim.ClaimID;
                    provider.FileID = 0;
                    provider.ServiceLineNumber = "0";
                    provider.LoopName = "2310E";
                    XElement pickupNM1 = pickup.Descendants(ns + "NM1_AmbulancePick_upLocation").FirstOrDefault();
                    if (pickupNM1 != null)
                    {
                        foreach (XElement ele in pickupNM1.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                        }
                    }
                    XElement pickupN3 = pickup.Descendants(ns + "N3_AmbulancePick_upLocationAddress").FirstOrDefault();
                    if (pickupN3 != null)
                    {
                        foreach (XElement ele in pickupN3.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N301")) provider.ProviderAddress = ele.Value;
                            if (ele.Name.ToString().StartsWith("N302")) provider.ProviderAddress2 = ele.Value;
                        }
                    }
                    XElement pickupN4 = pickup.Descendants(ns + "N4_AmbulancePick_upLocationCity_State_ZipCode").FirstOrDefault();
                    if (pickupN4 != null)
                    {
                        foreach (XElement ele in pickupN4.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N401")) provider.ProviderCity = ele.Value;
                            if (ele.Name.ToString().StartsWith("N402")) provider.ProviderState = ele.Value;
                            if (ele.Name.ToString().StartsWith("N403")) provider.ProviderZip = ele.Value;
                            if (ele.Name.ToString().StartsWith("N404")) provider.ProviderCountry = ele.Value;
                            if (ele.Name.ToString().StartsWith("N407")) provider.ProviderCountrySubCode = ele.Value;
                        }
                    }
                    claim.Providers.Add(provider);
                }
                XElement dropoff = loop2000B.Descendants(ns + "NM1_SubLoop_4").Descendants(ns + "TS837_2310F_Loop").FirstOrDefault();
                if (dropoff != null)
                {
                    provider = new ClaimProvider();
                    provider.ClaimID = hipaaclaim.ClaimID;
                    provider.FileID = 0;
                    provider.ServiceLineNumber = "0";
                    provider.LoopName = "2310F";
                    XElement dropoffNM1 = dropoff.Descendants(ns + "NM1_AmbulanceDrop_offLocation").FirstOrDefault();
                    if (dropoffNM1 != null)
                    {
                        foreach (XElement ele in dropoffNM1.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                        }
                    }
                    XElement dropoffN3 = dropoff.Descendants(ns + "N3_AmbulanceDrop_offLocationAddress").FirstOrDefault();
                    if (dropoffN3 != null)
                    {
                        foreach (XElement ele in dropoffN3.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N301")) provider.ProviderAddress = ele.Value;
                            if (ele.Name.ToString().StartsWith("N302")) provider.ProviderAddress2 = ele.Value;
                        }
                    }
                    XElement dropoffN4 = dropoff.Descendants(ns + "N4_AmbulanceDrop_offLocationCity_State_ZipCode").FirstOrDefault();
                    if (dropoffN4 != null)
                    {
                        foreach (XElement ele in dropoffN4.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N401")) provider.ProviderCity = ele.Value;
                            if (ele.Name.ToString().StartsWith("N402")) provider.ProviderState = ele.Value;
                            if (ele.Name.ToString().StartsWith("N403")) provider.ProviderZip = ele.Value;
                            if (ele.Name.ToString().StartsWith("N404")) provider.ProviderCountry = ele.Value;
                            if (ele.Name.ToString().StartsWith("N407")) provider.ProviderCountrySubCode = ele.Value;
                        }
                    }
                    claim.Providers.Add(provider);
                }
                XElement loop2320 = loop2000B.Descendants(ns + "TS837_2320_Loop").FirstOrDefault();
                if (loop2320 != null)
                {
                    subscriber = new ClaimSBR();
                    subscriber.ClaimID = hipaaclaim.ClaimID;
                    subscriber.FileID = 0;
                    subscriber.LoopName = "2320";
                    XElement othersubscriber = loop2320.Descendants(ns + "SBR_OtherSubscriberInformation").FirstOrDefault();
                    if (othersubscriber != null)
                    {
                        foreach (XElement ele in othersubscriber.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("SBR01")) subscriber.SubscriberSequenceNumber = ele.Value;
                            if (ele.Name.ToString().StartsWith("SBR02")) subscriber.SubscriberRelationshipCode = ele.Value;
                            if (ele.Name.ToString().StartsWith("SBR03")) subscriber.InsuredGroupNumber = ele.Value;
                            if (ele.Name.ToString().StartsWith("SBR04")) subscriber.OtherInsuredGroupName = ele.Value;
                            if (ele.Name.ToString().StartsWith("SBR05")) subscriber.InsuredTypeCode = ele.Value;
                            if (ele.Name.ToString().StartsWith("SBR09")) subscriber.ClaimFilingCode = ele.Value;
                        }
                        claim.Subscribers.Add(subscriber);
                    }
                    XElement othersubscriberAMT = loop2320.Descendants(ns + "AMT_SubLoop").FirstOrDefault();
                    if (othersubscriberAMT != null)
                    {
                        XElement cobPayerPaidAmount = othersubscriberAMT.Descendants(ns + "AMT_CoordinationofBenefits_COB_PayerPaidAmount").FirstOrDefault();
                        if (cobPayerPaidAmount != null)
                        {
                            foreach (XElement ele in cobPayerPaidAmount.Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("AMT02")) subscriber.COBPayerPaidAmount = ele.Value;
                            }
                        }
                    }
                    XElement othersubscriberOI = loop2320.Descendants(ns + "OI_OtherInsuranceCoverageInformation").FirstOrDefault();
                    foreach (XElement ele in othersubscriberOI.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("OI03")) subscriber.BenefitsAssignmentCertificationIndicator = ele.Value;
                        if (ele.Name.ToString().StartsWith("OI04")) subscriber.PatientSignatureSourceCode = ele.Value;
                        if (ele.Name.ToString().StartsWith("OI06")) subscriber.ReleaseOfInformationCode = ele.Value;
                    }
                    XElement loop2330A = loop2320.Descendants(ns + "NM1_SubLoop_5").FirstOrDefault().Descendants(ns + "TS837_2330A_Loop").FirstOrDefault();
                    XElement otherSubcriberNM1 = loop2330A.Descendants(ns + "NM1_OtherSubscriberName").FirstOrDefault();
                    foreach (XElement ele in otherSubcriberNM1.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("NM103")) subscriber.LastName = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM104")) subscriber.FirstName = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM105")) subscriber.MidddleName = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM107")) subscriber.NameSuffix = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM108")) subscriber.IDQualifier = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM109")) subscriber.IDCode = ele.Value;
                    }
                    XElement otherSubscriberN3 = loop2330A.Descendants(ns + "N3_OtherSubscriberAddress").FirstOrDefault();
                    if (otherSubscriberN3 != null)
                    {
                        foreach (XElement ele in otherSubscriberN3.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N301")) subscriber.SubscriberAddress = ele.Value;
                            if (ele.Name.ToString().StartsWith("N302")) subscriber.SubscriberAddress2 = ele.Value;
                        }
                    }
                    XElement otherSubscriberN4 = loop2330A.Descendants(ns + "N4_OtherSubscriberCity_State_ZIPCode").FirstOrDefault();
                    if (otherSubscriberN4 != null)
                    {
                        foreach (XElement ele in otherSubscriberN4.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N401")) subscriber.SubscriberCity = ele.Value;
                            if (ele.Name.ToString().StartsWith("N402")) subscriber.SubscriberState = ele.Value;
                            if (ele.Name.ToString().StartsWith("N403")) subscriber.SubscriberZip = ele.Value;
                            if (ele.Name.ToString().StartsWith("N404")) subscriber.SubscriberCountry = ele.Value;
                            if (ele.Name.ToString().StartsWith("N407")) subscriber.SubscriberCountrySubCode = ele.Value;
                        }
                    }
                    claim.Subscribers.Add(subscriber);
                    XElement loop2330B = loop2320.Descendants(ns + "NM1_SubLoop_5").FirstOrDefault().Descendants(ns + "TS837_2330B_Loop").FirstOrDefault();
                    provider = new ClaimProvider();
                    provider.ClaimID = hipaaclaim.ClaimID;
                    provider.FileID = 0;
                    provider.LoopName = "2330B";
                    provider.ServiceLineNumber = "0";
                    provider.RepeatSequence = claim.Subscribers.Last().SubscriberSequenceNumber;
                    XElement otherPayerNM1 = loop2330B.Descendants(ns + "NM1_OtherPayerName").FirstOrDefault();
                    foreach (XElement ele in otherPayerNM1.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                    }
                    XElement otherPayerN3 = loop2330B.Descendants(ns + "N3_OtherPayerAddress").FirstOrDefault();
                    if (otherPayerN3 != null)
                    {
                        foreach (XElement ele in otherPayerN3.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N301")) provider.ProviderAddress = ele.Value;
                            if (ele.Name.ToString().StartsWith("N302")) provider.ProviderAddress2 = ele.Value;
                        }
                    }
                    XElement otherPayerN4 = loop2330B.Descendants(ns + "N4_OtherPayerCity_State_ZIPCode").FirstOrDefault();
                    if (otherPayerN4 != null)
                    {
                        foreach (XElement ele in otherPayerN4.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N401")) provider.ProviderCity = ele.Value;
                            if (ele.Name.ToString().StartsWith("N402")) provider.ProviderState = ele.Value;
                            if (ele.Name.ToString().StartsWith("N403")) provider.ProviderZip = ele.Value;
                            if (ele.Name.ToString().StartsWith("N404")) provider.ProviderCountry = ele.Value;
                            if (ele.Name.ToString().StartsWith("N407")) provider.ProviderCountrySubCode = ele.Value;
                        }
                    }
                    claim.Providers.Add(provider);
                }
                foreach (XElement loop2400 in loop2000B.Descendants(ns + "TS837_2400_Loop"))
                {
                    ServiceLine line = new ServiceLine();
                    line.ClaimID = hipaaclaim.ClaimID;
                    line.FileID = 0;
                    XElement LX = loop2400.Descendants(ns + "LX_ServiceLineNumber").FirstOrDefault();
                    foreach (XElement ele in LX.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("LX01")) line.ServiceLineNumber = ele.Value;
                    }
                    XElement sv1 = loop2400.Descendants(ns + "SV1_ProfessionalService").FirstOrDefault();
                    XElement lineProcedure = sv1.Descendants(ns + "C003_CompositeMedicalProcedureIdentifier").FirstOrDefault();
                    XElement lineDiagPointer = sv1.Descendants(ns + "C004_CompositeDiagnosisCodePointer").FirstOrDefault();
                    foreach (XElement ele in lineProcedure.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("C00301")) line.ServiceIDQualifier = ele.Value;
                        if (ele.Name.ToString().StartsWith("C00302")) line.ProcedureCode = ele.Value;
                        if (ele.Name.ToString().StartsWith("C00303")) line.ProcedureModifier1 = ele.Value;
                        if (ele.Name.ToString().StartsWith("C00304")) line.ProcedureModifier2 = ele.Value;
                        if (ele.Name.ToString().StartsWith("C00305")) line.ProcedureModifier3 = ele.Value;
                        if (ele.Name.ToString().StartsWith("C00306")) line.ProcedureModifier4 = ele.Value;
                        if (ele.Name.ToString().StartsWith("C00307")) line.ProcedureDescription = ele.Value;
                    }
                    foreach (XElement ele in sv1.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("SV102")) line.LineItemChargeAmount = ele.Value;
                        if (ele.Name.ToString().StartsWith("SV103")) line.LineItemUnit = ele.Value;
                        if (ele.Name.ToString().StartsWith("SV104")) line.ServiceUnitQuantity = ele.Value;
                        if (ele.Name.ToString().StartsWith("SV105")) line.LineItemPOS = ele.Value;
                        if (ele.Name.ToString().StartsWith("SV109")) line.EmergencyIndicator = ele.Value;
                        if (ele.Name.ToString().StartsWith("SV111")) line.EPSDTIndicator = ele.Value;
                        if (ele.Name.ToString().StartsWith("SV112")) line.FamilyPlanningIndicator = ele.Value;
                        if (ele.Name.ToString().StartsWith("SV115")) line.CopayStatusCode = ele.Value;
                    }
                    foreach (XElement ele in lineDiagPointer.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("C00401")) line.DiagPointer1 = ele.Value;
                        if (ele.Name.ToString().StartsWith("C00402")) line.DiagPointer2 = ele.Value;
                        if (ele.Name.ToString().StartsWith("C00403")) line.DiagPointer3 = ele.Value;
                        if (ele.Name.ToString().StartsWith("C00404")) line.DiagPointer4 = ele.Value;
                    }
                    XElement dtps = loop2400.Descendants(ns + "DTP_SubLoop_2").FirstOrDefault();
                    foreach (XElement ele in dtps.Descendants())
                    {
                        foreach (XElement child_ele in ele.Descendants())
                        {
                            if (child_ele.Name.ToString().StartsWith("DTP01"))
                            {
                                switch (child_ele.Value)
                                {
                                    case "472":
                                        line.ServiceFromDate = ((XElement)child_ele.NextNode.NextNode).Value.Split('-')[0];
                                        if (((XElement)child_ele.NextNode.NextNode).Value.Contains("-")) line.ServiceToDate = ((XElement)child_ele.NextNode.NextNode).Value.Split('-')[1];
                                        break;
                                }
                                break;
                            }
                        }
                    }
                    XElement lineCN1 = loop2400.Descendants(ns + "CN1_ContractInformation_2").FirstOrDefault();
                    if (lineCN1 != null)
                    {
                        foreach (XElement ele in lineCN1.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("CN101")) line.ContractTypeCode = ele.Value;
                            if (ele.Name.ToString().StartsWith("CN102")) line.ContractAmount = ele.Value;
                            if (ele.Name.ToString().StartsWith("CN103")) line.ContractPercentage = ele.Value;
                            if (ele.Name.ToString().StartsWith("CN104")) line.ContractCode = ele.Value;
                            if (ele.Name.ToString().StartsWith("CN105")) line.TermsDiscountPercentage = ele.Value;
                            if (ele.Name.ToString().StartsWith("CN106")) line.ContractVersionIdentifier = ele.Value;
                        }
                    }
                    if (loop2400.Descendants(ns + "QTY_SubLoop").Count() > 0)
                    {
                        foreach (XElement ele in loop2400.Descendants(ns + "QTY_SubLoop").Nodes())
                        {
                            foreach (XElement child_ele in ele.Descendants())
                            {
                                if (child_ele.Name.ToString().StartsWith("QTY01"))
                                {
                                    switch (child_ele.Value)
                                    {
                                        case "PT":
                                            line.AmbulancePatientCount = ((XElement)child_ele.NextNode).Value;
                                            break;
                                        case "FL":
                                            line.ObstetricAdditionalUnits = ((XElement)child_ele.NextNode).Value;
                                            break;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    claim.Lines.Add(line);
                    XElement lineREF = loop2400.Descendants(ns + "REF_SubLoop_7").FirstOrDefault();
                    if (lineREF != null)
                    {
                        foreach (XElement ele in lineREF.Nodes())
                        {
                            ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                            si.ClaimID = hipaaclaim.ClaimID;
                            si.FileID = 0;
                            si.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                            si.LoopName = "2400";
                            foreach (XElement child_ele in ele.Descendants())
                            {
                                if (child_ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = child_ele.Value;
                                if (child_ele.Name.ToString().StartsWith("REF02")) si.ProviderID = child_ele.Value;
                            }
                            claim.SecondaryIdentifications.Add(si);
                        }
                    }
                    XElement lineNte = loop2400.Descendants(ns + "NTE_SubLoop").FirstOrDefault();
                    if (lineNte != null)
                    {
                        foreach (XElement ele in lineNte.Nodes())
                        {
                            ClaimNte nte = new ClaimNte();
                            nte.ClaimID = hipaaclaim.ClaimID;
                            nte.FileID = 0;
                            nte.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                            foreach (XElement child_ele in ele.Descendants())
                            {
                                if (child_ele.Name.ToString().StartsWith("NTE01")) nte.NoteCode = child_ele.Value;
                                if (child_ele.Name.ToString().StartsWith("NTE02")) nte.NoteText = child_ele.Value;
                            }
                            claim.Notes.Add(nte);
                        }
                    }
                    XElement loop2410 = loop2400.Descendants(ns + "TS837_2410_Loop").FirstOrDefault();
                    if (loop2410 != null)
                    {
                        XElement linDrugIdentification = loop2410.Descendants(ns + "LIN_DrugIdentification").FirstOrDefault();
                        if (linDrugIdentification != null)
                        {
                            foreach (XElement ele in linDrugIdentification.Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("LIN02")) claim.Lines.Last().LINQualifier = ele.Value;
                                if (ele.Name.ToString().StartsWith("LIN03")) claim.Lines.Last().NationalDrugCode = ele.Value;
                            }
                        }
                        XElement ctpDrugQuantity = loop2410.Descendants(ns + "CTP_DrugQuantity").FirstOrDefault();
                        if (ctpDrugQuantity != null)
                        {
                            foreach (XElement ele in ctpDrugQuantity.Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("CTP04")) claim.Lines.Last().DrugQuantity = ele.Value;
                            }
                            foreach (XElement ele in ctpDrugQuantity.Descendants(ns + "C001_CompositeUnitofMeasure_6").First().Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("C00101")) claim.Lines.Last().DrugQualifier = ele.Value;
                            }
                        }
                    }
                    XElement loop2420A = loop2400.Descendants(ns + "NM1_SubLoop_6").Descendants(ns + "TS837_2420A_Loop").FirstOrDefault();
                    if (loop2420A != null)
                    {
                        provider = new ClaimProvider();
                        provider.ClaimID = hipaaclaim.ClaimID;
                        provider.FileID = 0;
                        provider.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                        provider.LoopName = "2420A";
                        XElement loop2420ANM1 = loop2420A.Descendants(ns + "NM1_RenderingProviderName_2").FirstOrDefault();
                        if (loop2420ANM1 != null)
                        {
                            foreach (XElement ele in loop2420ANM1.Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                                if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                                if (ele.Name.ToString().StartsWith("NM104")) provider.ProviderFirstName = ele.Value;
                                if (ele.Name.ToString().StartsWith("NM105")) provider.ProviderMiddle = ele.Value;
                                if (ele.Name.ToString().StartsWith("NM107")) provider.ProviderSuffix = ele.Value;
                                if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                                if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                            }
                        }
                        XElement loop2420APRV = loop2420A.Descendants(ns + "PRV_RenderingProviderSpecialtyInformation_2").FirstOrDefault();
                        if (loop2420APRV != null)
                        {
                            foreach (XElement ele in loop2420APRV.Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("PRV03")) provider.ProviderTaxonomyCode = ele.Value;
                            }
                        }
                        claim.Providers.Add(provider);
                    }
                    XElement loop2420E = loop2400.Descendants(ns + "NM1_SubLoop_6").Descendants(ns + "TS837_2420E_Loop").FirstOrDefault();
                    if (loop2420E != null)
                    {
                        provider = new ClaimProvider();
                        provider.ClaimID = hipaaclaim.ClaimID;
                        provider.FileID = 0;
                        provider.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                        provider.LoopName = "2420E";
                        XElement loop2420ENM1 = loop2420E.Descendants(ns + "NM1_OrderingProviderName").FirstOrDefault();
                        if (loop2420ENM1 != null)
                        {
                            foreach (XElement ele in loop2420ENM1.Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                                if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                                if (ele.Name.ToString().StartsWith("NM104")) provider.ProviderFirstName = ele.Value;
                                if (ele.Name.ToString().StartsWith("NM105")) provider.ProviderMiddle = ele.Value;
                                if (ele.Name.ToString().StartsWith("NM107")) provider.ProviderSuffix = ele.Value;
                                if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                                if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                            }
                        }
                        claim.Providers.Add(provider);
                    }
                    foreach (XElement loop2430 in loop2400.Descendants(ns + "TS837_2430_Loop"))
                    {
                        ClaimLineSVD svd = new ClaimLineSVD();
                        svd.ClaimID = hipaaclaim.ClaimID;
                        svd.FileID = 0;
                        svd.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                        svd.RepeatSequence = (claim.SVDs.Count + 1).ToString();
                        XElement linesvd = loop2430.Descendants(ns + "SVD_LineAdjudicationInformation").FirstOrDefault();
                        claim.SVDs.Add(svd);
                        if (linesvd != null)
                        {
                            foreach (XElement ele in linesvd.Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("SVD01")) claim.SVDs.Last().OtherPayerPrimaryIdentifier = ele.Value;
                                if (ele.Name.ToString().StartsWith("SVD02")) claim.SVDs.Last().ServiceLinePaidAmount = ele.Value;
                                if (ele.Name.ToString().StartsWith("SVD05")) claim.SVDs.Last().PaidServiceUnitCount = ele.Value;
                                if (ele.Name.ToString().StartsWith("SVD06")) claim.SVDs.Last().BundledLineNumber = ele.Value;
                            }
                            XElement svdProcedure = linesvd.Descendants(ns + "C003_CompositeMedicalProcedureIdentifier_3").FirstOrDefault();
                            foreach (XElement ele in svdProcedure.Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("C00301")) claim.SVDs.Last().ServiceQualifier = ele.Value;
                                if (ele.Name.ToString().StartsWith("C00302")) claim.SVDs.Last().ProcedureCode = ele.Value;
                                if (ele.Name.ToString().StartsWith("C00303")) claim.SVDs.Last().ProcedureModifier1 = ele.Value;
                                if (ele.Name.ToString().StartsWith("C00304")) claim.SVDs.Last().ProcedureModifier2 = ele.Value;
                                if (ele.Name.ToString().StartsWith("C00305")) claim.SVDs.Last().ProcedureModifier3 = ele.Value;
                                if (ele.Name.ToString().StartsWith("C00306")) claim.SVDs.Last().ProcedureModifier4 = ele.Value;
                                if (ele.Name.ToString().StartsWith("C00307")) claim.SVDs.Last().ProcedureDescription = ele.Value;
                            }
                        }
                        foreach (XElement linecas in loop2400.Descendants(ns + "CAS_LineAdjustment"))
                        {
                            ClaimCAS cas = new ClaimCAS();
                            cas.ClaimID = hipaaclaim.ClaimID;
                            cas.FileID = 0;
                            cas.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                            cas.SubscriberSequenceNumber = claim.SVDs.Last().RepeatSequence;
                            foreach (XElement ele in linecas.Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("CAS01")) cas.GroupCode = ele.Value;
                                if (ele.Name.ToString().StartsWith("CAS02")) cas.ReasonCode = ele.Value;
                                if (ele.Name.ToString().StartsWith("CAS03")) cas.AdjustmentAmount = ele.Value;
                            }
                            claim.Cases.Add(cas);
                        }
                        XElement linedtp = loop2400.Descendants(ns + "DTP_LineCheckorRemittanceDate").FirstOrDefault();
                        foreach (XElement ele in linedtp.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("DTP03")) claim.SVDs.Last().AdjudicationDate = ele.Value;
                        }
                        XElement lineRemainingPatientLiabilityAMT = loop2400.Descendants(ns + "AMT_RemainingPatientLiability_2").FirstOrDefault();
                        if (lineRemainingPatientLiabilityAMT != null)
                        {
                            foreach (XElement ele in lineRemainingPatientLiabilityAMT.Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("AMT02")) claim.SVDs.Last().ReaminingPatientLiabilityAmount = ele.Value;
                            }
                        }

                    }
                }

                claims.Add(claim);
                if (claims.Count >= 1000)
                {
                    SHRUtil.SaveClaims(ref claims);
                    claims.Clear();
                }
            }
            //System.IO.File.AppendAllText(@"c:\temp\hipaatemp.txt", sb.ToString());
            SHRUtil.SaveClaims(ref claims);
            context.Dispose();
        }
        public static void SubHist_P305_Old()
        {
            List<SubHist_Header> headers = Utility.GetSubHistHeader("P305");
            Claim claim;
            List<Claim> claims = new List<Claim>();
            int sequence = 1000000;
            foreach (SubHist_Header header in headers)
            {
                claim = new Claim();
                claim.Header.FileID = 0;
                claim.Header.ClaimID = header.ClaimNo;
                claim.Header.ClaimAmount = header.BilledAmount;
                claim.Header.ClaimPOS = header.PlaceOfService;
                claim.Header.ClaimType = "B";
                claim.Header.ClaimFrequencyCode = header.ClaimFrequency;
                claim.Header.ClaimProviderSignature = "Y";
                claim.Header.ClaimProviderAssignment = "A";
                claim.Header.ClaimBenefitAssignment = "Y";
                claim.Header.ClaimReleaseofInformationCode = "Y";
                claim.Header.ClaimPatientSignatureSourceCode = "P";
                claim.Header.AdmissionDate = header.AdmissionDate;
                claim.Header.DischargeDate = header.DischargeDate;
                claim.Header.ContractTypeCode = header.ContractType;
                claim.Header.ContractAmount = header.ContractAmount;
                claim.Header.AmbulanceReasonCode = header.AmbulanceTransportReasonCode;
                claim.Header.AmbulanceQuantity = header.TransportDistance;
                claim.Header.AdmissionTypeCode = header.AdmissionType;
                claim.Header.AdmissionSourceCode = header.AdmissionSource;
                claim.Header.PatientStatusCode = header.PatientStatus;
                claim.Header.PatientResponsibilityAmount = header.EstimatedAmountDue;
                ClaimProvider provider = new ClaimProvider();
                provider.FileID = 0;
                provider.LoopName = "2000A";
                provider.ClaimID = header.ClaimNo;
                provider.ServiceLineNumber = "0";
                provider.ProviderQualifier = "85";
                provider.ProviderTaxonomyCode = header.BillProvSpecialty;
                provider.ProviderLastName = header.BillProvLast;
                provider.ProviderFirstName = header.BillProvFirst;
                provider.ProviderIDQualifier = "XX";
                provider.ProviderID = header.BillProvNPI;
                provider.ProviderZip = header.BillProvZip;
                BillingProviderAdditionalDataElements elem = Utility.GetBillingProviderAdditionalDataElements(header.BillProvNPI);
                if (elem != null)
                {
                    provider.ProviderAddress = elem.ProviderAddress;
                    provider.ProviderCity = elem.ProviderCity;
                    provider.ProviderState = elem.ProviderState;
                    provider.ProviderZip = elem.ProviderZip;
                    provider.ProviderCountry = elem.ProviderCountry;
                    ClaimSecondaryIdentification si1 = new ClaimSecondaryIdentification();
                    si1.ClaimID = header.ClaimNo;
                    si1.FileID = 0;
                    si1.LoopName = "2010AA";
                    si1.ProviderQualifier = "EI";
                    si1.ServiceLineNumber = "0";
                    si1.ProviderID = elem.EIN;
                    claim.SecondaryIdentifications.Add(si1);
                }
                claim.Providers.Add(provider);
                ClaimSBR subscriber = new ClaimSBR();
                subscriber.ClaimID = header.ClaimNo;
                subscriber.FileID = 0;
                subscriber.LoopName = "2000B";
                subscriber.SubscriberSequenceNumber = "S";
                subscriber.LastName = header.SubscriberLast;
                subscriber.FirstName = header.SubscriberFirst;
                subscriber.IDQualifier = "MI";
                subscriber.IDCode = header.SubscriberID;
                subscriber.SubscriberZip = header.SubscriberZip;
                subscriber.SubscriberBirthDate = header.SubscriberDOB;
                subscriber.SubscriberGender = header.SubscriberGender;
                claim.Subscribers.Add(subscriber);
                if (!string.IsNullOrEmpty(header.SubscriberSSN))
                {
                    ClaimSecondaryIdentification sid = new ClaimSecondaryIdentification();
                    sid.ClaimID = header.ClaimNo;
                    sid.FileID = 0;
                    sid.LoopName = "2000B";
                    sid.ProviderQualifier = "SY";
                    sid.ProviderID = header.SubscriberSSN;
                    sid.RepeatSequence = "S";
                    sid.ServiceLineNumber = "0";
                    claim.SecondaryIdentifications.Add(sid);
                }
                if (!string.IsNullOrEmpty(header.RendProvNPI) && !string.IsNullOrEmpty(header.RendProvLast))
                {
                    provider = new ClaimProvider();
                    provider.ClaimID = header.ClaimNo;
                    provider.FileID = 0;
                    provider.LoopName = "2310B";
                    provider.ProviderQualifier = "82";
                    provider.ProviderLastName = header.RendProvLast;
                    provider.ProviderFirstName = header.RendProvFirst;
                    provider.ProviderIDQualifier = "XX";
                    provider.ProviderID = header.RendProvNPI;
                    claim.Providers.Add(provider);
                }
                if (!string.IsNullOrEmpty(header.PickUpAddress) && !string.IsNullOrEmpty(header.PickUpCity))
                {
                    provider = new ClaimProvider();
                    provider.ClaimID = header.ClaimNo;
                    provider.FileID = 0;
                    provider.LoopName = "2310E";
                    provider.ProviderQualifier = "PW";
                    provider.ProviderAddress = header.PickUpAddress;
                    provider.ProviderAddress2 = header.PickUpAddress2;
                    provider.ProviderCity = header.PickUpCity;
                    provider.ProviderState = header.PickUpState;
                    provider.ProviderZip = header.PickUpZip;
                    claim.Providers.Add(provider);
                }
                if (!string.IsNullOrEmpty(header.DropOffAddress) && !string.IsNullOrEmpty(header.DropOffCity))
                {
                    provider = new ClaimProvider();
                    provider.ClaimID = header.ClaimNo;
                    provider.FileID = 0;
                    provider.LoopName = "2310F";
                    provider.ProviderQualifier = "45";
                    provider.ProviderLastName = header.DropOffName;
                    provider.ProviderAddress = header.DropOffAddress;
                    provider.ProviderAddress2 = header.DropOffAddress2;
                    provider.ProviderCity = header.DropOffCity;
                    provider.ProviderState = header.DropOffState;
                    provider.ProviderZip = header.DropOffZip;
                    claim.Providers.Add(provider);
                }
                ClaimHI hi = new ClaimHI();
                hi.ClaimID = header.ClaimNo;
                hi.FileID = 0;
                hi.HIQual = "";
                hi.HICode = "";
                claim.His.Add(hi);
                ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                si.ClaimID = header.ClaimNo;
                si.FileID = 0;
                si.LoopName = "2300";
                si.ProviderQualifier = "F8";
                si.ProviderID = header.ClaimNo;
                claim.SecondaryIdentifications.Add(si);
                claims.Add(claim);
                if (claims.Count >= 1000)
                {
                    SHRUtil.SaveClaims(ref claims);
                    claims.Clear();
                }
                sequence++;
            }
            SHRUtil.SaveClaims(ref claims);
            claims.Clear();
            List<SubHist_Line> lines = Utility.GetSubHistLine("P305");
            List<ServiceLine> claimlines = new List<ServiceLine>();
            List<ClaimLineSVD> claimlinesvds = new List<ClaimLineSVD>();
            List<ClaimCAS> claimcases = new List<ClaimCAS>();
            foreach (SubHist_Line line in lines)
            {
                ServiceLine claimline = new ServiceLine();
                claimline.FileID = 0;
                claimline.ClaimID = line.ClaimNo;
                claimline.ServiceLineNumber = line.ServiceLineNumber;
                claimline.ServiceIDQualifier = line.ServiceIDQualifier;
                claimline.ProcedureCode = line.ServiceID1;
                claimline.ProcedureModifier1 = line.ProcedureModifier1;
                claimline.ProcedureModifier2 = line.ProcedureModifier2;
                claimline.ProcedureModifier3 = line.ProcedureModifier3;
                claimline.ProcedureModifier4 = line.ProcedureModifier4;
                claimline.ProcedureDescription = line.Description;
                claimline.LineItemChargeAmount = line.ChargeAmount;
                claimline.DiagPointer1 = line.DiagnosisPointer1;
                claimline.DiagPointer2 = line.DiagnosisPointer2;
                claimline.DiagPointer3 = line.DiagnosisPointer3;
                claimline.DiagPointer4 = line.DiagnosisPointer4;
                claimline.ServiceFromDate = line.ServiceDate;
                claimline.ServiceToDate = line.ServiceDate;
                claimlines.Add(claimline);
                if (!string.IsNullOrEmpty(line.PayedAmount))
                {
                    ClaimLineSVD claimlinesvd = new ClaimLineSVD();
                    claimlinesvd.ClaimID = line.ClaimNo;
                    claimlinesvd.FileID = 0;
                    claimlinesvd.ServiceLineNumber = line.ServiceLineNumber;
                    claimlinesvd.ServiceQualifier = line.ServiceIDQualifier;
                    claimlinesvd.ProcedureCode = line.ServiceID1;
                    claimlinesvd.ProcedureModifier1 = line.ProcedureModifier1;
                    claimlinesvd.ProcedureModifier2 = line.ProcedureModifier2;
                    claimlinesvd.ProcedureModifier3 = line.ProcedureModifier3;
                    claimlinesvd.ProcedureModifier4 = line.ProcedureModifier4;
                    claimlinesvd.ProcedureDescription = line.Description;
                    claimlinesvd.ServiceLinePaidAmount = line.PayedAmount;
                    claimlinesvds.Add(claimlinesvd);
                }
                if (!string.IsNullOrEmpty(line.AdjustedAmount))
                {
                    ClaimCAS claimcas = new ClaimCAS();
                    claimcas.ClaimID = line.ClaimNo;
                    claimcas.FileID = 0;
                    claimcas.ServiceLineNumber = line.ServiceLineNumber;
                    claimcas.GroupCode = "CO";
                    claimcas.ReasonCode = "45";
                    claimcas.AdjustmentAmount = line.AdjustedAmount;
                    claimcases.Add(claimcas);
                }
            }
            SHRUtil.SaveLines(ref claimlines, ref claimlinesvds, ref claimcases);
            claimlines.Clear();
            claimlinesvds.Clear();
            claimcases.Clear();
        }
        public static void SubHist_I305()
        {
            //var context = new SHRContext();
            //HashSet<string> hs = new HashSet<string>();
            //foreach (Hipaa_XML hipaaclaim in context.Hipaa_XML)
            //{

            //    XDocument xdoc = XDocument.Parse(hipaaclaim.ClaimHipaaXML);
            //    Recursive_Search_Name(xdoc.Root, ref hs);
            //}
            //StringBuilder sb = new StringBuilder();
            //foreach (string s in hs) sb.AppendLine(s);
            //System.IO.File.AppendAllText(@"C:\temp\HipaaXMLNames_CAPI.txt", sb.ToString());
            //context.Dispose();
            Claim claim;
            List<Claim> claims = new List<Claim>();
            var context = new SHRContext();
            //StringBuilder sb = new StringBuilder();
            foreach (Hipaa_XML hipaaclaim in context.Hipaa_XML.Where(x => x.ClaimType == "I"))
            {

                XDocument xdoc = XDocument.Parse(hipaaclaim.ClaimHipaaXML);
                //Recursive_Search_Value(xdoc.Root, ref sb);


                claim = new Claim();
                claim.Header.ClaimID = hipaaclaim.ClaimID;
                claim.Header.FileID = 0;
                XNamespace ns = "http://schemas.microsoft.com/BizTalk/EDI/X12/2006";
                //XElement st = xdoc.Descendants("ST").FirstOrDefault();
                //XElement bht = xdoc.Descendants(ns + "BHT_BeginningofHierarchicalTransaction").FirstOrDefault();
                //XElement loop1000A = xdoc.Descendants(ns + "NM1_SubLoop").FirstOrDefault().Descendants(ns + "TS837_1000A_Loop").FirstOrDefault();
                //XElement loop1000B = xdoc.Descendants(ns + "NM1_SubLoop").FirstOrDefault().Descendants(ns + "TS837_1000B_Loop").FirstOrDefault();
                ClaimProvider provider = new ClaimProvider();
                provider.ClaimID = hipaaclaim.ClaimID;
                provider.FileID = 0;
                provider.ServiceLineNumber = "0";
                provider.LoopName = "2000A";
                XElement loop2000A = xdoc.Descendants(ns + "TS837_2000A_Loop").FirstOrDefault();
                XElement BillProvPRV = loop2000A.Descendants(ns + "PRV_BillingProviderSpecialtyInformation").FirstOrDefault();
                if (BillProvPRV != null)
                {
                    foreach (XElement ele in BillProvPRV.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("PRV03"))
                        {
                            provider.ProviderTaxonomyCode = ele.Value;
                        }
                    }
                }
                XElement loop2010AA = loop2000A.Descendants(ns + "NM1_SubLoop_2").FirstOrDefault().Descendants(ns + "TS837_2010AA_Loop").FirstOrDefault();
                XElement BillProvNM1 = loop2010AA.Descendants(ns + "NM1_BillingProviderName").FirstOrDefault();
                foreach (XElement ele in BillProvNM1.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM104")) provider.ProviderFirstName = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM105")) provider.ProviderMiddle = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM107")) provider.ProviderSuffix = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                }
                XElement BillProvN3 = loop2010AA.Descendants(ns + "N3_BillingProviderAddress").FirstOrDefault();
                foreach (XElement ele in BillProvN3.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("N301")) provider.ProviderAddress = ele.Value;
                    if (ele.Name.ToString().StartsWith("N302")) provider.ProviderAddress2 = ele.Value;
                }
                XElement BillProvN4 = loop2010AA.Descendants(ns + "N4_BillingProviderCity_State_ZIPCode").FirstOrDefault();
                foreach (XElement ele in BillProvN4.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("N401")) provider.ProviderCity = ele.Value;
                    if (ele.Name.ToString().StartsWith("N402")) provider.ProviderState = ele.Value;
                    if (ele.Name.ToString().StartsWith("N403")) provider.ProviderZip = ele.Value;
                    if (ele.Name.ToString().StartsWith("N404")) provider.ProviderCountry = ele.Value;
                    if (ele.Name.ToString().StartsWith("N407")) provider.ProviderCountrySubCode = ele.Value;
                }
                claim.Providers.Add(provider);
                //XElement BillProvSecondaryIdentifications = loop2010AA.Descendants(ns + "REF_SubLoop").FirstOrDefault();
                //claim.Providers.Add(provider);
                //foreach (XElement ele in BillProvSecondaryIdentifications.Nodes())
                //{
                //    ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                //    si.ClaimID = hipaaclaim.ClaimID;
                //    si.FileID = 0;
                //    si.ServiceLineNumber = "0";
                //    si.LoopName = "2010AA";
                //    foreach (XElement child_ele in ele.Descendants())
                //    {
                //        if (child_ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = child_ele.Value;
                //        if (child_ele.Name.ToString().StartsWith("REF02")) si.ProviderID = child_ele.Value;
                //    }
                //    claim.SecondaryIdentifications.Add(si);
                //}
                //for institutional
                XElement BillingProviderTaxIdentification = loop2010AA.Descendants(ns + "REF_BillingProviderTaxIdentification").FirstOrDefault();
                if (BillingProviderTaxIdentification != null)
                {
                    ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                    si.ClaimID = hipaaclaim.ClaimID;
                    si.FileID = 0;
                    si.ServiceLineNumber = "0";
                    si.LoopName = "2010AA";
                    foreach (XElement ele in BillingProviderTaxIdentification.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = ele.Value;
                        if (ele.Name.ToString().StartsWith("REF02")) si.ProviderID = ele.Value;
                    }
                    claim.SecondaryIdentifications.Add(si);
                }
                foreach (XElement BillProvPER in loop2010AA.Descendants(ns + "PER_BillingProviderContactInformation"))
                {
                    ProviderContact pc = new ProviderContact();
                    pc.ClaimID = hipaaclaim.ClaimID;
                    pc.FileID = 0;
                    pc.ServiceLineNumber = "0";
                    pc.LoopName = "2000A";
                    foreach (XElement ele in BillProvPER.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("PER02")) pc.ContactName = ele.Value;
                        if (ele.Name.ToString().StartsWith("PER04"))
                        {
                            switch (((XElement)ele.PreviousNode).Value)
                            {
                                case "TE":
                                    pc.Phone = ele.Value;
                                    break;
                                case "EM":
                                    pc.Email = ele.Value;
                                    break;
                                case "FX":
                                    pc.Fax = ele.Value;
                                    break;
                            }
                        }
                        if (ele.Name.ToString().StartsWith("PER06"))
                        {
                            switch (((XElement)ele.PreviousNode).Value)
                            {
                                case "TE":
                                    pc.Phone = ele.Value;
                                    break;
                                case "EM":
                                    pc.Email = ele.Value;
                                    break;
                                case "FX":
                                    pc.Fax = ele.Value;
                                    break;
                                case "EX":
                                    pc.PhoneEx = ele.Value;
                                    break;
                            }
                        }
                        if (ele.Name.ToString().StartsWith("PER08"))
                        {
                            switch (((XElement)ele.PreviousNode).Value)
                            {
                                case "TE":
                                    pc.Phone = ele.Value;
                                    break;
                                case "EM":
                                    pc.Email = ele.Value;
                                    break;
                                case "FX":
                                    pc.Fax = ele.Value;
                                    break;
                                case "EX":
                                    pc.PhoneEx = ele.Value;
                                    break;
                            }
                        }
                    }
                    claim.ProviderContacts.Add(pc);
                }
                XElement loop2010AB = loop2000A.Descendants(ns + "NM1_SubLoop_2").FirstOrDefault().Descendants(ns + "TS837_2010AB_Loop").FirstOrDefault();
                if (loop2010AB != null)
                {
                    provider = new ClaimProvider();
                    provider.ClaimID = hipaaclaim.ClaimID;
                    provider.FileID = 0;
                    provider.ServiceLineNumber = "0";
                    provider.LoopName = "2010AB";
                    XElement loop2010ABNM1 = loop2010AB.Descendants(ns + "NM1_Pay_toAddressName").FirstOrDefault();
                    if (loop2010ABNM1 != null)
                    {
                        foreach (XElement ele in loop2010ABNM1.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM104")) provider.ProviderFirstName = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM105")) provider.ProviderMiddle = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM107")) provider.ProviderSuffix = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                        }
                    }
                    XElement loop2010ABN3 = loop2010AB.Descendants(ns + "N3_Pay_ToAddress_ADDRESS").FirstOrDefault();
                    if (loop2010ABN3 != null)
                    {
                        foreach (XElement ele in loop2010ABN3.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N301")) provider.ProviderAddress = ele.Value;
                            if (ele.Name.ToString().StartsWith("N302")) provider.ProviderAddress2 = ele.Value;
                        }
                    }
                    XElement loop2010ABN4 = loop2010AB.Descendants(ns + "N4_Pay_toAddressCity_State_ZIPCode").FirstOrDefault();
                    if (loop2010ABN4 != null)
                    {
                        foreach (XElement ele in loop2010ABN4.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N401")) provider.ProviderCity = ele.Value;
                            if (ele.Name.ToString().StartsWith("N402")) provider.ProviderState = ele.Value;
                            if (ele.Name.ToString().StartsWith("N403")) provider.ProviderZip = ele.Value;
                            if (ele.Name.ToString().StartsWith("N404")) provider.ProviderCountry = ele.Value;
                            if (ele.Name.ToString().StartsWith("N407")) provider.ProviderCountrySubCode = ele.Value;
                        }
                    }
                    claim.Providers.Add(provider);
                }
                ClaimSBR subscriber = new ClaimSBR();
                subscriber.ClaimID = hipaaclaim.ClaimID;
                subscriber.FileID = 0;
                subscriber.LoopName = "2000B";
                XElement loop2000B = loop2000A.Descendants(ns + "TS837_2000B_Loop").FirstOrDefault();
                XElement Subscriber = loop2000B.Descendants(ns + "SBR_SubscriberInformation").FirstOrDefault();
                foreach (XElement ele in Subscriber.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("SBR01")) subscriber.SubscriberSequenceNumber = ele.Value;
                    if (ele.Name.ToString().StartsWith("SBR02")) subscriber.SubscriberRelationshipCode = ele.Value;
                    if (ele.Name.ToString().StartsWith("SBR03")) subscriber.InsuredGroupNumber = ele.Value;
                    if (ele.Name.ToString().StartsWith("SBR04")) subscriber.OtherInsuredGroupName = ele.Value;
                    if (ele.Name.ToString().StartsWith("SBR05")) subscriber.InsuredTypeCode = ele.Value;
                    if (ele.Name.ToString().StartsWith("SBR09")) subscriber.ClaimFilingCode = ele.Value;
                }
                XElement subscriberPAT = loop2000B.Descendants(ns + "PAT_PatientInformation").FirstOrDefault();
                if (subscriberPAT != null)
                {
                    foreach (XElement ele in subscriberPAT.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("PAT06")) subscriber.DeathDate = ele.Value;
                        if (ele.Name.ToString().StartsWith("PAT08")) subscriber.Weight = ele.Value;
                        if (ele.Name.ToString().StartsWith("PAT09")) subscriber.PregnancyIndicator = ele.Value;
                    }
                }
                XElement loop2010BA = loop2000A.Descendants(ns + "NM1_SubLoop_3").FirstOrDefault().Descendants(ns + "TS837_2010BA_Loop").FirstOrDefault();
                XElement subscriberNM1 = loop2010BA.Descendants(ns + "NM1_SubscriberName").FirstOrDefault();
                foreach (XElement ele in subscriberNM1.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("NM103")) subscriber.LastName = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM104")) subscriber.FirstName = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM105")) subscriber.MidddleName = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM107")) subscriber.NameSuffix = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM108")) subscriber.IDQualifier = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM109")) subscriber.IDCode = ele.Value;
                }
                XElement subscriberN3 = loop2010BA.Descendants(ns + "N3_SubscriberAddress").FirstOrDefault();
                if (subscriberN3 != null)
                {
                    foreach (XElement ele in subscriberN3.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("N301")) subscriber.SubscriberAddress = ele.Value;
                        if (ele.Name.ToString().StartsWith("N302")) subscriber.SubscriberAddress2 = ele.Value;
                    }
                }
                XElement subscriberN4 = loop2010BA.Descendants(ns + "N4_SubscriberCity_State_ZIPCode").FirstOrDefault();
                if (subscriberN4 != null)
                {
                    foreach (XElement ele in subscriberN4.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("N401")) subscriber.SubscriberCity = ele.Value;
                        if (ele.Name.ToString().StartsWith("N402")) subscriber.SubscriberState = ele.Value;
                        if (ele.Name.ToString().StartsWith("N403")) subscriber.SubscriberZip = ele.Value;
                        if (ele.Name.ToString().StartsWith("N404")) subscriber.SubscriberCountry = ele.Value;
                        if (ele.Name.ToString().StartsWith("N407")) subscriber.SubscriberCountrySubCode = ele.Value;
                    }
                }
                XElement subscriberDMG = loop2010BA.Descendants(ns + "DMG_SubscriberDemographicInformation").FirstOrDefault();
                if (subscriberDMG != null)
                {
                    foreach (XElement ele in subscriberDMG.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("DMG02")) subscriber.SubscriberBirthDate = ele.Value;
                        if (ele.Name.ToString().StartsWith("DMG03")) subscriber.SubscriberGender = ele.Value;
                    }
                }
                XElement subscriberREF = loop2010BA.Descendants(ns + "REF_SubLoop_3").FirstOrDefault();
                if (subscriberREF != null)
                {
                    foreach (XElement ele in subscriberREF.Nodes())
                    {
                        ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                        si.ClaimID = hipaaclaim.ClaimID;
                        si.FileID = 0;
                        si.ServiceLineNumber = "0";
                        si.LoopName = "2010BA";
                        foreach (XElement child_ele in ele.Descendants())
                        {
                            if (child_ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = child_ele.Value;
                            if (child_ele.Name.ToString().StartsWith("REF02")) si.ProviderID = child_ele.Value;
                        }
                        claim.SecondaryIdentifications.Add(si);
                    }
                }
                claim.Subscribers.Add(subscriber);
                provider = new ClaimProvider();
                provider.ClaimID = hipaaclaim.ClaimID;
                provider.FileID = 0;
                provider.ServiceLineNumber = "0";
                provider.LoopName = "2010BB";
                XElement loop2010BB = loop2000A.Descendants(ns + "NM1_SubLoop_3").FirstOrDefault().Descendants(ns + "TS837_2010BB_Loop").FirstOrDefault();
                XElement subscriberPayerNM1 = loop2010BB.Descendants(ns + "NM1_PayerName").FirstOrDefault();
                foreach (XElement ele in subscriberPayerNM1.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                }
                XElement subscriberPayerN3 = loop2010BB.Descendants(ns + "N3_PayerAddress").FirstOrDefault();
                if (subscriberPayerN3 != null)
                {
                    foreach (XElement ele in subscriberPayerN3.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("N301")) provider.ProviderAddress = ele.Value;
                        if (ele.Name.ToString().StartsWith("N302")) provider.ProviderAddress2 = ele.Value;
                    }
                }
                XElement subscriberPayerN4 = loop2010BB.Descendants(ns + "N4_PayerCity_StatE_ZIPCode").FirstOrDefault();
                if (subscriberPayerN4 != null)
                {
                    foreach (XElement ele in subscriberPayerN4.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("N401")) provider.ProviderCity = ele.Value;
                        if (ele.Name.ToString().StartsWith("N402")) provider.ProviderState = ele.Value;
                        if (ele.Name.ToString().StartsWith("N403")) provider.ProviderZip = ele.Value;
                        if (ele.Name.ToString().StartsWith("N404")) provider.ProviderCountry = ele.Value;
                        if (ele.Name.ToString().StartsWith("N407")) provider.ProviderCountrySubCode = ele.Value;
                    }
                }
                claim.Providers.Add(provider);
                XElement loop2300 = loop2000B.Descendants(ns + "TS837_2300_Loop").FirstOrDefault();
                XElement Clm = loop2300.Descendants(ns + "CLM_Claiminformation").FirstOrDefault();
                XElement ClmPOS = Clm.Descendants(ns + "C023_HealthCareServiceLocationInformation").FirstOrDefault();
                foreach (XElement ele in Clm.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("CLM02")) claim.Header.ClaimAmount = ele.Value;
                    if (ele.Name.ToString().StartsWith("CLM06")) claim.Header.ClaimProviderSignature = ele.Value;
                    if (ele.Name.ToString().StartsWith("CLM07")) claim.Header.ClaimProviderAssignment = ele.Value;
                    if (ele.Name.ToString().StartsWith("CLM08")) claim.Header.ClaimBenefitAssignment = ele.Value;
                    if (ele.Name.ToString().StartsWith("CLM09")) claim.Header.ClaimReleaseofInformationCode = ele.Value;
                }
                foreach (XElement ele in ClmPOS.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("C02301")) claim.Header.ClaimPOS = ele.Value;
                    if (ele.Name.ToString().StartsWith("C02302")) claim.Header.ClaimType = ele.Value;
                    if (ele.Name.ToString().StartsWith("C02303")) claim.Header.ClaimFrequencyCode = ele.Value;
                }
                XElement claimDTP = loop2300.Descendants(ns + "DTP_SubLoop").FirstOrDefault();
                if (claimDTP != null)
                {
                    foreach (XElement ele in claimDTP.Nodes())
                    {
                        foreach (XElement child_ele in ele.Descendants())
                        {
                            if (child_ele.Name.ToString().StartsWith("DTP01"))
                            {
                                switch (child_ele.Value)
                                {
                                    case "304":
                                        claim.Header.LastSeenDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                        break;
                                    case "431":
                                        claim.Header.CurrentIllnessDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                        break;
                                    case "435":
                                        claim.Header.AdmissionDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                        break;
                                    case "439":
                                        claim.Header.AccidentDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                        break;
                                    case "454":
                                        claim.Header.InitialTreatmentDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                        break;
                                    case "471":
                                        claim.Header.PrescriptionDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                        break;
                                    case "484":
                                        claim.Header.LastMenstrualPeriodDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                        break;
                                    case "096":
                                        //institutional discharge hour
                                        claim.Header.DischargeDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                        break;
                                    case "434":
                                        //institutional statement date
                                        claim.Header.StatementFromDate = ((XElement)child_ele.NextNode.NextNode).Value.Split('-')[0];
                                        claim.Header.StatementToDate = ((XElement)child_ele.NextNode.NextNode).Value.Split('-')[1];
                                        break;
                                }
                                break;
                            }
                        }
                    }
                }
                XElement CL1 = loop2300.Descendants(ns + "CL1_InstitutionalClaimCode").FirstOrDefault();
                if (CL1 != null)
                {
                    foreach (XElement ele in CL1.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("CL101")) claim.Header.AdmissionTypeCode = ele.Value;
                        if (ele.Name.ToString().StartsWith("CL102")) claim.Header.AdmissionSourceCode = ele.Value;
                        if (ele.Name.ToString().StartsWith("CL103")) claim.Header.PatientStatusCode = ele.Value;
                    }
                }
                XElement CN1 = loop2000B.Descendants(ns + "CN1_ContractInformation").FirstOrDefault();
                if (CN1 != null)
                {
                    foreach (XElement ele in CN1.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("CN101")) claim.Header.ContractTypeCode = ele.Value;
                        if (ele.Name.ToString().StartsWith("CN102")) claim.Header.ContractAmount = ele.Value;
                        if (ele.Name.ToString().StartsWith("CN104")) claim.Header.ContractCode = ele.Value;
                    }
                }
                XElement patientResponsibilityAmount = loop2300.Descendants(ns + "AMT_PatientEstimatedAmountDue").FirstOrDefault();
                if (patientResponsibilityAmount != null)
                {
                    foreach (XElement ele in patientResponsibilityAmount.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("AMT02")) claim.Header.PatientResponsibilityAmount = ele.Value;
                    }
                }
                XElement ClaimSecondaryIdentifications = loop2300.Descendants(ns + "REF_SubLoop_4").FirstOrDefault();
                if (ClaimSecondaryIdentifications != null)
                {
                    foreach (XElement ele in ClaimSecondaryIdentifications.Nodes())
                    {
                        ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                        si.ClaimID = hipaaclaim.ClaimID;
                        si.FileID = 0;
                        si.ServiceLineNumber = "0";
                        si.LoopName = "2300";
                        foreach (XElement child_ele in ele.Descendants())
                        {
                            if (child_ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = child_ele.Value;
                            if (child_ele.Name.ToString().StartsWith("REF02")) si.ProviderID = child_ele.Value;
                        }
                        claim.SecondaryIdentifications.Add(si);
                    }
                }
                if (claim.SecondaryIdentifications.Where(x => x.LoopName == "2300" && x.ProviderQualifier == "F8").FirstOrDefault() == null)
                {
                    ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                    si.ClaimID = hipaaclaim.ClaimID;
                    si.FileID = 0;
                    si.LoopName = "2300";
                    si.ServiceLineNumber = "0";
                    si.ProviderQualifier = "F8";
                    si.ProviderID = hipaaclaim.ClaimID;
                    claim.SecondaryIdentifications.Add(si);
                }
                XElement note = loop2000B.Descendants(ns + "NTE_SubLoop").FirstOrDefault();
                if (note != null)
                {
                    foreach (XElement ele in note.Nodes())
                    {
                        ClaimNte nte = new ClaimNte();
                        nte.ClaimID = hipaaclaim.ClaimID;
                        nte.FileID = 0;
                        nte.ServiceLineNumber = "0";
                        foreach (XElement child_ele in ele.Descendants())
                        {
                            if (child_ele.Name.ToString().StartsWith("NTE01")) nte.NoteCode = child_ele.Value;
                            if (child_ele.Name.ToString().StartsWith("NTE02")) nte.NoteText = child_ele.Value;
                        }
                        claim.Notes.Add(nte);
                    }
                }
                //XElement claimCR1 = loop2000B.Descendants(ns + "CR1_AmbulanceTransportInformation").FirstOrDefault();
                //if (claimCR1 != null)
                //{
                //    foreach (XElement ele in claimCR1.Descendants())
                //    {
                //        if (ele.Name.ToString().StartsWith("CR102")) claim.Header.AmbulanceWeight = ele.Value;
                //        if (ele.Name.ToString().StartsWith("CR104")) claim.Header.AmbulanceReasonCode = ele.Value;
                //        if (ele.Name.ToString().StartsWith("CR106")) claim.Header.AmbulanceQuantity = ele.Value;
                //        if (ele.Name.ToString().StartsWith("CR109")) claim.Header.AmbulanceRoundTripDescription = ele.Value;
                //        if (ele.Name.ToString().StartsWith("CR110")) claim.Header.AmbulanceStretcherDescription = ele.Value;
                //    }
                //}
                //XElement claimlevelCRC = loop2000B.Descendants(ns + "CRC_SubLoop").FirstOrDefault();
                //if (claimlevelCRC != null)
                //{
                //    foreach (XElement ele in claimlevelCRC.Nodes())
                //    {
                //        ClaimCRC claimCRC = new ClaimCRC();
                //        claimCRC.ClaimID = hipaaclaim.ClaimID;
                //        claimCRC.FileID = 0;
                //        claimCRC.ServiceLineNumber = "0";
                //        foreach (XElement child_ele in ele.Descendants())
                //        {
                //            if (child_ele.Name.ToString().StartsWith("CRC01")) claimCRC.CodeCategory = child_ele.Value;
                //            if (child_ele.Name.ToString().StartsWith("CRC02")) claimCRC.ConditionIndicator = child_ele.Value;
                //            if (child_ele.Name.ToString().StartsWith("CRC03")) claimCRC.ConditionCode = child_ele.Value;
                //            if (child_ele.Name.ToString().StartsWith("CRC04")) claimCRC.ConditionCode2 = child_ele.Value;
                //            if (child_ele.Name.ToString().StartsWith("CRC05")) claimCRC.ConditionCode3 = child_ele.Value;
                //            if (child_ele.Name.ToString().StartsWith("CRC06")) claimCRC.ConditionCode4 = child_ele.Value;
                //            if (child_ele.Name.ToString().StartsWith("CRC07")) claimCRC.ConditionCode5 = child_ele.Value;
                //        }
                //        claim.CRCs.Add(claimCRC);
                //    }
                //}
                XElement his = loop2300.Descendants(ns + "HI_SubLoop").FirstOrDefault();
                foreach (XElement ele in his.Nodes())
                {
                    foreach (XElement child_ele in ele.Nodes())
                    {
                        ClaimHI hi = new ClaimHI();
                        hi.ClaimID = hipaaclaim.ClaimID;
                        hi.FileID = 0;
                        foreach (XElement grand_ele in child_ele.Descendants())
                        {
                            if (grand_ele.Name.ToString().StartsWith("C02201")) hi.HIQual = grand_ele.Value;
                            if (grand_ele.Name.ToString().StartsWith("C02202")) hi.HICode = grand_ele.Value;
                            if (grand_ele.Name.ToString().StartsWith("C02204")) hi.HIFromDate = grand_ele.Value;
                            if (grand_ele.Name.ToString().StartsWith("C02205")) hi.HIAmount = grand_ele.Value;
                            if (grand_ele.Name.ToString().StartsWith("C02209")) hi.PresentOnAdmissionIndicator = grand_ele.Value;
                        }
                        claim.His.Add(hi);
                    }
                }
                //XElement claimAMT = loop2000B.Descendants(ns + "AMT_PatientAmountPaid").FirstOrDefault();
                //if (claimAMT != null)
                //{
                //    foreach (XElement ele in claimAMT.Descendants())
                //    {
                //        if (ele.Name.ToString().StartsWith("AMT02")) claim.Header.PatientPaidAmount = ele.Value;
                //    }
                //}
                XElement claimlevelPWK = loop2000B.Descendants(ns + "PWK_ClaimSupplementalInformation").FirstOrDefault();
                if (claimlevelPWK != null)
                {
                    ClaimPWK pwk = new ClaimPWK();
                    pwk.ClaimID = hipaaclaim.ClaimID;
                    pwk.FileID = 0;
                    pwk.ServiceLineNumber = "0";
                    foreach (XElement ele in claimlevelPWK.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("PWK01")) pwk.ReportTypeCode = ele.Value;
                        if (ele.Name.ToString().StartsWith("PWK02")) pwk.ReportTransmissionCode = ele.Value;
                    }
                    claim.PWKs.Add(pwk);
                }

                XElement AtteProv = loop2300.Descendants(ns + "NM1_SubLoop_4").Descendants(ns + "TS837_2310A_Loop").FirstOrDefault();
                if (AtteProv != null)
                {
                    provider = new ClaimProvider();
                    provider.ClaimID = hipaaclaim.ClaimID;
                    provider.FileID = 0;
                    provider.ServiceLineNumber = "0";
                    provider.LoopName = "2310A";
                    provider.RepeatSequence = (claim.Providers.Count + 1).ToString();
                    foreach (XElement ele in AtteProv.Descendants(ns + "NM1_AttendingProviderName").Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM104")) provider.ProviderFirstName = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM105")) provider.ProviderMiddle = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM107")) provider.ProviderSuffix = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                    }
                    if (AtteProv.Descendants(ns + "REF_AttendingProviderSecondaryIdentification").Count() > 0)
                    {
                        foreach (XElement ele in AtteProv.Descendants(ns + "REF_ReferringProviderSecondaryIdentification"))
                        {
                            ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                            si.ClaimID = hipaaclaim.ClaimID;
                            si.FileID = 0;
                            si.ServiceLineNumber = "0";
                            si.LoopName = "2310A";
                            foreach (XElement child_ele in ele.Descendants())
                            {
                                if (child_ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = child_ele.Value;
                                if (child_ele.Name.ToString().StartsWith("REF02")) si.ProviderID = child_ele.Value;
                            }
                            claim.SecondaryIdentifications.Add(si);
                        }
                    }
                    claim.Providers.Add(provider);
                }

                XElement operProv = loop2300.Descendants(ns + "NM1_SubLoop_4").Descendants(ns + "TS837_2310B_Loop").FirstOrDefault();
                if (operProv != null)
                {
                    provider = new ClaimProvider();
                    provider.ClaimID = hipaaclaim.ClaimID;
                    provider.FileID = 0;
                    provider.ServiceLineNumber = "0";
                    provider.LoopName = "2310B";
                    XElement operProvNM1 = operProv.Descendants(ns + "NM1_OperatingPhysicianName").FirstOrDefault();
                    if (operProvNM1 != null)
                    {
                        foreach (XElement ele in operProvNM1.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM104")) provider.ProviderFirstName = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM105")) provider.ProviderMiddle = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM107")) provider.ProviderSuffix = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                            if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                        }
                    }
                    claim.Providers.Add(provider);
                    if (operProv.Descendants(ns + "REF_OperatingPhysicianSecondaryIdentification").Count() > 0)
                    {
                        foreach (XElement rendProvSI in operProv.Descendants(ns + "REF_OperatingPhysicianSecondaryIdentification"))
                        {
                            ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                            si.ClaimID = hipaaclaim.ClaimID;
                            si.FileID = 0;
                            si.ServiceLineNumber = "0";
                            si.LoopName = "2310B";
                            foreach (XElement ele in rendProvSI.Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = ele.Value;
                                if (ele.Name.ToString().StartsWith("REF02")) si.ProviderID = ele.Value;
                            }
                            claim.SecondaryIdentifications.Add(si);
                        }
                    }
                }
                XElement loop2320 = loop2300.Descendants(ns + "TS837_2320_Loop").FirstOrDefault();
                if (loop2320 != null)
                {
                    subscriber = new ClaimSBR();
                    subscriber.ClaimID = hipaaclaim.ClaimID;
                    subscriber.FileID = 0;
                    subscriber.LoopName = "2320";
                    XElement othersubscriber = loop2320.Descendants(ns + "SBR_OtherSubscriberInformation").FirstOrDefault();
                    if (othersubscriber != null)
                    {
                        foreach (XElement ele in othersubscriber.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("SBR01")) subscriber.SubscriberSequenceNumber = ele.Value;
                            if (ele.Name.ToString().StartsWith("SBR02")) subscriber.SubscriberRelationshipCode = ele.Value;
                            if (ele.Name.ToString().StartsWith("SBR03")) subscriber.InsuredGroupNumber = ele.Value;
                            if (ele.Name.ToString().StartsWith("SBR04")) subscriber.OtherInsuredGroupName = ele.Value;
                            if (ele.Name.ToString().StartsWith("SBR05")) subscriber.InsuredTypeCode = ele.Value;
                            if (ele.Name.ToString().StartsWith("SBR09")) subscriber.ClaimFilingCode = ele.Value;
                        }
                        claim.Subscribers.Add(subscriber);
                    }
                    XElement othersubscriberAMT = loop2320.Descendants(ns + "AMT_SubLoop").FirstOrDefault();
                    if (othersubscriberAMT != null)
                    {
                        XElement cobPayerPaidAmount = othersubscriberAMT.Descendants(ns + "AMT_CoordinationofBenefits_COB_PayerPaidAmount").FirstOrDefault();
                        if (cobPayerPaidAmount != null)
                        {
                            foreach (XElement ele in cobPayerPaidAmount.Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("AMT02")) subscriber.COBPayerPaidAmount = ele.Value;
                            }
                        }
                    }
                    XElement othersubscriberOI = loop2320.Descendants(ns + "OI_OtherInsuranceCoverageInformation").FirstOrDefault();
                    foreach (XElement ele in othersubscriberOI.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("OI03")) subscriber.BenefitsAssignmentCertificationIndicator = ele.Value;
                        if (ele.Name.ToString().StartsWith("OI04")) subscriber.PatientSignatureSourceCode = ele.Value;
                        if (ele.Name.ToString().StartsWith("OI06")) subscriber.ReleaseOfInformationCode = ele.Value;
                    }
                    XElement loop2330A = loop2320.Descendants(ns + "NM1_SubLoop_5").FirstOrDefault().Descendants(ns + "TS837_2330A_Loop").FirstOrDefault();
                    XElement otherSubcriberNM1 = loop2330A.Descendants(ns + "NM1_OtherSubscriberName").FirstOrDefault();
                    foreach (XElement ele in otherSubcriberNM1.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("NM103")) subscriber.LastName = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM104")) subscriber.FirstName = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM105")) subscriber.MidddleName = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM107")) subscriber.NameSuffix = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM108")) subscriber.IDQualifier = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM109")) subscriber.IDCode = ele.Value;
                    }
                    XElement otherSubscriberN3 = loop2330A.Descendants(ns + "N3_OtherSubscriberAddress").FirstOrDefault();
                    if (otherSubscriberN3 != null)
                    {
                        foreach (XElement ele in otherSubscriberN3.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N301")) subscriber.SubscriberAddress = ele.Value;
                            if (ele.Name.ToString().StartsWith("N302")) subscriber.SubscriberAddress2 = ele.Value;
                        }
                    }
                    XElement otherSubscriberN4 = loop2330A.Descendants(ns + "N4_OtherSubscriberCity_State_ZIPCode").FirstOrDefault();
                    if (otherSubscriberN4 != null)
                    {
                        foreach (XElement ele in otherSubscriberN4.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N401")) subscriber.SubscriberCity = ele.Value;
                            if (ele.Name.ToString().StartsWith("N402")) subscriber.SubscriberState = ele.Value;
                            if (ele.Name.ToString().StartsWith("N403")) subscriber.SubscriberZip = ele.Value;
                            if (ele.Name.ToString().StartsWith("N404")) subscriber.SubscriberCountry = ele.Value;
                            if (ele.Name.ToString().StartsWith("N407")) subscriber.SubscriberCountrySubCode = ele.Value;
                        }
                    }
                    claim.Subscribers.Add(subscriber);
                    XElement loop2330B = loop2320.Descendants(ns + "NM1_SubLoop_5").FirstOrDefault().Descendants(ns + "TS837_2330B_Loop").FirstOrDefault();
                    provider = new ClaimProvider();
                    provider.ClaimID = hipaaclaim.ClaimID;
                    provider.FileID = 0;
                    provider.LoopName = "2330B";
                    provider.ServiceLineNumber = "0";
                    provider.RepeatSequence = claim.Subscribers.Last().SubscriberSequenceNumber;
                    XElement otherPayerNM1 = loop2330B.Descendants(ns + "NM1_OtherPayerName").FirstOrDefault();
                    foreach (XElement ele in otherPayerNM1.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                    }
                    XElement otherPayerN3 = loop2330B.Descendants(ns + "N3_OtherPayerAddress").FirstOrDefault();
                    if (otherPayerN3 != null)
                    {
                        foreach (XElement ele in otherPayerN3.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N301")) provider.ProviderAddress = ele.Value;
                            if (ele.Name.ToString().StartsWith("N302")) provider.ProviderAddress2 = ele.Value;
                        }
                    }
                    XElement otherPayerN4 = loop2330B.Descendants(ns + "N4_OtherPayerCity_State_ZIPCode").FirstOrDefault();
                    if (otherPayerN4 != null)
                    {
                        foreach (XElement ele in otherPayerN4.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("N401")) provider.ProviderCity = ele.Value;
                            if (ele.Name.ToString().StartsWith("N402")) provider.ProviderState = ele.Value;
                            if (ele.Name.ToString().StartsWith("N403")) provider.ProviderZip = ele.Value;
                            if (ele.Name.ToString().StartsWith("N404")) provider.ProviderCountry = ele.Value;
                            if (ele.Name.ToString().StartsWith("N407")) provider.ProviderCountrySubCode = ele.Value;
                        }
                    }
                    claim.Providers.Add(provider);
                }
                foreach (XElement loop2400 in loop2300.Descendants(ns + "TS837_2400_Loop"))
                {
                    ServiceLine line = new ServiceLine();
                    line.ClaimID = hipaaclaim.ClaimID;
                    line.FileID = 0;
                    XElement LX = loop2400.Descendants(ns + "LX_ServiceLineNumber").FirstOrDefault();
                    foreach (XElement ele in LX.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("LX01")) line.ServiceLineNumber = ele.Value;
                    }
                    XElement sv2 = loop2400.Descendants(ns + "SV2_InstitutionalServiceLine").FirstOrDefault();
                    XElement lineProcedure = sv2.Descendants(ns + "C003_CompositeMedicalProcedureIdentifier").FirstOrDefault();
                    //XElement lineDiagPointer = sv2.Descendants(ns + "C004_CompositeDiagnosisCodePointer").FirstOrDefault();
                    if (lineProcedure != null)
                    {
                        foreach (XElement ele in lineProcedure.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("C00301")) line.ServiceIDQualifier = ele.Value;
                            if (ele.Name.ToString().StartsWith("C00302")) line.ProcedureCode = ele.Value;
                            if (ele.Name.ToString().StartsWith("C00303")) line.ProcedureModifier1 = ele.Value;
                            if (ele.Name.ToString().StartsWith("C00304")) line.ProcedureModifier2 = ele.Value;
                            if (ele.Name.ToString().StartsWith("C00305")) line.ProcedureModifier3 = ele.Value;
                            if (ele.Name.ToString().StartsWith("C00306")) line.ProcedureModifier4 = ele.Value;
                            if (ele.Name.ToString().StartsWith("C00307")) line.ProcedureDescription = ele.Value;
                        }
                    }
                    foreach (XElement ele in sv2.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("SV201")) line.RevenueCode = ele.Value;
                        if (ele.Name.ToString().StartsWith("SV203")) line.LineItemChargeAmount = ele.Value;
                        if (ele.Name.ToString().StartsWith("SV204")) line.LineItemUnit = ele.Value;
                        if (ele.Name.ToString().StartsWith("SV205")) line.ServiceUnitQuantity = ele.Value;
                        if (ele.Name.ToString().StartsWith("SV207")) line.LineItemDeniedChargeAmount = ele.Value;
                    }
                    //foreach (XElement ele in lineDiagPointer.Descendants())
                    //{
                    //    if (ele.Name.ToString().StartsWith("C00401")) line.DiagPointer1 = ele.Value;
                    //    if (ele.Name.ToString().StartsWith("C00402")) line.DiagPointer2 = ele.Value;
                    //    if (ele.Name.ToString().StartsWith("C00403")) line.DiagPointer3 = ele.Value;
                    //    if (ele.Name.ToString().StartsWith("C00404")) line.DiagPointer4 = ele.Value;
                    //}
                    XElement dtps = loop2400.Descendants(ns + "DTP_Date_ServiceDate").FirstOrDefault();
                    if (dtps != null)
                    {
                        foreach (XElement ele in dtps.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("DTP01"))
                            {
                                switch (ele.Value)
                                {
                                    case "472":
                                        line.ServiceFromDate = ((XElement)ele.NextNode.NextNode).Value.Split('-')[0];
                                        if (((XElement)ele.NextNode.NextNode).Value.Contains("-")) line.ServiceToDate = ((XElement)ele.NextNode.NextNode).Value.Split('-')[1];
                                        break;
                                }
                                break;
                            }
                        }
                    }
                    XElement lineCN1 = loop2400.Descendants(ns + "CN1_ContractInformation_2").FirstOrDefault();
                    if (lineCN1 != null)
                    {
                        foreach (XElement ele in lineCN1.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("CN101")) line.ContractTypeCode = ele.Value;
                            if (ele.Name.ToString().StartsWith("CN102")) line.ContractAmount = ele.Value;
                            if (ele.Name.ToString().StartsWith("CN103")) line.ContractPercentage = ele.Value;
                            if (ele.Name.ToString().StartsWith("CN104")) line.ContractCode = ele.Value;
                            if (ele.Name.ToString().StartsWith("CN105")) line.TermsDiscountPercentage = ele.Value;
                            if (ele.Name.ToString().StartsWith("CN106")) line.ContractVersionIdentifier = ele.Value;
                        }
                    }
                    //if (loop2400.Descendants(ns + "QTY_SubLoop").Count() > 0)
                    //{
                    //    foreach (XElement ele in loop2400.Descendants(ns + "QTY_SubLoop").Nodes())
                    //    {
                    //        foreach (XElement child_ele in ele.Descendants())
                    //        {
                    //            if (child_ele.Name.ToString().StartsWith("QTY01"))
                    //            {
                    //                switch (child_ele.Value)
                    //                {
                    //                    case "PT":
                    //                        line.AmbulancePatientCount = ((XElement)child_ele.NextNode).Value;
                    //                        break;
                    //                    case "FL":
                    //                        line.ObstetricAdditionalUnits = ((XElement)child_ele.NextNode).Value;
                    //                        break;
                    //                }
                    //                break;
                    //            }
                    //        }
                    //    }
                    //}
                    claim.Lines.Add(line);
                    XElement lineREF = loop2400.Descendants(ns + "REF_SubLoop_7").FirstOrDefault();
                    if (lineREF != null)
                    {
                        foreach (XElement ele in lineREF.Nodes())
                        {
                            ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                            si.ClaimID = hipaaclaim.ClaimID;
                            si.FileID = 0;
                            si.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                            si.LoopName = "2400";
                            foreach (XElement child_ele in ele.Descendants())
                            {
                                if (child_ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = child_ele.Value;
                                if (child_ele.Name.ToString().StartsWith("REF02")) si.ProviderID = child_ele.Value;
                            }
                            claim.SecondaryIdentifications.Add(si);
                        }
                    }
                    XElement lineNte = loop2400.Descendants(ns + "NTE_SubLoop").FirstOrDefault();
                    if (lineNte != null)
                    {
                        foreach (XElement ele in lineNte.Nodes())
                        {
                            ClaimNte nte = new ClaimNte();
                            nte.ClaimID = hipaaclaim.ClaimID;
                            nte.FileID = 0;
                            nte.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                            foreach (XElement child_ele in ele.Descendants())
                            {
                                if (child_ele.Name.ToString().StartsWith("NTE01")) nte.NoteCode = child_ele.Value;
                                if (child_ele.Name.ToString().StartsWith("NTE02")) nte.NoteText = child_ele.Value;
                            }
                            claim.Notes.Add(nte);
                        }
                    }
                    XElement loop2410 = loop2400.Descendants(ns + "TS837_2410_Loop").FirstOrDefault();
                    if (loop2410 != null)
                    {
                        XElement linDrugIdentification = loop2410.Descendants(ns + "LIN_DrugIdentification").FirstOrDefault();
                        if (linDrugIdentification != null)
                        {
                            foreach (XElement ele in linDrugIdentification.Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("LIN02")) claim.Lines.Last().LINQualifier = ele.Value;
                                if (ele.Name.ToString().StartsWith("LIN03")) claim.Lines.Last().NationalDrugCode = ele.Value;
                            }
                        }
                        XElement ctpDrugQuantity = loop2410.Descendants(ns + "CTP_DrugQuantity").FirstOrDefault();
                        if (ctpDrugQuantity != null)
                        {
                            foreach (XElement ele in ctpDrugQuantity.Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("CTP04")) claim.Lines.Last().DrugQuantity = ele.Value;
                            }
                            foreach (XElement ele in ctpDrugQuantity.Descendants(ns + "C001_CompositeUnitofMeasure_2").First().Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("C00101")) claim.Lines.Last().DrugQualifier = ele.Value;
                            }
                        }
                    }
                    //XElement loop2420A = loop2400.Descendants(ns + "NM1_SubLoop_6").Descendants(ns + "TS837_2420A_Loop").FirstOrDefault();
                    //if (loop2420A != null)
                    //{
                    //    provider = new ClaimProvider();
                    //    provider.ClaimID = hipaaclaim.ClaimID;
                    //    provider.FileID = 0;
                    //    provider.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                    //    provider.LoopName = "2420A";
                    //    XElement loop2420ANM1 = loop2420A.Descendants(ns + "NM1_RenderingProviderName_2").FirstOrDefault();
                    //    if (loop2420ANM1 != null)
                    //    {
                    //        foreach (XElement ele in loop2420ANM1.Descendants())
                    //        {
                    //            if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                    //            if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                    //            if (ele.Name.ToString().StartsWith("NM104")) provider.ProviderFirstName = ele.Value;
                    //            if (ele.Name.ToString().StartsWith("NM105")) provider.ProviderMiddle = ele.Value;
                    //            if (ele.Name.ToString().StartsWith("NM107")) provider.ProviderSuffix = ele.Value;
                    //            if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                    //            if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                    //        }
                    //    }
                    //    XElement loop2420APRV = loop2420A.Descendants(ns + "PRV_RenderingProviderSpecialtyInformation_2").FirstOrDefault();
                    //    if (loop2420APRV != null)
                    //    {
                    //        foreach (XElement ele in loop2420APRV.Descendants())
                    //        {
                    //            if (ele.Name.ToString().StartsWith("PRV03")) provider.ProviderTaxonomyCode = ele.Value;
                    //        }
                    //    }
                    //    claim.Providers.Add(provider);
                    //}
                    //XElement loop2420E = loop2400.Descendants(ns + "NM1_SubLoop_6").Descendants(ns + "TS837_2420E_Loop").FirstOrDefault();
                    //if (loop2420E != null)
                    //{
                    //    provider = new ClaimProvider();
                    //    provider.ClaimID = hipaaclaim.ClaimID;
                    //    provider.FileID = 0;
                    //    provider.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                    //    provider.LoopName = "2420E";
                    //    XElement loop2420ENM1 = loop2420E.Descendants(ns + "NM1_OrderingProviderName").FirstOrDefault();
                    //    if (loop2420ENM1 != null)
                    //    {
                    //        foreach (XElement ele in loop2420ENM1.Descendants())
                    //        {
                    //            if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                    //            if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                    //            if (ele.Name.ToString().StartsWith("NM104")) provider.ProviderFirstName = ele.Value;
                    //            if (ele.Name.ToString().StartsWith("NM105")) provider.ProviderMiddle = ele.Value;
                    //            if (ele.Name.ToString().StartsWith("NM107")) provider.ProviderSuffix = ele.Value;
                    //            if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                    //            if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                    //        }
                    //    }
                    //    claim.Providers.Add(provider);
                    //}
                    foreach (XElement loop2430 in loop2400.Descendants(ns + "TS837_2430_Loop"))
                    {
                        ClaimLineSVD svd = new ClaimLineSVD();
                        svd.ClaimID = hipaaclaim.ClaimID;
                        svd.FileID = 0;
                        svd.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                        svd.RepeatSequence = (claim.SVDs.Count + 1).ToString();
                        XElement linesvd = loop2430.Descendants(ns + "SVD_LineAdjudicationInformation").FirstOrDefault();
                        claim.SVDs.Add(svd);
                        if (linesvd != null)
                        {
                            foreach (XElement ele in linesvd.Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("SVD01")) claim.SVDs.Last().OtherPayerPrimaryIdentifier = ele.Value;
                                if (ele.Name.ToString().StartsWith("SVD02")) claim.SVDs.Last().ServiceLinePaidAmount = ele.Value;
                                if (ele.Name.ToString().StartsWith("SVD04")) claim.SVDs.Last().ServiceLineRevenueCode = ele.Value;
                                if (ele.Name.ToString().StartsWith("SVD05")) claim.SVDs.Last().PaidServiceUnitCount = ele.Value;
                                if (ele.Name.ToString().StartsWith("SVD06")) claim.SVDs.Last().BundledLineNumber = ele.Value;
                            }
                            XElement svdProcedure = linesvd.Descendants(ns + "C003_CompositeMedicalProcedureIdentifier_2").FirstOrDefault();
                            if (svdProcedure != null)
                            {
                                foreach (XElement ele in svdProcedure.Descendants())
                                {
                                    if (ele.Name.ToString().StartsWith("C00301")) claim.SVDs.Last().ServiceQualifier = ele.Value;
                                    if (ele.Name.ToString().StartsWith("C00302")) claim.SVDs.Last().ProcedureCode = ele.Value;
                                    if (ele.Name.ToString().StartsWith("C00303")) claim.SVDs.Last().ProcedureModifier1 = ele.Value;
                                    if (ele.Name.ToString().StartsWith("C00304")) claim.SVDs.Last().ProcedureModifier2 = ele.Value;
                                    if (ele.Name.ToString().StartsWith("C00305")) claim.SVDs.Last().ProcedureModifier3 = ele.Value;
                                    if (ele.Name.ToString().StartsWith("C00306")) claim.SVDs.Last().ProcedureModifier4 = ele.Value;
                                    if (ele.Name.ToString().StartsWith("C00307")) claim.SVDs.Last().ProcedureDescription = ele.Value;
                                }
                            }
                        }
                        foreach (XElement linecas in loop2400.Descendants(ns + "CAS_LineAdjustment"))
                        {
                            ClaimCAS cas = new ClaimCAS();
                            cas.ClaimID = hipaaclaim.ClaimID;
                            cas.FileID = 0;
                            cas.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                            cas.SubscriberSequenceNumber = claim.SVDs.Last().RepeatSequence;
                            foreach (XElement ele in linecas.Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("CAS01")) cas.GroupCode = ele.Value;
                                if (ele.Name.ToString().StartsWith("CAS02")) cas.ReasonCode = ele.Value;
                                if (ele.Name.ToString().StartsWith("CAS03")) cas.AdjustmentAmount = ele.Value;
                            }
                            claim.Cases.Add(cas);
                        }
                        XElement linedtp = loop2400.Descendants(ns + "DTP_LineCheckorRemittanceDate").FirstOrDefault();
                        foreach (XElement ele in linedtp.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("DTP03")) claim.SVDs.Last().AdjudicationDate = ele.Value;
                        }
                        //XElement lineRemainingPatientLiabilityAMT = loop2400.Descendants(ns + "AMT_RemainingPatientLiability_2").FirstOrDefault();
                        //if (lineRemainingPatientLiabilityAMT != null)
                        //{
                        //    foreach (XElement ele in lineRemainingPatientLiabilityAMT.Descendants())
                        //    {
                        //        if (ele.Name.ToString().StartsWith("AMT02")) claim.SVDs.Last().ReaminingPatientLiabilityAmount = ele.Value;
                        //    }
                        //}

                    }
                }

                claims.Add(claim);
                if (claims.Count >= 1000)
                {
                    SHRUtil.SaveClaims(ref claims);
                    claims.Clear();
                }
            }
            SHRUtil.SaveClaims(ref claims);
            context.Dispose();
        }
        public static void SubHist_P306()
        {
            List<HipaaXML> hipaaclaims = Utility.GetHipaaXMLForCAPProcedureCode_P();
            List<Hipaa_XML> hipaaxmls = new List<Hipaa_XML>();
            foreach (HipaaXML hipaaclaim in hipaaclaims) { hipaaxmls.Add(new Hipaa_XML { ClaimType = "P", ClaimID = hipaaclaim.ClaimID, EncounterId = hipaaclaim.EncounterId, ClaimHipaaXML = hipaaclaim.ClaimHipaaXML }); }
            using (var context1 = new SHRContext())
            {
                context1.Hipaa_XML.AddRange(hipaaxmls);
                context1.SaveChanges();
            }

        }

        public static void SubHist_I306()
        {
            List<HipaaXML> hipaaclaims = Utility.GetHipaaXMLForCAP0x001B2_I();
            List<Hipaa_XML> hipaaxmls = new List<Hipaa_XML>();
            foreach (HipaaXML hipaaclaim in hipaaclaims) { hipaaxmls.Add(new Hipaa_XML { ClaimType = "I", ClaimID = hipaaclaim.ClaimID, EncounterId = hipaaclaim.EncounterId, ClaimHipaaXML = hipaaclaim.ClaimHipaaXML }); }
            using (var context1 = new SHRContext())
            {
                context1.Hipaa_XML.AddRange(hipaaxmls);
                context1.SaveChanges();
            }

        }
    }
}
