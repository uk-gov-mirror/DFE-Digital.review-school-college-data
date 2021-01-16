using System;
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
        public static int AddResults(int StudentID, int QualificationID, int PointID, short KeyStageID, int Year, char SeasonCode, bool EvidenceRequired, bool AcceptOnEntry, int? Marks, UserContext uc, string GradeText)
        {
            int newchangeId = -1;
            bool isLateRButSave = false;

            using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope())
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        if (!Validation.Common.IsExamYearValid(context, Year))
                        {
                            throw Web09Exception.GetBusinessException(Web09MessageList.ExamYearInvalid);
                        }

                        if (!Validation.Common.IsExamSeasonValid(context, SeasonCode.ToString()))
                        {
                            throw Web09Exception.GetBusinessException(Web09MessageList.ExamSeasonInvalid);
                        }

                        if (!Validation.Common.IsKeyStageValid(context, KeyStageID))
                        {
                            throw Web09Exception.GetBusinessException(Web09MessageList.KeystageInvalid);
                        }

                        ChangeType ct = context.ChangeType.First();

                        Changes change = new Changes { ChangeDate = DateTime.Now, ChangeTypeID = ct.ChangeTypeID, Forename=uc.Forename, Surname=uc.Surname, UserName=uc.UserName, RoleName=uc.RoleName};                        

                        //StudentID, ExamNumber, ExamYear, season code, subjectID, qualificationtypeID, ,r-incl, , sublevelid, Dataoriginid                        
                        Points point = null;

                        if (string.IsNullOrEmpty(GradeText))
                        {
                            point = (from p in context.Points
                                            where p.PointID == PointID
                                            select p).First();
                        }

                        Students student = (from s in context.Students
                                            where s.StudentID == StudentID
                                            select s).First();


                        short examNumber = 0;
                        
                        var qry = (from r in context.Results
                                 where r.Students.StudentID == StudentID
                                 select r.ExamNumber).ToList();

                        if (qry.Count > 0)
                            examNumber = qry.Max();

                        DataOrigin dataOrigin = (from d in context.DataOrigin
                                              where d.DataOriginDescription == "User Addition"
                                              select d).First();

                        string scode = SeasonCode.ToString();

                        Seasons season = (from s in context.Seasons
                                          where s.SeasonCode == scode
                                          select s).First();                        

                        Results result = new Results();
                        result.Students = student;
                        result.ExamNumber = ++examNumber;
                        result.ExamYear = Year;                        
                        result.DataOrigin = dataOrigin;
                        result.Seasons = season;                          

                        if (KeyStageID < 4)
                        {
                            Subjects subject = (from s in context.Subjects
                                                where s.SubjectID == QualificationID
                                                select s).First();

                            string keystageString = "KS" + KeyStageID.ToString();

                            IQueryable<SubLevels> q = context.SubLevels
                                                    .Include("QualificationTypes")
                                                    .Where(s => s.SubLevelDescription.Contains(keystageString));
                            SubLevels sublevel = q.First();

                            result.Subjects = subject;
                            result.QualificationTypes = sublevel.QualificationTypes;
                            result.SubLevels = sublevel;                                                  
                        }
                        else
                        {
                            IQueryable<QANSubjects> query  = context.QANSubjects
                                                                    .Include("AwardingBodies")
                                                                    .Include("Subjects")
                                                                    .Include("QANs")
                                                                    .Where(r =>
                                                                        r.QANSubjectID == QualificationID);

                            QANSubjects qualification = query.First();

                            QualificationTypes qtypes = (from qt in context.QualificationTypes
                                                         where qt.QualificationTypeCode == qualification.QualificationTypeCode
                                                         select qt).First();

                            SubLevels sublevel = (from s in context.SubLevels
                                                  where s.QualificationTypes.QualificationTypeID == qtypes.QualificationTypeID
                                                        && s.SubLevelDescription == qualification.LevelDescription
                                                  select s).First();
                           
                            result.QualificationTypes = qtypes;
                            result.SubLevels = sublevel;
                            result.AwardingBodies = qualification.AwardingBodies;
                            result.BoardSubjectNumber = qualification.BoardSubjectNumber;
                            result.Subjects = qualification.Subjects;
                            result.QANs = qualification.QANs;

                            if (!string.IsNullOrEmpty(GradeText))
                            {
                                //this is in the unrecognized qualification case
                                
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
                                { //create a point entry
                                    point = new Points();
                                    point.GradeCode = GradeText;
                                    point.GradeDescription = GradeText;
                                    point.QANs = qualification.QANs;
                                    point.QualificationTypes = qtypes;

                                    context.AddToPoints(point);
                                }
                            }
                        }

                        //check if the marks entered are withing the max and min allowed
                        if (KeyStageID == 2 && Marks != null && Marks > -1)
                        {
                           var  ranges = (from tmr in context.TestMarkRanges
                                                  where tmr.SubjectID == result.Subjects.SubjectID
                                                  && tmr.ExamYear == Year
                                                  && tmr.GradeCode == point.GradeCode                                                 
                                                  && tmr.MinMark <= Marks
                                                  && tmr.MaxMark >= Marks
                                                  select tmr).ToList();

                           if (ranges != null && ranges.Count < 1)
                               throw Web09Exception.GetBusinessException(Web09MessageList.MarksNotInRange);
                               //return "Marks are not within correct range.";
 
                        }

                        //check if a non-cancelled result already exists for the same student, subject, exam and year
                        var res = (from r in context.Results
                                   join c in context.ResultChanges on r.ResultID equals c.ResultID
                                   join s in context.ResultStatus on c.ResultStatus.ResultStatusID equals s.ResultStatusID
                                                   where r.Students.StudentID == StudentID
                                                   && r.Subjects.SubjectID == result.Subjects.SubjectID 
                                                   && r.QualificationTypes.QualificationTypeID == result.QualificationTypes.QualificationTypeID
                                                   && r.ExamYear == result.ExamYear                                                   
                                                   && r.Seasons.SeasonCode == scode
                                                   && r.ExamYear == Year
                                                   && s.ResultStatusDescription != "Cancelled"
                                                   //&& r.QANs.QANID == result.QANs.QANID
                                                   orderby r.DataOrigin.DataOriginID descending
                                                   select new {r, r.AwardingBodies, r.QANs}).ToList();                        

                        if (res != null && res.Count > 0)
                        {
                            if (KeyStageID >= 4 && result.AwardingBodies != null)
                            {
                                res = res.Where(r1 => r1.AwardingBodies.AwardingBodyID == result.AwardingBodies.AwardingBodyID).ToList();
                            }

                            if (KeyStageID >= 4 && result.QANs != null)
                            {
                                res = res.Where(r1 => r1.r.QANs!=null && r1.r.QANs.QANID == result.QANs.QANID).ToList();
                            }

                            if (res != null && res.Count > 0)
                            {
                                Results r = res.First().r;

                                if (!r.DataOriginReference.IsLoaded)
                                    r.DataOriginReference.Load();

                                if (r.DataOrigin.DataOriginDescription.Equals("Late Load")) 
                                {
                                    ResultChanges rc1 = (from c in context.ResultChanges
                                                         where c.ResultID == r.ResultID
                                                         orderby c.ChangeID descending
                                                         select c).First();

                                    if (!rc1.ResultStatusReference.IsLoaded)
                                        rc1.ResultStatusReference.Load();

                                    //throw exception only if late result is in accepted state not otherwise
                                    if (rc1.ResultStatus.ResultStatusDescription.Equals("Accepted") || rc1.ResultStatus.ResultStatusDescription.Equals("Undecided"))
                                    {
                                        isLateRButSave = true;
                                    }

                                    // Defect #2518: if the late result is in undecided state, we need to find an "initial load" result, and check it (as below)
                                    if (rc1.ResultStatus.ResultStatusDescription.Equals("Undecided"))
                                    {
                                        if (res != null)
                                        {
                                            int index = 1;

                                            while ((index < res.Count) && !(r.DataOrigin.DataOriginDescription.Equals("Initial Load")))
                                            {
                                                r = res.ElementAt(index).r;
                                                if (!r.DataOriginReference.IsLoaded)
                                                    r.DataOriginReference.Load();

                                                index++;
                                            }
                                        }
                                    }
                                }

                                //check if the last status for this result was a 'Withdraw', if so, display special message
                                //this will only be when the result has a dataorigin = 'Initial Load'
                                bool isInitialLoad  = r.DataOrigin.DataOriginDescription.Equals("Initial Load");
                                bool isUserAddition = r.DataOrigin.DataOriginDescription.Equals("User Addition");
                                if (isInitialLoad || isUserAddition )
                                {
                                    ResultChanges rc1 = (from c in context.ResultChanges
                                                         where c.ResultID == r.ResultID
                                                         orderby c.ChangeID descending
                                                         select c).First();

                                    if (!rc1.ResultStatusReference.IsLoaded)
                                        rc1.ResultStatusReference.Load();

                                    if (!rc1.PointsReference.IsLoaded)
                                        rc1.PointsReference.Load();

                                    if (rc1.ResultStatus.ResultStatusDescription.Equals("Withdrawn"))
                                    {
                                        throw Web09Exception.GetBusinessException(Web09MessageList.AddingWithdrawnResult);
                                    }

                                    // TFS 16659
                                    if (rc1.Points != null && rc1.Points.PointID == PointID)
                                    {
                                        throw Web09Exception.GetBusinessException(Web09MessageList.DuplicateResultExists);
                                    }

                                    // TFS 16659, 17 August 2012
                                    if (rc1.Points != null && rc1.Points.PointID != PointID)
                                    {
                                        throw Web09Exception.GetBusinessException(Web09MessageList.AddedResultExists); 
                                    }

                                    if (rc1.ResultStatus.ResultStatusDescription.Equals("Unamended"))
                                    {
                                        throw Web09Exception.GetBusinessException(Web09MessageList.UnamendedResultExists);
                                    }

                                    if (rc1.ResultStatus.ResultStatusDescription.Equals("Amended"))
                                    {
                                        throw Web09Exception.GetBusinessException(Web09MessageList.AmendedResultExists);
                                    }
                                }
                            }
                        }

                        context.AddToChanges(change);
                        context.AddToResults(result);

                        ResultStatus rstatus = (from rs in context.ResultStatus
                                                where rs.ResultStatusDescription.Equals("Added")
                                                select rs).First();

                                            
                        ResultChanges rc = new ResultChanges();
                        rc.ChangeID = change.ChangeID;
                        rc.Changes = change;                        
                        if (point != null)
                            rc.Points = point;                        

                        if(Marks.HasValue)
                            rc.TestMark = Marks.Value.ToString();                        
                        rc.ResultID = result.ResultID;
                        rc.Results = result;
                        rc.ResultStatus = rstatus;

                        context.AddToResultChanges(rc);

                        
                        ResultEvidence re = new ResultEvidence();
                     

                        ScrutinyStatus ss = new ScrutinyStatus();
                        //get 'Accept' scrutinycode entry
                        if ((AcceptOnEntry && EvidenceRequired) || !EvidenceRequired)
                        {
                            ss = (from s in context.ScrutinyStatus
                                  where s.ScrutinyStatusDescription == "Accept"
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
                            Evidence newE         = new Evidence();
                            newE.Barcode          = "";
                            newE.DateCreated      = DateTime.Now;
                            newE.DocumentLocation = Web09.Services.Common.Web09Constants.SCRUTINY_NOEVIDENCE_FILENAME;
                            newE.DocumentType     = "text/plain"; ;
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

                        //add a entry to ResultRequestChanges
                        ResultRequestChanges rrc = new ResultRequestChanges();
                        rrc.ResultRequests = rr;
                        rrc.Changes = rc.Changes;
                        rrc.ScrutinyStatus = ss;
                        context.AddToResultRequestChanges(rrc);
                                                
                        context.SaveChanges();

                        newchangeId = change.ChangeID; //this is returned
                    }
                }

                transaction.Complete();

                if (isLateRButSave) //result has been added but we also show the warning.
                {
                    //bad programming but doing it due to time constraint ;)
                    //tag the new changeId to the msg and the receiver can untag it and use it.
                    //2283 defect requires this
                    throw new BusinessLevelException("There is a late result for this qualification. However, your request has been saved.:" + newchangeId.ToString());
                }

                return newchangeId;
            }
        }
    }
}
