using System.Linq;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Entities;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Amendments;

namespace Dfe.Rscd.Api.Services
{
    public static class RemovePupilMapper
    {
        public static AmendmentDetail ToAmendmentDetail(this rscd_Removepupil crmPupil)
        {
            var amendmentDetail = new AmendmentDetail();
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_ReasonCode, crmPupil.rscd_reasoncode.Value);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_ReasonDescription, crmPupil.rscd_ReasonDescription);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_SubReasonDescription, crmPupil.rscd_Subreason);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_CountryOfOrigin, crmPupil.rscd_Countryoforigin);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOfArrivalUk, crmPupil.rscd_DateOfArrival);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_NativeLanguage, crmPupil.rscd_Language);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_LAESTABNumber, crmPupil.rscd_LAESTABofexcludedschool);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_PreviousLAESTABNumber, crmPupil.rscd_PupilsmainschoolLAESTAB);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_ExclusionDate, crmPupil.rscd_PupilExclusionDate);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOnRoll, crmPupil.rscd_Dateonroll);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOffRoll, crmPupil.rscd_DateOffRoll);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_Detail, crmPupil.rscd_Details);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_CountryLeftEnglandFor, crmPupil.rscd_CountrypupilleftEnglandfor);

            return amendmentDetail;
        }

        public static rscd_Removepupil ToCrmRemovePupil(this AmendmentDetail amendmentDetail, Amendment amendment)
        {
            var removeDto = new rscd_Removepupil();
            var pupil = amendment.Pupil;
            
            removeDto.rscd_reasoncode = amendmentDetail.GetField<int?>(RemovePupilAmendment.FIELD_ReasonCode);
            removeDto.rscd_ReasonDescription = amendmentDetail.GetField<string>(RemovePupilAmendment.FIELD_ReasonDescription);
            removeDto.rscd_Subreason = amendmentDetail.GetField<string>(RemovePupilAmendment.FIELD_SubReasonDescription);
            removeDto.rscd_Countryoforigin = amendmentDetail.GetField<string>(RemovePupilAmendment.FIELD_CountryOfOrigin);
            removeDto.rscd_Language = amendmentDetail.GetField<string>(RemovePupilAmendment.FIELD_NativeLanguage);
            removeDto.rscd_DateOfArrival = amendmentDetail.GetDateTimeUTC(RemovePupilAmendment.FIELD_DateOfArrivalUk);
            removeDto.rscd_LAESTABofexcludedschool = amendmentDetail.GetField<string>(RemovePupilAmendment.FIELD_LAESTABNumber);
            removeDto.rscd_PupilsmainschoolLAESTAB = amendmentDetail.GetField<string>(RemovePupilAmendment.FIELD_PreviousLAESTABNumber);
            removeDto.rscd_PupilExclusionDate = amendmentDetail.GetDateTimeUTC(RemovePupilAmendment.FIELD_ExclusionDate);
            removeDto.rscd_DateOffRoll = amendmentDetail.GetDateTimeUTC(RemovePupilAmendment.FIELD_DateOffRoll);
            removeDto.rscd_Dateonroll = amendmentDetail.GetDateTimeUTC(RemovePupilAmendment.FIELD_DateOnRoll);
            removeDto.rscd_Details = amendmentDetail.GetField<string>(RemovePupilAmendment.FIELD_Detail);
            removeDto.rscd_CountrypupilleftEnglandfor = amendmentDetail.GetField<string>(RemovePupilAmendment.FIELD_CountryLeftEnglandFor);

            if (pupil.Allocations != null && pupil.Allocations.Count > 0)
            {
                removeDto.rscd_allocationyear = pupil.Allocations.Select(x => x.Year).FirstOrDefault();
                removeDto.rscd_allocationyeardescription =
                    GenerateAllocationYearDescription(removeDto.rscd_allocationyear);

                if (pupil.Allocations.Count > 1)
                {
                    removeDto.rscd_allocationyear_1 =
                        pupil.Allocations.Select(x => x.Year).Skip(1).FirstOrDefault();
                    removeDto.rscd_allocationyear_1description =
                        GenerateAllocationYearDescription(removeDto.rscd_allocationyear_1);
                }

                if (pupil.Allocations.Count > 2)
                {
                    removeDto.rscd_allocationyear_2 =
                        pupil.Allocations.Select(x => x.Year).Skip(2).FirstOrDefault();
                    removeDto.rscd_allocationyear_2description =
                        GenerateAllocationYearDescription(removeDto.rscd_allocationyear_2);
                }
            }

            return removeDto;
        }

        private static string GenerateAllocationYearDescription(int? allocationYear)
        {
            if (allocationYear != null)
                // generate "YYYY/YY" representation for helpdesk UI
                return $"{allocationYear - 1}/{allocationYear.ToString().Remove(0, 2)}";

            return string.Empty;
        }
    }
}
