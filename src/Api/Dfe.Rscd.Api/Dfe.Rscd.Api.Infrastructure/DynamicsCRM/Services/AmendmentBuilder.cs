using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Config;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Extensions;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Xrm.Sdk;
using System;
using System.Linq;
using Dfe.Rscd.Api.Domain.Interfaces;

namespace Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Services
{
    public class AmendmentBuilder : IAmendmentBuilder
    {
        private readonly rscd_Checkingwindow[] ks4Windows = new[]
            {rscd_Checkingwindow.KS4June, rscd_Checkingwindow.KS4Late, rscd_Checkingwindow.KS4Errata};
        private readonly string ALLOCATION_YEAR;
        private readonly EntityReference _firstLineTeam;
        private readonly EntityReference _autoRecordedUser;


        private IOrganizationService _organizationService;
        private IOutcomeService _outcomeService;
        private IPupilService _pupilService;

        public AmendmentBuilder(IOrganizationService organizationService, IOutcomeService outcomeService, IPupilService pupilService, IOptions<DynamicsOptions> dynamicsOptions, IConfiguration configuration)
        {
            _pupilService = pupilService;
            _outcomeService = outcomeService;
            _organizationService = organizationService;
            _firstLineTeam = new EntityReference("team", dynamicsOptions.Value.Helpdesk1stLineTeamId);
            _autoRecordedUser = new EntityReference("systemuser", dynamicsOptions.Value.AutoRecordedUser);
            ALLOCATION_YEAR = configuration["AllocationYear"];
        }

        private Entity BuildAmendmentType(Amendment amendment)
        {
            // Create Remove
            if (amendment.AmendmentType == AmendmentType.RemovePupil)
            {
                var pupil = _pupilService.GetById(amendment.CheckingWindow, amendment.Pupil.Id);
                amendment.Pupil = pupil;
                var removeDto = new rscd_Removepupil
                {
                    rscd_Name = amendment.Pupil.FullName
                };
                var removeDetail = (RemovePupil)amendment.AmendmentDetail;

                removeDto.rscd_reasoncode = removeDetail.ReasonCode;
                removeDto.rscd_Subreason = removeDetail.SubReason;
                removeDto.rscd_Details = removeDetail.Detail;

                if (pupil.Allocations != null)
                {
                    removeDto.rscd_allocationyear = pupil.Allocations.Select(x=>x.Year).FirstOrDefault();
                    removeDto.rscd_allocationyeardescription = GenerateAllocationYearDescription(removeDto.rscd_allocationyear);

                    if (pupil.Allocations.Count > 1)
                    {
                        removeDto.rscd_allocationyear_1 = pupil.Allocations.Select(x => x.Year).Skip(1).FirstOrDefault();
                        removeDto.rscd_allocationyear_1description = GenerateAllocationYearDescription(removeDto.rscd_allocationyear_1);
                    }

                    if (pupil.Allocations.Count > 2)
                    {
                        removeDto.rscd_allocationyear_2 = pupil.Allocations.Select(x => x.Year).Skip(2).FirstOrDefault();
                        removeDto.rscd_allocationyear_2description = GenerateAllocationYearDescription(removeDto.rscd_allocationyear_2);
                    }
                }

                return removeDto;
            }

            // Create add
            if (amendment.AmendmentType == AmendmentType.AddPupil)
            {
                var addDto = new rscd_Addpupil
                {
                    rscd_Name = amendment.Pupil.FullName
                };
                var addDetail = (AddPupil)amendment.AmendmentDetail;
                addDto.rscd_Reason = addDetail.Reason.ToCRMAddReason();
                addDto.rscd_PreviousschoolURN = amendment.Pupil.URN;
                addDto.rscd_PreviousschoolLAESTAB = amendment.Pupil.LaEstab;
                var reading = addDetail.PriorAttainmentResults.FirstOrDefault(r => r.Subject == Ks2Subject.Reading);
                if (reading != null)
                {
                    addDto.rscd_Readingexamyear = reading.ExamYear;
                    addDto.rscd_Readingexammark = reading.TestMark;
                    addDto.rscd_Readingscaledscore = reading.ScaledScore;
                }

                var writing = addDetail.PriorAttainmentResults.FirstOrDefault(r => r.Subject == Ks2Subject.Writing);
                if (writing != null)
                {
                    addDto.rscd_Writingexamyear = writing.ExamYear;
                    addDto.rscd_Writingteacherassessment = writing.TestMark;
                    addDto.rscd_Writingscaledscore = writing.ScaledScore;
                }

                var maths = addDetail.PriorAttainmentResults.FirstOrDefault(r => r.Subject == Ks2Subject.Maths);
                if (maths != null)
                {
                    addDto.rscd_Mathsexamyear = maths.ExamYear;
                    addDto.rscd_Mathsexammark = maths.TestMark;
                    addDto.rscd_Mathsscaledscore = maths.ScaledScore;
                }

                return addDto;
            }

            return null;
        }

