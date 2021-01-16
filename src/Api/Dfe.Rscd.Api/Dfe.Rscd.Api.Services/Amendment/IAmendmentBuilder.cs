using System;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services
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