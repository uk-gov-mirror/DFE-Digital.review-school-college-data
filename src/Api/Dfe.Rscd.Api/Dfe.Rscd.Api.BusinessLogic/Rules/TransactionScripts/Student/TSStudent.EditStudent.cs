using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Transactions;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;


namespace Web09.Checking.Business.Logic.TransactionScripts
{
	public partial class TSStudent
	{


        public static StudentEditOutcome EditStudent(Students student, PromptAnswerList adjPromptAnswers, UserContext userContext, int previousStudentChangeId)
        {

            using(EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {

                    //A pupil adjustment will be required if any of the following attributes
                    //were altered:
                    //  *   Date of Birth
                    //  *   NC Year Group
                    //  *   AdmissionDate
                    //Check these values to verify if adjustment is required.

                    PupilEditRequest editRequest;
                    List<string> completionMessages = new List<string>();
                    

                    StudentChanges currentStudentChangeObj = context.StudentChanges
                        .Include("YearGroups")
                        .Where(sc => sc.StudentID == student.StudentID && sc.DateEnd == null)
                        .First();

                    if(student.StudentChanges.Count == 0)
                        throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

                    StudentChanges editedStudentChangeObj = student.StudentChanges.First();

                    if (editedStudentChangeObj == null || editedStudentChangeObj.YearGroups == null ||
                        editedStudentChangeObj.DOB == null)
                        throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

                    List<AdjustmentPromptAnalysis> multiRequestAnalysisList = new List<AdjustmentPromptAnalysis>();
                    AdjustmentPromptAnalysis editPupilAdjustmentAnalysis;

                    //Determine if admission date adjustment is required.
                    editPupilAdjustmentAnalysis = IsAdmissionDateAdjustmentRequired(context, student, adjPromptAnswers);
                    if (!editPupilAdjustmentAnalysis.IsComplete || (editPupilAdjustmentAnalysis.IsComplete && editPupilAdjustmentAnalysis.IsAdjustmentCreated))
                        multiRequestAnalysisList.Add(editPupilAdjustmentAnalysis);
                    else if (editPupilAdjustmentAnalysis.IsComplete && editPupilAdjustmentAnalysis.CompletedNonRequest != null &&
                        !string.IsNullOrEmpty(editPupilAdjustmentAnalysis.CompletedNonRequest.RequestCompletionDisplayMessage))
                        completionMessages.Add(editPupilAdjustmentAnalysis.CompletedNonRequest.RequestCompletionDisplayMessage);

                    //Determine if DOB adjustment is required.
                    editPupilAdjustmentAnalysis = IsDOBAdjustmentRequired(context, student, adjPromptAnswers);
                    if (!editPupilAdjustmentAnalysis.IsComplete || (editPupilAdjustmentAnalysis.IsComplete && editPupilAdjustmentAnalysis.IsAdjustmentCreated))
                        multiRequestAnalysisList.Add(editPupilAdjustmentAnalysis);
                    else if (editPupilAdjustmentAnalysis.IsComplete && editPupilAdjustmentAnalysis.CompletedNonRequest != null &&
                        !string.IsNullOrEmpty(editPupilAdjustmentAnalysis.CompletedNonRequest.RequestCompletionDisplayMessage))
                        completionMessages.Add(editPupilAdjustmentAnalysis.CompletedNonRequest.RequestCompletionDisplayMessage);

                    //Determine if DOB adjustment is required.
                    editPupilAdjustmentAnalysis = IsNcYearGroupAdjustmentRequired(context, student, adjPromptAnswers);
                    if (!editPupilAdjustmentAnalysis.IsComplete || (editPupilAdjustmentAnalysis.IsComplete && editPupilAdjustmentAnalysis.IsAdjustmentCreated))
                        multiRequestAnalysisList.Add(editPupilAdjustmentAnalysis);
                    else if (editPupilAdjustmentAnalysis.IsComplete && editPupilAdjustmentAnalysis.CompletedNonRequest != null &&
                        !string.IsNullOrEmpty(editPupilAdjustmentAnalysis.CompletedNonRequest.RequestCompletionDisplayMessage))
                        completionMessages.Add(editPupilAdjustmentAnalysis.CompletedNonRequest.RequestCompletionDisplayMessage);

                    //if a student request is required, but a student request already exists, throw an error.
                    if (multiRequestAnalysisList.Count > 0 && TSStudent.DoesStudentHaveOutstandingAdjustment(context, student.StudentID))
                        throw Web09Exception.GetBusinessException(Web09MessageList.EditStudentOutstandingAdjustmentExists);
                    


                    //Analyse the list of requests to determine next action.
                    if(multiRequestAnalysisList.Count == 1)
                    {
                        if (multiRequestAnalysisList[0].IsComplete)
                        {
                            //Only one request was implicitly raised. Include any outcome 
                            //message in the completion messages list and return with
                            //a status of IsStudentEditSave false.
                            if ((multiRequestAnalysisList[0].IsAdjustmentCreated && 
                                multiRequestAnalysisList[0].CompletedRequest != null &&
                                !string.IsNullOrEmpty(multiRequestAnalysisList[0].CompletedRequest.RequestCompletionDisplayMessage)))   
                            {
                                completionMessages.Add(multiRequestAnalysisList[0].CompletedRequest.RequestCompletionDisplayMessage);
                            }
                            else if(multiRequestAnalysisList[0].CompletedNonRequest != null && !string.IsNullOrEmpty(multiRequestAnalysisList[0].CompletedNonRequest.RequestCompletionDisplayMessage))
                            {
                                completionMessages.Add(multiRequestAnalysisList[0].CompletedNonRequest.RequestCompletionDisplayMessage);
                            }
                            
                            return new StudentEditOutcome(false, true, multiRequestAnalysisList[0], completionMessages);
                        }
                        else 
                        {
                            //Either the adjustment is created and not complete or further prompts to be displayed.
                            //The multiRequestAnalysisList[0] object will indicate which.
                            return new StudentEditOutcome(false, true, multiRequestAnalysisList[0], completionMessages); 
                        }
                    }
                    else if (multiRequestAnalysisList.Count > 1)
                    {
                        List<Prompts> furtherPrompts = new List<Prompts>();
                        bool outstandingPromptsExists = false;
                        int adjRequestsCreated = 0;

                        PromptAnswerList inferredPromptAnswers = new PromptAnswerList();
                        
                        foreach (AdjustmentPromptAnalysis adjAnalysis in multiRequestAnalysisList)
                        {
                            if (!adjAnalysis.IsComplete)
                            {
                                outstandingPromptsExists = true;
                                furtherPrompts.AddRange(adjAnalysis.FurtherPrompts);
                            }
                            else if (adjAnalysis.IsAdjustmentCreated)
                            {
                                //The adjustment has been completed. If it has generated a request,
                                //set the hasAdjRequestBeenCreated to true. If it hasn't we will ignore
                                //it and continue with the pupil edit.
                                inferredPromptAnswers.AddRange(adjAnalysis.CompletedRequest.PromptAnswerList);
                                if (!string.IsNullOrEmpty(adjAnalysis.CompletedRequest.RequestCompletionDisplayMessage))
                                    completionMessages.Add(adjAnalysis.CompletedRequest.RequestCompletionDisplayMessage);

                                adjRequestsCreated++;
                            }
                            else if (adjAnalysis.CompletedNonRequest != null &&
                                !string.IsNullOrEmpty(adjAnalysis.CompletedNonRequest.RequestCompletionDisplayMessage))
                            {
                                //We need to save the message that results even if it's a non request situation.
                                completionMessages.Add(adjAnalysis.CompletedNonRequest.RequestCompletionDisplayMessage);
                            }
                        }

                        if (outstandingPromptsExists)
                        {
                            //Multi-request is not completed.
                            //Further prompts are required - > return those prompts 
                            //still outstanding to the UI.
                            return new StudentEditOutcome(false, true, new AdjustmentPromptAnalysis(furtherPrompts), completionMessages);
                        }
                        else if (adjRequestsCreated > 0)
                        {
                            //No more outstanding prompts, and at least one request has been generated and completed.

                            inferredPromptAnswers = StripAnswerListOfDuplicates(inferredPromptAnswers);
                            AdjustmentPromptAnalysis multiRequestOutcome = new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                                null,
                                inferredPromptAnswers,
                                Contants.SCRUTINY_REASON_MULTIPLE_REQUESTS,
                                null,
                                Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                null)
                                );

                            return new StudentEditOutcome(false, true, multiRequestOutcome, completionMessages);
                        }
                        else
                        {
                            //All adjustments are complete, but no adjustment request
                            //was actually generated. Pupil may be saved.
                            editRequest = new PupilEditRequest { Pupil = student, UserContext = userContext, PreviousStudentChangeID = previousStudentChangeId };
                            SaveStudentEdit(conn, context, editRequest, false, false, false);
                            return new StudentEditOutcome(true, false, null, completionMessages);
                        }

                    }
                    else
                    {
                        //No request actually generated => submit pupil changes for save
                        editRequest = new PupilEditRequest { Pupil = student, UserContext = userContext, PreviousStudentChangeID = previousStudentChangeId };
                        SaveStudentEdit(conn, context, editRequest, false, false, false);
                        return new StudentEditOutcome(true, false, null, completionMessages);
                    }
                }
            }
       }

        public static bool EditStudentByDA(Students student, UserContext userContext, int previousStudentChangeId)
        {
            try
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        // Determine what was changed DOB, DOA or Year group
                        // Save new change record
                        if (userContext.UserName == null)
                            throw Web09Exception.GetBusinessException(Web09MessageList.InvalidStudentEditRequest);
                        if (userContext.UserName == string.Empty)
                            throw Web09Exception.GetBusinessException(Web09MessageList.InvalidStudentEditRequest);

                        // Generate new change object
                        Changes newChange = CreateChangeObject(context, 2, userContext);
                        context.AddToChanges(newChange);

                        StudentChanges currentStudentChange = context.StudentChanges.Include("YearGroups").Where(objSC => objSC.StudentID == student.StudentID && objSC.ChangeID == previousStudentChangeId).FirstOrDefault();

                        if(currentStudentChange==null)
                            throw Web09Exception.GetBusinessException(Web09MessageList.InvalidStudentEditRequest);

                        Int16 promptIDDOA = 0;
                        Int16 promptIDDOB = 0;
                        Int16 promptIDYearGroup = 0;

                        switch (student.Cohorts.KeyStage)
                        {
                            case 2:
                                promptIDDOA = Contants.PROMPT_ID_ADMISSION_DATE_KS2;
                                promptIDDOB = Contants.PROMPT_ID_DOB_KS2;
                                promptIDYearGroup = Contants.PROMPT_ID_NC_YEAR_GROUP_KS2;
                                break;
                            case 3:
                                promptIDDOA = Contants.PROMPT_ID_ADMISSION_DATE_KS3;
                                promptIDDOB = Contants.PROMPT_ID_DOB_KS3;
                                promptIDYearGroup = Contants.PROMPT_ID_NC_YEAR_GROUP_KS3;
                                break;
                            case 4:
                                promptIDDOA = Contants.PROMPT_ID_ADMISSION_DATE_KS4;
                                promptIDDOB = Contants.PROMPT_ID_DOB_KS4;
                                promptIDYearGroup = Contants.PROMPT_ID_NC_YEAR_GROUP_KS4;
                                break;
                            default:
                                promptIDDOA = Contants.PROMPT_ID_ADMISSION_DATE_KS5;
                                promptIDDOB = Contants.PROMPT_ID_DOB_KS5;
                                promptIDYearGroup = Contants.PROMPT_ID_NC_YEAR_GROUP_KS5;
                                break;
                        }

                        // Date of addmission can also have these prompts as well 820,810,21600,31600,1600
                        StudentRequests sr = context.StudentRequests
                            .Include("Students")
                            .Include("StudentRequestData")
                            .Include("StudentRequestData.Prompts")
                            .Include("StudentRequestChanges")
                            .Where(objSR => objSR.Students.StudentID == student.StudentID && !objSR.StudentRequestChanges.Any(src => src.ScrutinyStatus.ScrutinyStatusCode == "C" && src.DateEnd == null)).First();
                        
                        // does any prompt data already exists, if not create one request data and an audit
                        StudentRequestData srdDOB = null;
                        StudentRequestData srdDOA = null;
                        StudentRequestData srdYearGroup = null;
                        StudentRequestData srdDOAOther = null;

                        srdDOB = sr.StudentRequestData.Where(objSRD => objSRD.Prompts.ColumnName == "DoB").FirstOrDefault();
                        srdDOA = sr.StudentRequestData.Where(objSRD => objSRD.Prompts.ColumnName == "OnrollDate").FirstOrDefault();
                        srdYearGroup = sr.StudentRequestData.Where(objSRD => objSRD.Prompts.ColumnName == "ActualYearGroup").FirstOrDefault();
                        
                        // not implemented yet
                        srdDOAOther = sr.StudentRequestData.Where(objSRD => objSRD.PromptID == 810 || objSRD.PromptID == 820 || objSRD.PromptID == 21600 || objSRD.PromptID == 31600 || objSRD.PromptID == 1600).FirstOrDefault();

                        if (student.StudentChanges.First().DOB != null && student.StudentChanges.First().DOB != "" && student.StudentChanges.First().DOB!=currentStudentChange.DOB)
                        {
                            string oldValue = currentStudentChange.DOB;
                            string newValue = student.StudentChanges.First().DOB;

                            if (srdDOB == null)
                            {
                                // create student request data
                                StudentRequestData newSRD = new StudentRequestData();
                                newSRD.Prompts = context.Prompts.Where(p => p.PromptID == promptIDDOB).FirstOrDefault();
                                newSRD.PromptValue = student.StudentChanges.First().DOB;
                                newSRD.StudentRequests = sr;
                                newSRD.UpdateByDA = 1;
                                context.AddToStudentRequestData(newSRD);
                            }
                            else
                            {
                                oldValue = srdDOB.PromptValue;
                                promptIDDOB = srdDOB.PromptID;
                                // Update Existing Data                        
                                srdDOB.PromptValue = student.StudentChanges.First().DOB;
                                srdDOB.UpdateByDA = 1;
                                context.ApplyPropertyChanges("StudentRequestData", srdDOB);
                            }

                            // generate audit record
                            Audit audit = new Audit();
                            audit.Changes = newChange;
                            audit.Prompts = context.Prompts.Where(p=>p.PromptID==promptIDDOB).FirstOrDefault();
                            audit.StudentRequestID = sr.StudentRequestID;
                            audit.StudentRequests = context.StudentRequests.FirstOrDefault(objSR => objSR.StudentRequestID == sr.StudentRequestID);
                            audit.OldValue = oldValue;
                            audit.NewValue = newValue;
                            context.AddToAudit(audit);

                            context.SaveChanges();
                        }

                        if (student.StudentChanges.First().ENTRYDAT != null && student.StudentChanges.First().ENTRYDAT != "" && student.StudentChanges.First().ENTRYDAT!=currentStudentChange.ENTRYDAT)
                        {
                            string oldValue = currentStudentChange.ENTRYDAT;
                            string newValue = student.StudentChanges.First().ENTRYDAT;

                            if (srdDOA == null)
                            {
                                // create student request data
                                StudentRequestData newSRD = new StudentRequestData();
                                newSRD.Prompts = context.Prompts.Where(p => p.PromptID == promptIDDOA).FirstOrDefault(); 
                                newSRD.PromptValue = student.StudentChanges.First().ENTRYDAT;
                                newSRD.StudentRequests = sr;
                                newSRD.UpdateByDA = 1;
                                context.AddToStudentRequestData(newSRD);
                            }
                            else
                            {
                                oldValue = srdDOA.PromptValue;
                                promptIDDOA = srdDOA.PromptID;

                                // Update Existing Data                        
                                srdDOA.PromptValue = student.StudentChanges.First().ENTRYDAT;
                                srdDOA.UpdateByDA = 1;
                                context.ApplyPropertyChanges("StudentRequestData", srdDOA);
                            }

                            // generate audit record
                            Audit audit = new Audit();
                            audit.Changes = newChange;
                            audit.Prompts = context.Prompts.Where(p => p.PromptID == promptIDDOA).FirstOrDefault();
                            audit.StudentRequestID = sr.StudentRequestID;
                            audit.StudentRequests = context.StudentRequests.FirstOrDefault(objSR => objSR.StudentRequestID == sr.StudentRequestID);
                            audit.OldValue = oldValue;
                            audit.NewValue = newValue;
                            context.AddToAudit(audit);

                            context.SaveChanges();
                        }

                        if (student.StudentChanges.First().YearGroups != null && student.StudentChanges.First().YearGroups.YearGroupCode != currentStudentChange.YearGroups.YearGroupCode)
                        {
                            string oldValue = currentStudentChange.YearGroups.YearGroupCode;
                            string newValue = student.StudentChanges.First().YearGroups.YearGroupCode;

                            if (srdYearGroup == null)
                            {
                                // create student request data
                                StudentRequestData newSRD = new StudentRequestData();
                                newSRD.Prompts = context.Prompts.Where(p => p.PromptID == promptIDYearGroup).FirstOrDefault();
                                newSRD.PromptValue = student.StudentChanges.First().YearGroups.YearGroupCode;
                                newSRD.StudentRequests = sr;
                                newSRD.UpdateByDA = 1;
                                context.AddToStudentRequestData(newSRD);
                            }
                            else
                            {
                                // Update Existing Data                        
                                oldValue = srdYearGroup.PromptValue;
                                promptIDYearGroup = srdYearGroup.PromptID;

                                srdYearGroup.PromptValue = student.StudentChanges.First().YearGroups.YearGroupCode;
                                srdYearGroup.UpdateByDA = 1;
                                context.ApplyPropertyChanges("StudentRequestData", srdYearGroup);
                            }

                            // generate audit record
                            Audit audit = new Audit();
                            audit.Changes = newChange;
                            audit.Prompts = context.Prompts.Where(p => p.PromptID == promptIDYearGroup).FirstOrDefault();
                            audit.StudentRequestID = sr.StudentRequestID;
                            audit.StudentRequests = context.StudentRequests.FirstOrDefault(objSR => objSR.StudentRequestID == sr.StudentRequestID);
                            audit.OldValue = oldValue;
                            audit.NewValue = newValue;
                            context.AddToAudit(audit);

                            context.SaveChanges();
                        }

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        private static void SaveStudentEdit(EntityConnection conn, Web09_Entities context, PupilEditRequest request, bool isAdmissionDateRequestRequired, bool isDobRequestRequired, bool isNCYearGroupRequestRequired)
        {
            using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope() )
            {
                // Save new change record
                if (request.UserContext.UserName == null)
                    throw Web09Exception.GetBusinessException(Web09MessageList.InvalidStudentEditRequest);
                if (request.UserContext.UserName == string.Empty)
                    throw Web09Exception.GetBusinessException(Web09MessageList.InvalidStudentEditRequest);

                Changes newChange = CreateChangeObject(context, 2, request.UserContext);
                context.AddToChanges(newChange);
                
                // Throw an exception if the Student is null, has no id or is invalid
                if (request.Pupil == null)
                    throw Web09Exception.GetBusinessException(Web09MessageList.InvalidStudentEditRequest);
                if (request.Pupil.StudentID == 0)
                    throw Web09Exception.GetBusinessException(Web09MessageList.InvalidStudentEditRequest);
                if (request.Pupil.StudentChanges.First() == null)
                    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

                //Determine if a new studentChange object is required
                StudentChanges updatedStudentChange = request.Pupil.StudentChanges.First();

                System.Data.Common.DbConnection con = conn.StoreConnection;
                System.Data.Common.DbCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = con;               


                //For AdmissionDate, DOB, and NCYearGroup:  
                //      If a request is required for these, then they
                //      can't be changed, so should be set to the same
                //      values as they were prior to the edit.
                if (isAdmissionDateRequestRequired)
                {
                    cmd.CommandText = "SELECT ENTRYDAT FROM Student.StudentChanges Where StudentID=" + request.Pupil.StudentID.ToString() + " and DateEnd IS NULL";
                    updatedStudentChange.ENTRYDAT = cmd.ExecuteScalar().ToString();
                }

                if (isDobRequestRequired)
                {
                    cmd.CommandText = "SELECT DOB FROM Student.StudentChanges Where StudentID=" + request.Pupil.StudentID.ToString() + " and DateEnd IS NULL";
                    updatedStudentChange.DOB = cmd.ExecuteScalar().ToString();
                }

                if (isNCYearGroupRequestRequired)
                {
                    cmd.CommandText = "SELECT ActualYearGroup FROM Student.StudentChanges Where StudentID=" + request.Pupil.StudentID.ToString() + " and DateEnd IS NULL";
                    updatedStudentChange.YearGroups = new YearGroups
                    {
                        YearGroupCode = cmd.ExecuteScalar().ToString()
                    };
                }

                bool isNewStudent = false;

                Web09.Checking.Business.Logic.Validation.Student.ValidationResponse validationResponse = Validation.Student.ValidateStudent(
                    context, request.Pupil, isNewStudent,
                    StudentAdjustmentType.InclusionAdjustmentForPupilEdit,
                    request.CompletedAdjustment);

                if (!validationResponse.IsValid)
                {
                    List<string> rejectionReasons = new List<string>();
                    List<string> rejectionPupilFieldEnums = new List<string>();
                    foreach (Web09.Checking.Business.Logic.Validation.Student.ValidationFailure validationFailure in validationResponse.ErrorMessages)
                    {
                        rejectionReasons.Add(validationFailure.Message);
                        rejectionPupilFieldEnums.Add(Enum.GetName(typeof(Web09.Checking.Business.Logic.Validation.Student.PupilFieldEnum), validationFailure.PupilField));
                    }

                    throw new InvalidPupilException("Pupil validation failed.",
                        request.Pupil.StudentID,rejectionReasons,
                        rejectionPupilFieldEnums);
                }

                StudentChanges currentStudentChange = context.StudentChanges
                        .Include("Students")
                        .Include("YearGroups")
                        .Include("Changes")
                        .Where(sc => sc.StudentID == updatedStudentChange.StudentID && sc.DateEnd == null)
                        .First();

                //Concurrency check:    Before accepting changes, ensure the record has not
                //                      been previously updated by another user. Go no further
                //                      with the transaction.
                if (currentStudentChange.ChangeID != request.PreviousStudentChangeID)
                    throw Web09Exception.GetBusinessException(Web09MessageList.TransactionDataConcurrencyError);

                if (IsNewStudentChangeObjectRequired(context, updatedStudentChange))
                {
                    cmd.CommandText = "UPDATE Student.StudentChanges SET DateEnd=GetDate() Where StudentID=" + request.Pupil.StudentID.ToString() + " and DateEnd IS NULL";
                    cmd.ExecuteNonQuery();

                    Ethnicities currentEthnicityParent = context.Ethnicities.Where(e => e.EthnicityCode== currentStudentChange.Ethnicities.EthnicityCode).FirstOrDefault();

                    // updatedStudentChange will have parent ethnicity code 
                    // to compare with 
                    // parent code for current ethnicity code which can be a child or a parent but will have same parent code
                    bool isEthnicityChanged = (currentEthnicityParent.ParentEthnicityCode != updatedStudentChange.Ethnicities.EthnicityCode);
                    bool isLanguageNull = (updatedStudentChange.Languages == null);
                    bool isSENNull = (updatedStudentChange.SENStatus == null);
                        

                    //Ethnicities updatedEthnicity = context.Ethnicities.Where(e => e.ParentEthnicityCode == updatedStudentChange.Ethnicities.EthnicityCode).FirstOrDefault();

                    StudentChanges scInsert = new StudentChanges
                    {
                        Students = context.Students.Where(s => s.StudentID == request.Pupil.StudentID).First(),//currentStudentChange.Students,
                        Changes = newChange,
                        Forename = updatedStudentChange.Forename,
                        Surname = updatedStudentChange.Surname,
                        Gender = updatedStudentChange.Gender,
                        DOB = updatedStudentChange.DOB,
                        UPN = updatedStudentChange.UPN,
                        Age = updatedStudentChange.Age,
                        PostCode = updatedStudentChange.PostCode,
                        ENTRYDAT = updatedStudentChange.ENTRYDAT,
                        YearGroups = context.YearGroups.Where(yg => yg.YearGroupCode == updatedStudentChange.YearGroups.YearGroupCode).First(),
                        Ethnicities = isEthnicityChanged ? context.Ethnicities.Where(e => e.EthnicityCode == updatedStudentChange.Ethnicities.EthnicityCode).First() : context.Ethnicities.Where(e => e.EthnicityCode == currentStudentChange.Ethnicities.EthnicityCode).First(),
                        FSM = updatedStudentChange.FSM,
                        Languages = (!isLanguageNull) ? context.Languages.Where(l => l.LanguageCode == updatedStudentChange.Languages.LanguageCode).First() : null,
                        SENStatus = (!isSENNull) ? context.SENStatus.Where(sen => sen.SENStatusCode == updatedStudentChange.SENStatus.SENStatusCode).First() : null,
                        LookedAfterEver = updatedStudentChange.LookedAfterEver,
                        AMDFlag = currentStudentChange.AMDFlag,
                        NORFLAGE = currentStudentChange.NORFLAGE,
                        StudentStatus = context.StudentStatus.Where(ss => ss.StudentStatusID == Contants.STUDENT_STATUS_ID_AMENDED).First()
                    };

                    // student update to foorvus ref
                    //Apply a new forvus index if the current number is null or 0.                    
                    if (!request.Pupil.ForvusIndex.HasValue || request.Pupil.ForvusIndex.Value == 0)
                    {
                        // Get student from DB context apply property changes
                        Students studentToUpdate = context.Students
                            .Include("Cohorts")
                            .Include("Schools")
                            .Where(s => s.StudentID == request.Pupil.StudentID).FirstOrDefault();
                        AttachNewForvusIndexNumber(context, ref studentToUpdate, false);
                        context.ApplyPropertyChanges("Students", studentToUpdate);
                    }

                    context.AddToStudentChanges(scInsert);
                    context.SaveChanges();
                }
                else
                {
                    //Student did not have any changes that required an update. If no requests exist,
                    //throw an error to indicate no changes were saved.
                    if (request.CompletedAdjustment == null)
                        throw Web09Exception.GetBusinessException(Web09MessageList.NoStudentChangeDetected);
                }
                
                // Record the adjustment if applicable
                if (request.CompletedAdjustment != null)
                {
                    request.CompletedAdjustment.InclusionReasonID = 30;
                    TSIncludeRemovePupil.SaveAdjustmentRequest(context, request.CompletedAdjustment, newChange);
                }

                //Everything is successful, accept changes, close transaction.
                context.AcceptAllChanges();
                transaction.Complete();
            }
        }

        /// <summary>
        /// Save a student that has a completed adjustment attached.
        /// </summary>
        /// <param name="request">An object that contains the student, </param>
        public static void SaveStudentEditWithCompletedAdjustment(PupilEditRequest request)
        {

            using(EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    //Firstly, need to reverse engineer which field(s) the adjustment is for based on the pupil details.
                    Students updatedStudent = request.Pupil;

                    bool isAdmissionDateAdjustmentRequired = false;
                    bool isDOBAdjustmentRequired = false;
                    bool isNCYearChangeAdjustmentRequired = false;

                    AdjustmentPromptAnalysis promptAnalysis;
                    PromptAnswerList inferredPromptAnswers = new PromptAnswerList();

                    promptAnalysis = IsAdmissionDateAdjustmentRequired(context, updatedStudent, request.CompletedAdjustment.PromptAnswerList);
                    if ((promptAnalysis.IsComplete && promptAnalysis.IsAdjustmentCreated))
                        isAdmissionDateAdjustmentRequired = true;
                    

                    promptAnalysis = IsDOBAdjustmentRequired(context, updatedStudent, request.CompletedAdjustment.PromptAnswerList);
                    if ((promptAnalysis.IsComplete && promptAnalysis.IsAdjustmentCreated))
                        isDOBAdjustmentRequired = true;
                    

                    promptAnalysis = IsNcYearGroupAdjustmentRequired(context, updatedStudent, request.CompletedAdjustment.PromptAnswerList);
                    if ((promptAnalysis.IsComplete && promptAnalysis.IsAdjustmentCreated))
                        isNCYearChangeAdjustmentRequired = true;

                    SaveStudentEdit(conn, context, request, isAdmissionDateAdjustmentRequired, isDOBAdjustmentRequired, isNCYearChangeAdjustmentRequired);
                    
                }
            }
        }

        #region Determine if adjustments are required.

        /// <summary>
        /// Determine if an adjustment is generated for the Admission date for an edited pupil.
        /// </summary>
        /// <param name="context">Web09 entity object contxt.</param>
        /// <param name="updatedStudent">The updated student details</param>
        /// <param name="priorPromptAnswers">Any prompt answers previously provided.</param>
        /// <returns></returns>
        private static AdjustmentPromptAnalysis IsAdmissionDateAdjustmentRequired(Web09_Entities context, Students updatedStudent, PromptAnswerList priorPromptAnswers)
        {
            StudentChanges editedStudentChangeObj = updatedStudent.StudentChanges.First();

            StudentChanges currentStudentChangeObj = context.StudentChanges
                        .Include("YearGroups")
                        .Where(sc => sc.StudentID == updatedStudent.StudentID && sc.DateEnd == null)
                        .First();

            if (!editedStudentChangeObj.ENTRYDAT.Trim().Equals(currentStudentChangeObj.ENTRYDAT.Trim()) && updatedStudent.Cohorts.KeyStage != 5 && updatedStudent.Cohorts.KeyStage != 3)
            {
                PromptAnswerList inferredPromptAnswers = new PromptAnswerList();
                inferredPromptAnswers.Add(GetInferredAdmissionDatePromptAnswer(updatedStudent));
                if (priorPromptAnswers != null && priorPromptAnswers.Count > 0) inferredPromptAnswers.AddRange(priorPromptAnswers);

                return TSIncludeRemovePupil.ProcessAdmissionDateForPupilEdit(context,
                                    updatedStudent.Cohorts.KeyStage,
                                    GetAdmissionDatePromptID(updatedStudent),
                                    inferredPromptAnswers,
                                    updatedStudent.StudentID,
                                    null);
            }
            else
                return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(null));
        }

        /// <summary>
        /// Determine if an adjustment is generated for the DOB for an edited pupil.
        /// </summary>
        /// <param name="context">Web09 entity object contxt.</param>
        /// <param name="updatedStudent">The updated student details</param>
        /// <param name="priorPromptAnswers">Any prompt answers previously provided.</param>
        /// <returns></returns>
        private static AdjustmentPromptAnalysis IsDOBAdjustmentRequired(Web09_Entities context, Students updatedStudent, PromptAnswerList priorPromptAnswers)
        {
            StudentChanges editedStudentChangeObj = updatedStudent.StudentChanges.First();
            
            StudentChanges currentStudentChangeObj = context.StudentChanges
                        .Include("YearGroups")
                        .Where(sc => sc.StudentID == updatedStudent.StudentID && sc.DateEnd == null)
                        .First();

            if (editedStudentChangeObj.DOB != currentStudentChangeObj.DOB && updatedStudent.Cohorts.KeyStage != 3)
            {
                PromptAnswerList inferredPromptAnswers = new PromptAnswerList();
                inferredPromptAnswers.Add(GetInferredDOBPromptAnswer(updatedStudent));
                if (priorPromptAnswers != null && priorPromptAnswers.Count > 0) inferredPromptAnswers.AddRange(priorPromptAnswers);

                return TSIncludeRemovePupil.ProcessDOBAdjustmentRequest(updatedStudent, null, inferredPromptAnswers);
                
            }
            else
                return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(null));
        }

        /// <summary>
        /// Determine if an adjustment is generated for the NC Year group for an edited pupil.
        /// </summary>
        /// <param name="context">Web09 entity object contxt.</param>
        /// <param name="updatedStudent">The updated student details</param>
        /// <param name="priorPromptAnswers">Any prompt answers previously provided.</param>
        /// <returns></returns>
        private static AdjustmentPromptAnalysis IsNcYearGroupAdjustmentRequired(Web09_Entities context, Students updatedStudent, PromptAnswerList priorPromptAnswers)
        {
            StudentChanges currentStudentChangeObj = context.StudentChanges
                        .Include("YearGroups")
                        .Where(sc => sc.StudentID == updatedStudent.StudentID && sc.DateEnd == null)
                        .First();


            if (updatedStudent.StudentChanges.First().YearGroups.YearGroupCode != currentStudentChangeObj.YearGroups.YearGroupCode && updatedStudent.Cohorts.KeyStage != 3)
            {
                PromptAnswerList inferredPromptAnswers = new PromptAnswerList();
                inferredPromptAnswers.Add(GetInferredNCYearGroupPromptAnswer(updatedStudent));
                if (priorPromptAnswers != null && priorPromptAnswers.Count > 0) inferredPromptAnswers.AddRange(priorPromptAnswers);

                return TSIncludeRemovePupil.ProcessNCYearGroup(context, updatedStudent, null, inferredPromptAnswers, null);                
            }
            else
                return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(null));

        }

        #endregion


        #region GetInferredPromptAnswers

        /// <summary>
        /// If an adjustment is required due to a change to the student's Admission Date attribute,
        /// this method will generate the PromptAnswer object by extracting the new value from
        /// the updated pupil
        /// </summary>
        /// <param name="updatedStudent">The student who has been updated to produce the new DOB value.</param>
        /// <returns></returns>
        private static PromptAnswer GetInferredAdmissionDatePromptAnswer(Students updatedStudent)
        {
            StudentChanges editedStudentChangeObj = updatedStudent.StudentChanges.First();

            int admissionDatePromptId = GetAdmissionDatePromptID(updatedStudent);

            PromptAnswer autoGeneratedAnswer;
            autoGeneratedAnswer = new PromptAnswer(admissionDatePromptId);
            autoGeneratedAnswer.PromptAnswerType = PromptAnswer.PromptAnswerTypeEnum.Date;

            if (!String.IsNullOrEmpty(editedStudentChangeObj.ENTRYDAT))
                autoGeneratedAnswer.PromptDateTimeAnswer = new DateTime(
                    int.Parse(editedStudentChangeObj.ENTRYDAT.Substring(0, 4)),
                    int.Parse(editedStudentChangeObj.ENTRYDAT.Substring(4, 2)),
                    int.Parse(editedStudentChangeObj.ENTRYDAT.Substring(6, 2)));
            else
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            return autoGeneratedAnswer;

        }

        /// <summary>
        /// If an adjustment is required due to a change to the student's DOB attribute,
        /// this method will generate the PromptAnswer object by extracting the new value from
        /// the updated pupil
        /// </summary>
        /// <param name="updatedStudent">The student who has been updated to produce the new DOB value.</param>
        /// <returns></returns>
        private static PromptAnswer GetInferredDOBPromptAnswer(Students updatedStudent)
        {
            StudentChanges editedStudentChangeObj = updatedStudent.StudentChanges.First();

            int dobPromptId;

            switch (updatedStudent.Cohorts.KeyStage)
            {
                case (2):
                    dobPromptId = Contants.PROMPT_ID_DOB_KS2;
                    break;
                case (3):
                    dobPromptId = Contants.PROMPT_ID_DOB_KS3;
                    break;
                case (4):
                    dobPromptId = Contants.PROMPT_ID_DOB_KS4;
                    break;
                case (5):
                    dobPromptId = Contants.PROMPT_ID_DOB_KS5;
                    break;
                default:
                    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);
            }
            PromptAnswer autoGeneratedAnswer;

            autoGeneratedAnswer = new PromptAnswer(dobPromptId);
            autoGeneratedAnswer.PromptAnswerType = PromptAnswer.PromptAnswerTypeEnum.Date;

            if (!String.IsNullOrEmpty(editedStudentChangeObj.DOB))
                autoGeneratedAnswer.PromptDateTimeAnswer = new DateTime(
                    int.Parse(editedStudentChangeObj.DOB.Substring(0, 4)),
                    int.Parse(editedStudentChangeObj.DOB.Substring(4, 2)),
                    int.Parse(editedStudentChangeObj.DOB.Substring(6, 2)));
            else
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            return autoGeneratedAnswer;

        }

        /// <summary>
        /// If an adjustment is required due to a change to the student's NC Year group attribute,
        /// this method will generate the PromptAnswer object by extracting the new value from
        /// the updated pupil
        /// </summary>
        /// <param name="updatedStudent">The student who has been updated to produce the new NC Year group value.</param>
        /// <returns></returns>
        private static PromptAnswer GetInferredNCYearGroupPromptAnswer(Students updatedStudent)
        {
            StudentChanges editedStudentChangeObj = updatedStudent.StudentChanges.First();

            int ncYearGroupPromptId;

            switch (updatedStudent.Cohorts.KeyStage)
            {
                case (2):
                    ncYearGroupPromptId = Contants.PROMPT_ID_NC_YEAR_GROUP_KS2;
                    break;
                case (3):
                    ncYearGroupPromptId = Contants.PROMPT_ID_NC_YEAR_GROUP_KS3;
                    break;
                case (4):
                    ncYearGroupPromptId = Contants.PROMPT_ID_NC_YEAR_GROUP_KS4;
                    break;
                case (5):
                    ncYearGroupPromptId = Contants.PROMPT_ID_NC_YEAR_GROUP_KS5;

                    break;
                default:
                    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);
            }

            PromptAnswer autoGeneratedAnswer;

            autoGeneratedAnswer = new PromptAnswer(ncYearGroupPromptId);
            autoGeneratedAnswer.PromptAnswerType = PromptAnswer.PromptAnswerTypeEnum.Integer;

            int newYearGroup;

            if (int.TryParse(editedStudentChangeObj.YearGroups.YearGroupCode, out newYearGroup))
                autoGeneratedAnswer.PromptIntegerAnswer = newYearGroup;
            else
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            return autoGeneratedAnswer;
        }

        /// <summary>
        /// Get the admission date prompt id based on the students key stage.
        /// </summary>
        /// <param name="student">The student for whom the key stage is required.</param>
        /// <returns>The key-stage-sensitive admission date prompt id</returns>
        private static int GetAdmissionDatePromptID(Students student)
        {
            int admissionDatePromptId;

            switch (student.Cohorts.KeyStage)
            {
                case (2):
                    admissionDatePromptId = Contants.PROMPT_ID_ADMISSION_DATE_FOR_PUPIL_EDIT_KS2;
                    break;
                case (3):
                    admissionDatePromptId = Contants.PROMPT_ID_ADMISSION_DATE_KS3;
                    break;
                case (4):
                    admissionDatePromptId = Contants.PROMPT_ID_ADMISSION_DATE_FOR_PUPIL_EDIT_KS4;
                    break;
                case (5):
                    throw Web09Exception.GetBusinessException(Web09MessageList.InvalidKS5AdmissionDateRequest);
                default:
                    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);
            }

            return admissionDatePromptId;
        }

        #endregion

        #region isNewStudentChangeObjectRequired

        private static bool IsNewStudentChangeObjectRequired(Web09_Entities context, StudentChanges updatedStudentChange)
        {
            StudentChanges currentStudentChange = context.StudentChanges
                .Include("Ethnicities")
                .Include("Languages")
                .Include("SENStatus")
                .Include("YearGroups")
                .Where(sc => sc.StudentID == updatedStudentChange.StudentID && sc.DateEnd == null)
                .First();

            bool isNewChangeRequired = false;

            if (updatedStudentChange.Surname != currentStudentChange.Surname) isNewChangeRequired = true;
            if (updatedStudentChange.Forename != currentStudentChange.Forename) isNewChangeRequired = true;
            if (updatedStudentChange.Gender != currentStudentChange.Gender) isNewChangeRequired = true;
            if (updatedStudentChange.DOB != currentStudentChange.DOB) isNewChangeRequired = true;
            if (updatedStudentChange.ENTRYDAT != currentStudentChange.ENTRYDAT) isNewChangeRequired = true;
            if (updatedStudentChange.YearGroups.YearGroupCode != currentStudentChange.YearGroups.YearGroupCode) isNewChangeRequired = true;
            if (updatedStudentChange.PostCode != currentStudentChange.PostCode) isNewChangeRequired = true;
            if (updatedStudentChange.UPN != currentStudentChange.UPN) isNewChangeRequired = true;

            //If both the current of existing ethnicities are not null, compare if they've changed.
            //If either current or existing is null, compare if they are both null, if not, ethnicities has obviously changed
            if (updatedStudentChange.Ethnicities != null && currentStudentChange.Ethnicities != null)
            {
                if (updatedStudentChange.Ethnicities.EthnicityCode != currentStudentChange.Ethnicities.ParentEthnicityCode) isNewChangeRequired = true;
            }
            else
            {
                if (!(updatedStudentChange.Ethnicities == null && currentStudentChange.Ethnicities == null)) isNewChangeRequired = true;
            }

            if (updatedStudentChange.FSM != currentStudentChange.FSM) isNewChangeRequired = true;

            //If both the current of existing languages are not null, compare if they've changed.
            //If either current or existing is null, compare if they are both null, if not, language has obviously changed
            if (currentStudentChange.Languages != null && updatedStudentChange.Languages != null)
            {
                if (updatedStudentChange.Languages.LanguageCode != currentStudentChange.Languages.LanguageCode) isNewChangeRequired = true;
            }
            else
            {
                if (!(currentStudentChange.Languages == null && updatedStudentChange.Languages == null)) isNewChangeRequired = true;
            }

            //If both the current of existing SENStatus are not null, compare if they've changed.
            //If either current or existing is null, compare if they are both null, if not, SENStatus has obviously changed
            if (updatedStudentChange.SENStatus != null && currentStudentChange.SENStatus != null)
            {
                if (updatedStudentChange.SENStatus.SENStatusCode != currentStudentChange.SENStatus.SENStatusCode) isNewChangeRequired = true;
            }
            else
            {
                if (!(currentStudentChange.SENStatus == null && updatedStudentChange.SENStatus == null)) isNewChangeRequired = true;
            }

            if (updatedStudentChange.LookedAfterEver != currentStudentChange.LookedAfterEver) isNewChangeRequired = true;

            return isNewChangeRequired;

        }

        #endregion

        #region Private routines

        private static PromptAnswerList StripAnswerListOfDuplicates(PromptAnswerList answersIn)
        {
            
            PromptAnswerList answersOut = new PromptAnswerList();
            bool hasDuplicateEntries;

            for (int i = 0; i < answersIn.Count; i++)
            {
                hasDuplicateEntries = false;

                for (int j = 0; j <= i; j++)
                {
                    if (answersIn[i].PromptID == answersIn[j].PromptID && i != j)
                    {
                        hasDuplicateEntries = true;
                        break;
                    }
                }

                if (!hasDuplicateEntries) answersOut.Add(answersIn[i]);
            }

            return answersOut;
        }

        #endregion       
    }
}
