using Dfe.CspdAlpha.Web.Application.Models.Amendments;
using ApiAmendmentType = Dfe.Rscd.Web.ApiClient.AmendmentType;

namespace Dfe.CspdAlpha.Web.Application.Application.Extensions
{
    public static class AmendmentTypeExtensions
    {
        public static AmendmentType ToApplicationAmendmentType(
            this ApiAmendmentType amendmentType)
        {
            switch (amendmentType)
            {
                case ApiAmendmentType.AddPupil:
                    return AmendmentType.AddPupil;
                case ApiAmendmentType.RemovePupil:
                    return AmendmentType.RemovePupil;
            }

            return AmendmentType.Unknown;
        }
        public static ApiAmendmentType ToApiAmendmentType(
            this AmendmentType amendmentType)
        {
            switch (amendmentType)
            {
                case AmendmentType.AddPupil:
                    return ApiAmendmentType.AddPupil;
                case AmendmentType.RemovePupil:
                    return ApiAmendmentType.RemovePupil;
            }

            return ApiAmendmentType.AddPupil; // TODO: should have an unknown option here
        }
    }
}
