using System.Linq;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Config;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Extensions;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Xrm.Sdk;

namespace Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Services
{
    public class RemovePupilAmendmentBuilder : AmendmentBuilder<RemovePupilAmendment>
    {
        public RemovePupilAmendmentBuilder(IOrganizationService organizationService, IOutcomeService outcomeService,
            IPupilService pupilService, IOptions<DynamicsOptions> dynamicsOptions, IConfiguration configuration) : base(
            organizationService, outcomeService, pupilService, dynamicsOptions, configuration)
        {
        }

        protected override void MapAmendmentToDto(RemovePupilAmendment amendment, rscd_Amendment amendmentDto)
        {
            amendmentDto.rscd_UPN = amendment.Pupil.UPN;
            amendmentDto.rscd_ULN = amendment.Pupil.ULN;
            amendmentDto.rscd_Name = amendment.Pupil.FullName;
            amendmentDto.rscd_Firstname = amendment.Pupil.ForeName;
            amendmentDto.rscd_Lastname = amendment.Pupil.LastName;
            amendmentDto.rscd_Gender = amendment.Pupil.Gender.ToCRMGenderType();
            amendmentDto.rscd_Dateofbirth = amendment.Pupil.DateOfBirth;
            amendmentDto.rscd_Age = amendment.Pupil.Age;

            if (Ks4Windows.Any(w => w == amendmentDto.rscd_Checkingwindow))
            {
                amendmentDto.rscd_Dateofadmission = amendment.Pupil.DateOfAdmission;
                amendmentDto.rscd_Yeargroup = amendment.Pupil.YearGroup;
            }

            amendmentDto.rscd_rm_scrutiny_reason_code = amendment.ScrutinyReasonCode;
            amendmentDto.rscd_rm_amdflag = amendment.AmdFlag;

            amendmentDto.rscd_Evidencestatus = amendment.EvidenceStatus.ToCRMEvidenceStatus();
        }

        protected override Entity MapAmendmentTypeToDto(RemovePupilAmendment amendment)
        {
            var pupil = PupilService.GetById(amendment.CheckingWindow, amendment.Pupil.Id);
            amendment.Pupil = pupil;
            var removeDto = new rscd_Removepupil
            {
                rscd_Name = amendment.Pupil.FullName
            };
            var removeDetail = amendment.AmendmentDetail;

            removeDto.rscd_reasoncode = removeDetail.ReasonCode;
            removeDto.rscd_Subreason = removeDetail.SubReason;
            removeDto.rscd_Details = removeDetail.Detail;

            if (pupil.Allocations != null)
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

        protected override string RelationshipKey => "rscd_Amendment_Removepupil";
        public override AmendmentType AmendmentType => AmendmentType.RemovePupil;
    }
}