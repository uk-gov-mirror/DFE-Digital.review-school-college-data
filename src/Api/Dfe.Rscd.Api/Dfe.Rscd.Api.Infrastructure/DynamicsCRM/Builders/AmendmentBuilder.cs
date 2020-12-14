using System;
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
    public abstract class AmendmentBuilder<T> : IAmendmentBuilder where T : Amendment
    {
        private readonly string _allocationYear;
        private readonly EntityReference _autoRecordedUser;
        private readonly EntityReference _firstLineTeam;


        private readonly IOrganizationService _organizationService;
        private readonly IOutcomeService _outcomeService;

        protected readonly rscd_Checkingwindow[] Ks4Windows =
            {rscd_Checkingwindow.KS4June, rscd_Checkingwindow.KS4Late, rscd_Checkingwindow.KS4Errata};

        protected readonly IPupilService PupilService;

        protected AmendmentBuilder(IOrganizationService organizationService, IOutcomeService outcomeService,
            IPupilService pupilService, IOptions<DynamicsOptions> dynamicsOptions, IConfiguration configuration)
        {
            PupilService = pupilService;
            _outcomeService = outcomeService;
            _organizationService = organizationService;
            _firstLineTeam = new EntityReference("team", dynamicsOptions.Value.Helpdesk1stLineTeamId);
            _autoRecordedUser = new EntityReference("systemuser", dynamicsOptions.Value.AutoRecordedUser);
            _allocationYear = configuration["AllocationYear"];
        }

        protected abstract string RelationshipKey { get; }

        public abstract Amendment CreateAmendment();

        public abstract AmendmentDetail CreateAmendmentDetails(CrmServiceContext context, rscd_Amendment amendment);

        public Guid BuildAmendments(Amendment amendment)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendmentDto = new rscd_Amendment
                {
                    rscd_Checkingwindow = amendment.CheckingWindow.ToCRMCheckingWindow(),
                    rscd_Amendmenttype = amendment.AmendmentType.ToCRMAmendmentType(),
                    rscd_Academicyear = _allocationYear,
                    rscd_URN = amendment.URN,
                    OwnerId = _firstLineTeam
                };

                _outcomeService.SetOutcome(amendmentDto, amendment);

                MapAmendmentToDto((T) amendment, amendmentDto);
                var amendmentTypeEntity = MapAmendmentTypeToDto((T) amendment);
                context.AddObject(amendmentTypeEntity);
                context.AddObject(amendmentDto);

                // Save
                var result = context.SaveChanges();
                if (result.HasError)
                    throw result.FirstOrDefault(e => e.Error != null)?.Error ?? new ApplicationException();

                if (amendmentDto.rscd_Outcome == rscd_Outcome.Autoapproved ||
                    amendmentDto.rscd_Outcome == rscd_Outcome.Autorejected ||
                    amendmentDto.rscd_Evidencestatus == rscd_Evidencestatus.Later)
                {
                    amendmentDto.StateCode = rscd_AmendmentState.Inactive;
                    amendmentDto.rscd_recorded_by = _autoRecordedUser;
                    _organizationService.Update(amendmentDto);
                }

                var relationship = new Relationship(RelationshipKey);
                _organizationService.Associate(amendmentTypeEntity.LogicalName, amendmentTypeEntity.Id, relationship,
                    new EntityReferenceCollection
                    {
                        new EntityReference(amendmentDto.LogicalName, amendmentDto.Id)
                    });

                return amendmentDto.Id;
            }
        }

        public abstract AmendmentType AmendmentType { get; }

        protected abstract void MapAmendmentToDto(T amendment, rscd_Amendment amendmentDto);

        protected abstract Entity MapAmendmentTypeToDto(T amendment);
    }
}