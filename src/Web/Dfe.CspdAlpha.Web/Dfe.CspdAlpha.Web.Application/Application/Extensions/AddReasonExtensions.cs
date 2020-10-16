using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using ApiAddReason = Dfe.Rscd.Web.ApiClient.AddReason;

namespace Dfe.CspdAlpha.Web.Application.Application.Extensions
{
    public static class AddReasonExtensions
    {
        public static AddReason ToApplicationAddReason(
            this ApiAddReason addReason)
        {
            switch (addReason)
            {
                case ApiAddReason.New:
                    return AddReason.New;
                case ApiAddReason.Existing:
                    return AddReason.Existing;
            }

            return AddReason.Unknown;
        }
        public static ApiAddReason ToApiAddReason(
            this AddReason addReason)
        {
            switch (addReason)
            {
                case AddReason.New:
                    return ApiAddReason.New;
                case AddReason.Existing:
                    return ApiAddReason.Existing;
            }

            throw new ApplicationException("Add reason does not exist in API: " + addReason);
        }
    }
}
