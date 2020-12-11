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
    public class AddPupilAmendmentBuilder : AmendmentBuilder<AddPupilAmendment>
    {
        public AddPupilAmendmentBuilder(IOrganizationService organizationService, IOutcomeService outcomeService,
            IPupilService pupilService, IOptions<DynamicsOptions> dynamicsOptions, IConfiguration configuration) : base(
            organizationService, outcomeService, pupilService, dynamicsOptions, configuration)
        {
        }

        protected override void MapAmendmentToDto(AddPupilAmendment amendment, rscd_Amendment amendmentDto)
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

        protected override Entity MapAmendmentTypeToDto(AddPupilAmendment amendment)
        {
            var amendmentDetail = (AddPupilAmendmentDetail) amendment.AmendmentDetail ?? new AddPupilAmendmentDetail();
            var addDto = new rscd_Addpupil
            {
                rscd_Name = amendment.Pupil.FullName,
                rscd_Reason = amendmentDetail.Reason.ToCRMAddReason(),
                rscd_PreviousschoolURN = amendment.Pupil.URN,
                rscd_PreviousschoolLAESTAB = amendment.Pupil.LaEstab
            };

            var reading = amendmentDetail.PriorAttainmentResults
                .FirstOrDefault(r => r.Subject == Ks2Subject.Reading);
            if (reading != null)
            {
                addDto.rscd_Readingexamyear = reading.ExamYear;
                addDto.rscd_Readingexammark = reading.TestMark;
                addDto.rscd_Readingscaledscore = reading.ScaledScore;
            }

            var writing = amendmentDetail.PriorAttainmentResults
                .FirstOrDefault(r => r.Subject == Ks2Subject.Writing);
            if (writing != null)
            {
                addDto.rscd_Writingexamyear = writing.ExamYear;
                addDto.rscd_Writingteacherassessment = writing.TestMark;
                addDto.rscd_Writingscaledscore = writing.ScaledScore;
            }

            var maths = amendmentDetail.PriorAttainmentResults
                .FirstOrDefault(r => r.Subject == Ks2Subject.Maths);
            if (maths != null)
            {
                addDto.rscd_Mathsexamyear = maths.ExamYear;
                addDto.rscd_Mathsexammark = maths.TestMark;
                addDto.rscd_Mathsscaledscore = maths.ScaledScore;
            }

            return addDto;
        }

        protected override string RelationshipKey => "rscd_Amendment_Addpupil";
        public override AmendmentType AmendmentType => AmendmentType.AddPupil;
    }
}