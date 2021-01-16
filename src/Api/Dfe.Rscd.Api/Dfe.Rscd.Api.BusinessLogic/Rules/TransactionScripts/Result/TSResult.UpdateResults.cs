using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Transactions;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
        private static ResultStatus GetResultStatusEntityForLateResults(Web09_Entities context, string status)
        {
            ResultStatus statusEntity = (from rs in context.ResultStatus
                                         where rs.ResultStatusDescription == status
                                                && rs.DataOrigin.DataOriginID == 2
                                          select rs).First();

            return statusEntity;
        }

        private static void CreateResultRequest(Web09_Entities context, ResultChanges rc, bool EvidenceRequired, bool AcceptOnEntry)
        {
            
            ResultEvidence re = new ResultEvidence();          

            ScrutinyStatus ss = new ScrutinyStatus();

            if ((AcceptOnEntry && EvidenceRequired))
            {
                //get 'Accept' scrutinycode entry
                ss = (from s in context.ScrutinyStatus
                                     where s.ScrutinyStatusDescription == "Accept"
                                     select s).First();               
            }
            else if (!EvidenceRequired)
            {
                //get 'Accept' scrutinycode entry
                ss = (from s in context.ScrutinyStatus
                      where s.ScrutinyStatusDescription == "Accepted Automatically"
                      select s).First();               
            }
            else
            {
                ss = (from s in context.ScrutinyStatus
                      where s.ScrutinyStatusDescription == "Evidence not provided"
                      select s).First();
            }

            //only if evidence is required but accept on entry is selected, create dummy evidence 
            if (AcceptOnEntry && EvidenceRequired)
            {
                //else create a new evidence entry
                Evidence newE = new Evidence();
                newE.Barcode = "";
                newE.DateCreated = DateTime.Now;
                newE.DocumentLocation = Web09.Services.Common.Web09Constants.SCRUTINY_NOEVIDENCE_FILENAME;
                newE.DocumentType = "text/plain";
                context.AddToEvidence(newE);

                //create a result evidence entry                            
                re.Evidence = newE;
                context.AddToResultEvidence(re);
            }

            //now add an entry in ResultRequest table
            ResultRequests rr = new ResultRequests();           
            rr.Results = rc.Results;
            rr.Changes = rc.Changes;
            rr.ScrutinyStatus = ss;

            if (AcceptOnEntry && EvidenceRequired)
            {
                rr.ResultEvidence = re;
            }

            context.AddToResultRequests(rr);

            ResultRequestChanges rrc = new ResultRequestChanges();            
            rrc.Changes = rc.Changes;
            rrc.ResultRequests = rr;
            rrc.ScrutinyStatus = ss;          
            context.AddToResultRequestChanges(rrc);
        }

        private static void CreateQueryResultRequest(Web09_Entities context, ResultChanges rc)
        {
            ResultEvidence re = new ResultEvidence();
            ScrutinyStatus ss = new ScrutinyStatus();

            ss = (from s in context.ScrutinyStatus
                  where s.ScrutinyStatusDescription == "Query"
                  select s).First();

            //create a new evidence entry which is the 'no evidence' type because it is only a dummy
            Evidence newE = new Evidence();
            newE.Barcode = "";
            newE.DateCreated = DateTime.Now;
            newE.DocumentLocation = "~/documents/NoEvidence.txt";
            newE.DocumentType = "text/plain"; ;
            context.AddToEvidence(newE);

            //create a result evidence entry                        
            re.Evidence = newE;
            context.AddToResultEvidence(re);

            //now add an entry in ResultRequest table
            ResultRequests rr = new ResultRequests();
            rr.Results = rc.Results;
            rr.Changes = rc.Changes;
            rr.ScrutinyStatus = ss;
            rr.ResultEvidence = re;
            context.AddToResultRequests(rr);

            ResultRequestChanges rrc = new ResultRequestChanges();
            rrc.Changes = rc.Changes;
            rrc.ResultRequests = rr;
            rrc.ScrutinyStatus = ss;
            context.AddToResultRequestChanges(rrc);
        }

        public static int UpdateResults(int resultId, int pointId, int? marks, string resultChangeType, bool EvidenceRequired, bool AcceptOnEntry, int resultCurrentChangeId, UserContext uc, string GradeText)
        {
           
                string updateStatus = "success";
                int newChangeId = -1;
                Changes change = null;
                bool isOkToSave = false;
                using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope() )
                {
                    using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                    {
                        conn.Open();

                        using (Web09_Entities context = new Web09_Entities(conn))
                        {
                            ChangeType ct = context.ChangeType.First();

                            //get most recent RC entry (this may or may not be used)
                            ResultChanges prevRC = (from rchg in context.ResultChanges
                                                    where rchg.ResultID == resultId
                                                    orderby rchg.ChangeID descending
                                                    select rchg).First();
                            if (!prevRC.PointsReference.IsLoaded)
                            {
                                prevRC.PointsReference.Load();
                            }
                            if (!prevRC.ResultsReference.IsLoaded)
                            {
                                prevRC.ResultsReference.Load();
                            }

                            Points point = prevRC.Points;
                            //get the points object for the given grade
                            if (pointId != -1)
                            {
                                point = (from p in context.Points
                                         where p.PointID == pointId
                                         select p).First();
                            }                            

                            IQueryable<Results> query1 = context.Results
                                                        .Include("AwardingBodies")
                                                        .Include("Seasons")
                                                        .Include("Students")
                                                        .Include("Subjects")
                                                        .Include("SubLevels")
                                                        .Where(r =>
                                                         r.ResultID == resultId);
                            Results result = query1.First();

                            if (!string.IsNullOrEmpty(GradeText))
                            {
                                result.QANsReference.Load();
                                result.QualificationTypesReference.Load();

                                //create a new points entry if it does not already exist
                                var pointsQuery = (from p in context.Points
                                                   where p.QANs.QANID == result.QANs.QANID
                                                   && p.QualificationTypes.QualificationTypeID == result.QualificationTypes.QualificationTypeID
                                                   && p.GradeCode == GradeText
                                                   select p).ToList();

                                if (pointsQuery.Count > 0) //if found use it
                                {
                                    point = pointsQuery.First();
                                }
                                else
                                {
                                    //create new points entry
                                    //create a point entry
                                    point = new Points();
                                    point.GradeCode = GradeText;
                                    point.GradeDescription = GradeText;
                                    point.QANs = result.QANs;
                                    point.QualificationTypes = result.QualificationTypes;

                                    context.AddToPoints(point);
                                    context.SaveChanges(); //this is needed since ref to QTypes later in the code
                                }
                            }

                            //look if a un-cancelled match exists for the same qualif and session for the same student
                            IEnumerable<Results> matches = context.Results
                                                          .Include("ResultChanges")
                                                          .Include("AwardingBodies")
                                                          .Include("DataOrigin")
                                                          .Where(r => r.Students.StudentID == result.Students.StudentID
                                                                 && r.SubLevels.SubLevelID == result.SubLevels.SubLevelID
                                                                 && r.Subjects.SubjectID == result.Subjects.SubjectID
                                                                 && r.ExamYear == result.ExamYear
                                                                 && r.Seasons.SeasonCode == result.Seasons.SeasonCode
                                                                 && r.ResultChanges.Any(rc => rc.ResultStatus.ResultStatusDescription != "Cancelled")).ToList();

                            if (result.AwardingBodies != null && matches != null)
                            {
                                matches = matches.Where(r => r.AwardingBodies!=null && r.AwardingBodies.AwardingBodyID == result.AwardingBodies.AwardingBodyID);
                            }

                            if (matches.Count() <= 0)
                                isOkToSave = true;

                            //For non-cancellation requests, check if the marks entered are withing the max and min allowed
                            if (!resultChangeType.Contains("Cancelled") && result.SubLevels.SubLevelDescription.Contains("KS2") && marks != null)
                            {
                                var ranges = (from tmr in context.TestMarkRanges
                                              where tmr.SubjectID == result.Subjects.SubjectID
                                              && tmr.ExamYear == result.ExamYear
                                              && tmr.GradeCode == point.GradeCode
                                              && tmr.MinMark <= marks
                                              && tmr.MaxMark >= marks
                                              select tmr).ToList();

                                if (ranges != null && ranges.Count < 1)
                                    throw Web09Exception.GetBusinessException(Web09MessageList.MarksNotInRangeResubmit);
                                //return "Marks are not within correct range. Please modify and re-submit.";

                            }

                            if (resultChangeType.Equals("CancelledAmendment"))
                            {
                                ResultStatus cancelRStatus = (from rs in context.ResultStatus
                                                              where rs.ResultStatusDescription == "Cancelled"
                                                                    && rs.DataOrigin.DataOriginID == 3
                                                              select rs).First();

                                //get most recent RCs
                                var query3 = (from rchg in context.ResultChanges
                                              .Include("ResultStatus")
                                              where rchg.ResultID == resultId
                                              orderby rchg.ChangeID descending
                                              select rchg).ToList();

                                

                                //if there are no more amended entries to cancel, do nothing
                                if (query3.Any(x => x.ResultStatus.ResultStatusDescription=="Amended" && x.DateEnd == null))
                                {
                                    prevRC = query3.First(x => x.ResultStatus.ResultStatusDescription == "Amended" && x.DateEnd == null);

                                    context.Attach(prevRC);

                                    if (prevRC.ChangeID != resultCurrentChangeId)
                                        throw Web09Exception.GetBusinessException(Web09MessageList.TransactionDataConcurrencyError);
                                    else
                                    {
                                        var currentRC = query3.First(x => x.ResultStatus.ResultStatusDescription != "Cancelled" && x.ResultStatus.ResultStatusDescription != "Withdrawn" && x.ResultStatus.ResultStatusDescription != "Rejected" && x.DateEnd != null);
                                        context.Attach(currentRC);
                                        currentRC.DateEnd = null;
                                        context.ApplyPropertyChanges("ResultChanges", currentRC);

                                        prevRC.ResultStatus = cancelRStatus;
                                        prevRC.DateEnd = DateTime.Now;
                                        context.ApplyPropertyChanges("ResultChanges", prevRC);

                                        //set RR and RRC for the Amendment to cancelled as well
                                        var rrQuery = (from rrq in context.ResultRequests
                                                       where rrq.Changes.ChangeID == prevRC.ChangeID
                                                        && rrq.Results.ResultID == prevRC.ResultID
                                                       orderby rrq.ResultRequestID descending
                                                       select rrq).ToList();

                                        if (rrQuery.Count > 0)
                                        {
                                            ScrutinyStatus ss = (from s in context.ScrutinyStatus
                                                                 where s.ScrutinyStatusDescription == "Cancelled"
                                                                 select s).First();

                                            //set the SS code in RR to cancelled
                                            ResultRequests rr = rrQuery.First();
                                            ResultRequests newrr = rr;
                                            newrr.ScrutinyStatus = ss;
                                            
                                            context.Attach(rr);
                                            context.ApplyPropertyChanges("ResultRequests", newrr);

                                            //get the most recent RRC for this RR and set its dateend to now
                                            var rrcQuery = (from rrcq in context.ResultRequestChanges
                                                            where rrcq.ResultRequestID == rr.ResultRequestID
                                                                && rrcq.DateEnd == null
                                                            orderby rrcq.ChangeID descending
                                                            select rrcq).ToList();

                                            if (rrcQuery.Count > 0)
                                            {
                                                ResultRequestChanges rrcOld = rrcQuery.First();
                                                rrcOld.DateEnd = DateTime.Now;
                                                context.ApplyPropertyChanges("ResultRequestChanges", rrcOld);
                                            }

                                            //now add a new entry
                                            change = new Changes { ChangeDate = DateTime.Now, ChangeTypeID = ct.ChangeTypeID, RoleName = uc.RoleName, UserName = uc.UserName, Forename = uc.Forename, Surname = uc.Surname };
                                            context.AddToChanges(change);

                                            //now create a new RRC row with SScode = cancelled
                                            ResultRequestChanges rrc = new ResultRequestChanges();
                                            rrc.Changes = change;
                                            rrc.Comments = "Cancelled due to user action of cancellation.";
                                            rrc.ResultRequests = rr;
                                            rrc.ScrutinyStatus = ss;

                                            context.AddToResultRequestChanges(rrc);
                                        }
                                    }
                                }

                            }

                            if (resultChangeType.Equals("CancelledAddition"))
                            {
                                ResultStatus cancelRStatus = (from rs in context.ResultStatus
                                                              where rs.ResultStatusDescription == "Cancelled"
                                                                    && rs.DataOrigin.DataOriginID == 3
                                                              select rs).First();

                                //get the most recent entry with status='Added'
                                prevRC = (from rchg in context.ResultChanges
                                          where rchg.ResultID == resultId
                                                && rchg.ResultStatus.ResultStatusDescription == "Added"
                                          orderby rchg.ChangeID descending
                                          select rchg).First();

                                ResultChanges newPrevRC = prevRC;

                                if (prevRC.ChangeID != resultCurrentChangeId)
                                    throw Web09Exception.GetBusinessException(Web09MessageList.TransactionDataConcurrencyError);
                                else
                                {
                                    newPrevRC.ResultStatus = cancelRStatus; // TFS 16632 remove this line
                                    newPrevRC.DateEnd = DateTime.Now;
                                    context.ApplyPropertyChanges("ResultChanges", newPrevRC);

                                    //set RR and RRC for the Amendment to cancelled as well
                                    var rrQuery = (from rrq in context.ResultRequests
                                                   where rrq.Changes.ChangeID == prevRC.ChangeID
                                                    && rrq.Results.ResultID == prevRC.ResultID
                                                   orderby rrq.ResultRequestID descending
                                                   select rrq).ToList();

                                    if (rrQuery.Count > 0)
                                    {
                                        ScrutinyStatus ss = (from s in context.ScrutinyStatus
                                                             where s.ScrutinyStatusDescription == "Cancelled"
                                                             select s).First();

                                        //set the SS code in RR to cancelled
                                        ResultRequests rr = rrQuery.First();
                                        ResultRequests newrr = rr;
                                        newrr.ScrutinyStatus = ss;
                                        context.Attach(rr);
                                        context.ApplyPropertyChanges("ResultRequests", newrr);

                                        //get the most recent RRC for this RR and set its dateend to now
                                        var rrcQuery = (from rrcq in context.ResultRequestChanges
                                                        where rrcq.ResultRequestID == rr.ResultRequestID
                                                            && rrcq.DateEnd == null
                                                        orderby rrcq.ChangeID descending
                                                        select rrcq).ToList();

                                        if (rrcQuery.Count > 0)
                                        {
                                            ResultRequestChanges rrcOld = rrcQuery.First();
                                            rrcOld.DateEnd = DateTime.Now;
                                            context.ApplyPropertyChanges("ResultRequestChanges", rrcOld);
                                        }

                                        //now add a new entry
                                        change = new Changes { ChangeDate = DateTime.Now, ChangeTypeID = ct.ChangeTypeID, RoleName = uc.RoleName, UserName = uc.UserName, Forename = uc.Forename, Surname = uc.Surname };
                                        context.AddToChanges(change);

                                        // TFS 16332, now create a new RC row with SScode = cancelled
                                        ResultChanges newRC = new ResultChanges();
                                        newRC.Changes = change;
                                        newRC.Results = result;
                                        newRC.Points = prevRC.Points;
                                        newRC.TierCode = prevRC.TierCode;
                                        newRC.TestMark = prevRC.TestMark;
                                        newRC.Finegrade = prevRC.Finegrade;
                                        newRC.ResultStatus = cancelRStatus;
                                        context.AddToResultChanges(newRC);

                                        //now create a new RRC row with SScode = cancelled
                                        ResultRequestChanges rrc = new ResultRequestChanges();
                                        rrc.Changes = change;
                                        rrc.Comments = "Cancelled due to user action of cancellation.";
                                        rrc.ResultRequests = rr;
                                        rrc.ScrutinyStatus = ss;

                                        context.AddToResultRequestChanges(rrc);
                                    }
                                }
                            }

                            //Late results cases
                            if (resultChangeType.Equals("Undecided") || resultChangeType.Equals("Accepted")
                                    || resultChangeType.Equals("Rejected"))
                            {
                                if (resultChangeType.Equals("Undecided"))
                                {
                                    isOkToSave = true;
                                }

                                //do checks for similar result
                                if (resultChangeType.Equals("Accepted"))
                                {
                                    //check if result exists
                                    if (matches.Count() > 0)
                                    {
                                        var otherMatches = matches.Where(r => r.DataOrigin.DataOriginID != 2).ToList();
                                        if (otherMatches.Count > 0)
                                        {
                                            Results matchR = otherMatches.Last();

                                            //if this is a initial result check if it is withdrawn
                                            if (matchR.DataOrigin.DataOriginID == 1)
                                            {
                                                var withdrawn = (from rc in context.ResultChanges
                                                                 where rc.ResultID == matchR.ResultID
                                                                 && rc.ResultStatus.ResultStatusDescription.Equals("Withdrawn")
                                                                 && rc.DateEnd == null
                                                                 orderby rc.ChangeID ascending
                                                                 select rc).ToList();
                                                // if the result is withdrawn, display warning and save
                                                if (withdrawn.Count > 0)
                                                {

                                                    updateStatus = "Please note the grade conflict between a late result and a request.";
                                                    isOkToSave = true;
                                                }
                                            }

                                            //now get the most recent RC for this matching result
                                            ResultChanges rcMatch = (from rc in context.ResultChanges
                                                                     where rc.ResultID == matchR.ResultID
                                                                        && rc.DateEnd == null
                                                                     orderby rc.ChangeID descending
                                                                     select rc).First();
                                            rcMatch.ResultStatusReference.Load();
                                            rcMatch.PointsReference.Load();

                                            //now check the grade
                                            if (rcMatch.Points.GradeCode == point.GradeCode)
                                            {
                                                //can be saved without warning
                                                isOkToSave = true;
                                            }
                                            else
                                            {
                                                //if most recent RC for the matching result is Amended/Added
                                                if (rcMatch.ResultStatus.ResultStatusDescription.Equals("Amended")
                                                    || rcMatch.ResultStatus.ResultStatusDescription.Equals("Added"))
                                                {
                                                    //need to show warning
                                                    updateStatus = "Please note the grade conflict between a late result and a request.";
                                                    isOkToSave = true;
                                                }
                                                else
                                                {
                                                    //it is ok to save without warning
                                                    isOkToSave = true;
                                                }
                                            } 
                                        }
                                        else
                                            isOkToSave = true;

                                    }
                                }

                                //do checks for similar result
                                if (resultChangeType.Equals("Rejected"))
                                {
                                    //check if result exists
                                    if (matches.Count() > 0)
                                    {
                                        var otherMatches = matches.Where(r => r.DataOrigin.DataOriginID != 2).ToList();
                                        if (otherMatches.Count > 0)
                                        {
                                            Results matchR = otherMatches.Last();

                                            //now get the most recent RC for this matching result
                                            ResultChanges rcMatch = (from rc in context.ResultChanges
                                                                     where rc.ResultID == matchR.ResultID
                                                                     && rc.DateEnd == null
                                                                     orderby rc.ChangeID descending
                                                                     select rc).First();

                                            rcMatch.PointsReference.Load();

                                            //now check the grade
                                            if (rcMatch.Points.GradeCode == point.GradeCode)
                                            {
                                                isOkToSave = false;
                                                throw Web09Exception.GetBusinessException(Web09MessageList.LRMatchesUnamended);
                                                //updateStatus = "This late result matches an unamended or requested result below. Please amend that if not correct.";                                            
                                            }
                                            else
                                            {
                                                //it is ok to save without warning
                                                isOkToSave = true;
                                            }
                                        }
                                        else
                                            isOkToSave = true;
                                    }
                                }

                                if (isOkToSave)
                                {
                                    //pick the latest RC with dataorigin=2 i.e New Grade entry
                                    prevRC = (from rchg in context.ResultChanges
                                              where rchg.ResultID == resultId
                                              orderby rchg.ChangeID descending
                                              select rchg).First();

                                    if (prevRC.ChangeID != resultCurrentChangeId)
                                        throw Web09Exception.GetBusinessException(Web09MessageList.TransactionDataConcurrencyError);
                                    else
                                    {
                                        prevRC.ResultStatusReference.Load();
                                        
                                        if(prevRC.Changes!=null)
                                            prevRC.ChangesReference.Load();

                                        prevRC.PointsReference.Load();

                                        //if it is not already in thet state or is force accepted
                                        if (!prevRC.ResultStatus.ResultStatusDescription.Equals(resultChangeType)
                                             && !prevRC.ResultStatus.ResultStatusDescription.Equals("Force accepted"))
                                        {
                                            // Set old result date end to null
                                            ResultChanges prevResultChange = context.ResultChanges.Where(rchg => rchg.ResultID == resultId && rchg.ChangeID == resultCurrentChangeId).First();
                                            prevResultChange.DateEnd = DateTime.Now;
                                            context.ApplyPropertyChanges("ResultChanges", prevResultChange);

                                            // handle new result change
                                            change = new Changes { ChangeDate = DateTime.Now, ChangeTypeID = ct.ChangeTypeID, RoleName = uc.RoleName, UserName = uc.UserName, Forename = uc.Forename, Surname = uc.Surname };
                                            context.AddToChanges(change);

                                            ResultStatus status = GetResultStatusEntityForLateResults(context, resultChangeType);

                                            ResultChanges rc = new ResultChanges
                                            {
                                                Changes = change,
                                                Points = prevRC.Points,
                                                Results = prevRC.Results,
                                                ResultStatus = status
                                            };
                                            context.AddToResultChanges(rc);
                                        }
                                    }
                                }
                            }


                            if (resultChangeType.Equals("Withdraw"))
                            {
                                //check if result exists
                                if (matches.Count() > 0)
                                {
                                    var lateMatches = matches.Where(r => r.DataOrigin.DataOriginID == 2).ToList();
                                    if (lateMatches.Count > 0)
                                    {
                                        Results matchR = lateMatches.Last(); //get most recent late result

                                        //now get the most recent RC for this matching result
                                        ResultChanges rcMatch = (from rc in context.ResultChanges
                                                                 where rc.ResultID == matchR.ResultID
                                                                 && rc.ResultStatus.DataOrigin.DataOriginID == 2
                                                                 orderby rc.ChangeID descending
                                                                 select rc).First();
                                        rcMatch.ResultStatusReference.Load();
                                        rcMatch.PointsReference.Load();

                                        //now check the grade
                                        if (rcMatch.Points.GradeCode == point.GradeCode)
                                        {
                                            if (rcMatch.ResultStatus.ResultStatusDescription.Equals("Rejected"))
                                            {
                                                isOkToSave = true;
                                            }
                                            else
                                            {
                                                //can be saved with warning
                                                updateStatus = "Please note that the matching late result above has not been rejected.";
                                                isOkToSave = true;
                                            }
                                        }
                                        else
                                        {
                                            if (rcMatch.ResultStatus.ResultStatusDescription.Equals("Rejected"))
                                            {
                                                isOkToSave = true;
                                            }
                                            else
                                            {
                                                //can be saved with warning
                                                updateStatus = "Please note that the matching late result above has not been rejected.";
                                                isOkToSave = true;
                                            }
                                        }
                                    }
                                    else
                                        isOkToSave = true;
                                }

                                if (isOkToSave)
                                {
                                    //first check if it is already withdrawn, if so do nothing
                                    var alreadyWithdrawn = (from rchg in context.ResultChanges
                                                            where rchg.ResultID == resultId
                                                                  && rchg.ResultStatus.ResultStatusDescription == "Withdrawn"
                                                                  && rchg.DateEnd == null
                                                            orderby rchg.ChangeID descending
                                                            select rchg).ToList();

                                    if (alreadyWithdrawn.Count > 0)
                                    {
                                        ResultChanges mostrecentWithdraw = alreadyWithdrawn.First();

                                        //backward compatibility: Cancel any previous withdraw rows that have datened = null
                                        if (alreadyWithdrawn.Count > 1)
                                        {                                            
                                            var queryOtherWithdraw = (from rchg in context.ResultChanges
                                                                      where rchg.ResultID == resultId
                                                                            && rchg.ResultStatus.ResultStatusDescription == "Withdrawn"
                                                                            && rchg.ChangeID < mostrecentWithdraw.ChangeID
                                                                      orderby rchg.ChangeID descending
                                                                      select rchg).ToList();

                                            //set dateend to now for all, these should be set so that they dont
                                            //cause confusion in new logic
                                            for (int i = 0; i < queryOtherWithdraw.Count; i++)
                                            {
                                                ResultChanges oldWithdrawRC = queryOtherWithdraw.ElementAt(i);
                                                oldWithdrawRC.DateEnd = DateTime.Now;
                                                context.ApplyPropertyChanges("ResultChanges", oldWithdrawRC);
                                            }

                                            if (queryOtherWithdraw.Count > 0)
                                                context.SaveChanges();
                                        }
                                    }
                                    else
                                    {                                        
                                        ResultStatus withdrawRStatus = (from rs in context.ResultStatus
                                                                        where rs.ResultStatusDescription == "Withdrawn"
                                                                            && rs.DataOrigin.DataOriginID == 1
                                                                        select rs).First();


                                        var query4 = (from rchg in context.ResultChanges
                                                      where rchg.ResultID == resultId
                                                            && rchg.ResultStatus.ResultStatusDescription == "Unamended"
                                                      orderby rchg.ChangeID descending
                                                      select rchg).ToList();

                                        //get the same grade details from the Unamended row
                                        if (query4.Count > 0)
                                        {
                                            prevRC = query4.First();
                                            prevRC.PointsReference.Load();
                                            prevRC.ResultsReference.Load();

                                            // TFS 16332, amend prevRC so DateEnd is now
                                            prevRC.DateEnd = DateTime.Now;
                                            context.ApplyPropertyChanges("ResultChanges", prevRC);

                                            //now add a new RC entry
                                            change = new Changes { ChangeDate = DateTime.Now, ChangeTypeID = ct.ChangeTypeID, RoleName = uc.RoleName, UserName = uc.UserName, Forename = uc.Forename, Surname = uc.Surname };
                                            context.AddToChanges(change);

                                            //copy grade info from original unamended RC
                                            ResultChanges rc = new ResultChanges
                                            {
                                                Changes = change,
                                                Points = prevRC.Points,
                                                Results = prevRC.Results,
                                                ResultStatus = withdrawRStatus,
                                                TierCode = prevRC.TierCode,
                                                TestMark = prevRC.TestMark,
                                                Finegrade = prevRC.Finegrade
                                            };

                                            context.AddToResultChanges(rc);

                                            CreateResultRequest(context, rc, EvidenceRequired, AcceptOnEntry);

                                        }

                                        //if the recent changeId belongs to a valid Amendment for thihs result.
                                        //automatically make it cancelled
                                        ResultStatus cancelRStatus = (from rs in context.ResultStatus
                                                                      where rs.ResultStatusDescription == "Cancelled"
                                                                            && rs.DataOrigin.DataOriginID == 3
                                                                      select rs).First();

                                        var query5 = (from rchg in context.ResultChanges
                                                      where rchg.ResultID == resultId
                                                            && rchg.ResultStatus.ResultStatusDescription == "Amended"
                                                            && rchg.ChangeID == resultCurrentChangeId
                                                      orderby rchg.ChangeID descending
                                                      select rchg).ToList();

                                        //if there is no Amended entry then it is already cancelled, so do nothing
                                        if (query5.Count > 0)
                                        {
                                            prevRC = query5.First();

                                            if(prevRC.Changes!=null)
                                                prevRC.ChangesReference.Load();

                                            ResultChanges newPrevRC = prevRC;

                                            newPrevRC.ResultStatus = cancelRStatus;
                                            newPrevRC.DateEnd = DateTime.Now;
                                            context.Attach(prevRC);
                                            context.ApplyPropertyChanges("ResultChanges", newPrevRC);

                                            //set RR and RRC for the Amendment to cancelled as well
                                            var rrQuery = (from rrq in context.ResultRequests
                                                           where rrq.Changes.ChangeID == prevRC.ChangeID
                                                            && rrq.Results.ResultID == prevRC.ResultID
                                                           orderby rrq.ResultRequestID descending
                                                           select rrq).ToList();

                                            if (rrQuery.Count > 0)
                                            {
                                                ScrutinyStatus ss = (from s in context.ScrutinyStatus
                                                                     where s.ScrutinyStatusDescription == "Cancelled"
                                                                     select s).First();

                                                //set the SS code in RR to cancelled
                                                ResultRequests rr = rrQuery.First();
                                                ResultRequests newrr = rr;
                                                newrr.ScrutinyStatus = ss;
                                                context.Attach(rr);
                                                context.ApplyPropertyChanges("ResultRequests", newrr);

                                                //get the most recent RRC for this RR and set its dateend to null
                                                var rrcQuery = (from rrcq in context.ResultRequestChanges
                                                                where rrcq.ResultRequestID == rr.ResultRequestID
                                                                    && rrcq.DateEnd == null
                                                                orderby rrcq.ChangeID descending
                                                                select rrcq).ToList();

                                                if (rrcQuery.Count > 0)
                                                {
                                                    ResultRequestChanges rrcOld = rrcQuery.First();
                                                    rrcOld.DateEnd = DateTime.Now;
                                                    context.ApplyPropertyChanges("ResultRequestChanges", rrcOld);
                                                }

                                                //now create a new RRC row with SScode = cancelled
                                                ResultRequestChanges rrc = new ResultRequestChanges();
                                                rrc.Changes = change;
                                                rrc.Comments = "Cancelled automatically due to withdrawal of result.";
                                                rrc.ResultRequests = rr;
                                                rrc.ScrutinyStatus = ss;

                                                context.AddToResultRequestChanges(rrc);
                                            }

                                        }
                                    }
                                }
                            }

                            if (resultChangeType.Equals("Amended"))
                            {
                                bool didUnWithdraw = false;

                                ResultStatus amendedRStatus = (from rs in context.ResultStatus
                                                               where rs.ResultStatusDescription == "Amended"
                                                                   && rs.DataOrigin.DataOriginID == 1
                                                               select rs).First();

                                //first check if the result had been withdrawn before
                                var query4 = (from rchg in context.ResultChanges
                                              where rchg.ResultID == resultId
                                                    && rchg.ResultStatus.ResultStatusDescription == "Withdrawn"
                                                    && rchg.DateEnd == null
                                              orderby rchg.ChangeID descending
                                              select rchg).ToList();

                                //if Withdrawn, then un-withdraw it first
                                if (query4.Count > 0)
                                {
                                    prevRC = query4.First();


                                    //backward compatibility: Cancel any previous withdraw rows that have datened = null
                                    if (query4.Count > 1)
                                    {
                                        var queryOtherWithdraw = (from rchg in context.ResultChanges
                                                                  where rchg.ResultID == resultId
                                                                        && rchg.ResultStatus.ResultStatusDescription == "Withdrawn"
                                                                        && rchg.ChangeID < prevRC.ChangeID
                                                                  orderby rchg.ChangeID descending
                                                                  select rchg).ToList();

                                        //set dateend to now for all, these should be set so that they dont
                                        //cause confusion in new logic
                                        for (int i = 0; i < queryOtherWithdraw.Count; i++)
                                        {
                                            ResultChanges oldWithdrawRC = queryOtherWithdraw.ElementAt(i);
                                            oldWithdrawRC.DateEnd = DateTime.Now;
                                            context.ApplyPropertyChanges("ResultChanges", oldWithdrawRC);
                                        }

                                        if (queryOtherWithdraw.Count > 0)
                                            context.SaveChanges();
                                    }


                                    //backward compatibility: 
                                    //By old logic when there is withdraw there is no corresponding Unamended entry
                                    //if so, then simply change the RS to 'Unamended' for such rows

                                    var oldRCQuery = (from rchg in context.ResultChanges
                                                      where rchg.ResultID == resultId
                                                            && rchg.ResultStatus.ResultStatusDescription == "Unamended"
                                                      orderby rchg.ChangeID descending
                                                      select rchg).ToList();

                                    //it is the behavior of old code, so handle it accordingly 
                                    if (oldRCQuery.Count <= 0)
                                    {
                                        //set RS of Withdrawn row to 'Unamended'
                                        //if withdrawn, change back the status to Unamended
                                        ResultStatus unamendedRStatus = (from rs in context.ResultStatus
                                                                         where rs.ResultStatusDescription == "Unamended"
                                                                            && rs.DataOrigin.DataOriginID == 1
                                                                         select rs).First();

                                        //change it back to Unamended and save to DB
                                        ResultChanges newPrevRC = prevRC;
                                        newPrevRC.ResultStatus = unamendedRStatus;
                                        newPrevRC.DateEnd = DateTime.Now;
                                        context.Attach(prevRC);
                                        context.ApplyPropertyChanges("ResultChanges", newPrevRC);
                                        context.SaveChanges();

                                        //we also must have created a RR and RRC for this row..cancel it
                                        //set RR and RRC for the Withdrawn to cancelled as well
                                        var rrQuery = (from rrq in context.ResultRequests
                                                       where rrq.Changes.ChangeID == prevRC.ChangeID
                                                        && rrq.Results.ResultID == prevRC.ResultID
                                                       orderby rrq.ResultRequestID descending
                                                       select rrq).ToList();

                                        if (rrQuery.Count > 0)
                                        {
                                            ScrutinyStatus ss = (from s in context.ScrutinyStatus
                                                                 where s.ScrutinyStatusDescription == "Cancelled"
                                                                 select s).First();

                                            //set the SS code in RR to cancelled
                                            ResultRequests rr = rrQuery.First();
                                            ResultRequests newrr = rr;
                                            newrr.ScrutinyStatus = ss;
                                            context.Attach(rr);
                                            context.ApplyPropertyChanges("ResultRequests", newrr);

                                            //get the most recent RRC for this RR and set its dateend to null
                                            var rrcQuery = (from rrcq in context.ResultRequestChanges
                                                            where rrcq.ResultRequestID == rr.ResultRequestID
                                                                && rrcq.DateEnd == null
                                                            orderby rrcq.ChangeID descending
                                                            select rrcq).ToList();

                                            if (rrcQuery.Count > 0)
                                            {
                                                ResultRequestChanges rrcOld = rrcQuery.First();
                                                rrcOld.DateEnd = DateTime.Now;
                                                context.ApplyPropertyChanges("ResultRequestChanges", rrcOld);
                                            }

                                            //now add a new entry
                                            change = new Changes { ChangeDate = DateTime.Now, ChangeTypeID = ct.ChangeTypeID, RoleName = uc.RoleName, UserName = uc.UserName, Forename = uc.Forename, Surname = uc.Surname };
                                            context.AddToChanges(change);

                                            //now create a new RRC row with SScode = cancelled
                                            ResultRequestChanges rrc = new ResultRequestChanges();
                                            rrc.Changes = change;
                                            rrc.Comments = "This Withdrawal request is cancelled automatically due to un-withdrawal of result.";
                                            rrc.ResultRequests = rr;
                                            rrc.ScrutinyStatus = ss;

                                            context.AddToResultRequestChanges(rrc);
                                        }

                                    }
                                    else //it is the new withdraw logic
                                    {
                                        didUnWithdraw = true;

                                        //set DateEnd for the Withdrawn RC                                           
                                        prevRC.DateEnd = DateTime.Now;

                                        //Defect #1866, 2503: Correct behaviour when un-withdrawal is performed (as per Edit Results UC: "ResultChanges table - set Withdrawal row to cancel")
                                        ResultStatus cancelRStatus = (from rs in context.ResultStatus
                                                                      where rs.ResultStatusDescription == "Cancelled"
                                                                            && rs.DataOrigin.DataOriginID == 3
                                                                      select rs).First();

                                        context.ApplyPropertyChanges("ResultChanges", prevRC);

                                        //set RR and RRC for the Withdrawn to cancelled as well
                                        var rrQuery = (from rrq in context.ResultRequests
                                                       where rrq.Changes.ChangeID == prevRC.ChangeID
                                                        && rrq.Results.ResultID == prevRC.ResultID
                                                       orderby rrq.ResultRequestID descending
                                                       select rrq).ToList();

                                        if (rrQuery.Count > 0)
                                        {
                                            ScrutinyStatus ss = (from s in context.ScrutinyStatus
                                                                 where s.ScrutinyStatusDescription == "Cancelled"
                                                                 select s).First();

                                            //set the SS code in RR to cancelled
                                            ResultRequests rr = rrQuery.First();
                                            ResultRequests newrr = rr;
                                            newrr.ScrutinyStatus = ss;
                                            context.Attach(rr);
                                            context.ApplyPropertyChanges("ResultRequests", newrr);

                                            //get the most recent RRC for this RR and set its dateend to null
                                            var rrcQuery = (from rrcq in context.ResultRequestChanges
                                                            where rrcq.ResultRequestID == rr.ResultRequestID
                                                                && rrcq.DateEnd == null
                                                            orderby rrcq.ChangeID descending
                                                            select rrcq).ToList();

                                            if (rrcQuery.Count > 0)
                                            {
                                                ResultRequestChanges rrcOld = rrcQuery.First();
                                                rrcOld.DateEnd = DateTime.Now;
                                                context.ApplyPropertyChanges("ResultRequestChanges", rrcOld);
                                            }

                                            //now add a new entry
                                            change = new Changes { ChangeDate = DateTime.Now, ChangeTypeID = ct.ChangeTypeID, RoleName = uc.RoleName, UserName = uc.UserName, Forename = uc.Forename, Surname = uc.Surname };
                                            context.AddToChanges(change);

                                            // TFS 16332
                                            ResultChanges newRC = new ResultChanges();
                                            newRC.Changes       = change;
                                            newRC.Results       = result;
                                            newRC.Points        = prevRC.Points;
                                            newRC.TierCode      = prevRC.TierCode;
                                            newRC.TestMark      = prevRC.TestMark;
                                            newRC.Finegrade     = prevRC.Finegrade;
                                            newRC.ResultStatus  = cancelRStatus;
                                            context.AddToResultChanges(newRC);
                                            
                                            //now create a new RRC row with SScode = cancelled
                                            ResultRequestChanges rrc = new ResultRequestChanges();
                                            rrc.Changes = change;
                                            rrc.Comments = "This Withdrawal request is cancelled automatically due to un-withdrawal of result.";
                                            rrc.ResultRequests = rr;
                                            rrc.ScrutinyStatus = ss;

                                            context.AddToResultRequestChanges(rrc);
                                        }
                                    }

                                }

                                var query2 = (from rchg in context.ResultChanges
                                              where rchg.ResultID == resultId
                                                    && rchg.ResultStatus.ResultStatusDescription == "Amended"
                                              orderby rchg.ChangeID descending
                                              select rchg).ToList();

                                if (query2.Count < 1)
                                {
                                    prevRC = (from rchg in context.ResultChanges
                                              where rchg.ResultID == resultId
                                                    && rchg.ResultStatus.ResultStatusDescription == "Unamended"
                                              orderby rchg.ChangeID descending
                                              select rchg).First();
                                }
                                else
                                    prevRC = query2.First();

                                //check if result exists
                                if (matches.Count() > 0)
                                {
                                    var lateMatches = matches.Where(r => r.DataOrigin.DataOriginID == 2).ToList();
                                    if (lateMatches.Count > 0)
                                    {
                                        Results matchR = lateMatches.Last(); //get most recent late result

                                        //now get the most recent RC for this matching result
                                        ResultChanges rcMatch = (from rc in context.ResultChanges
                                                                 where rc.ResultID == matchR.ResultID
                                                                 && rc.ResultStatus.DataOrigin.DataOriginID == 2
                                                                 orderby rc.ChangeID descending
                                                                 select rc).First();

                                        rcMatch.ResultStatusReference.Load();
                                        rcMatch.PointsReference.Load();
                                        prevRC.PointsReference.Load();

                                        //if you are changign the current grade which is same as late result to something else
                                        if (rcMatch.Points.GradeCode == prevRC.Points.GradeCode)
                                        {
                                            if (rcMatch.Points.GradeCode != point.GradeCode)
                                            {
                                                if (!(rcMatch.ResultStatus.ResultStatusDescription.Equals("Rejected")))
                                                {
                                                    //can be saved with warning
                                                    updateStatus = "Your request matched a late result.";
                                                    isOkToSave = true;
                                                }
                                                else
                                                {
                                                    isOkToSave = true;
                                                }
                                            }
                                            else
                                                isOkToSave = true;
                                        }

                                        //if the current result and late result grade differs
                                        if (rcMatch.Points.GradeCode != prevRC.Points.GradeCode)
                                        {
                                            if (rcMatch.Points.GradeCode != point.GradeCode)
                                            {
                                                if (!(rcMatch.ResultStatus.ResultStatusDescription.Equals("Rejected")))
                                                {
                                                    //can be saved with warning
                                                    updateStatus = "Your request matched a late result.";
                                                    isOkToSave = true;
                                                }
                                                else
                                                {
                                                    isOkToSave = true;
                                                }
                                            }
                                            else
                                            {
                                                isOkToSave = false;
                                                throw Web09Exception.GetBusinessException(Web09MessageList.MatchesLRAbove);
                                                //updateStatus = "Your request matches a late result above. Please accept that instead.";                                            
                                            }
                                        }
                                    }
                                    else
                                        isOkToSave = true;
                                }                                

                                if (isOkToSave)
                                {
                                    //check for concurrency only if we did not do a un-withdraw first
                                    //in case there was a un-withdraw, the concurrency will not match

                                    if (!didUnWithdraw && prevRC.ChangeID != resultCurrentChangeId)
                                        throw Web09Exception.GetBusinessException(Web09MessageList.TransactionDataConcurrencyError);
                                    else
                                    {
                                        // has grade changed?
                                        bool isDifferentMarks = false;
                                        bool isDifferentGrade = false;

                                        //add a new change only if the grade is different from the most recent change

                                        if (marks.HasValue)
                                        {
                                            string mark = "";
                                            mark = marks.ToString();
                                            if (prevRC.TestMark != mark)
                                                isDifferentMarks = true;
                                        }

                                        if (point != prevRC.Points)
                                        {
                                            isDifferentGrade = true;
                                        }

                                        if (isDifferentGrade || isDifferentMarks)
                                        {
                                            //cancel the pending request before adding a new one
                                            ResultStatus cancelRStatus = (from rs in context.ResultStatus
                                                                          where rs.ResultStatusDescription == "Cancelled"
                                                                                && rs.DataOrigin.DataOriginID == 3
                                                                          select rs).First();

                                            //get most recent 'amended' RC
                                            //this is needed here since the prevRC logic above also picks up
                                            //unamended RC which we do not want here
                                            var query3 = (from rchg in context.ResultChanges
                                                          .Include("ResultStatus")
                                                          where rchg.ResultID == resultId
                                                          && rchg.DateEnd == null
                                                          orderby rchg.ChangeID descending
                                                          select rchg);
                                            
                                            //if there are no more amended entries to cancel, do nothing
                                            if (query3.Any(x=>x.ResultStatus.ResultStatusDescription == "Amended"))
                                            {
                                                ResultChanges prevRC1 = query3.First();
                                                context.Attach(prevRC1);
                                                //ResultChanges newPrevRC = prevRC1;
                                                prevRC1.DateEnd = DateTime.Now;
                                                prevRC1.ResultStatus = cancelRStatus;

                                                context.ApplyPropertyChanges("ResultChanges", prevRC1);

                                                //set RR and RRC for the Amendment to cancelled as well
                                                var rrQuery = (from rrq in context.ResultRequests
                                                               where rrq.Changes.ChangeID == prevRC1.ChangeID
                                                                && rrq.Results.ResultID == prevRC1.ResultID
                                                               orderby rrq.ResultRequestID descending
                                                               select rrq).ToList();

                                                if (rrQuery.Count > 0)
                                                {
                                                    ScrutinyStatus ss = (from s in context.ScrutinyStatus
                                                                         where s.ScrutinyStatusDescription == "Cancelled"
                                                                         select s).First();

                                                    //set the SS code in RR to cancelled
                                                    ResultRequests rr = rrQuery.First();
                                                    ResultRequests newrr = rr;
                                                    newrr.ScrutinyStatus = ss;
                                                    context.Attach(rr);
                                                    context.ApplyPropertyChanges("ResultRequests", newrr);

                                                    //get the most recent RRC for this RR and set its dateend to now
                                                    var rrcQuery = (from rrcq in context.ResultRequestChanges
                                                                    where rrcq.ResultRequestID == rr.ResultRequestID
                                                                        && rrcq.DateEnd == null
                                                                    orderby rrcq.ChangeID descending
                                                                    select rrcq).ToList();

                                                    if (rrcQuery.Count > 0)
                                                    {
                                                        ResultRequestChanges rrcOld = rrcQuery.First();
                                                        rrcOld.DateEnd = DateTime.Now;
                                                        context.ApplyPropertyChanges("ResultRequestChanges", rrcOld);
                                                    }

                                                    //now add a new entry
                                                    change = new Changes { ChangeDate = DateTime.Now, ChangeTypeID = ct.ChangeTypeID, RoleName = uc.RoleName, UserName = uc.UserName, Forename = uc.Forename, Surname = uc.Surname };
                                                    context.AddToChanges(change);

                                                    //now create a new RRC row with SScode = cancelled
                                                    ResultRequestChanges rrc = new ResultRequestChanges();
                                                    rrc.Changes = change;
                                                    rrc.Comments = "Cancelled automatically due to a more recent amendment taking place.";
                                                    rrc.ResultRequests = rr;
                                                    rrc.ScrutinyStatus = ss;

                                                    context.AddToResultRequestChanges(rrc);
                                                }
                                            } else if(query3.Count() > 0) { //set date end on previous change
                                                ResultChanges prevRC1 = query3.First();
                                                context.Attach(prevRC1);
                                                //ResultChanges newPrevRC = prevRC1;
                                                prevRC1.DateEnd = DateTime.Now;
                                                context.ApplyPropertyChanges("ResultChanges", prevRC);
                                            }

                                            //now add a new entry
                                            change = new Changes { ChangeDate = DateTime.Now, ChangeTypeID = ct.ChangeTypeID, RoleName = uc.RoleName, UserName = uc.UserName, Forename = uc.Forename, Surname = uc.Surname };
                                            context.AddToChanges(change);

                                            ResultChanges rc = new ResultChanges
                                            {
                                                Changes = change,
                                                Points = point,
                                                Results = prevRC.Results,
                                                ResultStatus = amendedRStatus

                                            };

                                            if (isDifferentMarks)
                                                rc.TestMark = marks.ToString();

                                            context.AddToResultChanges(rc);

                                            point.QualificationTypesReference.Load();

                                            //if KS2 and new grade is F or L set SS code to Query
                                            if (AcceptOnEntry==false &&
                                                (point.GradeCode.Equals("F") || point.GradeCode.Equals("L"))
                                                && point.QualificationTypes.SubLevels.Any(sl => sl.SubLevelDescription.Contains("KS2")))
                                            {
                                                CreateQueryResultRequest(context, rc);
                                            }
                                            else
                                                CreateResultRequest(context, rc, EvidenceRequired, AcceptOnEntry);

                                        }
                                    }
                                }
                            }

                            if (resultChangeType.Equals("Added"))
                            {
                                ResultStatus addedRStatus = (from rs in context.ResultStatus
                                                             where rs.ResultStatusDescription == "Added"
                                                                    && rs.DataOrigin.DataOriginID == 3
                                                             select rs).First();

                                //get the most recent entry with status='Added'
                                prevRC = (from rchg in context.ResultChanges
                                          where rchg.ResultID == resultId
                                                && rchg.ResultStatus.ResultStatusDescription == "Added"
                                          orderby rchg.ChangeID descending
                                          select rchg).First();

                                //check if result exists
                                if (matches.Count() > 0)
                                {
                                    var lateMatches = matches.Where(r => r.DataOrigin.DataOriginID == 2).ToList();
                                    if (lateMatches.Count > 0)
                                    {
                                        Results matchR = lateMatches.Last(); //get most recent late result

                                        //now get the most recent RC for this matching result
                                        ResultChanges rcMatch = (from rc in context.ResultChanges
                                                                 where rc.ResultID == matchR.ResultID
                                                                 && rc.ResultStatus.DataOrigin.DataOriginID == 2
                                                                 orderby rc.ChangeID descending
                                                                 select rc).First();

                                        rcMatch.ResultStatusReference.Load();
                                        rcMatch.PointsReference.Load();

                                        //if you are changign the current grade which is same as late result to something else
                                        if (rcMatch.Points.GradeCode == prevRC.Points.GradeCode)
                                        {
                                            if (rcMatch.Points.GradeCode != point.GradeCode)
                                            {
                                                if (!(rcMatch.ResultStatus.ResultStatusDescription.Equals("Rejected")))
                                                {
                                                    //can be saved with warning
                                                    updateStatus = "Your request matched a late result.";
                                                    isOkToSave = true;
                                                }
                                                else
                                                {
                                                    isOkToSave = true;
                                                }
                                            }
                                        }

                                        //if the current result and late result grade differs
                                        if (rcMatch.Points.GradeCode != prevRC.Points.GradeCode)
                                        {
                                            if (rcMatch.Points.GradeCode != point.GradeCode)
                                            {
                                                if (!(rcMatch.ResultStatus.ResultStatusDescription.Equals("Rejected")))
                                                {
                                                    //can be saved with warning
                                                    updateStatus = "Your request matched a late result.";
                                                    isOkToSave = true;
                                                }
                                                else
                                                {
                                                    isOkToSave = true;
                                                }
                                            }
                                            else
                                            {
                                                isOkToSave = false;
                                                throw Web09Exception.GetBusinessException(Web09MessageList.MatchesLRAbove);
                                                //updateStatus = "Your request matches a late result above. Please accept that instead.";                                            
                                            }
                                        }
                                    }
                                    else
                                        isOkToSave = true;
                                }

                                if (isOkToSave)
                                {
                                    if (prevRC.ChangeID != resultCurrentChangeId)
                                        throw Web09Exception.GetBusinessException(Web09MessageList.TransactionDataConcurrencyError);
                                    else
                                    {
                                        // has grade changed?
                                        bool isDifferentMarks = false;
                                        bool isDifferentGrade = false;

                                        //add a new change only if the grade is different from the most recent change

                                        if (marks.HasValue)
                                        {
                                            string mark = "";
                                            mark = marks.ToString();
                                            if (prevRC.TestMark != mark)
                                                isDifferentMarks = true;
                                        }

                                        if (point != prevRC.Points)
                                        {
                                            isDifferentGrade = true;
                                        }

                                        if (isDifferentGrade || isDifferentMarks)
                                        {
                                            //cancel the previous RC since pending requests are automatically Cancelled
                                            ResultStatus cancelRStatus = (from rs in context.ResultStatus
                                                                          where rs.ResultStatusDescription == "Cancelled"
                                                                                && rs.DataOrigin.DataOriginID == 3
                                                                          select rs).First();

                                            ResultChanges newPrevRC = prevRC;
                                            newPrevRC.ResultStatus = cancelRStatus;
                                            newPrevRC.DateEnd = DateTime.Now;                                           
                                            //context.Attach(prevRC); commented out to fix defect 3464
                                            context.ApplyPropertyChanges("ResultChanges", newPrevRC);

                                            //set RR and RRC for the Amendment to cancelled as well
                                            var rrQuery = (from rrq in context.ResultRequests
                                                           where rrq.Changes.ChangeID == prevRC.ChangeID
                                                            && rrq.Results.ResultID == prevRC.ResultID
                                                           orderby rrq.ResultRequestID descending
                                                           select rrq).ToList();

                                            if (rrQuery.Count > 0)
                                            {
                                                ScrutinyStatus ss = (from s in context.ScrutinyStatus
                                                                     where s.ScrutinyStatusDescription == "Cancelled"
                                                                     select s).First();

                                                //set the SS code in RR to cancelled
                                                ResultRequests rr = rrQuery.First();
                                                ResultRequests newrr = rr;
                                                newrr.ScrutinyStatus = ss;
                                                context.Attach(rr);
                                                context.ApplyPropertyChanges("ResultRequests", newrr);

                                                //get the most recent RRC for this RR and set its dateend to now
                                                var rrcQuery = (from rrcq in context.ResultRequestChanges
                                                                where rrcq.ResultRequestID == rr.ResultRequestID
                                                                    && rrcq.DateEnd == null
                                                                orderby rrcq.ChangeID descending
                                                                select rrcq).ToList();

                                                if (rrcQuery.Count > 0)
                                                {
                                                    ResultRequestChanges rrcOld = rrcQuery.First();
                                                    rrcOld.DateEnd = DateTime.Now;
                                                    context.ApplyPropertyChanges("ResultRequestChanges", rrcOld);
                                                }

                                                //now add a new entry
                                                change = new Changes { ChangeDate = DateTime.Now, ChangeTypeID = ct.ChangeTypeID, RoleName = uc.RoleName, UserName = uc.UserName, Forename = uc.Forename, Surname = uc.Surname };
                                                context.AddToChanges(change);

                                                //now create a new RRC row with SScode = cancelled
                                                ResultRequestChanges rrc = new ResultRequestChanges();
                                                rrc.Changes = change;
                                                rrc.Comments = "Cancelled automatically due to a more recent amendment taking place.";
                                                rrc.ResultRequests = rr;
                                                rrc.ScrutinyStatus = ss;

                                                context.AddToResultRequestChanges(rrc);
                                            }

                                            //now add the new entry
                                            change = new Changes { ChangeDate = DateTime.Now, ChangeTypeID = ct.ChangeTypeID, RoleName = uc.RoleName, UserName = uc.UserName, Forename = uc.Forename, Surname = uc.Surname };
                                            context.AddToChanges(change);
                                            ResultChanges rc = new ResultChanges
                                            {
                                                Changes = change,
                                                Points = point,
                                                Results = prevRC.Results,
                                                ResultStatus = addedRStatus
                                            };

                                            if (isDifferentMarks)
                                                rc.TestMark = marks.ToString();

                                            context.AddToResultChanges(rc);

                                            point.QualificationTypesReference.Load();

                                            //if KS2 and new grade is F or L set SS code to Query
                                            if (AcceptOnEntry == false &&
                                                (point.GradeCode.Equals("F") || point.GradeCode.Equals("L"))
                                                && point.QualificationTypes.SubLevels.Any(sl => sl.SubLevelDescription.Contains("KS2")))
                                            {
                                                CreateQueryResultRequest(context, rc);
                                            }
                                            else
                                                CreateResultRequest(context, rc, EvidenceRequired, AcceptOnEntry);

                                        }
                                    }
                                }
                            }

                            context.SaveChanges();

                            //get the new change Id
                            if (change != null)
                            {
                                newChangeId = change.ChangeID;
                            }
                        }
                    }

                    transaction.Complete();

                    //this is when the msg is only a warning and changes are saved anyway
                    if (!updateStatus.Equals("success"))
                    {
                        //If it is Warning1/2/3 then it means we also need to return the newChangeId
                        //cannot do both so we must tag the new change id at the end of the exception message
                        //and untag it before the exception gets displayed
                        if (updateStatus.Contains("Please note the grade conflict between a late result and a request")
                         || updateStatus.Contains("Please note that this conflicts with a requested result below")
                         || updateStatus.Contains("Please note that the matching late result above has not been rejected"))
                        {
                            updateStatus += ":" + newChangeId.ToString();
                        }

                        throw new BusinessLevelException(updateStatus);
                    }
                    return newChangeId;
                }

            

            }
        
        
        
    }
}
