using System.Collections.Generic;
using System.Linq;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.Business.Logic.Validation;
using Web09.Checking.DataAccess;
using Web09.Services.DataContracts;

namespace Web09.Services.Adapters.EntityTranslators
{
    public class TranslateBetweenDataContractCompletedPupilAdjustmentAndBusinessEntityCompletedStudentAdjustment
    {

        public static CompletedPupilAdjustment TranslateBusinessEntityCompletedStudentAdjustmentToDataContractCompletedPupilAdjustment(CompletedStudentAdjustment adjustmentIn)
        {
            CompletedPupilAdjustment adjustmentOut = new CompletedPupilAdjustment();

            adjustmentOut.PupilID = adjustmentIn.StudentID;
            if(adjustmentIn.InclusionReasonID.HasValue) adjustmentOut.InclusionReasonID = (short)adjustmentIn.InclusionReasonID.Value;
            if (adjustmentIn.PromptAnswerList != null && adjustmentIn.PromptAnswerList.Count > 0)
                adjustmentOut.PromptAnswerList = TranslateBetweenDataContractPromptAnswersAndBusinessEntityPromptAnswers
                    .TranslateBusinessEntityPromptAnswerListToDataContractPromptAnswerList(adjustmentIn.PromptAnswerList);
            adjustmentOut.ScrutinyReasonID = (short)adjustmentIn.ScrutinyReasonID;
            adjustmentOut.RejectionReasonCode = (short?)adjustmentIn.RejectionReasonCode;
            adjustmentOut.ScrutinyStatusCode = adjustmentIn.ScrutinyStatusCode;
            adjustmentOut.RequestCompletionDisplayMessage = adjustmentIn.RequestCompletionDisplayMessage;
            adjustmentOut.ScrutinyStatusDescription = adjustmentIn.ScrutinyStatusDescription;
            if (adjustmentIn.StudentRequestID.HasValue && adjustmentIn.StudentRequestID.Value != 0)
                adjustmentOut.StudentRequestID = adjustmentIn.StudentRequestID.Value;

            adjustmentOut.ForvusId = adjustmentIn.ForvusId;

            return adjustmentOut;
            
        }

        public static DataContracts.ValidationFailureList TranslateBusinessEntityCompletedStudentAdjustmentMessageListToDataContractCompletedPupilAdjustmentMessageList(List<Student.ValidationFailure> valueIN)
        {
            ValidationFailureList returnList = new ValidationFailureList();
            foreach(var itm in valueIN)
            {
                returnList.Add(new ValidationFailure
                {
                     Message=itm.Message,
                      PupilFieldEnum=itm.PupilField.ToString()
                }
                );
            }

            return returnList;
        }

        public static CompletedStudentAdjustment TranslateDataContractCompletedPupilAdjustmentToBusinessEntityCompletedStudentAdjustment(CompletedPupilAdjustment adjustmentIn)
        {

            Web09.Checking.Business.Logic.Entities.PromptAnswerList promptAnswers = new Web09.Checking.Business.Logic.Entities.PromptAnswerList();
            promptAnswers = TranslateBetweenDataContractPromptAnswersAndBusinessEntityPromptAnswers
                    .TranslateDataContractPromptAnswerListToBusinessEntityPromptAnswerList(adjustmentIn.PromptAnswerList);

            CompletedStudentAdjustment adjustmentOut = new CompletedStudentAdjustment(
                adjustmentIn.PupilID,
                adjustmentIn.InclusionReasonID,
                promptAnswers,
                adjustmentIn.ScrutinyReasonID,
                adjustmentIn.RejectionReasonCode,
                adjustmentIn.ScrutinyStatusCode,
                adjustmentIn.RequestCompletionDisplayMessage);

            if (adjustmentIn.StudentRequestID.HasValue && adjustmentIn.StudentRequestID.Value != 0)
                adjustmentOut.StudentRequestID = adjustmentIn.StudentRequestID.Value;

            return adjustmentOut;
        }

        public static CompletedPupilNonAdjustment TranslateBusinessEntityCompletedNonAdjustmentToDataContractCompletedNonAdjustment(CompletedNonStudentAdjustment nonAdjustmentResultIn)
        {
            return new CompletedPupilNonAdjustment{ RequestCompletionDisplayMessage = nonAdjustmentResultIn.RequestCompletionDisplayMessage};
        }

