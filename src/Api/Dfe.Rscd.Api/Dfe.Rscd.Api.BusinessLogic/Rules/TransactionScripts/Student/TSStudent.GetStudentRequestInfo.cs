using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Globalization;
using System.Linq;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.Business.Logic.Validation;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent : Logic.TSBase
    {
        public static CompletedStudentAdjustment GetStudentRequestInfo(int studentRequestID)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    StudentRequests sr = context.StudentRequests
                        .Include("Students")
                        .Include("InclusionAdjustmentReasons")
                        .Include("Reasons")
                        .Where(s => s.StudentRequestID == studentRequestID).FirstOrDefault();

                    if (sr == null)
                        throw new BusinessLevelException("Student request not found");

                    StudentRequestChanges src = context.StudentRequestChanges
                        .Include("Reasons")
                        .Include("ScrutinyStatus")
                        .Where(s => s.StudentRequestID == studentRequestID && s.DateEnd == null)
                        .FirstOrDefault();

                    if (src == null)
                        throw new BusinessLevelException("Student request changes not found");

                    PromptAnswerList aList = new PromptAnswerList();
                    StudentRequests studentRequest = null;
                    Students student = null;
                    StudentChanges studentCurrentChange = null;
                    StudentChanges studentOriginalChange = null;
                    Reasons requestReasons = null;

                    var queryREQ = from sreq in context.StudentRequests
                                   where sreq.StudentRequestID == studentRequestID
                                   select new
                                   {
                                       StudentRequest = sreq,
                                       StudentRequestReason = sreq.Reasons,
                                       StudentMainInfo = sreq.Students,
                                       StudentCurrentInfo = sreq.Students.StudentChanges.Where(std => std.DateEnd == null).FirstOrDefault(),
                                       StudentOriginalInfo = sreq.Students.StudentChanges.OrderBy(std => std.ChangeID).FirstOrDefault()
                                   };

                    foreach (var data in queryREQ)
                    {
                        studentRequest = data.StudentRequest;
                        student = data.StudentMainInfo;
                        studentCurrentChange = data.StudentCurrentInfo;
                        studentOriginalChange = data.StudentOriginalInfo;
                        requestReasons = data.StudentRequestReason;
                    }

                    var query = from srdObj in context.StudentRequestData
                                where srdObj.StudentRequestID == studentRequestID
                                select new
                                {
                                    REQ = srdObj,
                                    PROMPTS = srdObj.Prompts,
                                    PROMPTTYPES = srdObj.Prompts.PromptTypes,
                                    StudentRequest = srdObj.StudentRequests,
                                    StudentRequestReason = srdObj.StudentRequests.Reasons,
                                    StudentMainInfo = srdObj.StudentRequests.Students,
                                    StudentCurrentInfo = srdObj.StudentRequests.Students.StudentChanges.Where(std => std.DateEnd == null).FirstOrDefault(),
                                    StudentOriginalInfo = srdObj.StudentRequests.Students.StudentChanges.OrderBy(std => std.ChangeID).FirstOrDefault(),
                                };

                    foreach (var data in query)
                    {
                        PromptAnswer pa = new PromptAnswer(data.REQ.PromptID, data.REQ.Prompts.PromptText, data.REQ.Prompts.PromptShortText, data.REQ.UpdateByDA);

                        pa.PromptAnswerType = (PromptAnswer.PromptAnswerTypeEnum)data.PROMPTTYPES.PromptTypeID;

                        switch (data.PROMPTTYPES.PromptTypeID)
                        {
                            case (short)PromptAnswer.PromptAnswerTypeEnum.Date:
                                if (data.REQ.PromptValue != "")
                                    pa.PromptDateTimeAnswer = new DateTime(int.Parse(data.REQ.PromptValue.Substring(0, 4)), int.Parse(data.REQ.PromptValue.Substring(4, 2)), int.Parse(data.REQ.PromptValue.Substring(6, 2)));
                                else
                                    pa.PromptDateTimeAnswer = null;

                                break;
                            case (short)PromptAnswer.PromptAnswerTypeEnum.Info:
                                break;
                            case (short)PromptAnswer.PromptAnswerTypeEnum.Integer:

                                if (data.REQ.PromptValue != "")
                                    pa.PromptIntegerAnswer = (int?)int.Parse(data.REQ.PromptValue);
                                else
                                    pa.PromptIntegerAnswer = null;

                                break;

                            case (short)PromptAnswer.PromptAnswerTypeEnum.ListBox:

                                if (data.REQ.PromptValue != "")
                                {
                                    int listOrder = 0;
                                    if (int.TryParse(data.REQ.PromptValue, out listOrder))
                                        pa.PromptSelectedValueAnswer = context.PromptResponses.Where(pr => pr.PromptID == data.REQ.PromptID && pr.ListOrder == listOrder).FirstOrDefault().ListValue;
                                    else
                                        pa.PromptSelectedValueAnswer = "";
                                }
                                break;

                            case (short)PromptAnswer.PromptAnswerTypeEnum.Text:
                                pa.PromptStringAnswer = data.REQ.PromptValue;
                                break;

                            case (short)PromptAnswer.PromptAnswerTypeEnum.YesNo:

                                bool? answer = true;

                                if (data.REQ.PromptValue.ToLower() == "yes")
                                    answer = true;
                                else
                                    answer = false;

                                pa.PromptYesNoAnswer = answer;
                                break;
                        };

                        aList.Add(pa);
                    }

                    aList = SetStudentRequestInfoPromptMessages(
                        context,
                        aList,
                        studentRequest,
                        student,
                        studentCurrentChange,
                        studentOriginalChange,
                        requestReasons
                        );
                    

                    int? rejectionReason;
                    if (sr.Reasons != null && sr.Reasons.IsRejection.HasValue && sr.Reasons.IsRejection.Value)
                        rejectionReason = sr.Reasons.ReasonID;
                    else
                        rejectionReason = null;

                    CompletedStudentAdjustment csa = new CompletedStudentAdjustment
                        (
                         sr.Students.StudentID,
                         sr.InclusionAdjustmentReasons.IncAdjReasonID,
                         aList,// answerlist
                         sr.Reasons.ReasonID,
                         rejectionReason,
                         src.ScrutinyStatus.ScrutinyStatusCode,
                         ""
                        );

                    // Pupil Field level messages warning, errors and information
                    csa.InformationMessageList = SetStudentRequestInfoFieldInformationMessages(
                        context,
                        aList,
                        studentRequest,
                        student,
                        studentCurrentChange,
                        studentOriginalChange,
                        requestReasons
                        );

                    csa.WarningMessageList = SetStudentRequestInfoFieldWarningMessages(
                        context,
                        aList,
                        studentRequest,
                        student,
                        studentCurrentChange,
                        studentOriginalChange,
                        requestReasons
                        );


                    csa.ErrorMessageList = SetStudentRequestInfoFieldErrorMessages(
                        context,
                        aList,
                        studentRequest,
                        student,
                        studentCurrentChange,
                        studentOriginalChange,
                        requestReasons
                        );

                    return csa;
                }
            }
        }

        public static CompletedStudentAdjustment GetStudentRequestInfoByStudentID(int studentId)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {

                    int? studentRequestId = context.StudentRequestChanges
                        .Where(src => src.StudentRequests.Students.StudentID == studentId && src.DateEnd == null && src.ScrutinyStatus.ScrutinyStatusCode != Contants.SCRUTINY_STATUS_CANCELLED)
                        .Select(src => src.StudentRequests.StudentRequestID)
                        .FirstOrDefault();

                    if (!studentRequestId.HasValue)
                        throw new BusinessLevelException("Student request not found");

                    StudentRequests studentRequest = context.StudentRequests
                        .Include("InclusionAdjustmentReasons")
                        .Include("Reasons")
                        .Where(sr => sr.StudentRequestID == studentRequestId.Value)
                        .Select(sr => sr)
                        .FirstOrDefault();

                    //Add the latest StudentRequestChange to the student request. Be certain to retrieve the latest change
                    //for the current student request, ie where the status is not Cancelled.
                    StudentRequestChanges studentRequestCurrentChange = context.StudentRequestChanges
                        .Include("ScrutinyStatus")
                        .Where(src => src.StudentRequests.StudentRequestID == studentRequest.StudentRequestID)
                        .OrderBy(src => src.DateEnd ?? DateTime.Now)
                        .FirstOrDefault();


                    PromptAnswerList promptAnswers = GetCompletedStudentAdjustmentPromptAnswers(context, studentRequest.StudentRequestID);

                    //Get the rejection reason if one is required.
                    int? rejectionReason;
                    if (studentRequest.Reasons.IsRejection.HasValue && studentRequest.Reasons.IsRejection.Value == true &&
                        studentRequestCurrentChange.Reasons != null)
                        rejectionReason = studentRequestCurrentChange.Reasons.ReasonID;
                    else
                        rejectionReason = null;


                    return new CompletedStudentAdjustment(studentId,
                        studentRequest.InclusionAdjustmentReasons.IncAdjReasonID,
                        promptAnswers,
                        studentRequest.Reasons.ReasonID,
                        rejectionReason,
                        studentRequestCurrentChange.ScrutinyStatus.ScrutinyStatusCode,
                        "");

                }
            }
        }

        private static PromptAnswerList GetCompletedStudentAdjustmentPromptAnswers(Web09_Entities context, int studentRequestId)
        {
            var query = from srdObj in context.StudentRequestData
                        where srdObj.StudentRequestID == studentRequestId
                        select new
                        {
                            REQ = srdObj,
                            PROMPTS = srdObj.Prompts,
                            PROMPTTYPES = srdObj.Prompts.PromptTypes,
                            StudentRequest = srdObj.StudentRequests,
                            StudentRequestReason = srdObj.StudentRequests.Reasons,
                            StudentMainInfo = srdObj.StudentRequests.Students,
                            StudentRequestCurrentInfo = srdObj.StudentRequests.StudentRequestChanges.Where(src => src.DateEnd == null).FirstOrDefault(),
                            StudentCurrentInfo = srdObj.StudentRequests.Students.StudentChanges.Where(std => std.DateEnd == null).FirstOrDefault(),
                            StudentOriginalInfo = srdObj.StudentRequests.Students.StudentChanges.OrderByDescending(std => std.ChangeID).FirstOrDefault(),
                        };

            PromptAnswerList aList = new PromptAnswerList();

            foreach (var data in query)
            {
                PromptAnswer pa = new PromptAnswer(data.REQ.PromptID, data.REQ.Prompts.PromptText, data.REQ.Prompts.PromptShortText, data.REQ.UpdateByDA);

                pa.PromptAnswerType = (PromptAnswer.PromptAnswerTypeEnum)data.PROMPTTYPES.PromptTypeID;

                switch (data.PROMPTTYPES.PromptTypeID)
                {
                    case (short)PromptAnswer.PromptAnswerTypeEnum.Date:
                        if (data.REQ.PromptValue != "")
                            pa.PromptDateTimeAnswer = new DateTime(int.Parse(data.REQ.PromptValue.Substring(0, 4)), int.Parse(data.REQ.PromptValue.Substring(4, 2)), int.Parse(data.REQ.PromptValue.Substring(6, 2)));
                        else
                            pa.PromptDateTimeAnswer = null;

                        break;
                    case (short)PromptAnswer.PromptAnswerTypeEnum.Info:
                        break;
                    case (short)PromptAnswer.PromptAnswerTypeEnum.Integer:

                        if (data.REQ.PromptValue != "")
                            pa.PromptIntegerAnswer = (int?)int.Parse(data.REQ.PromptValue);
                        else
                            pa.PromptIntegerAnswer = null;

                        break;

                    case (short)PromptAnswer.PromptAnswerTypeEnum.ListBox:

                        if (data.REQ.PromptValue != "")
                        {
                            int listOrder = 0;
                            if (int.TryParse(data.REQ.PromptValue, out listOrder))
                                pa.PromptSelectedValueAnswer = context.PromptResponses.Where(pr => pr.PromptID == data.REQ.PromptID && pr.ListOrder == listOrder).FirstOrDefault().ListValue;
                            else
                                pa.PromptSelectedValueAnswer = "";
                        }
                        break;

                    case (short)PromptAnswer.PromptAnswerTypeEnum.Text:
                        pa.PromptStringAnswer = data.REQ.PromptValue;
                        break;

                    case (short)PromptAnswer.PromptAnswerTypeEnum.YesNo:

                        bool? answer = true;

                        if (data.REQ.PromptValue.ToLower() == "yes")
                            answer = true;
                        else
                            answer = false;

                        pa.PromptYesNoAnswer = answer;
                        break;
                };

                aList.Add(pa);
            }

            return aList;

        }

        public static PromptAnswerList SetStudentRequestInfoPromptMessages
            (
            Web09_Entities context,
            PromptAnswerList aList,
            StudentRequests studentRequest,
            Students student,
            StudentChanges studentCurrentChange,
            StudentChanges studentOriginalChange,
            Reasons requestReasons
            )
        {
            List<CohortConfiguration> listConfig = context.CohortConfiguration.ToList();

            DateTime ascDate = DateTime.Parse(listConfig.Where(c => c.ConfigurationCode == Contants.ANNUAL_SCHOOL_CENSUS_DATE_LOOKUP_CODE).FirstOrDefault().ConfigurationValue);
            int tableYear = int.Parse(listConfig.Where(c => c.ConfigurationCode == Contants.COHORT_CONFIG_LOOKUP_CODE_TABLE_YEAR).FirstOrDefault().ConfigurationValue);                     

            foreach (var answer in aList)
            {
                answer.WarningMessage = "";
                answer.ErrorMessage = "";
                answer.InformationMessage = "";

                // Forvus Index to merge pupils
                if (
                    answer.PromptID == Contants.PROMPT_ID_RESULTS_BELONG_TO_ANOTHER_PUPIL_KS5
                    ||
                    answer.PromptID == Contants.PROMPT_ID_RESULTS_BELONG_TO_ANOTHER_PUPIL_KS4
                    ||
                    answer.PromptID == Contants.PROMPT_ID_RESULTS_BELONG_TO_ANOTHER_PUPIL_KS2
                    )
                {
                    if (requestReasons.ReasonID == Contants.SCRUTINY_REASON_MERGE_PUPILS)
                    {
                        int forvusIndex = 0;
                        if (answer.GetAnswerAsString() == "")
                            answer.WarningMessage = "required to merge results";
                        else if (!int.TryParse(answer.GetAnswerAsString(), out forvusIndex))
                        {
                            answer.WarningMessage = "required to merge results";
                            forvusIndex = 0;
                        }
                        else if (forvusIndex == 0) // Forvus index value is provided but is zero, means not provided
                        {
                            answer.WarningMessage = "required to merge results";
                            forvusIndex = 0;
                        }
                        else
                        {
                            int studentForvusIndex = student.ForvusIndex.HasValue ? student.ForvusIndex.Value : 0;
                            if (forvusIndex == studentForvusIndex)
                                answer.WarningMessage = "same as original";
                            else
                                answer.WarningMessage = "only for merging results";

                            if (student.Schools == null) student.SchoolsReference.Load();

                            var matchedStudent = (from std in context.Students
                                                  where std.ForvusIndex == forvusIndex
                                                  && std.Schools.DFESNumber == student.Schools.DFESNumber
                                                  select std
                                                ).FirstOrDefault();

                            if (matchedStudent == null)
                            {
                                answer.ErrorMessage = "original pupil data not found";
                            }

                            if (forvusIndex < 5000)
                            {
                                var matchedStudent5000 = (from std in context.Students
                                                          where std.ForvusIndex == forvusIndex
                                                          select new
                                                          {
                                                              SC = std.StudentChanges.Where(sc => sc.DateEnd == null).FirstOrDefault(),
                                                              SCH = std.Schools
                                                          }
                                                ).FirstOrDefault();
                                if (forvusIndex < 5000 && matchedStudent5000 != null)
                                {
                                    DateTime dob = new DateTime(
                                    int.Parse(matchedStudent5000.SC.DOB.Substring(0, 4)),
                                    int.Parse(matchedStudent5000.SC.DOB.Substring(4, 2)),
                                    int.Parse(matchedStudent5000.SC.DOB.Substring(6, 2))
                                    );
                                    answer.InformationMessage = matchedStudent5000.SCH.DFESNumber.ToString() + ", " + matchedStudent5000.SC.Surname + ", " + dob.ToString("dd/MM/yyyy") + ", " + matchedStudent5000.SC.Gender == "M" ? "Male" : "Female";
                                }
                            }
                        }
                    }
                }

                // Date Of Arrival
                if (answer.PromptID == Contants.PROMPT_ID_DATE_OF_ARRIVAL)
                {
                    if (requestReasons.ReasonID == Contants.SCRUTINY_REASON_ADMITTED_INTO_6TH_FORM_FROM_ABROAD
                        ||
                        requestReasons.ReasonID == Contants.SCRUTINY_REASON_RECENTLY_FROM_ABROAD
                        )
                    {
                        if (answer.GetAnswerAsString() == "")
                            answer.WarningMessage = "missing";
                        else
                        {
                            //20071211
                            String answerText = answer.GetAnswerAsString();
                            DateTime arrivalDate;

                            if ( DateTime.TryParse(answerText, out arrivalDate) )
                            {
                                DateTime arrivalDateEarliest = ACSDate.AddYears(-2);             

                                // TFS 22667
                                if (arrivalDate < arrivalDateEarliest)
                                {
                                    answer.ErrorMessage = UKArrivalDateTooEarly();
                                }

                            }
                           
                        }
                    }
                }

                // Language
                if (answer.PromptID == Contants.PROMPT_ID_LANGUAGE)
                {
                    if (requestReasons.ReasonID == Contants.SCRUTINY_REASON_ADMITTED_INTO_6TH_FORM_FROM_ABROAD
                        ||
                        requestReasons.ReasonID == Contants.SCRUTINY_REASON_RECENTLY_FROM_ABROAD
                        )
                    {
                        if (answer.GetAnswerAsString() == "")
                            answer.WarningMessage = "missing";
                        else
                        {

                            String strValue = answer.GetAnswerAsString();

                            // Not in lookup
                            var values = (from pr in context.PromptResponses
                                          where pr.PromptID == answer.PromptID
                                          && pr.ListValue == strValue
                                          select pr)
                                        .FirstOrDefault();

                            if (values == null)
                                answer.WarningMessage = "not listed";
                            else if (values.Rejected) // Not Acceptable
                                answer.WarningMessage = "not acceptable";

                            if (studentOriginalChange != null)
                            {
                                studentOriginalChange.LanguagesReference.Load();
                                //fix for defect 3249, independent schools have null language codes.
                                if (studentOriginalChange.Languages != null)
                                {
                                    if (studentOriginalChange.Languages.LanguageCode == "ENG"
                                        ||
                                        studentOriginalChange.Languages.LanguageCode == "ENB")
                                        answer.ErrorMessage = string.Format("First Language Code is {0} in January census", studentOriginalChange.Languages.LanguageCode);
                                }
                            }
                        }
                    }
                }

                // Country
                if (answer.PromptID == Contants.PROMPT_ID_COUNTRY)
                {
                    if (requestReasons.ReasonID == Contants.SCRUTINY_REASON_ADMITTED_INTO_6TH_FORM_FROM_ABROAD
                        ||
                        requestReasons.ReasonID == Contants.SCRUTINY_REASON_RECENTLY_FROM_ABROAD
                        )
                    {
                        if (answer.GetAnswerAsString() == "")
                            answer.WarningMessage = "missing";
                        else
                        {

                            String strValue = answer.GetAnswerAsString();

                            // Not in lookup
                            var values = (from pr in context.PromptResponses
                                          where pr.PromptID == answer.PromptID
                                          && pr.ListValue == strValue
                                          select pr)
                                        .FirstOrDefault();

                            if (values == null)
                                answer.WarningMessage = "not listed";
                            else if (values.Rejected) // Not Acceptable
                                answer.WarningMessage = "not acceptable";
                        }
                    }
                }

                // PROMPT_ID_COUNTRY_LIVING_NOW
                if (answer.PromptID == Contants.PROMPT_ID_COUNTRY_WHERE_LIVING_NOW)
                {
                    if (requestReasons.ReasonID == Contants.SCRUTINY_REASON_EMIGRATED)
                    {
                        if (answer.GetAnswerAsString() == "")
                            answer.WarningMessage = "missing";
                        else
                        {
                            String strValue = answer.GetAnswerAsString();

                            // Not in lookup
                            var values = (from pr in context.PromptResponses
                                          where pr.PromptID == answer.PromptID
                                          && pr.ListValue == strValue
                                          select pr)
                                        .FirstOrDefault();

                            if (values == null)
                                answer.WarningMessage = "not listed";
                            else if (values.Rejected) // Not Acceptable
                                answer.WarningMessage = "not acceptable";
                        }
                    }
                }

                // PROMPT_ID_DATE_LEFT_ENGLAND
                if (answer.PromptID == Contants.PROMPT_ID_DATE_LEFT_ENGLAND)
                {
                    if (requestReasons.ReasonID == Contants.SCRUTINY_REASON_EMIGRATED)
                    {
                        if (answer.GetAnswerAsString() == "")
                            answer.WarningMessage = "missing";
                    }
                }

                //DFES VALUE FOR PROMPT_ID_ADMISSION_FOLLOWING_PERMANENT_EXCLUSION AND PROMPT_ID_REMOVE_DUAL_REGISTERED
                if (
                    answer.PromptID == Contants.PROMPT_ID_ADMISSION_FOLLOWING_PERMANENT_EXCLUSION
                    ||
                    answer.PromptID == Contants.PROMPT_ID_REMOVE_DUAL_REGISTERED
                    )
                {
                    if (
                        requestReasons.ReasonID == Contants.SCRUTINY_REASON_ADMISSION_FOLLOWING_PERMANENT_EXCLUSION
                        ||
                        requestReasons.ReasonID == Contants.SCRUTINY_REASON_REMOVE_DUAL_REGISTERED
                        )
                    {
                        string strValue = answer.GetAnswerAsString();
                        if (strValue == "")
                            answer.WarningMessage = "missing";
                        else
                        {
                            if (strValue.Length != 7)
                                answer.WarningMessage = "wrong length";
                            else
                            {
                                // valid DCSF
                                int dcsfNo = 0;
                                if (!int.TryParse(strValue, out dcsfNo))
                                    answer.WarningMessage = "Invalid DfE No";
                                else if (!Validation.Common.IsDCSFNumberValid(context, int.Parse(strValue)))
                                    answer.WarningMessage = "DfE No not found";
                            }
                        }
                    }
                }

                //PROMPT_ID_DATE_OF_DEATH
                if (answer.PromptID == Contants.PROMPT_ID_DATE_OF_DEATH)
                {
                    if (
                        requestReasons.ReasonID == Contants.SCRUTINY_REASON_DEATH
                        )
                    {
                        string strValue = answer.GetAnswerAsString();
                        if (strValue == "")
                            answer.WarningMessage = "missing";
                    }
                }

                //PROMPT_ID_REVISED_ADMISSION_DATE
                if (answer.PromptID == Contants.PROMPT_ID_REVISED_ADMISSION_DATE)
                {
                    string strValue = answer.GetAnswerAsString();

                    if (strValue == "")
                        answer.WarningMessage = "missing";
                    else
                    {
                        DateTime studentRevisedAdmissionDate = DateTime.Parse(strValue);
                        DateTime earliestAbroadAdmissionDate = new DateTime(
                        tableYear - 2, 07, 01);

                        DateTime earliestExclusionAdmissionDate = new DateTime(
                        tableYear - 2, 09, 01);

                        if (studentRevisedAdmissionDate > ascDate)
                            answer.WarningMessage = "after ASC";
                        else if (
                            requestReasons.ReasonID == Contants.SCRUTINY_REASON_RECENTLY_FROM_ABROAD
                            &&
                            studentRevisedAdmissionDate < earliestAbroadAdmissionDate
                           )
                            answer.WarningMessage = "before " + earliestAbroadAdmissionDate.ToString("dd/MM/yyyy");
                        else if (
                            requestReasons.ReasonID == Contants.SCRUTINY_REASON_ADMISSION_FOLLOWING_PERMANENT_EXCLUSION
                            &&
                            studentRevisedAdmissionDate < earliestExclusionAdmissionDate
                           )
                            answer.WarningMessage = "before " + earliestExclusionAdmissionDate.ToString("dd/MM/yyyy");
                    }
                }
            }

            return aList;

        }

        private static DateTime ACSDate
        {
            get
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        return GetASCDate(context);
                    }
                }
            }
        }

        private static DateTime GetASCDate(Web09_Entities context)
        {
            return DateTime.Parse(context.CohortConfiguration
                .Where(cc => cc.ConfigurationCode == Contants.ANNUAL_SCHOOL_CENSUS_DATE_LOOKUP_CODE)
                .Select(cc => cc.ConfigurationValue)
                .First());
        }

        public static List<Student.ValidationFailure> SetStudentRequestInfoFieldInformationMessages
            (
             Web09_Entities context,
             PromptAnswerList aList,
             StudentRequests studentRequest,
             Students student,
             StudentChanges studentCurrentChange,
             StudentChanges studentOriginalChange,
             Reasons requestReasons
            )
        {

            List<Student.ValidationFailure> msgList = new List<Student.ValidationFailure>();

            return msgList;

        }

        public static string UKArrivalDateTooEarly()
        {
            return "UK Arrival Date more than 2 years before ASC Date";
        }

        public static List<Student.ValidationFailure> SetStudentRequestInfoFieldWarningMessages
            (
             Web09_Entities context,
             PromptAnswerList aList,
             StudentRequests studentRequest,
             Students student,
             StudentChanges studentCurrentChange,
             StudentChanges studentOriginalChange,
             Reasons requestReasons
            )
        {
            List<CohortConfiguration> listConfig = context.CohortConfiguration.ToList();

            DateTime ascDate = DateTime.Parse(listConfig.Where(c => c.ConfigurationCode == Contants.ANNUAL_SCHOOL_CENSUS_DATE_LOOKUP_CODE).FirstOrDefault().ConfigurationValue);
            int intCurrentYear = int.Parse(listConfig.Where(c => c.ConfigurationCode == Contants.COHORT_CONFIG_LOOKUP_CODE_TABLE_YEAR).FirstOrDefault().ConfigurationValue);

            List<Student.ValidationFailure> msgList = new List<Student.ValidationFailure>();                      

            //is forvus index provided
            if (!(student.ForvusIndex.HasValue && student.ForvusIndex.Value != 0))
                msgList.Add(new Student.ValidationFailure
                {
                    PupilField = Student.PupilFieldEnum.ForvusIndex,
                    Message = "missing"
                }
                );

            // Date of birth
            int age = TSStudent.CalculateStudentAge(studentCurrentChange.DOB);

            student.CohortsReference.Load();
            if (student.Cohorts.KeyStage == 4 && age != 15)
            {
                msgList.Add(new Student.ValidationFailure
                {
                    PupilField = Student.PupilFieldEnum.DateOfBirth,
                    Message = "not 15"
                });
            }
            else if (student.Cohorts.KeyStage == 5 && !(age >= 16 && age <= 18))
            {
                msgList.Add(new Student.ValidationFailure
                {
                    PupilField = Student.PupilFieldEnum.DateOfBirth,
                    Message = "not 16-18"
                });
            }

            studentOriginalChange.YearGroupsReference.Load();
            studentCurrentChange.YearGroupsReference.Load();
            if (
                student.Cohorts.KeyStage == 4
                &&
                !(
                    studentCurrentChange.YearGroups.YearGroupCode != "10"
                    ||
                    studentCurrentChange.YearGroups.YearGroupCode != "11"
                    ||
                    studentCurrentChange.YearGroups.YearGroupCode != "12"
                    )
                )
            {
                msgList.Add(new Student.ValidationFailure
                {
                    PupilField = Student.PupilFieldEnum.YearGroup,
                    Message = "not 10-12"
                });
            }

            //*********************************************************************
            // Defect 2412
            //*********************************************************************
            String strStudentEntryDAT = studentCurrentChange.ENTRYDAT;
            //Defensive cast of DB admissiondate string to date time value
            DateTime? admissionDateDT = TSStudent.TryConvertDateTimeDBString(strStudentEntryDAT);

            if (!admissionDateDT.HasValue)
            {
                if (
                    student.ForvusIndex.HasValue && student.ForvusIndex.Value < 90000
                    &&
                    (
                        requestReasons.ReasonID == Contants.SCRUTINY_REASON_ADMISSION_FOLLOWING_PERMANENT_EXCLUSION
                        ||
                        requestReasons.ReasonID == Contants.SCRUTINY_REASON_RECENTLY_FROM_ABROAD
                    )
                   )
                {
                    msgList.Add(new Student.ValidationFailure
                    {
                        PupilField = Student.PupilFieldEnum.AdmissionDate,
                        Message = "missing"
                    });
                }
            }
            else if (requestReasons.ReasonID == Contants.SCRUTINY_REASON_ADMISSION_FOLLOWING_PERMANENT_EXCLUSION)
            {
                if(!requestReasons.StudentRequests.IsLoaded) 
                    requestReasons.StudentRequests.Load();

                var request = requestReasons.StudentRequests.First();
                 if(!request.StudentRequestData.IsLoaded)
                     request.StudentRequestData.Load();
                
                var responseData =   request.StudentRequestData.First(x => x.PromptID == 1002);
                if(responseData!=null)
                {
                    var exclusionDate = DateTime.ParseExact(responseData.PromptValue, "yyyyMMdd", CultureInfo.InvariantCulture);

                    var yearString= (DateTime.Now.Year - 1).ToString();

                    var websiteMode = Web09.Services.Common.WebSiteModeHelper.GetWebSiteModeEnum(TSStudent.GetWebsiteMode());
                    if ( student != null && student.Cohorts != null && student.Cohorts.KeyStage == 4 )
                    {
                      // TFS 17980
                      yearString = (DateTime.Now.Year - 2).ToString();
                    }
                    
                    var yearAgoInSept = DateTime.Parse("1 Sep " + yearString);
                    if (exclusionDate < yearAgoInSept)
                    {
                        aList.First(x => x.PromptID == 1002).ErrorMessage = "Date is before 1 September " + yearString;
                    }   
                }
            }
            else if (student.Cohorts.KeyStage != 2)
            {
                DateTime studentEntryDAT = new DateTime(
                int.Parse(strStudentEntryDAT.Substring(0, 4)),
                int.Parse(strStudentEntryDAT.Substring(4, 2)),
                int.Parse(strStudentEntryDAT.Substring(6, 2))
                );

                DateTime earliestAbroadAdmissionDate = new DateTime(
                intCurrentYear - 2, 07, 01);

                DateTime earliestExclusionAdmissionDate = new DateTime(
                intCurrentYear - 2, 09, 01);

                if (studentEntryDAT > ascDate)
                    msgList.Add(new Student.ValidationFailure
                    {
                        PupilField = Student.PupilFieldEnum.AdmissionDate,
                        Message = "after ASC"
                    });
                else if
                    (
                    requestReasons.ReasonID == Contants.SCRUTINY_REASON_RECENTLY_FROM_ABROAD
                    &&
                    studentEntryDAT < earliestAbroadAdmissionDate
                    )
                    msgList.Add(new Student.ValidationFailure
                    {
                        PupilField = Student.PupilFieldEnum.AdmissionDate,
                        Message = "before " + earliestAbroadAdmissionDate.ToString("dd/MM/yyyy")
                    });
                else if
                    (
                    requestReasons.ReasonID == Contants.SCRUTINY_REASON_ADMISSION_FOLLOWING_PERMANENT_EXCLUSION
                    &&
                    studentEntryDAT < earliestExclusionAdmissionDate
                    )
                    msgList.Add(new Student.ValidationFailure
                    {
                        PupilField = Student.PupilFieldEnum.AdmissionDate,
                        Message = "before " + earliestExclusionAdmissionDate.ToString("dd/MM/yyyy")
                    });

            }

            return msgList;

        }

        private static string GetWebsiteMode()
        {
            string _webSiteMode = string.Empty;

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    _webSiteMode = context.CohortConfiguration
                        .Where(cc => cc.ConfigurationCode == "WebsiteMode")
                        .Select(cc => cc.ConfigurationValue)
                        .FirstOrDefault();
                }
            }
           
            return _webSiteMode;
        }


        public static List<Student.ValidationFailure> SetStudentRequestInfoFieldErrorMessages
            (
             Web09_Entities context,
             PromptAnswerList aList,
             StudentRequests studentRequest,
             Students student,
             StudentChanges studentCurrentChange,
             StudentChanges studentOriginalChange,
             Reasons requestReasons
            )
        {

            List<Student.ValidationFailure> msgList = new List<Student.ValidationFailure>();

            List<CohortConfiguration> listConfig = context.CohortConfiguration.ToList();
            DateTime ascDate = DateTime.Parse(listConfig.Where(c => c.ConfigurationCode == Contants.ANNUAL_SCHOOL_CENSUS_DATE_LOOKUP_CODE).FirstOrDefault().ConfigurationValue);
            int intCurrentYear = int.Parse(listConfig.Where(c => c.ConfigurationCode == Contants.COHORT_CONFIG_LOOKUP_CODE_TABLE_YEAR).FirstOrDefault().ConfigurationValue);

            student.CohortsReference.Load();
            String strQTCode = "KS" + student.Cohorts.KeyStage.ToString();

            // forvus index
            if (student.Cohorts.KeyStage == 4 && student.ForvusIndex.HasValue && student.ForvusIndex.Value != 0)
            {
                // does student have priors for KS1, KS2, KS3
                if (
                    (
                        from stdResult in context.Results
                        where
                        stdResult.QualificationTypes.QualificationTypeCollections.Any(t => t.QualificationTypeCollectionCode.StartsWith("KS1"))
                        && stdResult.Students.StudentID == student.StudentID
                        select stdResult
                    ).Any()
                    ||
                    (
                        from stdResult in context.Results
                        where
                        stdResult.QualificationTypes.QualificationTypeCollections.Any(t => t.QualificationTypeCollectionCode.StartsWith("KS2"))
                        && stdResult.Students.StudentID == student.StudentID
                        select stdResult
                    ).Any()
                    ||
                    (
                        from stdResult in context.Results
                        where
                        stdResult.QualificationTypes.QualificationTypeCollections.Any(t => t.QualificationTypeCollectionCode.StartsWith("KS3"))
                        && stdResult.Students.StudentID == student.StudentID
                        select stdResult
                    ).Any()
                  )
                {
                    msgList.Add(new Student.ValidationFailure
                        {
                            PupilField = Student.PupilFieldEnum.ForvusIndex,
                            Message = "priors for this number"
                        }
                    );
                }
            }

            // forename changed
            if (studentCurrentChange.Forename.ToLower() != studentOriginalChange.Forename.ToLower())
                msgList.Add(new Student.ValidationFailure
                {
                    PupilField = Student.PupilFieldEnum.ForeName,
                    Message = studentOriginalChange.Forename + " in original pupil data"
                });

            // surname changed                 
            if (studentCurrentChange.Surname.ToLower() != studentOriginalChange.Surname.ToLower())
            {

                student.SchoolsReference.Load();
                var exisitngStudent = (
                    from std in context.StudentChanges
                    where std.DateEnd == null
                    && std.Students.Schools.DFESNumber == student.Schools.DFESNumber
                    && std.Surname == studentCurrentChange.Surname
                    && std.Students.StudentID != student.StudentID
                    select new
                    {
                        stdChange = std,
                        std = std.Students
                    }
                    ).FirstOrDefault();

                if (exisitngStudent != null)
                {
                    msgList.Add(new Student.ValidationFailure
                    {
                        PupilField = Student.PupilFieldEnum.Surname,
                        Message = exisitngStudent.stdChange.Surname + " in original pupil data. try Forvus No " + exisitngStudent.std.ForvusIndex.Value.ToString()
                    });
                }
                else
                {
                    msgList.Add(new Student.ValidationFailure
                    {
                        PupilField = Student.PupilFieldEnum.Surname,
                        Message = studentOriginalChange.Surname + " in original pupil data"
                    });
                }
            }

            // DOB changed
            if (studentCurrentChange.DOB != studentOriginalChange.DOB)
            {
                DateTime dtDOB = new DateTime(
                    int.Parse(studentOriginalChange.DOB.Substring(0, 4)),
                    int.Parse(studentOriginalChange.DOB.Substring(4, 2)),
                    int.Parse(studentOriginalChange.DOB.Substring(6, 2))
                    );
                msgList.Add(new Student.ValidationFailure
                {
                    PupilField = Student.PupilFieldEnum.DateOfBirth,
                    Message = dtDOB.ToString("dd/MM/yyyy") + " in original pupil data"
                });
            }

            // gender changed
            if (studentCurrentChange.Gender.ToLower() != studentOriginalChange.Gender.ToLower())
                msgList.Add(new Student.ValidationFailure
                {
                    PupilField = Student.PupilFieldEnum.Gender,
                    Message = studentOriginalChange.Gender + " in original pupil data"
                });

            // sen changed
            if (studentCurrentChange != null && studentOriginalChange != null && studentCurrentChange.SENStatus != null && studentOriginalChange.SENStatus != null)
            {
                studentCurrentChange.SENStatusReference.Load();
                studentOriginalChange.SENStatusReference.Load();
                if (studentCurrentChange.SENStatus.SENStatusCode != studentOriginalChange.SENStatus.SENStatusCode)
                    msgList.Add(new Student.ValidationFailure
                    {
                        PupilField = Student.PupilFieldEnum.SEN,
                        Message = studentOriginalChange.SENStatus.SENStatusDescription + " in original pupil data"
                    });
            }

            // year group changed
            studentOriginalChange.YearGroupsReference.Load();
            studentCurrentChange.YearGroupsReference.Load();
            if (studentOriginalChange.YearGroups.YearGroupCode != studentCurrentChange.YearGroups.YearGroupCode)
            {
                msgList.Add(new Student.ValidationFailure
                {
                    PupilField = Student.PupilFieldEnum.DateOfBirth,
                    Message = studentOriginalChange.YearGroups.YearGroupCode + " in original pupil data"
                });
            }


            //Admission Date
            String strStudentEntryDAT = studentOriginalChange.ENTRYDAT;
            //Defensive cast of DB admissiondate string to date time value
            DateTime? admissionDateDT = TSStudent.TryConvertDateTimeDBString(strStudentEntryDAT);


            if (admissionDateDT.HasValue)
            {
                DateTime studentEntryDAT = new DateTime(
                int.Parse(strStudentEntryDAT.Substring(0, 4)),
                int.Parse(strStudentEntryDAT.Substring(4, 2)),
                int.Parse(strStudentEntryDAT.Substring(6, 2))
                );

                DateTime earliestAbroadAdmissionDate = new DateTime(
                intCurrentYear - 2, 06, 01);

                DateTime earliestExclusionAdmissionDate = new DateTime(
                intCurrentYear - 2, 09, 01);

                if (
                      studentEntryDAT < earliestAbroadAdmissionDate
                      &&
                      requestReasons.ReasonID == Contants.SCRUTINY_REASON_RECENTLY_FROM_ABROAD
                  )
                {
                    msgList.Add(new Student.ValidationFailure
                    {
                        PupilField = Student.PupilFieldEnum.AdmissionDate,
                        Message = studentEntryDAT.ToString("dd/MM/yyyy") + " in ASC - before " + earliestAbroadAdmissionDate.ToString("dd/MM/yyyy")
                    });
                }

            }
            return msgList;
        }
    }
}
