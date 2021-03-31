﻿using System;
using System.Linq;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Entities;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Amendments;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Config;
using Microsoft.Xrm.Sdk;
using Microsoft.Extensions.Logging;

namespace Dfe.Rscd.Api.Services
{
    public abstract class AmendmentBuilder : IAmendmentBuilder
    {
        private readonly string _allocationYear;
        private readonly EntityReference _autoRecordedUser;
        private readonly EntityReference _firstLineTeam;
        private readonly IOrganizationService _organizationService;
        private readonly IOutcomeService _outcomeService;
        private readonly ILogger<AmendmentBuilder> _logger;

        protected readonly rscd_Checkingwindow[] Ks4Windows =
            {rscd_Checkingwindow.KS4June, rscd_Checkingwindow.KS4Late, rscd_Checkingwindow.KS4Errata};

        protected readonly IPupilService PupilService;

        protected AmendmentBuilder(
            IOrganizationService organizationService, 
            IOutcomeService outcomeService,
            IPupilService pupilService, 
            DynamicsOptions dynamicsOptions, 
            IAllocationYearConfig year,
            ILogger<AmendmentBuilder> logger)
        {
            PupilService = pupilService;
            _outcomeService = outcomeService;
            _organizationService = organizationService;
            _firstLineTeam = new EntityReference("team", dynamicsOptions.Helpdesk1stLineTeamId);
            _autoRecordedUser = new EntityReference("systemuser", dynamicsOptions.AutoRecordedUser);
            _allocationYear = year.Value;
            _logger = logger;
        }

        public abstract string RelationshipKey { get; }

        public abstract Amendment CreateAmendment();

        public abstract AmendmentDetail CreateAmendmentDetails(CrmServiceContext context, rscd_Amendment amendment);

        public abstract rscd_Amendmenttype CrmAmendmentType { get; }

        public AmendmentOutcome BuildAmendments(Amendment amendment)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                AmendmentOutcome outcome;
                
                var amendmentDto = new rscd_Amendment
                {
                    rscd_Checkingwindow = amendment.CheckingWindow.ToCRMCheckingWindow(),
                    rscd_Amendmenttype = amendment.AmendmentType.ToCRMAmendmentType(),
                    rscd_Academicyear = _allocationYear,
                    rscd_URN = amendment.URN,
                    OwnerId = _firstLineTeam
                };

                outcome = _outcomeService.ApplyRules(amendmentDto, amendment);

                if (amendment.IsUserConfirmed && outcome.IsComplete && outcome.FurtherQuestions == null)
                {
                    MapAmendmentToDto(amendment, amendmentDto);
                    var amendmentTypeEntity = MapAmendmentTypeToDto(amendment);
                    context.AddObject(amendmentTypeEntity);
                    context.AddObject(amendmentDto);

                    // Save
                    var result = context.SaveChanges();
                    if (result.HasError)
                        throw result.FirstOrDefault(e => e.Error != null)?.Error ?? new ApplicationException();

                    if (amendmentDto.rscd_Outcome == rscd_Outcome.Autoapproved ||
                        amendmentDto.rscd_Outcome == rscd_Outcome.Autorejected)
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

                    outcome.IsAmendmentCreated = true;
                    outcome.NewAmendmentId = amendmentDto.Id;

                    _logger.LogInformation("Amendment requested - ID: {amendmentId}", amendmentDto.Id);
                }

                return outcome;
            }
        }

        public abstract AmendmentType AmendmentType { get; }

        protected abstract void MapAmendmentToDto(Amendment amendment, rscd_Amendment amendmentDto);

        protected abstract Entity MapAmendmentTypeToDto(Amendment amendment);
    }
}