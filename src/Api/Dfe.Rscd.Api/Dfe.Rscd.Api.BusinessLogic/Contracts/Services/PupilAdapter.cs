using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using Web09.Common.Exceptions;
using Web09.Services.DataContracts;
using Web09.Services.FaultContracts;
using Web09.Services.MessageContracts;
using Web09.Services.ServiceContracts;
using Web09.Services.StubAdapters;

using Web09.Checking.DataAccess;
using Web09.Services.Adapters.EntityTranslators;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.Business.Logic.TransactionScripts;
using System.Linq;
using Web09.Checking.Infrastructure.CosmosDb.Core;
using Web09.Checking.Infrastructure.CosmosDb.DTOs;
using Web09.Checking.Infrastructure.CosmosDb.Interfaces;
using Web09.Services.StubAdapters.Entity_Translators;

namespace Web09.Services.Adapters
{
    public class PupilAdapter : IPupilServiceContract
    {
        private readonly IPupilService _service;

        public PupilAdapter(IPupilService service)
        {
            _service = service;
        }

        #region Private members

        private ValidationFailureList ConstructPupilEditWarningList(Web09.Checking.Business.Logic.Validation.Student.ValidationResponse validationResponse)
        {
            ValidationFailureList warningList = new ValidationFailureList();
            foreach (Web09.Checking.Business.Logic.Validation.Student.ValidationFailure warningMessage in validationResponse.WarningMessages)
            {
                warningList.Add(new ValidationFailure
                {
                    Message = warningMessage.Message,
                    PupilFieldEnum = Enum.GetName(typeof(Web09.Checking.Business.Logic.Validation.Student.PupilFieldEnum), warningMessage.PupilField)
                });
            }

            return warningList;
        }

        private void ConstructPupilRejectionReasonLists(Web09.Checking.Business.Logic.Validation.Student.ValidationResponse validationResponse,
            ref List<string> rejectionReasons,
            ref List<string> rejectionPupilFieldEnums)
        {

            foreach (Web09.Checking.Business.Logic.Validation.Student.ValidationFailure validationFailure in validationResponse.ErrorMessages)
            {
                rejectionReasons.Add(validationFailure.Message);
                rejectionPupilFieldEnums.Add(Enum.GetName(typeof(Web09.Checking.Business.Logic.Validation.Student.PupilFieldEnum), validationFailure.PupilField));
            }
        }

        private void ConstructPupilAdjustmentRejectionList(Web09.Checking.Business.Logic.Validation.AdjustmentPromptAnswers.AdjustmentPromptValidationResponse validationResponse,
            ref List<int> promptIdList,
            ref List<string> validationFailureMsgList)
        {
            foreach (Web09.Checking.Business.Logic.Validation.AdjustmentPromptAnswers.AdjustmentPromptValidationFailure validationFailure in validationResponse.ValidationFailures)
            {
                promptIdList.Add(validationFailure.promptId);
                validationFailureMsgList.Add(validationFailure.Message);
            }
        }

        #endregion


        #region IPupilServiceContract Members