        public static CohortAdjustmentRequest TranslateBusinessEntityCohortAdjustmentRequestToDataContractCohortAdjustmentStudentRequest(CohortAdjustmentRequestEntity requestIn)
        {
            CohortAdjustmentRequest requestOut = new CohortAdjustmentRequest{
                AmendCode=requestIn.StudentRequest.StudentRequestChanges.FirstOrDefault().AmendCodes!=null ? requestIn.StudentRequest.StudentRequestChanges.FirstOrDefault().AmendCodes.AmendCode:"",
                ChangeID = requestIn.StudentRequest.StudentRequestChanges.FirstOrDefault().Changes.ChangeID,
                Comments = requestIn.StudentRequest.StudentRequestChanges.FirstOrDefault().Comments,
                CommentsHistory= requestIn.CommentHistory,
                DCSFNotification = requestIn.StudentRequest.StudentRequestChanges.FirstOrDefault().DCSFNotification,
                DCSFNumber= null,
                DCSFUpdateDate = requestIn.DCSFUpdateDate,
                ForvusUpdateDate = requestIn.ForvusUpdateDate,
                IncAdjReasonDescription = requestIn.StudentRequest.InclusionAdjustmentReasons!=null?requestIn.StudentRequest.InclusionAdjustmentReasons.IncAdjReasonDescription:"",
                IncAdjReasonID = requestIn.StudentRequest.InclusionAdjustmentReasons.IncAdjReasonID,
                ReasonID = requestIn.StudentRequest.StudentRequestChanges.FirstOrDefault().Reasons!= null ? requestIn.StudentRequest.StudentRequestChanges.FirstOrDefault().Reasons.ReasonID: 0,
                ReasonText= requestIn.StudentRequest.StudentRequestChanges.FirstOrDefault().Reasons != null ? requestIn.StudentRequest.StudentRequestChanges.FirstOrDefault().Reasons.ReasonText: "",
                RequestBy=requestIn.RequestBy,
                RequestDate=requestIn.RequestDate,
                RequestID=requestIn.StudentRequest.StudentRequestID,
                RequestType=requestIn.RequestType,
                ScrutinyReasonDescription=requestIn.StudentRequest.Reasons.ReasonText,
                ScrutinyReasonID=requestIn.StudentRequest.Reasons.ReasonID,
                ScrutinyStatusCode = requestIn.StudentRequest.StudentRequestChanges.FirstOrDefault().ScrutinyStatus.ScrutinyStatusCode,
                ScrutinyStatusDescription = requestIn.StudentRequest.StudentRequestChanges.FirstOrDefault().ScrutinyStatus.ScrutinyStatusDescription,
                StudentID=requestIn.StudentRequest.Students.StudentID,
                SuggestedDecision=requestIn.SuggestedDecision,
                ScrutinyReasonStatus = (requestIn.StudentRequest.Reasons.IsRejection.HasValue == false ? "" : (requestIn.StudentRequest.Reasons.IsRejection.Value? "invalid":"valid")),
                OriginalAmendCode=requestIn.OriginalAmendCode
            };

            return requestOut;
        }

        public static CohortAdjustmentRequest TranslateBusinessEntityCohortAdjustmentRequestToDataContractCohortAdjustmentSchoolRequest(CohortAdjustmentRequestEntity requestIn)
        {
            CohortAdjustmentRequest requestOut = new CohortAdjustmentRequest
            {
                AmendCode = "",
                ChangeID = requestIn.SchoolRequest.SchoolRequestChanges.FirstOrDefault().Changes.ChangeID,
                Comments = requestIn.SchoolRequest.SchoolRequestChanges.FirstOrDefault().Comments,
                CommentsHistory = requestIn.CommentHistory,
                DCSFNotification = requestIn.SchoolRequest.SchoolRequestChanges.FirstOrDefault().DCSFNotification,
                DCSFNumber = requestIn.SchoolRequest.Schools.DFESNumber,
                DCSFUpdateDate = requestIn.DCSFUpdateDate,
                ForvusUpdateDate = requestIn.ForvusUpdateDate,
                IncAdjReasonDescription = "", //requestIn.SchoolRequest.InclusionAdjustmentReasons != null ? requestIn.SchoolRequest.InclusionAdjustmentReasons.IncAdjReasonDescription : "",
                IncAdjReasonID = 0, //requestIn.SchoolRequest.InclusionAdjustmentReasons.IncAdjReasonID,
                ReasonID = 0, //requestIn.SchoolRequest.SchoolRequestChanges.FirstOrDefault().Reasons != null ? requestIn.SchoolRequest.SchoolRequestChanges.FirstOrDefault().Reasons.ReasonID : 0,
                ReasonText = "", //requestIn.SchoolRequest.SchoolRequestChanges.FirstOrDefault().Reasons != null ? requestIn.SchoolRequest.SchoolRequestChanges.FirstOrDefault().Reasons.ReasonText : "",
                RequestBy = requestIn.RequestBy,
                RequestDate = requestIn.RequestDate,
                RequestID = requestIn.SchoolRequest.SchoolRequestID,
                RequestType = requestIn.RequestType,
                ScrutinyReasonDescription = "",//requestIn.SchoolRequest.Reasons.ReasonText,
                ScrutinyReasonID = requestIn.SchoolReasonId,
                ScrutinyStatusCode = requestIn.SchoolRequest.SchoolRequestChanges.FirstOrDefault().ScrutinyStatus.ScrutinyStatusCode,
                ScrutinyStatusDescription = requestIn.SchoolRequest.SchoolRequestChanges.FirstOrDefault().ScrutinyStatus.ScrutinyStatusDescription,
                StudentID = null,
                SuggestedDecision = requestIn.SuggestedDecision,
                ScrutinyReasonStatus = "",//(requestIn.SchoolRequest.Reasons.IsRejection.HasValue == false ? "" : (requestIn.SchoolRequest.Reasons.IsRejection.Value ? "invalid" : "valid")),
                OriginalAmendCode = "", //requestIn.OriginalAmendCode
                DocumentLocation = requestIn.DocumentLocation,
                DocumentType = requestIn.DocumentType
            };

            return requestOut;
        }

