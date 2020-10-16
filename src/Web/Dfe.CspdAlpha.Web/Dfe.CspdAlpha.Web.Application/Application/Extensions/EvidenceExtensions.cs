using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Application.Extensions
{
    public static class EvidenceExtensions
    {
        public static EvidenceOption ToApplicationEvidenceOption(
            this EvidenceStatus evidenceStatus)
        {
            switch (evidenceStatus)
            {
                case EvidenceStatus.Now:
                    return EvidenceOption.UploadNow;
                case EvidenceStatus.Later:
                    return EvidenceOption.UploadLater;
                case EvidenceStatus.NotRequired:
                    return EvidenceOption.NotRequired;
            }

            return EvidenceOption.Unknown;
        }
        public static EvidenceStatus ToApiEvidenceStatus(
            this EvidenceOption evidenceOption)
        {
            switch (evidenceOption)
            {
                case EvidenceOption.UploadNow:
                    return EvidenceStatus.Now;
                case EvidenceOption.UploadLater:
                    return EvidenceStatus.Later;
                case EvidenceOption.NotRequired:
                    return EvidenceStatus.NotRequired;
            }

            return EvidenceStatus.Unknown;
        }
    }
}
