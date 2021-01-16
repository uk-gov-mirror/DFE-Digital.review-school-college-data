using System;
using System.Data.EntityClient;
using System.Linq;
using System.Transactions;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent : Logic.TSBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="student">A student EF object that contains a student change object.</param>
        /// <param name="completedStudentAdjustment">The details of the completed pupil adjustment.</param>
        public static int MoveStudent(Students studentIn, int dfesNumber, CompletedStudentAdjustment completedAdjustment, UserContext userContext)
        {

            try
            {
            
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {

                        using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope() )
                        {
                            //Create the change object.
                            Changes changeObj = CreateChangeObject(context, Contants.CHANGE_TYPE_ID_SCHOOL_AMENDMENT, userContext);
                            context.AddToChanges(changeObj);
                            context.SaveChanges();

                            //Get the original student.
                            Students studentToUpdate = context.Students
                                .Where(s => s.StudentID == studentIn.StudentID)
                                .Select(s => s)
                                .FirstOrDefault();

                            if (studentToUpdate == null)
                                throw Web09Exception.GetBusinessExceptionWithSingleParam(Web09MessageList.StudentNotFound, studentIn.StudentID.ToString());

                            //Create the new student record. This is effectively a manual clone of the student
                            //passed in, which should already exist in the database.
                            studentToUpdate.Schools = context.Schools.Where(sch => sch.DFESNumber == studentIn.Schools.DFESNumber).First();
                            studentToUpdate.Cohorts = context.Cohorts.Where(cht => cht.KeyStage == studentIn.Cohorts.KeyStage).First();
                            studentToUpdate.PINCLs = context.PINCLs.Where(p => p.P_INCL == studentIn.PINCLs.P_INCL).First();
                            studentToUpdate.OriginalStudentID = studentIn.OriginalStudentID;

                            //Apply a new forvus index if the current number is null or 0.
                            if (!studentToUpdate.ForvusIndex.HasValue || studentToUpdate.ForvusIndex.Value == 0)
                                AttachNewForvusIndexNumber(context, ref studentToUpdate, false);

                            //Create the student change object.
                            StudentChanges studentChangeInsert = CreateNewStudentChangeObjectForAddStudent(context, studentIn.StudentChanges.First(), changeObj);
                            StudentChanges studentChangeCurrent = context.StudentChanges
                                .Where(sc => sc.Students.StudentID == studentIn.StudentID && sc.DateEnd == null)
                                .FirstOrDefault();
                            studentChangeCurrent.DateEnd = DateTime.Now;

                            if(studentChangeCurrent == null)
                                throw Web09Exception.GetBusinessExceptionWithSingleParam(Web09MessageList.StudentNotFound, studentIn.StudentID.ToString());
                            
                            studentToUpdate.StudentChanges.Add(studentChangeInsert);
                            context.ApplyPropertyChanges("StudentChanges", studentChangeCurrent);
                            context.ApplyPropertyChanges("Students", studentToUpdate);
                            context.SaveChanges();
                            
                            //Process the Adjustment Request
                            if (completedAdjustment != null)
                            {
                                completedAdjustment.StudentID = studentChangeInsert.StudentID;
                                TSIncludeRemovePupil.SaveAdjustmentRequest(context, completedAdjustment, changeObj);
                            }

                            //Accept all changes
                            context.AcceptAllChanges();
                            transaction.Complete();

                            return studentChangeInsert.StudentID;
                         

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        
    }
}
