using System;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.BusinessLogic.Contracts.Entities.Core.Enums;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Services.Extensions
{
    public static class AddReasonExtensions
    {
        public static string ToDomainAddReason(this cr3d5_Addreason addReason)
        {
            switch (addReason)
            {
                case cr3d5_Addreason.Newpupil:
                    return AddReason.New;
                case cr3d5_Addreason.Existingpupil:
                    return AddReason.Existing;
                default:
                    throw new ApplicationException();
            }
        }

        public static cr3d5_Addreason ToCrmAddReason(this string addReason)
        {
            switch (addReason)
            {
                case AddReason.New:
                    return cr3d5_Addreason.Newpupil;
                case AddReason.Existing:
                    return cr3d5_Addreason.Existingpupil;
                default:
                    throw new ApplicationException();
            }
        }
    }
}