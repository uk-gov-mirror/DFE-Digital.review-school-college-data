using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public bool CreateAddPupilAmendment(AddPupilAmendment amendment)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendmentDto = new new_AddPupilAmendment();

                // TODO: Map from domain model

                // don't need to set status as this will default to "Requested" in backend

                // new_Name is the description used in the list view in Dynamics - I think we can
                // set this up to be "calculated" in Dynamics
                amendmentDto.new_Name = "Geoff Jones"; 
                amendmentDto.cr3d5_Laestab = "7654321";
                amendmentDto.cr3d5_PupilId = "111111111111";
                amendmentDto.new_Forename = "Geoff";
                amendmentDto.new_Surname = "Jones";
                amendmentDto.new_DOB = new DateTime(2002, 1, 17);
                amendmentDto.cr3d5_Gender = cr3d5_Gender.Male;
                amendmentDto.cr3d5_AdmissionDate = new DateTime(2007, 6, 1);
                amendmentDto.cr3d5_Yeargroup = "8";
                amendmentDto.cr3d5_Postcode = "GH6 7DF";

                // prior attainment result
                amendmentDto.cr3d5_PriorAttainmentResultFor = "";
                amendmentDto.cr3d5_PriorAttainmentTest = "";
                amendmentDto.cr3d5_PriorAttainmentAcademicYear = "";
                amendmentDto.cr3d5_PriorAttainmentLevel = "";

                // evidence
                amendmentDto.cr3d5_EvidenceOption = cr3d5_EvidenceOption.UploadEvidenceNow;
                // TODO: Evidence file refs

                amendmentDto.cr3d5_IncludeInPerformanceResults = true;
                
                context.AddObject(amendmentDto);
                context.SaveChanges();
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
