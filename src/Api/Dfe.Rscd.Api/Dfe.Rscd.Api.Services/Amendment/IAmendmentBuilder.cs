using System;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Amendments;

namespace Dfe.Rscd.Api.Services
{
    public interface IAmendmentBuilder
    {
        AmendmentType AmendmentType { get; }
        rscd_Amendmenttype CrmAmendmentType  { get; }
        AmendmentOutcome BuildAmendments(Amendment amendment);
        AmendmentDetail CreateAmendmentDetails(CrmServiceContext context, rscd_Amendment amendment);

        Amendment CreateAmendment();

        string RelationshipKey { get; }
    }
}