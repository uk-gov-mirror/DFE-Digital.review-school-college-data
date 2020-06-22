using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using Microsoft.Xrm.Sdk;
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
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendmentDto = new new_AddPupilAmendment();

                // TODO: Map from domain model

                // don't need to set status as this will default to "Requested" in backend
                amendmentDto.new_Name = amendment.Pupil.FullName;
                amendmentDto.cr3d5_Laestab = amendment.Pupil.LaEstab;
                amendmentDto.cr3d5_PupilId = amendment.Pupil.Id.Value;
                amendmentDto.new_Forename = amendment.Pupil.FullName;
                amendmentDto.new_Surname = amendment.Pupil.LastName;
                amendmentDto.cr3d5_Yeargroup = amendment.Pupil.YearGroup;
                amendmentDto.cr3d5_Postcode = amendment.Pupil.PostCode;
                amendmentDto.cr3d5_IncludeInPerformanceResults = amendment.InclusionConfirmed;
                amendmentDto.cr3d5_Gender = amendment.Pupil.Gender == Gender.Male ? cr3d5_Gender.Male: cr3d5_Gender.Female;
                amendmentDto.new_DOB = amendment.Pupil.DateOfBirth;
                amendmentDto.cr3d5_AdmissionDate = amendment.Pupil.DateOfAdmission;

                context.AddObject(amendmentDto);
                context.SaveChanges();
                id = amendmentDto.Id.ToString();
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
