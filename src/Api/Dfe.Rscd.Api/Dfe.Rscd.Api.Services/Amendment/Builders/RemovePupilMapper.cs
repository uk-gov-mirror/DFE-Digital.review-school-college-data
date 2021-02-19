using System.Linq;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
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
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_OutcomeDescription, crmPupil.rscd_Subreason);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_ReasonDescription, crmPupil.rscd_reasondescription);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_CountryOfOrigin, crmPupil.rscd_Countryoforigin);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOfArrivalUk, crmPupil.rscd_Dateofarrival.HasValue ? crmPupil.rscd_Dateofarrival.Value.ToShortDateString() : string.Empty);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_NativeLanguage, crmPupil.rscd_Language);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_LAESTABNumber, crmPupil.rscd_LAESTABofexcludedschool);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_ExclusionDate, crmPupil.rscd_Pupilexclusiondate);
            amendmentDetail.SetField(RemovePupilAmendment.FIELD_DateOffRoll, crmPupil.rscd_Dateoffroll);

            return amendmentDetail;
        }

        public static rscd_Removepupil ToCrmRemovePupil(this AmendmentDetail amendmentDetail, Amendment amendment)
        {
            var removeDto = new rscd_Removepupil();
            var pupil = amendment.Pupil;
            
            removeDto.rscd_reasoncode = amendmentDetail.GetField<int?>(RemovePupilAmendment.FIELD_ReasonCode);
            removeDto.rscd_Subreason = amendmentDetail.GetField<string>(RemovePupilAmendment.FIELD_ReasonDescription);
            removeDto.rscd_Countryoforigin = amendmentDetail.GetField<string>(RemovePupilAmendment.FIELD_CountryOfOrigin);
            removeDto.rscd_Language = amendmentDetail.GetField<string>(RemovePupilAmendment.FIELD_NativeLanguage);
            removeDto.rscd_Dateofarrival = amendmentDetail.GetDateTime(RemovePupilAmendment.FIELD_DateOfArrivalUk);
            removeDto.rscd_LAESTABofexcludedschool = amendmentDetail.GetField<string>(RemovePupilAmendment.FIELD_LAESTABNumber);
            removeDto.rscd_Pupilexclusiondate = amendmentDetail.GetDateTime(RemovePupilAmendment.FIELD_ExclusionDate);
            removeDto.rscd_Dateoffroll = amendmentDetail.GetDateTime(RemovePupilAmendment.FIELD_DateOffRoll);

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
