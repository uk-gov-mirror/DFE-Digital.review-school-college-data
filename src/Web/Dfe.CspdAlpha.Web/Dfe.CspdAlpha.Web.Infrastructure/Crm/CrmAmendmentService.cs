using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Domain.Core.Enums;

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

        public CrmAmendmentService(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        public bool CreateAddPupilAmendment(AddPupilAmendment amendment, out string id)
        {
            var amendmentDto = new new_AddPupilAmendment();

            using (var context = new CrmServiceContext(_organizationService))
            {
                // Reason for adding
                amendmentDto.new_addreason = amendment.AddReason;
                // Pupil data
                // don't need to set status as this will default to "Requested" in backend
                amendmentDto.new_Name = amendment.Pupil.FullName;
                amendmentDto.cr3d5_Laestab = amendment.Pupil.LaEstab;
                amendmentDto.new_URN = amendment.Pupil.Urn.Value;
                amendmentDto.cr3d5_PupilId = amendment.Pupil.Id.Value;
                amendmentDto.new_Forename = amendment.Pupil.FullName;
                amendmentDto.new_Surname = amendment.Pupil.LastName;
                amendmentDto.cr3d5_Yeargroup = amendment.Pupil.YearGroup;
                amendmentDto.cr3d5_Postcode = amendment.Pupil.PostCode;
                amendmentDto.cr3d5_Gender =
                    amendment.Pupil.Gender == Gender.Male ? cr3d5_Gender.Male : cr3d5_Gender.Female;
                amendmentDto.new_DOB = amendment.Pupil.DateOfBirth;
                amendmentDto.cr3d5_AdmissionDate = amendment.Pupil.DateOfAdmission;

                // prior attainment result
                amendmentDto.cr3d5_PriorAttainmentResultFor = amendment.PriorAttainment.ResultFor;
                amendmentDto.cr3d5_PriorAttainmentTest = amendment.PriorAttainment.Test;
                amendmentDto.cr3d5_PriorAttainmentAcademicYear = amendment.PriorAttainment.AcademicYear;
                amendmentDto.cr3d5_PriorAttainmentLevel = amendment.PriorAttainment.AttainmentLevel;

                // Inclusion details
                amendmentDto.cr3d5_IncludeInPerformanceResults = amendment.InclusionConfirmed;

                // Evidence status
                switch (amendment.EvidenceStatus)
                {
                    case EvidenceStatus.Now:
                        amendmentDto.cr3d5_EvidenceOption = cr3d5_EvidenceOption.UploadEvidenceNow;
                        break;
                    case EvidenceStatus.Later:
                        amendmentDto.cr3d5_EvidenceOption = cr3d5_EvidenceOption.UploadEvidenceLater;
                        break;
                    case EvidenceStatus.NotRequired:
                    default:
                        amendmentDto.cr3d5_EvidenceOption = cr3d5_EvidenceOption.DontUploadEvidence;
                        break;
                }

                // Save
                context.AddObject(amendmentDto);
                context.SaveChanges();

                var addPUp = context.CreateQuery<new_AddPupilAmendment>().Single(e => e.Id == amendmentDto.Id);
                id = addPUp?.cr3d5_AddPupilRef;
            }

            // Upload Evidence
            if (amendment.EvidenceStatus == EvidenceStatus.Now && amendment.EvidenceList.Any())
            {
                var relatedFileUploads = new EntityReferenceCollection();
                foreach (var evidence in amendment.EvidenceList)
                {
                    relatedFileUploads.Add(new EntityReference("cr3d5_fileupload", new Guid(evidence.Id)));
                }

                var relationship = new Relationship("cr3d5_new_AddPupilAmendment_cr3d5_EvidenceFileS");
                _organizationService.Associate(new_AddPupilAmendment.EntityLogicalName, amendmentDto.Id, relationship,
                    relatedFileUploads);
            }

            return true;
        }

        public IEnumerable<AddPupilAmendment> GetAddPupilAmendments(int laestab)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendment = context.new_AddPupilAmendmentSet.Where(
                    x => x.cr3d5_Laestab == laestab.ToString());

                // TODO: Map to domain model

                return null;
            }
        }

        public AddPupilAmendment GetAddPupilAmendmentDetail(Guid amendmentId)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendment = context.new_AddPupilAmendmentSet.Where(
                    x => x.Id == amendmentId).FirstOrDefault();

                // TODO: Get relationship name from attribute
                context.LoadProperty(amendment, "cr3d5_new_AddPupilAmendment_cr3d5_EvidenceFileS");

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
            var amendment = (new_AddPupilAmendment) _organizationService.Retrieve(
                new_AddPupilAmendment.EntityLogicalName, amendmentId, cols);

            if (amendment == null
                || amendment.cr3d5_AmendmentStatus == new_amendmentStatus.Cancelled)
            {
                return false;
            }

            amendment.cr3d5_AmendmentStatus = new_amendmentStatus.Cancelled;

            _organizationService.Update(amendment);

            return true;
        }
    }
}
