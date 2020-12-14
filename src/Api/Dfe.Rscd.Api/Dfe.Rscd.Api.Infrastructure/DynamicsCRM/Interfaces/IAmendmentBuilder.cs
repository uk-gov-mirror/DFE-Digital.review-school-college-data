using System;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Interfaces
{
    public interface IAmendmentBuilder
    {
        AmendmentType AmendmentType { get; }
        rscd_Amendmenttype CrmAmendmentType  { get; }
        Guid BuildAmendments(Amendment amendment);
        AmendmentDetail CreateAmendmentDetails(CrmServiceContext context, rscd_Amendment amendment);

        Amendment CreateAmendment();

        string RelationshipKey { get; }
    }
}