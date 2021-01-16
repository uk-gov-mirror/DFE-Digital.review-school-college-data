using System.Data.EntityClient;
using System.Linq;
using System.Transactions;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent
    {

        public static void CancelStudentRequest(int requestId, UserContext userContext)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {

                    using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope() )
                    {
                        StudentRequests studentRequest = context.StudentRequests
                            .Include("Students")
                            .Include("Students.PINCLS")
                            .Where(sr => sr.StudentRequestID == requestId)
                            .Select(sr => sr)
                            .FirstOrDefault();

                        if (studentRequest == null)
                            throw Web09Exception.GetBusinessException(Web09MessageList.StudentRequestNotFound);

                        Changes changeObj = CreateChangeObject(context, Contants.CHANGE_TYPE_ID_SCHOOL_AMENDMENT, userContext);
                        context.AddToChanges(changeObj);
                        context.SaveChanges();

                        StudentRequestChanges currentRequestChangeObj = context.StudentRequestChanges
                            .Where(src => src.StudentRequestID == requestId && src.DateEnd == null)
                            .Select(src => src)
                            .FirstOrDefault();

                        currentRequestChangeObj.DateEnd = System.DateTime.Now;
                        context.ApplyPropertyChanges("StudentRequestChanges", currentRequestChangeObj);
                        context.SaveChanges();

                        StudentRequestChanges updatedRequestChangeObj = new StudentRequestChanges();
                        updatedRequestChangeObj.StudentRequests = studentRequest;
                        updatedRequestChangeObj.Changes = changeObj;
                        updatedRequestChangeObj.ScrutinyStatus = context.ScrutinyStatus
                            .Where(ss => ss.ScrutinyStatusCode == Contants.SCRUTINY_STATUS_CANCELLED)
                            .Select(ss => ss)
                            .FirstOrDefault();
                        context.AddToStudentRequestChanges(updatedRequestChangeObj);

                        //if the student request is an Add Pupil request, then reset the PINCL back to NULL
                        //as cancelling the request should set the pupil back to unlisted again.
                        string pinclCode = studentRequest.Students.PINCLs.P_INCL ;
                        if(pinclCode == Contants.PINCL_UNLISTED_PUPIL_WITH_ADD_PUPIL_REQUEST_PLASC_KS2 ||
                            pinclCode == Contants.PINCL_UNLISTED_PUPIL_WITH_ADD_PUPIL_REQUEST_NON_PLASC_KS4 ||
                            pinclCode == Contants.PINCL_UNLISTED_PUPIL_WITH_ADD_PUPIL_REQUEST_PLASC_KS4 ||
                            pinclCode == Contants.PINCL_UNLISTED_PUPIL_WITH_ADD_PUPIL_REQUEST_PLASC_KS5)
                        {
                            
                            Students student = studentRequest.Students;
                            student.PINCLs = null;
                            context.ApplyPropertyChanges("Students", student);
                        }

                        context.SaveChanges();

                        context.AcceptAllChanges();
                        transaction.Complete();
                    }

                }

            }
        }
    }
}
