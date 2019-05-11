using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportModule.Model
{
    public class SubHist_Header
    {
        public string ProductionFlag { get; set; }
        public string GroupControlNumber { get; set; }
        public string ClaimNo { get; set; }
        public string PlaceOfService { get; set; }
        public string FacilityCode { get; set; }
        public string ClaimFrequency { get; set; }
        public string BillProvNPI { get; set; }
        public string BillProvLast { get; set; }
        public string BillProvFirst { get; set; }
        public string BillProvSpecialty { get; set; }
        public string BillProvZip { get; set; }
        public string SubscriberLast { get; set; }
        public string SubscriberFirst { get; set; }
        public string SubscriberID { get; set; }
        public string SubscriberZip { get; set; }
        public string SubscriberDOB { get; set; }
        public string SubscriberGender { get; set; }
        public string SubscriberSSN { get; set; }
        public string PatientLast { get; set; }
        public string PatientFirst { get; set; }
        public string PatientID { get; set; }
        public string PatientSSN { get; set; }
        public string PatientDOB { get; set; }
        public string PatientGender { get; set; }
        public string RendProvNPI { get; set; }
        public string RendProvLast { get; set; }
        public string RendProvFirst { get; set; }
        public string PickUpAddress { get; set; }
        public string PickUpAddress2 { get; set; }
        public string PickUpCity { get; set; }
        public string PickUpState { get; set; }
        public string PickUpZip { get; set; }
        public string DropOffName { get; set; }
        public string DropOffAddress { get; set; }
        public string DropOffAddress2 { get; set; }
        public string DropOffCity { get; set; }
        public string DropOffState { get; set; }
        public string DropOffZip { get; set; }
        public string BilledAmount { get; set; }
        public string EstimatedAmountDue { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string AdmissionDate { get; set; }
        public string AdmissionType { get; set; }
        public string AdmissionSource { get; set; }
        public string DischargeDate { get; set; }
        public string PatientStatus { get; set; }
        public string ClearinghouseID { get; set; }
        public string PayerClaimControlNumber { get; set; }
        public string DemonstrationProjectID { get; set; }
        public string ContractType { get; set; }
        public string ContractAmount { get; set; }
        public string PrincipalDiagnosis_Qual { get; set; }
        public string PrincipalDiagnosis { get; set; }
        public string Diag2_Qual { get; set; }
        public string Diag2 { get; set; }
        public string Diag3_Qual { get; set; }
        public string Diag3 { get; set; }
        public string Diag4_Qual { get; set; }
        public string Diag4 { get; set; }
        public string Diag5_Qual { get; set; }
        public string Diag5 { get; set; }
        public string Diag6_Qual { get; set; }
        public string Diag6 { get; set; }
        public string Diag7_Qual { get; set; }
        public string Diag7 { get; set; }
        public string Diag8_Qual { get; set; }
        public string Diag8 { get; set; }
        public string Diag9_Qual { get; set; }
        public string Diag9 { get; set; }
        public string Diag10_Qual { get; set; }
        public string Diag10 { get; set; }
        public string Diag11_Qual { get; set; }
        public string Diag11 { get; set; }
        public string Diag12_Qual { get; set; }
        public string Diag12 { get; set; }
        public string Diag13_Qual { get; set; }
        public string Diag13 { get; set; }
        public string Diag14_Qual { get; set; }
        public string Diag14 { get; set; }
        public string Diag15_Qual { get; set; }
        public string Diag15 { get; set; }
        public string Diag16_Qual { get; set; }
        public string Diag16 { get; set; }
        public string Diag17_Qual { get; set; }
        public string Diag17 { get; set; }
        public string Diag18_Qual { get; set; }
        public string Diag18 { get; set; }
        public string Diag19_Qual { get; set; }
        public string Diag19 { get; set; }
        public string Diag20_Qual { get; set; }
        public string Diag20 { get; set; }
        public string Diag21_Qual { get; set; }
        public string Diag21 { get; set; }
        public string Diag22_Qual { get; set; }
        public string Diag22 { get; set; }
        public string Diag23_Qual { get; set; }
        public string Diag23 { get; set; }
        public string Diag24_Qual { get; set; }
        public string Diag24 { get; set; }
        public string Diag25_Qual { get; set; }
        public string Diag25 { get; set; }
        public string AmbulanceTransportReasonCode { get; set; }
        public string TransportDistance { get; set; }

    }

    public class SubHist_Line
    {
        public string ClaimNo { get; set; }
        public string ServiceLineNumber { get; set; }
        public string ServiceDate { get; set; }
        public string ServiceIDQualifier { get; set; }
        public string ServiceID1 { get; set; }
        public string ProcedureModifier1 { get; set; }
        public string ProcedureModifier2 { get; set; }
        public string ProcedureModifier3 { get; set; }
        public string ProcedureModifier4 { get; set; }
        public string Description { get; set; }
        public string ServiceID2 { get; set; }
        public string ChargeAmount { get; set; }
        public string DiagnosisPointer1 { get; set; }
        public string DiagnosisPointer2 { get; set; }
        public string DiagnosisPointer3 { get; set; }
        public string DiagnosisPointer4 { get; set; }
        public string PayedAmount { get; set; }
        public string AdjustedAmount { get; set; }//use default CO, 45

    }
}
