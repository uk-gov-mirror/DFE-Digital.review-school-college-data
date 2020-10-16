using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using ApiKs2Subject = Dfe.Rscd.Web.ApiClient.Ks2Subject;

namespace Dfe.CspdAlpha.Web.Application.Application.Extensions
{
    public static class Ks2SubjectExtensions
    {
        public static Ks2Subject ToApplicationKs2Subject(
            this ApiKs2Subject ks2Subject)
        {
            switch (ks2Subject)
            {
                case ApiKs2Subject.Reading:
                    return Ks2Subject.Reading;
                case ApiKs2Subject.Writing:
                    return Ks2Subject.Writing;
                case ApiKs2Subject.Maths:
                    return Ks2Subject.Maths;
            }

            return Ks2Subject.Unknown;
        }
        public static ApiKs2Subject ToApiKs2Subject(
            this Ks2Subject ks2Subject)
        {
            switch (ks2Subject)
            {
                case Ks2Subject.Reading:
                    return ApiKs2Subject.Reading;
                case Ks2Subject.Writing:
                    return ApiKs2Subject.Writing;
                case Ks2Subject.Maths:
                    return ApiKs2Subject.Maths;
            }

            return ApiKs2Subject.Unknown;
        }
    }
}