        public static CohortAdjustmentRequestEntity TranslateDataContractCohortAdjustmentStudentRequestToBusinessEntityCohortAdjustmentRequest(CohortAdjustmentRequest requestIn)
        {
            CohortAdjustmentRequestEntity requestOut = new CohortAdjustmentRequestEntity
            {
                StudentRequest = new StudentRequests
                {
                    InclusionAdjustmentReasons = new InclusionAdjustmentReasons { IncAdjReasonID = requestIn.IncAdjReasonID },
                    Reasons = new Reasons { ReasonID = requestIn.ScrutinyReasonID },
                    StudentRequestID = requestIn.RequestID,
                    Students = new Students { StudentID=requestIn.StudentID.Value },
                },
                 ParentInfochangeID=requestIn.ParentInfoChangeID
            };

            requestOut.StudentRequest.StudentRequestChanges = new System.Data.Objects.DataClasses.EntityCollection<StudentRequestChanges>();
            requestOut.StudentRequest.StudentRequestChanges.Add(
                new StudentRequestChanges
                {
                    AmendCodes = new AmendCodes { AmendCode = requestIn.AmendCode },
                    Changes = new Changes { ChangeID = requestIn.ChangeID },
                    DCSFNotification = requestIn.DCSFNotification,
                    Comments = requestIn.Comments,
                    ScrutinyStatus = new ScrutinyStatus { ScrutinyStatusCode = requestIn.ScrutinyStatusCode },
                    Reasons = new Reasons { ReasonID = requestIn.ReasonID} // 0 means no reason selected
                }
            );
            return requestOut;
        }

        public static CohortAdjustmentRequestEntity TranslateDataContractCohortAdjustmentSchoolRequestToBusinessEntityCohortAdjustmentRequest(CohortAdjustmentRequest requestIn)
        {
            CohortAdjustmentRequestEntity requestOut = new CohortAdjustmentRequestEntity
            {
                SchoolRequest = new SchoolRequests
                {
                    SchoolRequestID= requestIn.RequestID,
                    Schools = new Web09.Checking.DataAccess.Schools{ DFESNumber=requestIn.DCSFNumber.Value }
                }
                ,
                ParentInfochangeID = 0,
                SchoolReasonId = requestIn.ScrutinyReasonID
            };

            requestOut.SchoolRequest.SchoolRequestChanges = new System.Data.Objects.DataClasses.EntityCollection<SchoolRequestChanges>();
            requestOut.SchoolRequest.SchoolRequestChanges.Add(
                new SchoolRequestChanges
                {
                    //AmendCodes = new AmendCodes { AmendCode = requestIn.AmendCode },
                    Changes = new Changes { ChangeID = requestIn.ChangeID },
                    DCSFNotification = requestIn.DCSFNotification,
                    Comments = requestIn.Comments,
                    ScrutinyStatus = new ScrutinyStatus { ScrutinyStatusCode = requestIn.ScrutinyStatusCode }
                    //Reasons = new Reasons { ReasonID = requestIn.ReasonID } // 0 means no reason selected
                }
            );
            return requestOut;
        }
    }
}
