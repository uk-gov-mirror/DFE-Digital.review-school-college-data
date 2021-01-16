using System;
using System.Data.EntityClient;
using System.Transactions;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.Business.Logic.Validation;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent
    {

        public static void IncludeRemovePupil(ref int pupilId, CompletedStudentAdjustment completedStudentAdjustment, UserContext userContext, out int studentRequestID)
        {
            using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope() )
            {

                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {

                        if (!Student.ValidateStudentHasNoPriorStudentRequests(context, pupilId))
                            throw new Exception(String.Format("A student adjustment request has already been created for student id {0}", pupilId.ToString()));

                        //Create the change object.
                        Changes changeObj = CreateChangeObject(context, Contants.CHANGE_TYPE_ID_SCHOOL_AMENDMENT, userContext);
                        context.AddToChanges(changeObj);
                        context.SaveChanges();

                        //Process the Adjustment Request
                        StudentRequests request = TSIncludeRemovePupil.SaveAdjustmentRequest(context, completedStudentAdjustment, changeObj);
                        studentRequestID = request.StudentRequestID;
                        //Submit all inserts also consider data concurreny issue while saving this request.
                        context.AcceptAllChanges();
                    }
                }

                transaction.Complete();
            }

            //return pupilId;
        }
    }
}
