using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Config;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Extensions;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Interfaces;
using Microsoft.Xrm.Sdk;

namespace Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Builders
{
    public class AddPupilAmendmentBuilder : AmendmentBuilder
    {
        public AddPupilAmendmentBuilder(IOrganizationService organizationService, IOutcomeService outcomeService,
            IPupilService pupilService, DynamicsOptions dynamicsOptions, IAllocationYearConfig year) : base(
            organizationService, outcomeService, pupilService, dynamicsOptions, year)
        {
        }

        public override string RelationshipKey => "rscd_Amendment_Addpupil";

        public override AmendmentType AmendmentType => AmendmentType.AddPupil;

        public override Amendment CreateAmendment()
        {
            return new AddPupilAmendment();
        }

        public override AmendmentDetail CreateAmendmentDetails(CrmServiceContext context, rscd_Amendment amendment)
        {
            var addPupil = context.rscd_AddpupilSet.First(x => x.Id == amendment.rscd_Addpupil.Id);

            var amendmentDetails = new AmendmentDetail();
            amendmentDetails.AddField(AddPupilAmendment.FIELD_Reason, addPupil.rscd_Reason.Value.ToDomainAddReason());
            amendmentDetails.AddField(AddPupilAmendment.FIELD_PreviousSchoolLAEstab,
                addPupil.rscd_PreviousschoolLAESTAB);
            amendmentDetails.AddField(AddPupilAmendment.FIELD_PreviousSchoolURN, addPupil.rscd_PreviousschoolURN);
            amendmentDetails.AddField(AddPupilAmendment.FIELD_PriorAttainmentResults, new List<PriorAttainmentResult>
            {
                new PriorAttainmentResult
                {
                    Ks2Subject = Ks2Subject.Reading, ExamYear = addPupil.rscd_Readingexamyear,
                    Mark = addPupil.rscd_Readingexammark, ScaledScore = addPupil.rscd_Readingscaledscore
                },
                new PriorAttainmentResult
                {
                    Ks2Subject = Ks2Subject.Writing, ExamYear = addPupil.rscd_Writingexamyear,
                    Mark = addPupil.rscd_Writingteacherassessment,
                    ScaledScore = addPupil.rscd_Writingscaledscore
                },
                new PriorAttainmentResult
                {
                    Ks2Subject = Ks2Subject.Maths, ExamYear = addPupil.rscd_Mathsexamyear,
                    Mark = addPupil.rscd_Mathsexammark, ScaledScore = addPupil.rscd_Mathsscaledscore
                }
            });

            return amendmentDetails;
        }

        public override rscd_Amendmenttype CrmAmendmentType => rscd_Amendmenttype.Addapupil;

        protected override void MapAmendmentToDto(Amendment amendment, rscd_Amendment amendmentDto)
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

            amendmentDto.rscd_Evidencestatus = amendment.EvidenceStatus.ToCRMEvidenceStatus();
        }

        protected override Entity MapAmendmentTypeToDto(Amendment amendment)
        {
            var amendmentDetail = amendment.AmendmentDetail;
            var addDto = new rscd_Addpupil
            {
                rscd_Name = amendment.Pupil.FullName,
                rscd_Reason = amendmentDetail.GetField<string>(AddPupilAmendment.FIELD_Reason).ToCrmAddReason(),
                rscd_PreviousschoolURN = amendment.Pupil.URN,
                rscd_PreviousschoolLAESTAB = amendment.Pupil.DfesNumber
            };

            var results =
                amendmentDetail.GetList<PriorAttainmentResult>(AddPupilAmendment.FIELD_PriorAttainmentResults);

            var reading = results.FirstOrDefault(r => r.Ks2Subject == Ks2Subject.Reading);

            if (reading != null)
            {
                addDto.rscd_Readingexamyear = reading.ExamYear;
                addDto.rscd_Readingexammark = reading.Mark;
                addDto.rscd_Readingscaledscore = reading.ScaledScore;
            }

            var writing = results
                .FirstOrDefault(r => r.Ks2Subject == Ks2Subject.Writing);
            if (writing != null)
            {
                addDto.rscd_Writingexamyear = writing.ExamYear;
                addDto.rscd_Writingteacherassessment = writing.Mark;
                addDto.rscd_Writingscaledscore = writing.ScaledScore;
            }

            var maths = results
                .FirstOrDefault(r => r.Ks2Subject == Ks2Subject.Maths);
            if (maths != null)
            {
                addDto.rscd_Mathsexamyear = maths.ExamYear;
                addDto.rscd_Mathsexammark = maths.Mark;
                addDto.rscd_Mathsscaledscore = maths.ScaledScore;
            }

            return addDto;
        }
    }
}