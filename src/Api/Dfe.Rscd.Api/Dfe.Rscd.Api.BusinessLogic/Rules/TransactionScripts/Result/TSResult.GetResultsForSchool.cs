using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
        public static List<ResultWithScrutinyStatus> GetResultsForSchoolPage(int page, int rowsPerPage, int dcsfNumber, string sortExpression)
        {
			List<ResultWithScrutinyStatus> result = new List<ResultWithScrutinyStatus>();
			
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    bool isJuneChecking = TSResult.IsJuneChecking(context);                 

                    var query = context.Results
                                .Where
                                (
                                    r =>
                                        r.Students.Schools.DFESNumber == dcsfNumber
                                )
                                .Select
                                (
                                    r =>
                                        new
                                        {
                                            Result = r,

                                            Seasons = r.Seasons,
                                            RINCLs = r.RINCLs,
                                            Subjects = r.Subjects,
                                            QualificationTypes = r.QualificationTypes,
                                            QualificationTypeCollections = r.QualificationTypes.QualificationTypeCollections,
                                            QANs = r.QANs,
                                            SubLevels = r.QualificationTypes.SubLevels.FirstOrDefault(),
                                            NationalCentreName=context.NationalCentreNumbers.Where(ncn=>ncn.NationalCentreNumber== r.NationalCentreNumber).FirstOrDefault().NationalCentreName,

                                            Student = r.Students,

                                            StudentChanges =
                                                r.Students.StudentChanges.OrderByDescending
                                                (
                                                    sc =>
                                                        sc.ChangeID
                                                )
                                                .Take(1),
                                            AwardingBody = r.AwardingBodies,
                                            LatestChange =
                                                //((System.Data.Objects.ObjectQuery<ResultChanges>)
                                                r.ResultChanges
                                                .Where
                                                (
                                                    rc =>
                                                        rc.ResultStatus.ResultStatusDescription == "Added"
                                                        || rc.ResultStatus.ResultStatusDescription == "Amended"
                                                )
                                                .OrderByDescending
                                                (
                                                    rc =>
                                                        rc.ChangeID
                                                )
                                                //.AsQueryable())
                                                //.Include("Points")
                                                //.Include("ResultStatus")
                                                .FirstOrDefault(),
                                            LatestChangePoints =
                                            r.ResultChanges
                                            .Where
                                            (
                                                rc =>
                                                    rc.ResultStatus.ResultStatusDescription == "Added"
                                                    || rc.ResultStatus.ResultStatusDescription == "Amended"
                                            )
                                            .OrderByDescending
                                            (
                                                rc =>
                                                    rc.ChangeID
                                            )                                            
                                            .FirstOrDefault().Points,
                                            LatestChangeResultStatus =
                                                r.ResultChanges
                                                .Where
                                                (
                                                    rc =>
                                                        rc.ResultStatus.ResultStatusDescription == "Added"
                                                        || rc.ResultStatus.ResultStatusDescription == "Amended"
                                                )
                                                .OrderByDescending
                                                (
                                                    rc =>
                                                        rc.ChangeID
                                                )
                                                .FirstOrDefault().ResultStatus,
                                            LatestChangeResultChanges=
                                            r.ResultChanges
                                            .Where
                                            (
                                                rc =>
                                                    rc.ResultStatus.ResultStatusDescription == "Added"
                                                    || rc.ResultStatus.ResultStatusDescription == "Amended"
                                            )
                                            .OrderByDescending
                                            (
                                                rc =>
                                                    rc.ChangeID
                                            )
                                            .FirstOrDefault().Changes,

                                            OriginalChange =
                                                //((System.Data.Objects.ObjectQuery<ResultChanges>)
                                                r.ResultChanges.Where
                                                (
                                                    rc =>
                                                        rc.ResultStatus.ResultStatusDescription == "Added"
                                                        || rc.ResultStatus.ResultStatusDescription == "Unamended"
                                                )
                                                .OrderBy
                                                (
                                                    rc =>
                                                        rc.ChangeID
                                                )
                                                //)
                                                //.Include("Points")
                                                //.Include("ResultStatus")
                                                //.Include("Changes")
                                                .FirstOrDefault(),
                                            OriginalChangePoints =
                                            ((System.Data.Objects.ObjectQuery<ResultChanges>)
                                            r.ResultChanges.Where
                                            (
                                                rc =>
                                                    rc.ResultStatus.ResultStatusDescription == "Added"
                                                    || rc.ResultStatus.ResultStatusDescription == "Unamended"
                                            )
                                            .OrderBy
                                            (
                                                rc =>
                                                    rc.ChangeID
                                            )
                                            )                                            
                                            .FirstOrDefault().Points,
                                            OriginalChangeResultStatus =
                                            ((System.Data.Objects.ObjectQuery<ResultChanges>)
                                            r.ResultChanges.Where
                                            (
                                                rc =>
                                                    rc.ResultStatus.ResultStatusDescription == "Added"
                                                    || rc.ResultStatus.ResultStatusDescription == "Unamended"
                                            )
                                            .OrderBy
                                            (
                                                rc =>
                                                    rc.ChangeID
                                            )
                                            )
                                            .FirstOrDefault().ResultStatus,
                                            OriginalChangeChanges =
                                            ((System.Data.Objects.ObjectQuery<ResultChanges>)
                                            r.ResultChanges.Where
                                            (
                                                rc =>
                                                    rc.ResultStatus.ResultStatusDescription == "Added"
                                                    || rc.ResultStatus.ResultStatusDescription == "Unamended"
                                            )
                                            .OrderBy
                                            (
                                                rc =>
                                                    rc.ChangeID
                                            )
                                            )
                                            .FirstOrDefault().Changes,
                                            LatestStudentChange =
                                                r.Students.StudentChanges
                                                .OrderByDescending(sc => sc.ChangeID)
                                                .FirstOrDefault(),
                                            ScrutinyStatus =
                                                (
                                                    from rr in r.ResultRequests
                                                    where rr.Changes.ChangeID ==                                                        
                                                            r.ResultChanges
                                                            .Where
                                                            (
                                                                rc =>
                                                                    rc.ResultStatus.ResultStatusDescription == "Added"
                                                                    || rc.ResultStatus.ResultStatusDescription == "Amended"
                                                            )
                                                            .OrderByDescending
                                                            (
                                                                rc =>
                                                                    rc.ChangeID
                                                            )
                                                            .FirstOrDefault().ChangeID                                                        
                                                    select new
                                                    {
                                                        rr.ResultRequestChanges.Where(rrc=>rrc.DateEnd==null).FirstOrDefault().ScrutinyStatus
                                                    }
                                                ).FirstOrDefault().ScrutinyStatus
                                        }
                                );

                    query = query.Where(r => r.LatestChange != null);
 
                    //if it is a June KS4 checking exercise only return graded results

                    if (isJuneChecking)
                    {
                        //get graded exams if ks4 else all if not ks4
                        query = query.Where
                                 (
                                     r => 
                                         (
                                             r.Result.Students.Cohorts.KeyStage == 4 && 
                                             r.Result.SubLevels.SubLevelCode.CompareTo("831") == -1 && 
                                             r.Result.SubLevels.SubLevelCode.CompareTo("800") == 1
                                         ) || 
                                         r.Result.Students.Cohorts.KeyStage != 4
                                 );
                    }

                    query = query.Where(r=>
                                      !(
                                          (r.SubLevels.SubLevelDescription.Contains("KS2") || r.SubLevels.SubLevelDescription.Contains("KS3"))
                                       && (r.SubLevels.SubLevelCode.Contains("550") || r.SubLevels.SubLevelCode.Contains("650"))
                                      )
                                      &&
                                       (r.OriginalChange != null && r.ScrutinyStatus != null && r.ScrutinyStatus.ScrutinyStatusCode != "N")
                                      );

					if (sortExpression.Contains("ForvusNo"))
					{
					    if (sortExpression.Contains("ASC"))
                            query = query.OrderBy(r => r.Result.Students.ForvusIndex);
                        else
                            query = query.OrderByDescending(r => r.Result.Students.ForvusIndex);
					}
					else if (sortExpression.Contains("Surname"))
					{
                        if (sortExpression.Contains("ASC"))
                            query = query.OrderBy(r => r.LatestStudentChange.Surname);
                        else
                            query = query.OrderByDescending(r => r.LatestStudentChange.Surname);
					}
					else if (sortExpression.Contains("Forename"))
					{
                        if (sortExpression.Contains("ASC"))
                            query = query.OrderBy(r => r.LatestStudentChange.Forename);
                        else
                            query = query.OrderByDescending(r => r.LatestStudentChange.Forename);
					}
					
					else if (sortExpression.Contains("Notes"))
					{
                        // TODO: Determine sort order for 'Notes'                        
					}
                    else if (sortExpression.Contains("AwardingBodyName"))
					{
                        if (sortExpression.Contains("ASC"))
                            query = query.OrderBy(r => r.AwardingBody.AwardingBodyName);
                        else
                            query = query.OrderByDescending(r => r.AwardingBody.AwardingBodyName);
					}
					else if (sortExpression.Contains("Session"))
					{
                        if (sortExpression.Contains("ASC"))
                            query = query
                                .OrderBy(r => r.Result.Seasons.SeasonCode)
                                .ThenBy(r => r.Result.ExamYear);
                        else
                            query = query
                                .OrderByDescending(r => r.Result.Seasons.SeasonCode)
                                .ThenByDescending(r => r.Result.ExamYear);
					}
                    else if (sortExpression.Contains("ExamYear"))
					{
                        if (sortExpression.Contains("ASC"))
                            query = query.OrderBy(r => r.Result.ExamYear);
                        else
                            query = query.OrderByDescending(r => r.Result.ExamYear);
					}
					else if (sortExpression.Contains("Qualification"))
					{
                        if (sortExpression.Contains("ASC"))
                            query = query.OrderBy(r => r.Result.QualificationTypes.QualificationTypeTitle);
                        else
                            query = query.OrderByDescending(r => r.Result.QualificationTypes.QualificationTypeTitle);
					}
                    else if (sortExpression.Contains("SubjectDescription"))
					{
					    // TODO: Determine sort order for 'Test'
					}
					else if (sortExpression.Contains("Syllabus"))
					{
                        if (sortExpression.Contains("ASC"))
                            query = query.OrderBy(r => r.Result.BoardSubjectNumber);
                        else
                            query = query.OrderByDescending(r => r.Result.BoardSubjectNumber);
					}
					else if (sortExpression.Contains("QAN"))
					{
                        if (sortExpression.Contains("ASC"))
                            query = query.OrderBy(r => r.Result.QANs == null ? (string)null : r.Result.QANs.QUID);
                        else
                            query = query.OrderByDescending(r => r.Result.QANs == null ? (string)null : r.Result.QANs.QUID);
					}
                    else if (sortExpression.Contains("Title"))
					{
                        if (sortExpression.Contains("ASC"))
                            query = query.OrderBy(r => r.Result.Subjects.SubjectDescription);
                        else
                            query = query.OrderByDescending(r => r.Result.Subjects.SubjectDescription);
					}
                    else if (sortExpression.Contains("NationalCentreNumber"))
                    {
                        if (sortExpression.Contains("ASC"))
                            query = query.OrderBy(r => r.Result.NationalCentreNumber);
                        else
                            query = query.OrderByDescending(r => r.Result.NationalCentreNumber);
                    }
                    else if (sortExpression.Contains("NewGrade"))
                    {
                        if (sortExpression.Contains("ASC"))
                            query = query.OrderBy(r => r.LatestChange.Points.GradeCode);
                        else
                            query = query.OrderByDescending(r => r.LatestChange.Points.GradeCode);
                    }
                    else if (sortExpression.Contains("TierCode"))
                    {
                        if (sortExpression.Contains("ASC"))
                            query = query.OrderBy(r => r.LatestChange.Points.GradeCode);
                        else
                            query = query.OrderByDescending(r => r.LatestChange.Points.GradeCode);
                    }
                    else if (sortExpression.Contains("TestMark"))
                    {
                        if (sortExpression.Contains("ASC"))
                            query = query.OrderBy(r => r.LatestChange.TestMark);
                        else
                            query = query.OrderByDescending(r => r.LatestChange.TestMark);
                    }
                    else if (sortExpression.Contains("FineGrade"))
                    {
                        if (sortExpression.Contains("ASC"))
                            query = query.OrderBy(r => r.LatestChange.Finegrade);
                        else
                            query = query.OrderByDescending(r => r.LatestChange.Finegrade);
                    }
                    
                    else if (sortExpression.Contains("Notes"))
                    {
                        // TODO: Add proper sort logic for 'Notes'
                        query = query.OrderBy(r => r.Result.ResultID);
                    }
                    else if (sortExpression.Contains("Decision"))
                    {
                        if (sortExpression.Contains("ASC"))
                            result.OrderBy(r => r.ScrutinyStatus.ScrutinyStatusDescription);
                        else
                            result.OrderByDescending(r => r.ScrutinyStatus.ScrutinyStatusDescription);
                    }

                    else if (sortExpression.Contains("ScrutinyStatus"))
                    {
                        if (sortExpression.Contains("ASC"))
                            result.OrderBy(r => r.ScrutinyStatus.ScrutinyStatusCode);
                        else
                            result.OrderByDescending(r => r.ScrutinyStatus.ScrutinyStatusCode);
                    }
                    else
                    {
                        query = query.OrderBy(r => r.Result.ResultID);
                    }

                    var results1 = query
                        .Skip((page - 1) * rowsPerPage)
                        .Take(rowsPerPage)
                        .ToList();

					for (int i = 0; i < results1.Count; i++)
					{
						ResultWithScrutinyStatus rs = new ResultWithScrutinyStatus();

						if (results1[i].OriginalChange != null)
						{
                            if(!results1[i].OriginalChange.ResultStatusReference.IsLoaded)
							    results1[i].OriginalChange.ResultStatusReference.Load();

                            if(!results1[i].OriginalChange.PointsReference.IsLoaded)
							    results1[i].OriginalChange.PointsReference.Load();
						}
                        
                        if (results1[i].LatestChange != null)
                        {
                            if(!results1[i].LatestChange.ResultStatusReference.IsLoaded)
                                results1[i].LatestChange.ResultStatusReference.Load();
                            if(!results1[i].LatestChange.PointsReference.IsLoaded)
                                results1[i].LatestChange.PointsReference.Load();
                            if(!results1[i].LatestChange.ChangesReference.IsLoaded)
                                results1[i].LatestChange.ChangesReference.Load();
                        }
                        
						if (results1[i].StudentChanges != null)
						{
                            if(!results1[i].Result.StudentsReference.IsLoaded)
							    results1[i].Result.StudentsReference.Load();

							results1[i].Result.Students.StudentChanges.Attach(results1[i].StudentChanges.First());
						}
                        
                        if(!results1[i].Result.SeasonsReference.IsLoaded)
						    results1[i].Result.SeasonsReference.Load();

                        if(!results1[i].Result.RINCLsReference.IsLoaded)
						    results1[i].Result.RINCLsReference.Load();

                        if(!results1[i].Result.SubjectsReference.IsLoaded)
						    results1[i].Result.SubjectsReference.Load();

                        if(!results1[i].Result.QualificationTypesReference.IsLoaded)
						    results1[i].Result.QualificationTypesReference.Load();
						
                        if(!results1[i].Result.QANsReference.IsLoaded)
                            results1[i].Result.QANsReference.Load();

                        if(!results1[i].Result.QualificationTypes.QualificationTypeCollections.IsLoaded)
						    results1[i].Result.QualificationTypes.QualificationTypeCollections.Load();

						rs.Results = results1[i].Result;

                        if (!rs.Results.QualificationTypes.SubLevels.IsLoaded)
                        {
                            //rs.Results.QualificationTypes.SubLevels.Load();
                            rs.Results.QualificationTypes.SubLevels.Add(results1[i].SubLevels);
                        }

                        if (results1[i].OriginalChange != null)
                        {
                            if(!results1[i].OriginalChange.ChangesReference.IsLoaded)
                                results1[i].OriginalChange.ChangesReference.Load();
                        }

						rs.FirstChange = results1[i].OriginalChange;
						rs.LastChange = results1[i].LatestChange;

                        //load the scrutiny status if it exists
                        if (rs.LastChange != null)
                        {
                            rs.ScrutinyStatus = results1[i].ScrutinyStatus;

                            //var qSS = (from rr in context.ResultRequests
                            //           where rr.Results.ResultID == rs.Results.ResultID
                            //                  && rr.Changes.ChangeID == rs.LastChange.ChangeID
                            //           select new { rr }).ToList();

                            //if (qSS.Count() > 0)
                            //{
                            //    ResultRequests rr1 = qSS.FirstOrDefault().rr;
                            //    if (rr1 != null)
                            //    {
                            //        var qRRC = (from rrc in context.ResultRequestChanges
                            //                    where rrc.ResultRequests.ResultRequestID == rr1.ResultRequestID
                            //                          && rrc.DateEnd == null
                            //                   select  rrc ).ToList();

                            //        if (qRRC.Count > 0)
                            //        {
                            //            ResultRequestChanges rrc1 = qRRC.First();
                            //            rrc1.ScrutinyStatusReference.Load();
                            //            rs.ScrutinyStatus = rrc1.ScrutinyStatus;
                            //        }
                            //    }
                            //}
                        }

						//load the national centre name
                        //var q = (from n in context.NationalCentreNumbers
                        //         where n.NationalCentreNumber == rs.Results.NationalCentreNumber
                        //         select new { n }).ToList();
                        //if (q.Count > 0)
                        //{
                        //    NationalCentreNumbers ncn = q.First().n;

                        //    rs.NationalCentreName = ncn.NationalCentreName;
                        //}

                        rs.NationalCentreName = results1[i].NationalCentreName;

						//include the result in actual result only if it is not a KS2/KS3 Single Level Test
                        //SubLevels sl = rs.Results.QualificationTypes.SubLevels.First();
                        //SubLevels sl = results1[i].SubLevels;

                        //if (!(
                        //        (sl.SubLevelDescription.Contains("KS2") || sl.SubLevelDescription.Contains("KS3"))
                        //     && (sl.SubLevelCode.Contains("550") || sl.SubLevelCode.Contains("650"))
                        //    ))
                        //{
                        //    if (rs.FirstChange != null && rs.ScrutinyStatus != null && rs.ScrutinyStatus.ScrutinyStatusCode != "N")
                        //    {
								result.Add(rs);                                
                        //    }
                        //}
					}				
                   

					return result                        
                        .ToList();
                }

            }

            throw new NotImplementedException();
        }
    }
}