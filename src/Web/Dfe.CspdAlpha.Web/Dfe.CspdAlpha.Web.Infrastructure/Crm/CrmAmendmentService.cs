using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Core.Enums;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dfe.CspdAlpha.Web.Infrastructure.Crm
{
    /// <summary>
    /// Dynamics 365-backed amendment store.
    /// </summary>
    /// <remarks>Currently uses Dynamics SDK (OrganizationServiceContext), which curiously 
    /// doesn't seem to support async calls. Will probably want to switch to use Web API
    /// at some point.</remarks>
    public class CrmAmendmentService : IAmendmentService
    {
        private readonly IOrganizationService _organizationService;
        private readonly Guid _firstLineTeamId = Guid.Parse("469cdfc3-1aba-ea11-a812-000d3a4b2a11");

        private const string fileUploadRelationshipName = "cr3d5_new_Amendment_Evidence_cr3d5_Fileupload";

        public CrmAmendmentService(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        public bool CreateAddPupilAmendment(AddPupilAmendment amendment, out string id)
        {
            var amendmentDto = new new_Amendment();

            using (var context = new CrmServiceContext(_organizationService))
            {
                // Reason for adding
                amendmentDto.cr3d5_addreason = amendment.AddReason;
                // Pupil data
                // don't need to set status as this will default to "Requested" in backend
                amendmentDto.new_Name = amendment.Pupil.FullName;
                amendmentDto.cr3d5_laestab = amendment.Pupil.LaEstab;
                amendmentDto.cr3d5_urn = amendment.Pupil.Urn.Value;
                amendmentDto.cr3d5_pupilid = amendment.Pupil.Id.Value;
                amendmentDto.cr3d5_forename = amendment.Pupil.ForeName;
                amendmentDto.cr3d5_surname = amendment.Pupil.LastName;
                amendmentDto.cr3d5_yeargroup = amendment.Pupil.YearGroup;
                amendmentDto.cr3d5_postcode = amendment.Pupil.PostCode;
                amendmentDto.cr3d5_gender =
                    amendment.Pupil.Gender == Gender.Male ? cr3d5_Gender.Male : cr3d5_Gender.Female;
                amendmentDto.cr3d5_dob = amendment.Pupil.DateOfBirth;
                amendmentDto.cr3d5_admissiondate = amendment.Pupil.DateOfAdmission;

                // prior attainment result
                amendmentDto.cr3d5_priorattainmentresultfor = amendment.PriorAttainment.ResultFor;
                amendmentDto.cr3d5_priorattainmenttest = amendment.PriorAttainment.Test;
                amendmentDto.cr3d5_priorattainmentacademicyear = amendment.PriorAttainment.AcademicYear;
                amendmentDto.cr3d5_priorattainmentlevel = amendment.PriorAttainment.AttainmentLevel;

                // Inclusion details
                amendmentDto.cr3d5_includeinperformanceresults = amendment.InclusionConfirmed;

                // Evidence status
                switch (amendment.EvidenceStatus)
                {
                    case EvidenceStatus.Now:
                        amendmentDto.cr3d5_evidenceoption = cr3d5_EvidenceOption.UploadEvidenceNow;
                        break;
                    case EvidenceStatus.Later:
                        amendmentDto.cr3d5_evidenceoption = cr3d5_EvidenceOption.UploadEvidenceLater;
                        break;
                    case EvidenceStatus.NotRequired:
                    default:
                        amendmentDto.cr3d5_evidenceoption = cr3d5_EvidenceOption.DontUploadEvidence;
                        break;
                }

                // assign to helpdesk 1st line
                amendmentDto.OwnerId = new EntityReference("team", _firstLineTeamId);

                // Save
                context.AddObject(amendmentDto);
                context.SaveChanges();

                var addPUp = context.CreateQuery<new_Amendment>().Single(e => e.Id == amendmentDto.Id);
                id = addPUp?.cr3d5_addpupilref;
            }

            // Upload Evidence
            if (amendment.EvidenceStatus == EvidenceStatus.Now && amendment.EvidenceList.Any())
            {
                var relatedFileUploads = new EntityReferenceCollection();
                foreach (var evidence in amendment.EvidenceList)
                {
                    relatedFileUploads.Add(new EntityReference(cr3d5_Fileupload.EntityLogicalName, new Guid(evidence.Id)));
                }

                var relationship = new Relationship(fileUploadRelationshipName);
                _organizationService.Associate(new_Amendment.EntityLogicalName, amendmentDto.Id, relationship,
                    relatedFileUploads);
            }

            return true;
        }

        public IEnumerable<AddPupilAmendment> GetAddPupilAmendments(int laestab)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendment = context.new_AmendmentSet.Where(
                    x => x.cr3d5_laestab == laestab.ToString());

                // TODO: Map to domain model

                return null;
            }
        }

        public IEnumerable<AddPupilAmendment> GetAddPupilAmendments(string urn)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendments = context.new_AmendmentSet.Where(
                    x => x.cr3d5_urn == urn).ToList();

                return amendments.Select(Convert);
            }
        }

        private AddPupilAmendment Convert(new_Amendment amendment)
        {
            return new AddPupilAmendment
            {
                Id = amendment.Id.ToString(),
                Reference = amendment.cr3d5_addpupilref,
                Status = amendment.cr3d5_amendmentstatus.ToString(),
                CreatedDate = amendment.CreatedOn ?? DateTime.MinValue,
                AddReason = amendment.cr3d5_addreason,
                EvidenceStatus = amendment.cr3d5_evidenceoption == cr3d5_EvidenceOption.UploadEvidenceNow ? EvidenceStatus.Now : amendment.cr3d5_evidenceoption == cr3d5_EvidenceOption.UploadEvidenceLater ? EvidenceStatus.Later : EvidenceStatus.NotRequired,
                Pupil = new Pupil
                {
                    ForeName = amendment.cr3d5_forename,
                    LastName = amendment.cr3d5_surname,
                    DateOfBirth = amendment.cr3d5_dob ?? DateTime.MinValue,
                    DateOfAdmission = amendment.cr3d5_admissiondate ?? DateTime.MinValue,
                    Gender = amendment.cr3d5_gender == cr3d5_Gender.Male ? Gender.Male : Gender.Female,
                    Id = new PupilId(amendment.cr3d5_pupilid),
                    Urn = new URN(amendment.cr3d5_urn)
                },
                PriorAttainment = new PriorAttainment
                {
                    ResultFor = amendment.cr3d5_priorattainmentresultfor,
                    Test = amendment.cr3d5_priorattainmenttest,
                    AttainmentLevel = amendment.cr3d5_priorattainmentlevel,
                    AcademicYear = amendment.cr3d5_priorattainmentacademicyear
                },
                InclusionConfirmed = amendment.cr3d5_includeinperformanceresults ?? false
            };
        }


        public AddPupilAmendment GetAddPupilAmendmentDetail(Guid amendmentId)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendment = context.new_AmendmentSet.Where(
                    x => x.Id == amendmentId).FirstOrDefault();

                // TODO: Get relationship name from attribute
                context.LoadProperty(amendment, fileUploadRelationshipName);

                // TODO: Map to domain model

                return null;
            }
        }

        /// <remarks>In a lot of cases, performing queries using IOrganizationService rather than
        /// CrmServiceContext is likely to give better performance by explicitly only retrieving
        /// the fields you actually need.</remarks>
        public bool CancelAddPupilAmendment(Guid amendmentId)
        {
            // TODO: Get field name from attribute
            var cols = new ColumnSet(
                        new String[] { "cr3d5_amendmentstatus" });
            var amendment = (new_Amendment) _organizationService.Retrieve(
                new_Amendment.EntityLogicalName, amendmentId, cols);

            if (amendment == null
                || amendment.cr3d5_amendmentstatus == new_amendmentStatus.Cancelled)
            {
                return false;
            }

            amendment.cr3d5_amendmentstatus = new_amendmentStatus.Cancelled;

            _organizationService.Update(amendment);

            return true;
        }
    }
}
