using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent
    {

        #region ValidatePupil overloaded methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="student">A student that encapsulates the latest student change to be validated</param>
        /// <returns></returns>
        public static Web09.Checking.Business.Logic.Validation.Student.ValidationResponse ValidatePupil(ref Students student, bool isNewStudent, StudentAdjustmentType? adjustmentType, CompletedStudentAdjustment completedAdjustment, List<Validation.Student.ValidationFailure> acknowledgedWarnings)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return ValidatePupil(context, ref student, isNewStudent, adjustmentType, completedAdjustment, acknowledgedWarnings);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="student">A student that encapsulates the latest student change to be validated</param>
        /// <returns></returns>
        public static Web09.Checking.Business.Logic.Validation.Student.ValidationResponse ValidatePupilDA(ref Students student, bool isNewStudent, StudentAdjustmentType? adjustmentType, CompletedStudentAdjustment completedAdjustment, List<Validation.Student.ValidationFailure> acknowledgedWarnings)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return ValidatePupilDA(context, ref student, isNewStudent, adjustmentType, completedAdjustment, acknowledgedWarnings);
                }
            }
        }
        /// <summary>
        /// Overloaded method to validate pupil, no context provided, and no acknowledged warnings exist.
        /// </summary>
        /// <param name="student">The student to be validated.</param>
        /// <param name="isNewStudent">Boolean set to true to indicate if student is a new student to be inserted.</param>
        /// <returns>Validation response containing errors and warning messages.</returns>
        public static Web09.Checking.Business.Logic.Validation.Student.ValidationResponse ValidatePupil(ref Students student, bool isNewStudent)
        {
            return ValidatePupil(ref student, isNewStudent, null, null, null);
        }

        /// <summary>
        /// Overloaded method to validate pupil, no context provided, and no adjustment request exists,
        /// but a list of previously acknowledged warnings is provided..
        /// </summary>
        /// <param name="student">Student to be validated</param>
        /// <param name="isNewStudent">Boolean value to indicate if the student is a new record to be created.</param>
        /// <param name="acknowledgedWarnings">Any warning messages that have already been acknowledged</param>
        /// <returns>Validation response containing errors and warning messages not previously acknowledged.</returns>
        public static Web09.Checking.Business.Logic.Validation.Student.ValidationResponse ValidatePupil(ref Students student, bool isNewStudent, List<Validation.Student.ValidationFailure> acknowledgedWarnings)
        {
            return ValidatePupil(ref student, isNewStudent, null, null, acknowledgedWarnings);
        }

        /// <summary>
        /// Overloaded method to validate pupil, no context provided, and no adjustment request exists,
        /// but a list of previously acknowledged warnings is provided..
        /// </summary>
        /// <param name="student">Student to be validated</param>
        /// <param name="isNewStudent">Boolean value to indicate if the student is a new record to be created.</param>
        /// <param name="acknowledgedWarnings">Any warning messages that have already been acknowledged</param>
        /// <returns>Validation response containing errors and warning messages not previously acknowledged.</returns>
        public static Web09.Checking.Business.Logic.Validation.Student.ValidationResponse ValidatePupilDA(ref Students student, bool isNewStudent, List<Validation.Student.ValidationFailure> acknowledgedWarnings)
        {
            return ValidatePupilDA(ref student, isNewStudent, null, null, acknowledgedWarnings);
        }

        /// <summary>
        /// Overloaded method to Validate pupil with context provided
        /// </summary>
        /// <param name="context"></param>
        /// <param name="student"></param>
        /// <param name="isNewStudent"></param>
        /// <param name="adjustmentType"></param>
        /// <param name="completedAdjustment"></param>
        /// <param name="acknowledgedWarnings"></param>
        /// <returns></returns>
        public static Web09.Checking.Business.Logic.Validation.Student.ValidationResponse ValidatePupil(Web09_Entities context, ref Students student, bool isNewStudent, StudentAdjustmentType? adjustmentType, CompletedStudentAdjustment completedAdjustment, List<Validation.Student.ValidationFailure> acknowledgedWarnings)
        {
            return Logic.Validation.Student.ValidateStudent(context, ref student, isNewStudent, adjustmentType, completedAdjustment, acknowledgedWarnings);
        }

        /// <summary>
        /// Overloaded method to Validate pupil with context provided
        /// </summary>
        /// <param name="context"></param>
        /// <param name="student"></param>
        /// <param name="isNewStudent"></param>
        /// <param name="adjustmentType"></param>
        /// <param name="completedAdjustment"></param>
        /// <param name="acknowledgedWarnings"></param>
        /// <returns></returns>
        public static Web09.Checking.Business.Logic.Validation.Student.ValidationResponse ValidatePupilDA(Web09_Entities context, ref Students student, bool isNewStudent, StudentAdjustmentType? adjustmentType, CompletedStudentAdjustment completedAdjustment, List<Validation.Student.ValidationFailure> acknowledgedWarnings)
        {
            return Logic.Validation.Student.ValidateStudentDA(context, ref student, isNewStudent, adjustmentType, completedAdjustment, acknowledgedWarnings);
        }

        #endregion

        #region Validate Student Search Parameters

        public static Web09.Checking.Business.Logic.Validation.Student.ValidationResponse ValidateStudentSearchParams(string forename, string surname, DateTime dob, string gender)
        {
            return Logic.Validation.Student.ValidateStudentSearchParameters(forename, surname, dob, gender);
        }

        #endregion
    }
}
