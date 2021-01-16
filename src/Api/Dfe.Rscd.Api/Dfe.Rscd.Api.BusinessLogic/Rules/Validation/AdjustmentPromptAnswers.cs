using System;
using System.Collections.Generic;
using System.Linq;

using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.Business.Logic.TransactionScripts;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.Validation
{
    public class AdjustmentPromptAnswers : ValidationBase
    {

        public class AdjustmentPromptValidationFailure
        {
            public string Message;
            public int promptId;
        }

        public class AdjustmentPromptValidationResponse
        {
            public bool IsValid;
            public List<AdjustmentPromptValidationFailure> ValidationFailures;
        }

        public static AdjustmentPromptValidationResponse ValidateAdjustmentPromptAnswers(Students student, PromptAnswerList promptAnswers)
        {
            //Initialise the return object.
            bool isValid = true;
            List<AdjustmentPromptValidationFailure> validationFailures = new List<AdjustmentPromptValidationFailure>();
            
            string errMsg = "";

            foreach(PromptAnswer answer in promptAnswers)
            {
                switch (answer.ColumnType)
                {
                    case("ImmigrationDate"):

                        //Validate prompt Arrival Date
                        if (!IsValidArrivalDate(student, answer, promptAnswers, ref errMsg))
                        {
                            isValid = false;
                            validationFailures.Add(new AdjustmentPromptValidationFailure { Message = errMsg, promptId = answer.PromptID });
                        }
                        break;

                    case("PreviousSchool"):

                        //Validate prompt Excluding school
                        if(!IsValidExcludingSchool(answer, student, ref errMsg))
                        {
                            isValid = false;
                            validationFailures.Add(new AdjustmentPromptValidationFailure{ Message = errMsg, promptId = answer.PromptID });
                        }
                        break;

                    case("ExclusionDate"):

                        //validate exclusion date.
                        if(!IsValidExclusionDate(answer, ref errMsg))
                        {
                            isValid = false;
                            validationFailures.Add(new AdjustmentPromptValidationFailure{ Message = errMsg, promptId = answer.PromptID });
                        }
                        break;

                    case("OffrollDate"):

                        //Validate off role date.
                        if (!IsValidOffRoleDate(answer, ref errMsg))
                        {
                            isValid = false;
                            validationFailures.Add(new AdjustmentPromptValidationFailure { Message = errMsg, promptId = answer.PromptID });
                        }
                        break;

                    case("OnrollDate"):

                        //Validate admission date
                        if (!IsValidAdmissionDate(student, answer, ref errMsg))
                        {
                            isValid = false;
                            validationFailures.Add(new AdjustmentPromptValidationFailure { Message = errMsg, promptId = answer.PromptID });
                        }
                        break;

                    case ("NewForvusIndex"):

                        //Validate other forvus index number provided.
                        if (!IsValidForvusIndexNo(student, answer, ref errMsg))
                        {
                            isValid = false;
                            validationFailures.Add(new AdjustmentPromptValidationFailure { Message = errMsg, promptId = answer.PromptID });
                        }
                        break;

                    case ("ActualYearGroup"): 

                        //Validate Year group
                        if (!IsValidYearGroup(answer, ref errMsg))
                        {
                            isValid = false;
                            validationFailures.Add(new AdjustmentPromptValidationFailure { Message = errMsg, promptId = answer.PromptID });
                        }
                        break;

                    case("DoB"):
                        //Only captured via edit pupil, therefore already 
                        //validated in SubmitEditPupil service => NO VALIDATION REQUIRED
                    case(""):
                    default:
                        break;
                }
            }

            return new AdjustmentPromptValidationResponse
            {
                IsValid = isValid,
                ValidationFailures = validationFailures
            };
        }
                     
        private static bool IsValidAdmissionDate(Students student, PromptAnswer answer, ref string errMessage)
        {
            bool isValidDate = IsValidDateInput(answer, ref errMessage, "admission");

            if (!isValidDate)
            {
                //Return false, the error message will have already been set by the
                //IsValidDateInput field.
                return false;
            }
            else
            {
                
                if(student.StudentChanges.First() != null && student.StudentChanges.First().DOB != null)
                {
                    DateTime? studentDOB = TSStudent.TryConvertDateTimeDBString(student.StudentChanges.First().DOB);

                    if (answer.PromptDateTimeAnswer.HasValue && studentDOB.HasValue && 
                        (answer.PromptDateTimeAnswer.Value < studentDOB.Value))
                    {
                        errMessage = "Provided date is before date of birth";
                        return false;
                    }

                    if (answer.PromptDateTimeAnswer.HasValue && studentDOB.HasValue )
                    {
                        DateTime thirdBirthday = studentDOB.Value.AddYears(3);
                        if (answer.PromptDateTimeAnswer.Value < thirdBirthday)
                        {
                            errMessage = "Admission date provided makes pupil less than 3 years old when starting school";
                            return false;
                        }
                    }
                }

                //If this point is reached, the admission date is valid.
                return true;

            }
        }

        private static bool IsValidArrivalDate(Students student, PromptAnswer answer, PromptAnswerList allPromptAnswers, ref string errMessage)
        {
            if (!IsValidDateInput(answer, ref errMessage, "arrival"))
            {
                //Return false, the error message will have already been set by the
                //IsValidDateInput field.
                return false;
            }
            else
            {
                //Need to determine if the arrival date is after the admission date. A new admissino date may
                //have been provided by one of the other prompt answers. If this is the case, compare to this,
                //otherwise, use the admissiond date already assigned in the database. If no admission date exists,
                //pass the validation.

                DateTime? admissionDate = null;
                PromptAnswer admissionDateAnswer = allPromptAnswers.Where(a => a.ColumnType == "OnrollDate").FirstOrDefault();
                if (admissionDateAnswer != null &&
                    admissionDateAnswer.PromptDateTimeAnswer.HasValue &&
                    TSIncludeRemovePupil.IsPromptAnswerComplete(admissionDateAnswer))
                {
                    admissionDate = admissionDateAnswer.PromptDateTimeAnswer.Value;
                }
                else
                {
                    if (student.StudentChanges.First() != null &&
                        !String.IsNullOrEmpty(student.StudentChanges.First().ENTRYDAT))
                    {
                        admissionDate = TSStudent.TryConvertDateTimeDBString(student.StudentChanges.First().ENTRYDAT);
                    }
                }

                if (admissionDate.HasValue && answer.PromptDateTimeAnswer.HasValue && (answer.PromptDateTimeAnswer.Value > admissionDate.Value))
                {
                    errMessage = "The arrival date cannot be after the admission date.";
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

       
        private static bool IsValidExcludingSchool(PromptAnswer answer, Students student, ref string errMessage)
        {
            if ((!answer.PromptIntegerAnswer.HasValue && !answer.AllowNulls) || 
                (answer.PromptIntegerAnswer.HasValue && !TSSchool.IsRecognizedDCSF(answer.PromptIntegerAnswer.Value)))
            {
                errMessage = "The DfE number is not recognised.";
                return false;
            }
            else if ((student.Schools != null) && answer.PromptIntegerAnswer.Value == student.Schools.DFESNumber)
            {
                errMessage = "The excluding school cannot be the same as the student's current school.";
                return false;
            }
            else
            {
                return true;
            }
        }

        private static bool IsValidExclusionDate(PromptAnswer answer, ref string errMessage)
        {
            return IsValidDateInput(answer, ref errMessage, "exclusion");
        }

        private static bool IsValidOffRoleDate(PromptAnswer answer, ref string errMessage)
        {
            return IsValidDateInput(answer, ref errMessage, "off role");
        }

        private static bool IsValidForvusIndexNo(Students student, PromptAnswer answer, ref string errMessage)
        {
            if (student.Schools == null)
            {
                errMessage = "The given Forvus Index number cannot be verified as existing";
                return false;
            }
            if (!answer.PromptIntegerAnswer.HasValue && !answer.AllowNulls)
            {
                errMessage = "Please provide the Forvus Index number";
                return false;
            }
            if (answer.PromptIntegerAnswer.HasValue &&
                !TSStudent.IsValidForvusIndexNo(student.Schools.DFESNumber, answer.PromptIntegerAnswer.Value))
            {
                errMessage = "No pupil is listed for this school with the given Forvus Index number";
                return false;
            }
            return true;
        }

        private static bool IsValidYearGroup(PromptAnswer answer, ref string errMessage)
        {
            short yearGroupAnswer;
            if (!answer.PromptIntegerAnswer.HasValue && !answer.AllowNulls)
            {
                errMessage = "Please provide a numeric year group value";
                return false;
            }
            else if (!short.TryParse(answer.PromptIntegerAnswer.Value.ToString(), out yearGroupAnswer))
            {
                errMessage = "Please supply a valid year group";
                return false;
            }
            else if (answer.PromptIntegerAnswer.HasValue && !Student.IsValidYearGroupEntry(yearGroupAnswer))
            {
                errMessage = "Please supply a valid year group";
                return false;
            }
            else
            {
                return true;
            }
        }

        private static bool IsValidDateInput(PromptAnswer answer, ref string errMessage, string dateFieldDesc)
        {
            if (!answer.PromptDateTimeAnswer.HasValue && !answer.AllowNulls)
            {
                errMessage = "Please provide a valid " + dateFieldDesc + " date";
                return false;
            }
            else if (answer.PromptDateTimeAnswer.HasValue && answer.PromptDateTimeAnswer.Value > System.DateTime.Now)
            {
                errMessage = "Please provide a valid " + dateFieldDesc + " date that is in the past";
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
