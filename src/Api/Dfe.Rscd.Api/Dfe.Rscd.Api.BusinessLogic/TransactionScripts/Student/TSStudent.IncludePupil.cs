using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Transactions;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.Business.Logic.Validation;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent : Logic.TSBase
    {
        public static void IncludePupil(int studentID, short adjustmentReasonID, List<StudentRequestData> promptResponses)
            {
                using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope() )
                {
                    using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                    {
                        conn.Open();

                        using (Web09_Entities context = new Web09_Entities(conn))
                        {
                            Students student = GetStudent(context, studentID);
                            InclusionAdjustmentReasons adjustmentReason = GetInclusionAdjustmentReason(context,adjustmentReasonID);

                            // Record the change
                            //Users user = context.Users.First();
                            ChangeType ct = context.ChangeType.First();
                            Changes change = new Changes
                            {
                                ChangeDate = DateTime.Now,
                                ChangeTypeID = ct.ChangeTypeID
                            };
                            context.AddToChanges(change);

                            ScrutinyStatus scrutinyStatus;
                            Reasons scrutinyDecisionReason;
                            GetRejectionReasonAndScrutinyStatus(context, out scrutinyDecisionReason, out scrutinyStatus);

                            // Record the adjustment                                                                     
                            StudentRequests studentRequest = new StudentRequests
                            {
                                Changes = change,
                                InclusionAdjustmentReasons = context.InclusionAdjustmentReasons.First(iar=>iar.IncAdjReasonID==adjustmentReasonID),
                                Students = student,
                                Reasons = scrutinyDecisionReason
                            };

                            //TODO: Fix this to use new schema.
                            StudentRequestChanges requestChangeObj = new StudentRequestChanges
                            {
                               Changes = change,
                               AmendCodes = null,
                               Reasons = null,  //scrutinyDecsionReason,
                               ScrutinyStatus = scrutinyStatus
                            };

                            studentRequest.StudentRequestChanges.Add(requestChangeObj);

                            context.AddToStudentRequests(studentRequest);

                            // Save responses to prompts
                            promptResponses.ForEach(pr =>
                            {
                                pr.Prompts = context.Prompts.First(p => p.PromptID == pr.Prompts.PromptID);
                                pr.StudentRequests = studentRequest;
                                context.AddToStudentRequestData(pr);
                            }
                            );

                            context.SaveChanges();
                        }
                    }

                    transaction.Complete();
                }
            }

        private static void GetRejectionReasonAndScrutinyStatus(Web09_Entities context, out Reasons reason, out ScrutinyStatus status)
        {
            // TODO: Implement proper business logic to obtain the scrutiny status for the given adjustment reason
            status = context.ScrutinyStatus.First(s => s.ScrutinyStatusCode == "A");
            reason = context.Reasons.First();
        }

        public static GetAdjustmentReasonsResponse GetInclusionAdjustmentReasonsList(int studentId, string pInclString, string schoolINDNORPROX, PromptAnswer initialQnAnswer)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    string pincl_String;
                    if (studentId != 0)
                    {
                        pincl_String = pInclString;
                    }
                    else
                    {
                        pincl_String = "0";
                    }

                    List<Prompts> furtherPrompts = new List<Prompts>();
                    PromptAnswerList answerList = new PromptAnswerList();
                    if (pincl_String.Equals("406"))
                    {
                        if (initialQnAnswer == null)
                        {
                            furtherPrompts.Add(TSIncludeRemovePupil.GetPromptByPromptID(200));
                            return new GetAdjustmentReasonsResponse(furtherPrompts);
                        }
                        else if (TSIncludeRemovePupil.IsPromptAnswerComplete(initialQnAnswer))
                        {
                            if (initialQnAnswer.PromptYesNoAnswer.Value)
                            {
                                return new GetAdjustmentReasonsResponse(new CompletedNonStudentAdjustment(TSIncludeRemovePupil.GetInfoPromptText(210)));
                            }
                            else
                            {
                                answerList.Add(initialQnAnswer);
                                return new GetAdjustmentReasonsResponse(new CompletedStudentAdjustment(
                                    studentId,
                                    Contants.INCLUSION_ADJUSTMENT_REASON_WAS_PUPIL_ON_ROLL_ON_ANNUAL_CENSUS_DATE,
                                    answerList,
                                    Contants.SCRUTINY_REASON_NEVER_ON_ROLL,
                                    null,
                                    Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                                    TSIncludeRemovePupil.GetInfoPromptText(220))
                                    );
                            }
                        }
                        else
                        {
                            //Answer has been given but is a null
                            throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
                        }
                    }
                    else if (pincl_String.Equals("424"))
                    {
                        if (initialQnAnswer == null)
                        {
                            furtherPrompts.Add(TSIncludeRemovePupil.GetPromptByPromptID(200));
                            return new GetAdjustmentReasonsResponse(furtherPrompts);
                        }
                        else if (TSIncludeRemovePupil.IsPromptAnswerComplete(initialQnAnswer))
                        {
                            if (initialQnAnswer.PromptYesNoAnswer.Value)
                            {
                                return new GetAdjustmentReasonsResponse(GetInclusionAdjustmentReasons(context, pincl_String), TSIncludeRemovePupil.GetInfoPromptText(210));
                            }
                            else
                            {
                                answerList.Add(initialQnAnswer);
                                return new GetAdjustmentReasonsResponse(new CompletedStudentAdjustment(
                                    studentId,
                                    Contants.INCLUSION_ADJUSTMENT_REASON_WAS_PUPIL_ON_ROLL_ON_ANNUAL_CENSUS_DATE,
                                    answerList,
                                    Contants.SCRUTINY_REASON_NEVER_ON_ROLL,
                                    null,
                                    Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                                    TSIncludeRemovePupil.GetInfoPromptText(220))
                                    );
                            }
                        }
                        else
                        {
                            //Answer has been given but is a null
                            throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
                        }
                    }
                    else if (pincl_String.Equals("421") || pincl_String.Equals("422") || pincl_String.Equals("423"))
                    {
                        if (initialQnAnswer == null)
                        {
                            furtherPrompts.Add(TSIncludeRemovePupil.GetPromptByPromptID(100));
                            return new GetAdjustmentReasonsResponse(furtherPrompts);
                        }
                        else if (TSIncludeRemovePupil.IsPromptAnswerComplete(initialQnAnswer))
                        {
                            if (initialQnAnswer.PromptYesNoAnswer.Value)
                            {
                                return new GetAdjustmentReasonsResponse(GetInclusionAdjustmentReasons(context, pincl_String), TSIncludeRemovePupil.GetInfoPromptText(5110));

                            }
                            else
                            {

                                var promptId = 130;
                                if (schoolINDNORPROX == "1")
                                {
                                    promptId = 131;
                                }
                                answerList.Add(initialQnAnswer);
                                return new GetAdjustmentReasonsResponse(new CompletedStudentAdjustment(
                                    studentId,
                                    Contants.INCLUSION_ADJUSTMENT_REASON_WAS_PUPIL_ON_ROLL_ON_ANNUAL_CENSUS_DATE,
                                    answerList,
                                    Contants.SCRUTINY_REASON_NOT_ON_ROLL,
                                    null,
                                    Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                                    TSIncludeRemovePupil.GetInfoPromptText(promptId))
                                    );
                            }
                        }
                        else
                        {
                            //Answer has been given but is a null
                            throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
                        }
                    }
                    else if (pincl_String.Equals("521"))
                    {
                        if (initialQnAnswer == null)
                        {
                            furtherPrompts.Add(TSIncludeRemovePupil.GetPromptByPromptID(5100));
                            return new GetAdjustmentReasonsResponse(furtherPrompts);
                        }
                        else if (TSIncludeRemovePupil.IsPromptAnswerComplete(initialQnAnswer))
                        {
                            if (initialQnAnswer.PromptYesNoAnswer.Value)
                            {
                                string promptText = string.Empty;
                                if (initialQnAnswer.PromptID == 100)
                                {
                                    promptText = TSIncludeRemovePupil.GetInfoPromptText(130);
                                }

                                //From 5110, according to defects by e-mail for CC11-18
                                return new GetAdjustmentReasonsResponse(GetInclusionAdjustmentReasons(context, pincl_String), promptText);
                            }
                            else
                            {
                                answerList.Add(initialQnAnswer);
                                return new GetAdjustmentReasonsResponse(new CompletedStudentAdjustment(
                                    studentId,
                                    Contants.INCLUSION_ADJUSTMENT_REASON_PUPIL_WAS_NOT_ON_ROLL_AT_CENSUS_DATE,
                                    answerList,
                                    Contants.SCRUTINY_REASON_NOT_ON_ROLL,
                                    null,
                                    Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                                    TSIncludeRemovePupil.GetInfoPromptText(5130))
                                    );
                            }
                        }
                        else
                        {
                            //Answer has been given but is a null
                            throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
                        }
                    }
                    else
                    {
                        return new GetAdjustmentReasonsResponse(GetInclusionAdjustmentReasons(context, pincl_String), null);
                    }
                }
            }
        }


        private static InclusionAdjustmentReasons GetInclusionAdjustmentReason(Web09_Entities context, short adjustmentReasonID)
        {
            try
            {
                // Check that an adjustment reason exists with this ID
                return context.InclusionAdjustmentReasons.First(ar => ar.IncAdjReasonID == adjustmentReasonID);
            }
            catch
            {
                throw Web09Exception.GetBusinessException(Web09MessageList.AdjustmentReasonInvalidID);
            }
        }

        private static IEnumerable<InclusionAdjustmentReasons> GetInclusionAdjustmentReasons(Web09_Entities context, string p_INCLString)
        {
            bool isJuneChecking = TSResult.IsJuneChecking(context);                                

            List<InclusionAdjustmentReasons> adjReasonsQry =
                context.PINCLInclusionAdjustments
                    .Join(context.InclusionAdjustmentReasons,
                        piad => piad.IncAdjReasonID,
                        iar => iar.IncAdjReasonID,
                        (piad, iar) =>
                            new { piad = piad, iar = iar }
                    )
                    .Where(temp0 => (temp0.piad.P_INCL == p_INCLString) && (temp0.iar.InJuneChecking == (isJuneChecking) ? true : temp0.iar.InJuneChecking))
                    .Select(temp0 => temp0.iar)
                    .Distinct().ToList();


            return adjReasonsQry.OrderBy(x => x.ListOrder);  // TFS 18411
        }
    }
}
