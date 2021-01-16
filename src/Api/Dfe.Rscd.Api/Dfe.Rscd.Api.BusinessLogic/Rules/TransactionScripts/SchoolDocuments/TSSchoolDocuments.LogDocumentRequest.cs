using System;
using System.Data.EntityClient;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSSchoolDocuments : Logic.TSBase
    {
        public static void LogDocumentRequest(DocumentRequestsLogs requestLogIn)
        {

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using(Web09_Entities context = new Web09_Entities(conn))
                {
                    try
                    {
                        DocumentRequestsLogs docRequestLog = new DocumentRequestsLogs
                        {
                            RequestTime = requestLogIn.RequestTime,
                            UserName = requestLogIn.UserName,
                            Forename = requestLogIn.Forename,
                            Surname = requestLogIn.Surname,
                            Rolename = requestLogIn.Rolename,
                            ClientIPAddress = requestLogIn.ClientIPAddress,
                            DocumentID = requestLogIn.DocumentID,
                            DocumentTitleAtTimeOfRequest = requestLogIn.DocumentTitleAtTimeOfRequest,
                            DocumentPathAtTimeOfRequest = requestLogIn.DocumentPathAtTimeOfRequest
                        };
                        context.AddToDocumentRequestsLogs(docRequestLog);
                        context.SaveChanges();
                        context.AcceptAllChanges();
                    }
                    catch (Exception ex)
                    {
                        throw Web09Exception.GetException(ex);
                    }
                }
            }
        }
    }
}
