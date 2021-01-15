using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
             
        [Obsolete("This generates VERY inefficient SQL")]
        public static IList<ResultWithScrutinyStatus> GetLateResults(int page, int rowsPerPage, int dcsfNumber, short? keyStageID, string sortExpression)
        {
            IList<ResultWithScrutinyStatus> resultWithScrStatusList = new List<ResultWithScrutinyStatus>();

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    bool isJuneChecking = TSResult.IsJuneChecking(context);                   

                    // Query object to eager-load Results dependencies               
                    IQueryable<Results> query = context.Results
                            .Where(r =>
                                r.DataOrigin.DataOriginID == 2 &&
                                r.Students.Schools.DFESNumber == dcsfNumber);

                    if (keyStageID.HasValue)
                    {
                        query = query.Where(r => r.Students.Cohorts.KeyStage == keyStageID);
                    }

                    //if it is a June KS4 checking exercise only return graded results
                    if (isJuneChecking)
                    {
                        //get graded exams if ks4 else all if not ks4
                        query = query.Where(r => (r.Students.Cohorts.KeyStage == 4 && r.SubLevels.SubLevelCode.CompareTo("831") == -1
                                                    && r.SubLevels.SubLevelCode.CompareTo("800") == 1)
                                                     || r.Students.Cohorts.KeyStage != 4);
                    }

                    sortExpression = sortExpression == null ? string.Empty : sortExpression.ToLower();

                    query = query.OrderByDescending(p => p.ResultID); // default sort order
                    // The sortExpression linq came from looking at the TranslateBetweenDataContractResultAndBusinessEntityResultWithScrutinyStatus class
                    string sortColumns = sortExpression;
                    string[] columnList = sortColumns.Split(',');

                    // Defect #2404: allow sorting by multiple columns
                    foreach (string sortColumn in columnList)
                    {
                        bool orderByAscending = (sortExpression.EndsWith(" asc"));


                        if (sortColumn.Contains("surname "))
                        {
                            if (orderByAscending)
                                query = query.OrderBy(p => p.Students.StudentChanges.OrderByDescending(q => q.ChangeID).FirstOrDefault().Surname);
                            else
                                query = query.OrderByDescending(p => p.Students.StudentChanges.OrderByDescending(q => q.ChangeID).FirstOrDefault().Surname);
                        }


                        if (sortColumn.Contains("dob "))
                        {
                            if (orderByAscending)
                                query = query.OrderBy(p => p.Students.StudentChanges.OrderByDescending(q => q.ChangeID).FirstOrDefault().DOB);
                            else
                                query = query.OrderByDescending(p => p.Students.StudentChanges.OrderByDescending(q => q.ChangeID).FirstOrDefault().DOB);
                        }

                        if (sortColumn.Contains("awardingbody "))
                        {
                            if (orderByAscending)
                                query = query.OrderBy(p => p.AwardingBodies.AwardingBodyName);
                            else
                                query = query.OrderByDescending(p => p.AwardingBodies.AwardingBodyName);
                        }

                        if (sortColumn.Contains("lastupdated "))
                        {
                            if (orderByAscending)
                                query = query.OrderBy(p => p.ResultChanges.OrderByDescending(q => q.ChangeID).FirstOrDefault().Changes.ChangeDate);
                            else
                                query = query.OrderByDescending(p => p.ResultChanges.OrderByDescending(q => q.ChangeID).FirstOrDefault().Changes.ChangeDate);
                        }


                        if (sortColumn.Contains("dateloaded "))
                        {
                            if (orderByAscending)
                                query = query.OrderBy(p => p.ResultChanges.FirstOrDefault().Changes.ChangeDate);
                            else
                                query = query.OrderByDescending(p => p.ResultChanges.FirstOrDefault().Changes.ChangeDate);
                        }

                        if (sortColumn.Contains("qualification "))
                        {
                            if (orderByAscending)
                                query = query.OrderBy(p => p.QualificationTypes.QualificationTypeTitle);
                            else
                                query = query.OrderByDescending(p => p.QualificationTypes.QualificationTypeTitle);
                        }

                        if (sortColumn.Contains("gradeprev "))
                        {
                            if (orderByAscending)
                                query = query.OrderBy(p => p.ResultChanges.FirstOrDefault().Points.GradeCode);
                            else
                                query = query.OrderByDescending(p => p.ResultChanges.FirstOrDefault().Points.GradeCode);
                        }

                        if (sortColumn.Contains("finegrade "))
                        {
                            if (orderByAscending)
                                query = query.OrderBy(p => p.ResultChanges.FirstOrDefault().Finegrade);
                            else
                                query = query.OrderByDescending(p => p.ResultChanges.FirstOrDefault().Finegrade);
                        }

                        if (sortColumn.Contains("markprev "))
                        {
                            if (orderByAscending)
                                query = query.OrderBy(p => p.ResultChanges.FirstOrDefault().TestMark);
                            else
                                query = query.OrderByDescending(p => p.ResultChanges.FirstOrDefault().TestMark);
                        }

                        if (sortColumn.Contains("marknew "))
                        {
                            if (orderByAscending)
                                query = query.OrderBy(p => p.ResultChanges.OrderByDescending(q => q.ChangeID).FirstOrDefault().TestMark);
                            else
                                query = query.OrderByDescending(p => p.ResultChanges.OrderByDescending(q => q.ChangeID).FirstOrDefault().TestMark);
                        }

                        if (sortColumn.Contains("gradenew "))
                        {
                            if (orderByAscending)
                                query = query.OrderBy(p => p.ResultChanges.OrderByDescending(q => q.ChangeID).FirstOrDefault().Points.GradeCode);
                            else
                                query = query.OrderByDescending(p => p.ResultChanges.OrderByDescending(q => q.ChangeID).FirstOrDefault().Points.GradeCode);
                        }

                        if (sortColumn.Contains("qan "))
                        {
                            if (orderByAscending)
                                query = query.OrderBy(p => p.QANs.QUID);
                            else
                                query = query.OrderByDescending(p => p.QANs.QUID);
                        }

                        if (sortColumn.Contains("title "))
                        {
                            if (orderByAscending)
                                query = query.OrderBy(p => p.Subjects.SubjectDescription);
                            else
                                query = query.OrderByDescending(p => p.Subjects.SubjectDescription);
                        }

                        if (sortColumn.Contains("forename "))
                        {
                            if (orderByAscending)
                                query = query.OrderBy(p => p.Students.StudentChanges.OrderByDescending(q => q.ChangeID).FirstOrDefault().Forename);
                            else
                                query = query.OrderByDescending(p => p.Students.StudentChanges.OrderByDescending(q => q.ChangeID).FirstOrDefault().Forename);
                        }

                        if (sortColumn.Contains("status "))
                        {
                            if (orderByAscending)
                                query = query.OrderBy(p => p.RINCLs.DisplayFlag);
                            else
                                query = query.OrderByDescending(p => p.RINCLs.DisplayFlag);
                        }

                        if (sortColumn.Contains("session "))
                        {
                            if (orderByAscending)
                                query = query.OrderBy(p => p.Seasons.SeasonCode + p.ExamYear.ToString());
                            else
                                query = query.OrderByDescending(p => p.Seasons.SeasonCode + p.ExamYear.ToString());
                        }

                        if (sortColumn.Contains("forvusid "))
                        {
                            if (orderByAscending)
                                query = query.OrderBy(p => p.Students.ForvusIndex);
                            else
                                query = query.OrderByDescending(p => p.Students.ForvusIndex);
                        }

                    }
                    //Reduce the query result set down to the requested page only.
                    var lateResultsList = (query
                                               .Select(r => r) as ObjectQuery<Results>)
                        .Include("AwardingBodies")
                        .Include("DataOrigin")
                        .Include("QANs")
                        .Include("QualificationTypes")
                        .Include("QualificationTypes.QualificationTypeCollections")
                        .Include("QualificationTypes.SubLevels")
                        .Include("ResultChanges")
                        .Include("ResultChanges.Changes")
                        .Include("ResultChanges.Points")
                        .Include("ResultChanges.ResultStatus")
                        .Include("RINCLs")
                        .Include("Seasons")
                        .Include("Students")
                        .Include("Subjects")
                        .Include("Subjects.SubLevels")
                        .Include("Students.StudentChanges");
                    
                    var finalList = lateResultsList
                        .ToList()
                        .Skip((page - 1) * rowsPerPage)
                         .Take(rowsPerPage);

                    foreach(Results lateResult in finalList)
                        {

                            ResultWithScrutinyStatus rs = new ResultWithScrutinyStatus();

                            Results originalResult = context.Results
                                .Include("ResultChanges")
                                .Include("ResultChanges.Points")
                                .Include("ResultChanges.ResultStatus")
                                .Include("DataOrigin")
                                .Where(r => r.MATCHREG == lateResult.MATCHREG && r.DataOrigin.DataOriginID != 2 && r.Subjects.SubjectID==lateResult.Subjects.SubjectID)
                                .FirstOrDefault();

                            if (originalResult != null)
                            {
                                ResultChanges originalRC = null;
                                
                                //If the data origin is an initial load, get the first withdrawn or unamended result.
                                if (originalResult.DataOrigin.DataOriginID == 1)
                                {
                                    originalRC = originalResult.ResultChanges
                                        .Where(rc => rc.ResultStatus.ResultStatusDescription.Equals("Withdrawn") || 
                                            rc.ResultStatus.ResultStatusDescription.Equals("Unamended"))
                                        .OrderBy(rc => rc.ChangeID)
                                        .First();
                                }

                                //If the data origin is a late load, get the most recent, non-cancelled
                                //result change object.
                                else if (originalResult.DataOrigin.DataOriginID == 3)
                                {
                                    originalRC = originalResult.ResultChanges
                                        .Where(rc => !rc.ResultStatus.Equals("Cancelled"))
                                        .OrderByDescending(rc => rc.ChangeID)
                                        .First();
                                }

                                if (originalRC != null)
                                {
                                    rs.FirstChange = originalRC;
                                }
                            }

                            rs.Results = lateResult;
                            rs.LastChange = lateResult.ResultChanges.OrderByDescending(rc => rc.ChangeID).FirstOrDefault();
                            
                            

                            ResultChanges firstChange = lateResult.ResultChanges
                                   .OrderBy(rc => rc.ChangeID)
                                   .FirstOrDefault();

                            if (firstChange != null && firstChange.Changes != null)
                                rs.DateLoaded = firstChange.Changes.ChangeDate;

                            SubLevels sl = rs.Results.QualificationTypes.SubLevels.First();
                            if (!(
                               (sl.SubLevelDescription.Contains("KS2") || sl.SubLevelDescription.Contains("KS3"))
                                && (sl.SubLevelCode.Contains("550") || sl.SubLevelCode.Contains("650"))
                                ) && rs.LastChange != null)
                            {
                                resultWithScrStatusList.Add(rs);
                            }

                        }

                    return resultWithScrStatusList;
                }
            }
        }
    }
}
