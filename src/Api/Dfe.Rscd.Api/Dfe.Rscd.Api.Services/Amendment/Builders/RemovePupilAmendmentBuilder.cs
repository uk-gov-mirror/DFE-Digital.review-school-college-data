using System;
using System.Linq;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Config;
using Microsoft.Xrm.Sdk;

namespace Dfe.Rscd.Api.Services
{
    public class RemovePupilAmendmentBuilder : AmendmentBuilder
    {
        public RemovePupilAmendmentBuilder(IOrganizationService organizationService, IOutcomeService outcomeService,
            IPupilService pupilService, DynamicsOptions dynamicsOptions, IAllocationYearConfig year) : base(
            organizationService, outcomeService, pupilService, dynamicsOptions, year)
        {
        }

        public override string RelationshipKey => "rscd_Amendment_Removepupil";

        public override rscd_Amendmenttype CrmAmendmentType => rscd_Amendmenttype.Removeapupil;

        public override Amendment CreateAmendment()
        {
            return new RemovePupilAmendment();
        }

        public override AmendmentDetail CreateAmendmentDetails(CrmServiceContext context, rscd_Amendment amendment)
        {
            var amendmentDetail = new AmendmentDetail();

            if (amendment.rscd_Amendmenttype == rscd_Amendmenttype.Removeapupil)
            {
                if (amendment.rscd_Removepupil != null)
                {
                    var removePupil =
                        context.rscd_RemovepupilSet.FirstOrDefault(x => x.Id == amendment.rscd_Removepupil.Id);

                    amendmentDetail.AddField(RemovePupilAmendment.FIELD_ReasonCode, removePupil.rscd_reasoncode.Value);
                    amendmentDetail.AddField(RemovePupilAmendment.FIELD_OutcomeDescription, removePupil.rscd_Subreason);
                    amendmentDetail.AddField(RemovePupilAmendment.FIELD_ReasonDescription, removePupil.rscd_reasondescription);
                    amendmentDetail.AddField(RemovePupilAmendment.FIELD_CountryOfOrigin, removePupil.rscd_Countryoforigin);
                    amendmentDetail.AddField(RemovePupilAmendment.FIELD_DateOfArrivalUk, removePupil.rscd_Dateofarrival.HasValue ? removePupil.rscd_Dateofarrival.Value.ToShortDateString() : string.Empty);
                    amendmentDetail.AddField(RemovePupilAmendment.FIELD_NativeLanguage, removePupil.rscd_Language);

                    return amendmentDetail;
                }
            }

            amendmentDetail = new AmendmentDetail();
            amendmentDetail.AddField(RemovePupilAmendment.FIELD_ReasonCode, default(int?));
            amendmentDetail.AddField(RemovePupilAmendment.FIELD_OutcomeDescription, string.Empty);
            amendmentDetail.AddField(RemovePupilAmendment.FIELD_CountryOfOrigin, string.Empty);
            amendmentDetail.AddField(RemovePupilAmendment.FIELD_DateOfArrivalUk, string.Empty);
            amendmentDetail.AddField(RemovePupilAmendment.FIELD_NativeLanguage, string.Empty);

            return amendmentDetail;

        }

        public override AmendmentType AmendmentType => AmendmentType.RemovePupil;

        protected override void MapAmendmentToDto(Amendment amendment, rscd_Amendment amendmentDto)
        {
            amendmentDto.rscd_UPN = amendment.Pupil.UPN;
            amendmentDto.rscd_ULN = amendment.Pupil.ULN;
            amendmentDto.rscd_Name = amendment.Pupil.FullName;
            amendmentDto.rscd_Firstname = amendment.Pupil.Forename;
            amendmentDto.rscd_Lastname = amendment.Pupil.Surname;
            amendmentDto.rscd_Gender = amendment.Pupil.Gender.ToCRMGenderType();
            amendmentDto.rscd_Dateofbirth = amendment.Pupil.DOB;
            amendmentDto.rscd_Age = amendment.Pupil.Age;

            if (Ks4Windows.Any(w => w == amendmentDto.rscd_Checkingwindow))
            {
                amendmentDto.rscd_Dateofadmission = amendment.Pupil.AdmissionDate;
                amendmentDto.rscd_Yeargroup = amendment.Pupil.YearGroup;
            }

            amendmentDto.rscd_rm_scrutiny_reason_code = amendment.AmendmentDetail.GetField<int?>(RemovePupilAmendment.FIELD_ReasonCode);
            amendmentDto.rscd_subreason = amendment.AmendmentDetail.GetField<string>(RemovePupilAmendment.FIELD_OutcomeDescription);
            
            amendmentDto.rscd_Evidencestatus = amendment.EvidenceStatus.ToCRMEvidenceStatus();
        }

        protected override Entity MapAmendmentTypeToDto(Amendment amendment)
        {
            var pupil = PupilService.GetById(amendment.CheckingWindow, amendment.Pupil.Id.ToString());
            amendment.Pupil = pupil;
            var removeDto = new rscd_Removepupil
            {
                rscd_Name = amendment.Pupil.FullName
            };
            var removeDetail = amendment.AmendmentDetail;

            removeDto.rscd_reasoncode = removeDetail.GetField<int?>(RemovePupilAmendment.FIELD_ReasonCode);
            removeDto.rscd_Subreason = removeDetail.GetField<string>(RemovePupilAmendment.FIELD_ReasonDescription);
            removeDto.rscd_Countryoforigin = removeDetail.GetField<string>(RemovePupilAmendment.FIELD_CountryOfOrigin);
            removeDto.rscd_Language = removeDetail.GetField<string>(RemovePupilAmendment.FIELD_NativeLanguage);
            removeDto.rscd_Dateofarrival = removeDetail.GetDateTime(RemovePupilAmendment.FIELD_DateOfArrivalUk);

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

        private string GenerateAllocationYearDescription(int? allocationYear)
        {
            if (allocationYear != null)
                // generate "YYYY/YY" representation for helpdesk UI
                return $"{allocationYear - 1}/{allocationYear.ToString().Remove(0, 2)}";

            return string.Empty;
        }
    }
}