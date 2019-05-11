using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model;

namespace ExportModule
{
    public static class Utility
    {
        public static List<HipaaXML> GetClaimHipaaXML()
        {
            List<HipaaXML> result = null;

            using (var context = new SubHistContext())
            {
                context.Database.CommandTimeout = 1800;
                StringBuilder sb = new StringBuilder();
                sb.Append("select distinct ClaimID,ClaimHipaaXML from (");
                sb.Append("select c.clm01 as ClaimID,convert(varchar(max),a.transactionDocument) as ClaimHipaaXML from ");
                sb.Append("venus_bizprod_wpc_valid_837p.dbo.XMLDocument a ");
                sb.Append("inner join venus_bizprod_wpc_valid_837p.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select clm01,parentid,row_number() over(partition by clm01 order by id desc) rn from venus_bizprod_wpc_valid_837p.dbo.clm) c on c.rn=1 and c.parentid=b.id ");
                sb.Append("where CLM01 in");
                sb.Append("(");
                sb.Append("select distinct IehpClaimId from edrs.dbo.DHCSDenialMaster a ");
                sb.Append("inner join (select encounterreferencenumber,id,encounterstatus,transaction_id,row_number() over (partition by encounterreferencenumber order by id desc) as rn from IEHP_DHCSResponse.dbo.DHCSResponse_Encounter ");
                sb.Append(") b on b.rn=1 and a.IEHPClaimId=b.EncounterReferenceNumber ");
                sb.Append("inner join IEHP_DHCSResponse.dbo.DHCSResponse_Transaction c on b.Transaction_ID=c.ID ");
                sb.Append("inner join IEHP_DHCSResponse.dbo.DHCSResponse d on c.File_ID=d.ID ");
                sb.Append("inner join (select encounter_id,count(distinct issueid) as errorcounts from IEHP_DHCSResponse.dbo.DHCSResponse_EncounterResponse where Severity='Error' group by encounter_id) f on f.errorcounts=1 and f.Encounter_ID=b.ID ");
                sb.Append("cross apply (select top 1 encounter_id,severity,issueid,issnip,description from IEHP_DHCSResponse.dbo.DHCSResponse_EncounterResponse where Encounter_ID=b.id and Severity='Error' order by id desc) e ");
                sb.Append("where (d.ValidationStatus='Rejected' or (d.ValidationStatus='Accepted' and a.EncounterStatus<>'Accepted')) ");
                sb.Append("and (e.IssueId='0x39395df' or e.IssueId='0x001C3') ");
                sb.Append("and left(iehpclaimid,3)='306'");
                sb.Append(") ");
                sb.Append("union ");
                sb.Append("select c.clm01 as ClaimID,convert(varchar(max),a.transactionDocument) as ClaimHipaaXML from ");
                sb.Append("bizsqlb1_wpc_valid_837p.dbo.XMLDocument a ");
                sb.Append("inner join bizsqlb1_wpc_valid_837p.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select clm01,parentid,row_number() over(partition by clm01 order by id desc) rn from bizsqlb1_wpc_valid_837p.dbo.clm) c on c.rn=1 and c.parentid=b.id ");
                sb.Append("where CLM01 in");
                sb.Append("(");
                sb.Append("select distinct IehpClaimId from edrs.dbo.DHCSDenialMaster a ");
                sb.Append("inner join (select encounterreferencenumber,id,encounterstatus,transaction_id,row_number() over (partition by encounterreferencenumber order by id desc) as rn from IEHP_DHCSResponse.dbo.DHCSResponse_Encounter ");
                sb.Append(") b on b.rn=1 and a.IEHPClaimId=b.EncounterReferenceNumber ");
                sb.Append("inner join IEHP_DHCSResponse.dbo.DHCSResponse_Transaction c on b.Transaction_ID=c.ID ");
                sb.Append("inner join IEHP_DHCSResponse.dbo.DHCSResponse d on c.File_ID=d.ID ");
                sb.Append("inner join (select encounter_id,count(distinct issueid) as errorcounts from IEHP_DHCSResponse.dbo.DHCSResponse_EncounterResponse where Severity='Error' group by encounter_id) f on f.errorcounts=1 and f.Encounter_ID=b.ID ");
                sb.Append("cross apply (select top 1 encounter_id,severity,issueid,issnip,description from IEHP_DHCSResponse.dbo.DHCSResponse_EncounterResponse where Encounter_ID=b.id and Severity='Error' order by id desc) e ");
                sb.Append("where (d.ValidationStatus='Rejected' or (d.ValidationStatus='Accepted' and a.EncounterStatus<>'Accepted')) ");
                sb.Append("and (e.IssueId='0x39395df' or e.IssueId='0x001C3') ");
                sb.Append("and left(iehpclaimid,3)='306'");
                sb.Append(") ");
                sb.Append("union ");
                sb.Append("select c.clm01 as ClaimID,convert(varchar(max),a.transactionDocument) as ClaimHipaaXML from ");
                sb.Append("mars_wpc_valid_837p.dbo.XMLDocument a ");
                sb.Append("inner join mars_wpc_valid_837p.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select clm01,parentid,row_number() over(partition by clm01 order by id desc) rn from mars_wpc_valid_837p.dbo.clm) c on c.rn=1 and c.parentid=b.id ");
                sb.Append("where CLM01 in");
                sb.Append("(");
                sb.Append("select distinct IehpClaimId from edrs.dbo.DHCSDenialMaster a ");
                sb.Append("inner join (select encounterreferencenumber,id,encounterstatus,transaction_id,row_number() over (partition by encounterreferencenumber order by id desc) as rn from IEHP_DHCSResponse.dbo.DHCSResponse_Encounter ");
                sb.Append(") b on b.rn=1 and a.IEHPClaimId=b.EncounterReferenceNumber ");
                sb.Append("inner join IEHP_DHCSResponse.dbo.DHCSResponse_Transaction c on b.Transaction_ID=c.ID ");
                sb.Append("inner join IEHP_DHCSResponse.dbo.DHCSResponse d on c.File_ID=d.ID ");
                sb.Append("inner join (select encounter_id,count(distinct issueid) as errorcounts from IEHP_DHCSResponse.dbo.DHCSResponse_EncounterResponse where Severity='Error' group by encounter_id) f on f.errorcounts=1 and f.Encounter_ID=b.ID ");
                sb.Append("cross apply (select top 1 encounter_id,severity,issueid,issnip,description from IEHP_DHCSResponse.dbo.DHCSResponse_EncounterResponse where Encounter_ID=b.id and Severity='Error' order by id desc) e ");
                sb.Append("where (d.ValidationStatus='Rejected' or (d.ValidationStatus='Accepted' and a.EncounterStatus<>'Accepted')) ");
                sb.Append("and (e.IssueId='0x39395df' or e.IssueId='0x001C3') ");
                sb.Append("and left(iehpclaimid,3)='306'");
                sb.Append(") ");
                sb.Append(") t");
                result = context.Database.SqlQuery<HipaaXML>(sb.ToString()).ToList();
            }
            return result;
        }
        public static List<SubHist_Header> GetSubHistHeader(string Code)
        {
            List<SubHist_Header> result = null;
            using (var context = new SubHistContext())
            {
                context.Database.CommandTimeout = 1800;
                StringBuilder sb = new StringBuilder();
                sb.Append("select ");
                sb.Append("ProductionFlag,");
                sb.Append("GroupControlNumber,");
                sb.Append("ClaimNo,");
                sb.Append("PlaceOfService,");
                sb.Append("FacilityCode,");
                sb.Append("ClaimFrequency,");
                sb.Append("BillProvNPI,");
                sb.Append("BillProvLast,");
                sb.Append("BillProvFirst,");
                sb.Append("BillProvSpecialty,");
                sb.Append("BillProvZip,");
                sb.Append("SubscriberLast,");
                sb.Append("SubscriberFirst,");
                sb.Append("SubscriberID,");
                sb.Append("SubscriberZip,");
                sb.Append("SubscriberDOB,");
                sb.Append("SubscriberGender,");
                sb.Append("SubscriberSSN,");
                sb.Append("PatientLast,");
                sb.Append("PatientFirst,");
                sb.Append("PatientID,");
                sb.Append("PatientSSN,");
                sb.Append("PatientDOB,");
                sb.Append("PatientGender,");
                sb.Append("RendProvNPI,");
                sb.Append("RendProvLast,");
                sb.Append("RendProvFirst,");
                sb.Append("PickUpAddress,");
                sb.Append("PickUpAddress2,");
                sb.Append("PickUpCity,");
                sb.Append("PickUpState,");
                sb.Append("PickUpZip,");
                sb.Append("DropOffName,");
                sb.Append("DropOffAddress,");
                sb.Append("DropOffAddress2,");
                sb.Append("DropOffCity,");
                sb.Append("DropOffState,");
                sb.Append("DropOffZip,");
                sb.Append("BilledAmount,");
                sb.Append("EstimatedAmountDue,");
                sb.Append("ServiceDateFrom,");
                sb.Append("ServiceDateTo,");
                sb.Append("AdmissionDate,");
                sb.Append("AdmissionType,");
                sb.Append("AdmissionSource,");
                sb.Append("DischargeDate,");
                sb.Append("PatientStatus,");
                sb.Append("ClearinghouseID,");
                sb.Append("PayerClaimControlNumber,");
                sb.Append("DemonstrationProjectID,");
                sb.Append("ContractType,");
                sb.Append("ContractAmount,");
                sb.Append("PrincipalDiagnosis_Qual,");
                sb.Append("PrincipalDiagnosis,");
                sb.Append("Diag2_Qual,");
                sb.Append("Diag2,");
                sb.Append("Diag3_Qual,");
                sb.Append("Diag3,");
                sb.Append("Diag4_Qual,");
                sb.Append("Diag4,");
                sb.Append("Diag5_Qual,");
                sb.Append("Diag5,");
                sb.Append("Diag6_Qual,");
                sb.Append("Diag6,");
                sb.Append("Diag7_Qual,");
                sb.Append("Diag7,");
                sb.Append("Diag8_Qual,");
                sb.Append("Diag8,");
                sb.Append("Diag9_Qual,");
                sb.Append("Diag9,");
                sb.Append("Diag10_Qual,");
                sb.Append("Diag10,");
                sb.Append("Diag11_Qual,");
                sb.Append("Diag11,");
                sb.Append("Diag12_Qual,");
                sb.Append("Diag12,");
                sb.Append("Diag13_Qual,");
                sb.Append("Diag13,");
                sb.Append("Diag14_Qual,");
                sb.Append("Diag14,");
                sb.Append("Diag15_Qual,");
                sb.Append("Diag15,");
                sb.Append("Diag16_Qual,");
                sb.Append("Diag16,");
                sb.Append("Diag17_Qual,");
                sb.Append("Diag17,");
                sb.Append("Diag18_Qual,");
                sb.Append("Diag18,");
                sb.Append("Diag19_Qual,");
                sb.Append("Diag19,");
                sb.Append("Diag20_Qual,");
                sb.Append("Diag20,");
                sb.Append("Diag21_Qual,");
                sb.Append("Diag21,");
                sb.Append("Diag22_Qual,");
                sb.Append("Diag22,");
                sb.Append("Diag23_Qual,");
                sb.Append("Diag23,");
                sb.Append("Diag24_Qual,");
                sb.Append("Diag24,");
                sb.Append("Diag25_Qual,");
                sb.Append("Diag25,");
                sb.Append("AmbulanceTransportReasonCode,");
                sb.Append("TransportDistance ");
                sb.Append("from EncounterSubmissionHistory.dbo.SubmissionHistoryHeader a where a.ClaimNo in (");
                sb.Append("select distinct IehpClaimId from edrs.dbo.DHCSDenialMaster a ");
                sb.Append("inner join (select encounterreferencenumber,id,encounterstatus,transaction_id,row_number() over (partition by encounterreferencenumber order by id desc) as rn from IEHP_DHCSResponse.dbo.DHCSResponse_Encounter ");
                sb.Append(") b on b.rn=1 and a.IEHPClaimId=b.EncounterReferenceNumber ");
                sb.Append("inner join IEHP_DHCSResponse.dbo.DHCSResponse_Transaction c on b.Transaction_ID=c.ID ");
                sb.Append("inner join IEHP_DHCSResponse.dbo.DHCSResponse d on c.File_ID=d.ID ");
                sb.Append("inner join (select encounter_id,count(distinct issueid) as errorcounts from IEHP_DHCSResponse.dbo.DHCSResponse_EncounterResponse where Severity='Error' group by encounter_id) f on f.errorcounts=1 and f.Encounter_ID=b.ID ");
                sb.Append("cross apply (select top 1 encounter_id,severity,issueid,issnip,description from IEHP_DHCSResponse.dbo.DHCSResponse_EncounterResponse where Encounter_ID=b.id and Severity='Error' order by id desc) e ");
                sb.Append("where (d.ValidationStatus='Rejected' or (d.ValidationStatus='Accepted' and a.EncounterStatus<>'Accepted')) ");
                sb.Append("and (e.IssueId='0x39395df' or e.IssueId='0x001C3') ");
                sb.Append(") ");
                switch (Code)
                {
                    case "P305":
                        sb.Append("and claimtype='Professional' and right(rtrim(submitterid),3)='305'");
                        break;
                    case "I305":
                        sb.Append("and claimtype='Institutional' and right(rtrim(submitterid),3)='305'");
                        break;
                    case "P306":
                        sb.Append("and claimtype='Professional' and right(rtrim(submitterid),3)='306'");
                        break;
                    case "I306":
                        sb.Append("and claimtype='Institutional' and right(rtrim(submitterid),3)='306'");
                        break;
                }
                result = context.Database.SqlQuery<SubHist_Header>(sb.ToString()).ToList();
            }
            return result;
        }
        public static List<SubHist_Line> GetSubHistLine(string Code)
        {
            List<SubHist_Line> result = null;
            using (var context = new SubHistContext())
            {
                context.Database.CommandTimeout = 1800;
                StringBuilder sb = new StringBuilder();
                sb.Append("select ");
                sb.Append("b.ClaimNo,");
                sb.Append("ServiceLineNumber,");
                sb.Append("[Service Date] as ServiceDate,");
                sb.Append("ServiceIDQualifier,");
                sb.Append("ServiceID1,");
                sb.Append("ProcedureModifier1,");
                sb.Append("ProcedureModifier2,");
                sb.Append("ProcedureModifier3,");
                sb.Append("ProcedureModifier4,");
                sb.Append("Description,");
                sb.Append("ServiceID2,");
                sb.Append("ChargeAmount,");
                sb.Append("DiagnosisCodePointer1 as DiagnosisPointer1,");
                sb.Append("DiagnosisCodePointer2 as DiagnosisPointer2,");
                sb.Append("DiagnosisCodePointer3 as DiagnosisPointer3,");
                sb.Append("DiagnosisCodePointer4 as DiagnosisPointer4,");
                sb.Append("PayedAmount,");
                sb.Append("AdjustmentAmount as AdjustedAmount ");
                sb.Append("from EncounterSubmissionHistory.dbo.SubmissionHistServiceLine a inner join ");
                sb.Append("EncounterSubmissionHistory.dbo.SubmissionHistoryHeader b on a.SubHistID=b.SubHistoryID ");
                sb.Append("where b.ClaimNo in (");
                sb.Append("select distinct IehpClaimId from edrs.dbo.DHCSDenialMaster a ");
                sb.Append("inner join (select encounterreferencenumber,id,encounterstatus,transaction_id,row_number() over (partition by encounterreferencenumber order by id desc) as rn from IEHP_DHCSResponse.dbo.DHCSResponse_Encounter ");
                sb.Append(") b on b.rn=1 and a.IEHPClaimId=b.EncounterReferenceNumber ");
                sb.Append("inner join IEHP_DHCSResponse.dbo.DHCSResponse_Transaction c on b.Transaction_ID=c.ID ");
                sb.Append("inner join IEHP_DHCSResponse.dbo.DHCSResponse d on c.File_ID=d.ID ");
                sb.Append("inner join (select encounter_id,count(distinct issueid) as errorcounts from IEHP_DHCSResponse.dbo.DHCSResponse_EncounterResponse where Severity='Error' group by encounter_id) f on f.errorcounts=1 and f.Encounter_ID=b.ID ");
                sb.Append("cross apply (select top 1 encounter_id,severity,issueid,issnip,description from IEHP_DHCSResponse.dbo.DHCSResponse_EncounterResponse where Encounter_ID=b.id and Severity='Error' order by id desc) e ");
                sb.Append("where (d.ValidationStatus='Rejected' or (d.ValidationStatus='Accepted' and a.EncounterStatus<>'Accepted')) ");
                sb.Append("and (e.IssueId='0x39395df' or e.IssueId='0x001C3') ");
                sb.Append(") ");
                switch (Code)
                {
                    case "P305":
                        sb.Append("and b.claimtype='Professional' and right(rtrim(submitterid),3)='305'");
                        break;
                    case "I305":
                        sb.Append("and b.claimtype='Institutional' and right(rtrim(submitterid),3)='305'");
                        break;
                    case "P306":
                        sb.Append("and b.claimtype='Professional' and right(rtrim(submitterid),3)='306'");
                        break;
                    case "I306":
                        sb.Append("and b.claimtype='Institutional' and right(rtrim(submitterid),3)='306'");
                        break;
                }
                result = context.Database.SqlQuery<SubHist_Line>(sb.ToString()).ToList();
            }
            return result;

        }
        public static BillingProviderAdditionalDataElements GetBillingProviderAdditionalDataElements(string ProviderNPI)
        {
            BillingProviderAdditionalDataElements result = null;
            using (var context = new SubHistContext())
            {
                string sqltext = "select EIN,Prov1stPracticeAddress as ProviderAddress,ProvPracticeCity as ProviderCity,ProvPracticeState as ProviderState,ProvPracticeZip as ProviderZip,ProvPracticeCountry as ProviderCountry from bizsqlb1_edimanagement.dbo.NPI where NPI='" + ProviderNPI + "'";
                result = context.Database.SqlQuery<BillingProviderAdditionalDataElements>(sqltext).FirstOrDefault();
            }
            return result;
        }
        public static string GetPrimaryDiagnosisCode_Venus(string ClaimID)
        {
            string result = null;
            using (var context = new SubHistContext())
            {
                string sqltext = "select a.c02202 from venus_bizprod_wpc_valid_837p.dbo.c022_healthcarecodeinformation a inner join venus_bizprod_wpc_valid_837p.dbo.hi b on a.parentid=b.id inner join venus_bizprod_wpc_valid_837p.dbo.loops c on b.parentid=c.id and c.loopName='2300' inner join venus_bizprod_wpc_valid_837p.dbo.clm d on d.parentid=c.id where d.CLM01='" + ClaimID + "'";
                result = context.Database.SqlQuery<string>(sqltext).FirstOrDefault();
            }
            return result;
        }
        public static string GetPrimaryDiagnosisCode_Bizsqlb1(string ClaimID)
        {
            string result = null;
            using (var context = new SubHistContext())
            {
                string sqltext = "select a.c02202 from bizsqlb1_wpc_valid_837p.dbo.c022_healthcarecodeinformation a inner join bizsqlb1_wpc_valid_837p.dbo.hi b on a.parentid=b.id inner join bizsqlb1_wpc_valid_837p.dbo.loops c on b.parentid=c.id and c.loopName='2300' inner join bizsqlb1_wpc_valid_837p.dbo.clm d on d.parentid=c.id where d.CLM01='" + ClaimID + "'";
                result = context.Database.SqlQuery<string>(sqltext).FirstOrDefault();
            }
            return result;
        }

