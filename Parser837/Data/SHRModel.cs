using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Parser837
{
    public class ClaimTempHost
    {
        [Key]
        public string ClaimID { get; set; }
    }
    public class ClaimProvider
    {
        [Key]
        public long ID { get; set; }
        public int FileID { get; set; }
        public string LoopName { get; set; }
        public string ClaimID { get; set; }
        public string ServiceLineNumber { get; set; }
        public string ProviderQualifier { get; set; }
        //from PRV-2000A
        public string ProviderTaxonomyCode { get; set; }
        //from CUR-2000A
        public string ProviderCurrencyCode { get; set; }
        //from NM1*85, 2010AA
        public string ProviderLastName { get; set; }
        public string ProviderFirstName { get; set; }
        public string ProviderMiddle { get; set; }
        public string ProviderSuffix { get; set; }
        public string ProviderIDQualifier { get; set; }
        public string ProviderID { get; set; }
        //from N3-2010AA
        public string ProviderAddress { get; set; }
        public string ProviderAddress2 { get; set; }
        //from N4-2010AA
        public string ProviderCity { get; set; }
        public string ProviderState { get; set; }
        public string ProviderZip { get; set; }
        public string ProviderCountry { get; set; }
        public string ProviderCountrySubCode { get; set; }
        ////from REF-2010AA, go to secondaryidentification
        ////from REF-2330B, go to secondaryidentification
        public string RepeatSequence { get; set; }//for referring provider only
    }
    public class ProviderContact
    {
        [Key]
        public long ID { get; set; }
        public int FileID { get; set; }
        public string ClaimID { get; set; }
        public string ServiceLineNumber { get; set; }
        public string LoopName { get; set; }
        public string ProviderQualifier { get; set; }
        public string ProviderNPI { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Phone { get; set; }
        public string PhoneEx { get; set; }
    }
    public class ClaimHeader
    {
        [Key]
        public long ID { get; set; }
        public int FileID { get; set; }
        //from CLM-2300
        public string ClaimID { get; set; }
        public string ClaimAmount { get; set; }
        public string ClaimPOS { get; set; }
        public string ClaimType { get; set; }
        public string ClaimFrequencyCode { get; set; }
        public string ClaimProviderSignature { get; set; }
        public string ClaimProviderAssignment { get; set; }
        public string ClaimBenefitAssignment { get; set; }
        public string ClaimReleaseofInformationCode { get; set; }
        public string ClaimPatientSignatureSourceCode { get; set; }
        public string ClaimRelatedCausesQualifier { get; set; }
        public string ClaimRelatedCausesCode { get; set; }
        public string ClaimRelatedStateCode { get; set; }
        public string ClaimRelatedCountryCode { get; set; }
        public string ClaimSpecialProgramCode { get; set; }
        public string ClaimDelayReasonCode { get; set; }
        //from DTP-2300
        public string CurrentIllnessDate { get; set; }
        public string InitialTreatmentDate { get; set; }
        public string LastSeenDate { get; set; }
        public string AcuteManifestestationDate { get; set; }
        public string AccidentDate { get; set; }
        public string LastMenstrualPeriodDate { get; set; }
        public string LastXrayDate { get; set; }
        public string PrescriptionDate { get; set; }
        public string DisabilityDate { get; set; }
        public string DisabilityStartDate { get; set; }
        public string DisabilityEndDate { get; set; }
        public string LastWorkedDate { get; set; }
        public string AuthorizedReturnToWorkDate { get; set; }
        public string AdmissionDate { get; set; }
        public string DischargeDate { get; set; }
        public string AssumedStartDate { get; set; }
        public string AssumedEndDate { get; set; }
        public string FirstContactDate { get; set; }
        public string RepricerReceivedDate { get; set; }
        //from PWK-2300, in separate table to hold up to 10 repeats
        //from CN1-2300
        public string ContractTypeCode { get; set; }
        public string ContractAmount { get; set; }
        public string ContractPercentage { get; set; }
        public string ContractCode { get; set; }
        public string ContractTermsDiscountPercentage { get; set; }
        public string ContractVersionIdentifier { get; set; }
        //from AMT-2300
        public string PatientPaidAmount { get; set; }
        //from REF-2300
        public string ServiceAuthorizationExceptionCode { get; set; }
        public string MedicareSection4081Indicator { get; set; }
        public string MammographyCertificationNumber { get; set; }
        public string ReferralNumber { get; set; }
        public string PriorAuthorizationNumber { get; set; }
        public string PayerClaimControlNumber { get; set; }
        public string ClinicalLabNumber { get; set; }
        public string RepricedClaimNumber { get; set; }
        public string AdjustedClaimNumber { get; set; }
        public string InvestigationalID { get; set; }
        public string ValueAddedNetworkTraceNumber { get; set; }
        public string MedicalRecordNumber { get; set; }
        public string DemonstrationID { get; set; }
        public string CarePlanOversightNumber { get; set; }
        //from K3-2300, in separate table to hold up to 10 repeats
        //from NTE-2300, go to separate table
        //from CR1-2300
        public string AmbulanceWeight { get; set; }
        public string AmbulanceReasonCode { get; set; }
        public string AmbulanceQuantity { get; set; }
        public string AmbulanceRoundTripDescription { get; set; }
        public string AmbulanceStretcherDescription { get; set; }
        //from CR2-2300
        public string PatientConditionCode { get; set; }
        public string PatientConditionDescription1 { get; set; }
        public string PatientConditionDescription2 { get; set; }
        //from CRC-2300, in separate table to hold up to 3 repeats
        //from hi-2300, in separate table to hold up to 12-p/25-i diagnosis codes, anesthesia procedure code, BP/BO, condition code BG up to 24
        //from HCP-2300
        public string PricingMethodology { get; set; }
        public string RepricedAllowedAmount { get; set; }
        public string RepricedSavingAmount { get; set; }
        public string RepricingOrganizationID { get; set; }
        public string RepricingRate { get; set; }
        public string RepricedGroupCode { get; set; }
        public string RepricedGroupAmount { get; set; }
        //the following three items are from institutional, not available for professional
        public string RepricingRevenueCode { get; set; }
        public string RepricingUnit { get; set; }
        public string RepricingQuantity { get; set; }
        public string RejectReasonCode { get; set; }
        public string PolicyComplianceCode { get; set; }
        public string ExceptionCode { get; set; }
        //from institutional, 2300-DTP
        public string StatementFromDate { get; set; }
        public string StatementToDate { get; set; }
        //from institutional 2300-CL1
        public string AdmissionTypeCode { get; set; }
        public string AdmissionSourceCode { get; set; }
        public string PatientStatusCode { get; set; }
        //from institutional 2300-F3
        public string PatientResponsibilityAmount { get; set; }
    }
    public class ClaimPatient
    {
        //from PAT-2000C, loopo repeat>1
        [Key]
        public long ID { get; set; }
        public int FileID { get; set; }
        public string ClaimID { get; set; }
        public string PatientRelatedCode { get; set; }
        public string PatientRelatedDeathDate { get; set; }
        public string PatientRelatedUnit { get; set; }
        public string PatientRelatedWeight { get; set; }
        public string PatientRelatedPregnancyIndicator { get; set; }
        //from NM1*QC-2010CA
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientMiddle { get; set; }
        public string PatientSuffix { get; set; }
        //from N3-2010CA
        public string PatientAddress { get; set; }
        public string PatientAddress2 { get; set; }
        //from N4-2010CA
        public string PatientCity { get; set; }
        public string PatientState { get; set; }
        public string PatientZip { get; set; }
        public string PatientCountry { get; set; }
        public string PatientCountrySubCode { get; set; }
        //from DMG-2010CA
        public string PatientBirthDate { get; set; }
        public string PatientGender { get; set; }
        //from REF-2010CA
        public string PatientClaimNumber { get; set; }
        public string PatientSSN { get; set; }
        public string PatientMemberID { get; set; }
        //from PER-2010CA
        public string PatientContactName { get; set; }
        public string PatientContactPhone { get; set; }
        public string PatientContactPhoneEx { get; set; }
    }
    public class ClaimSecondaryIdentification
    {
        [Key]
        public long ID { get; set; }
        public int FileID { get; set; }
        public string ClaimID { get; set; }
        public string ServiceLineNumber { get; set; }
        public string LoopName { get; set; }
        public string ProviderQualifier { get; set; }
        public string ProviderID { get; set; }
        public string OtherPayerPrimaryIDentification { get; set; } //only for referral number, prior authorization
        public string RepeatSequence { get; set; }//only for referring provider
    }
    public class ClaimPWK
    {
        [Key]
        public long ID { get; set; }
        public int FileID { get; set; }
        public string ClaimID { get; set; }
        public string ServiceLineNumber { get; set; }
        public string ReportTypeCode { get; set; }
        public string ReportTransmissionCode { get; set; }
        public string AttachmentControlNumber { get; set; }
    }
    public class ClaimK3
    {
        [Key]
        public long ID { get; set; }
        public int FileID { get; set; }
        public string ClaimID { get; set; }
        public string ServiceLineNumber { get; set; }
        public string K3 { get; set; }
    }
    public class ClaimNte
    {
        [Key]
        public long ID { get; set; }
        public int FileID { get; set; }
        public string ClaimID { get; set; }
        public string ServiceLineNumber { get; set; }
        public string NoteCode { get; set; }
        public string NoteText { get; set; }
    }
    public class ClaimCRC
    {
        [Key]
        public long ID { get; set; }
        public int FileID { get; set; }
        public string ClaimID { get; set; }
        public string ServiceLineNumber { get; set; }
        public string CodeCategory { get; set; }
        public string ConditionIndicator { get; set; }
        public string ConditionCode { get; set; }
        public string ConditionCode2 { get; set; }
        public string ConditionCode3 { get; set; }
        public string ConditionCode4 { get; set; }
        public string ConditionCode5 { get; set; }
    }
    public class ClaimHI
    {
        [Key]
        public long ID { get; set; }
        public int FileID { get; set; }
        public string ClaimID { get; set; }
        public string HIQual { get; set; }
        public string HICode { get; set; }
        public string PresentOnAdmissionIndicator { get; set; }
        public string HIFromDate { get; set; }
        public string HIToDate { get; set; }
        public string HIAmount { get; set; }
    }
    public class ClaimSBR
    {
        //2320, other subscribers
        [Key]
        public long ID { get; set; }
        public int FileID { get; set; }
        public string ClaimID { get; set; }
        public string LoopName { get; set; }
        public string SubscriberSequenceNumber { get; set; }
        public string SubscriberRelationshipCode { get; set; }
        public string InsuredGroupNumber { get; set; }
        public string OtherInsuredGroupName { get; set; }
        public string InsuredTypeCode { get; set; }
        public string ClaimFilingCode { get; set; }
        //from pat
        public string DeathDate { get; set; }
        public string Unit { get; set; }
        public string Weight { get; set; }
        public string PregnancyIndicator { get; set; }
        //from nm1*il-2000b
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MidddleName { get; set; }
        public string NameSuffix { get; set; }
        public string IDQualifier { get; set; }
        public string IDCode { get; set; }
        //from N3-2010BA
        public string SubscriberAddress { get; set; }
        public string SubscriberAddress2 { get; set; }
        //from N4-2010BA
        public string SubscriberCity { get; set; }
        public string SubscriberState { get; set; }
        public string SubscriberZip { get; set; }
        public string SubscriberCountry { get; set; }
        public string SubscriberCountrySubCode { get; set; }
        //from DMG-2010BA
        public string SubscriberBirthDate { get; set; }
        public string SubscriberGender { get; set; }
        //from REF-2010BA
        public string SubscriberSSN { get; set; }//social security number
        public string SubscriberClaimNumber { get; set; }//property cacualty claim number
        //from PER-2010BA
        public string SubscriberContactName { get; set; }
        public string SubscriberContactPhone { get; set; }
        public string SubscriberContactPhoneEx { get; set; }
        //from CAS-2320, in separate table to hold up to 5 repeats, 30 reason codes
        //from AMT-2320
        public string COBPayerPaidAmount { get; set; }
        public string COBNonCoveredAmount { get; set; }
        public string COBRemainingPatientLiabilityAmount { get; set; }
        //from OI-2320
        public string BenefitsAssignmentCertificationIndicator { get; set; }
        public string PatientSignatureSourceCode { get; set; }
        public string ReleaseOfInformationCode { get; set; }
        //from MOA-2320
        public string ReimbursementRate { get; set; }
        public string HCPCSPayableAmount { get; set; }
        public string MOA_ClaimPaymentRemarkCode1 { get; set; }
        public string MOA_ClaimPaymentRemarkCode2 { get; set; }
        public string MOA_ClaimPaymentRemarkCode3 { get; set; }
        public string MOA_ClaimPaymentRemarkCode4 { get; set; }
        public string MOA_ClaimPaymentRemarkCode5 { get; set; }
        public string EndStageRenalDiseasePaymentAmount { get; set; }
        public string MOA_NonPayableProfessionalComponentBilledAmount { get; set; }
        //from institutional 2320-MIA
        public string CoveredDays { get; set; }
        public string LifetimePsychiatricDays { get; set; }
        public string ClaimDRGAmount { get; set; }
        public string MIA_ClaimPaymentRemarkCode1 { get; set; }
        public string ClaimDisproportionateShareAmount { get; set; }
        public string ClaimMSPPassThroughAmount { get; set; }
        public string ClaimPPSCapitalAmount { get; set; }
        public string PPSCapitalFSPDRGAmount { get; set; }
        public string PPSCapitalHSPDRGAmount { get; set; }
        public string PPSCapitalDSHDRGAmount { get; set; }
        public string OldCapitalAmount { get; set; }
        public string PPSCapitalIMEAmount { get; set; }
        public string PPSOperatingHospitalSpecificDRGAmount { get; set; }
        public string CostReportDayCount { get; set; }
        public string PPSOperatingFederalSpecificDRGAmount { get; set; }
        public string ClaimPPSCapitalOutlierAmount { get; set; }
        public string ClaimIndirectTeachingAmount { get; set; }
        public string MIA_NonPayableProfessionalComponentBilledAmount { get; set; }
        public string MIA_ClaimPaymentRemarkCode2 { get; set; }
        public string MIA_ClaimPaymentRemarkCode3 { get; set; }
        public string MIA_ClaimPaymentRemarkCode4 { get; set; }
        public string MIA_ClaimPaymentRemarkCode5 { get; set; }
        public string PPSCapitalExceptionAmount { get; set; }
        //from NM1*IL-2330A
        //from N3-2330A
        //from N4-2330A
        //from REF-2330A
        //from DTP-2330B
        public string PaymentDate { get; set; }
    }
    public class ClaimCAS
    {
        [Key]
        public long ID { get; set; }
        public int FileID { get; set; }
        public string ClaimID { get; set; }
        public string ServiceLineNumber { get; set; }
        public string SubscriberSequenceNumber { get; set; }
        public string GroupCode { get; set; }
        public string ReasonCode { get; set; }
        public string AdjustmentAmount { get; set; }
        public string AdjustmentQuantity { get; set; }
    }
    public class ClaimLineMEA
    {
        [Key]
        public long ID { get; set; }
        public int FileID { get; set; }
        public string ClaimID { get; set; }
        public string ServiceLineNumber { get; set; }
        public string TestCode { get; set; }
        public string MeasurementQualifier { get; set; }
        public string TestResult { get; set; }
    }
    public class ClaimLineAuth
    {
        [Key]
        public long ID { get; set; }
        public int FileID { get; set; }
        public string ClaimID { get; set; }
        public string ServiceLineNumber { get; set; }
        public string ReferenceQualifier { get; set; }
        public string ReferralNumber { get; set; }
        public string OtherPayerPrimaryIdentifier { get; set; }
    }
    public class ClaimLineSVD
    {
        [Key]
        public long ID { get; set; }
        public int FileID { get; set; }
        public string ClaimID { get; set; }
        public string ServiceLineNumber { get; set; }
        public string RepeatSequence { get; set; }
        public string OtherPayerPrimaryIdentifier { get; set; }
        public string ServiceLinePaidAmount { get; set; }
        public string ServiceQualifier { get; set; }
        public string ProcedureCode { get; set; }
        public string ProcedureModifier1 { get; set; }
        public string ProcedureModifier2 { get; set; }
        public string ProcedureModifier3 { get; set; }
        public string ProcedureModifier4 { get; set; }
        public string ProcedureDescription { get; set; }
        public string PaidServiceUnitCount { get; set; }
        public string BundledLineNumber { get; set; }
        public string ServiceLineRevenueCode { get; set; }
    }
    public class ClaimLineLQ
    {
        [Key]
        public long ID { get; set; }
        public int FileID { get; set; }
        public string ClaimID { get; set; }
        public string ServiceLineNumber { get; set; }
        public string LQSequence { get; set; }
        public string FormQualifier { get; set; }
        public string IndustryCode { get; set; }
    }
    public class ClaimLineFRM
    {
        [Key]
        public long ID { get; set; }
        public int FileID { get; set; }
        public string ClaimID { get; set; }
        public string ServiceLineNumber { get; set; }
        public string LQSequence { get; set; }
        public string QuestionNumber { get; set; }
        public string QuestionResponseIndicator { get; set; }
        public string QuestionResponse { get; set; }
        public string QuestionResponseDate { get; set; }
        public string AllowedChargePercentage { get; set; }

    }
    public class ServiceLine
    {
        [Key]
        public int ID { get; set; }
        public int FileID { get; set; }
        public string ClaimID { get; set; }
        public string ServiceLineNumber { get; set; }
        //from LX-2400
        //from SV1-2400
        public string ServiceIDQualifier { get; set; }
        public string ProcedureCode { get; set; }
        public string ProcedureModifier1 { get; set; }
        public string ProcedureModifier2 { get; set; }
        public string ProcedureModifier3 { get; set; }
        public string ProcedureModifier4 { get; set; }
        public string ProcedureDescription { get; set; }
        public string LineItemChargeAmount { get; set; }
        public string LineItemUnit { get; set; }
        public string ServiceUnitQuantity { get; set; }
        public string LineItemPOS { get; set; }
        public string DiagPointer1 { get; set; }
        public string DiagPointer2 { get; set; }
        public string DiagPointer3 { get; set; }
        public string DiagPointer4 { get; set; }
        public string EmergencyIndicator { get; set; }
        public string EPSDTIndicator { get; set; }
        public string FamilyPlanningIndicator { get; set; }
        public string CopayStatusCode { get; set; }
        //from SV5-2400
        public string DMEQualifier { get; set; }
        public string DMEProcedureCode { get; set; }
        public string DMEDays { get; set; }
        public string DMERentalPrice { get; set; }
        public string DMEPurchasePrice { get; set; }
        public string DMEFrequencyCode { get; set; }
        //from PWK-2400, in separate table
        //from CR1-2400
        public string PatientWeight { get; set; }
        public string AmbulanceTransportReasonCode { get; set; }
        public string TransportDistance { get; set; }
        public string RoundTripPurposeDescription { get; set; }
        public string StretcherPueposeDescription { get; set; }
        //from CR3-2400
        public string CertificationTypeCode { get; set; }
        public string DMEDuration { get; set; }
        //from CRC-2400, in separate table
        //from DTP-2400
        public string ServiceFromDate { get; set; }
        public string ServiceToDate { get; set; }
        public string PrescriptionDate { get; set; }
        public string CertificationDate { get; set; }
        public string BeginTherapyDate { get; set; }
        public string LastCertificationDate { get; set; }
        public string LastSeenDate { get; set; }
        public string TestDateHemo { get; set; }
        public string TestDateSerum { get; set; }
        public string ShippedDate { get; set; }
        public string LastXrayDate { get; set; }
        public string InitialTreatmentDate { get; set; }
        //from QTY-2400
        public string AmbulancePatientCount { get; set; }
        public string ObstetricAdditionalUnits { get; set; }
        //from MEA-2400, in separate table
        //from CN1-2400
        public string ContractTypeCode { get; set; }
        public string ContractAmount { get; set; }
        public string ContractPercentage { get; set; }
        public string ContractCode { get; set; }
        public string TermsDiscountPercentage { get; set; }
        public string ContractVersionIdentifier { get; set; }
        //from REF-2400
        public string RepricedLineItemReferenceNumber { get; set; }
        public string AdjustedRepricedLineItemReferenceNumber { get; set; }
        public string LineItemControlNumber { get; set; }
        public string MammographyCertificationNumber { get; set; }
        public string ClinicalLabNumber { get; set; }
        public string ReferringCLIANumber { get; set; }
        public string ImmunizationBatchNumber { get; set; }
        //from AMT-2400
        public string SalesTaxAmount { get; set; }
        public string PostageClaimedAmount { get; set; }
        //from K3-2400, in separate table
        //from NTE-2400, in separate table
        //from PS1-2400
        public string PurchasedServiceProviderIdentifier { get; set; }
        public string PurchasedServiceChargeAmount { get; set; }
        //from HCP-2400
        public string PricingMethodology { get; set; }
        public string RepricedAllowedAmount { get; set; }
        public string RepricedSavingAmount { get; set; }
        public string RepricingOrganizationIdentifier { get; set; }
        public string RepricingRate { get; set; }
        public string RepricedAmbulatoryPatientGroupCode { get; set; }
        public string RepricedAmbulatoryPatientGroupAmount { get; set; }
        public string HCPQualifier { get; set; }
        public string RepricedHCPCSCode { get; set; }
        public string RepricingUnit { get; set; }
        public string RepricingQuantity { get; set; }
        public string RejectReasonCode { get; set; }
        public string PolicyComplianceCode { get; set; }
        public string ExceptionCode { get; set; }
        //from LIN-2410
        public string LINQualifier { get; set; }
        public string NationalDrugCode { get; set; }
        //from CTP-2410
        public string DrugQuantity { get; set; }
        public string DrugQualifier { get; set; }
        //from REF-2410
        public string LinkSequenceNumber { get; set; }
        public string PharmacyPrescriptionNumber { get; set; }
        //from DTP-2430
        public string AdjudicationDate { get; set; }
        //from AMT-2430
        public string ReaminingPatientLiabilityAmount { get; set; }
        //from LQ-2440, in separate table
        //from institutional sv2
        public string RevenueCode { get; set; }
        public string LineItemDeniedChargeAmount { get; set; }
        //from institutional 2400-AMT
        public string FacilityTaxAmount { get; set; }

    }
    public class SubmissionLog
    {
        [Key]
        public int FileID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public string ReportType { get; set; }
        public int EncounterCount { get; set; }
        //from ISA
        public string SubmitterID { get; set; }
        public string ReceiverID { get; set; }
        public string InterchangeControlNumber { get; set; }

        public string ProductionFlag { get; set; }
        //from GS
        public string InterchangeDate { get; set; }
        public string InterchangeTime { get; set; }
        //from BHT
        public string BatchControlNumber { get; set; }
        //from NM1*41, loop 1000A
        public string SubmitterLastName { get; set; }
        public string SubmitterFirstName { get; set; }
        public string SubmitterMiddle { get; set; }
        //from per-1000A
        public string SubmitterContactName1 { get; set; }
        public string SubmitterContactEmail1 { get; set; }
        public string SubmitterContactFax1 { get; set; }
        public string SubmitterContactPhone1 { get; set; }
        public string SubmitterContactPhoneEx1 { get; set; }
        public string SubmitterContactName2 { get; set; }
        public string SubmitterContactEmail2 { get; set; }
        public string SubmitterContactFax2 { get; set; }
        public string SubmitterContactPhone2 { get; set; }
        public string SubmitterContactPhoneEx2 { get; set; }
        //from NM1*40, 1000B
        public string ReceiverLastName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class Claim
    {
        public ClaimHeader Header { get; set; }
        public List<ServiceLine> Lines { get; set; }
        public List<ClaimProvider> Providers { get; set; }
        public List<ProviderContact> ProviderContacts { get; set; }
        public List<ClaimSecondaryIdentification> SecondaryIdentifications { get; set; }
        public List<ClaimPWK> PWKs { get; set; }
        public List<ClaimK3> K3s { get; set; }
        public List<ClaimNte> Notes { get; set; }
        public List<ClaimCRC> CRCs { get; set; }
        public List<ClaimHI> His { get; set; }
        public List<ClaimSBR> Subscribers { get; set; }
        public List<ClaimPatient> Patients { get; set; }
        public List<ClaimCAS> Cases { get; set; }
        public List<ClaimLineMEA> Meas { get; set; }
        public List<ClaimLineAuth> Auths { get; set; }
        public List<ClaimLineSVD> SVDs { get; set; }
        public List<ClaimLineLQ> LQs { get; set; }
        public List<ClaimLineFRM> FRMs { get; set; }
        public Claim()
        {
            Header = new ClaimHeader();
            Lines = new List<ServiceLine>();
            Providers = new List<ClaimProvider>();
            ProviderContacts = new List<ProviderContact>();
            SecondaryIdentifications = new List<ClaimSecondaryIdentification>();
            PWKs = new List<ClaimPWK>();
            K3s = new List<ClaimK3>();
            Notes = new List<ClaimNte>();
            CRCs = new List<ClaimCRC>();
            His = new List<ClaimHI>();
            Subscribers = new List<ClaimSBR>();
            Patients = new List<ClaimPatient>();
            Cases = new List<ClaimCAS>();
            Meas = new List<ClaimLineMEA>();
            Auths = new List<ClaimLineAuth>();
            SVDs = new List<ClaimLineSVD>();
            LQs = new List<ClaimLineLQ>();
            FRMs = new List<ClaimLineFRM>();
        }
    }
}
