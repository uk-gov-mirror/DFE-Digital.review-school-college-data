using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using Microsoft.Xrm.Sdk;
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
                amendmentDto.new_Name = "Geoff Jones";
                amendmentDto.cr3d5_Laestab = "7654321";
                amendmentDto.cr3d5_PupilId = "111111111111";
                amendmentDto.new_Forename = "Geoff";
                amendmentDto.new_Surname = "Jones";
                amendmentDto.cr3d5_Yeargroup = "8";
                amendmentDto.cr3d5_Postcode = "GH6 7DF";
                amendmentDto.cr3d5_IncludeInPerformanceResults = true;
                amendmentDto.cr3d5_Gender = cr3d5_Gender.Male;
                amendmentDto.new_DOB = new DateTime(2002, 1, 17);
                amendmentDto.cr3d5_AdmissionDate = new DateTime(2007, 6, 1);

                context.AddObject(amendmentDto);
                context.SaveChanges();

                // TODO: Prior attainment result (if provided)
                // TODO: Evidence file refs
            }

            return true;
        }

        public IEnumerable<AddPupilAmendment> GetAddPupilAmendments(int laestab)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendment = context.new_AddPupilAmendmentSet.Where(
                    x => x.cr3d5_Laestab == laestab.ToString()).FirstOrDefault();

                // TODO: Get relationship name from attribute
                context.LoadProperty(amendment, "cr3d5_new_AddPupilAmendment_cr3d5_EvidenceFileS");

                // TODO: Map to domain model
                return null;
            }
        }
    }
}