        public static List<HipaaXML> GetHipaaXMLForCAP0x001B1_P()
        {
            List<HipaaXML> result = null;
            using (var context = new SubHistContext())
            {
                context.Database.CommandTimeout = 900;
                StringBuilder sb = new StringBuilder();
                sb.Append("select c.clm01 as ClaimID,d.EncounterId,cast(transactiondocument as varchar(max)) as ClaimHipaaXML from Bizsqlb1_wpc_valid_837p.dbo.XMLDocument a ");
                sb.Append("inner join Bizsqlb1_wpc_valid_837p.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select parentid,clm01,row_number() over(partition by clm01 order by ordinal desc) rn from Bizsqlb1_wpc_valid_837p.dbo.clm) c on c.rn=1 and c.parentid=b.id ");
                sb.Append("outer apply (select top 1 encounterid from edireports.dbo.DHCS_Response_FlatList where encounterreferencenumber=c.clm01 order by encounterstatus,EncounterSubmissionDate desc) d ");
                sb.Append("where c.clm01 in (select distinct encounterid from EDIReports.dbo.DHCS_CAP_Denials where DHCSErrorNum='0x001B1') ");
                sb.Append("union all ");
                sb.Append("select c.clm01 as ClaimID,d.EncounterId,cast(transactiondocument as varchar(max)) as ClaimHipaaXML from Venus_bizprod_wpc_valid_837p.dbo.XMLDocument a ");
                sb.Append("inner join Venus_bizprod_wpc_valid_837p.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select parentid,clm01,row_number() over(partition by clm01 order by ordinal desc) rn from Venus_bizprod_wpc_valid_837p.dbo.clm) c on c.rn=1 and c.parentid=b.id ");
                sb.Append("outer apply (select top 1 encounterid from edireports.dbo.DHCS_Response_FlatList where encounterreferencenumber=c.clm01 order by encounterstatus,EncounterSubmissionDate desc) d ");
                sb.Append("where c.clm01 in (select distinct encounterid from EDIReports.dbo.DHCS_CAP_Denials where DHCSErrorNum='0x001B1')");
                result = context.Database.SqlQuery<HipaaXML>(sb.ToString()).ToList();
            }
            return result;
        }
        public static List<HipaaXML> GetHipaaXMLForCAP0x001B1_I()
        {
            List<HipaaXML> result = null;
            using (var context = new SubHistContext())
            {
                context.Database.CommandTimeout = 900;
                StringBuilder sb = new StringBuilder();
                sb.Append("select c.clm01 as ClaimID,d.EncounterId,cast(transactiondocument as varchar(max)) as ClaimHipaaXML from Venus_bizprod_wpc_valid_837i.dbo.XMLDocument a ");
                sb.Append("inner join Venus_bizprod_wpc_valid_837i.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select parentid,clm01,row_number() over(partition by clm01 order by ordinal desc) rn from Venus_bizprod_wpc_valid_837i.dbo.clm) c on c.rn=1 and c.parentid=b.id ");
                sb.Append("outer apply (select top 1 encounterid from edireports.dbo.DHCS_Response_FlatList where encounterreferencenumber=c.clm01 order by encounterstatus,EncounterSubmissionDate desc) d ");
                sb.Append("where c.clm01 in (select distinct encounterid from EDIReports.dbo.DHCS_CAP_Denials where DHCSErrorNum='0x001B1')");
                result = context.Database.SqlQuery<HipaaXML>(sb.ToString()).ToList();
            }
            return result;
        }

