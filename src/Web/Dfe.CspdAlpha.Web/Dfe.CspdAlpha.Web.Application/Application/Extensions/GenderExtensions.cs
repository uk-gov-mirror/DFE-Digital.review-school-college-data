using Dfe.CspdAlpha.Web.Application.Models.Common;
using ApiGender = Dfe.Rscd.Web.ApiClient.Gender;

namespace Dfe.CspdAlpha.Web.Application.Application.Extensions
{
    public static class GenderExtensions
    {
        public static Gender ToApplicationGender(
            this ApiGender gender)
        {
            switch (gender)
            {
                case ApiGender.Male:
                    return Gender.Male;
                case ApiGender.Female:
                    return Gender.Female;
            }

            return Gender.Unknown;
        }
        public static ApiGender ToApiGender(
            this Gender gender)
        {
            switch (gender)
            {
                case Gender.Male:
                    return ApiGender.Male;
                case Gender.Female:
                    return ApiGender.Female;
            }

            return ApiGender.Unknown; 
        }
    }
}
