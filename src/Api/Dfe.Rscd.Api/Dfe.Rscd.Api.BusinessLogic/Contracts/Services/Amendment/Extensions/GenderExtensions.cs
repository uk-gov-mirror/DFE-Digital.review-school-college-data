using System;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.BusinessLogic.Contracts.Entities;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Services.Extensions
{
    public static class GenderExtensions
    {
        public static Gender ToDomainGenderType(this cr3d5_Gender gender)
        {
            switch (gender)
            {
                case cr3d5_Gender.Male:
                    return new Gender {Code = 'M', Description = "Male"};
                case cr3d5_Gender.Female:
                    return new Gender { Code = 'F', Description = "Female" };
                default:
                    throw new ApplicationException();
            }
        }

        public static cr3d5_Gender ToCRMGenderType(this Gender gender)
        {
            switch (gender.Code)
            {
                case 'M':
                    return cr3d5_Gender.Male;
                case 'F':
                    return cr3d5_Gender.Female;
                default:
                    throw new ApplicationException();
            }
        }
    }
}