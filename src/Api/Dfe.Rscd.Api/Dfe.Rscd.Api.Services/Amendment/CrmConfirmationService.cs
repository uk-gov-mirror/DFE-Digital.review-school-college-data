using System;
using System.Linq;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Entities;
using Dfe.Rscd.Api.Domain.Entities;
using Microsoft.Xrm.Sdk;

namespace Dfe.Rscd.Api.Services
{
    public class CrmConfirmationService : IConfirmationService
    {
        private readonly IOrganizationService _organizationService;

        public CrmConfirmationService(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        public ConfirmationRecord GetConfirmationRecord(string userId, string establishmentId)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var confirmation = context.new_reviewandconfirmschoolSet
                    .SingleOrDefault(r => r.new_UserID == userId && r.new_SchoolURN == establishmentId);
                return confirmation != null
                    ? new ConfirmationRecord
                    {
                        UserId = confirmation.new_UserID,
                        EstablishmentId = confirmation.new_SchoolURN,
                        DataConfirmed = confirmation.new_Confirmed ?? false,
                        ReviewCompleted = confirmation.new_Reviewed ?? false,
                        ConfirmationDate = confirmation.rscd_Confirmationdate ?? DateTime.MinValue
                    }
                    : null;
            }
        }

        public bool UpdateConfirmationRecord(ConfirmationRecord confirmationRecord)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var confirmation = context.new_reviewandconfirmschoolSet
                    .SingleOrDefault(r =>
                        r.new_UserID == confirmationRecord.UserId &&
                        r.new_SchoolURN == confirmationRecord.EstablishmentId);
                if (confirmation == null) throw new ApplicationException();
                confirmation.new_Reviewed = confirmationRecord.ReviewCompleted;
                confirmation.new_Confirmed = confirmationRecord.DataConfirmed;
                if (confirmationRecord.DataConfirmed) confirmation.rscd_Confirmationdate = DateTime.Now.Date;
                context.UpdateObject(confirmation);
                context.SaveChanges();
                return true;
            }
        }

        public bool CreateConfirmationRecord(ConfirmationRecord confirmationRecord)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var confirmation = new new_reviewandconfirmschool
                {
                    new_UserID = confirmationRecord.UserId,
                    new_SchoolURN = confirmationRecord.EstablishmentId,
                    new_Reviewed = confirmationRecord.ReviewCompleted,
                    new_Confirmed = confirmationRecord.DataConfirmed
                };
                context.AddObject(confirmation);
                context.SaveChanges();
                return true;
            }
        }
    }
}