if not exists (select * from sys.schemas where name='Sub_History')
begin
exec ('create schema Sub_History')
end

if object_id('Sub_History.ClaimProviders') is not null drop table Sub_History.ClaimProviders
go
create table Sub_History.ClaimProviders
(
ID bigint identity(1,1) not null,
FileID int not null,
LoopName varchar(50) null,
ClaimID varchar(50) null,
ServiceLineNumber varchar(50) null,
ProviderQualifier varchar(50) null,
ProviderTaxonomyCode varchar(50) null,
ProviderCurrencyCode varchar(50) null,
ProviderLastName varchar(100) null,
ProviderFirstName varchar(50) null,
ProviderMiddle varchar(50) null,
ProviderSuffix varchar(50) null,
ProviderIDQualifier varchar(50) null,
ProviderID varchar(50) null,
ProviderAddress varchar(100) null,
ProviderAddress2 varchar(50) null,
ProviderCity varchar(50) null,
ProviderState varchar(50) null,
ProviderZip varchar(50) null,
ProviderCountry varchar(50) null,
ProviderCountrySubCode varchar(50) null,
RepeatSequence varchar(50) null,
CONSTRAINT [PK_Sub_History.ClaimProviders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

if object_id('Sub_History.ProviderContacts') is not null drop table Sub_History.ProviderContacts
go
create table Sub_History.ProviderContacts
(
ID bigint identity(1,1) not null,
FileID int not null,
ClaimID varchar(50) null,
ServiceLineNumber varchar(50) null,
LoopName varchar(50) null,
ProviderQualifier varchar(50) null,
ProviderNPI varchar(50) null,
ContactName varchar(50) null,
Email varchar(50) null,
Fax varchar(50) null,
Phone varchar(50) null,
PhoneEx varchar(50) null,
CONSTRAINT [PK_Sub_History.ProviderContacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

if object_id('Sub_History.ClaimHeaders') is not null drop table Sub_History.ClaimHeaders
go
create table Sub_History.ClaimHeaders
(
 ID bigint identity(1,1) not null,
FileID int not null,
ClaimID varchar(50) null,
ClaimAmount varchar(50) null,
ClaimPOS varchar(50) null,
ClaimType varchar(50) null,
ClaimFrequencyCode varchar(50) null,
ClaimProviderSignature varchar(50) null,
ClaimProviderAssignment varchar(50) null,
ClaimBenefitAssignment varchar(50) null,
ClaimReleaseofInformationCode varchar(50) null,
ClaimPatientSignatureSourceCode varchar(50) null,
ClaimRelatedCausesQualifier varchar(50) null,
ClaimRelatedCausesCode varchar(50) null,
ClaimRelatedStateCode varchar(50) null,
ClaimRelatedCountryCode varchar(50) null,
ClaimSpecialProgramCode varchar(50) null,
ClaimDelayReasonCode varchar(50) null,
CurrentIllnessDate varchar(50) null,
InitialTreatmentDate varchar(50) null,
LastSeenDate varchar(50) null,
AcuteManifestestationDate varchar(50) null,
AccidentDate varchar(50) null,
LastMenstrualPeriodDate varchar(50) null,
LastXrayDate varchar(50) null,
PrescriptionDate varchar(50) null,
DisabilityDate varchar(50) null,
DisabilityStartDate varchar(50) null,
DisabilityEndDate varchar(50) null,
LastWorkedDate varchar(50) null,
AuthorizedReturnToWorkDate varchar(50) null,
AdmissionDate varchar(50) null,
DischargeDate varchar(50) null,
AssumedStartDate varchar(50) null,
AssumedEndDate varchar(50) null,
FirstContactDate varchar(50) null,
RepricerReceivedDate varchar(50) null,
ContractTypeCode varchar(50) null,
ContractAmount varchar(50) null,
ContractPercentage varchar(50) null,
ContractCode varchar(50) null,
ContractTermsDiscountPercentage varchar(50) null,
ContractVersionIdentifier varchar(50) null,
PatientPaidAmount varchar(50) null,
ServiceAuthorizationExceptionCode varchar(50) null,
MedicareSection4081Indicator varchar(50) null,
MammographyCertificationNumber varchar(50) null,
ReferralNumber varchar(50) null,
PriorAuthorizationNumber varchar(50) null,
PayerClaimControlNumber varchar(50) null,
ClinicalLabNumber varchar(50) null,
RepricedClaimNumber varchar(50) null,
AdjustedClaimNumber varchar(50) null,
InvestigationalID varchar(50) null,
ValueAddedNetworkTraceNumber varchar(50) null,
MedicalRecordNumber varchar(50) null,
DemonstrationID varchar(50) null,
CarePlanOversightNumber varchar(50) null,
AmbulanceWeight varchar(50) null,
AmbulanceReasonCode varchar(50) null,
AmbulanceQuantity varchar(50) null,
AmbulanceRoundTripDescription varchar(50) null,
AmbulanceStretcherDescription varchar(50) null,
PatientConditionCode varchar(50) null,
PatientConditionDescription1 varchar(50) null,
PatientConditionDescription2 varchar(50) null,
PricingMethodology varchar(50) null,
RepricedAllowedAmount varchar(50) null,
RepricedSavingAmount varchar(50) null,
RepricingOrganizationID varchar(50) null,
RepricingRate varchar(50) null,
RepricedGroupCode varchar(50) null,
RepricedGroupAmount varchar(50) null,
RepricingRevenueCode varchar(50) null,
RepricingUnit varchar(50) null,
RepricingQuantity varchar(50) null,
RejectReasonCode varchar(50) null,
PolicyComplianceCode varchar(50) null,
ExceptionCode varchar(50) null,
StatementFromDate varchar(50) null,
StatementToDate varchar(50) null,
AdmissionTypeCode varchar(50) null,
AdmissionSourceCode varchar(50) null,
PatientStatusCode varchar(50) null,
PatientResponsibilityAmount varchar(50) null,
CONSTRAINT [PK_Sub_History.ClaimHeaders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

if object_id('Sub_History.ClaimPatients') is not null drop table Sub_History.ClaimPatients
go
create table Sub_History.ClaimPatients
(
ID bigint identity(1,1) not null,
FileID int not null,
ClaimID varchar(50) null,
PatientRelatedCode varchar(50) null,
PatientRelatedDeathDate varchar(50) null,
PatientRelatedUnit varchar(50) null,
PatientRelatedWeight varchar(50) null,
PatientRelatedPregnancyIndicator varchar(50) null,
PatientLastName varchar(50) null,
PatientFirstName varchar(50) null,
PatientMiddle varchar(50) null,
PatientSuffix varchar(50) null,
PatientAddress varchar(100) null,
PatientAddress2 varchar(50) null,
PatientCity varchar(50) null,
PatientState varchar(50) null,
PatientZip varchar(50) null,
PatientCountry varchar(50) null,
PatientCountrySubCode varchar(50) null,
PatientBirthDate varchar(50) null,
PatientGender varchar(50) null,
PatientClaimNumber varchar(50) null,
PatientSSN varchar(50) null,
PatientMemberID varchar(50) null,
PatientContactName varchar(50) null,
PatientContactPhone varchar(50) null,
PatientContactPhoneEx varchar(50) null,
CONSTRAINT [PK_Sub_History.ClaimPatients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go
    
if object_id('Sub_History.ClaimSecondaryIdentifications') is not null drop table Sub_History.ClaimSecondaryIdentifications
go
create table Sub_History.ClaimSecondaryIdentifications
(
 ID bigint identity(1,1) not null,
FileID int not null,
ClaimID varchar(50) null,
ServiceLineNumber varchar(50) null,
LoopName varchar(50) null,
ProviderQualifier varchar(50) null,
ProviderID varchar(50) null,
OtherPayerPrimaryIDentification varchar(50) null,
RepeatSequence varchar(50) null,
CONSTRAINT [PK_Sub_History.ClaimSecondaryIdentifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go
    
if object_id('Sub_History.ClaimPWKs') is not null drop table Sub_History.ClaimPWKs
go
create table Sub_History.ClaimPWKs
(
ID bigint identity(1,1) not null,
FileID int not null,
ClaimID varchar(50) null,
ServiceLineNumber varchar(50) null,
ReportTypeCode varchar(50) null,
ReportTransmissionCode varchar(50) null,
AttachmentControlNumber varchar(50) null,
CONSTRAINT [PK_Sub_History.ClaimPWKs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

if object_id('Sub_History.ClaimK3s') is not null drop table Sub_History.ClaimK3s
go
create table Sub_History.ClaimK3s
(
ID bigint identity(1,1) not null,
FileID int not null,
ClaimID varchar(50) null,
ServiceLineNumber varchar(50) null,
K3 varchar(50) null,
CONSTRAINT [PK_Sub_History.ClaimK3s] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

if object_id('Sub_History.ClaimNtes') is not null drop table Sub_History.ClaimNtes
go
create table Sub_History.ClaimNtes
(
ID bigint identity(1,1) not null,
FileID int not null,
ClaimID varchar(50) null,
ServiceLineNumber varchar(50) null,
NoteCode varchar(50) null,
NoteText varchar(200) null,
CONSTRAINT [PK_Sub_History.ClaimNtes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

if object_id('Sub_History.ClaimCRCs') is not null drop table Sub_History.ClaimCRCs
go
create table Sub_History.ClaimCRCs
(
ID bigint identity(1,1) not null,
FileID int not null,
ClaimID varchar(50) null,
ServiceLineNumber varchar(50) null,
CodeCategory varchar(50) null,
ConditionIndicator varchar(50) null,
ConditionCode varchar(50) null,
ConditionCode2 varchar(50) null,
ConditionCode3 varchar(50) null,
ConditionCode4 varchar(50) null,
ConditionCode5 varchar(50) null,
CONSTRAINT [PK_Sub_History.ClaimCRCs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go
    
if object_id('Sub_History.ClaimHIs') is not null drop table Sub_History.ClaimHIs
go
create table Sub_History.ClaimHIs
(
ID bigint identity(1,1) not null,
FileID int not null,
ClaimID varchar(50) null,
HIQual varchar(50) null,
HICode varchar(50) null,
PresentOnAdmissionIndicator varchar(50) null,
HIFromDate varchar(50) null,
HIToDate varchar(50) null,
HIAmount varchar(50) null,
CONSTRAINT [PK_Sub_History.ClaimHIs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

if object_id('Sub_History.ClaimSBRs') is not null drop table Sub_History.ClaimSBRs
go
create table Sub_History.ClaimSBRs
(
ID bigint identity(1,1) not null,
FileID int not null,
ClaimID varchar(50) null,
LoopName varchar(50) null,
SubscriberSequenceNumber varchar(50) null,
SubscriberRelationshipCode varchar(50) null,
InsuredGroupNumber varchar(50) null,
OtherInsuredGroupName varchar(50) null,
InsuredTypeCode varchar(50) null,
ClaimFilingCode varchar(50) null,
DeathDate varchar(50) null,
Unit varchar(50) null,
Weight varchar(50) null,
PregnancyIndicator varchar(50) null,
LastName varchar(50) null,
FirstName varchar(50) null,
MidddleName varchar(50) null,
NameSuffix varchar(50) null,
IDQualifier varchar(50) null,
IDCode varchar(50) null,
SubscriberAddress varchar(100) null,
SubscriberAddress2 varchar(50) null,
SubscriberCity varchar(50) null,
SubscriberState varchar(50) null,
SubscriberZip varchar(50) null,
SubscriberCountry varchar(50) null,
SubscriberCountrySubCode varchar(50) null,
SubscriberBirthDate varchar(50) null,
SubscriberGender varchar(50) null,
SubscriberSSN varchar(50) null,
SubscriberClaimNumber varchar(50) null,
SubscriberContactName varchar(50) null,
SubscriberContactPhone varchar(50) null,
SubscriberContactPhoneEx varchar(50) null,
COBPayerPaidAmount varchar(50) null,
COBNonCoveredAmount varchar(50) null,
COBRemainingPatientLiabilityAmount varchar(50) null,
BenefitsAssignmentCertificationIndicator varchar(50) null,
PatientSignatureSourceCode varchar(50) null,
ReleaseOfInformationCode varchar(50) null,
ReimbursementRate varchar(50) null,
HCPCSPayableAmount varchar(50) null,
MOA_ClaimPaymentRemarkCode1 varchar(50) null,
MOA_ClaimPaymentRemarkCode2 varchar(50) null,
MOA_ClaimPaymentRemarkCode3 varchar(50) null,
MOA_ClaimPaymentRemarkCode4 varchar(50) null,
MOA_ClaimPaymentRemarkCode5 varchar(50) null,
EndStageRenalDiseasePaymentAmount varchar(50) null,
MOA_NonPayableProfessionalComponentBilledAmount varchar(50) null,
CoveredDays varchar(50) null,
LifetimePsychiatricDays varchar(50) null,
ClaimDRGAmount varchar(50) null,
MIA_ClaimPaymentRemarkCode1 varchar(50) null,
ClaimDisproportionateShareAmount varchar(50) null,
ClaimMSPPassThroughAmount varchar(50) null,
ClaimPPSCapitalAmount varchar(50) null,
PPSCapitalFSPDRGAmount varchar(50) null,
PPSCapitalHSPDRGAmount varchar(50) null,
PPSCapitalDSHDRGAmount varchar(50) null,
OldCapitalAmount varchar(50) null,
PPSCapitalIMEAmount varchar(50) null,
PPSOperatingHospitalSpecificDRGAmount varchar(50) null,
CostReportDayCount varchar(50) null,
PPSOperatingFederalSpecificDRGAmount varchar(50) null,
CLaimPPSCapitalOutlierAmount varchar(50) null,
ClaimIndirectTeachingAmount varchar(50) null,
MIA_NonPayableProfessionalComponentBilledAmount varchar(50) null,
MIA_ClaimPaymentRemarkCode2 varchar(50) null,
MIA_ClaimPaymentRemarkCode3 varchar(50) null,
MIA_ClaimPaymentRemarkCode4 varchar(50) null,
MIA_ClaimPaymentRemarkCode5 varchar(50) null,
PPSCapitalExceptionAmount varchar(50) null,
PaymentDate varchar(50) null,
CONSTRAINT [PK_Sub_History.ClaimSBRs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

if object_id('Sub_History.ClaimCAS') is not null drop table Sub_History.ClaimCAS
go

create table Sub_History.ClaimCAS
(
ID bigint identity(1,1) not null,
FileID int not null,
ClaimID varchar(50) null,
ServiceLineNumber varchar(50) null,
SubscriberSequenceNumber varchar(50) null,
GroupCode varchar(50) null,
ReasonCode varchar(50) null,
AdjustmentAmount varchar(50) null,
AdjustmentQuantity varchar(50) null,
CONSTRAINT [PK_Sub_History.ClaimCAS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

if object_id('Sub_History.ClaimLineMEAs') is not null drop table Sub_History.ClaimLineMEAs
go

create table Sub_History.ClaimLineMEAs
(
ID bigint identity(1,1) not null,
FileID int not null,
ClaimID varchar(50) null,
ServiceLineNumber varchar(50) null,
TestCode varchar(50) null,
MeasurementQualifier varchar(50) null,
TestResult varchar(50) null,
CONSTRAINT [PK_Sub_History.ClaimLineMEAs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

if object_id('Sub_History.ClaimLineAuths') is not null drop table Sub_History.ClaimLineAuths
go

create table Sub_History.ClaimLineAuths
(
ID bigint identity(1,1) not null,
FileID int not null,
ClaimID varchar(50) null,
ServiceLineNumber varchar(50) null,
ReferenceQualifier varchar(50) null,
ReferralNumber varchar(50) null,
OtherPayerPrimaryIdentifier varchar(50) null,
CONSTRAINT [PK_Sub_History.ClaimLineAuths] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

if object_id('Sub_History.ClaimLineSVDs') is not null drop table Sub_History.ClaimLineSVDs
go

create table Sub_History.ClaimLineSVDs
(
ID bigint identity(1,1) not null,
FileID int not null,
ClaimID varchar(50) null,
ServiceLineNumber varchar(50) null,
RepeatSequence varchar(50) null,
OtherPayerPrimaryIdentifier varchar(50) null,
ServiceLinePaidAmount varchar(50) null,
ServiceQualifier varchar(50) null,
ProcedureCode varchar(50) null,
ProcedureModifier1 varchar(50) null,
ProcedureModifier2 varchar(50) null,
ProcedureModifier3 varchar(50) null,
ProcedureModifier4 varchar(50) null,
ProcedureDescription varchar(100) null,
PaidServiceUnitCount varchar(50) null,
BundledLineNumber varchar(50) null,
ServiceLineRevenueCode varchar(50) null,
CONSTRAINT [PK_Sub_History.ClaimLineSVDs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

if object_id('Sub_History.ClaimLineLQs') is not null drop table Sub_History.ClaimLineLQs
go

create table Sub_History.ClaimLineLQs
(
ID bigint identity(1,1) not null,
FileID int not null,
ClaimID varchar(50) null,
ServiceLineNumber varchar(50) null,
LQSequence varchar(50) null,
FormQualifier varchar(50) null,
IndustryCode varchar(50) null,
CONSTRAINT [PK_Sub_History.ClaimLineLQs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

if object_id('Sub_History.ClaimLineFRMs') is not null drop table Sub_History.ClaimLineFRMs
go

create table Sub_History.ClaimLineFRMs
(
ID bigint identity(1,1) not null,
FileID int not null,
ClaimID varchar(50) null,
ServiceLineNumber varchar(50) null,
LQSequence varchar(50) null,
QuestionNumber varchar(50) null,
QuestionResponseIndicator varchar(50) null,
QuestionResponse varchar(50) null,
QuestionResponseDate varchar(50) null,
AllowedChargePercentage varchar(50) null,
CONSTRAINT [PK_Sub_History.ClaimLineFRMs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

if object_id('Sub_History.ServiceLines') is not null drop table Sub_History.ServiceLines
go

create table Sub_History.ServiceLines
(
ID bigint identity(1,1) not null,
FileID int not null,
ClaimID varchar(50) null,
ServiceLineNumber varchar(50) null,
ServiceIDQualifier varchar(50) null,
ProcedureCode varchar(50) null,
ProcedureModifier1 varchar(50) null,
ProcedureModifier2 varchar(50) null,
ProcedureModifier3 varchar(50) null,
ProcedureModifier4 varchar(50) null,
ProcedureDescription varchar(100) null,
LineItemChargeAmount varchar(50) null,
LineItemUnit varchar(50) null,
ServiceUnitQuantity varchar(50) null,
LineItemPOS varchar(50) null,
DiagPointer1 varchar(50) null,
DiagPointer2 varchar(50) null,
DiagPointer3 varchar(50) null,
DiagPointer4 varchar(50) null,
EmergencyIndicator varchar(50) null,
EPSDTIndicator varchar(50) null,
FamilyPlanningIndicator varchar(50) null,
CopayStatusCode varchar(50) null,
DMEQualifier varchar(50) null,
DMEProcedureCode varchar(50) null,
DMEDays varchar(50) null,
DMERentalPrice varchar(50) null,
DMEPurchasePrice varchar(50) null,
DMEFrequencyCode varchar(50) null,
PatientWeight varchar(50) null,
AmbulanceTransportReasonCode varchar(50) null,
TransportDistance varchar(50) null,
RoundTripPurposeDescription varchar(50) null,
StretcherPueposeDescription varchar(50) null,
CertificationTypeCode varchar(50) null,
DMEDuration varchar(50) null,
ServiceFromDate varchar(50) null,
ServiceToDate varchar(50) null,
PrescriptionDate varchar(50) null,
CertificationDate varchar(50) null,
BeginTherapyDate varchar(50) null,
LastCertificationDate varchar(50) null,
LastSeenDate varchar(50) null,
TestDateHemo varchar(50) null,
TestDateSerum varchar(50) null,
ShippedDate varchar(50) null,
LastXrayDate varchar(50) null,
InitialTreatmentDate varchar(50) null,
AmbulancePatientCount varchar(50) null,
ObstetricAdditionalUnits varchar(50) null,
ContractTypeCode varchar(50) null,
ContractAmount varchar(50) null,
ContractPercentage varchar(50) null,
ContractCode varchar(50) null,
TermsDiscountPercentage varchar(50) null,
ContractVersionIdentifier varchar(50) null,
RepricedLineItemReferenceNumber varchar(50) null,
AdjustedRepricedLineItemReferenceNumber varchar(50) null,
LineItemControlNumber varchar(50) null,
MammographyCertificationNumber varchar(50) null,
ClinicalLabNumber varchar(50) null,
ReferringCLIANumber varchar(50) null,
ImmunizationBatchNumber varchar(50) null,
SalesTaxAmount varchar(50) null,
PostageClaimedAmount varchar(50) null,
PurchasedServiceProviderIdentifier varchar(50) null,
PurchasedServiceChargeAmount varchar(50) null,
PricingMethodology varchar(50) null,
RepricedAllowedAmount varchar(50) null,
RepricedSavingAmount varchar(50) null,
RepricingOrganizationIdentifier varchar(50) null,
RepricingRate varchar(50) null,
RepricedAmbulatoryPatientGroupCode varchar(50) null,
RepricedAmbulatoryPatientGroupAmount varchar(50) null,
HCPQualifier varchar(50) null,
RepricedHCPCSCode varchar(50) null,
RepricingUnit varchar(50) null,
RepricingQuantity varchar(50) null,
RejectReasonCode varchar(50) null,
PolicyComplianceCode varchar(50) null,
ExceptionCode varchar(50) null,
LINQualifier varchar(50) null,
NationalDrugCode varchar(50) null,
DrugQuantity varchar(50) null,
DrugQualifier varchar(50) null,
LinkSequenceNumber varchar(50) null,
PharmacyPrescriptionNumber varchar(50) null,
AdjudicationDate varchar(50) null,
ReaminingPatientLiabilityAmount varchar(50) null,
RevenueCode varchar(50) null,
LineItemDeniedChargeAmount varchar(50) null,
FacilityTaxAmount varchar(50) null,
CONSTRAINT [PK_Sub_History.ServiceLines] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

if object_id('Sub_History.SubmissionLogs') is not null drop table Sub_History.SubmissionLogs
go

create table Sub_History.SubmissionLogs
(
FileID int identity(1,1) not null,
FileName varchar(200) null,
FilePath varchar(200) null,
FileType varchar(50) null,
ReportType varchar(50) null,
EncounterCount int not null,
SubmitterID varchar(50) null,
ReceiverID varchar(50) null,
InterchangeControlNumber varchar(50) null,
ProductionFlag varchar(50) null,
InterchangeDate varchar(50) null,
InterchangeTime varchar(50) null,
BatchControlNumber varchar(50) null,
SubmitterLastName varchar(50) null,
SubmitterFirstName varchar(50) null,
SubmitterMiddle varchar(50) null,
SubmitterContactName1 varchar(50) null,
SubmitterContactEmail1 varchar(50) null,
SubmitterContactFax1 varchar(50) null,
SubmitterContactPhone1 varchar(50) null,
SubmitterContactPhoneEx1 varchar(50) null,
SubmitterContactName2 varchar(50) null,
SubmitterContactEmail2 varchar(50) null,
SubmitterContactFax2 varchar(50) null,
SubmitterContactPhone2 varchar(50) null,
SubmitterContactPhoneEx2 varchar(50) null,
ReceiverLastName varchar(50) null,
CreatedDate datetime not null
CONSTRAINT [PK_Sub_History.SubmissionLogs] PRIMARY KEY CLUSTERED 
(
	[FileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