        public SearchPupilsResponse SearchPupils(SearchPupilsRequest request)
        {
            try
            {
                var keyStage = request.KeyStage;
                var school = _service.GetSchoolByDFESNumber(keyStage == 4 ? CheckingWindow.KS4June : CheckingWindow.KS5,
                    request.DFESNumber.ToString());
                
                var pupils = _service.QueryPupils(keyStage == 4 ? CheckingWindow.KS4June : CheckingWindow.KS5, 
                    new PupilsSearchRequest { DFESNumber = request.DFESNumber.ToString(), Forename = request.Forename, Surname = request.Surname });

                return new SearchPupilsResponse
                    {PupilDetailsList = DocumentTranslator.TranslatePupilsFromDTO(pupils, school.SchoolName, keyStage)};
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }


        public SearchPupilsV2Response SearchPupilsV2(SearchPupilsRequest request)
        {
            try
            {
                var keyStage = request.KeyStage;
                var school = _service.GetSchoolByDFESNumber(keyStage == 4 ? CheckingWindow.KS4June : CheckingWindow.KS5,
                    request.DFESNumber.ToString());

                var pupils = _service.QueryPupils(keyStage == 4 ? CheckingWindow.KS4June : CheckingWindow.KS5,
                    new PupilsSearchRequest { DFESNumber = request.DFESNumber.ToString(), Forename = request.Forename, Surname = request.Surname });

                return new SearchPupilsV2Response { JSON = DocumentTranslator.TranslatePupilSearchJSONResultFromDTO(pupils, school.SchoolName, keyStage) };
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public AddOrMovePupilResponse AddPupil(AddOrMovePupilRequest request)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public GetPupilResponse GetPupil(GetPupilRequest request)
        {
            try
            {
                GetPupilResponse response = new GetPupilResponse();

                PupilDTO pupil = _service.GetByCandidateNumber(CheckingWindow.KS4June, request.PupilID);
                EstablishmentProxyDTO school;
                short keyStage = 4;

                if (pupil != null)
                {
                    school = _service.GetSchoolByDFESNumber(CheckingWindow.KS4June, pupil.DFESNumber);
                }
                else
                {
                    pupil = _service.GetByCandidateNumber(CheckingWindow.KS5, request.PupilID);
                    if (pupil == null)
                    {
                        throw new Exception("Pupil not found");
                    }
                    school = _service.GetSchoolByDFESNumber(CheckingWindow.KS5, pupil.DFESNumber);
                    keyStage = 5;
                }

                response.PupilDetails = DocumentTranslator.TranslatePupilFromDTO(pupil, school.SchoolName, keyStage);
                return response;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public GetPupilV2Response GetPupilV2(GetPupilV2Request request)
        {
            try
            {
                GetPupilV2Response response = new GetPupilV2Response();

                PupilDTO pupil = _service.GetByCandidateNumber(CheckingWindow.KS4June, request.PupilID);
                EstablishmentProxyDTO school;
                short keyStage = 4;

                if (pupil != null)
                {
                    school = _service.GetSchoolByDFESNumber(CheckingWindow.KS4June, pupil.DFESNumber);
                }
                else
                {
                    pupil = _service.GetByCandidateNumber(CheckingWindow.KS5, request.PupilID);
                    school = _service.GetSchoolByDFESNumber(CheckingWindow.KS5, pupil.DFESNumber);
                    keyStage = 5;
                }

                response.JSON = DocumentTranslator.TranslatePupilJSONResultFromDTO(pupil, school.SchoolName, keyStage);

                return response;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public AddOrMovePupilResponse MovePupil(AddOrMovePupilRequest request)
        {
            throw new NotImplementedException();
        }

        public GetPupilAdjustmentReasonsResponse GetPupilAdjustmentReasons(GetPupilAdjustmentReasonsRequest request)
        {
            try
            {
                GetPupilAdjustmentReasonsResponse responseMsg = new GetPupilAdjustmentReasonsResponse();

                Web09.Checking.Business.Logic.Entities.PromptAnswer initialQnAnswer = null;

                if (request.InitialQuestionAnswer != null)
                    initialQnAnswer = TranslateBetweenDataContractPromptAnswersAndBusinessEntityPromptAnswers
                        .TranslateDataContractPromptAnswerToBusinessEntityPromptAnswer(request.InitialQuestionAnswer);

                var pupil = GetPupil(new GetPupilRequest { PupilID = request.PupilID});

                Web09.Checking.Business.Logic.Entities.GetAdjustmentReasonsResponse businessResponse = 
                    TSStudent.GetInclusionAdjustmentReasonsList(request.PupilID, pupil.PupilDetails.PINCLCode, "0" , initialQnAnswer);

                //If an adjustment was created and completed, save it to the database
                if (businessResponse.IsComplete && businessResponse.IsAdjustmentCreated && businessResponse.CompletedRequest != null)
                {
                    Web09.Checking.Business.Logic.Entities.UserContext userContext = TranslateBetweenDataContractUserContextAndBusinessEntityUserContext
                        .TranslateDataContractUserContextToBusinessEntityUserContext(request.UserContext);

                    if (request.PupilAdjustmentType == PupilAdjustmentType.PupilInclusionAdjustmentOnly ||
                        request.PupilAdjustmentType == PupilAdjustmentType.PupilRemovalAdjustmentOnly)
                    {
                        int pupilId = request.PupilID;
                        int studentRequestId;
                        //TSStudent.IncludeRemovePupil(ref pupilId, businessResponse.CompletedRequest, userContext, out studentRequestId);
                        businessResponse.CompletedRequest.StudentRequestID = 121233;
                    }
                    else if (request.PupilAdjustmentType == PupilAdjustmentType.InclusionAdjustmentForPupilAdd)
                    {
                        //September functionality
                        throw new NotImplementedException();
                    }
                    else if (request.PupilAdjustmentType == PupilAdjustmentType.InclusionAdjustmentForPupilMove)
                    {
                        //September functionality
                        throw new NotImplementedException();
                    }

                    responseMsg.CompletedAdjustment = TranslateBetweenDataContractCompletedPupilAdjustmentAndBusinessEntityCompletedStudentAdjustment
                        .TranslateBusinessEntityCompletedStudentAdjustmentToDataContractCompletedPupilAdjustment(businessResponse.CompletedRequest);
                }
                else if (!businessResponse.IsComplete && businessResponse.AdjustmentReasonList != null && businessResponse.AdjustmentReasonList.Count() > 0)
                {
                    //Adjutment reasons to be displayed along with prior message.
                    responseMsg.AdjustmentReasons = TranslateBetweenDataContractAdjustmentReasonsAndBusinessEntityAdjustmentReasons
                        .TranslateBusinessEntityAdjustmentReasonsToDataContractAdjustmentReasons(businessResponse.AdjustmentReasonList);

                    responseMsg.PriorMessage = businessResponse.PriorMessage;
                }
                else if (!businessResponse.IsComplete && businessResponse.FurtherPrompts != null && businessResponse.FurtherPrompts.Count > 0)
                {
                    responseMsg.Prompt = TranslateBetweenDataContractPromptAndBusinessEntityPrompt
                        .TranslateBusinessEntityPromptToDataContractPrompt(businessResponse.FurtherPrompts[0]);
                }
                else if (businessResponse.IsComplete && !businessResponse.IsAdjustmentCreated)
                {
                    responseMsg.CompletedNonAdjustment = TranslateBetweenDataContractCompletedPupilAdjustmentAndBusinessEntityCompletedStudentAdjustment
                        .TranslateBusinessEntityCompletedNonAdjustmentToDataContractCompletedNonAdjustment(businessResponse.CompletedNonRequest);
                }

                return responseMsg;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }


        public ProcessInclusionPromptsResponse GetAdjustmentPrompts(GetAdjustmentPromptsRequest request)
        {
            try
            {
                GetAdjustmentPromptsResponse response = new GetAdjustmentPromptsResponse();

                bool isAddPupilRequest;
                if (request.PupilAdjustmentType == PupilAdjustmentType.InclusionAdjustmentForPupilAdd ||
                    request.PupilAdjustmentType == PupilAdjustmentType.InclusionAdjustmentForPupilMove)
                    isAddPupilRequest = true;
                else
                    isAddPupilRequest = false;

                Students student = TranslateBetweenDataContractPupilAndBusinessEntityPupil
                    .TranslateDataContractPupilToBusinessEntityStudent(request.PupilDetails, isAddPupilRequest);

                StudentAdjustmentType adjustmentType = TranslateBetweenDataContractPupilAdjustmentTypeAndBusinessEntityStudentAdjustmentType
                    .TranslanteDatacontractPupilAdjustmentTypeToBusinessEntityStudentAdjustmentType(request.PupilAdjustmentType);

                AdjustmentPromptAnalysis promptAnalysis = TSIncludeRemovePupil.GetAdjustmentPrompts(request.DFESNumber, student, request.PupilInclusionReasonID, adjustmentType);


                //If the adjustment is already complete, save it to the database.
                if (promptAnalysis.IsAdjustmentCreated && promptAnalysis.IsComplete)
                {
                    
                }

                return CreateAdjustmentPromptResponse(promptAnalysis);
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }


        public ProcessInclusionPromptsResponse ProcessInclusionPromptResponses(ProcessInclusionPromptsRequest request)
        {
            try
            {
                bool isAddPupilRequest;
                if (request.PupilAdjustmentType == PupilAdjustmentType.InclusionAdjustmentForPupilAdd ||
                    request.PupilAdjustmentType == PupilAdjustmentType.InclusionAdjustmentForPupilMove)
                    isAddPupilRequest = true;
                else
                    isAddPupilRequest = false;


                //Translate the pupil details into a Students business object
                Students studentBusObj = TranslateBetweenDataContractPupilAndBusinessEntityPupil
                    .TranslateDataContractPupilToBusinessEntityStudent(request.PupilDetails, isAddPupilRequest);

                //Translate the Data Contract Prompt Answers to Business Entity Prompt Answers.
                Web09.Checking.Business.Logic.Entities.PromptAnswerList promptAnswers = TranslateBetweenDataContractPromptAnswersAndBusinessEntityPromptAnswers
                    .TranslateDataContractPromptAnswerListToBusinessEntityPromptAnswerList(request.PromptAnswerList);

                //Run the PromptAnswer validations
                Web09.Checking.Business.Logic.Validation.AdjustmentPromptAnswers.AdjustmentPromptValidationResponse validationResponse =
                    Web09.Checking.Business.Logic.Validation.AdjustmentPromptAnswers.ValidateAdjustmentPromptAnswers(studentBusObj, promptAnswers);

                if (!validationResponse.IsValid)
                {
                    List<int> promptIdList = new List<int>();
                    List<string> validationFailureList = new List<string>();

                    ConstructPupilAdjustmentRejectionList(validationResponse, ref promptIdList, ref validationFailureList);

                    throw Web09Exception.GetException(
                        new FaultException<InvalidAdjustmentPromptAnswers>(
                            new InvalidAdjustmentPromptAnswers
                            {
                                PromptIDList = promptIdList,
                                ValidationFailureMsgList = validationFailureList
                            })
                            );
                }
                else
                {

                    //Run the transaction
                    AdjustmentPromptAnalysis promptAnalysis = TSIncludeRemovePupil.ProcessInclusionPromptResponses(request.DFESNumber, studentBusObj, request.PupilInclusionReasonID, promptAnswers);

                    //If the adjustment is complete and an adjustment was created, save the adjustment.
                    if (promptAnalysis.IsComplete && promptAnalysis.IsAdjustmentCreated && promptAnalysis.CompletedRequest != null)
                    {
                        Debug.WriteLine("Save");
                    }


                    return CreateAdjustmentPromptResponse(promptAnalysis);
                }
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        private ProcessInclusionPromptsResponse CreateAdjustmentPromptResponse(AdjustmentPromptAnalysis promptAnalysis)
        {
            ProcessInclusionPromptsResponse response = new ProcessInclusionPromptsResponse();

            if (promptAnalysis.IsComplete)
            {
                response.IsAdjustmentComplete = true;
                response.PromptList = null;

                //Fill out the scrutiny status
                if (promptAnalysis.IsAdjustmentCreated)
                {
                    response.CompletedAdjustment = TranslateBetweenDataContractCompletedPupilAdjustmentAndBusinessEntityCompletedStudentAdjustment
                        .TranslateBusinessEntityCompletedStudentAdjustmentToDataContractCompletedPupilAdjustment(promptAnalysis.CompletedRequest);
                    response.IsAdjustmentCreated = true;
                }
                else
                {
                    response.CompletedNonAdjustment = TranslateBetweenDataContractCompletedPupilAdjustmentAndBusinessEntityCompletedStudentAdjustment
                        .TranslateBusinessEntityCompletedNonAdjustmentToDataContractCompletedNonAdjustment(promptAnalysis.CompletedNonRequest);
                    response.IsAdjustmentCreated = false;
                }
            }
            else
            {
                response.IsAdjustmentComplete = false;
                response.PromptList = TranslateBetweenDataContractPromptAndBusinessEntityPrompt
                    .TranslateBusinessEntityPromptListToDataContractPromptList(promptAnalysis.FurtherPrompts); ;
            }

            return response;
        }

        public void SubmitAddPupil(SubmitAddPupilRequest request)
        {
            throw new NotImplementedException();
        }

        public SubmitEditPupilResponse SubmitEditPupil(SubmitEditPupilRequest request)
        {
            try
            {
                SubmitEditPupilResponse editPupilResponse = new SubmitEditPupilResponse();

                //No warnings exist.    
                editPupilResponse.WarningList = null;

                //Set parameter to indicate if pupil has been saved.
                editPupilResponse.IsPupilSaved = true;

                return editPupilResponse;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public ValidatePupilResponse ValidatePupil(ValidatePupilRequest request)
        {
            try
            {
                ValidatePupilResponse validatePupilResponse = new ValidatePupilResponse();
                validatePupilResponse.IsPupilValid = true;

                validatePupilResponse.FilledPupil = request.Pupil;

                return validatePupilResponse;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }


        public GetPupilWithResultsCountResponse GetPupilWithResultsCount(GetPupilWithResultsCountRequest request)
        {
            try
            {
                return new GetPupilWithResultsCountResponse
                {
                    PupilWithResultsCount = 6
                };
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public GetPupilWithResultsPageResponse GetPupilWithResultsPage(GetPupilWithResultsPageRequest request)
        {
            try
            {

                return new GetPupilWithResultsPageResponse
                {
                    TotalResultCount = 6,
                    PupilWithResultsList = DataBuilder.CreateResults()
                };
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public AddOrMovePupilResponse IncludeRemovePupil(IncludeRemovePupilRequest request)
        {
            try
            {
                // TODO: Remove Pupil Adjustment Reasons
                return new AddOrMovePupilResponse { PupilID = request.PupilID };

            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public GetPupilsWithoutResultResponse GetPupilsWithoutResult(GetPupilsWithoutResultRequest request)
        {
            try
            {
                var dfesNumber = request.DCSFNumber;
                return new GetPupilsWithoutResultResponse
                {
                    PupilList = DataBuilder.CreatePupilDetailsList(5)
                };
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public DoesStudentHaveOutstandingAdjustmentResponse DoesStudentHaveOutstandingAdjustment(DoesStudentHaveOutstandingAdjustmentRequestMsg request)
        {
            return new DoesStudentHaveOutstandingAdjustmentResponse { HasOutstandingAdjustment = false };
        }

        public GetPupilRequestInfoResponse GetPupilRequestInfo(GetPupilRequestInfoRequest request)
        {
            try
            {
                return new GetPupilRequestInfoResponse
                {
                    CompletedAdjustment = new CompletedPupilAdjustment
                    {
                        PupilID = 1,
                        ForvusId = 233,
                        ScrutinyReasonID = 1,
                        ScrutinyStatusCode = "233",
                        ScrutinyStatusDescription = "Under Scrutiny",
                        PromptAnswerList = new DataContracts.PromptAnswerList()
                    },
                    WarningMessageList = new ValidationFailureList(),
                    ErrorMessageList = new ValidationFailureList(),
                    InformationMessageList = new ValidationFailureList()
                };
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }


        public void SubmitEditPupilWithCompletedAdjustment(SubmitEditPupilWithCompletedAdjustmentRequest request)
        {
            try
            {
                Debug.WriteLine("SubmitEdit");

            }
            catch (Exception ex)
            {
                if (ex is InvalidPupilException)
                {
                    throw Web09Exception.GetException(
                        new FaultException<PupilHasInvalidDetailsFault>(
                        new PupilHasInvalidDetailsFault
                        {
                            PupilID = ((InvalidPupilException)ex).PupilID,
                            RejectionMessages = ((InvalidPupilException)ex).RejectionMessages,
                            RejectionPupilFieldEnums = ((InvalidPupilException)ex).RejectionPupilFieldEnums
                        }
                        ));
                }
                else
                    throw Web09Exception.GetException(ex);
            }
        }

        public void AttachPupilAdjustmentEvidence(AttachPupilAdjustmentEvidenceRequest request)
        {
            Debug.WriteLine("AttachPupilEvidence");
        }

        public GetPupilRequestByPupilIDResponse GetPupilRequestByPupilID(GetPupilRequestByPupilIDRequest request)
        {
            try
            {
                return new GetPupilRequestByPupilIDResponse
                {
                    CompletedAdjustment = new CompletedPupilAdjustment
                    {
                        PupilID = request.PupilID,
                        ForvusId = 233,
                        ScrutinyReasonID = 1,
                        ScrutinyStatusCode = "233",
                        ScrutinyStatusDescription = "Under Scrutiny",
                        PromptAnswerList = new DataContracts.PromptAnswerList()
                    },
                    AdjustmentRequestInfo = new CohortAdjustmentRequest()
                };
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public void CancelPupilRequest(CancelPupilRequestMsg request)
        {
            Debug.WriteLine("Cancel");
        }


        public GetAddNewPupilAdjReasonsResponse GetAddNewPupilAdjustmentReasons(GetAddNewPupilAdjReasonsRequest request)
        {
            GetAddNewPupilAdjReasonsResponse response = new GetAddNewPupilAdjReasonsResponse();
            response.AdjustmentReasons = TranslateBetweenDataContractAdjustmentReasonsAndBusinessEntityAdjustmentReasons
                        .TranslateBusinessEntityAdjustmentReasonsToDataContractAdjustmentReasons(TSStudent.GetAddNewStudentAdjustmentReasons(request.DCSFNumber, request.KeyStage));

            return response;
        }


        public GetResultsForMultipleStudentsResponse GetResultsForMultipleStudents(GetResultsForMultipleStudentsRequest request)
        {
            try
            {
                return new GetResultsForMultipleStudentsResponse
                {
                    Results = DataBuilder.CreateResults()
                };
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }

        public SubmitDAEditPupilResponse SubmitDAEditPupil(SubmitDAEditPupilRequest request)
        {
            try
            {
                SubmitDAEditPupilResponse editPupilResponse = new SubmitDAEditPupilResponse();

                //Set parameter to indicate if pupil has been saved.
                editPupilResponse.IsPupilSaved = true;

                return editPupilResponse;
            }
            catch (Exception ex)
            {
                throw Web09Exception.GetException(ex);
            }
        }


        #endregion


    }
}
