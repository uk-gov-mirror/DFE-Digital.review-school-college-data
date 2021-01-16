using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {           
        /// <summary>
        /// Get a list of ResultRequests where the evidenceID points to the scanned barcode
        /// 
        /// </summary>
        /// <param name="evidenceID"></param>
        /// <param name="uc"></param>
        public static void ScanBarcode(Int32 evidenceID, UserContext uc)
        {

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    //Create the change object.                  
                    Changes changeObj = CreateChangeObject(context, 1, uc);                   
                    context.AddToChanges(changeObj);

                    // TFS 16338, for barcode 129433
                    // This returns back ( C1 = 1, ResultRequestID = 142092, ChangeID = 520782, ResultID = 200241010, ResultEvidenceID = 129270, ScrutinyStatusCode = 'C'
                    string barcode = evidenceID.ToString();
                    List<ResultRequests> requestList=(  
                                                        from rr in context.ResultRequests
                                                        where rr.ResultEvidence.Evidence.Barcode == barcode 
                                                        select rr
                                                      ).ToList();

                    if (requestList.Count <= 0)
                        throw new BusinessLevelException("Barcode doesnt exist.");

                    DateTime dateEnd = DateTime.Now;
                    ScrutinyStatus ssLogged = context.ScrutinyStatus.Where(s => s.ScrutinyStatusCode.ToLower() == "l").FirstOrDefault();
                    foreach (ResultRequests rr in requestList)
                    {
                        // Turn off old one if awaiting evidence only.. leave others as is

                        ResultRequestChanges rrc = context.ResultRequestChanges
                            .Include("ScrutinyStatus")
                            .Include("ResultReasons")
                            .Include("ResultRequests")
                            .Where(
                                rc =>
                                    rc.ResultRequestID==rr.ResultRequestID 
                                    && rc.DateEnd == null
                                ).FirstOrDefault();

                        if (rrc.ScrutinyStatus.ScrutinyStatusCode == "W")
                        {
                            rrc.DateEnd = dateEnd;
                            // add logged status with change id
                            ResultRequestChanges newRC = new ResultRequestChanges
                            {
                                Comments = "",
                                DateEnd = null,
                                ResultReasons = rrc.ResultReasons!=null ? rrc.ResultReasons : (ResultReasons) null,
                                ResultRequests = rrc.ResultRequests,
                                ResultRequestID = rrc.ResultRequestID,
                                ScrutinyStatus = ssLogged,
                                Changes = changeObj
                            };

                            context.ApplyPropertyChanges("ResultRequestChanges", rrc);
                            context.AddToResultRequestChanges(newRC);
                        }
                    }

                    context.SaveChanges();
                }
            }
        }

    }
}
