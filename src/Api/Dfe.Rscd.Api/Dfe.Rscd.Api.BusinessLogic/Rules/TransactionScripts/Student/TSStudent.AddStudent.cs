using System.Data.EntityClient;
using System.Linq;
using System.Transactions;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent : Logic.TSBase
    {

        public static int AddNewStudent(Students studentIn, CompletedStudentAdjustment completedAdjustment, UserContext userContext)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {

                    using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope() )
                    {

                        Changes changeObj = CreateChangeObject(context, Contants.CHANGE_TYPE_ID_SCHOOL_AMENDMENT, userContext);
                        context.AddToChanges(changeObj);
                        context.SaveChanges();

                        //Create the new student object.
                        Students studentInsert = new Students
                        {
                            Schools = context.Schools.Where(sch => sch.DFESNumber == studentIn.Schools.DFESNumber).First(),
                            Cohorts = context.Cohorts.Where(cht => cht.KeyStage == studentIn.Cohorts.KeyStage).First(),
                            PINCLs = context.PINCLs.Where(pincl => pincl.P_INCL == studentIn.PINCLs.P_INCL).First(),
                            DataOrigin = context.DataOrigin.Where(dorgn => dorgn.DataOriginID == Contants.DATA_ORIGIN_ID_USER_ADDITION).First()
                        };

                        //Assign a new Forvus index number by incrementing the
                        //last used forvus index by 1
                        AttachNewForvusIndexNumber(context, ref studentInsert, true);

                        //Create the student change object.
                        StudentChanges scInsert = CreateNewStudentChangeObjectForAddStudent(context, studentIn.StudentChanges.First(), changeObj);

                        //Attach the new student change object to the new parent student object
                        //and add to the database.
                        studentInsert.StudentChanges.Add(scInsert);
                        context.AddToStudents(studentInsert);
                        context.SaveChanges();

                        //Save the adjustment.
                        if (completedAdjustment != null)
                        {
                            completedAdjustment.StudentID = scInsert.StudentID;
                            completedAdjustment.ForvusId = scInsert.Students.ForvusIndex ?? 0;
                            TSIncludeRemovePupil.SaveAdjustmentRequest(context, completedAdjustment, changeObj);
                        }

                        //Accept all changes, close the transaction, and return the
                        //new student id.
                        context.AcceptAllChanges();
                        transaction.Complete();
                        return studentInsert.StudentID;
                    }

                };
            }
        }


        private static StudentChanges CreateNewStudentChangeObjectForAddStudent(Web09_Entities context, StudentChanges studentChangeIn, Changes changeObj)
        {
            StudentChanges scNewInsert = new StudentChanges();
            scNewInsert.Changes = changeObj;
            scNewInsert.Surname = studentChangeIn.Surname;
            scNewInsert.Forename = studentChangeIn.Forename;
            scNewInsert.Gender = studentChangeIn.Gender;
            scNewInsert.DOB = studentChangeIn.DOB;
            scNewInsert.Age = studentChangeIn.Age;
            scNewInsert.PostCode = studentChangeIn.PostCode;
            scNewInsert.ENTRYDAT = studentChangeIn.ENTRYDAT;
            scNewInsert.YearGroups = context.YearGroups.Where(yg => yg.YearGroupCode == studentChangeIn.YearGroups.YearGroupCode).FirstOrDefault();
            scNewInsert.UPN = studentChangeIn.UPN;

            if(studentChangeIn.Ethnicities != null)
                scNewInsert.Ethnicities = context.Ethnicities.Where(e => e.EthnicityCode == studentChangeIn.Ethnicities.EthnicityCode).First();

            scNewInsert.FSM = studentChangeIn.FSM;

            if(studentChangeIn.Languages != null)
                scNewInsert.Languages = context.Languages.Where(l => l.LanguageCode == studentChangeIn.Languages.LanguageCode).First();

            if(studentChangeIn.SENStatus != null)
                scNewInsert.SENStatus = context.SENStatus.Where(sen => sen.SENStatusCode == studentChangeIn.SENStatus.SENStatusCode).First();

            scNewInsert.LookedAfterEver = studentChangeIn.LookedAfterEver;
            scNewInsert.AMDFlag = (studentChangeIn.AMDFlag != null) ? studentChangeIn.AMDFlag : " ";
            scNewInsert.NORFLAGE = context.NORFLAGE.FirstOrDefault(nf => nf.NORFLAGE1 == Contants.NORFLAGE_ID_PUPIL_ADDED);
            scNewInsert.StudentStatus = context.StudentStatus.Where(ss => ss.StudentStatusID == Contants.STUDENT_STATUS_ID_ADDED).First();

            return scNewInsert;

        }

       
    }
}
