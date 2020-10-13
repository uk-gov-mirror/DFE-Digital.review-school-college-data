using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Config;
using Microsoft.Extensions.Options;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Extensions;
using Microsoft.Azure.Cosmos.Linq;
using System.Xml;
using System.Text;
using System.IO;
using System.Dynamic;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Services
{
    public class CrmAmendmentService : IAmendmentService
    {
        private readonly IOrganizationService _organizationService;
        private readonly Guid _firstLineTeamId;
        private IEstablishmentService _establishmentService;

        private const string fileUploadRelationshipName = "cr3d5_new_Amendment_Evidence_cr3d5_Fileupload";

        private readonly rscd_Checkingwindow[] ks4Windows = new[]
            {rscd_Checkingwindow.KS4Errata, rscd_Checkingwindow.KS4Late, rscd_Checkingwindow.KS4Errata};

        public CrmAmendmentService(
            IOrganizationService organizationService,
            IEstablishmentService establishmentService,
            IOptions<DynamicsOptions> config)
        {
            _establishmentService = establishmentService;
            _organizationService = organizationService;
            _firstLineTeamId = config.Value.Helpdesk1stLineTeamId;
        }

        private cr3d5_establishment GetOrCreateEstablishment(CheckingWindow checkingWindow, string id,
            CrmServiceContext context)
        {
            var establishmentDto =
                context.cr3d5_establishmentSet.SingleOrDefault(
                    e => e.cr3d5_Urn == id);
            if (establishmentDto == null)
            {
                establishmentDto =
                    context.cr3d5_establishmentSet.SingleOrDefault(
                        e => e.cr3d5_LAEstab == id);
            }

            if (establishmentDto != null)
            {
                return establishmentDto;
            }

            Establishment establishment = null;
            try
            {
                establishment = _establishmentService.GetByURN(checkingWindow, new URN(id));
            }
            catch
            {
            }

            if (establishment == null)
            {
                establishment = _establishmentService.GetByLAId(checkingWindow, id);
            }

            if (establishment == null)
            {
                return null;
            }


            establishmentDto = new cr3d5_establishment
            {
                cr3d5_name = establishment.Name,
                cr3d5_Urn = establishment.Urn.Value,
                cr3d5_LAEstab = establishment.LaEstab,
                cr3d5_Schooltype = establishment.SchoolType,
                cr3d5_Numberofamendments = 0
            };
            context.AddObject(establishmentDto);
            var result = context.SaveChanges();
            if (result.HasError)
            {
                throw result.FirstOrDefault(e => e.Error != null)?.Error ?? new ApplicationException();
            }

            return establishmentDto;
        }

        public string CreateAmendment(Amendment amendment)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendmentDto = new rscd_Amendment
                {
                    rscd_Amendmenttype = amendment.AmendmentType.ToCRMAmendmentType(),
                    rscd_Checkingwindow = amendment.CheckingWindow.ToCRMCheckingWindow(),
                    rscd_Academicyear = "2019" // TODO: must be from config
                };

                // pupil details
                amendmentDto.rscd_UPN = amendment.Pupil.UPN; // TODO: need to set up ULN
                amendmentDto.rscd_Name = amendment.Pupil.FullName;
                amendmentDto.rscd_Firstname = amendment.Pupil.ForeName;
                amendmentDto.rscd_Lastname = amendment.Pupil.LastName;
                amendmentDto.rscd_Gender = amendment.Pupil.Gender.ToCRMGenderType();
                amendmentDto.rscd_Dateofbirth = amendment.Pupil.DateOfBirth;
                if (ks4Windows.Any(w => w == amendmentDto.rscd_Checkingwindow))
                {
                    amendmentDto.rscd_Dateofadmission = amendment.Pupil.DateOfAdmission;
                    amendmentDto.rscd_Yeargroup = amendment.Pupil.YearGroup;
                }

                // TODO: default statuses for now
                amendmentDto.rscd_Outcome = rscd_Outcome.AwaitingDfEreview;
                amendmentDto.rscd_Recordedby = "RSCD Website";

                // assign to helpdesk 1st line
                amendmentDto.OwnerId = new EntityReference("team", _firstLineTeamId);

                // Create Remove
                var removeDto = new rscd_Removepupil
                {
                    rscd_Name = amendment.Pupil.FullName
                };
                var removeDetail = (RemovePupil) amendment.AmendmentDetail;
                removeDto.rscd_Reason = removeDetail.Reason;
                removeDto.rscd_Subreason = removeDetail.SubReason;
                removeDto.rscd_Details = removeDetail.Detail;
                // Save
                context.AddObject(amendmentDto);
                context.AddObject(removeDto);
                var result = context.SaveChanges();
                if (result.HasError)
                {
                    throw result.FirstOrDefault(e => e.Error != null)?.Error ?? new ApplicationException();
                }

                var relationship = new Relationship("rscd_Amendment_Removepupilamendment_rscd_");
                _organizationService.Associate(rscd_Amendment.EntityLogicalName, amendmentDto.Id, relationship,
                    new EntityReferenceCollection
                    {
                        new EntityReference(rscd_Removepupil.EntityLogicalName, removeDto.Id)
                    });


                // Relate to establishment
                var amendmentEstablishment = GetOrCreateEstablishment(amendment.CheckingWindow, amendment.URN, context);
                RelateEstablishmentToAmendment(amendmentEstablishment, amendmentDto.Id, context);


                var addPUp = context.CreateQuery<rscd_Amendment>().Single(e => e.Id == amendmentDto.Id);
                return addPUp?.rscd_Referencenumber;
            }
        }

        public void RelateEstablishmentToAmendment(cr3d5_establishment establishment, Guid amendmentId,
            CrmServiceContext context)
        {
            var relationship = new Relationship("rscd_cr3d5_establishment_rscd_amendments");
            _organizationService.Associate(cr3d5_establishment.EntityLogicalName, establishment.Id, relationship,
                new EntityReferenceCollection
                {
                    new EntityReference(rscd_Amendment.EntityLogicalName, amendmentId)
                });
        }

        //public bool CreateAddPupilAmendment(CheckingWindow checkingWindow, AddPupilAmendment amendment, out string id)
        //{
        //    Guid amendmentId;
        //    using (var context = new CrmServiceContext(_organizationService))
        //    {
        //        var amendmentDto = new new_Amendment
        //        {
        //            rscd_Amendmenttype = new_Amendment_rscd_Amendmenttype.Addpupil
        //        };

        //        // Reason for adding
        //        amendmentDto.cr3d5_addreasontype = amendment.AddReason == AddReason.New
        //            ? cr3d5_Pupiltype.Newpupil
        //            : cr3d5_Pupiltype.Existingpupil;

        //        // Pupil data
        //        amendmentDto.cr3d5_pupilid = amendment.AddReason == AddReason.Existing ? amendment.Pupil.UPN : string.Empty;
        //        amendmentDto.cr3d5_laestab = amendment.AddReason == AddReason.Existing ? amendment.Pupil.LaEstab : string.Empty;
        //        amendmentDto.cr3d5_urn = amendment.Pupil.Urn.Value;
        //        amendmentDto.new_Name = amendment.Pupil.FullName;
        //        amendmentDto.cr3d5_forename = amendment.Pupil.ForeName;
        //        amendmentDto.cr3d5_surname = amendment.Pupil.LastName;
        //        amendmentDto.cr3d5_gender =
        //            amendment.Pupil.Gender == Gender.Male ? cr3d5_Gender.Male : cr3d5_Gender.Female;
        //        amendmentDto.cr3d5_dob = amendment.Pupil.DateOfBirth;
        //        amendmentDto.cr3d5_admissiondate = amendment.Pupil.DateOfAdmission;
        //        amendmentDto.cr3d5_yeargroup = amendment.Pupil.YearGroup;

        //        // prior attainment result
        //        if (amendment.PriorAttainmentResults.Any(r => r.Subject == Ks2Subject.Reading))
        //        {
        //            var readingResult = amendment.PriorAttainmentResults.First(r => r.Subject == Ks2Subject.Reading);
        //            amendmentDto.rscd_ReadingExamYear = readingResult.ExamYear;
        //            amendmentDto.rscd_ReadingTestMark = readingResult.TestMark;
        //            amendmentDto.rscd_ReadingScaledScore = readingResult.ScaledScore;
        //        }
        //        if (amendment.PriorAttainmentResults.Any(r => r.Subject == Ks2Subject.Writing))
        //        {
        //            var writingResult = amendment.PriorAttainmentResults.First(r => r.Subject == Ks2Subject.Writing);
        //            amendmentDto.rscd_WritingExamYear = writingResult.ExamYear;
        //            amendmentDto.rscd_WritingTestMark = writingResult.TestMark;
        //            amendmentDto.rscd_WritingScaledScore = writingResult.ScaledScore;
        //        }
        //        if (amendment.PriorAttainmentResults.Any(r => r.Subject == Ks2Subject.Maths))
        //        {
        //            var mathsResult = amendment.PriorAttainmentResults.First(r => r.Subject == Ks2Subject.Maths);
        //            amendmentDto.rscd_MathsExamYear = mathsResult.ExamYear;
        //            amendmentDto.rscd_MathsTestMark = mathsResult.TestMark;
        //            amendmentDto.rscd_MathsScaledScore = mathsResult.ScaledScore;
        //        }

        //        // Inclusion details
        //        amendmentDto.cr3d5_includeinperformanceresults = amendment.InclusionConfirmed;

        //        // Evidence status
        //        switch (amendment.EvidenceStatus)
        //        {
        //            case EvidenceStatus.Now:
        //                amendmentDto.cr3d5_evidenceoption = cr3d5_EvidenceOption.UploadEvidenceNow;
        //                break;
        //            case EvidenceStatus.Later:
        //                amendmentDto.cr3d5_evidenceoption = cr3d5_EvidenceOption.UploadEvidenceLater;
        //                break;
        //            case EvidenceStatus.NotRequired:
        //            default:
        //                amendmentDto.cr3d5_evidenceoption = cr3d5_EvidenceOption.DontUploadEvidence;
        //                break;
        //        }

        //        // assign to helpdesk 1st line
        //        amendmentDto.OwnerId = new EntityReference("team", _firstLineTeamId);

        //        // Save
        //        context.AddObject(amendmentDto);
        //        var result = context.SaveChanges();
        //        if (result.HasError)
        //        {
        //            throw result.FirstOrDefault(e => e.Error != null)?.Error ?? new ApplicationException();
        //        }
        //        // Relate to establishment
        //        var amendmentEstablishment = GetOrCreateEstablishment(checkingWindow, amendment.Pupil.Urn.Value, context);
        //        RelateEstablishment(amendmentEstablishment, amendmentDto.Id, context);

        //        // If add existing pupil then create matching remove amendment if valid establishment
        //        if (amendmentDto.cr3d5_addreasontype == cr3d5_Pupiltype.Existingpupil)
        //        {
        //            var removeAmendmentEstablishment = GetOrCreateEstablishment(checkingWindow, amendment.Pupil.LaEstab, context);
        //            if (removeAmendmentEstablishment != null)
        //            {
        //                // Create remove amendment
        //                var removeDto = new new_Amendment
        //                {
        //                    rscd_Amendmenttype = new_Amendment_rscd_Amendmenttype.Removepupil,
        //                    new_Name = amendment.Pupil.FullName,
        //                    cr3d5_laestab = amendment.Pupil.LaEstab,
        //                    cr3d5_pupilid = amendment.Pupil.UPN,
        //                    cr3d5_forename = amendment.Pupil.ForeName,
        //                    cr3d5_surname = amendment.Pupil.LastName,
        //                    cr3d5_gender = amendment.Pupil.Gender == Gender.Male ? cr3d5_Gender.Male : cr3d5_Gender.Female,
        //                    cr3d5_dob = amendment.Pupil.DateOfBirth,
        //                    OwnerId = new EntityReference("team", _firstLineTeamId)
        //                };
        //                context.AddObject(removeDto);
        //                result = context.SaveChanges();
        //                if (result.HasError)
        //                {
        //                    throw result.FirstOrDefault(e => e.Error != null)?.Error ?? new ApplicationException();
        //                }
        //                // Relate to establishment
        //                RelateEstablishment(removeAmendmentEstablishment, removeDto.Id, context);
        //                // Create amendment relationship
        //                var addExistingPupilRelationship = new Relationship("rscd_new_amendment_new_amendment");
        //                addExistingPupilRelationship.PrimaryEntityRole = EntityRole.Referencing;
        //                _organizationService.Associate(new_Amendment.EntityLogicalName, amendmentDto.Id, addExistingPupilRelationship, new EntityReferenceCollection
        //                {
        //                    new EntityReference(new_Amendment.EntityLogicalName, removeDto.Id)
        //                });
        //                _organizationService.Associate(new_Amendment.EntityLogicalName, removeDto.Id, addExistingPupilRelationship, new EntityReferenceCollection
        //                {
        //                    new EntityReference(new_Amendment.EntityLogicalName, amendmentDto.Id)
        //                });
        //            }
        //        }

        //        var addPUp = context.CreateQuery<new_Amendment>().Single(e => e.Id == amendmentDto.Id);
        //        id = addPUp?.cr3d5_addpupilref;
        //        amendmentId = amendmentDto.Id;
        //    }

        //    // Upload Evidence
        //    if (amendment.EvidenceStatus == EvidenceStatus.Now && amendment.EvidenceList.Any())
        //    {
        //        RelateEvidence(amendmentId, amendment.EvidenceList, false);
        //    }

        //    return true;
        //}

        public void RelateEstablishment(cr3d5_establishment establishment, Guid amendmentId, CrmServiceContext context)
        {
            var relationship = new Relationship("cr3d5_cr3d5_establishment_new_amendment");
            _organizationService.Associate(cr3d5_establishment.EntityLogicalName, establishment.Id, relationship,
                new EntityReferenceCollection
                {
                    new EntityReference(rscd_Amendment.EntityLogicalName, amendmentId)
                });
        }

        //public void RelateEvidence(Guid amendmentId, List<Evidence> evidenceList, bool updateEvidenceOption)
        //{
        //    var relatedFileUploads = new EntityReferenceCollection();
        //    foreach (var evidence in evidenceList)
        //    {
        //        relatedFileUploads.Add(new EntityReference(cr3d5_Fileupload.EntityLogicalName, new Guid(evidence.Id)));
        //    }

        //    var relationship = new Relationship(fileUploadRelationshipName);
        //    _organizationService.Associate(new_Amendment.EntityLogicalName, amendmentId, relationship,
        //        relatedFileUploads);

        //    if (updateEvidenceOption)
        //    {
        //        UpdateEvidenceStatus(amendmentId);
        //    }
        //}

        //public IEnumerable<AddPupilAmendment> GetAddPupilAmendments(int laestab)
        //{
        //    using (var context = new CrmServiceContext(_organizationService))
        //    {
        //        var amendments = context.new_AmendmentSet.Where(
        //            x => x.cr3d5_laestab == laestab.ToString()).ToList();

        //        return amendments.Select(Convert);
        //    }
        //}

        public IEnumerable<Amendment> GetAmendments(CheckingWindow checkingWindow, string urn)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var establishment = context.cr3d5_establishmentSet.Where(x => x.cr3d5_Urn == urn).ToList();
                var amendments = context.rscd_AmendmentSet.Where(
                    x => x.rscd_Establishmentv2.Id == establishment.First().Id &&
                         x.rscd_Checkingwindow == rscd_Checkingwindow.KS5).ToList();


                return amendments.Select(a => Convert(a, urn))
                    .OrderByDescending(o => o.CreatedDate);
            }
        }

        private Amendment Convert(rscd_Amendment amendment, string urn)
        {
            return new Amendment
            {
                CheckingWindow = amendment.rscd_Checkingwindow.Value.ToCDomainCheckingWindow(),
                Id = amendment.Id.ToString(),
                Reference = amendment.rscd_Referencenumber,
                Status = GetStatus(amendment),
                Evidence = new Evidence(),
                EvidenceStatus = EvidenceStatus.NotRequired,
                URN = urn,
                Pupil = new Pupil
                {
                    UPN = amendment.rscd_UPN ?? amendment.rscd_ULN,
                    ForeName = amendment.rscd_Firstname,
                    LastName = amendment.rscd_Lastname,
                    Gender = amendment.rscd_Gender.Value.ToDomainGenderType(),
                    DateOfBirth = amendment.rscd_Dateofbirth.Value,
                    DateOfAdmission = amendment.rscd_Dateofadmission.GetValueOrDefault(),
                    YearGroup = amendment.rscd_Yeargroup,
                    Urn = new URN(urn)
                },
                AmendmentType = amendment.rscd_Amendmenttype.Value.ToDomainAmendmentType(),
                AmendmentDetail = GetAdmendmentDetails(amendment),
                CreatedDate = amendment.CreatedOn.Value
            };
        }

        private IAmendmentType GetAdmendmentDetails(rscd_Amendment amendment)
        {
            if (amendment.rscd_Amendmenttype == rscd_Amendmenttype.Removeapupil)
            {
                using (var context = new CrmServiceContext(_organizationService))
                {
                    var removePupil = context.rscd_RemovepupilSet
                        .Where(x => x.rscd_Removepupilamendment.Id == amendment.Id).FirstOrDefault();
                    return new RemovePupil
                    {
                        Reason = removePupil.rscd_Reason,
                        SubReason = removePupil.rscd_Subreason,
                        Detail = removePupil.rscd_Details
                    };
                }


            }

            return null;
        }

        public IEnumerable<AddPupilAmendment> GetAddPupilAmendments(CheckingWindow checkingWindow, string urn)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendments = context.new_AmendmentSet.Where(
                    x => x.cr3d5_urn == urn).ToList();

                return amendments.Select(Convert)
                    .Where(a => checkingWindow != CheckingWindow.KS4Late ||
                                (a.Status == "Approved" || a.Status == "Rejected"))
                    .OrderByDescending(o => o.CreatedDate);
            }
        }

        private AddPupilAmendment Convert(new_Amendment amendment)
        {
            return new AddPupilAmendment
            {
                Id = amendment.Id.ToString(),
                Reference = amendment.cr3d5_addpupilref,
                Status = GetStatus(amendment),
                CreatedDate = amendment.CreatedOn ?? DateTime.MinValue,
                AddReason = amendment.cr3d5_addreasontype == cr3d5_Pupiltype.Newpupil
                    ? AddReason.New
                    : AddReason.Existing,
                EvidenceStatus = amendment.cr3d5_evidenceoption == cr3d5_EvidenceOption.UploadEvidenceNow
                    ?
                    EvidenceStatus.Now
                    : amendment.cr3d5_evidenceoption == cr3d5_EvidenceOption.UploadEvidenceLater
                        ? EvidenceStatus.Later
                        : EvidenceStatus.NotRequired,
                Pupil = new Pupil
                {
                    Id = string.IsNullOrWhiteSpace(amendment.cr3d5_pupilid) ? string.Empty : amendment.cr3d5_pupilid,
                    Urn = new URN(amendment.cr3d5_urn),
                    LaEstab = amendment.cr3d5_laestab,
                    ForeName = amendment.cr3d5_forename,
                    LastName = amendment.cr3d5_surname,
                    DateOfBirth = amendment.cr3d5_dob ?? DateTime.MinValue,
                    Gender = amendment.cr3d5_gender == cr3d5_Gender.Male ? Gender.Male : Gender.Female,
                    DateOfAdmission = amendment.cr3d5_admissiondate ?? DateTime.MinValue,
                    YearGroup = amendment.cr3d5_yeargroup
                },
                PriorAttainmentResults = GetPriorAttainmentResult(amendment),
                InclusionConfirmed = amendment.cr3d5_includeinperformanceresults ?? false
            };
        }

        private List<PriorAttainment> GetPriorAttainmentResult(new_Amendment amendment)
        {
            var results = new List<PriorAttainment>();
            if (!string.IsNullOrEmpty(amendment.rscd_ReadingExamYear))
            {
                results.Add(new PriorAttainment
                {
                    Subject = Ks2Subject.Reading,
                    ExamYear = amendment.rscd_ReadingExamYear,
                    TestMark = amendment.rscd_ReadingTestMark,
                    ScaledScore = amendment.rscd_ReadingScaledScore
                });
            }

            if (!string.IsNullOrEmpty(amendment.rscd_WritingExamYear))
            {
                results.Add(new PriorAttainment
                {
                    Subject = Ks2Subject.Writing,
                    ExamYear = amendment.rscd_WritingExamYear,
                    TestMark = amendment.rscd_WritingTestMark,
                    ScaledScore = amendment.rscd_WritingScaledScore
                });
            }

            if (!string.IsNullOrEmpty(amendment.rscd_MathsExamYear))
            {
                results.Add(new PriorAttainment
                {
                    Subject = Ks2Subject.Maths,
                    ExamYear = amendment.rscd_MathsExamYear,
                    TestMark = amendment.rscd_MathsTestMark,
                    ScaledScore = amendment.rscd_MathsScaledScore
                });
            }

            return results;
        }

        private string GetStatus(new_Amendment amendment)
        {
            if (amendment.cr3d5_firstlinedecision == cr3d5_Decision.Approved ||
                amendment.cr3d5_firstlinedecision == cr3d5_Decision.Rejected)
            {
                return amendment.cr3d5_firstlinedecision.ToString();
            }

            if (amendment.cr3d5_secondlinedecision == cr3d5_Decision.Approved ||
                amendment.cr3d5_secondlinedecision == cr3d5_Decision.Rejected)
            {
                return amendment.cr3d5_secondlinedecision.ToString();
            }

            if (amendment.cr3d5_finaldecision != null)
            {
                return amendment.cr3d5_finaldecision.ToString();
            }

            return amendment.cr3d5_amendmentstatus.ToString();
        }

        private string GetStatus(rscd_Amendment amendment)
        {
            if (amendment.rscd_Decision1 == cr3d5_Decision.Approved ||
                amendment.rscd_Decision1 == cr3d5_Decision.Rejected)
            {
                return amendment.rscd_Decision1.ToString();
            }

            if (amendment.rscd_Decision2 == cr3d5_Decision.Approved ||
                amendment.rscd_Decision2 == cr3d5_Decision.Rejected)
            {
                return amendment.rscd_Decision2.ToString();
            }

            if (amendment.rscd_Decisionfinal != null)
            {
                return amendment.rscd_Decisionfinal.ToString();
            }

            return amendment.rscd_Amendmentstatus.ToString();
        }

        //public AddPupilAmendment GetAddPupilAmendmentDetail(Guid amendmentId)
        //{
        //    using (var context = new CrmServiceContext(_organizationService))
        //    {
        //        var amendment = context.new_AmendmentSet.Where(
        //            x => x.Id == amendmentId).FirstOrDefault();

        //        // TODO: Get relationship name from attribute
        //        var relationship = new Relationship(fileUploadRelationshipName);
        //        context.LoadProperty(amendment, relationship);

        //        return Convert(amendment);
        //    }
        //}

        ///// <remarks>In a lot of cases, performing queries using IOrganizationService rather than
        ///// CrmServiceContext is likely to give better performance by explicitly only retrieving
        ///// the fields you actually need.</remarks>
        //public bool CancelAddPupilAmendment(Guid amendmentId)
        //{
        //    // TODO: Get field name from attribute
        //    var cols = new ColumnSet(
        //                new String[] { "cr3d5_amendmentstatus" });
        //    var amendment = (new_Amendment)_organizationService.Retrieve(
        //        new_Amendment.EntityLogicalName, amendmentId, cols);

        //    if (amendment == null
        //        || amendment.cr3d5_amendmentstatus == new_amendmentStatus.Cancelled)
        //    {
        //        return false;
        //    }

        //    amendment.cr3d5_amendmentstatus = new_amendmentStatus.Cancelled;

        //    _organizationService.Update(amendment);
        //}

        public IEnumerable<IDictionary<string, object>> GetAmendments()
        {
            var fetchCount = 50;
            var pageNumber = 1;
            var recordCount = 0;

            // Specify the current paging cookie. For retrieving the first page, 
            // pagingCookie should be null.
            string pagingCookie = null;

            // Reference: https://docs.microsoft.com/en-us/powerapps/developer/common-data-service/org-service/page-large-result-sets-with-fetchxml
            // Only retrieving the attributes that are needed
            // Currently this only filters on Final decision == Approved
            // For Beta more filter criteria would be needed, depending on how we define the data model to support the different key stages
            // and types of amendment.
            var attributeDictionary = new Dictionary<string, string>
            {
                {"new_amendmentid", "AMENDMENT_ID"},
                {"rscd_amendmenttype", "AMENDMENT_TYPE"},
                {"cr3d5_addreasontype", "ADD_PUPIL_REASON"},
                {"cr3d5_urn", "URN"},
                {"cr3d5_laestab", "LAESTAB"},
                {"cr3d5_pupilid", "PUPIL_ID"},
                {"cr3d5_forename", "FORENAME"},
                {"cr3d5_surname", "SURNAME"},
                {"cr3d5_dob", "DATE_OF_BIRTH"},
                {"cr3d5_gender", "GENDER"},
                {"cr3d5_admissiondate", "ADMISSION_DATE"},
                {"cr3d5_yeargroup", "YEAR_GROUP"},
                {"cr3d5_includeinperformanceresults", "INCL_IN_PERF_RESULTS"},
                {"cr3d5_addpupilref", "REFERENCE_NUMBER"},
                {"rscd_readingexamyear", "PA_READING_EXAM_YEAR"},
                {"rscd_readingtestmark", "PA_READING_TEST_MARK"},
                {"rscd_readingscaledscore", "PA_READING_SCALED_SCORE"},
                {"rscd_writingexamyear", "PA_WRITING_EXAM_YEAR"},
                {"rscd_writingtestmark", "PA_WRITING_TEST_MARK"},
                {"rscd_writingscaledscore", "PA_WRITING_SCALED_SCORE"},
                {"rscd_mathsexamyear", "PA_MATHS_EXAM_YEAR"},
                {"rscd_mathstestmark", "PA_MATHS_TEST_MARK"},
                {"rscd_mathsscaledscore", "PA_MATHS_SCALED_SCORE"}
            };

            string fetchXml = @"<fetch version='1.0'>
                                  <entity name='new_amendment'>
                                    <filter>
                                        <condition attribute='cr3d5_finaldecisionname' operator= 'eq' value='Approved' />
                                    </filter>
                                    <order attribute='new_amendmentid' />
                                  </entity>
                               </fetch>";

            Directory.CreateDirectory("output");

            // Build fetchXml XmlDoc with specified attributes.
            var xmlDoc = CreateXmlDoc(fetchXml, attributeDictionary.Keys.ToArray());

            var optionsSets = GetOptionSets();
            var amendments = new List<IDictionary<string, object>>();

            while (true)
            {
                // update fetchXml with new paging values
                var xml = CreateXml(xmlDoc, pagingCookie, pageNumber, fetchCount);

                // Execute the fetch query and get the xml result.
                RetrieveMultipleRequest fetchRequest1 = new RetrieveMultipleRequest
                {
                    Query = new FetchExpression(xml)
                };

                EntityCollection returnCollection =
                    ((RetrieveMultipleResponse) _organizationService.Execute(fetchRequest1)).EntityCollection;

                recordCount += returnCollection.Entities.Count;

                foreach (var c in returnCollection.Entities)
                {
                    // Unfortunately, if a record doesn't have a value for a requested attribute, the attribute is not returned for that
                    // record. This makes things a bit trickier when writing to CSV as you can end up with values written against the wrong
                    // columns.
                    dynamic expandoObj = new ExpandoObject();
                    var expandoDict = (IDictionary<string, object>) expandoObj;

                    foreach (var attribute in attributeDictionary)
                    {
                        var attributeFound = c.TryGetAttributeValue(attribute.Key, out object recordAttributeValue);

                        if (attributeFound)
                        {
                            if (recordAttributeValue is OptionSetValue)
                            {
                                expandoDict[attribute.Value] = optionsSets[attribute.Key]
                                    .First(t => t.Item1 == ((OptionSetValue) recordAttributeValue).Value).Item2;
                            }
                            else
                            {
                                expandoDict[attribute.Value] = recordAttributeValue.ToString();
                            }
                        }
                        else
                        {
                            expandoDict[attribute.Value] = "";
                        }
                    }

                    amendments.Add(expandoObj);
                }

                // Check for more records, if it returns 1.
                if (returnCollection.MoreRecords)
                {
                    // Increment the page number to retrieve the next page.
                    pageNumber++;

                    // Set the paging cookie to the paging cookie returned from current results.                            
                    pagingCookie = returnCollection.PagingCookie;
                }
                else
                {
                    // If no more records in the result nodes, exit the loop.
                    break;
                }
            }

            return amendments;
        }

        private Dictionary<string, List<Tuple<int, string>>> GetOptionSets()
        {
            var attributes = new[] {"rscd_amendmenttype", "cr3d5_addreasontype", "cr3d5_gender"};
            var optionSetsLookup = new Dictionary<string, List<Tuple<int, string>>>();
            var attrReq = new RetrieveAttributeRequest
            {
                EntityLogicalName = "new_amendment",
                RetrieveAsIfPublished = true
            };
            foreach (var attribute in attributes)
            {
                attrReq.LogicalName = attribute;
                var attrResponse = (RetrieveAttributeResponse) _organizationService.Execute(attrReq);
                var attrMetaData = (EnumAttributeMetadata) attrResponse.AttributeMetadata;
                var lookup = attrMetaData.OptionSet.Options
                    .Select(o => new Tuple<int, string>(o.Value.Value, o.Label.UserLocalizedLabel.Label)).ToList();
                optionSetsLookup.Add(attribute, lookup);
            }

            return optionSetsLookup;
        }

        private static XmlDocument CreateXmlDoc(string xml, string[] attributes)
        {
            StringReader stringReader = new StringReader(xml);
            var reader = new XmlTextReader(stringReader);

            XmlDocument doc = new XmlDocument();
            doc.Load(reader);

            var entityNode = doc.DocumentElement.SelectSingleNode("/fetch/entity");

            foreach (var attribute in attributes)
            {
                var attNode = doc.CreateElement("attribute");

                attNode.SetAttribute("name", attribute);
                entityNode.AppendChild(attNode);
            }

            return doc;
        }

        private static string CreateXml(XmlDocument doc, string cookie, int page, int count)
        {
            XmlAttributeCollection attrs = doc.DocumentElement.Attributes;

            if (cookie != null)
            {
                XmlAttribute pagingAttr = doc.CreateAttribute("paging-cookie");
                pagingAttr.Value = cookie;
                attrs.Append(pagingAttr);
            }

            XmlAttribute pageAttr = doc.CreateAttribute("page");
            pageAttr.Value = System.Convert.ToString(page);
            attrs.Append(pageAttr);

            XmlAttribute countAttr = doc.CreateAttribute("count");
            countAttr.Value = System.Convert.ToString(count);
            attrs.Append(countAttr);

            StringBuilder sb = new StringBuilder(1024);
            StringWriter stringWriter = new StringWriter(sb);

            XmlTextWriter writer = new XmlTextWriter(stringWriter);
            doc.WriteTo(writer);
            writer.Close();

            return sb.ToString();
        }
    }
}
