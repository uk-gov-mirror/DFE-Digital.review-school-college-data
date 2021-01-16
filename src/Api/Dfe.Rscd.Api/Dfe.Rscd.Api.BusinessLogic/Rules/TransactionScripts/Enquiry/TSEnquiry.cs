using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts.Enquiry
{
    public class TSEnquiry : Logic.TSBase
    {
        public static List<Helplines> GetAllHelplines()
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return context
                        .Helplines
                        .OrderBy
                        (
                            helpline =>
                                helpline.ListOrder
                        )
                        .ToList();
                }
            }
        }

        public static List<CallTypes> GetCallTypesByHelpline(int helplineID)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    if (!context.Helplines.Any(helpline => helpline.HelplineID == helplineID))
                    {
                        throw new ArgumentOutOfRangeException("helplineID");
                    }

                    return context
                        .CallTypes
                        .Include("Helplines")
                        .Where
                        (
                            calltype =>
                                calltype.Helplines.HelplineID == helplineID
                        )
                        .OrderBy
                        (
                            calltype =>
                                calltype.ListOrder
                        )
                        .ToList();
                }
            }
        }
    }
}