        public static List<HipaaXML> GetHipaaXMLForCAP0x001B2_I()
        {
            List<HipaaXML> result = null;
            using (var context = new SubHistContext())
            {
                context.Database.CommandTimeout = 900;
                StringBuilder sb = new StringBuilder();
                sb.Append("select c.clm01 as ClaimID,d.EncounterId,cast(transactiondocument as varchar(max)) as ClaimHipaaXML from Bizsqlb1_wpc_valid_837I.dbo.XMLDocument a ");
                sb.Append("inner join Bizsqlb1_wpc_valid_837I.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select parentid,clm01,row_number() over(partition by clm01 order by ordinal desc) rn from Bizsqlb1_wpc_valid_837I.dbo.clm) c on c.rn=1 and c.parentid=b.id ");
                sb.Append("outer apply (select top 1 encounterid from edireports.dbo.DHCS_Response_FlatList where encounterreferencenumber=c.clm01 order by encounterstatus,EncounterSubmissionDate desc) d ");
                sb.Append("where c.clm01 in (select distinct encounterid from EDIReports.dbo.DHCS_CAP_Denials where DHCSErrorNum='0x001B2')");
                sb.Append("union all ");
                sb.Append("select c.clm01 as ClaimID,d.EncounterId,cast(transactiondocument as varchar(max)) as ClaimHipaaXML from Venus_bizprod_wpc_valid_837I.dbo.XMLDocument a ");
                sb.Append("inner join Venus_bizprod_wpc_valid_837I.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select parentid,clm01,row_number() over(partition by clm01 order by ordinal desc) rn from Venus_bizprod_wpc_valid_837I.dbo.clm) c on c.rn=1 and c.parentid=b.id ");
                sb.Append("outer apply (select top 1 encounterid from edireports.dbo.DHCS_Response_FlatList where encounterreferencenumber=c.clm01 order by encounterstatus,EncounterSubmissionDate desc) d ");
                sb.Append("where c.clm01 in (select distinct encounterid from EDIReports.dbo.DHCS_CAP_Denials where DHCSErrorNum='0x001B2')");
                result = context.Database.SqlQuery<HipaaXML>(sb.ToString()).ToList();
            }
            return result;
        }
        public static List<HipaaXML> GetHipaaXMLForCAP0x001B3_P()
        {
            List<HipaaXML> result = null;
            using (var context = new SubHistContext())
            {
                context.Database.CommandTimeout = 900;
                StringBuilder sb = new StringBuilder();
                sb.Append("select c.clm01 as ClaimID,d.EncounterId,cast(transactiondocument as varchar(max)) as ClaimHipaaXML from Bizsqlb1_wpc_valid_837p.dbo.XMLDocument a ");
                sb.Append("inner join Bizsqlb1_wpc_valid_837p.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select parentid,clm01,row_number() over(partition by clm01 order by ordinal desc) rn from Bizsqlb1_wpc_valid_837p.dbo.clm) c on c.rn=1 and c.parentid=b.id ");
                sb.Append("outer apply (select top 1 encounterid from edireports.dbo.DHCS_Response_FlatList where encounterreferencenumber=c.clm01 order by encounterstatus,EncounterSubmissionDate desc) d ");
                sb.Append("where c.clm01 in (select distinct encounterid from EDIReports.dbo.DHCS_CAP_Denials where DHCSErrorNum='0x001B3')");
                result = context.Database.SqlQuery<HipaaXML>(sb.ToString()).ToList();
            }
            return result;
        }
        public static List<HipaaXML> GetHipaaXMLForCAPProcedureCode_P()
        {
            List<HipaaXML> result = null;
            using (var context = new SubHistContext())
            {
                context.Database.CommandTimeout = 900;
                StringBuilder sb = new StringBuilder();
                sb.Append("select c.clm01 as ClaimID,d.EncounterId,cast(transactiondocument as varchar(max)) as ClaimHipaaXML from Bizsqlb1_wpc_valid_837p.dbo.XMLDocument a ");
                sb.Append("inner join Bizsqlb1_wpc_valid_837p.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select parentid,clm01,row_number() over(partition by clm01 order by ordinal desc) rn from Bizsqlb1_wpc_valid_837p.dbo.clm) c on c.rn=1 and c.parentid=b.id ");
                sb.Append("outer apply (select top 1 encounterid from edireports.dbo.DHCS_Response_FlatList where encounterreferencenumber=c.clm01 order by encounterstatus,EncounterSubmissionDate desc) d ");
                sb.Append("where c.clm01 in (");
                sb.Append("select distinct EncounterId from EDIReports.dbo.DHCS_CAP_ProcCode_DetailedError ");
                sb.Append("where ErrorType ='HCPCS' ");
                sb.Append("and  ltrim(rtrim(ProcedureHCPCSCode)) ");
                sb.Append("IN ('X3900','X3902','X3904','X3906','X3908','X3910','X3920','X3922','X3924','X3926',");
                sb.Append("'X3930','X3936','X4100','X4102','X4104','X4106','X4108','X4110','X4112','X4118',");
                sb.Append("'X4300','X4301','X4308','X4312','X4320','X4500','X4501','X4502','X4504','X4522',");
                sb.Append("'X4526','X4544','Z0100','Z0102','Z0104','Z0106','Z0108','Z4302','Z4306','Z4310',");
                sb.Append("'Z4311','Z4312','Z4315','Z5414','Z5802','Z5820','Z5900','Z5902','Z5904','Z5906',");
                sb.Append("'Z5908','Z5910','Z5912','Z5914','Z5916','Z5918','Z5920','Z5922','Z5924','Z5940',");
                sb.Append("'Z5942','Z5944','Z5946','Z5950','Z5956','Z5966','Z6004','Z6006','Z6020','Z6404',");
                sb.Append("'Z9725','Z9726','Z9727'))");
                result = context.Database.SqlQuery<HipaaXML>(sb.ToString()).ToList();
            }
            return result;
        }
        public static List<HipaaXML> GetHipaaXMLForCAP0x001C3_P()
        {
            List<HipaaXML> result = null;
            using (var context = new SubHistContext())
            {
                context.Database.CommandTimeout = 900;
                StringBuilder sb = new StringBuilder();
                sb.Append("select c.clm01 as ClaimID,cast(transactiondocument as varchar(max)) as ClaimHipaaXML from Bizsqlb1_wpc_valid_837p.dbo.XMLDocument a ");
                sb.Append("inner join Bizsqlb1_wpc_valid_837p.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select parentid,clm01,row_number() over(partition by clm01 order by ordinal desc) rn from Bizsqlb1_wpc_valid_837p.dbo.clm) c on c.rn=1 and c.parentid=b.id ");
                sb.Append("where c.clm01 in (select distinct encounterid from EDIReports.dbo.DHCS_CAP_Denials where DHCSErrorNum='0x001C3') ");
                sb.Append("union all ");
                sb.Append("select c.clm01 as ClaimID,cast(transactiondocument as varchar(max)) as ClaimHipaaXML from venus_bizprod_wpc_valid_837p.dbo.XMLDocument a ");
                sb.Append("inner join venus_bizprod_wpc_valid_837p.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select parentid,clm01,row_number() over(partition by clm01 order by ordinal desc) rn from venus_bizprod_wpc_valid_837p.dbo.clm) c on c.rn=1 and c.parentid=b.id ");
                sb.Append("where c.clm01 in (select distinct encounterid from EDIReports.dbo.DHCS_CAP_Denials where DHCSErrorNum='0x001C3') ");
                sb.Append("union all ");
                sb.Append("select c.clm01 as ClaimID,cast(transactiondocument as varchar(max)) as ClaimHipaaXML from mars_wpc_valid_837p.dbo.XMLDocument a ");
                sb.Append("inner join mars_wpc_valid_837p.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select parentid,clm01,row_number() over(partition by clm01 order by ordinal desc) rn from mars_wpc_valid_837p.dbo.clm) c on c.rn=1 and c.parentid=b.id ");
                sb.Append("where c.clm01 in (select distinct encounterid from EDIReports.dbo.DHCS_CAP_Denials where DHCSErrorNum='0x001C3')");
                result = context.Database.SqlQuery<HipaaXML>(sb.ToString()).ToList();
            }
            return result;
        }
        public static List<HipaaXML> GetHipaaXMLForCAP0x001C3_I()
        {
            List<HipaaXML> result = null;
            using (var context = new SubHistContext())
            {
                context.Database.CommandTimeout = 900;
                StringBuilder sb = new StringBuilder();
                sb.Append("select c.clm01 as ClaimID,cast(transactiondocument as varchar(max)) as ClaimHipaaXML from venus_bizprod_wpc_valid_837I.dbo.XMLDocument a ");
                sb.Append("inner join venus_bizprod_wpc_valid_837I.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select parentid,clm01,row_number() over(partition by clm01 order by ordinal desc) rn from venus_bizprod_wpc_valid_837I.dbo.clm) c on c.rn=1 and c.parentid=b.id ");
                sb.Append("where c.clm01 in (select distinct encounterid from EDIReports.dbo.DHCS_CAP_Denials where DHCSErrorNum='0x001C3')");
                sb.Append(" union all ");
                sb.Append("select c.clm01 as ClaimID,cast(transactiondocument as varchar(max)) as ClaimHipaaXML from mars_wpc_valid_837I.dbo.XMLDocument a ");
                sb.Append("inner join mars_wpc_valid_837I.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select parentid,clm01,row_number() over(partition by clm01 order by ordinal desc) rn from mars_wpc_valid_837I.dbo.clm) c on c.rn=1 and c.parentid=b.id ");
                sb.Append("where c.clm01 in (select distinct encounterid from EDIReports.dbo.DHCS_CAP_Denials where DHCSErrorNum='0x001C3')");
                result = context.Database.SqlQuery<HipaaXML>(sb.ToString()).ToList();
            }
            return result;
        }
        public static List<HipaaXML> GetHipaaXMLForCAP0x001C7_P()
        {
            List<HipaaXML> result = null;
            using (var context = new SubHistContext())
            {
                context.Database.CommandTimeout = 1800;
                StringBuilder sb = new StringBuilder();
                sb.Append("select c.clm01 as ClaimID,d.EncounterId,cast(transactiondocument as varchar(max)) as ClaimHipaaXML from Bizsqlb1_wpc_valid_837p.dbo.XMLDocument a ");
                sb.Append("inner join Bizsqlb1_wpc_valid_837p.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select parentid,clm01,row_number() over(partition by clm01 order by ordinal desc) rn from Bizsqlb1_wpc_valid_837p.dbo.clm) c on c.rn=1 and c.parentid=b.id ");
                sb.Append("outer apply (select top 1 encounterid from edireports.dbo.DHCS_Response_FlatList where encounterreferencenumber=c.clm01 order by encounterstatus,EncounterSubmissionDate desc) d ");
                sb.Append("where c.clm01 in (select distinct encounterid from EDIReports.dbo.DHCS_CAP_Denials where DHCSErrorNum='0x001C7') ");
                sb.Append("union all ");
                sb.Append("select c.clm01 as ClaimID,d.EncounterId,cast(transactiondocument as varchar(max)) as ClaimHipaaXML from venus_bizprod_wpc_valid_837p.dbo.XMLDocument a ");
                sb.Append("inner join venus_bizprod_wpc_valid_837p.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select parentid,clm01,row_number() over(partition by clm01 order by ordinal desc) rn from venus_bizprod_wpc_valid_837p.dbo.clm) c on c.rn=1 and c.parentid=b.id ");
                sb.Append("outer apply (select top 1 encounterid from edireports.dbo.DHCS_Response_FlatList where encounterreferencenumber=c.clm01 order by encounterstatus,EncounterSubmissionDate desc) d ");
                sb.Append("where c.clm01 in (select distinct encounterid from EDIReports.dbo.DHCS_CAP_Denials where DHCSErrorNum='0x001C7') ");
                sb.Append("union all ");
                sb.Append("select c.clm01 as ClaimID,d.EncounterId,cast(transactiondocument as varchar(max)) as ClaimHipaaXML from mars_wpc_valid_837p.dbo.XMLDocument a ");
                sb.Append("inner join mars_wpc_valid_837p.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select parentid,clm01,row_number() over(partition by clm01 order by ordinal desc) rn from mars_wpc_valid_837p.dbo.clm) c on c.rn=1 and c.parentid=b.id ");
                sb.Append("outer apply (select top 1 encounterid from edireports.dbo.DHCS_Response_FlatList where encounterreferencenumber=c.clm01 order by encounterstatus,EncounterSubmissionDate desc) d ");
                sb.Append("where c.clm01 in (select distinct encounterid from EDIReports.dbo.DHCS_CAP_Denials where DHCSErrorNum='0x001C7')");
                result = context.Database.SqlQuery<HipaaXML>(sb.ToString()).ToList();
            }
            return result;
        }
        public static List<HipaaXML> GetHipaaXNLForCAP0x001C7_I()
        {
            List<HipaaXML> result = null;
            using (var context = new SubHistContext())
            {
                context.Database.CommandTimeout = 1800;
                StringBuilder sb = new StringBuilder();
                sb.Append("select c.clm01 as ClaimID,d.EncounterId,cast(transactiondocument as varchar(max)) as ClaimHipaaXML from venus_bizprod_wpc_valid_837I.dbo.XMLDocument a ");
                sb.Append("inner join venus_bizprod_wpc_valid_837I.dbo.loops b on a.transactionid=b.transactionid and b.loopName='2300' ");
                sb.Append("inner join (select parentid,clm01,row_number() over(partition by clm01 order by ordinal desc) rn from venus_bizprod_wpc_valid_837I.dbo.clm) c on c.rn=1 and c.parentid=b.id  ");
                sb.Append("outer apply (select top 1 encounterid from edireports.dbo.DHCS_Response_FlatList where encounterreferencenumber=c.clm01 order by encounterstatus,EncounterSubmissionDate desc) d ");
                sb.Append("where c.clm01 in (select distinct encounterid from EDIReports.dbo.DHCS_CAP_Denials where DHCSErrorNum='0x001C7')");
                result = context.Database.SqlQuery<HipaaXML>(sb.ToString()).ToList();
            }
            return result;
        }
    }
}
