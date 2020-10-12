using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.Domain.Core.Enums;
using System;

namespace Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Extensions
{
    public static class AmendmentTypeExtension
    {
        public static AmendmentType ToDomainAmendmentType(this rscd_Amendmenttype amendmentType)
        {
            switch (amendmentType)
            {
                case rscd_Amendmenttype.Addapupil:
                    return AmendmentType.AddPupil;
                case rscd_Amendmenttype.Removeapupil:
                    return AmendmentType.RemovePupil;
                default:
                    throw new ApplicationException();
            }
        }
        public static rscd_Amendmenttype  ToCRMAmendmentType(this AmendmentType amendmentType)
        {
            switch (amendmentType)
            {
                case AmendmentType.AddPupil:
                    return rscd_Amendmenttype.Addapupil;
                case AmendmentType.RemovePupil:
                    return rscd_Amendmenttype.Removeapupil;
                default:
                    throw new ApplicationException();
            }
        }
    }
}
