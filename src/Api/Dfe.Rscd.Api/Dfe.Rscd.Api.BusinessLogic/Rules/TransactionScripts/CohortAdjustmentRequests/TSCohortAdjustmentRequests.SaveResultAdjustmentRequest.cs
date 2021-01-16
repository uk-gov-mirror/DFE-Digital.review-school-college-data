using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSCohortAdjustmentRequests : Logic.TSBase
    {
        public static void SaveResultAdjustmentRequests(List<ResultAdjustmentChangeRequest> requests, string scrutinyStatusCode, int? ReasonId, string comments, UserContext uc)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    foreach (ResultAdjustmentChangeRequest r in requests)
                    {
                        //get the most recent RRC row
                        var q =
                            (
                                from rrc in context.ResultRequestChanges
                                where rrc.ResultRequestID == r.RequestId
                                      && rrc.DateEnd == null
                                orderby rrc.ChangeID descending
                                select new { rrc }
                            ).ToList();

                        if (q.Count > 0)
                        {
                            var rrc = q.First().rrc;
                            rrc.ScrutinyStatusReference.Load();
                            if (scrutinyStatusCode == "L" &&
                                (rrc.ScrutinyStatus.ScrutinyStatusCode != "W" &&
                                 rrc.ScrutinyStatus.ScrutinyStatusCode != "N"))
                            {
                                throw Web09Exception.GetBusinessException(Web09MessageList.UnamendedResultExists);
                            }

                            //if there have been updates since, throw a concurrency exception
                            if (rrc.ChangeID != r.ChangeID)
                                throw Web09Exception.GetBusinessException(
                                    Web09MessageList.TransactionDataConcurrencyError);
                            else
                            {
                                rrc.ResultRequestsReference.Load();

                                // turn off the old one
                                rrc.DateEnd = DateTime.Now;
                                context.ApplyPropertyChanges("ResultRequestChanges", rrc);

                                //Create the change object.
                                Changes changeObj = CreateChangeObject(context, 1, uc);
                                context.AddToChanges(changeObj);

                                ResultReasons rr = null;

                                if (ReasonId.HasValue)
                                {
                                    rr = (from rr1 in context.ResultReasons
                                          where rr1.ResultReasonID == ReasonId.Value
                                          select rr1).First();
                                }

                                ScrutinyStatus ss = (from ss1 in context.ScrutinyStatus
                                                     where ss1.ScrutinyStatusCode == scrutinyStatusCode
                                                     select ss1).First();

                                //create new row
                                ResultRequestChanges rrcNew = new ResultRequestChanges();
                                rrcNew.Changes = changeObj;
                                rrcNew.Comments = comments;
                                if (rr != null)
                                    rrcNew.ResultReasons = rr;
                                rrcNew.ResultRequests = rrc.ResultRequests;
                                rrcNew.ScrutinyStatus = ss;

                                context.AddToResultRequestChanges(rrcNew);

                                context.SaveChanges();
                            }
                        }
                    }
                }
            }
        }
    }
}
