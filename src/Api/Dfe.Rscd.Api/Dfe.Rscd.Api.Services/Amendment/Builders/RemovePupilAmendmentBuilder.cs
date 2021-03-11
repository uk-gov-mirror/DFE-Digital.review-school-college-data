using System;
using System.Linq;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Entities;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Amendments;
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
            if (amendment.rscd_Amendmenttype == rscd_Amendmenttype.Removeapupil)
            {
                if (amendment.rscd_Removepupil != null)
                {
                    var removePupil =
                        context.rscd_RemovepupilSet.FirstOrDefault(x => x.Id == amendment.rscd_Removepupil.Id);

                    return removePupil.ToAmendmentDetail();
                }
            }

            return new AmendmentDetail();
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
            amendmentDto.rscd_OutcomeReason = amendment.AmendmentDetail.GetField<string>(RemovePupilAmendment.FIELD_OutcomeDescription);
            
            amendmentDto.rscd_Evidencestatus = amendment.EvidenceStatus.ToCRMEvidenceStatus();
        }

        protected override Entity MapAmendmentTypeToDto(Amendment amendment)
        {
            var pupil = PupilService.GetById(amendment.CheckingWindow, amendment.Pupil.Id.ToString());
            amendment.Pupil = pupil;

            return amendment.AmendmentDetail.ToCrmRemovePupil(amendment);
        }
    }
}