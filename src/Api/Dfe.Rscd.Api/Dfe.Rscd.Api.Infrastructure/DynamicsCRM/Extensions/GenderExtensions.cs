using System;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.Domain.Core.Enums;

namespace Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Extensions
{
    public static class GenderExtensions
    {
        public static Gender ToDomainGenderType(this cr3d5_Gender gender)
        {
            switch (gender)
            {
                case cr3d5_Gender.Male:
                    return Gender.Male;
                case cr3d5_Gender.Female:
                    return Gender.Female;
                default:
                    throw new ApplicationException();
            }
        }
        public static cr3d5_Gender ToCRMGenderType(this Gender amendmentType)
        {
            switch (amendmentType)
            {
                case Gender.Male:
                    return cr3d5_Gender.Male;
                case Gender.Female:
                    return cr3d5_Gender.Female;
                default:
                    throw new ApplicationException();
            }
        }
    }
}
