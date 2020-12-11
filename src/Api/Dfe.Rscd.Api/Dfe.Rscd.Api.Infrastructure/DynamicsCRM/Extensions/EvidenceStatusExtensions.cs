using System;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.Domain.Core.Enums;

namespace Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Extensions
{
    public static class EvidenceStatusExtensions
    {
        public static EvidenceStatus ToDomainEvidenceStatus(this rscd_Evidencestatus evidencestatus)
        {
            switch (evidencestatus)
            {
                case rscd_Evidencestatus.Now:
                    return EvidenceStatus.Now;
                case rscd_Evidencestatus.Later:
                    return EvidenceStatus.Later;
                case rscd_Evidencestatus.Notrequired:
                    return EvidenceStatus.NotRequired;
                default:
                    throw new ApplicationException();
            }
        }

        public static rscd_Evidencestatus ToCRMEvidenceStatus(this EvidenceStatus evidencestatus)
        {
            switch (evidencestatus)
            {
                case EvidenceStatus.Now:
                    return rscd_Evidencestatus.Now;
                case EvidenceStatus.Later:
                    return rscd_Evidencestatus.Later;
                case EvidenceStatus.NotRequired:
                    return rscd_Evidencestatus.Notrequired;
                default:
                    throw new ApplicationException();
            }
        }
    }
}