        private string GenerateAllocationYearDescription(int? allocationYear)
        {
            if (allocationYear != null)
            {
                // generate "YYYY/YY" representation for helpdesk UI
                return $"{allocationYear - 1}/{allocationYear.ToString().Remove(0, 2)}";
            }

            return string.Empty;
        }

        public Guid BuildAmendments(Amendment amendment)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendmentDto = new rscd_Amendment
                {
                    rscd_Checkingwindow = amendment.CheckingWindow.ToCRMCheckingWindow(),
                    rscd_Amendmenttype = amendment.AmendmentType.ToCRMAmendmentType(),
                    rscd_Academicyear = ALLOCATION_YEAR,
                    rscd_URN = amendment.URN,
                    OwnerId = _firstLineTeam
                };

                // pupil details
                amendmentDto.rscd_UPN = amendment.Pupil.UPN;
                amendmentDto.rscd_ULN = amendment.Pupil.ULN;
                amendmentDto.rscd_Name = amendment.Pupil.FullName;
                amendmentDto.rscd_Firstname = amendment.Pupil.ForeName;
                amendmentDto.rscd_Lastname = amendment.Pupil.LastName;
                amendmentDto.rscd_Gender = amendment.Pupil.Gender.ToCRMGenderType();
                amendmentDto.rscd_Dateofbirth = amendment.Pupil.DateOfBirth;
                amendmentDto.rscd_Age = amendment.Pupil.Age;
                if (ks4Windows.Any(w => w == amendmentDto.rscd_Checkingwindow))
                {
                    amendmentDto.rscd_Dateofadmission = amendment.Pupil.DateOfAdmission;
                    amendmentDto.rscd_Yeargroup = amendment.Pupil.YearGroup;
                } 
                amendmentDto.rscd_Evidencestatus = amendment.EvidenceStatus.ToCRMEvidenceStatus();

                var amendmentTypeEntity = BuildAmendmentType(amendment);

                context.AddObject(amendmentTypeEntity);

                _outcomeService.SetOutcome(amendmentDto, amendment);

                amendmentDto.rscd_rm_scrutiny_reason_code = amendment.ScrutinyReasonCode;
                amendmentDto.rscd_rm_amdflag = amendment.AmdFlag;

                context.AddObject(amendmentDto);

                // Save
                var result = context.SaveChanges();
                if (result.HasError)
                {
                    throw result.FirstOrDefault(e => e.Error != null)?.Error ?? new ApplicationException();
                }

                if (amendmentDto.rscd_Outcome == rscd_Outcome.Autoapproved ||
                    amendmentDto.rscd_Outcome == rscd_Outcome.Autorejected ||
                    amendmentDto.rscd_Evidencestatus == rscd_Evidencestatus.Later)
                {
                    amendmentDto.StateCode = rscd_AmendmentState.Inactive;
                    amendmentDto.rscd_recorded_by = _autoRecordedUser; 
                    _organizationService.Update(amendmentDto);
                }

                var relationship = new Relationship(amendmentDto.rscd_Amendmenttype == rscd_Amendmenttype.Removeapupil ? "rscd_Amendment_Removepupil" : "rscd_Amendment_Addpupil"); // TODO: need better way to derive this
                _organizationService.Associate(amendmentTypeEntity.LogicalName, amendmentTypeEntity.Id, relationship,
                    new EntityReferenceCollection
                    {
                        new EntityReference(amendmentDto.LogicalName, amendmentDto.Id)
                    });

                return amendmentDto.Id;
            }
        }
    }
}
