using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser837
{
    public class Process837
    {
        public static void Process837Line(string s837Line, ref SubmissionLog submittedFile, ref string LoopName, ref bool saveFlag, ref Claim claim, ref List<Claim> claims)
        {
            ServiceLine line;
            ClaimCAS cas;
            ClaimCRC crc;
            ClaimHI hi;
            ClaimK3 k3;
            ClaimLineAuth auth;
            ClaimLineFRM frm;
            ClaimLineLQ lq;
            ClaimLineMEA mea;
            ClaimLineSVD svd;
            ClaimNte nte;
            ClaimPatient patient;
            ClaimProvider provider;
            ClaimPWK pwk;
            ClaimSBR sbr;
            ClaimSecondaryIdentification secondaryIdentification;
            ProviderContact providercontact;
            string[] temparray;

            string[] segments = s837Line.Split('*');
            switch (segments[0])
            {
                case "ST":
                    LoopName = "TransactionHeader";
                    break;
                case "BHT":
                    break;
                case "PER":
                    if (LoopName == "1000A")
                    {
                        if (!string.IsNullOrEmpty(submittedFile.SubmitterContactName1))
                        {
                            submittedFile.SubmitterContactName2 = segments[2];
                            switch (segments[3])
                            {
                                case "EM":
                                    submittedFile.SubmitterContactEmail2 = segments[4];
                                    break;
                                case "FX":
                                    submittedFile.SubmitterContactFax2 = segments[4];
                                    break;
                                case "TE":
                                    submittedFile.SubmitterContactPhone2 = segments[4];
                                    break;
                            }
                            if (segments.Length > 5)
                            {
                                switch (segments[5])
                                {
                                    case "EM":
                                        submittedFile.SubmitterContactEmail2 = segments[6];
                                        break;
                                    case "FX":
                                        submittedFile.SubmitterContactFax2 = segments[6];
                                        break;
                                    case "TE":
                                        submittedFile.SubmitterContactPhone2 = segments[6];
                                        break;
                                    case "EX":
                                        submittedFile.SubmitterContactPhoneEx2 = segments[6];
                                        break;
                                }
                            }
                            if (segments.Length > 7)
                            {
                                switch (segments[7])
                                {
                                    case "EM":
                                        submittedFile.SubmitterContactEmail2 = segments[8];
                                        break;
                                    case "FX":
                                        submittedFile.SubmitterContactFax2 = segments[8];
                                        break;
                                    case "TE":
                                        submittedFile.SubmitterContactPhone2 = segments[8];
                                        break;
                                    case "EX":
                                        submittedFile.SubmitterContactPhoneEx2 = segments[8];
                                        break;
                                }
                            }

                        }
                        else
                        {
                            submittedFile.SubmitterContactName1 = string.IsNullOrEmpty(segments[2]) ? segments[1] : segments[2];
                            switch (segments[3])
                            {
                                case "EM":
                                    submittedFile.SubmitterContactEmail1 = segments[4];
                                    break;
                                case "FX":
                                    submittedFile.SubmitterContactFax1 = segments[4];
                                    break;
                                case "TE":
                                    submittedFile.SubmitterContactPhone1 = segments[4];
                                    break;
                            }
                            if (segments.Length > 5)
                            {
                                switch (segments[5])
                                {
                                    case "EM":
                                        submittedFile.SubmitterContactEmail1 = segments[6];
                                        break;
                                    case "FX":
                                        submittedFile.SubmitterContactFax1 = segments[6];
                                        break;
                                    case "TE":
                                        submittedFile.SubmitterContactPhone1 = segments[6];
                                        break;
                                    case "EX":
                                        submittedFile.SubmitterContactPhoneEx1 = segments[6];
                                        break;
                                }
                            }
                            if (segments.Length > 7)
                            {
                                switch (segments[7])
                                {
                                    case "EM":
                                        submittedFile.SubmitterContactEmail1 = segments[8];
                                        break;
                                    case "FX":
                                        submittedFile.SubmitterContactFax1 = segments[8];
                                        break;
                                    case "TE":
                                        submittedFile.SubmitterContactPhone1 = segments[8];
                                        break;
                                    case "EX":
                                        submittedFile.SubmitterContactPhoneEx1 = segments[8];
                                        break;
                                }
                            }
                        }
                    }
                    else if (LoopName == "2010BA")
                    {
                        claim.Subscribers.Last().SubscriberContactName = segments[2];
                        claim.Subscribers.Last().SubscriberContactPhone = segments[4];
                        if (segments.Length > 6) claim.Subscribers.Last().SubscriberContactPhoneEx = segments[6];
                    }
                    else if (LoopName == "2010CA")
                    {
                        claim.Patients.Last().PatientContactName = segments[2];
                        claim.Patients.Last().PatientContactPhone = segments[4];
                        if (segments.Length > 6) claim.Patients.Last().PatientContactPhoneEx = segments[6];
                    }
                    else if (LoopName == "2420E")
                    {
                        providercontact = new ProviderContact();
                        providercontact.ClaimID = claim.Header.ClaimID;
                        providercontact.FileID = submittedFile.FileID;
                        providercontact.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                        providercontact.LoopName = "2420E";
                        providercontact.ProviderNPI = claim.Providers.Last().ProviderID;
                        providercontact.ProviderQualifier = segments[1];
                        providercontact.ContactName = segments[2];
                        switch (segments[3])
                        {
                            case "EM":
                                providercontact.Email = segments[4];
                                break;
                            case "FX":
                                providercontact.Fax = segments[4];
                                break;
                            case "TE":
                                providercontact.Phone = segments[4];
                                break;
                        }
                        if (segments.Length > 5)
                        {
                            switch (segments[5])
                            {
                                case "EM":
                                    providercontact.Email = segments[6];
                                    break;
                                case "FX":
                                    providercontact.Fax = segments[6];
                                    break;
                                case "TE":
                                    providercontact.Phone = segments[6];
                                    break;
                                case "EX":
                                    providercontact.PhoneEx = segments[6];
                                    break;
                            }

                        }
                        if (segments.Length > 7)
                        {
                            switch (segments[7])
                            {
                                case "EM":
                                    providercontact.Email = segments[8];
                                    break;
                                case "FX":
                                    providercontact.Fax = segments[8];
                                    break;
                                case "TE":
                                    providercontact.Phone = segments[8];
                                    break;
                                case "EX":
                                    providercontact.PhoneEx = segments[8];
                                    break;
                            }
                        }
                        claim.ProviderContacts.Add(providercontact);
                    }
                    else
                    {
                        providercontact = new ProviderContact();
                        providercontact.FileID = submittedFile.FileID;
                        providercontact.ClaimID = claim.Header.ClaimID;
                        providercontact.ServiceLineNumber = "0";
                        providercontact.LoopName = LoopName;
                        providercontact.ProviderNPI = claim.Providers.Last().ProviderID;
                        providercontact.ProviderQualifier = segments[1];
                        providercontact.ContactName = segments[2];
                        switch (segments[3])
                        {
                            case "EM":
                                providercontact.Email = segments[4];
                                break;
                            case "FX":
                                providercontact.Fax = segments[4];
                                break;
                            case "TE":
                                providercontact.Phone = segments[4];
                                break;
                        }
                        if (segments.Length > 5)
                        {
                            switch (segments[5])
                            {
                                case "EM":
                                    providercontact.Email = segments[6];
                                    break;
                                case "FX":
                                    providercontact.Fax = segments[6];
                                    break;
                                case "TE":
                                    providercontact.Phone = segments[6];
                                    break;
                                case "EX":
                                    providercontact.PhoneEx = segments[6];
                                    break;
                            }

                        }
                        if (segments.Length > 7)
                        {
                            switch (segments[7])
                            {
                                case "EM":
                                    providercontact.Email = segments[8];
                                    break;
                                case "FX":
                                    providercontact.Fax = segments[8];
                                    break;
                                case "TE":
                                    providercontact.Phone = segments[8];
                                    break;
                                case "EX":
                                    providercontact.PhoneEx = segments[8];
                                    break;
                            }
                        }
                        claim.ProviderContacts.Add(providercontact);
                    }
                    break;
                case "NM1":
                    switch (segments[1])
                    {
                        case "41":
                            LoopName = "1000A";
                            break;
                        case "40":
                            LoopName = "1000B";
                            break;
                        case "85":
                            //billing provider
                            if (LoopName == "2000A")
                            {
                                claim.Providers.Last().ProviderLastName = segments[3];
                                if (segments.Length > 4) claim.Providers.Last().ProviderFirstName = segments[4];
                                if (segments.Length > 5) claim.Providers.Last().ProviderMiddle = segments[5];
                                if (segments.Length > 7) claim.Providers.Last().ProviderSuffix = segments[7];
                                if (segments.Length > 8) claim.Providers.Last().ProviderIDQualifier = segments[8];
                                if (segments.Length > 9) claim.Providers.Last().ProviderID = segments[9];
                                LoopName = "2010AA";
                            }
                            else if (string.Compare(LoopName, "2331") == -1)
                            {
                                if (submittedFile.FileType == "005010X222A1")
                                {
                                    //professional other payer billing provider
                                    LoopName = "2330G";
                                    provider = new ClaimProvider();
                                    provider.ClaimID = claim.Header.ClaimID;
                                    provider.FileID = submittedFile.FileID;
                                    provider.ServiceLineNumber = "0";
                                    provider.LoopName = "2330G";
                                    provider.ProviderQualifier = "85";
                                    provider.RepeatSequence = claim.Subscribers.Last().SubscriberSequenceNumber;
                                    claim.Providers.Add(provider);
                                }
                                else if (submittedFile.FileType == "005010X222A2")
                                {
                                    //institutional other payer billing provider
                                    LoopName = "2330I";
                                    provider = new ClaimProvider();
                                    provider.ClaimID = claim.Header.ClaimID;
                                    provider.FileID = submittedFile.FileID;
                                    provider.ServiceLineNumber = "0";
                                    provider.LoopName = "2330I";
                                    provider.ProviderQualifier = "85";
                                    provider.RepeatSequence = claim.Subscribers.Last().SubscriberSequenceNumber;
                                    claim.Providers.Add(provider);
                                }
                            }
                            break;
                        case "87":
                            provider = new ClaimProvider();
                            provider.FileID = submittedFile.FileID;
                            provider.ClaimID = claim.Header.ClaimID;
                            provider.LoopName = "2010AB";
                            provider.ServiceLineNumber = "0";
                            provider.ProviderQualifier = "87";
                            claim.Providers.Add(provider);
                            LoopName = "2010AB";
                            //"PayToProvier";
                            break;
                        case "PE":
                            provider = new ClaimProvider();
                            provider.FileID = submittedFile.FileID;
                            provider.LoopName = "2010AC";
                            provider.ServiceLineNumber = "0";
                            provider.ProviderQualifier = "PE";
                            provider.ProviderLastName = segments[3];
                            provider.ProviderIDQualifier = segments[8];
                            provider.ProviderID = segments[9];
                            claim.Providers.Add(provider);
                            LoopName = "2010AC";
                            //"PayToPlan";
                            break;
                        case "IL":
                            if (LoopName == "2000B")
                            {
                                claim.Subscribers.Last().LastName = segments[3];
                                if (segments.Length > 4) claim.Subscribers.Last().FirstName = segments[4];
                                if (segments.Length > 5) claim.Subscribers.Last().MidddleName = segments[5];
                                if (segments.Length > 7) claim.Subscribers.Last().NameSuffix = segments[7];
                                if (segments.Length > 8) claim.Subscribers.Last().IDQualifier = segments[8];
                                if (segments.Length > 9) claim.Subscribers.Last().IDCode = segments[9];
                                LoopName = "2010BA";
                            }
                            else if (string.Compare(LoopName, "2331") == -1)
                            {
                                LoopName = "2330A";
                                claim.Subscribers.Last().LastName = segments[3];
                                claim.Subscribers.Last().FirstName = segments[4];
                                claim.Subscribers.Last().MidddleName = segments[5];
                                claim.Subscribers.Last().NameSuffix = segments[7];
                                claim.Subscribers.Last().IDQualifier = segments[8];
                                claim.Subscribers.Last().IDCode = segments[9];
                            }
                            break;
                        case "PR":
                            if (string.Compare(LoopName, "2011") == -1)
                            {
                                LoopName = "2010BB";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = "0";
                                provider.LoopName = "2010BB";
                                provider.ProviderQualifier = "PR";
                                provider.ProviderLastName = segments[3];
                                if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                if (segments.Length > 9) provider.ProviderID = segments[9];
                                claim.Providers.Add(provider);
                                //"Payer";
                            }
                            else if (string.Compare(LoopName, "2331") == -1)
                            {
                                //"OtherPayer";
                                LoopName = "2330B";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = "0";
                                provider.LoopName = "2330B";
                                provider.ProviderQualifier = "PR";
                                provider.ProviderLastName = segments[3];
                                provider.ProviderIDQualifier = segments[8];
                                provider.ProviderID = segments[9];
                                claim.Providers.Add(provider);
                            }
                            break;
                        case "QC":
                            if (LoopName == "2010CA")
                            {
                                claim.Patients.Last().PatientLastName = segments[3];
                                if (segments.Length > 4) claim.Patients.Last().PatientFirstName = segments[4];
                                if (segments.Length > 5) claim.Patients.Last().PatientMiddle = segments[5];
                                if (segments.Length > 7) claim.Patients.Last().PatientSuffix = segments[7];
                            }
                            //"Patient";
                            break;
                        case "DN":
                            if (string.Compare(LoopName, "2311") == -1)
                            {
                                if (submittedFile.FileType == "005010X222A1")
                                {
                                    //professional ReferringProvider
                                    LoopName = "2310A";
                                    provider = new ClaimProvider();
                                    provider.ClaimID = claim.Header.ClaimID;
                                    provider.FileID = submittedFile.FileID;
                                    provider.ServiceLineNumber = "0";
                                    provider.LoopName = "2310A";
                                    provider.ProviderQualifier = "DN";
                                    provider.ProviderLastName = segments[3];
                                    if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                    if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                    if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                    if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                    if (segments.Length > 9) provider.ProviderID = segments[9];
                                    provider.RepeatSequence = (claim.Providers.Count + 1).ToString();
                                    claim.Providers.Add(provider);
                                }
                                else if (submittedFile.FileType == "005010X222A2")
                                {
                                    //institutional ReferringProvider
                                    LoopName = "2310F";
                                    provider = new ClaimProvider();
                                    provider.ClaimID = claim.Header.ClaimID;
                                    provider.FileID = submittedFile.FileID;
                                    provider.ServiceLineNumber = "0";
                                    provider.LoopName = "2310F";
                                    provider.ProviderQualifier = "DN";
                                    provider.ProviderLastName = segments[3];
                                    if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                    if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                    if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                    if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                    if (segments.Length > 9) provider.ProviderID = segments[9];
                                    claim.Providers.Add(provider);
                                }
                            }
                            else if (string.Compare(LoopName, "2331") == -1)
                            {
                                if (submittedFile.FileType == "005010X222A1")
                                {
                                    //professional other payer referring provider
                                    LoopName = "2330C";
                                    provider = new ClaimProvider();
                                    provider.ClaimID = claim.Header.ClaimID;
                                    provider.FileID = submittedFile.FileID;
                                    provider.ServiceLineNumber = "0";
                                    provider.LoopName = "2330C";
                                    provider.ProviderQualifier = "DN";
                                    provider.RepeatSequence = (claim.Providers.Count + 1).ToString();
                                    claim.Providers.Add(provider);
                                }
                                else if (submittedFile.FileType == "005010X222A2")
                                {
                                    //institutional other payer referring provider
                                    LoopName = "2330H";
                                    provider = new ClaimProvider();
                                    provider.ClaimID = claim.Header.ClaimID;
                                    provider.FileID = submittedFile.FileID;
                                    provider.ServiceLineNumber = "0";
                                    provider.LoopName = "2330H";
                                    provider.ProviderQualifier = "DN";
                                    provider.RepeatSequence = (claim.Providers.Count + 1).ToString();
                                    claim.Providers.Add(provider);
                                }
                            }
                            else if (string.Compare(LoopName, "2421") == -1)
                            {
                                if (submittedFile.FileType == "005010X222A1")
                                {
                                    //profewssional line level referring provider
                                    LoopName = "2420F";
                                    provider = new ClaimProvider();
                                    provider.ClaimID = claim.Header.ClaimID;
                                    provider.FileID = submittedFile.FileID;
                                    provider.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                                    provider.LoopName = "2420F";
                                    provider.ProviderQualifier = "DN";
                                    provider.ProviderLastName = segments[3];
                                    if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                    if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                    if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                    if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                    if (segments.Length > 9) provider.ProviderID = segments[9];
                                    claim.Providers.Add(provider);
                                }
                                else if (submittedFile.FileType == "005010X222A2")
                                {
                                    //institutional line level referring provider
                                    LoopName = "2420D";
                                    provider = new ClaimProvider();
                                    provider.ClaimID = claim.Header.ClaimID;
                                    provider.FileID = submittedFile.FileID;
                                    provider.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                                    provider.LoopName = "2420D";
                                    provider.ProviderQualifier = "DN";
                                    provider.ProviderLastName = segments[3];
                                    if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                    if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                    if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                    if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                    if (segments.Length > 9) provider.ProviderID = segments[9];
                                    claim.Providers.Add(provider);
                                }
                            }
                            break;
                        case "P3":
                            if (string.Compare(LoopName, "2311") == -1)
                            {
                                //primary care provider for the referring provider
                                LoopName = "2310A";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = "0";
                                provider.LoopName = "2310A";
                                provider.ProviderQualifier = "P3";
                                provider.ProviderLastName = segments[3];
                                if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                if (segments.Length > 9) provider.ProviderID = segments[9];
                                claim.Providers.Add(provider);
                            }
                            else if (string.Compare(LoopName, "2421") == -1)
                            {
                                LoopName = "2420F";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                                provider.LoopName = "2420F";
                                provider.ProviderQualifier = "P3";
                                provider.ProviderLastName = segments[3];
                                if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                if (segments.Length > 9) provider.ProviderID = segments[9];
                                claim.Providers.Add(provider);
                            }
                            break;
                        case "82":
                            //rendering provider
                            if (string.Compare(LoopName, "2311") == -1)
                            {
                                if (submittedFile.FileType == "005010X222A1")
                                {
                                    //professional RenderingProvider
                                    LoopName = "2310B";
                                    provider = new ClaimProvider();
                                    provider.ClaimID = claim.Header.ClaimID;
                                    provider.FileID = submittedFile.FileID;
                                    provider.ServiceLineNumber = "0";
                                    provider.LoopName = "2310B";
                                    provider.ProviderQualifier = "82";
                                    provider.ProviderLastName = segments[3];
                                    if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                    if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                    if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                    if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                    if (segments.Length > 9) provider.ProviderID = segments[9];
                                    claim.Providers.Add(provider);
                                }
                                else if (submittedFile.FileType == "005010X222A2")
                                {
                                    //institutional rendering provider
                                    LoopName = "2310D";
                                    provider = new ClaimProvider();
                                    provider.ClaimID = claim.Header.ClaimID;
                                    provider.FileID = submittedFile.FileID;
                                    provider.ServiceLineNumber = "0";
                                    provider.LoopName = "2310D";
                                    provider.ProviderQualifier = "82";
                                    provider.ProviderLastName = segments[3];
                                    if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                    if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                    if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                    if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                    if (segments.Length > 9) provider.ProviderID = segments[9];
                                    claim.Providers.Add(provider);
                                }

                            }
                            else if (string.Compare(LoopName, "2331") == -1)
                            {
                                if (submittedFile.FileType == "005010X222A1")
                                {
                                    //professional other payer rendering provider
                                    LoopName = "2330D";
                                    provider = new ClaimProvider();
                                    provider.ClaimID = claim.Header.ClaimID;
                                    provider.FileID = submittedFile.FileID;
                                    provider.ServiceLineNumber = "0";
                                    provider.LoopName = "2330D";
                                    provider.ProviderQualifier = "82";
                                    provider.RepeatSequence = claim.Subscribers.Last().SubscriberSequenceNumber;
                                    claim.Providers.Add(provider);
                                }
                                else if (submittedFile.FileType == "005010X222A2")
                                {
                                    //institutional other payer rendering provider
                                    LoopName = "2330G";
                                    provider = new ClaimProvider();
                                    provider.ClaimID = claim.Header.ClaimID;
                                    provider.FileID = submittedFile.FileID;
                                    provider.ServiceLineNumber = "0";
                                    provider.LoopName = "2330G";
                                    provider.ProviderQualifier = "82";
                                    provider.RepeatSequence = claim.Subscribers.Last().SubscriberSequenceNumber;
                                    claim.Providers.Add(provider);
                                }
                            }
                            else if (string.Compare(LoopName, "2421") == -1)
                            {
                                if (submittedFile.FileType == "005010X222A1")
                                {
                                    //professional line level rendering provider
                                    LoopName = "2420A";
                                    provider = new ClaimProvider();
                                    provider.ClaimID = claim.Header.ClaimID;
                                    provider.FileID = submittedFile.FileID;
                                    provider.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                                    provider.LoopName = "2420A";
                                    provider.ProviderQualifier = "82";
                                    provider.ProviderLastName = segments[3];
                                    if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                    if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                    if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                    if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                    if (segments.Length > 9) provider.ProviderID = segments[9];
                                    claim.Providers.Add(provider);
                                }
                                else if (submittedFile.FileType == "005010X222A2")
                                {
                                    //institutional line level rendering provider
                                    LoopName = "2420C";
                                    provider = new ClaimProvider();
                                    provider.ClaimID = claim.Header.ClaimID;
                                    provider.FileID = submittedFile.FileID;
                                    provider.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                                    provider.LoopName = "2420C";
                                    provider.ProviderQualifier = "82";
                                    provider.ProviderLastName = segments[3];
                                    if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                    if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                    if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                    if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                    if (segments.Length > 9) provider.ProviderID = segments[9];
                                    claim.Providers.Add(provider);
                                }
                            }
                            break;
                        case "77":
                            //service facility
                            if (string.Compare(LoopName, "2311") == -1)
                            {
                                if (submittedFile.FileType == "005010X222A1")
                                {
                                    //professional ServiceFacility
                                    LoopName = "2310C";
                                    provider = new ClaimProvider();
                                    provider.ClaimID = claim.Header.ClaimID;
                                    provider.FileID = submittedFile.FileID;
                                    provider.ServiceLineNumber = "0";
                                    provider.LoopName = "2310C";
                                    provider.ProviderQualifier = "77";
                                    provider.ProviderLastName = segments[3];
                                    if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                    if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                    if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                    if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                    if (segments.Length > 9) provider.ProviderID = segments[9];
                                    claim.Providers.Add(provider);
                                }
                                else if (submittedFile.FileType == "005010X222A2")
                                {
                                    //institutional ServiceFacility
                                    LoopName = "2310E";
                                    provider = new ClaimProvider();
                                    provider.ClaimID = claim.Header.ClaimID;
                                    provider.FileID = submittedFile.FileID;
                                    provider.ServiceLineNumber = "0";
                                    provider.LoopName = "2310E";
                                    provider.ProviderQualifier = "77";
                                    provider.ProviderLastName = segments[3];
                                    if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                    if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                    if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                    if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                    if (segments.Length > 9) provider.ProviderID = segments[9];
                                    claim.Providers.Add(provider);
                                }
                            }
                            else if (string.Compare(LoopName, "2331") == -1)
                            {
                                if (submittedFile.FileType == "005010X222A1")
                                {
                                    //professional other payer service facility
                                    LoopName = "2330E";
                                    provider = new ClaimProvider();
                                    provider.ClaimID = claim.Header.ClaimID;
                                    provider.FileID = submittedFile.FileID;
                                    provider.ServiceLineNumber = "0";
                                    provider.LoopName = "2330E";
                                    provider.ProviderQualifier = "77";
                                    provider.RepeatSequence = claim.Subscribers.Last().SubscriberSequenceNumber;
                                    claim.Providers.Add(provider);
                                }
                                else if (submittedFile.FileType == "005010X222A2")
                                {
                                    //institutional other payer service facility
                                    LoopName = "2330F";
                                    provider = new ClaimProvider();
                                    provider.ClaimID = claim.Header.ClaimID;
                                    provider.FileID = submittedFile.FileID;
                                    provider.ServiceLineNumber = "0";
                                    provider.LoopName = "2330F";
                                    provider.ProviderQualifier = "77";
                                    provider.RepeatSequence = claim.Subscribers.Last().SubscriberSequenceNumber;
                                    claim.Providers.Add(provider);
                                }
                            }
                            else if (string.Compare(LoopName, "2421") == -1)
                            {
                                LoopName = "2420C";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                                provider.LoopName = "2420C";
                                provider.ProviderQualifier = "77";
                                provider.ProviderLastName = segments[3];
                                if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                if (segments.Length > 9) provider.ProviderID = segments[9];
                                claim.Providers.Add(provider);
                            }
                            break;
                        case "DQ":
                            //supervising provider
                            if (string.Compare(LoopName, "2311") == -1)
                            {
                                LoopName = "2310D";
                                //"SupervisingProvider";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = "0";
                                provider.LoopName = "2310D";
                                provider.ProviderQualifier = "DQ";
                                provider.ProviderLastName = segments[3];
                                if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                if (segments.Length > 9) provider.ProviderID = segments[9];
                                claim.Providers.Add(provider);
                            }
                            else if (string.Compare(LoopName, "2331") == -1)
                            {
                                LoopName = "2330F";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = "0";
                                provider.LoopName = "2330F";
                                provider.ProviderQualifier = "DQ";
                                provider.RepeatSequence = claim.Subscribers.Last().SubscriberSequenceNumber;
                                claim.Providers.Add(provider);
                            }
                            else if (string.Compare(LoopName, "2421") == -1)
                            {
                                LoopName = "2420D";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                                provider.LoopName = "2420D";
                                provider.ProviderQualifier = "DQ";
                                provider.ProviderLastName = segments[3];
                                if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                if (segments.Length > 9) provider.ProviderID = segments[9];
                                claim.Providers.Add(provider);
                            }
                            break;
                        case "PW":
                            //ambulance pick-up
                            if (string.Compare(LoopName, "2311") == -1)
                            {
                                LoopName = "2310E";
                                //"AmbulancePickUp";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = "0";
                                provider.LoopName = "2310E";
                                provider.ProviderQualifier = "PW";
                                claim.Providers.Add(provider);
                            }
                            else if (string.Compare(LoopName, "2421") == -1)
                            {
                                LoopName = "2420G";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                                provider.LoopName = "2420G";
                                provider.ProviderQualifier = "PW";
                                claim.Providers.Add(provider);
                            }
                            break;
                        case "45":
                            //ambulance drop-off
                            if (string.Compare(LoopName, "2311") == -1)
                            {
                                LoopName = "2310F";
                                //"AnbulanceDropOff";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = "0";
                                provider.LoopName = "2310F";
                                provider.ProviderQualifier = "45";
                                if (segments.Length > 3) provider.ProviderLastName = segments[3];
                                claim.Providers.Add(provider);
                            }
                            else if (string.Compare(LoopName, "2421") == -1)
                            {
                                LoopName = "2420H";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                                provider.LoopName = "2420H";
                                provider.ProviderQualifier = "45";
                                if (segments.Length > 3) provider.ProviderLastName = segments[3];
                                claim.Providers.Add(provider);
                            }
                            break;
                        case "QB":
                            if (string.Compare(LoopName, "2421") == -1)
                            {
                                LoopName = "2420B";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                                provider.LoopName = "2420B";
                                provider.ProviderQualifier = "QB";
                                if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                if (segments.Length > 9) provider.ProviderID = segments[9];
                                claim.Providers.Add(provider);
                            }
                            break;
                        case "DK":
                            if (string.Compare(LoopName, "2421") == -1)
                            {
                                LoopName = "2420E";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                                provider.LoopName = "2420E";
                                provider.ProviderQualifier = "DK";
                                provider.ProviderLastName = segments[3];
                                if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                if (segments.Length > 9) provider.ProviderID = segments[9];
                                claim.Providers.Add(provider);

                            }
                            break;
                        case "71":
                            //institutional attending provider
                            if (string.Compare(LoopName, "2311") == -1)
                            {
                                LoopName = "2310A";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = "0";
                                provider.LoopName = "2310A";
                                provider.ProviderQualifier = "71";
                                provider.ProviderLastName = segments[3];
                                if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                if (segments.Length > 9) provider.ProviderID = segments[9];
                                claim.Providers.Add(provider);
                            }
                            else if (string.Compare(LoopName, "2331") == -1)
                            {
                                LoopName = "2330C";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = "0";
                                provider.LoopName = "2330C";
                                provider.ProviderQualifier = "71";
                                claim.Providers.Add(provider);
                            }
                            break;
                        case "72":
                            //institutional operating provider
                            if (string.Compare(LoopName, "2311") == -1)
                            {
                                LoopName = "2310B";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = "0";
                                provider.LoopName = "2310B";
                                provider.ProviderQualifier = "72";
                                provider.ProviderLastName = segments[3];
                                if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                if (segments.Length > 9) provider.ProviderID = segments[9];
                                claim.Providers.Add(provider);
                            }
                            else if (string.Compare(LoopName, "2331") == -1)
                            {
                                LoopName = "2330D";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = "0";
                                provider.LoopName = "2330D";
                                provider.ProviderQualifier = "72";
                                claim.Providers.Add(provider);
                            }
                            else if (string.Compare(LoopName, "2421") == -1)
                            {
                                LoopName = "2420A";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                                provider.LoopName = "2420A";
                                provider.ProviderQualifier = "72";
                                provider.ProviderLastName = segments[3];
                                if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                if (segments.Length > 9) provider.ProviderID = segments[9];
                                claim.Providers.Add(provider);
                            }
                            break;
                        case "ZZ":
                            //institutional other operating provider
                            if (string.Compare(LoopName, "2311") == -1)
                            {
                                LoopName = "2310C";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = "0";
                                provider.LoopName = "2310C";
                                provider.ProviderQualifier = "ZZ";
                                provider.ProviderLastName = segments[3];
                                if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                if (segments.Length > 9) provider.ProviderID = segments[9];
                                claim.Providers.Add(provider);
                            }
                            else if (string.Compare(LoopName, "2331") == -1)
                            {
                                LoopName = "2330E";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = "0";
                                provider.LoopName = "2330E";
                                provider.ProviderQualifier = "ZZ";
                                claim.Providers.Add(provider);
                            }
                            else if (string.Compare(LoopName, "2421") == -1)
                            {
                                LoopName = "2420B";
                                provider = new ClaimProvider();
                                provider.ClaimID = claim.Header.ClaimID;
                                provider.FileID = submittedFile.FileID;
                                provider.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                                provider.LoopName = "2420B";
                                provider.ProviderQualifier = "ZZ";
                                provider.ProviderLastName = segments[3];
                                if (segments.Length > 4) provider.ProviderFirstName = segments[4];
                                if (segments.Length > 5) provider.ProviderMiddle = segments[5];
                                if (segments.Length > 7) provider.ProviderSuffix = segments[7];
                                if (segments.Length > 8) provider.ProviderIDQualifier = segments[8];
                                if (segments.Length > 9) provider.ProviderID = segments[9];
                                claim.Providers.Add(provider);
                            }
                            break;
                    }
                    break;
                case "HL":
                    //save last claim
                    switch (segments[3])
                    {
                        case "20":
                            if (saveFlag)
                            {
                                InitilizeClaim("BillingProvider", ref claim, ref claims, ref submittedFile);
                                saveFlag = false;
                            }
                            LoopName = "2000A";
                            provider = new ClaimProvider();
                            provider.FileID = submittedFile.FileID;
                            provider.ClaimID = claim.Header.ClaimID;
                            provider.LoopName = "2000A";
                            provider.ProviderQualifier = "85";
                            provider.ServiceLineNumber = "0";
                            claim.Providers.Add(provider);
                            break;
                        case "22":
                            if (saveFlag)
                            {
                                InitilizeClaim("Subscriber", ref claim, ref claims, ref submittedFile);
                                saveFlag = false;
                            }
                            LoopName = "2000B";
                            sbr = new ClaimSBR();
                            sbr.FileID = submittedFile.FileID;
                            sbr.ClaimID = claim.Header.ClaimID;
                            sbr.LoopName = "2000B";
                            claim.Subscribers.Add(sbr);
                            break;
                        case "23":
                            if (saveFlag)
                            {
                                InitilizeClaim("Patient", ref claim, ref claims, ref submittedFile);
                                saveFlag = false;
                            }
                            LoopName = "2000C";
                            patient = new ClaimPatient();
                            patient.FileID = submittedFile.FileID;
                            patient.ClaimID = claim.Header.ClaimID;
                            claim.Patients.Add(patient);
                            break;
                    }
                    break;
                case "PRV":
                    if (LoopName == "2000A" || LoopName == "2310B" || LoopName == "2420A")
                    {
                        claim.Providers.Last().ProviderTaxonomyCode = segments[3];
                    }
                    if (LoopName == "2310A" && segments[1] == "AT")
                    {
                        claim.Providers.Last().ProviderTaxonomyCode = segments[3];
                    }
                    break;
                case "CUR":
                    if (LoopName == "2000A")
                    {
                        claim.Providers.Last().ProviderCurrencyCode = segments[2];
                    }
                    break;
                case "N3":
                    switch (LoopName)
                    {
                        case "2010AA":
                        case "2010AB":
                        case "2010AC":
                        case "2010BB":
                        case "2310C":
                        case "2310E":
                        case "2330B":
                        case "2420C":
                        case "2420E":
                        case "2420G":
                            claim.Providers.Last().ProviderAddress = segments[1];
                            if (segments.Length > 2) claim.Providers.Last().ProviderAddress2 = segments[2];
                            break;
                        case "2010BA":
                        case "2330A":
                            claim.Subscribers.Last().SubscriberAddress = segments[1];
                            if (segments.Length > 2) claim.Subscribers.Last().SubscriberAddress2 = segments[2];
                            break;
                        case "2010CA":
                            claim.Patients.Last().PatientAddress = segments[1];
                            if (segments.Length > 2) claim.Patients.Last().PatientAddress2 = segments[2];
                            break;
                    }
                    break;
                case "N4":
                    switch (LoopName)
                    {
                        case "2010AA":
                        case "2010AB":
                        case "2010AC":
                        case "2010BB":
                        case "2310C":
                        case "2310E":
                        case "2330B":
                        case "2420C":
                        case "2420E":
                        case "2420G":
                            claim.Providers.Last().ProviderCity = segments[1];
                            if (segments.Length > 2) claim.Providers.Last().ProviderState = segments[2];
                            if (segments.Length > 3) claim.Providers.Last().ProviderZip = segments[3];
                            if (segments.Length > 4) claim.Providers.Last().ProviderCountry = segments[4];
                            if (segments.Length > 7) claim.Providers.Last().ProviderCountrySubCode = segments[7];
                            break;
                        case "2010BA":
                        case "2330A":
                            claim.Subscribers.Last().SubscriberCity = segments[1];
                            if (segments.Length > 2) claim.Subscribers.Last().SubscriberState = segments[2];
                            if (segments.Length > 3) claim.Subscribers.Last().SubscriberZip = segments[3];
                            if (segments.Length > 4) claim.Subscribers.Last().SubscriberCountry = segments[4];
                            if (segments.Length > 7) claim.Subscribers.Last().SubscriberCountrySubCode = segments[7];
                            break;
                        case "2010CA":
                            claim.Patients.Last().PatientCity = segments[1];
                            if (segments.Length > 2) claim.Patients.Last().PatientState = segments[2];
                            if (segments.Length > 3) claim.Patients.Last().PatientZip = segments[3];
                            if (segments.Length > 4) claim.Patients.Last().PatientCountry = segments[4];
                            if (segments.Length > 7) claim.Patients.Last().PatientCountrySubCode = segments[7];
                            break;
                    }
                    break;
                case "REF":
                    if (LoopName == "2010AA" || LoopName == "2010AC" || LoopName == "2010BB")
                    {
                        secondaryIdentification = new ClaimSecondaryIdentification();
                        secondaryIdentification.FileID = submittedFile.FileID;
                        secondaryIdentification.ServiceLineNumber = "0";
                        secondaryIdentification.LoopName = LoopName;
                        secondaryIdentification.ProviderQualifier = segments[1];
                        secondaryIdentification.ProviderID = segments[2];
                        claim.SecondaryIdentifications.Add(secondaryIdentification);
                    }
                    else if (LoopName == "2010BA")
                    {
                        switch (segments[1])
                        {
                            case "SY":
                                claim.Subscribers.Last().SubscriberSSN = segments[2];
                                break;
                            case "Y4":
                                claim.Subscribers.Last().SubscriberClaimNumber = segments[2];
                                break;
                        }
                    }
                    else if (LoopName == "2010CA")
                    {
                        switch (segments[1])
                        {
                            case "Y4":
                                claim.Patients.Last().PatientClaimNumber = segments[2]; //property and casualty claim number
                                break;
                            case "1W":
                                claim.Patients.Last().PatientMemberID = segments[2];  //member identification number
                                break;
                            case "SY":
                                claim.Patients.Last().PatientSSN = segments[2]; //member social security number
                                break;
                        }
                    }
                    else if (LoopName == "2300")
                    {
                        switch (segments[1])
                        {
                            case "4N":
                                claim.Header.ServiceAuthorizationExceptionCode = segments[2];
                                break;
                            case "F5":
                                claim.Header.MedicareSection4081Indicator = segments[2];
                                break;
                            case "EW":
                                claim.Header.MammographyCertificationNumber = segments[2];
                                break;
                            case "9F":
                                claim.Header.ReferralNumber = segments[2];
                                break;
                            case "G1":
                                claim.Header.PriorAuthorizationNumber = segments[2];
                                break;
                            case "F8":
                                claim.Header.PayerClaimControlNumber = segments[2];
                                break;
                            case "X4":
                                claim.Header.ClinicalLabNumber = segments[2];
                                break;
                            case "9A":
                                claim.Header.RepricedClaimNumber = segments[2];
                                break;
                            case "9C":
                                claim.Header.AdjustedClaimNumber = segments[2];
                                break;
                            case "LX":
                                claim.Header.InvestigationalID = segments[2];
                                break;
                            case "D9":
                                claim.Header.ValueAddedNetworkTraceNumber = segments[2];
                                break;
                            case "EA":
                                claim.Header.MedicalRecordNumber = segments[2];
                                break;
                            case "P4":
                                claim.Header.DemonstrationID = segments[2];
                                break;
                            case "1J":
                                claim.Header.CarePlanOversightNumber = segments[2];
                                break;
                        }
                    }
                    else if (LoopName == "2310A" || LoopName == "2310B" || LoopName == "2310C" || LoopName == "2310D" || LoopName == "2330B" || LoopName == "2330C" || LoopName == "2330D" || LoopName == "2330E" || LoopName == "2330F" || LoopName == "2330G")
                    {
                        secondaryIdentification = new ClaimSecondaryIdentification();
                        secondaryIdentification.FileID = submittedFile.FileID;
                        secondaryIdentification.ServiceLineNumber = "0";
                        secondaryIdentification.LoopName = LoopName;
                        secondaryIdentification.ProviderQualifier = segments[1];
                        secondaryIdentification.ProviderID = segments[2];
                        if (!string.IsNullOrEmpty(claim.Providers.Last().RepeatSequence)) secondaryIdentification.RepeatSequence = claim.Providers.Last().RepeatSequence;
                        claim.SecondaryIdentifications.Add(secondaryIdentification);
                    }
                    else if (LoopName == "2330A")
                    {
                        claim.Subscribers.Last().SubscriberSSN = segments[2];//ssn
                    }
                    else if (LoopName == "2400")
                    {
                        switch (segments[1])
                        {
                            case "9B":
                                //service line repriced item reference number
                                claim.Lines.Last().RepricedLineItemReferenceNumber = segments[2];
                                break;
                            case "9D":
                                //service line adjusted repriced line item reference number
                                claim.Lines.Last().AdjustedRepricedLineItemReferenceNumber = segments[2];
                                break;
                            case "G1":
                                //service line Prior authorization number
                                auth = new ClaimLineAuth();
                                auth.ClaimID = claim.Header.ClaimID;
                                auth.FileID = submittedFile.FileID;
                                auth.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                                auth.ReferenceQualifier = segments[1];
                                auth.ReferralNumber = segments[2];
                                if (segments.Length > 4)
                                {
                                    auth.OtherPayerPrimaryIdentifier = segments[4].Split(':')[1];
                                }
                                claim.Auths.Add(auth);
                                break;
                            case "6R":
                                //service line item control number
                                claim.Lines.Last().LineItemControlNumber = segments[2];
                                break;
                            case "EW":
                                //service line mammography certification number
                                claim.Lines.Last().MammographyCertificationNumber = segments[2];
                                break;
                            case "X4":
                                //service line Clinical Laboratory Improvement Amendment (CLIA) number
                                claim.Lines.Last().ClinicalLabNumber = segments[2];
                                break;
                            case "F4":
                                //service line CLIA facility identification
                                claim.Lines.Last().ReferringCLIANumber = segments[2];
                                break;
                            case "BT":
                                //service line immunization batch number
                                claim.Lines.Last().ImmunizationBatchNumber = segments[2];
                                break;
                            case "9F":
                                //service line referral number
                                auth = new ClaimLineAuth();
                                auth.ClaimID = claim.Header.ClaimID;
                                auth.FileID = submittedFile.FileID;
                                auth.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                                auth.ReferenceQualifier = segments[1];
                                auth.ReferralNumber = segments[2];
                                if (segments.Length > 4)
                                {
                                    auth.OtherPayerPrimaryIdentifier = segments[4].Split(':')[1];
                                }
                                claim.Auths.Add(auth);
                                break;
                        }
                    }
                    else if (string.Compare(LoopName, "2411") == -1)
                    {
                        switch (segments[1])
                        {
                            case "XZ":
                                //service line drug identification
                                claim.Lines.Last().LinkSequenceNumber = segments[2];
                                LoopName = "2410";
                                break;
                            case "VY":
                                //service line drug line sequence number
                                claim.Lines.Last().PharmacyPrescriptionNumber = segments[2];
                                LoopName = "2410";
                                break;
                        }
                    }
                    else if (string.Compare(LoopName, "2421") == -1)
                    {
                        secondaryIdentification = new ClaimSecondaryIdentification();
                        secondaryIdentification.FileID = submittedFile.FileID;
                        secondaryIdentification.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                        secondaryIdentification.LoopName = LoopName;
                        secondaryIdentification.ProviderQualifier = segments[1];
                        secondaryIdentification.ProviderID = segments[2];
                        if (segments.Length > 4) secondaryIdentification.OtherPayerPrimaryIDentification = segments[4].Split(':')[1];
                        if (!string.IsNullOrEmpty(claim.Providers.Last().RepeatSequence)) secondaryIdentification.RepeatSequence = claim.Providers.Last().RepeatSequence;
                        claim.SecondaryIdentifications.Add(secondaryIdentification);
                    }
                    break;
                case "SBR":
                    if (LoopName == "2000B")
                    {
                        claim.Subscribers.Last().SubscriberSequenceNumber = segments[1];
                        if (segments.Length > 2) claim.Subscribers.Last().SubscriberRelationshipCode = segments[2];
                        if (segments.Length > 3) claim.Subscribers.Last().InsuredGroupNumber = segments[3];
                        if (segments.Length > 4) claim.Subscribers.Last().OtherInsuredGroupName = segments[4];
                        if (segments.Length > 5) claim.Subscribers.Last().InsuredTypeCode = segments[5];
                        if (segments.Length > 9) claim.Subscribers.Last().ClaimFilingCode = segments[9];
                    }
                    //loop2320, repeat 10 times
                    //(P), S, T, A, B, C, D, E, F, G, H

                    else if (string.Compare(LoopName, "2321") == -1)
                    {
                        LoopName = "2320";
                        //"OtherSubscriber";
                        sbr = new ClaimSBR();
                        sbr.ClaimID = claim.Header.ClaimID;
                        sbr.FileID = submittedFile.FileID;
                        sbr.LoopName = "2320";
                        sbr.SubscriberSequenceNumber = segments[1];
                        sbr.SubscriberRelationshipCode = segments[2];
                        if (segments.Length > 3) sbr.InsuredGroupNumber = segments[3];
                        if (segments.Length > 4) sbr.OtherInsuredGroupName = segments[4];
                        if (segments.Length > 5) sbr.InsuredTypeCode = segments[5];
                        if (segments.Length > 9) sbr.ClaimFilingCode = segments[9];
                        claim.Subscribers.Add(sbr);
                    }
                    break;
                case "PAT":
                    if (LoopName == "2000B")
                    {
                        if (segments.Length > 6) claim.Subscribers.Last().DeathDate = segments[6];
                        if (segments.Length > 7) claim.Subscribers.Last().Unit = segments[7];
                        if (segments.Length > 8) claim.Subscribers.Last().Weight = segments[8];
                        if (segments.Length > 9) claim.Subscribers.Last().PregnancyIndicator = segments[9];
                    }
                    else if (LoopName == "2000C")
                    {
                        claim.Patients.Last().PatientRelatedCode = segments[1];
                        if (segments.Length > 6) claim.Patients.Last().PatientRelatedDeathDate = segments[6];
                        if (segments.Length > 7) claim.Patients.Last().PatientRelatedUnit = segments[7];
                        if (segments.Length > 8) claim.Patients.Last().PatientRelatedWeight = segments[8];
                        if (segments.Length > 9) claim.Patients.Last().PatientRelatedPregnancyIndicator = segments[9];
                        LoopName = "2010CA";
                    }
                    break;
                case "DMG":
                    switch (LoopName)
                    {
                        case "2010BA":
                            claim.Subscribers.Last().SubscriberBirthDate = segments[2];
                            claim.Subscribers.Last().SubscriberGender = segments[3];
                            break;
                        case "2010CA":
                            claim.Patients.Last().PatientBirthDate = segments[2];
                            claim.Patients.Last().PatientGender = segments[3];
                            break;
                    }
                    break;
                case "CLM":
                    //"Claim";
                    LoopName = "2300";
                    if (saveFlag)
                    {
                        InitilizeClaim("Claim", ref claim, ref claims, ref submittedFile);
                    }
                    claim.Header.ClaimID = segments[1];
                    claim.Header.ClaimAmount = segments[2];
                    claim.Header.ClaimPOS = segments[5].Split(':')[0];
                    claim.Header.ClaimType = segments[5].Split(':')[1];
                    claim.Header.ClaimFrequencyCode = segments[5].Split(':')[2];
                    claim.Header.ClaimProviderSignature = segments[6];
                    claim.Header.ClaimProviderAssignment = segments[7];
                    claim.Header.ClaimBenefitAssignment = segments[8];
                    claim.Header.ClaimReleaseofInformationCode = segments[9];
                    if (segments.Length > 10) claim.Header.ClaimPatientSignatureSourceCode = segments[10];
                    if (segments.Length > 11)
                    {
                        temparray = segments[11].Split(':');
                        claim.Header.ClaimRelatedCausesQualifier = temparray[0];
                        if (temparray.Length > 1) claim.Header.ClaimRelatedCausesCode = temparray[1];
                        if (temparray.Length > 3) claim.Header.ClaimRelatedStateCode = temparray[3];
                        if (temparray.Length > 4) claim.Header.ClaimRelatedCountryCode = temparray[4];
                    }
                    if (segments.Length > 12) claim.Header.ClaimSpecialProgramCode = segments[12];
                    if (segments.Length > 20) claim.Header.ClaimDelayReasonCode = segments[20];
                    saveFlag = true;
                    break;
                case "DTP":
                    switch (segments[1])
                    {
                        case "431":
                            //onset of current illness or injury date
                            if (LoopName == "2300")
                            {
                                claim.Header.CurrentIllnessDate = segments[3];
                            }
                            break;
                        case "454":
                            //initial treatment date
                            if (LoopName == "2300")
                            {
                                claim.Header.InitialTreatmentDate = segments[3];
                            }
                            else if (LoopName == "2400")
                            {
                                //service line initial treatment date
                                claim.Lines.Last().InitialTreatmentDate = segments[3];
                            }
                            break;
                        case "304":
                            //last seen date
                            if (LoopName == "2300")
                            {
                                claim.Header.LastSeenDate = segments[3];
                            }
                            else if (LoopName == "2400")
                            {
                                //service line last seen date
                                claim.Lines.Last().LastSeenDate = segments[3];
                            }
                            break;
                        case "453":
                            //acute manifestation date
                            if (LoopName == "2300")
                            {
                                claim.Header.AcuteManifestestationDate = segments[3];
                            }
                            break;
                        case "439":
                            //accident date
                            if (LoopName == "2300")
                            {
                                claim.Header.AccidentDate = segments[3];
                            }
                            break;
                        case "484":
                            //last menstrual period date
                            if (LoopName == "2300")
                            {
                                claim.Header.LastMenstrualPeriodDate = segments[3];
                            }
                            break;
                        case "455":
                            //last x-ray date
                            if (LoopName == "2300")
                            {
                                claim.Header.LastXrayDate = segments[3];
                            }
                            else if (LoopName == "2400")
                            {
                                //service line last x-ray date
                                claim.Lines.Last().LastXrayDate = segments[3];
                            }
                            break;
                        case "471":
                            //prescription date
                            if (LoopName == "2300")
                            {
                                claim.Header.PrescriptionDate = segments[3];
                            }
                            else if (LoopName == "2400")
                            {
                                claim.Lines.Last().PrescriptionDate = segments[3];
                            }
                            break;
                        case "314":
                            //both disability start and end date are being reported
                            if (LoopName == "2300")
                            {
                                claim.Header.DisabilityStartDate = segments[3].Split('-')[0];
                                claim.Header.DisabilityEndDate = segments[3].Split('-')[1];
                            }
                            break;
                        case "360":
                            //initial disability period start date
                            if (LoopName == "2300")
                            {
                                claim.Header.DisabilityStartDate = segments[3];
                            }
                            break;
                        case "361":
                            //initial disability period end date
                            if (LoopName == "2300")
                            {
                                claim.Header.DisabilityEndDate = segments[3];
                            }
                            break;
                        case "297":
                            //last worked date
                            if (LoopName == "2300")
                            {
                                claim.Header.LastWorkedDate = segments[3];
                            }
                            break;
                        case "296":
                            //work return date
                            if (LoopName == "2300")
                            {
                                claim.Header.AuthorizedReturnToWorkDate = segments[3];
                            }
                            break;
                        case "435":
                            //related hospital admission date
                            if (LoopName == "2300")
                            {
                                claim.Header.AdmissionDate = segments[3];
                            }
                            break;
                        case "096":
                            //related hospital discharge date, for institutional, this will be discharge time
                            if (LoopName == "2300")
                            {
                                claim.Header.DischargeDate = segments[3];
                            }
                            break;
                        case "090":
                            //assumed care date
                            if (LoopName == "2300")
                            {
                                claim.Header.AssumedStartDate = segments[3];
                            }
                            break;
                        case "091":
                            //relinquished care date
                            if (LoopName == "2300")
                            {
                                claim.Header.AssumedEndDate = segments[3];
                            }
                            break;
                        case "444":
                            //property and casualty first contact date
                            if (LoopName == "2300")
                            {
                                claim.Header.FirstContactDate = segments[3];
                            }
                            break;
                        case "050":
                            //repricer received date
                            if (LoopName == "2300")
                            {
                                claim.Header.RepricerReceivedDate = segments[3];
                            }
                            break;
                        case "573":
                            //adjudication or payment date
                            if (LoopName == "2330B")
                            {
                                claim.Subscribers.Last().PaymentDate = segments[3];
                            }
                            else if (string.Compare(LoopName, "2431") == -1)
                            {
                                LoopName = "2430";
                                claim.Lines.Last().AdjudicationDate = segments[2];
                            }
                            break;
                        case "472":
                            //service line service date
                            if (LoopName == "2400")
                            {
                                if (segments[2] == "D8")
                                {
                                    claim.Lines.Last().ServiceFromDate = segments[3];
                                    claim.Lines.Last().ServiceToDate = segments[3];
                                }
                                else if (segments[2] == "RD8")
                                {
                                    claim.Lines.Last().ServiceFromDate = segments[3].Split('-')[0];
                                    claim.Lines.Last().ServiceToDate = segments[3].Split('-')[1];
                                }
                            }
                            break;
                        case "607":
                            //service line certification date
                            claim.Lines.Last().CertificationDate = segments[3];
                            break;
                        case "463":
                            //service line therapy begin date
                            claim.Lines.Last().BeginTherapyDate = segments[3];
                            break;
                        case "461":
                            //service line last certification date
                            claim.Lines.Last().LastCertificationDate = segments[3];
                            break;
                        case "738":
                            claim.Lines.Last().TestDateHemo = segments[3];
                            break;
                        //service line test date
                        case "739":
                            claim.Lines.Last().TestDateSerum = segments[3];
                            break;
                        case "011":
                            //service line shipped date
                            claim.Lines.Last().ShippedDate = segments[3];
                            break;
                        case "434":
                            claim.Header.StatementFromDate = segments[3].Split('-')[0];
                            claim.Header.StatementToDate = segments[3].Split('-')[1];
                            break;
                    }
                    break;
                case "PWK":
                    if (LoopName == "2300")
                    {
                        pwk = new ClaimPWK();
                        pwk.ClaimID = claim.Header.ClaimID;
                        pwk.FileID = submittedFile.FileID;
                        pwk.ServiceLineNumber = "0";
                        pwk.ReportTypeCode = segments[1];
                        pwk.ReportTransmissionCode = segments[2];
                        if (segments.Length > 6) pwk.AttachmentControlNumber = segments[6];
                        claim.PWKs.Add(pwk);
                    }
                    else if (LoopName == "2400")
                    {
                        pwk = new ClaimPWK();
                        pwk.ClaimID = claim.Header.ClaimID;
                        pwk.FileID = submittedFile.FileID;
                        pwk.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                        pwk.ReportTypeCode = segments[1];
                        pwk.ReportTransmissionCode = segments[2];
                        if (segments.Length > 6) pwk.AttachmentControlNumber = segments[6];
                        claim.PWKs.Add(pwk);
                    }
                    break;
                case "CN1":
                    if (LoopName == "2300")
                    {
                        claim.Header.ContractTypeCode = segments[1];
                        if (segments.Length > 2) claim.Header.ContractAmount = segments[2];
                        if (segments.Length > 3) claim.Header.ContractPercentage = segments[3];
                        if (segments.Length > 4) claim.Header.ContractCode = segments[4];
                        if (segments.Length > 5) claim.Header.ContractTermsDiscountPercentage = segments[5];
                        if (segments.Length > 6) claim.Header.ContractVersionIdentifier = segments[6];
                    }
                    else if (LoopName == "2400")
                    {
                        claim.Lines.Last().ContractTypeCode = segments[1];
                        if (segments.Length > 2) claim.Lines.Last().ContractAmount = segments[2];
                        if (segments.Length > 3) claim.Lines.Last().ContractPercentage = segments[3];
                        if (segments.Length > 4) claim.Lines.Last().ContractCode = segments[4];
                        if (segments.Length > 5) claim.Lines.Last().TermsDiscountPercentage = segments[5];
                        if (segments.Length > 6) claim.Lines.Last().ContractVersionIdentifier = segments[6];
                    }

                    break;
                case "AMT":
                    if (LoopName == "2300")
                    {
                        switch (segments[1])
                        {
                            case "F5":
                                claim.Header.PatientPaidAmount = segments[2];
                                break;
                        }
                    }
                    else if (string.Compare(LoopName, "2321") == -1)
                    {
                        //COB
                        LoopName = "2320";
                        switch (segments[1])
                        {
                            case "D":
                                claim.Subscribers.Last().COBPayerPaidAmount = segments[2];
                                break;
                            case "A8":
                                claim.Subscribers.Last().COBNonCoveredAmount = segments[2];
                                break;
                            case "EAF":
                                claim.Subscribers.Last().COBRemainingPatientLiabilityAmount = segments[2];
                                break;
                        }
                    }
                    else if (LoopName == "2400")
                    {
                        switch (segments[1])
                        {
                            case "T":
                            case "GT":
                                //service line sales tax amount
                                claim.Lines.Last().SalesTaxAmount = segments[2];
                                break;
                            case "F4":
                                //service line postage claimed amount
                                claim.Lines.Last().PostageClaimedAmount = segments[2];
                                break;
                            case "N8":
                                claim.Lines.Last().FacilityTaxAmount = segments[2];
                                break;
                        }
                    }
                    else if (string.Compare(LoopName, "2431") == -1)
                    {
                        switch (segments[1])
                        {
                            case "EAF":
                                LoopName = "2430";
                                claim.Lines.Last().ReaminingPatientLiabilityAmount = segments[2];
                                break;
                        }
                    }
                    break;
                case "K3":
                    //file information
                    if (LoopName == "2300")
                    {
                        k3 = new ClaimK3();
                        k3.ClaimID = claim.Header.ClaimID;
                        k3.FileID = submittedFile.FileID;
                        k3.ServiceLineNumber = "0";
                        k3.K3 = segments[1];
                        claim.K3s.Add(k3);
                    }
                    else if (LoopName == "2400")
                    {
                        k3 = new ClaimK3();
                        k3.ClaimID = claim.Header.ClaimID;
                        k3.FileID = submittedFile.FileID;
                        k3.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                        k3.K3 = segments[1];
                        claim.K3s.Add(k3);
                    }
                    break;
                case "NTE":
                    //claim note
                    if (LoopName == "2300")
                    {
                        nte = new ClaimNte();
                        nte.ClaimID = claim.Header.ClaimID;
                        nte.FileID = submittedFile.FileID;
                        nte.ServiceLineNumber = "0";
                        nte.NoteCode = segments[1];
                        nte.NoteText = segments[2];
                        claim.Notes.Add(nte);
                    }
                    else if (LoopName == "2400")
                    {
                        nte = new ClaimNte();
                        nte.ClaimID = claim.Header.ClaimID;
                        nte.FileID = submittedFile.FileID;
                        nte.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                        nte.NoteCode = segments[1];
                        nte.NoteText = segments[2];
                        claim.Notes.Add(nte);
                    }
                    break;
                case "CR1":
                    if (LoopName == "2300")
                    {
                        claim.Header.AmbulanceWeight = segments[2];
                        claim.Header.AmbulanceReasonCode = segments[4];
                        claim.Header.AmbulanceQuantity = segments[6];
                        if (segments.Length > 9) claim.Header.AmbulanceRoundTripDescription = segments[9];
                        if (segments.Length > 10) claim.Header.AmbulanceStretcherDescription = segments[10];
                    }
                    else if (LoopName == "2400")
                    {
                        claim.Lines.Last().PatientWeight = segments[2];
                        claim.Lines.Last().AmbulanceTransportReasonCode = segments[4];
                        claim.Lines.Last().TransportDistance = segments[6];
                        if (segments.Length > 9) claim.Lines.Last().RoundTripPurposeDescription = segments[9];
                        if (segments.Length > 10) claim.Lines.Last().StretcherPueposeDescription = segments[10];
                    }
                    break;
                case "CR2":
                    if (LoopName == "2300")
                    {
                        claim.Header.PatientConditionCode = segments[8];
                        if (segments.Length > 11) claim.Header.PatientConditionDescription1 = segments[11];
                        if (segments.Length > 12) claim.Header.PatientConditionDescription2 = segments[12];
                    }
                    break;
                case "CR3":
                    if (LoopName == "2400")
                    {
                        //DME certification
                        claim.Lines.Last().CertificationTypeCode = segments[1];
                        claim.Lines.Last().DMEDuration = segments[3];
                    }
                    break;
                case "CRC":
                    //CRC-07, ambulance certification, repeat 3 times
                    //CRC-E1, patient condition information vision, repeat 3 times
                    //CRC-75, homebound indicator, repeat 1
                    //CRC-ZZ, EPSDT referral, repeat 1
                    if (LoopName == "2300")
                    {
                        crc = new ClaimCRC();
                        crc.ClaimID = claim.Header.ClaimID;
                        crc.FileID = submittedFile.FileID;
                        crc.ServiceLineNumber = "0";
                        crc.CodeCategory = segments[1];
                        crc.ConditionIndicator = segments[2];
                        crc.ConditionCode = segments[3];
                        if (segments.Length > 4) crc.ConditionCode2 = segments[4];
                        if (segments.Length > 5) crc.ConditionCode3 = segments[5];
                        if (segments.Length > 6) crc.ConditionCode4 = segments[6];
                        if (segments.Length > 7) crc.ConditionCode5 = segments[7];
                        claim.CRCs.Add(crc);
                    }
                    else if (LoopName == "2400")
                    {
                        //07-ambulance certification
                        //70-hospice employee indicator
                        //09-DME indicator
                        crc = new ClaimCRC();
                        crc.ClaimID = claim.Header.ClaimID;
                        crc.FileID = submittedFile.FileID;
                        crc.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                        crc.CodeCategory = segments[1];
                        crc.ConditionIndicator = segments[2];
                        crc.ConditionCode = segments[3];
                        if (segments.Length > 4) crc.ConditionCode2 = segments[4];
                        if (segments.Length > 5) crc.ConditionCode3 = segments[5];
                        if (segments.Length > 6) crc.ConditionCode4 = segments[6];
                        if (segments.Length > 7) crc.ConditionCode5 = segments[7];
                        claim.CRCs.Add(crc);
                    }
                    break;
                case "HI":
                    if (LoopName == "2300" && (segments[1].StartsWith("ABK") || segments[1].StartsWith("BK")))
                    {
                        for (int i = 1; i < segments.Length; i++)
                        {
                            temparray = segments[i].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 8) hi.PresentOnAdmissionIndicator = temparray[8];
                            claim.His.Add(hi);
                        }
                    }
                    else if (LoopName == "2300" && segments[1].StartsWith("BP"))
                    {
                        //anesthesia related procedure
                        hi = new ClaimHI();
                        hi.ClaimID = claim.Header.ClaimID;
                        hi.FileID = submittedFile.FileID;
                        hi.HIQual = segments[1].Split(':')[0];
                        hi.HICode = segments[1].Split(':')[1];
                        claim.His.Add(hi);
                        if (segments.Length > 2)
                        {
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = segments[2].Split(':')[0];
                            hi.HICode = segments[2].Split(':')[1];
                            claim.His.Add(hi);
                        }
                    }
                    else if (LoopName == "2300" && segments[1].StartsWith("BG"))
                    {
                        //condition information
                        hi = new ClaimHI();
                        hi.ClaimID = claim.Header.ClaimID;
                        hi.FileID = submittedFile.FileID;
                        hi.HIQual = segments[1].Split(':')[0];
                        hi.HICode = segments[1].Split(':')[1];
                        claim.His.Add(hi);
                        if (segments.Length > 2)
                        {
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = segments[2].Split(':')[0];
                            hi.HICode = segments[2].Split(':')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 3)
                        {
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = segments[3].Split(':')[0];
                            hi.HICode = segments[3].Split(':')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 4)
                        {
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = segments[4].Split(':')[0];
                            hi.HICode = segments[4].Split(':')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 5)
                        {
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = segments[5].Split(':')[0];
                            hi.HICode = segments[5].Split(':')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 6)
                        {
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = segments[6].Split(':')[0];
                            hi.HICode = segments[6].Split(':')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 7)
                        {
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = segments[7].Split(':')[0];
                            hi.HICode = segments[7].Split(':')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 8)
                        {
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = segments[8].Split(':')[0];
                            hi.HICode = segments[8].Split(':')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 9)
                        {
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = segments[9].Split(':')[0];
                            hi.HICode = segments[9].Split(':')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 10)
                        {
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = segments[10].Split(':')[0];
                            hi.HICode = segments[10].Split(':')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 11)
                        {
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = segments[11].Split(':')[0];
                            hi.HICode = segments[11].Split(':')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 12)
                        {
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = segments[12].Split(':')[0];
                            hi.HICode = segments[12].Split(':')[1];
                            claim.His.Add(hi);
                        }
                    }
                    else if (LoopName == "2300" && (segments[1].StartsWith("ABJ") || segments[1].StartsWith("BJ")))
                    {
                        //institutional admitting diagnosis
                        hi = new ClaimHI();
                        hi.ClaimID = claim.Header.ClaimID;
                        hi.FileID = submittedFile.FileID;
                        hi.HIQual = segments[1].Split(':')[0];
                        hi.HICode = segments[1].Split(':')[1];
                        claim.His.Add(hi);
                    }
                    else if (LoopName == "2300" && (segments[1].StartsWith("APR") || segments[1].StartsWith("PR")))
                    {
                        //institutional patient reason for visit
                        hi = new ClaimHI();
                        hi.ClaimID = claim.Header.ClaimID;
                        hi.FileID = submittedFile.FileID;
                        hi.HIQual = segments[1].Split(':')[0];
                        hi.HICode = segments[1].Split(':')[1];
                        claim.His.Add(hi);
                        if (segments.Length > 2)
                        {
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = segments[2].Split(':')[0];
                            hi.HICode = segments[2].Split(':')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 3)
                        {
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = segments[3].Split(':')[0];
                            hi.HICode = segments[3].Split(':')[1];
                            claim.His.Add(hi);
                        }
                    }
                    else if (LoopName == "2300" && (segments[1].StartsWith("ABN") || segments[1].StartsWith("BN")))
                    {
                        //intitutional external cause of injury
                        temparray = segments[1].Split(':');
                        hi = new ClaimHI();
                        hi.ClaimID = claim.Header.ClaimID;
                        hi.FileID = submittedFile.FileID;
                        hi.HIQual = temparray[0];
                        hi.HICode = temparray[1];
                        if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];

                        claim.His.Add(hi);
                        if (segments.Length > 2)
                        {
                            temparray = segments[2].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 3)
                        {
                            temparray = segments[3].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 4)
                        {
                            temparray = segments[4].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 5)
                        {
                            temparray = segments[5].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 6)
                        {
                            temparray = segments[6].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 7)
                        {
                            temparray = segments[7].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 8)
                        {
                            temparray = segments[8].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 9)
                        {
                            temparray = segments[9].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 10)
                        {
                            temparray = segments[10].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 11)
                        {
                            temparray = segments[11].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 12)
                        {
                            temparray = segments[12].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                    }
                    else if (LoopName == "2300" && segments[1].StartsWith("DR"))
                    {
                        //institutional diagnosis related group information
                        hi = new ClaimHI();
                        hi.ClaimID = claim.Header.ClaimID;
                        hi.FileID = submittedFile.FileID;
                        hi.HIQual = segments[1].Split(':')[0];
                        hi.HICode = segments[1].Split(':')[1];
                        claim.His.Add(hi);
                    }
                    else if (LoopName == "2300" && (segments[1].StartsWith("ABF") || segments[1].StartsWith("BF")))
                    {
                        //institutional additional diagnosis codes
                        temparray = segments[1].Split(':');
                        hi = new ClaimHI();
                        hi.ClaimID = claim.Header.ClaimID;
                        hi.FileID = submittedFile.FileID;
                        hi.HIQual = temparray[0];
                        hi.HICode = temparray[1];
                        if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                        claim.His.Add(hi);
                        if (segments.Length > 2)
                        {
                            temparray = segments[2].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 3)
                        {
                            temparray = segments[3].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 4)
                        {
                            temparray = segments[4].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 5)
                        {
                            temparray = segments[5].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 6)
                        {
                            temparray = segments[6].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 7)
                        {
                            temparray = segments[7].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 8)
                        {
                            temparray = segments[8].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 9)
                        {
                            temparray = segments[9].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 10)
                        {
                            temparray = segments[10].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 11)
                        {
                            temparray = segments[11].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 12)
                        {
                            temparray = segments[12].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 9) hi.PresentOnAdmissionIndicator = temparray[9];
                            claim.His.Add(hi);
                        }
                    }
                    else if (LoopName == "2300" && (segments[1].StartsWith("BBR") || segments[1].StartsWith("BR") || segments[1].StartsWith("CAH")))
                    {
                        //institutional principle procedure information
                        temparray = segments[1].Split(':');
                        hi = new ClaimHI();
                        hi.ClaimID = claim.Header.ClaimID;
                        hi.FileID = submittedFile.FileID;
                        hi.HIQual = temparray[0];
                        hi.HICode = temparray[1];
                        hi.HIFromDate = temparray[3];
                        claim.His.Add(hi);
                    }
                    else if (LoopName == "2300" && (segments[1].StartsWith("BBQ") || segments[1].StartsWith("BQ")))
                    {
                        //institutional other principle procedure information
                        temparray = segments[1].Split(':');
                        hi = new ClaimHI();
                        hi.ClaimID = claim.Header.ClaimID;
                        hi.FileID = submittedFile.FileID;
                        hi.HIQual = temparray[0];
                        hi.HICode = temparray[1];
                        hi.HIFromDate = temparray[3];
                        claim.His.Add(hi);
                        if (segments.Length > 2)
                        {
                            temparray = segments[2].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                hi.HIFromDate = temparray[3];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 3)
                        {
                            temparray = segments[3].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                hi.HIFromDate = temparray[3];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 4)
                        {
                            temparray = segments[4].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                hi.HIFromDate = temparray[3];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 5)
                        {
                            temparray = segments[5].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                hi.HIFromDate = temparray[3];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 6)
                        {
                            temparray = segments[6].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                hi.HIFromDate = temparray[3];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 7)
                        {
                            temparray = segments[7].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                hi.HIFromDate = temparray[3];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 8)
                        {
                            temparray = segments[8].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                hi.HIFromDate = temparray[3];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 9)
                        {
                            temparray = segments[9].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                hi.HIFromDate = temparray[3];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 10)
                        {
                            temparray = segments[10].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                hi.HIFromDate = temparray[3];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 11)
                        {
                            temparray = segments[11].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                hi.HIFromDate = temparray[3];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 12)
                        {
                            temparray = segments[12].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                hi.HIFromDate = temparray[3];
                                claim.His.Add(hi);
                            }
                        }
                    }
                    else if (LoopName == "2300" && segments[1].StartsWith("BI"))
                    {
                        //institutional occurrence span codes
                        temparray = segments[1].Split(':');
                        hi = new ClaimHI();
                        hi.ClaimID = claim.Header.ClaimID;
                        hi.FileID = submittedFile.FileID;
                        hi.HIQual = temparray[0];
                        hi.HICode = temparray[1];
                        hi.HIFromDate = temparray[3].Split('-')[0];
                        hi.HIToDate = temparray[3].Split('-')[1];
                        claim.His.Add(hi);
                        if (segments.Length > 2)
                        {
                            temparray = segments[2].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3].Split('-')[0];
                            hi.HIToDate = temparray[3].Split('-')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 3)
                        {
                            temparray = segments[3].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3].Split('-')[0];
                            hi.HIToDate = temparray[3].Split('-')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 4)
                        {
                            temparray = segments[4].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3].Split('-')[0];
                            hi.HIToDate = temparray[3].Split('-')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 5)
                        {
                            temparray = segments[5].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3].Split('-')[0];
                            hi.HIToDate = temparray[3].Split('-')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 6)
                        {
                            temparray = segments[6].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3].Split('-')[0];
                            hi.HIToDate = temparray[3].Split('-')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 7)
                        {
                            temparray = segments[7].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3].Split('-')[0];
                            hi.HIToDate = temparray[3].Split('-')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 8)
                        {
                            temparray = segments[8].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3].Split('-')[0];
                            hi.HIToDate = temparray[3].Split('-')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 9)
                        {
                            temparray = segments[9].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3].Split('-')[0];
                            hi.HIToDate = temparray[3].Split('-')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 10)
                        {
                            temparray = segments[10].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3].Split('-')[0];
                            hi.HIToDate = temparray[3].Split('-')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 11)
                        {
                            temparray = segments[11].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3].Split('-')[0];
                            hi.HIToDate = temparray[3].Split('-')[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 12)
                        {
                            temparray = segments[12].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3].Split('-')[0];
                            hi.HIToDate = temparray[3].Split('-')[1];
                            claim.His.Add(hi);
                        }
                    }
                    else if (LoopName == "2300" && segments[1].StartsWith("BH"))
                    {
                        //institutional occurrence codes
                        temparray = segments[1].Split(':');
                        hi = new ClaimHI();
                        hi.ClaimID = claim.Header.ClaimID;
                        hi.FileID = submittedFile.FileID;
                        hi.HIQual = temparray[0];
                        hi.HICode = temparray[1];
                        hi.HIFromDate = temparray[3];
                        claim.His.Add(hi);
                        if (segments.Length > 2)
                        {
                            temparray = segments[2].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 3)
                        {
                            temparray = segments[3].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 4)
                        {
                            temparray = segments[4].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 5)
                        {
                            temparray = segments[5].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 6)
                        {
                            temparray = segments[6].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 7)
                        {
                            temparray = segments[7].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 8)
                        {
                            temparray = segments[8].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 9)
                        {
                            temparray = segments[9].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 10)
                        {
                            temparray = segments[10].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 11)
                        {
                            temparray = segments[11].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 12)
                        {
                            temparray = segments[12].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            hi.HIFromDate = temparray[3];
                            claim.His.Add(hi);
                        }
                    }
                    else if (LoopName == "2300" && segments[1].StartsWith("BE"))
                    {
                        //institutional value codes
                        temparray = segments[1].Split(':');
                        if (temparray.Length > 1)
                        {
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            if (temparray.Length > 4) hi.HIAmount = temparray[4];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 2)
                        {
                            temparray = segments[2].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                if (temparray.Length > 4) hi.HIAmount = temparray[4];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 3)
                        {
                            temparray = segments[3].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                if (temparray.Length > 4) hi.HIAmount = temparray[4];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 4)
                        {
                            temparray = segments[4].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                if (temparray.Length > 4) hi.HIAmount = temparray[4];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 5)
                        {
                            temparray = segments[5].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                if (temparray.Length > 4) hi.HIAmount = temparray[4];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 6)
                        {
                            temparray = segments[6].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                if (temparray.Length > 4) hi.HIAmount = temparray[4];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 7)
                        {
                            temparray = segments[7].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                if (temparray.Length > 4) hi.HIAmount = temparray[4];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 8)
                        {
                            temparray = segments[8].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                if (temparray.Length > 4) hi.HIAmount = temparray[4];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 9)
                        {
                            temparray = segments[9].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                if (temparray.Length > 4) hi.HIAmount = temparray[4];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 10)
                        {
                            temparray = segments[10].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                if (temparray.Length > 4) hi.HIAmount = temparray[4];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 11)
                        {
                            temparray = segments[11].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                if (temparray.Length > 4) hi.HIAmount = temparray[4];
                                claim.His.Add(hi);
                            }
                        }
                        if (segments.Length > 12)
                        {
                            temparray = segments[12].Split(':');
                            if (temparray.Length > 1)
                            {
                                hi = new ClaimHI();
                                hi.ClaimID = claim.Header.ClaimID;
                                hi.FileID = submittedFile.FileID;
                                hi.HIQual = temparray[0];
                                hi.HICode = temparray[1];
                                if (temparray.Length > 4) hi.HIAmount = temparray[4];
                                claim.His.Add(hi);
                            }
                        }
                    }
                    else if (LoopName == "2300" && segments[1].StartsWith("BG"))
                    {
                        //institutional condition codes
                        temparray = segments[1].Split(':');
                        hi = new ClaimHI();
                        hi.ClaimID = claim.Header.ClaimID;
                        hi.FileID = submittedFile.FileID;
                        hi.HIQual = temparray[0];
                        hi.HICode = temparray[1];
                        claim.His.Add(hi);
                        if (segments.Length > 2)
                        {
                            temparray = segments[2].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 3)
                        {
                            temparray = segments[3].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 4)
                        {
                            temparray = segments[4].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 5)
                        {
                            temparray = segments[5].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 6)
                        {
                            temparray = segments[6].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 7)
                        {
                            temparray = segments[7].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 8)
                        {
                            temparray = segments[8].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 9)
                        {
                            temparray = segments[9].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 10)
                        {
                            temparray = segments[10].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 11)
                        {
                            temparray = segments[11].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 12)
                        {
                            temparray = segments[12].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                    }
                    else if (LoopName == "2300" && segments[1].StartsWith("TC"))
                    {
                        //institutional treatment codes
                        temparray = segments[1].Split(':');
                        hi = new ClaimHI();
                        hi.ClaimID = claim.Header.ClaimID;
                        hi.FileID = submittedFile.FileID;
                        hi.HIQual = temparray[0];
                        hi.HICode = temparray[1];
                        claim.His.Add(hi);
                        if (segments.Length > 2)
                        {
                            temparray = segments[2].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 3)
                        {
                            temparray = segments[3].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 4)
                        {
                            temparray = segments[4].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 5)
                        {
                            temparray = segments[5].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 6)
                        {
                            temparray = segments[6].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 7)
                        {
                            temparray = segments[7].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 8)
                        {
                            temparray = segments[8].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 9)
                        {
                            temparray = segments[9].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 10)
                        {
                            temparray = segments[10].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 11)
                        {
                            temparray = segments[11].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                        if (segments.Length > 12)
                        {
                            temparray = segments[12].Split(':');
                            hi = new ClaimHI();
                            hi.ClaimID = claim.Header.ClaimID;
                            hi.FileID = submittedFile.FileID;
                            hi.HIQual = temparray[0];
                            hi.HICode = temparray[1];
                            claim.His.Add(hi);
                        }
                    }
                    break;
                case "HCP":
                    //claim pricing / repricing information
                    if (LoopName == "2300")
                    {
                        claim.Header.PricingMethodology = segments[1];
                        claim.Header.RepricedAllowedAmount = segments[2];
                        if (segments.Length > 3) claim.Header.RepricedSavingAmount = segments[3];
                        if (segments.Length > 4) claim.Header.RepricingOrganizationID = segments[4];
                        if (segments.Length > 5) claim.Header.RepricingRate = segments[5];
                        if (segments.Length > 6) claim.Header.RepricedGroupCode = segments[6];
                        if (segments.Length > 7) claim.Header.RepricedGroupAmount = segments[7];
                        if (segments.Length > 8) claim.Header.RepricingRevenueCode = segments[8];
                        if (segments.Length > 11) claim.Header.RepricingUnit = segments[11];
                        if (segments.Length > 12) claim.Header.RepricingQuantity = segments[12];
                        if (segments.Length > 13) claim.Header.RejectReasonCode = segments[13];
                        if (segments.Length > 14) claim.Header.PolicyComplianceCode = segments[14];
                        if (segments.Length > 15) claim.Header.ExceptionCode = segments[15];
                    }
                    else if (LoopName == "2400")
                    {
                        claim.Lines.Last().PricingMethodology = segments[1];
                        claim.Lines.Last().RepricedAllowedAmount = segments[2];
                        if (segments.Length > 3) claim.Lines.Last().RepricedSavingAmount = segments[3];
                        if (segments.Length > 4) claim.Lines.Last().RepricingOrganizationIdentifier = segments[4];
                        if (segments.Length > 5) claim.Lines.Last().RepricingRate = segments[5];
                        if (segments.Length > 6) claim.Lines.Last().RepricedAmbulatoryPatientGroupCode = segments[6];
                        if (segments.Length > 7) claim.Lines.Last().RepricedAmbulatoryPatientGroupAmount = segments[7];
                        if (segments.Length > 9) claim.Lines.Last().HCPQualifier = segments[9];
                        if (segments.Length > 10) claim.Lines.Last().RepricedHCPCSCode = segments[10];
                        if (segments.Length > 11) claim.Lines.Last().RepricingUnit = segments[11];
                        if (segments.Length > 12) claim.Lines.Last().RepricingQuantity = segments[12];
                        if (segments.Length > 13) claim.Lines.Last().RejectReasonCode = segments[13];
                        if (segments.Length > 14) claim.Lines.Last().PolicyComplianceCode = segments[14];
                        if (segments.Length > 15) claim.Lines.Last().ExceptionCode = segments[15];
                    }
                    break;
                case "CAS":
                    //segment repeat 5 times, CO(contractual obiligation), CR(Correction and reversals), OA(other adjustments), PI(payer initiated reductions), PR(patient responsibility)
                    //reason code for each group maximum:6
                    if (string.Compare(LoopName, "2321") == -1)
                    {
                        LoopName = "2320";
                        cas = new ClaimCAS();
                        cas.ClaimID = claim.Header.ClaimID;
                        cas.FileID = submittedFile.FileID;
                        cas.ServiceLineNumber = "0";
                        cas.GroupCode = segments[1];
                        cas.ReasonCode = segments[2];
                        cas.AdjustmentAmount = segments[3];
                        if (segments.Length > 4) cas.AdjustmentQuantity = segments[4];
                        cas.SubscriberSequenceNumber = claim.Subscribers.Last().SubscriberSequenceNumber;
                        claim.Cases.Add(cas);
                        if (segments.Length > 5)
                        {
                            cas = new ClaimCAS();
                            cas.ClaimID = claim.Header.ClaimID;
                            cas.FileID = submittedFile.FileID;
                            cas.ServiceLineNumber = "0";
                            cas.GroupCode = segments[1];
                            cas.ReasonCode = segments[5];
                            if (segments.Length > 6) cas.AdjustmentAmount = segments[6];
                            if (segments.Length > 7) cas.AdjustmentQuantity = segments[7];
                            cas.SubscriberSequenceNumber = claim.Subscribers.Last().SubscriberSequenceNumber;
                            claim.Cases.Add(cas);
                        }
                        if (segments.Length > 8)
                        {
                            cas = new ClaimCAS();
                            cas.ClaimID = claim.Header.ClaimID;
                            cas.FileID = submittedFile.FileID;
                            cas.ServiceLineNumber = "0";
                            cas.GroupCode = segments[1];
                            cas.ReasonCode = segments[8];
                            if (segments.Length > 9) cas.AdjustmentAmount = segments[9];
                            if (segments.Length > 10) cas.AdjustmentQuantity = segments[10];
                            cas.SubscriberSequenceNumber = claim.Subscribers.Last().SubscriberSequenceNumber;
                            claim.Cases.Add(cas);
                        }
                        if (segments.Length > 11)
                        {
                            cas = new ClaimCAS();
                            cas.ClaimID = claim.Header.ClaimID;
                            cas.FileID = submittedFile.FileID;
                            cas.ServiceLineNumber = "0";
                            cas.GroupCode = segments[1];
                            cas.ReasonCode = segments[11];
                            if (segments.Length > 12) cas.AdjustmentAmount = segments[12];
                            if (segments.Length > 13) cas.AdjustmentQuantity = segments[13];
                            cas.SubscriberSequenceNumber = claim.Subscribers.Last().SubscriberSequenceNumber;
                            claim.Cases.Add(cas);
                        }
                        if (segments.Length > 14)
                        {
                            cas = new ClaimCAS();
                            cas.ClaimID = claim.Header.ClaimID;
                            cas.FileID = submittedFile.FileID;
                            cas.ServiceLineNumber = "0";
                            cas.GroupCode = segments[1];
                            cas.ReasonCode = segments[14];
                            if (segments.Length > 15) cas.AdjustmentAmount = segments[15];
                            if (segments.Length > 16) cas.AdjustmentQuantity = segments[16];
                            cas.SubscriberSequenceNumber = claim.Subscribers.Last().SubscriberSequenceNumber;
                            claim.Cases.Add(cas);
                        }
                        if (segments.Length > 17)
                        {
                            cas = new ClaimCAS();
                            cas.ClaimID = claim.Header.ClaimID;
                            cas.FileID = submittedFile.FileID;
                            cas.ServiceLineNumber = "0";
                            cas.GroupCode = segments[1];
                            cas.ReasonCode = segments[17];
                            if (segments.Length > 18) cas.AdjustmentAmount = segments[18];
                            if (segments.Length > 19) cas.AdjustmentQuantity = segments[19];
                            cas.SubscriberSequenceNumber = claim.Subscribers.Last().SubscriberSequenceNumber;
                            claim.Cases.Add(cas);
                        }
                    }
                    else if (string.Compare(LoopName, "2431") == -1)
                    {
                        LoopName = "2430";
                        cas = new ClaimCAS();
                        cas.ClaimID = claim.Header.ClaimID;
                        cas.FileID = submittedFile.FileID;
                        cas.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                        cas.GroupCode = segments[1];
                        cas.ReasonCode = segments[2];
                        cas.AdjustmentAmount = segments[3];
                        if (segments.Length > 4) cas.AdjustmentQuantity = segments[4];
                        cas.SubscriberSequenceNumber = claim.SVDs.Last().RepeatSequence;
                        claim.Cases.Add(cas);
                        if (segments.Length > 5)
                        {
                            cas = new ClaimCAS();
                            cas.ClaimID = claim.Header.ClaimID;
                            cas.FileID = submittedFile.FileID;
                            cas.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                            cas.GroupCode = segments[1];
                            cas.ReasonCode = segments[5];
                            if (segments.Length > 6) cas.AdjustmentAmount = segments[6];
                            if (segments.Length > 7) cas.AdjustmentQuantity = segments[7];
                            cas.SubscriberSequenceNumber = claim.SVDs.Last().RepeatSequence;
                            claim.Cases.Add(cas);
                        }
                        if (segments.Length > 8)
                        {
                            cas = new ClaimCAS();
                            cas.ClaimID = claim.Header.ClaimID;
                            cas.FileID = submittedFile.FileID;
                            cas.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                            cas.GroupCode = segments[1];
                            cas.ReasonCode = segments[8];
                            if (segments.Length > 9) cas.AdjustmentAmount = segments[9];
                            if (segments.Length > 10) cas.AdjustmentQuantity = segments[10];
                            cas.SubscriberSequenceNumber = claim.SVDs.Last().RepeatSequence;
                            claim.Cases.Add(cas);
                        }
                        if (segments.Length > 11)
                        {
                            cas = new ClaimCAS();
                            cas.ClaimID = claim.Header.ClaimID;
                            cas.FileID = submittedFile.FileID;
                            cas.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                            cas.GroupCode = segments[1];
                            cas.ReasonCode = segments[11];
                            if (segments.Length > 12) cas.AdjustmentAmount = segments[12];
                            if (segments.Length > 13) cas.AdjustmentQuantity = segments[13];
                            cas.SubscriberSequenceNumber = claim.SVDs.Last().RepeatSequence;
                            claim.Cases.Add(cas);
                        }
                        if (segments.Length > 14)
                        {
                            cas = new ClaimCAS();
                            cas.ClaimID = claim.Header.ClaimID;
                            cas.FileID = submittedFile.FileID;
                            cas.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                            cas.GroupCode = segments[1];
                            cas.ReasonCode = segments[14];
                            if (segments.Length > 15) cas.AdjustmentAmount = segments[15];
                            if (segments.Length > 16) cas.AdjustmentQuantity = segments[16];
                            cas.SubscriberSequenceNumber = claim.SVDs.Last().RepeatSequence;
                            claim.Cases.Add(cas);
                        }
                        if (segments.Length > 17)
                        {
                            cas = new ClaimCAS();
                            cas.ClaimID = claim.Header.ClaimID;
                            cas.FileID = submittedFile.FileID;
                            cas.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                            cas.GroupCode = segments[1];
                            cas.ReasonCode = segments[17];
                            if (segments.Length > 18) cas.AdjustmentAmount = segments[18];
                            if (segments.Length > 19) cas.AdjustmentQuantity = segments[19];
                            cas.SubscriberSequenceNumber = claim.SVDs.Last().RepeatSequence;
                            claim.Cases.Add(cas);
                        }
                    }
                    break;
                case "OI":
                    //other insurance coverage information
                    if (LoopName == "2320")
                    {
                        claim.Subscribers.Last().BenefitsAssignmentCertificationIndicator = segments[3];
                        claim.Subscribers.Last().PatientSignatureSourceCode = segments[4];
                        claim.Subscribers.Last().ReleaseOfInformationCode = segments[6];
                    }
                    break;
                case "MOA":
                    //outpatient adjudication information
                    if (LoopName == "2320")
                    {
                        if (segments.Length > 1) claim.Subscribers.Last().ReimbursementRate = segments[1];
                        if (segments.Length > 2) claim.Subscribers.Last().HCPCSPayableAmount = segments[2];
                        if (segments.Length > 3) claim.Subscribers.Last().MOA_ClaimPaymentRemarkCode1 = segments[3];
                        if (segments.Length > 4) claim.Subscribers.Last().MOA_ClaimPaymentRemarkCode2 = segments[4];
                        if (segments.Length > 5) claim.Subscribers.Last().MOA_ClaimPaymentRemarkCode3 = segments[5];
                        if (segments.Length > 6) claim.Subscribers.Last().MOA_ClaimPaymentRemarkCode4 = segments[6];
                        if (segments.Length > 7) claim.Subscribers.Last().MOA_ClaimPaymentRemarkCode5 = segments[7];
                        if (segments.Length > 8) claim.Subscribers.Last().EndStageRenalDiseasePaymentAmount = segments[8];
                        if (segments.Length > 9) claim.Subscribers.Last().MOA_NonPayableProfessionalComponentBilledAmount = segments[9];
                    }
                    break;
                case "LX":
                    LoopName = "2400";
                    line = new ServiceLine();
                    line.ClaimID = claim.Header.ClaimID;
                    line.FileID = submittedFile.FileID;
                    line.ServiceLineNumber = segments[1];
                    claim.Lines.Add(line);
                    break;
                case "SV1":
                    temparray = segments[1].Split(':');
                    claim.Lines.Last().ServiceIDQualifier = temparray[0];
                    claim.Lines.Last().ProcedureCode = temparray[1];
                    if (temparray.Length > 2) claim.Lines.Last().ProcedureModifier1 = temparray[2];
                    if (temparray.Length > 3) claim.Lines.Last().ProcedureModifier2 = temparray[3];
                    if (temparray.Length > 4) claim.Lines.Last().ProcedureModifier3 = temparray[4];
                    if (temparray.Length > 5) claim.Lines.Last().ProcedureModifier4 = temparray[5];
                    if (temparray.Length > 6) claim.Lines.Last().ProcedureDescription = temparray[6];
                    claim.Lines.Last().LineItemChargeAmount = segments[2];
                    claim.Lines.Last().LineItemUnit = segments[3];
                    claim.Lines.Last().ServiceUnitQuantity = segments[4];
                    claim.Lines.Last().LineItemPOS = segments[5];
                    temparray = segments[7].Split(':');
                    claim.Lines.Last().DiagPointer1 = temparray[0];
                    if (temparray.Length > 1) claim.Lines.Last().DiagPointer2 = temparray[1];
                    if (temparray.Length > 2) claim.Lines.Last().DiagPointer3 = temparray[2];
                    if (temparray.Length > 3) claim.Lines.Last().DiagPointer4 = temparray[3];
                    if (segments.Length > 9) claim.Lines.Last().EmergencyIndicator = segments[9];
                    if (segments.Length > 11) claim.Lines.Last().EPSDTIndicator = segments[11];
                    if (segments.Length > 12) claim.Lines.Last().FamilyPlanningIndicator = segments[12];
                    if (segments.Length > 15) claim.Lines.Last().CopayStatusCode = segments[15];
                    break;
                case "SV5":
                    claim.Lines.Last().DMEQualifier = segments[1].Split(':')[0];
                    claim.Lines.Last().DMEProcedureCode = segments[1].Split(':')[1];
                    claim.Lines.Last().DMEDays = segments[3];
                    claim.Lines.Last().DMERentalPrice = segments[4];
                    claim.Lines.Last().DMEPurchasePrice = segments[5];
                    claim.Lines.Last().DMEFrequencyCode = segments[6];
                    break;
                case "QTY":
                    if (LoopName == "2400")
                    {
                        switch (segments[1])
                        {
                            case "PT":
                                claim.Lines.Last().AmbulancePatientCount = segments[2];
                                break;
                            case "FL":
                                claim.Lines.Last().ObstetricAdditionalUnits = segments[2];
                                break;
                        }
                    }
                    break;
                case "MEA":
                    //service line test result, segment repeat 5 times
                    mea = new ClaimLineMEA();
                    mea.ClaimID = claim.Header.ClaimID;
                    mea.FileID = submittedFile.FileID;
                    mea.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                    mea.TestCode = segments[1];
                    mea.MeasurementQualifier = segments[2];
                    mea.TestResult = segments[3];
                    claim.Meas.Add(mea);
                    break;
                case "PS1":
                    //service line purchased service information
                    claim.Lines.Last().PurchasedServiceProviderIdentifier = segments[1];
                    claim.Lines.Last().PurchasedServiceChargeAmount = segments[2];
                    break;
                case "LIN":
                    if (string.Compare(LoopName, "2411") == -1)
                    {
                        claim.Lines.Last().LINQualifier = segments[2];
                        claim.Lines.Last().NationalDrugCode = segments[3];
                        LoopName = "2410";
                    }
                    break;
                case "CTP":
                    if (string.Compare(LoopName, "2411") == -1)
                    {
                        //line level drug quantity
                        claim.Lines.Last().DrugQuantity = segments[4];
                        claim.Lines.Last().DrugQualifier = segments[5];
                        LoopName = "2410";

                    }
                    break;
                case "SVD":
                    if (string.Compare(LoopName, "2431") == -1)
                    {
                        LoopName = "2430";
                        svd = new ClaimLineSVD();
                        svd.ClaimID = claim.Header.ClaimID;
                        svd.FileID = submittedFile.FileID;
                        svd.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                        svd.OtherPayerPrimaryIdentifier = segments[1];
                        svd.ServiceLinePaidAmount = segments[2];
                        temparray = segments[3].Split(':');
                        if (temparray.Length > 0) svd.ServiceQualifier = temparray[0];
                        if (temparray.Length > 1) svd.ProcedureCode = temparray[1];
                        if (temparray.Length > 2) svd.ProcedureModifier1 = temparray[2];
                        if (temparray.Length > 3) svd.ProcedureModifier2 = temparray[3];
                        if (temparray.Length > 4) svd.ProcedureModifier3 = temparray[4];
                        if (temparray.Length > 5) svd.ProcedureModifier4 = temparray[5];
                        if (temparray.Length > 6) svd.ProcedureDescription = temparray[6];
                        svd.ServiceLineRevenueCode = segments[4];
                        svd.PaidServiceUnitCount = segments[5];
                        if (segments.Length > 6) svd.BundledLineNumber = segments[6];
                        svd.RepeatSequence = (claim.SVDs.Count + 1).ToString();
                        claim.SVDs.Add(svd);
                    }
                    break;
                case "LQ":
                    //form identification code
                    if (string.Compare(LoopName, "2441") == -1)
                    {
                        LoopName = "2440";
                        lq = new ClaimLineLQ();
                        lq.ClaimID = claim.Header.ClaimID;
                        lq.FileID = submittedFile.FileID;
                        lq.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                        lq.LQSequence = (claim.LQs.Count + 1).ToString();
                        lq.FormQualifier = segments[1];
                        lq.IndustryCode = segments[2];
                        claim.LQs.Add(lq);
                    }
                    break;
                case "FRM":
                    if (string.Compare(LoopName, "2411") == -1)
                    {
                        LoopName = "2440";
                        frm = new ClaimLineFRM();
                        frm.ClaimID = claim.Header.ClaimID;
                        frm.FileID = submittedFile.FileID;
                        frm.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                        frm.LQSequence = claim.LQs.Last().LQSequence;
                        frm.QuestionNumber = segments[1];
                        if (segments.Length > 2) frm.QuestionResponseIndicator = segments[2];
                        if (segments.Length > 3) frm.QuestionResponse = segments[3];
                        if (segments.Length > 4) frm.QuestionResponseDate = segments[4];
                        if (segments.Length > 5) frm.AllowedChargePercentage = segments[5];
                        claim.FRMs.Add(frm);
                    }
                    break;
                case "SE":
                    break;
                case "GE":
                    break;
                case "IEA":
                    break;
                case "CL1":
                    if (LoopName == "2300")
                    {
                        claim.Header.AdmissionTypeCode = segments[1];
                        claim.Header.AdmissionSourceCode = segments[2];
                        claim.Header.PatientStatusCode = segments[3];
                    }
                    break;
                case "F3":
                    if (LoopName == "2300")
                    {
                        claim.Header.PatientResponsibilityAmount = segments[2];
                    }
                    break;
                case "MIA":
                    LoopName = "2320";
                    claim.Subscribers.Last().CoveredDays = segments[1];
                    if (segments.Length > 3) claim.Subscribers.Last().LifetimePsychiatricDays = segments[3];
                    if (segments.Length > 4) claim.Subscribers.Last().ClaimDRGAmount = segments[4];
                    if (segments.Length > 5) claim.Subscribers.Last().MIA_ClaimPaymentRemarkCode1 = segments[5];
                    if (segments.Length > 6) claim.Subscribers.Last().ClaimDisproportionateShareAmount = segments[6];
                    if (segments.Length > 7) claim.Subscribers.Last().ClaimMSPPassThroughAmount = segments[7];
                    if (segments.Length > 8) claim.Subscribers.Last().ClaimPPSCapitalAmount = segments[8];
                    if (segments.Length > 9) claim.Subscribers.Last().PPSCapitalFSPDRGAmount = segments[9];
                    if (segments.Length > 10) claim.Subscribers.Last().PPSCapitalDSHDRGAmount = segments[10];
                    if (segments.Length > 11) claim.Subscribers.Last().PPSCapitalDSHDRGAmount = segments[11];
                    if (segments.Length > 12) claim.Subscribers.Last().OldCapitalAmount = segments[12];
                    if (segments.Length > 13) claim.Subscribers.Last().PPSCapitalIMEAmount = segments[13];
                    if (segments.Length > 14) claim.Subscribers.Last().PPSOperatingHospitalSpecificDRGAmount = segments[14];
                    if (segments.Length > 15) claim.Subscribers.Last().CostReportDayCount = segments[15];
                    if (segments.Length > 16) claim.Subscribers.Last().PPSOperatingFederalSpecificDRGAmount = segments[16];
                    if (segments.Length > 17) claim.Subscribers.Last().ClaimPPSCapitalOutlierAmount = segments[17];
                    if (segments.Length > 18) claim.Subscribers.Last().ClaimIndirectTeachingAmount = segments[18];
                    if (segments.Length > 19) claim.Subscribers.Last().MIA_NonPayableProfessionalComponentBilledAmount = segments[19];
                    if (segments.Length > 20) claim.Subscribers.Last().MIA_ClaimPaymentRemarkCode2 = segments[20];
                    if (segments.Length > 21) claim.Subscribers.Last().MIA_ClaimPaymentRemarkCode3 = segments[21];
                    if (segments.Length > 22) claim.Subscribers.Last().MIA_ClaimPaymentRemarkCode4 = segments[22];
                    if (segments.Length > 23) claim.Subscribers.Last().MIA_ClaimPaymentRemarkCode5 = segments[23];
                    if (segments.Length > 24) claim.Subscribers.Last().PPSCapitalExceptionAmount = segments[24];
                    break;
                case "SV2":
                    claim.Lines.Last().RevenueCode = segments[1];
                    temparray = segments[2].Split(':');
                    if (temparray.Length > 0) claim.Lines.Last().ServiceIDQualifier = temparray[0];
                    if (temparray.Length > 1) claim.Lines.Last().ProcedureCode = temparray[1];
                    if (temparray.Length > 2) claim.Lines.Last().ProcedureModifier1 = temparray[2];
                    if (temparray.Length > 3) claim.Lines.Last().ProcedureModifier2 = temparray[3];
                    if (temparray.Length > 4) claim.Lines.Last().ProcedureModifier3 = temparray[4];
                    if (temparray.Length > 5) claim.Lines.Last().ProcedureModifier4 = temparray[5];
                    if (temparray.Length > 6) claim.Lines.Last().ProcedureDescription = temparray[6];
                    claim.Lines.Last().LineItemChargeAmount = segments[3];
                    claim.Lines.Last().LineItemUnit = segments[4];
                    claim.Lines.Last().ServiceUnitQuantity = segments[5];
                    if (segments.Length > 7) claim.Lines.Last().LineItemDeniedChargeAmount = segments[7];
                    break;
                default:
                    break;
            }

        }
        public static void InitilizeClaim(string loopFlag, ref Claim claim, ref List<Claim> claims, ref SubmissionLog submittedFile)
        {
            foreach (ClaimProvider provider in claim.Providers)
            {
                provider.ClaimID = claim.Header.ClaimID;
            }
            foreach (ProviderContact providerContact in claim.ProviderContacts)
            {
                providerContact.ClaimID = claim.Header.ClaimID;
            }
            foreach (ClaimSecondaryIdentification secondaryIdentification in claim.SecondaryIdentifications)
            {
                secondaryIdentification.ClaimID = claim.Header.ClaimID;
            }
            foreach (ClaimSBR subscriber in claim.Subscribers)
            {
                subscriber.ClaimID = claim.Header.ClaimID;
            }
            foreach (ClaimPatient patient in claim.Patients)
            {
                patient.ClaimID = claim.Header.ClaimID;
            }
            claims.Add(claim);
            claim = new Claim();
            claim.Header.FileID = submittedFile.FileID;
            if (loopFlag == "Claim")
            {
                claim.Providers.AddRange(claims.Last().Providers);
                claim.ProviderContacts.AddRange(claims.Last().ProviderContacts);
                claim.SecondaryIdentifications.AddRange(claims.Last().SecondaryIdentifications);
                claim.Subscribers.AddRange(claims.Last().Subscribers);
                claim.Patients.AddRange(claims.Last().Patients);
            }
            else if (loopFlag == "Patient")
            {
                claim.Subscribers.AddRange(claims.Last().Subscribers);
                claim.Patients.AddRange(claims.Last().Patients);
            }
            else if (loopFlag == "Subscriber")
            {
                claim.Patients.AddRange(claims.Last().Patients);
            }
            if (claims.Count >= 1000)
            {
                SHRUtil.SaveClaims(ref claims);
                claims.Clear();
            }
        }
    }
}
