using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{   
    public partial class TSIncludeRemovePupil
    {
                
        internal static StudentRequests SaveAdjustmentRequest(Web09_Entities context, CompletedStudentAdjustment adjustment, Changes changeObject)
        {
            //Create the inclusion request before the transaction so that it
            //may be returned following the completion of the transaction.
            StudentRequests inclusionRequest = new StudentRequests();

            var alreadyHaveRequest = (from sr in context.StudentRequests
                                      join src in context.StudentRequestChanges on sr.StudentRequestID equals src.StudentRequestID
                                      where sr.Students.StudentID == adjustment.StudentID
                                      && src.DateEnd == null
                                      && src.ScrutinyStatus.ScrutinyStatusCode != "C"
                                      select sr);

            if (alreadyHaveRequest.ToList().Count != 0)
                throw new BusinessLevelException("You have made an adjustment request for this pupil");
            

            if (adjustment.InclusionReasonID.HasValue)
                inclusionRequest.InclusionAdjustmentReasons = context.InclusionAdjustmentReasons.First(iar => iar.IncAdjReasonID == adjustment.InclusionReasonID.Value);


            //Create the StudentRequestChanges object to attach to the request.
            StudentRequestChanges requestChangeObject = new StudentRequestChanges();
            requestChangeObject.ScrutinyStatus = context.ScrutinyStatus
                .Where(ss => ss.ScrutinyStatusCode == adjustment.ScrutinyStatusCode)
                .Select(ss => ss)
                .First();

            requestChangeObject.Changes = changeObject;

            //Set the Amend code to the default amend code for the request reason if one exists.
            List<AmendCodes> amdCodesList = context.Reasons
                .Where(rr => rr.ReasonID == adjustment.ScrutinyReasonID)
                .Select(rr => rr.AmendCodes).ToList();
            if (amdCodesList.Count == 1) requestChangeObject.AmendCodes = amdCodesList[0];

            inclusionRequest.StudentRequestChanges.Add(requestChangeObject);


            //Set the reasonid for the StudentRequest object:
            //If request is rejected, set to rejection reason
            //If request is pending or accepted, set to scrutiny reason.
            Reasons acceptOrRejectReason;
            if (!adjustment.RejectionReasonCode.HasValue)
            {
                //Add the Inclusion adjustment request reason
                acceptOrRejectReason = context.Reasons
                    .Where(r => r.ReasonID == adjustment.ScrutinyReasonID)
                    .Select(r => r)
                    .First();

                inclusionRequest.Reasons = acceptOrRejectReason;

                if(acceptOrRejectReason.IsRejection.HasValue && acceptOrRejectReason.IsRejection.Value)
                {
                    //If the request is invalid and rejected no reason required for child change object
                    requestChangeObject.Reasons = null;
                }
                else
                {
                    //If the request is valid and rejected reason IS required for child change object
                    requestChangeObject.Reasons = acceptOrRejectReason;
                }
            }
            else
            {
                //

                //  => Request was rejected, set the reason to the RejectionReasonCode
                acceptOrRejectReason = context.Reasons
                    .Where(r => r.ReasonID == adjustment.RejectionReasonCode)
                    .First();

                inclusionRequest.Reasons = acceptOrRejectReason;

                if (adjustment.ScrutinyStatusCode == Contants.SCRUTINY_STATUS_PENDINGFORVUS ||
                    adjustment.ScrutinyStatusCode == Contants.SCRUTINY_STATUS_PENDINGDCSF ||
                    (acceptOrRejectReason.IsRejection.HasValue && !acceptOrRejectReason.IsRejection.Value))
                {
                    //If the request is either pending, or it is a valid reason and accepted, then no reason is required for child change object.
                    requestChangeObject.Reasons = null;
                }
                else
                {
                    //If the request is invalid and accepted reason IS required for child change object
                    requestChangeObject.Reasons = acceptOrRejectReason;
                }
            }

            //Set the student association
            inclusionRequest.Students = context.Students
                .Where(s => s.StudentID == adjustment.StudentID)
                .Select(s => s).First();            

            //Set the associated change object.
            inclusionRequest.Changes = changeObject;

            // regenerate forvus index if not provided
            if (!inclusionRequest.Students.ForvusIndex.HasValue || inclusionRequest.Students.ForvusIndex.Value == 0)
            {
                // Get student from DB context apply property changes
                Students studentToUpdate = context.Students
                    .Include("Cohorts")
                    .Include("Schools")
                    .Where(s => s.StudentID == inclusionRequest.Students.StudentID).FirstOrDefault();
                TSStudent.AttachNewForvusIndexNumber(context, ref studentToUpdate, false);
                context.ApplyPropertyChanges("Students", studentToUpdate);
            }

            //Add the student request object.
            context.AddToStudentRequests(inclusionRequest);
            context.SaveChanges();

            //Complete the prompt answers
            if (adjustment.PromptAnswerList != null && adjustment.PromptAnswerList.Count > 0)
            {
                foreach (PromptAnswer answer in adjustment.PromptAnswerList)
                {

                    //For information prompts, no answer is necessary.
                    if (answer.PromptAnswerType != PromptAnswer.PromptAnswerTypeEnum.Info)
                    {
                        //Create a student request object.
                        StudentRequestData requestData = new StudentRequestData
                        {
                            StudentRequestID = inclusionRequest.StudentRequestID,
                            PromptID = (short)answer.PromptID,
                            StudentRequests = inclusionRequest,
                            Prompts = context.Prompts.Where(p => p.PromptID == answer.PromptID).First()
                        };

                        switch (answer.PromptAnswerType)
                        {
                            case (PromptAnswer.PromptAnswerTypeEnum.Date):
                                requestData.PromptValue = (answer.PromptDateTimeAnswer.HasValue) ? answer.PromptDateTimeAnswer.Value.ToString("yyyyMMdd") : "";
                                break;
                            case (PromptAnswer.PromptAnswerTypeEnum.Info):
                                //No answer required.
                                break;
                            case (PromptAnswer.PromptAnswerTypeEnum.Integer):
                                requestData.PromptValue = answer.PromptIntegerAnswer.ToString();
                                break;
                            case (PromptAnswer.PromptAnswerTypeEnum.ListBox):
                                requestData.PromptValue = answer.PromptSelectedValueAnswer.ToString();
                                break;
                            case (PromptAnswer.PromptAnswerTypeEnum.Text):
                                requestData.PromptValue = (answer.PromptStringAnswer == null) ? "" : answer.PromptStringAnswer;
                                break;
                            case (PromptAnswer.PromptAnswerTypeEnum.YesNo):
                                requestData.PromptValue = (answer.PromptYesNoAnswer.Value) ? "Yes" : "No";
                                break;
                            default:
                                //Should never occur.
                                break;
                        }

                        context.AddToStudentRequestData(requestData);
                        context.SaveChanges();
                    }
                }

                // TFS 25530 create a Student change for new admission date, if we have a completed prompt for "Please revise admission date if available"              
                foreach (PromptAnswer answer in adjustment.PromptAnswerList)
                {                                       

                    if ( answer.ColumnType != null && 
                         answer.ColumnType.Equals("OnrollDate", StringComparison.CurrentCultureIgnoreCase) && 
                         answer.PromptAnswerType != null &&
                         answer.PromptAnswerType == PromptAnswer.PromptAnswerTypeEnum.Date && 
                         answer.PromptShortText != null &&
                         answer.PromptShortText.Equals("Please revise admission date if available", StringComparison.CurrentCultureIgnoreCase) &&
                         answer.PromptDateTimeAnswer.HasValue )
                    {                        
                        DateTime entrydat = answer.PromptDateTimeAnswer.Value;

                        StudentAndChange studentAndChange       = GetNewStudentChange(context, changeObject, adjustment.StudentID);
                        studentAndChange.StudentChange.ENTRYDAT = entrydat.Year.ToString() + entrydat.Month.ToString("00") + entrydat.Day.ToString("00");

                        studentAndChange.SaveStudentChange(context);
                    }
                }
            }

            return inclusionRequest;
        }       
               

        class StudentAndChange
        {
            public Students Student { get; set; }
            public StudentChanges StudentChange { get; set; }

            public void SaveStudentChange(Web09_Entities context)
            {
                Student.StudentChanges.Add(StudentChange);
                context.SaveChanges();
            }
        }

        private static StudentAndChange GetNewStudentChange(Web09_Entities context, Changes changeObject, int studentID)
        {
            StudentAndChange studentAndChange = new StudentAndChange
            {
                Student = context.Students.Include("StudentChanges").Where(s => s.StudentID == studentID).FirstOrDefault(),
                StudentChange = new StudentChanges()
            };

            studentAndChange.StudentChange.Students = studentAndChange.Student;
            studentAndChange.StudentChange.Changes = changeObject;

            StudentChanges studentChangeExisting = context.StudentChanges
                .Include("YearGroups")
                .Include("Ethnicities")
                .Include("Languages")
                .Include("SENStatus")
                .Where(s => s.StudentID == studentID && s.DateEnd == null)
                .OrderBy(s => s.ChangeID)
                .FirstOrDefault();

            studentAndChange.StudentChange.StudentStatus   = context.StudentStatus.Where(ss => ss.StudentStatusID == Contants.STUDENT_STATUS_ID_AMENDED).First();
            studentAndChange.StudentChange.ENTRYDAT        = studentChangeExisting.ENTRYDAT;
            studentAndChange.StudentChange.Surname         = studentChangeExisting.Surname;
            studentAndChange.StudentChange.Forename        = studentChangeExisting.Forename;
            studentAndChange.StudentChange.Gender          = studentChangeExisting.Gender;
            studentAndChange.StudentChange.DOB             = studentChangeExisting.DOB;
            studentAndChange.StudentChange.Age             = studentChangeExisting.Age;
            studentAndChange.StudentChange.PostCode        = studentChangeExisting.PostCode;
            studentAndChange.StudentChange.YearGroups      = context.YearGroups.Where(yg => yg.YearGroupCode == studentChangeExisting.YearGroups.YearGroupCode).FirstOrDefault();
            studentAndChange.StudentChange.UPN             = studentChangeExisting.UPN;
            studentAndChange.StudentChange.AMDFlag         = studentChangeExisting.AMDFlag;
            studentAndChange.StudentChange.NORFLAGE        = studentChangeExisting.NORFLAGE;
            studentAndChange.StudentChange.FSM             = studentChangeExisting.FSM;
            studentAndChange.StudentChange.Ethnicities     = context.Ethnicities.Where(e => e.EthnicityCode == studentChangeExisting.Ethnicities.EthnicityCode).First();
            studentAndChange.StudentChange.Languages       = context.Languages.Where(l => l.LanguageCode == studentChangeExisting.Languages.LanguageCode).First();
            studentAndChange.StudentChange.SENStatus       = (!(studentChangeExisting.SENStatus == null)) ? context.SENStatus.Where(sen => sen.SENStatusCode == studentChangeExisting.SENStatus.SENStatusCode).First() : null;
            studentAndChange.StudentChange.LookedAfterEver = studentChangeExisting.LookedAfterEver;
            studentAndChange.StudentChange.Languages       = (!(studentChangeExisting.Languages == null)) ? context.Languages.Where(l => l.LanguageCode == studentChangeExisting.Languages.LanguageCode).First() : null;

            studentChangeExisting.DateEnd = DateTime.Now;

            return studentAndChange;
        }          

        internal static StudentRequests SaveAdjustmentRequest(int? inclusionReasonId, CompletedStudentAdjustment adjustment)           
        {

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {                                  
                    ChangeType changeType = context.ChangeType.Where(ct => ct.ChangeTypeID == 2).First();
                    Changes changeDetails = new Changes();
                    changeDetails.ChangeTypeID = changeType.ChangeTypeID;
                    changeDetails.ChangeDate = DateTime.Now;
                    context.AddToChanges(changeDetails);
                    context.SaveChanges();

                    return SaveAdjustmentRequest(context, adjustment, changeDetails);                     
                
                }
            }
        }

       

    }
}
