using System.Collections.Generic;
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

namespace Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Builders
{
    public class AddPupilAmendmentBuilder : AmendmentBuilder
    {
        public AddPupilAmendmentBuilder(IOrganizationService organizationService, IOutcomeService outcomeService,
            IPupilService pupilService, IOptions<DynamicsOptions> dynamicsOptions, IConfiguration configuration) : base(
            organizationService, outcomeService, pupilService, dynamicsOptions, configuration)
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
            amendmentDetails.AddField(AddPupilAmendment.FIELD_PriorAttainmentResults, new List<PriorAttainment>
            {
                new PriorAttainment
                {
                    Subject = Ks2Subject.Reading, ExamYear = addPupil.rscd_Readingexamyear,
                    TestMark = addPupil.rscd_Readingexammark, ScaledScore = addPupil.rscd_Readingscaledscore
                },
                new PriorAttainment
                {
                    Subject = Ks2Subject.Writing, ExamYear = addPupil.rscd_Writingexamyear,
                    TestMark = addPupil.rscd_Writingteacherassessment,
                    ScaledScore = addPupil.rscd_Writingscaledscore
                },
                new PriorAttainment
                {
                    Subject = Ks2Subject.Maths, ExamYear = addPupil.rscd_Mathsexamyear,
                    TestMark = addPupil.rscd_Mathsexammark, ScaledScore = addPupil.rscd_Mathsscaledscore
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
            var amendmentDetail = new AmendmentDetail();
            var addDto = new rscd_Addpupil
            {
                rscd_Name = amendment.Pupil.FullName,
                rscd_Reason = amendmentDetail.GetField<string>(AddPupilAmendment.FIELD_Reason).ToCrmAddReason(),
                rscd_PreviousschoolURN = amendment.Pupil.URN,
                rscd_PreviousschoolLAESTAB = amendment.Pupil.LaEstab
            };

            var results =
                amendmentDetail.GetField<List<PriorAttainment>>(AddPupilAmendment.FIELD_PriorAttainmentResults);

            var reading = results.FirstOrDefault(r => r.Subject == Ks2Subject.Reading);

            if (reading != null)
            {
                addDto.rscd_Readingexamyear = reading.ExamYear;
                addDto.rscd_Readingexammark = reading.TestMark;
                addDto.rscd_Readingscaledscore = reading.ScaledScore;
            }

            var writing = results
                .FirstOrDefault(r => r.Subject == Ks2Subject.Writing);
            if (writing != null)
            {
                addDto.rscd_Writingexamyear = writing.ExamYear;
                addDto.rscd_Writingteacherassessment = writing.TestMark;
                addDto.rscd_Writingscaledscore = writing.ScaledScore;
            }

            var maths = results
                .FirstOrDefault(r => r.Subject == Ks2Subject.Maths);
            if (maths != null)
            {
                addDto.rscd_Mathsexamyear = maths.ExamYear;
                addDto.rscd_Mathsexammark = maths.TestMark;
                addDto.rscd_Mathsscaledscore = maths.ScaledScore;
            }

            return addDto;
        }
    }
}