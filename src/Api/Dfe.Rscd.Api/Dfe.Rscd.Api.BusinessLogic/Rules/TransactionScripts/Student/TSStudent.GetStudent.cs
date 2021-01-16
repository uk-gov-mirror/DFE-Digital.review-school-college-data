using System;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent : Logic.TSBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="latest">Latest or UnAmended</param>
        /// <returns></returns>
        public static Students GetStudent(int studentId)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {                
                conn.Open();

                try
                {

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        return GetStudent(context, studentId, true);
                    }
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="latest">Latest or UnAmended</param>
        /// <returns></returns>
        public static Students GetStudent(int studentId, bool latest)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                try
                {

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        return GetStudent(context, studentId, latest);
                    }
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private static Students GetStudent(Web09_Entities context, int studentID)
        {
            return GetStudent(context, studentID, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="studentID"></param>
        /// <param name="latest">Latest or UnAmended</param>
        /// <returns></returns>
        private static Students GetStudent(Web09_Entities context, int studentID, bool latest)
        {
            try
            {

                //  Retrieve the pupil's details.               
                Students newStudent = context.Students
                    .Include("Schools")
                    .Include("PINCLs")
                    .Include("Cohorts")
                    .Where(s => s.StudentID == studentID)
                    .Select(s => s)
                    .FirstOrDefault();

                //  Throw error if no student found.
                if (newStudent == null)
                    throw Web09Exception.GetBusinessException(Web09MessageList.StudentInvalidID);


                StudentChanges studentChange = new StudentChanges();

                if (latest)
                {
                    studentChange  = context.StudentChanges
                        .Include("Ethnicities")
                        .Include("YearGroups")
                        .Include("Languages")
                        .Include("SENStatus")
                        .Where(
                        s => s.StudentID == studentID
                            && s.DateEnd == null
                        )
                        .FirstOrDefault();                        
                }
                else
                {
                    studentChange = context.StudentChanges
                        .Include("Ethnicities")
                        .Include("YearGroups")
                        .Include("Languages")
                        .Include("SENStatus")
                        .Where(
                        s => s.StudentID == studentID
                        )
                        .OrderBy(s => s.ChangeID)
                        .FirstOrDefault();                        
                }

                //  Throw error if no student found.
                if (studentChange == null)
                    throw Web09Exception.GetBusinessException(Web09MessageList.StudentInvalidID);

                newStudent.StudentChanges.Attach(studentChange);

                return studentChange.Students;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        

        /// <summary>
        /// Retrieve the first language for a given student currently assigned in the database.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        internal static Languages GetCurrentFirstLanguage(Web09_Entities context, int studentId)
        {
            return context.StudentChanges
                .Where(sc => sc.StudentID == studentId && sc.DateEnd == null)
                .Select(sc => sc.Languages)
                .FirstOrDefault();
        }

        /// <summary>
        /// Determine if a student has an outstanding adjustment request (ie a request with a scrutiny
        /// status other than cancelled).
        /// </summary>
        /// <param name="studentId">The id of the student for whom the check is being performed.</param>
        /// <returns>Boolean, true to indicate an outstanding adjustment request exists.</returns>
        public static bool DoesStudentHaveOutstandingAdjustment(Web09_Entities context, int studentId)
        {
            int numStudentRequests = context.StudentRequestChanges
                        .Where(src => src.StudentRequests.Students.StudentID == studentId && src.DateEnd == null && src.ScrutinyStatus.ScrutinyStatusCode.ToLower() != "c")
                        .Count();

            if (numStudentRequests > 0)
                return true;
            else
                return false;
                
        }

        /// <summary>
        /// Determine if a student has an outstanding adjustment request (ie a request with a scrutiny
        /// status other than cancelled). Overloaded method where no entity framework object context exists.
        /// </summary>
        /// <param name="studentId">The id of the student for whom the check is being performed.</param>
        /// <returns>Boolean, true to indicate an outstanding adjustment request exists.</returns>
        public static bool DoesStudentHaveOutstandingAdjustment(int studentId)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();
                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return DoesStudentHaveOutstandingAdjustment(context, studentId);
                }
            }
        }


        public static void LoadStudentInfoForTranslator(int studentId, string PostCode, ref string IDACIValue, ref bool hasResultAmendments, ref bool hasOutstandingRequest)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();
                using (Web09_Entities context = new Web09_Entities(conn))
                {                    
                    hasResultAmendments = TSStudent.HasResultAmendments(studentId, context);
                    IDACIValue = TSStudent.GetIDACIValue(PostCode);
                    hasOutstandingRequest = TSStudent.DoesStudentHaveOutstandingAdjustment(context, studentId);
                }
            }
        }


    }
}
