using System;
using System.Data.EntityClient;
using System.Linq;

using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent : Logic.TSBase
    {

        public static Ethnicities GetStudentEthnicity(string ethnicityCode)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    Ethnicities studentEthnicity = new Ethnicities();
                    var query = context.Ethnicities.Where(e => e.EthnicityCode == ethnicityCode).Select(e => e).ToList();

                    if(query.Count() == 0 )
                    {
                        throw new Exception();
                    }
                    else
                    {
                        studentEthnicity = query[0];
                    }

                    return studentEthnicity;

                }
            }
        }

        public static Languages GetStudentFirstLanguage(string languageCode) 
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    Languages studentFirstLanguage = context.Languages.Where(l => l.LanguageCode == languageCode).Select(l => l).First();

                    if (studentFirstLanguage == null)
                        throw new Exception();
                    
                    return studentFirstLanguage;
                }
            }
        }

        public static SENStatus GetStudentSENStatus(string senCode) 
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return GetStudentSENStatus(context, senCode);
                }
            }
        }

        public static SENStatus GetStudentSENStatus(Web09_Entities context, string senCode)
        {
            SENStatus studentSENStatus = new SENStatus();
            var query = context.SENStatus.Where(s => s.SENStatusCode == senCode).Select(s => s).ToList();

            if (query.Count() == 0)
            {
                studentSENStatus = null;
            }
            else
            {
                studentSENStatus = query[0];
            }

            return studentSENStatus;
        }

        public static StudentStatus GetStudentStatus(int studentStatusId) 
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    StudentStatus studentStatus = new StudentStatus();
                    var query = context.StudentStatus.Where(s => s.StudentStatusID == studentStatusId).Select(s => s).ToList();

                    if (query.Count() == 0)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        studentStatus = query[0];
                    }

                    return studentStatus;
                }
            }
        }

        internal static int? GetStudentCurrentNCYearGroup(Web09_Entities context, int studentID)
        {
            string yearGrpStr = context.StudentChanges.Where(sc => sc.StudentID == studentID).Select(sc => sc.YearGroups.YearGroupCode).First();
            int yearGrpInt;
            if (int.TryParse(yearGrpStr, out yearGrpInt))
                return yearGrpInt;
            else
                return null;
        }

        internal static string GetStudentAdmissionDate(Web09_Entities context, int studentId)
        {
            return context.StudentChanges
                .Where(sc => sc.StudentID == studentId && sc.DateEnd == null)
                .Select(sc => sc.ENTRYDAT)
                .First();
        }


        /// <summary>
        /// Overloaded where no context object is required
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        internal static string GetStudentAdmissionDate(int studentId)
        {
            using(EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using(Web09_Entities context = new Web09_Entities(conn))
                {
                    return GetStudentAdmissionDate(context, studentId);
                }
            }
        }

        internal static YearGroups GetStudentNCYearGroup(Web09_Entities context, int studentId)
        {
            return context.StudentChanges
                .Where(sc => sc.StudentID == studentId && sc.DateEnd == null)
                .Select(sc => sc.YearGroups)
                .First();
        }

        internal static string GetStudentDOB(Web09_Entities context, int studentId)
        {
            return context.StudentChanges
                .Where(sc => sc.StudentID == studentId && sc.DateEnd == null)
                .Select(sc => sc.DOB)
                .First();
        }

        public static DateTime? GetCurrentStudentAdmissionDate(Web09_Entities context, int studentId)
        {
            string admissionDateDBStr = context.StudentChanges
                        .Where(sc => sc.StudentID == studentId && sc.DateEnd == null)
                        .Select(sc => sc.ENTRYDAT).FirstOrDefault();

            if (!String.IsNullOrEmpty(admissionDateDBStr))
            {
                return TryConvertDateTimeDBString(admissionDateDBStr);
            }
            else
            {
                return null;
            }
        }

        public static DateTime? GetCurrentStudentAdmissionDate(int studentId)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return GetCurrentStudentAdmissionDate(context, studentId);
                }
            }
        }
    }
}
