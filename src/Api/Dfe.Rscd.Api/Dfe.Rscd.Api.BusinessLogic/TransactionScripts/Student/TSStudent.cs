using System;
using System.Data;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent : Logic.TSBase
    {
        public TSStudent()
        {
        }

        private static Changes CreateChangeObject(Web09_Entities context, int changeTypeId, UserContext userContext)
        {

            Changes changeObj = new Changes();

            //Get the Change Type
            var changeTypeQry = context.ChangeType.Where(ct => ct.ChangeTypeID == changeTypeId).Select(ct => ct).ToList();

            if(changeTypeQry.Count == 0)
                throw new Exception(String.Format("No change type exists with change type id of {0}", changeTypeId.ToString()));
            else
                changeObj.ChangeTypeID = changeTypeQry[0].ChangeTypeID;

            changeObj.ChangeDate = System.DateTime.Now;
            changeObj.RoleName = userContext.RoleName;
            changeObj.Surname = userContext.Surname;
            changeObj.Forename = userContext.Forename;
            changeObj.UserName = userContext.UserName;

            return changeObj;
        }

        public static short CalculateStudentAge(string strStudentDOB)
        {
            // Age Calculations from strig DOB
            // 20070505
            DateTime studentDOB = new DateTime(
                int.Parse(strStudentDOB.Substring(0,4)), 
                int.Parse(strStudentDOB.Substring(4,2)), 
                int.Parse(strStudentDOB.Substring(6,2))
                );            
            
            return CalculateStudentAge(studentDOB);

        }

        public static short CalculateStudentAge(DateTime studentDOB)
        {
            // Age Calculations
            int studentAge;

            int currentAcademicYear = int.Parse(TSCommonLists.GetKeyStageConfiguration(9)
                .Where(r => r.ConfigurationCode == Contants.COHORT_CONFIG_LOOKUP_CODE_TABLE_YEAR).FirstOrDefault().ConfigurationValue) - 1;

            DateTime lastAcademicYearDate = new DateTime(currentAcademicYear, 9, 1);
            if (studentDOB.AddYears(lastAcademicYearDate.Year - studentDOB.Year) >= lastAcademicYearDate)            
                studentAge = currentAcademicYear - studentDOB.Year - 1;
            else
                studentAge = Math.Abs(currentAcademicYear - studentDOB.Year);

            return (short)studentAge;
            
        }


        // with context
        public static short CalculateStudentAge( Web09_Entities context, DateTime studentDOB)
        {
            // Age Calculations
            int studentAge;

            CohortConfiguration configValue = context.CohortConfiguration.Where(r => r.ConfigurationCode == Contants.COHORT_CONFIG_LOOKUP_CODE_TABLE_YEAR).FirstOrDefault();

            int currentAcademicYear = int.Parse(configValue.ConfigurationValue) - 1;

            DateTime lastAcademicYearDate = new DateTime(currentAcademicYear, 9, 1);
            if (studentDOB.AddYears(lastAcademicYearDate.Year - studentDOB.Year) >= lastAcademicYearDate)
                studentAge = currentAcademicYear - studentDOB.Year - 1;
            else
                studentAge = Math.Abs(currentAcademicYear - studentDOB.Year);

            return (short)studentAge;            
        }        

        public static DateTime GetStudentDOBDateTime(string dob)
        {

            int numericDate;
            if (Int32.TryParse(dob, out numericDate) && numericDate >= 10000000 && numericDate <= 99999999)
            {
                DateTime dobReturn = new DateTime(Convert.ToInt16(dob.Substring(0, 4)),
                    Convert.ToInt16(dob.Substring(4, 2)),
                    Convert.ToInt16(dob.Substring(6, 2)));
                return dobReturn;
            }
            else
            {
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);
            }
        }

        internal static bool HasPriorResults(Web09_Entities context, int studentId, short currentStudentKS)
        {
            if (studentId == 0)
            {
                return false;
            }
            else
            {
                if (currentStudentKS == 2)
                {
                    if((
                            from stdResult in context.Results
                            where
                            stdResult.QualificationTypes.QualificationTypeCollections.Any(t => t.QualificationTypeCollectionCode.StartsWith("KS1"))
                            && stdResult.Students.StudentID == studentId
                            select stdResult
                        ).Any()
                      )
                        return true;
                    else
                        return false;
                }
                else //Key stage must be either 4 or five, therefore check for prior results.
                {
                    int priorResultsCount = context.Results
                        .Where(r => r.Students.StudentID == studentId &&
                            (
                                r.SubLevels.SubLevelCode == "500" ||
                                r.SubLevels.SubLevelCode == "550" ||
                                r.SubLevels.SubLevelCode == "600" ||
                                r.SubLevels.SubLevelCode == "650" ||
                                r.SubLevels.SubLevelCode == "700"
                            ))
                        .Count();

                    if (priorResultsCount > 0)
                        return true;
                    else
                        return false;
                }
                
            }
        }

        internal static bool HasKS2FutureResults(Web09_Entities context, int studentId)
        {
            int ks2FutureResultsCount = context.ResultChanges
                .Where(rc => rc.Results.Students.StudentID == studentId && rc.DateEnd == null && 
                    rc.Points.GradeDescription.ToLower().Equals("pupil to take test in future"))
                .Count();

            return (ks2FutureResultsCount > 0);
        }

        internal static bool AreAllKS2ResultsUndiscountedMResults(int studentId)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return AreAllKS2ResultsUndiscountedMResults(context, studentId);
                }
            }
        }

        internal static bool AreAllKS2ResultsUndiscountedMResults(Web09_Entities context, int studentId)
        {
            try
            {
                //  Retrieve the count of results, and the count of undiscounted M results
                int ks2UndiscountedMResultsCount = context.ResultChanges
                                .Where(rc => rc.Results.Students.StudentID == studentId && 
                                    rc.DateEnd == null && 
                                    rc.Results.RINCLs.R_INCL >= 20 && rc.Results.RINCLs.R_INCL <= 29 && 
                                    rc.Results.RINCLs.DisplayFlag.Trim() == "√" && 
                                    rc.Points.GradeCode.ToLower() == "m").ToList().Count();

                int allKS2UndiscountedResultsCount = context.Results
                    .Where(r => r.Students.StudentID == studentId &&
                        r.RINCLs.R_INCL >= 20 && r.RINCLs.R_INCL <= 29 &&
                        r.RINCLs.DisplayFlag.Trim() == "√").Count();

                //  Compare the counts. If they match, all results are undiscounted M results
                if (ks2UndiscountedMResultsCount == allKS2UndiscountedResultsCount)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Determine if a student record exists for a given studentId
        /// </summary>
        /// <param name="context">The Web09 Entity Framework object context.</param>
        /// <param name="studentId">The student id for which the check will be performed.</param>
        /// <returns>Boolean, true indicating student record does exists</returns>
        internal static bool IsStudentListed(Web09_Entities context, int studentId)
        {
            if (studentId == 0) return false;

            int recognisedStudentCount = context.Students
                .Where(s => s.StudentID == studentId)
                .Count();

            if (recognisedStudentCount == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Determine if a student record exists for a given studentId. Overloaded, does not required
        /// context object.
        /// </summary>
        /// <param name="studentId">The student id for which the check will be performed.</param>
        /// <returns>Boolean, true indicating student record does exists</returns>
        internal static bool IsStudentListed(int studentId)
        {

            if(studentId == 0) return false;

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return IsStudentListed(context, studentId);
                }
            }
        }

        /// <summary>
        /// Determine if a student exists with a given forvus index number for a particular school. Overloaded, no context
        /// object is required.
        /// </summary>
        /// <param name="dfesNumber">The dfes number of the school to whom the pupil is being tested to belong</param>
        /// <param name="forvusIndex">The forvus index number to be validated.</param>
        /// <returns></returns>
        internal static bool IsValidForvusIndexNo(int dfesNumber, int forvusIndex)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();
                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return IsValidForvusIndexNo(context, dfesNumber, forvusIndex);
                }
            }
        }

        /// <summary>
        /// Determine if a student exists with a given forvus index number for a particular school
        /// </summary>
        /// <param name="context">Web09 entites framework object context</param>
        /// <param name="dfesNumber">The dfes number of the school to whom the pupil is being tested to belong</param>
        /// <param name="forvusIndex">The forvus index number to be validated.</param>
        /// <returns></returns>
        internal static bool IsValidForvusIndexNo(Web09_Entities context, int dfesNumber, int forvusIndex)
        {
            int studentCount = context.Students
                .Where(s => s.Schools.DFESNumber == dfesNumber && s.ForvusIndex == forvusIndex)
                .Count();

            if (studentCount != 1)
                return false;
            else
                return true;

        }

        
        /// <summary>
        /// Determine if a student has a current attainment at Level 2 including English and maths
        /// </summary>
        /// <param name="context"></param>
        /// <param name="studentId">The ID of the student for whom the check is being performed.</param>
        /// <returns></returns>
        internal static bool IsCurrentAttainmentLev2ForEnglishAndMaths(Web09_Entities context, int studentId)
        {
            StudentValues studentLev2EMValue = context.StudentValues
                .Where(sv => sv.StudentID == studentId && sv.ValueTypeID == Contants.STUDENT_VALUE_TYPE_ID_LEV2EM)
                .Select(sv => sv)
                .FirstOrDefault();

            if (studentLev2EMValue != null && studentLev2EMValue.Value == "1")
                return true;
            else
                return false;
        }       

        public static String GetIDACIValue(String postCode)
        {
            string idaciValue = "N/A";

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                var connection = conn.StoreConnection;
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[Student].[GetIDACI]";
                    SetInputParamForCommand(cmd, "PostCode", postCode);

                    Object returnObj = cmd.ExecuteScalar();
                    if (returnObj != null)
                    {
                        double idaciDouble;
                        if (Double.TryParse(returnObj.ToString(), out idaciDouble))
                        {
                            idaciValue = Math.Round(idaciDouble, 2).ToString();
                        }                           
                    }
                }

            }

            return idaciValue;          
        }

        public static bool HasResultAmendments(int studentID)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return HasResultAmendments(studentID, context);
                }
            }
        }

        public static bool HasResultAmendments(int studentID, Web09_Entities context)
        {
            try                
            {
                var result = from rc in context.ResultChanges
                             where rc.Results.Students.StudentID == studentID
                             && rc.DateEnd == null
                             &&
                             (
                             rc.ResultStatus.ResultStatusDescription == "Amended"
                             ||
                             rc.ResultStatus.ResultStatusDescription == "Added"
                             ||
                             rc.ResultStatus.ResultStatusDescription == "Withdrawn"
                             )
                             select rc;

                    if (result.Count() > 0)
                        return true;
                    else
                        return false;
            }
            catch(Exception     ex)
            {
                throw ex;
            }                
        }

        internal static DateTime ConvertDateTimeDBString(string dateTimeString)
        {
            short year;
            short month;
            short day;

            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
            System.Globalization.DateTimeStyles styles = System.Globalization.DateTimeStyles.None;
            DateTime returnDateValue;

            if (!String.IsNullOrEmpty(dateTimeString) &&
                short.TryParse(dateTimeString.Substring(0, 4), out year) &&
                short.TryParse(dateTimeString.Substring(4, 2), out month) &&
                short.TryParse(dateTimeString.Substring(6, 2), out day) &&
                DateTime.TryParse(day.ToString() + "/" + month.ToString() + "/" + year.ToString(), culture, styles, out returnDateValue))
            {
                return returnDateValue;
            }
            else
            {
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);
            }
            
        }

        public static DateTime? TryConvertDateTimeDBString(string dateTimeString)
        {
            short year;
            short month;
            short day;

            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
            System.Globalization.DateTimeStyles styles = System.Globalization.DateTimeStyles.None;
            DateTime returnDateValue;

            if (!String.IsNullOrEmpty(dateTimeString) &&
                dateTimeString.Length == 8 &&
                short.TryParse(dateTimeString.Substring(0, 4), out year) &&
                short.TryParse(dateTimeString.Substring(4, 2), out month) &&
                short.TryParse(dateTimeString.Substring(6, 2), out day))
                
            {
                if (DateTime.TryParse(day.ToString() + "/" + month.ToString() + "/" + year.ToString(), culture, styles, out returnDateValue))
                    return returnDateValue;
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        internal static void AttachNewForvusIndexNumber(Web09_Entities context, ref Students student, bool isNewStudent)
        {
            int dfesNumber = student.Schools.DFESNumber;
            int keyStage = student.Cohorts.KeyStage;
            int forvusIndexRangeMin = (isNewStudent) ? 90000 : 80000;
            int forvusIndexRangeMax = (isNewStudent) ? 99999 : 89999;

            //Assign a new Forvus index number by incrementing the
            //last used forvus index by 1
            int? currentMaxForvusIndex = context.Students
                .Where(s => s.Schools.DFESNumber == dfesNumber && s.Cohorts.KeyStage == keyStage &&
                    s.ForvusIndex > forvusIndexRangeMin && s.ForvusIndex < forvusIndexRangeMax)
                .Select(s => s.ForvusIndex)
                .Max();

            if (currentMaxForvusIndex.HasValue)
                student.ForvusIndex = currentMaxForvusIndex + 1;
            else
                if (keyStage == 2)
                    student.ForvusIndex = forvusIndexRangeMin + 2001;
                else if (keyStage == 4)
                    student.ForvusIndex = forvusIndexRangeMin + 4001;
                else if (keyStage == 5)
                    student.ForvusIndex = forvusIndexRangeMin + 5001;
        }

        internal static IDACI GetPostCode(Web09_Entities context, ref StudentChanges studentChange)
        {
            string postCode = studentChange.PostCode.Trim();

            if (!postCode.Contains(" "))
            {
                switch (postCode.Length)
                {
                    case (5):
                        postCode = postCode.Substring(0, 2) + " " + postCode.Substring(2, 3);
                        studentChange.PostCode = postCode;
                        break;
                    case (6):
                        postCode = postCode.Substring(0, 3) + " " + postCode.Substring(3, 3);
                        studentChange.PostCode = postCode;
                        break;
                    case (7):
                        postCode = postCode.Substring(0, 4) + " " + postCode.Substring(4, 3);
                        studentChange.PostCode = postCode;
                        break;
                    default:
                        return null;
                }
            }
            return context.IDACI.FirstOrDefault(i => i.PostCode == postCode);
        }
                    
    }
}