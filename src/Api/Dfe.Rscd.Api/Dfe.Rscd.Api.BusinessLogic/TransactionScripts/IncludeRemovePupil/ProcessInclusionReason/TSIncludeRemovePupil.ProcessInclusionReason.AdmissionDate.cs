using System;
using System.Collections.Generic;
using System.Linq;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {

        #region ProcessAdmissionDate (For a include/remove request)

        internal static AdjustmentPromptAnalysis ProcessAdmissionDate(Web09_Entities context,
            int keystage,
            int admissionDatePromptID,
            PromptAnswerList answers,
            int studentId,
            int? inclusionReasonId)
        {

            switch (keystage)
            {
                case (4):
                    return ProcessAdmissionDateForKS4(studentId, inclusionReasonId, answers);
                case (2):
                    return ProcessAdmissionDateForKS2AndKS3(context, keystage, admissionDatePromptID, answers, studentId, inclusionReasonId);
                case (3):
                    throw Web09Exception.GetBusinessException(Web09MessageList.InvalidKS3StudentAdjustmentRequest);
                case(5):
                    throw Web09Exception.GetBusinessException(Web09MessageList.InvalidKS5AdmissionDateRequest);
                default:
                    throw Web09Exception.GetBusinessException(Web09MessageList.KeystageInvalid);
            }
                
        }



        /// <summary>
        /// Collect date joined role and analyse the response.
        /// </summary>
        /// <param name="context">Web09 entities object context</param>
        /// <param name="keyStage">The key stage of the student for whom the request is being genereated</param>
        /// <param name="admissionDate">The admission date or date joined roll</param>
        /// <param name="promptAnswers">The prompt answers provided so far in the request generation process</param>
        /// <param name="studentId">The id of the student for whom the request is being generated</param>
        /// <param name="inclusionReasonId">The inclusion reason</param>
        /// <returns>AdjustmentPromptAnalysis object indicating whether adjustment is complete or not.</returns>
        private static AdjustmentPromptAnalysis ProcessAdmissionDateForKS2AndKS3(Web09_Entities context,
            int keyStage,
            int admissionDatePromptID,
            PromptAnswerList promptAnswers,
            int studentId,
            int? inclusionReasonId)
        {

            if (!promptAnswers.HasPromptAnswer(admissionDatePromptID))
            {
                List<Prompts> furtherPrompts = new List<Prompts>();
                furtherPrompts.Add(GetPromptByPromptID(admissionDatePromptID));
                return new AdjustmentPromptAnalysis(furtherPrompts);
            }
            else if (promptAnswers.HasPromptAnswer(admissionDatePromptID) && IsPromptAnswerComplete(promptAnswers, admissionDatePromptID))
            {

                DateTime admissionDate = promptAnswers.GetPromptAnswerByPromptID(admissionDatePromptID).PromptDateTimeAnswer.Value;
                DateTime annualSchoolCensusDate = GetAnnualSchoolCensusDate(context);
                DateTime ksTestEndDate = GetKS3TestEndDate(context);


                if (admissionDate == null)
                    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);

                List<Prompts> FurtherPrompts = new List<Prompts>();


                if (admissionDate <= annualSchoolCensusDate)
                {

                    //process submission with a reason of Add Pupil and a Scrutiny Status of Accept. Include
                    //info prompt 32210 for KS2 and 22210 for KS2as reqest completion message
                    int omissionPromptID;
                    if (keyStage == 2)
                        omissionPromptID = Contants.PROMPT_ID_PUPIL_OMISSION_KS2;
                    else
                        omissionPromptID = Contants.PROMPT_ID_PUPIL_OMISSION_KS3;

                    return ProcessSingularFurtherPrompt(omissionPromptID,
                        studentId,
                        inclusionReasonId,
                        promptAnswers,
                        Contants.SCRUTINY_REASON_ADD_PUPIL,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        null);

                }
                else if (admissionDate <= ksTestEndDate && admissionDate > annualSchoolCensusDate)
                {
                    //process submission with Scrutiny Status of Pending and include info prompt
                    //32220 for KS3 and 22220 for KS2 as request completion message
                    int infoPromptID;
                    if (keyStage == 2)
                        infoPromptID = Contants.PROMPT_ID_PUPIL_ADDITION_REVIEW_INFO_KS2;
                    else
                        infoPromptID = Contants.PROMPT_ID_PUPIL_ADDITION_REVIEW_INFO_KS3;

                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                        studentId,
                        inclusionReasonId,
                        promptAnswers,
                        Contants.SCRUTINY_REASON_ADD_PUPIL,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        GetInfoPromptText(context, infoPromptID))
                        );

                }
                else // i.e.: if (admissionDate > KSTestEndDate)
                {

                    //process submission with Scrutiny Status of Pending and include info prompt
                    //32230 for KS3 and 22230 for KS2 as request completion message
                    int explanationPromptID;
                    if (keyStage == 2)
                        explanationPromptID = Contants.PROMPT_ID_PUBLISH_PUPIL_EXPLANATION_KS2;
                    else
                        explanationPromptID = Contants.PROMPT_ID_PUBLISH_PUPIL_EXPLANATION_KS3;

                    return ProcessSingularFurtherPrompt(explanationPromptID,
                        studentId,
                        inclusionReasonId,
                        promptAnswers,
                        Contants.SCRUTINY_REASON_JOINED_AFTER_TEST_WEEK,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        null);

                }
            }
            else
            {
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            }

        }

        private static AdjustmentPromptAnalysis ProcessAdmissionDateForKS4(int studentId, int? inclusionReasonId, PromptAnswerList answers)
        {
            List<Prompts> furtherPrompts = new List<Prompts>();

            if(!answers.HasPromptAnswer(Contants.PROMPT_ID_ADMISSION_DATE_KS4))
            {
                furtherPrompts.Add(GetPromptByPromptID(Contants.PROMPT_ID_ADMISSION_DATE_KS4));
                return new AdjustmentPromptAnalysis(furtherPrompts);
            }
            if(answers.HasPromptAnswer(Contants.PROMPT_ID_ADMISSION_DATE_KS4) && IsPromptAnswerComplete(answers, Contants.PROMPT_ID_ADMISSION_DATE_KS4))
            {
                DateTime admissionDateEntered = answers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_ADMISSION_DATE_KS4).PromptDateTimeAnswer.Value;

                if (admissionDateEntered <= AnnualSchoolCensusDate)
                {
                    return ProcessSingularFurtherPrompt(2210,
                        studentId,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_ADD_PUPIL,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        null);
                }
                else // => Admission date is > annual school census date.
                {
                    return ProcessSingularFurtherPrompt(2220,
                        studentId,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_JOINED_AFTER_ASC,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        null);
                }

            }
            else 
            {
                //Raise completed non request outcome
                return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment("No admission date was given. A request has not been generated"));
            }
        }

        #endregion

        #region ProcessAdmissionDate (For a pupil edit - implicit request)

        internal static AdjustmentPromptAnalysis ProcessAdmissionDateForPupilEdit(Web09_Entities context,
            int keystage,
            int admissionDatePromptID,
            PromptAnswerList answers,
            int studentId,
            int? inclusionReasonId)
        {

            switch (keystage)
            {
                case (4):
                    return ProcessAdmissionDateForKS4PupilEdit(context, studentId, inclusionReasonId, answers);
                case (2):
                    return ProcessAdmissionDateForKS2PupilEdit(context, keystage, admissionDatePromptID, answers, studentId, inclusionReasonId);
                case (3):
                    throw Web09Exception.GetBusinessException(Web09MessageList.InvalidKS3StudentAdjustmentRequest);
                case (5):
                    throw Web09Exception.GetBusinessException(Web09MessageList.InvalidKS5AdmissionDateRequest);
                default:
                    throw Web09Exception.GetBusinessException(Web09MessageList.KeystageInvalid);
            }

        }

        private static AdjustmentPromptAnalysis ProcessAdmissionDateForKS4PupilEdit(Web09_Entities context, int studentId, int? inclusionReasonId, PromptAnswerList answers)
        {
            List<Prompts> furtherPrompts = new List<Prompts>();

            if (!answers.HasPromptAnswer(Contants.PROMPT_ID_ADMISSION_DATE_FOR_PUPIL_EDIT_KS4))
            {
                furtherPrompts.Add(GetPromptByPromptID(Contants.PROMPT_ID_ADMISSION_DATE_FOR_PUPIL_EDIT_KS4));
                return new AdjustmentPromptAnalysis(furtherPrompts);
            }
            if (answers.HasPromptAnswer(Contants.PROMPT_ID_ADMISSION_DATE_FOR_PUPIL_EDIT_KS4) &&
                IsPromptAnswerComplete(answers, Contants.PROMPT_ID_ADMISSION_DATE_FOR_PUPIL_EDIT_KS4))
            {
                DateTime admissionDateEntered = answers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_ADMISSION_DATE_FOR_PUPIL_EDIT_KS4).PromptDateTimeAnswer.Value;
                DateTime? currentAdmissionDate = null;
                string currentAdmissionDateStr = context.StudentChanges
                    .Where(sc => sc.StudentID == studentId && sc.DateEnd == null)
                    .Select(sc => sc.ENTRYDAT)
                    .FirstOrDefault();
                currentAdmissionDate = TSStudent.TryConvertDateTimeDBString(currentAdmissionDateStr);

                if (currentAdmissionDate.HasValue && currentAdmissionDate.Value < AnnualSchoolCensusDate &&
                    admissionDateEntered > AnnualSchoolCensusDate)
                {
                    return ProcessSingularFurtherPrompt(1610,
                        studentId,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_ADMISSION_DATE_CHANGE,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        null);
                }
                else if (!currentAdmissionDate.HasValue && admissionDateEntered > AnnualSchoolCensusDate)
                {
                    //=> Admission date is missing, and the new value is after census date.

                    return ProcessSingularFurtherPrompt(1620,
                        studentId,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_JOINED_AFTER_ASC,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        null);
                }
                else
                {
                    //No request is required => Raise completed non request outcome
                    //No message should be provided.
                    return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(""));
                }

            }
            else
            {
                //Raise completed non request outcome
                return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment("No admission date was given. A request has not been generated"));
            }
        }

        private static AdjustmentPromptAnalysis ProcessAdmissionDateForKS2PupilEdit(
            Web09_Entities context,
            int keystage,
            int admissionDatePromptID,
            PromptAnswerList answers,
            int studentId, int? inclusionReasonId)
        {
           
            if(!answers.HasPromptAnswer(Contants.PROMPT_ID_ADMISSION_DATE_FOR_PUPIL_EDIT_KS2))
            {
                return new AdjustmentPromptAnalysis(new List<Prompts>{GetPromptByPromptID(Contants.PROMPT_ID_ADMISSION_DATE_FOR_PUPIL_EDIT_KS2)});
            }
            else if(answers.HasPromptAnswer(Contants.PROMPT_ID_ADMISSION_DATE_FOR_PUPIL_EDIT_KS2) && 
                IsPromptAnswerComplete(answers, Contants.PROMPT_ID_ADMISSION_DATE_FOR_PUPIL_EDIT_KS2))
            {

                DateTime admissionDateEntered = answers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_ADMISSION_DATE_FOR_PUPIL_EDIT_KS2).PromptDateTimeAnswer.Value;
                DateTime? currentAdmissionDate = null;

                string currentAdmissionDateStr = context.StudentChanges
                    .Where(sc => sc.StudentID == studentId && sc.DateEnd == null)
                    .Select(sc => sc.ENTRYDAT)
                    .FirstOrDefault();

                currentAdmissionDate = TSStudent.TryConvertDateTimeDBString(currentAdmissionDateStr);

                DateTime ks2TestStartDate = KS2TestStartDate;
                DateTime ks2TestEndDate = KS2TestEndDate;

                if (admissionDateEntered <= ks2TestStartDate &&
                    !(inclusionReasonId.HasValue && inclusionReasonId.HasValue && inclusionReasonId.Value != (int)ReasonsForAdjustment.AdmittedFromAbroad))
                {
                    return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(GetInfoPromptText(21610)));
                }
                else if (admissionDateEntered >= ks2TestStartDate && admissionDateEntered <= ks2TestEndDate)
                {
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                        studentId,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_ADMISSION_DATE_CHANGE,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        GetInfoPromptText(21620))
                        );
                }
                else if (admissionDateEntered >= KS2TestEndDate)
                {
                    return ProcessSingularFurtherPrompt(21630,
                        studentId,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_ADMISSION_DATE_CHANGE,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        null);
                }
                else
                {
                    return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment("No adjustment request required for this admission date change."));
                }
            }
            else
            {
                //Raise completed non request outcome
                return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment("No admission date was given. A request has not been generated"));
            }      
            

        }

        #endregion
    }
}
