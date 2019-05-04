using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportModule.Model.Interfaces;

namespace ExportModule.Model.X12Segments
{
    public class MIA : X12SegmentBase
    {
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
        public MIA()
        {
            SegmentCode = "MIA";
            LoopName = "2320";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(CoveredDays);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("MEA*" + CoveredDays);
            if (!string.IsNullOrEmpty(PPSCapitalExceptionAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount)) sb.Append(ClaimDisproportionateShareAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimMSPPassThroughAmount)) sb.Append(ClaimMSPPassThroughAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalAmount)) sb.Append(ClaimPPSCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalFSPDRGAmount)) sb.Append(PPSCapitalFSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalHSPDRGAmount)) sb.Append(PPSCapitalHSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalDSHDRGAmount)) sb.Append(PPSCapitalDSHDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(OldCapitalAmount)) sb.Append(OldCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalIMEAmount)) sb.Append(PPSCapitalIMEAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSOperatingHospitalSpecificDRGAmount)) sb.Append(PPSOperatingHospitalSpecificDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(CostReportDayCount)) sb.Append(CostReportDayCount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSOperatingFederalSpecificDRGAmount)) sb.Append(PPSOperatingFederalSpecificDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalOutlierAmount)) sb.Append(ClaimPPSCapitalOutlierAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimIndirectTeachingAmount)) sb.Append(ClaimIndirectTeachingAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_NonPayableProfessionalComponentBilledAmount)) sb.Append(MIA_NonPayableProfessionalComponentBilledAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode2)) sb.Append(MIA_ClaimPaymentRemarkCode2);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode3)) sb.Append(MIA_ClaimPaymentRemarkCode3);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode4)) sb.Append(MIA_ClaimPaymentRemarkCode4);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode5)) sb.Append(MIA_ClaimPaymentRemarkCode5);
                sb.Append("*" + PPSCapitalExceptionAmount);
            }
            else if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode5))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount)) sb.Append(ClaimDisproportionateShareAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimMSPPassThroughAmount)) sb.Append(ClaimMSPPassThroughAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalAmount)) sb.Append(ClaimPPSCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalFSPDRGAmount)) sb.Append(PPSCapitalFSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalHSPDRGAmount)) sb.Append(PPSCapitalHSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalDSHDRGAmount)) sb.Append(PPSCapitalDSHDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(OldCapitalAmount)) sb.Append(OldCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalIMEAmount)) sb.Append(PPSCapitalIMEAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSOperatingHospitalSpecificDRGAmount)) sb.Append(PPSOperatingHospitalSpecificDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(CostReportDayCount)) sb.Append(CostReportDayCount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSOperatingFederalSpecificDRGAmount)) sb.Append(PPSOperatingFederalSpecificDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalOutlierAmount)) sb.Append(ClaimPPSCapitalOutlierAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimIndirectTeachingAmount)) sb.Append(ClaimIndirectTeachingAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_NonPayableProfessionalComponentBilledAmount)) sb.Append(MIA_NonPayableProfessionalComponentBilledAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode2)) sb.Append(MIA_ClaimPaymentRemarkCode2);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode3)) sb.Append(MIA_ClaimPaymentRemarkCode3);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode4)) sb.Append(MIA_ClaimPaymentRemarkCode4);
                sb.Append("*" + MIA_ClaimPaymentRemarkCode5);
            }
            else if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode4))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount)) sb.Append(ClaimDisproportionateShareAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimMSPPassThroughAmount)) sb.Append(ClaimMSPPassThroughAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalAmount)) sb.Append(ClaimPPSCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalFSPDRGAmount)) sb.Append(PPSCapitalFSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalHSPDRGAmount)) sb.Append(PPSCapitalHSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalDSHDRGAmount)) sb.Append(PPSCapitalDSHDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(OldCapitalAmount)) sb.Append(OldCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalIMEAmount)) sb.Append(PPSCapitalIMEAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSOperatingHospitalSpecificDRGAmount)) sb.Append(PPSOperatingHospitalSpecificDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(CostReportDayCount)) sb.Append(CostReportDayCount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSOperatingFederalSpecificDRGAmount)) sb.Append(PPSOperatingFederalSpecificDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalOutlierAmount)) sb.Append(ClaimPPSCapitalOutlierAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimIndirectTeachingAmount)) sb.Append(ClaimIndirectTeachingAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_NonPayableProfessionalComponentBilledAmount)) sb.Append(MIA_NonPayableProfessionalComponentBilledAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode2)) sb.Append(MIA_ClaimPaymentRemarkCode2);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode3)) sb.Append(MIA_ClaimPaymentRemarkCode3);
                sb.Append("*" + MIA_ClaimPaymentRemarkCode4);
            }
            else if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode3))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount)) sb.Append(ClaimDisproportionateShareAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimMSPPassThroughAmount)) sb.Append(ClaimMSPPassThroughAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalAmount)) sb.Append(ClaimPPSCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalFSPDRGAmount)) sb.Append(PPSCapitalFSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalHSPDRGAmount)) sb.Append(PPSCapitalHSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalDSHDRGAmount)) sb.Append(PPSCapitalDSHDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(OldCapitalAmount)) sb.Append(OldCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalIMEAmount)) sb.Append(PPSCapitalIMEAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSOperatingHospitalSpecificDRGAmount)) sb.Append(PPSOperatingHospitalSpecificDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(CostReportDayCount)) sb.Append(CostReportDayCount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSOperatingFederalSpecificDRGAmount)) sb.Append(PPSOperatingFederalSpecificDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalOutlierAmount)) sb.Append(ClaimPPSCapitalOutlierAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimIndirectTeachingAmount)) sb.Append(ClaimIndirectTeachingAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_NonPayableProfessionalComponentBilledAmount)) sb.Append(MIA_NonPayableProfessionalComponentBilledAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode2)) sb.Append(MIA_ClaimPaymentRemarkCode2);
                sb.Append("*" + MIA_ClaimPaymentRemarkCode3);
            }
            else if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode2))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount)) sb.Append(ClaimDisproportionateShareAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimMSPPassThroughAmount)) sb.Append(ClaimMSPPassThroughAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalAmount)) sb.Append(ClaimPPSCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalFSPDRGAmount)) sb.Append(PPSCapitalFSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalHSPDRGAmount)) sb.Append(PPSCapitalHSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalDSHDRGAmount)) sb.Append(PPSCapitalDSHDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(OldCapitalAmount)) sb.Append(OldCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalIMEAmount)) sb.Append(PPSCapitalIMEAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSOperatingHospitalSpecificDRGAmount)) sb.Append(PPSOperatingHospitalSpecificDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(CostReportDayCount)) sb.Append(CostReportDayCount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSOperatingFederalSpecificDRGAmount)) sb.Append(PPSOperatingFederalSpecificDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalOutlierAmount)) sb.Append(ClaimPPSCapitalOutlierAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimIndirectTeachingAmount)) sb.Append(ClaimIndirectTeachingAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_NonPayableProfessionalComponentBilledAmount)) sb.Append(MIA_NonPayableProfessionalComponentBilledAmount);
                sb.Append("*" + MIA_ClaimPaymentRemarkCode2);
            }
            else if (!string.IsNullOrEmpty(MIA_NonPayableProfessionalComponentBilledAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount)) sb.Append(ClaimDisproportionateShareAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimMSPPassThroughAmount)) sb.Append(ClaimMSPPassThroughAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalAmount)) sb.Append(ClaimPPSCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalFSPDRGAmount)) sb.Append(PPSCapitalFSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalHSPDRGAmount)) sb.Append(PPSCapitalHSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalDSHDRGAmount)) sb.Append(PPSCapitalDSHDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(OldCapitalAmount)) sb.Append(OldCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalIMEAmount)) sb.Append(PPSCapitalIMEAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSOperatingHospitalSpecificDRGAmount)) sb.Append(PPSOperatingHospitalSpecificDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(CostReportDayCount)) sb.Append(CostReportDayCount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSOperatingFederalSpecificDRGAmount)) sb.Append(PPSOperatingFederalSpecificDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalOutlierAmount)) sb.Append(ClaimPPSCapitalOutlierAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimIndirectTeachingAmount)) sb.Append(ClaimIndirectTeachingAmount);
                sb.Append("*" + MIA_NonPayableProfessionalComponentBilledAmount);
            }
            else if (!string.IsNullOrEmpty(ClaimIndirectTeachingAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount)) sb.Append(ClaimDisproportionateShareAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimMSPPassThroughAmount)) sb.Append(ClaimMSPPassThroughAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalAmount)) sb.Append(ClaimPPSCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalFSPDRGAmount)) sb.Append(PPSCapitalFSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalHSPDRGAmount)) sb.Append(PPSCapitalHSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalDSHDRGAmount)) sb.Append(PPSCapitalDSHDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(OldCapitalAmount)) sb.Append(OldCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalIMEAmount)) sb.Append(PPSCapitalIMEAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSOperatingHospitalSpecificDRGAmount)) sb.Append(PPSOperatingHospitalSpecificDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(CostReportDayCount)) sb.Append(CostReportDayCount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSOperatingFederalSpecificDRGAmount)) sb.Append(PPSOperatingFederalSpecificDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalOutlierAmount)) sb.Append(ClaimPPSCapitalOutlierAmount);
                sb.Append("*" + ClaimIndirectTeachingAmount);
            }
            else if (!string.IsNullOrEmpty(ClaimPPSCapitalOutlierAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount)) sb.Append(ClaimDisproportionateShareAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimMSPPassThroughAmount)) sb.Append(ClaimMSPPassThroughAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalAmount)) sb.Append(ClaimPPSCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalFSPDRGAmount)) sb.Append(PPSCapitalFSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalHSPDRGAmount)) sb.Append(PPSCapitalHSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalDSHDRGAmount)) sb.Append(PPSCapitalDSHDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(OldCapitalAmount)) sb.Append(OldCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalIMEAmount)) sb.Append(PPSCapitalIMEAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSOperatingHospitalSpecificDRGAmount)) sb.Append(PPSOperatingHospitalSpecificDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(CostReportDayCount)) sb.Append(CostReportDayCount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSOperatingFederalSpecificDRGAmount)) sb.Append(PPSOperatingFederalSpecificDRGAmount);
                sb.Append("*" + ClaimPPSCapitalOutlierAmount);
            }
            else if (!string.IsNullOrEmpty(PPSOperatingFederalSpecificDRGAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount)) sb.Append(ClaimDisproportionateShareAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimMSPPassThroughAmount)) sb.Append(ClaimMSPPassThroughAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalAmount)) sb.Append(ClaimPPSCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalFSPDRGAmount)) sb.Append(PPSCapitalFSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalHSPDRGAmount)) sb.Append(PPSCapitalHSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalDSHDRGAmount)) sb.Append(PPSCapitalDSHDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(OldCapitalAmount)) sb.Append(OldCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalIMEAmount)) sb.Append(PPSCapitalIMEAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSOperatingHospitalSpecificDRGAmount)) sb.Append(PPSOperatingHospitalSpecificDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(CostReportDayCount)) sb.Append(CostReportDayCount);
                sb.Append("*" + PPSOperatingFederalSpecificDRGAmount);
            }
            else if (!string.IsNullOrEmpty(CostReportDayCount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount)) sb.Append(ClaimDisproportionateShareAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimMSPPassThroughAmount)) sb.Append(ClaimMSPPassThroughAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalAmount)) sb.Append(ClaimPPSCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalFSPDRGAmount)) sb.Append(PPSCapitalFSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalHSPDRGAmount)) sb.Append(PPSCapitalHSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalDSHDRGAmount)) sb.Append(PPSCapitalDSHDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(OldCapitalAmount)) sb.Append(OldCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalIMEAmount)) sb.Append(PPSCapitalIMEAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSOperatingHospitalSpecificDRGAmount)) sb.Append(PPSOperatingHospitalSpecificDRGAmount);
                sb.Append("*" + CostReportDayCount);
            }
            else if (!string.IsNullOrEmpty(PPSOperatingHospitalSpecificDRGAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount)) sb.Append(ClaimDisproportionateShareAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimMSPPassThroughAmount)) sb.Append(ClaimMSPPassThroughAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalAmount)) sb.Append(ClaimPPSCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalFSPDRGAmount)) sb.Append(PPSCapitalFSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalHSPDRGAmount)) sb.Append(PPSCapitalHSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalDSHDRGAmount)) sb.Append(PPSCapitalDSHDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(OldCapitalAmount)) sb.Append(OldCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalIMEAmount)) sb.Append(PPSCapitalIMEAmount);
                sb.Append("*" + PPSOperatingHospitalSpecificDRGAmount);
            }
            else if (!string.IsNullOrEmpty(PPSCapitalIMEAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount)) sb.Append(ClaimDisproportionateShareAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimMSPPassThroughAmount)) sb.Append(ClaimMSPPassThroughAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalAmount)) sb.Append(ClaimPPSCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalFSPDRGAmount)) sb.Append(PPSCapitalFSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalHSPDRGAmount)) sb.Append(PPSCapitalHSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalDSHDRGAmount)) sb.Append(PPSCapitalDSHDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(OldCapitalAmount)) sb.Append(OldCapitalAmount);
                sb.Append("*" + PPSCapitalIMEAmount);
            }
            else if (!string.IsNullOrEmpty(OldCapitalAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount)) sb.Append(ClaimDisproportionateShareAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimMSPPassThroughAmount)) sb.Append(ClaimMSPPassThroughAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalAmount)) sb.Append(ClaimPPSCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalFSPDRGAmount)) sb.Append(PPSCapitalFSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalHSPDRGAmount)) sb.Append(PPSCapitalHSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalDSHDRGAmount)) sb.Append(PPSCapitalDSHDRGAmount);
                sb.Append("*" + OldCapitalAmount);
            }
            else if (!string.IsNullOrEmpty(PPSCapitalDSHDRGAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount)) sb.Append(ClaimDisproportionateShareAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimMSPPassThroughAmount)) sb.Append(ClaimMSPPassThroughAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalAmount)) sb.Append(ClaimPPSCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalFSPDRGAmount)) sb.Append(PPSCapitalFSPDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalHSPDRGAmount)) sb.Append(PPSCapitalHSPDRGAmount);
                sb.Append("*" + PPSCapitalDSHDRGAmount);
            }
            else if (!string.IsNullOrEmpty(PPSCapitalHSPDRGAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount)) sb.Append(ClaimDisproportionateShareAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimMSPPassThroughAmount)) sb.Append(ClaimMSPPassThroughAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalAmount)) sb.Append(ClaimPPSCapitalAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(PPSCapitalFSPDRGAmount)) sb.Append(PPSCapitalFSPDRGAmount);
                sb.Append("*" + PPSCapitalHSPDRGAmount);
            }
            else if (!string.IsNullOrEmpty(PPSCapitalFSPDRGAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount)) sb.Append(ClaimDisproportionateShareAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimMSPPassThroughAmount)) sb.Append(ClaimMSPPassThroughAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimPPSCapitalAmount)) sb.Append(ClaimPPSCapitalAmount);
                sb.Append("*" + PPSCapitalFSPDRGAmount);
            }
            else if (!string.IsNullOrEmpty(ClaimPPSCapitalAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount)) sb.Append(ClaimDisproportionateShareAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimMSPPassThroughAmount)) sb.Append(ClaimMSPPassThroughAmount);
                sb.Append("*" + ClaimPPSCapitalAmount);
            }
            else if (!string.IsNullOrEmpty(ClaimMSPPassThroughAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount)) sb.Append(ClaimDisproportionateShareAmount);
                sb.Append("*" + ClaimMSPPassThroughAmount);
            }
            else if (!string.IsNullOrEmpty(ClaimDisproportionateShareAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*");
                if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1)) sb.Append(MIA_ClaimPaymentRemarkCode1);
                sb.Append("*" + ClaimDisproportionateShareAmount);
            }
            else if (!string.IsNullOrEmpty(MIA_ClaimPaymentRemarkCode1))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*");
                if (!string.IsNullOrEmpty(ClaimDRGAmount)) sb.Append(ClaimDRGAmount);
                sb.Append("*" + MIA_ClaimPaymentRemarkCode1);
            }
            else if (!string.IsNullOrEmpty(ClaimDRGAmount))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(LifetimePsychiatricDays)) sb.Append(LifetimePsychiatricDays);
                sb.Append("*" + ClaimDRGAmount);
            }
            else if (!string.IsNullOrEmpty(LifetimePsychiatricDays))
            {
                sb.Append("*" + LifetimePsychiatricDays);
            }
            sb.Append("~");
            return sb.ToString();
        }
    }
}
