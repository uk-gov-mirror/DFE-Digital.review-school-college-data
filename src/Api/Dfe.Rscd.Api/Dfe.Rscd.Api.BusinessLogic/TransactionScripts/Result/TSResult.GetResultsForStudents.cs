using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
        public static IList<ResultWithScrutinyStatus> GetResultsForStudentsEntityFramework(List<int> pupilList, string QAN, string Qualification, string Syllabus, string Awardingbody, int year, string session, string subjectTitle, string resultType)
        {
            IList<ResultWithScrutinyStatus> result = new List<ResultWithScrutinyStatus>();

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                int[] pList = pupilList.ToArray();

                using (Web09_Entities context = new Web09_Entities(conn))
                {

                    //get the awarding body from the code or the actual name that might be passed
                    AwardingBodies awaBody = null;

                    if (Awardingbody != null && !Awardingbody.Equals(string.Empty))
                    {
                        var qA = (from a in context.AwardingBodies
                                  where a.AwardingBodyName.Equals(Awardingbody)
                                  select a).ToList();

                        //the passed in value could be AWA code
                        if (qA.Count <= 1)
                        {
                            qA = (from a in context.AwardingBodies
                                  where a.AwardingBodyCode.Equals(Awardingbody)
                                  select a).ToList();

                            if (qA.Count > 0)
                                awaBody = qA.First();
                        }
                        else
                            awaBody = qA.First();
                    }

                    bool isJuneChecking = TSResult.IsJuneChecking(context);                 

                    //the statement below is a KLUDGE. Need to initialize the variable to something
                    var query1 = context.Results
                                .Where(BuildContainsExpression<Results, int>(r => r.Students.StudentID, pList))
                                .Select(r =>
                                    new
                                    {
                                        Result = r,
                                        StudentChanges = r.Students.StudentChanges.OrderByDescending(sc => sc.ChangeID).Take(1),
                                        AwardingBody = r.AwardingBodies,
                                        LatestChange = r.ResultChanges.Where(rc => rc.ResultStatus.ResultStatusDescription == "Added").OrderByDescending(rc => rc.ChangeID).FirstOrDefault(),
                                        OriginalChange = r.ResultChanges.Where(rc => rc.ResultStatus.ResultStatusDescription == "Added").OrderBy(rc => rc.ChangeID).FirstOrDefault()
                                    });


                    if (resultType.Equals("Added"))
                    {
                        //originalchange in this case is retrieved in order to keep a consistent return value
                        //it will not be used at all for display
                        query1 = context.Results
                               .Where(BuildContainsExpression<Results, int>(r => r.Students.StudentID, pList))
                               .Select(r =>
                                   new
                                   {
                                       Result = r,
                                       StudentChanges = r.Students.StudentChanges.OrderByDescending(sc => sc.ChangeID).Take(1),
                                       AwardingBody = r.AwardingBodies,
                                       LatestChange = r.ResultChanges.Where(rc => rc.ResultStatus.ResultStatusDescription == "Added").OrderByDescending(rc => rc.ChangeID).FirstOrDefault(),
                                       OriginalChange = r.ResultChanges.Where(rc => rc.ResultStatus.ResultStatusDescription == "Added").OrderBy(rc => rc.ChangeID).FirstOrDefault()
                                   });

                        query1 = query1.Where(r => r.Result.DataOrigin.DataOriginID == 3);

                        //if it is a June KS4 checking exercise only return graded results

                        if (isJuneChecking)
                        {
                            //get graded exams if ks4 else all if not ks4
                            query1 = query1.Where(r => (r.Result.Students.Cohorts.KeyStage == 4 && r.Result.SubLevels.SubLevelCode.CompareTo("831") == -1
                                                        && r.Result.SubLevels.SubLevelCode.CompareTo("800") == 1)
                                                         || r.Result.Students.Cohorts.KeyStage != 4);
                        }
                    }

                    if (resultType.Equals("Late"))
                    {
                        query1 = context.Results
                                .Where(BuildContainsExpression<Results, int>(r => r.Students.StudentID, pList))
                                .Select(r =>
                                    new
                                    {
                                        Result = r,
                                        StudentChanges = r.Students.StudentChanges.OrderByDescending(sc => sc.ChangeID).Take(1),
                                        AwardingBody = r.AwardingBodies,
                                        LatestChange = r.ResultChanges.OrderByDescending(rc => rc.ChangeID).FirstOrDefault(),
                                        OriginalChange = r.ResultChanges.OrderBy(rc => rc.ChangeID).FirstOrDefault()
                                    });

                        query1 = query1.Where(r => r.Result.DataOrigin.DataOriginID == 2);

                        //if it is a June KS4 checking exercise only return graded results

                        if (isJuneChecking)
                        {
                            //get graded exams if ks4 else all if not ks4
                            query1 = query1.Where(r => (r.Result.Students.Cohorts.KeyStage == 4 && r.Result.SubLevels.SubLevelCode.CompareTo("831") == -1
                                                        && r.Result.SubLevels.SubLevelCode.CompareTo("800") == 1)
                                                         || r.Result.Students.Cohorts.KeyStage != 4);
                        }

                    }

                    if (resultType.Equals("Initial"))
                    {
                        query1 = context.Results
                                .Where(BuildContainsExpression<Results, int>(r => r.Students.StudentID, pList))
                                .Select(r =>
                                    new
                                    {
                                        Result = r,
                                        StudentChanges = r.Students.StudentChanges.OrderByDescending(sc => sc.ChangeID).Take(1),
                                        AwardingBody = r.AwardingBodies,
                                        LatestChange = r.ResultChanges.Where(rc => rc.ResultStatus.ResultStatusDescription == "Amended").OrderByDescending(rc => rc.ChangeID).FirstOrDefault(),
                                        OriginalChange = r.ResultChanges.Where(rc => rc.DateEnd == null && (rc.ResultStatus.ResultStatusDescription == "Unamended" || rc.ResultStatus.ResultStatusDescription == "Withdrawn")).OrderByDescending(rc => rc.ChangeID).FirstOrDefault()
                                    });

                        query1 = query1.Where(r => r.Result.DataOrigin.DataOriginID == 1 && r.Result.RINCLs != null);

                        //if it is a June KS4 checking exercise dont return initial results

                        if (isJuneChecking)
                        {
                            query1 = query1.Where(r => r.Result.Students.Cohorts.KeyStage != 4);
                        }

                    }

                    if (QAN != null && !QAN.Equals(string.Empty))
                    {
                        query1 = query1.Where(r => r.Result.QANs.QUID == QAN);
                    }

                    if (Qualification != null && !Qualification.Equals(string.Empty))
                    {
                        query1 = query1.Where(r => r.Result.QualificationTypes.QualificationTypeTitle == Qualification);
                    }

                    if (Syllabus != null && !Syllabus.Equals(string.Empty))
                    {
                        query1 = query1.Where(r => r.Result.BoardSubjectNumber == Syllabus);
                    }

                    if (awaBody != null)
                    {
                        query1 = query1.Where(r => r.Result.AwardingBodies.AwardingBodyName == awaBody.AwardingBodyName);
                    }

                    if (year > 0)
                    {
                        query1 = query1.Where(r => r.Result.ExamYear == year);
                    }

                    if (session != null && !session.Equals(string.Empty))
                    {
                        query1 = query1.Where(r => r.Result.Seasons.SeasonCode == session);
                    }

                    if (subjectTitle != null && !subjectTitle.Equals(string.Empty))
                    {
                        query1 = query1.Where(r => r.Result.Subjects.SubjectDescription == subjectTitle);
                    }

                    query1 = query1.OrderBy(r => r.Result.Subjects.SubjectCode);
                    var results1 = query1.ToList();

                    //if late results add additional filter to get current/added results that correspond to late
                    if (resultType.Equals("Late"))
                    {
                        var query2 = query1.GroupJoin(
                                                context.Results.Where(r => r.DataOrigin.DataOriginID != 2),
                                                d => new { d.Result.Students.StudentID, d.Result.SubLevels.SubLevelID, d.Result.Subjects.SubjectID, d.Result.AwardingBodies.AwardingBodyID, d.Result.ExamYear, d.Result.Seasons.SeasonCode },
                                                i => new { i.Students.StudentID, i.SubLevels.SubLevelID, i.Subjects.SubjectID, i.AwardingBodies.AwardingBodyID, i.ExamYear, i.Seasons.SeasonCode },
                                                (d, i) => new { d, i });

                        var results2 = query2.ToList();
                        for (int i = 0; i < results2.Count; i++)
                        {
                            ResultWithScrutinyStatus rs = new ResultWithScrutinyStatus();

                            if (results2[i].i.FirstOrDefault() != null)
                            {
                                results2[i].i.FirstOrDefault().ResultChanges.Load();
                            }

                            if (results2[i].d.LatestChange != null)
                            {
                                results2[i].d.LatestChange.ResultStatusReference.Load();
                                results2[i].d.LatestChange.PointsReference.Load();
                                results2[i].d.LatestChange.ChangesReference.Load();
                            }

                            if (results2[i].d.StudentChanges != null)
                            {
                                results2[i].d.Result.StudentsReference.Load();
                                results2[i].d.Result.Students.StudentChanges.Attach(results1[i].StudentChanges.First());
                            }

                            results2[i].d.Result.SeasonsReference.Load();
                            results2[i].d.Result.RINCLsReference.Load();
                            results2[i].d.Result.SubjectsReference.Load();
                            results2[i].d.Result.QualificationTypesReference.Load();
                            results2[i].d.Result.QANsReference.Load();
                            results2[i].d.Result.QualificationTypes.QualificationTypeCollections.Load();

                            rs.Results = results2[i].d.Result;
                            rs.Results.QualificationTypes.SubLevels.Load();

                            //load the scrutiny status if it exists
                            if (rs.LastChange != null)
                            {
                                var qSS = (from rr in context.ResultRequests
                                           where rr.Results.ResultID == rs.Results.ResultID
                                                  && rr.Changes.ChangeID == rs.LastChange.ChangeID
                                           select new { rr }).ToList();

                                if (qSS.Count() > 0)
                                {
                                    ResultRequests rr1 = qSS.FirstOrDefault().rr;
                                    if (rr1 != null)
                                    {
                                        rr1.ScrutinyStatusReference.Load();
                                        rs.ScrutinyStatus = rr1.ScrutinyStatus;

                                        //backward compatibility - RRC table was newky added
                                        //so latest requests take SS from RRC else from RR

                                        var qRRC = (from rrc in context.ResultRequestChanges
                                                    where rrc.ResultRequests.ResultRequestID == rr1.ResultRequestID
                                                          && rrc.DateEnd == null
                                                    select rrc).ToList();

                                        if (qRRC.Count > 0)
                                        {
                                            ResultRequestChanges rrc1 = qRRC.First();
                                            rrc1.ScrutinyStatusReference.Load();
                                            rs.ScrutinyStatus = rrc1.ScrutinyStatus;
                                        }

                                    }
                                }
                            }
                            //load the national centre name
                            var q = (from n in context.NationalCentreNumbers
                                     where n.NationalCentreNumber == rs.Results.NationalCentreNumber
                                     select new { n }).ToList();
                            if (q.Count > 0)
                            {
                                NationalCentreNumbers ncn = q.First().n;

                                rs.NationalCentreName = ncn.NationalCentreName;
                            }

                            if (results2[i].i.FirstOrDefault() != null)
                            {
                                Results originalResult = results2[i].i.FirstOrDefault();
                                originalResult.DataOriginReference.Load();
                                ResultChanges originalRC = new ResultChanges();

                                if (originalResult.DataOrigin.DataOriginID == 1)
                                {
                                    originalRC = (from rc in context.ResultChanges
                                                  where rc.ResultID == originalResult.ResultID
                                                  && (rc.ResultStatus.ResultStatusDescription.Equals("Withdrawn")
                                                        || rc.ResultStatus.ResultStatusDescription.Equals("Unamended"))
                                                  orderby rc.Changes.ChangeID ascending
                                                  select rc).First();

                                    if (originalRC != null)
                                    {
                                        originalRC.ChangesReference.Load();
                                        rs.FirstChange = originalRC;
                                        rs.FirstChange.PointsReference.Load();
                                        rs.FirstChange.ResultStatusReference.Load();
                                    }
                                }

                                if (originalResult.DataOrigin.DataOriginID == 3)
                                {
                                    var q1 = (from rc in context.ResultChanges
                                              where rc.ResultID == originalResult.ResultID
                                              && !rc.ResultStatus.ResultStatusDescription.Equals("Cancelled")
                                              orderby rc.Changes.ChangeID descending
                                              select rc).ToList();

                                    if (q1.Count > 0)
                                    {
                                        originalRC = q1.First();
                                        if (originalRC.Changes != null)
                                        {
                                            originalRC.ChangesReference.Load();
                                            rs.FirstChange = originalRC;
                                            rs.FirstChange.PointsReference.Load();
                                            rs.FirstChange.ResultStatusReference.Load();
                                        }
                                    }

                                }



                            }
                            rs.LastChange = results2[i].d.LatestChange;

                            rs.Results.Students.StudentChanges.OrderByDescending(sc => sc.ChangeID).Where(sc => sc.DateEnd == null).First().YearGroupsReference.Load();

                            //get the date when the late result was loaded
                            var dateLoadedChange = (from rc in context.ResultChanges
                                                    where rc.ResultID == rs.Results.ResultID
                                                    orderby rc.ChangeID ascending
                                                    select rc).First();

                            if (dateLoadedChange != null)
                            {
                                dateLoadedChange.ChangesReference.Load();
                            }

                            if (dateLoadedChange.Changes != null)
                                rs.DateLoaded = dateLoadedChange.Changes.ChangeDate;

                            //include the result in actual result only if it is not a KS2/KS3 Single Level Test
                            SubLevels sl = rs.Results.QualificationTypes.SubLevels.First();
                            if (!(
                                    (sl.SubLevelDescription.Contains("KS2") || sl.SubLevelDescription.Contains("KS3"))
                                 && (sl.SubLevelCode.Contains("550") || sl.SubLevelCode.Contains("650"))
                                ))
                            {
                                if (rs.LastChange != null)
                                {
                                    result.Add(rs);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < results1.Count; i++)
                        {
                            ResultWithScrutinyStatus rs = new ResultWithScrutinyStatus();

                            if (results1[i].OriginalChange != null)
                            {
                                results1[i].OriginalChange.ResultStatusReference.Load();
                                results1[i].OriginalChange.PointsReference.Load();
                            }

                            if (results1[i].LatestChange != null)
                            {
                                results1[i].LatestChange.ResultStatusReference.Load();
                                results1[i].LatestChange.PointsReference.Load();
                                results1[i].LatestChange.ChangesReference.Load();
                            }

                            if (results1[i].StudentChanges != null)
                            {
                                results1[i].Result.StudentsReference.Load();
                                results1[i].Result.Students.StudentChanges.Attach(results1[i].StudentChanges.First());
                            }

                            results1[i].Result.SeasonsReference.Load();
                            results1[i].Result.RINCLsReference.Load();
                            results1[i].Result.SubjectsReference.Load();
                            results1[i].Result.QualificationTypesReference.Load();
                            results1[i].Result.QANsReference.Load();
                            results1[i].Result.QualificationTypes.QualificationTypeCollections.Load();

                            rs.Results = results1[i].Result;
                            rs.Results.QualificationTypes.SubLevels.Load();

                            if (results1[i].OriginalChange != null)
                                results1[i].OriginalChange.ChangesReference.Load();

                            rs.FirstChange = results1[i].OriginalChange;
                            rs.LastChange = results1[i].LatestChange;

                            if (rs.LastChange != null)
                            {
                                //load the scrutiny status if it exists
                                var qSS = (from rr in context.ResultRequests
                                           where rr.Results.ResultID == rs.Results.ResultID
                                                  && rr.Changes.ChangeID == rs.LastChange.ChangeID
                                           select new { rr }).ToList();

                                if (qSS.Count() > 0)
                                {
                                    ResultRequests rr1 = qSS.FirstOrDefault().rr;
                                    if (rr1 != null)
                                    {
                                        rr1.ScrutinyStatusReference.Load();
                                        rs.ScrutinyStatus = rr1.ScrutinyStatus;

                                        //backward compatibility - RRC table was newky added
                                        //so latest requests take SS from RRC else from RR

                                        var qRRC = (from rrc in context.ResultRequestChanges
                                                    where rrc.ResultRequests.ResultRequestID == rr1.ResultRequestID
                                                          && rrc.DateEnd == null
                                                    select rrc).ToList();

                                        if (qRRC.Count > 0)
                                        {
                                            ResultRequestChanges rrc1 = qRRC.First();
                                            rrc1.ScrutinyStatusReference.Load();
                                            rs.ScrutinyStatus = rrc1.ScrutinyStatus;
                                        }

                                    }
                                }
                            }

                            rs.Results.Students.StudentChanges.OrderByDescending(sc => sc.ChangeID).Where(sc => sc.DateEnd == null).First().YearGroupsReference.Load();

                            //load the national centre name
                            var q = (from n in context.NationalCentreNumbers
                                     where n.NationalCentreNumber == rs.Results.NationalCentreNumber
                                     select new { n }).ToList();
                            if (q.Count > 0)
                            {
                                NationalCentreNumbers ncn = q.First().n;

                                rs.NationalCentreName = ncn.NationalCentreName;
                            }

                            //include the result in actual result only if it is not a KS2/KS3 Single Level Test
                            SubLevels sl = rs.Results.QualificationTypes.SubLevels.First();
                            if (!(
                                    (sl.SubLevelDescription.Contains("KS2") || sl.SubLevelDescription.Contains("KS3"))
                                 && (sl.SubLevelCode.Contains("550") || sl.SubLevelCode.Contains("650"))
                                ))
                            {
                                if (!(resultType.Equals("Added") && rs.FirstChange == null))
                                {
                                    result.Add(rs);
                                }
                            }

                        }
                    }
                    return result;
                }

            }
        }


        //to get an expression for IN clause
        static Expression<Func<TElement, bool>> BuildContainsExpression<TElement, TValue>(Expression<Func<TElement, TValue>> valueSelector, IEnumerable<TValue> values)
        {

            if (null == valueSelector) { throw new ArgumentNullException("valueSelector"); }

            if (null == values) { throw new ArgumentNullException("values"); }

            ParameterExpression p = valueSelector.Parameters.Single();

            // p => valueSelector(p) == values[0] || valueSelector(p) == ...

            if (!values.Any())
            {

                return e => false;

            }

            var equals = values.Select(value => (Expression)Expression.Equal(valueSelector.Body, Expression.Constant(value, typeof(TValue))));

            var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));

            return Expression.Lambda<Func<TElement, bool>>(body, p);


        }

        public static IList<ResultWithScrutinyStatus> GetResultsForStudents(List<int> pupilList,
            string QAN,
            string Qualification,
            string Syllabus,
            string Awardingbody,
            int year,
            string session,
            string subjectTitle,
            string resultType,
            short keyStageID)
        {
            IList<ResultWithScrutinyStatus> returnList = new List<ResultWithScrutinyStatus>();

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {

                conn.Open();
                System.Data.Common.DbConnection connection = conn.StoreConnection;
                System.Data.Common.DbCommand cmd = connection.CreateCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Student.GetResultsForStudents";

                StringBuilder pupilIdListParameter = new StringBuilder();
                for(int i = 0; i < pupilList.Count; i++)
                {
                    pupilIdListParameter.Append(pupilList[i]);
                    pupilIdListParameter.Append(((i == pupilList.Count - 1) ? "" : ","));
                }
                
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                {
                    DbType = System.Data.DbType.String,
                    Direction = System.Data.ParameterDirection.Input,
                    ParameterName = "@StudentIDList",
                    SqlValue = pupilIdListParameter.ToString()
                });

                if (!String.IsNullOrEmpty(QAN))
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.String,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@qan",
                        SqlValue = QAN
                    });
                }

                if (!String.IsNullOrEmpty(Qualification))
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.String,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@QualificationTypeTitle",
                        SqlValue = Qualification
                    });
                }

                if (!String.IsNullOrEmpty(Syllabus))
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.String,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@syllabus",
                        SqlValue = Syllabus
                    });
                }

                if (!String.IsNullOrEmpty(Awardingbody))
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.String,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@AwardingBodyCode",
                        SqlValue = Awardingbody
                    });
                }

                if (year != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@ExamYear",
                        SqlValue = year
                    });
                }

                if (!String.IsNullOrEmpty(session))
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.String,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@Session",
                        SqlValue = session
                    });
                }

                if (!String.IsNullOrEmpty(subjectTitle))
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.String,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@SubjectTitle",
                        SqlValue = subjectTitle
                    });
                }

                if (!String.IsNullOrEmpty(resultType))
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.String,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@ResultType",
                        SqlValue = resultType
                    });
                }

                //Defect #1866: AS level results withdrawal accepted without evidence. Problem in AS results available as KS4 and KS5 qualificaton type results.
                // We need to provide stored procedure with information about Student's KS.
                if (keyStageID != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.Int16,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@resultKeyStageID",
                        SqlValue = keyStageID.ToString()
                    });
                }
                
                System.Data.Common.DbDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ResultWithScrutinyStatus rs = new ResultWithScrutinyStatus();

                    Results result = new Results
                    {
                        ResultID = dr.IsDBNull(dr.GetOrdinal("ResultID")) ? 0 : int.Parse(dr["ResultID"].ToString()),
                        BoardSubjectNumber = dr.IsDBNull(dr.GetOrdinal("BoardSubjectNumber")) ? String.Empty : dr["BoardSubjectNumber"].ToString(),
                        ExamYear = dr.IsDBNull(dr.GetOrdinal("ExamYear")) ? 0 : int.Parse(dr["ExamYear"].ToString()),
                        NationalCentreNumber = dr.IsDBNull(dr.GetOrdinal("NationalCentreNumber")) ? String.Empty : dr["NationalCentreNumber"].ToString()
                    };
                    

                    if (!dr.IsDBNull(dr.GetOrdinal("DisplayFlag")))
                    {
                        result.RINCLs = new RINCLs
                        {
                            DisplayFlag = dr["DisplayFlag"].ToString(),
                            R_INCLDescription = dr.IsDBNull(dr.GetOrdinal("R_INCLDescription")) ? String.Empty : dr["R_INCLDescription"].ToString()
                        };
                    }

                    if (!dr.IsDBNull(dr.GetOrdinal("AwardingBodyID")))
                    {
                        result.AwardingBodies = new AwardingBodies
                        {
                            AwardingBodyID = int.Parse(dr["AwardingBodyID"].ToString()),
                            AwardingBodyCode = dr.IsDBNull(dr.GetOrdinal("AwardingBodyCode")) ? String.Empty : dr["AwardingBodyCode"].ToString(),
                            AwardingBodyName = dr.IsDBNull(dr.GetOrdinal("AwardingBodyName")) ? String.Empty : dr["AwardingBodyName"].ToString()
                        };
                    }

                    if (!dr.IsDBNull(dr.GetOrdinal("QualificationTypeID")))
                    {
                        result.QualificationTypes = new QualificationTypes
                        {
                            QualificationTypeID = int.Parse(dr["QualificationTypeID"].ToString()),
                            QualificationTypeTitle = dr.IsDBNull(dr.GetOrdinal("QualificationTypeTitle")) ? String.Empty : dr["QualificationTypeTitle"].ToString()
                        };

                        if(!dr.IsDBNull(dr.GetOrdinal("SubLevelCode")))
                        {
                            result.QualificationTypes.SubLevels.Add( new SubLevels 
                            { 
                                SubLevelCode = dr["SubLevelCode"].ToString(),
                                SubLevelDescription = !dr.IsDBNull(dr.GetOrdinal("SubLevelDescription")) ? String.Empty : dr["SubLevelDescription"].ToString() 
                            });
                        }

                        //Get the results qualification type collection item.
                        if (!dr.IsDBNull(dr.GetOrdinal("QualificationTypeCollectionID")))
                        {
                            result.QualificationTypes.QualificationTypeCollections.Add(
                                new QualificationTypeCollections
                                {
                                    QualificationTypeCollectionID = int.Parse(dr["QualificationTypeCollectionID"].ToString()),
                                    QualificationTypeCollectionCode = dr.IsDBNull(dr.GetOrdinal("QualificationTypeCollectionCode")) ? String.Empty :  dr["QualificationTypeCollectionCode"].ToString()
                                });
                        }
                    }

                    if (!dr.IsDBNull(dr.GetOrdinal("QANID")))
                    {
                        result.QANs = new QANs
                        {
                            QANID = int.Parse(dr["QANID"].ToString()),
                            QUID = dr.IsDBNull(dr.GetOrdinal("QUID")) ? String.Empty : dr["QUID"].ToString()
                        };
                    }

                    if (!dr.IsDBNull(dr.GetOrdinal("SeasonCode")))
                    {
                        result.Seasons = new Seasons{ SeasonCode = dr["SeasonCode"].ToString() };
                    }

                    if (!dr.IsDBNull(dr.GetOrdinal("Exam_Date")))
                    {
                        result.Exam_Date = Convert.ToString(dr["Exam_Date"]);
                    }

                    if (!dr.IsDBNull(dr.GetOrdinal("SubjectCode")))
                    {
                        result.Subjects = new Subjects
                        {
                            SubjectCode = dr["SubjectCode"].ToString(),
                            SubjectDescription = dr.IsDBNull(dr.GetOrdinal("StudentID")) ? String.Empty : dr["SubjectDescription"].ToString()
                        };
                    }

                    ResultChanges lastResultChange = null;
                    if (!dr.IsDBNull(dr.GetOrdinal("CurrentChangeID")))
                    {
                        lastResultChange = new ResultChanges
                        {
                            TestMark = dr.IsDBNull(dr.GetOrdinal("CurrentMark")) ? String.Empty : dr["CurrentMark"].ToString(),
                            Finegrade = dr.IsDBNull(dr.GetOrdinal("Finegrade")) ? String.Empty : dr["Finegrade"].ToString(),
                            TierCode = dr.IsDBNull(dr.GetOrdinal("CurrentTierCode")) ? String.Empty : dr["CurrentTierCode"].ToString(),
                            ChangeID = int.Parse(dr["CurrentChangeID"].ToString())
                        };

                        //Get the current change object.
                        lastResultChange.Changes = new Changes { ChangeID = int.Parse(dr["CurrentChangeID"].ToString()) };
                        if (!dr.IsDBNull(dr.GetOrdinal("CurrentChangeDate")))
                            lastResultChange.Changes.ChangeDate = DateTime.Parse(dr["CurrentChangeDate"].ToString());
                    
                        //Add the current point id if it exists in the result set.
                        if (!dr.IsDBNull(dr.GetOrdinal("CurrentPointID")))
                        {
                            lastResultChange.Points = new Points
                            {
                                PointID = int.Parse(dr["CurrentPointID"].ToString()),
                                GradeCode = dr.IsDBNull(dr.GetOrdinal("CurrentGrade")) ? String.Empty : dr["CurrentGrade"].ToString()
                            };
                        }

                        //Add the result status if it exists in the result set.
                        if (!dr.IsDBNull(dr.GetOrdinal("CurrentResultStatusDescription")))
                        {
                            lastResultChange.ResultStatus = new ResultStatus { ResultStatusDescription = dr["CurrentResultStatusDescription"].ToString() };
                        }
                    }

                    ResultChanges firstResultChange = null;
                    if (!dr.IsDBNull(dr.GetOrdinal("OriginalChangeID")))
                    {
                        firstResultChange = new ResultChanges
                        {
                            ChangeID = int.Parse(dr["OriginalChangeID"].ToString()),
                            TestMark = dr.IsDBNull(dr.GetOrdinal("OriginalMark")) ? String.Empty : dr["OriginalMark"].ToString(),
                            Finegrade = dr.IsDBNull(dr.GetOrdinal("Finegrade")) ? String.Empty : dr["Finegrade"].ToString()
                        };
                    }

                    //Add the original point id if it exists in the result set.
                    if (!dr.IsDBNull(dr.GetOrdinal("OriginalPointID")))
                    {
                        firstResultChange.Points = new Points
                        {
                            PointID = int.Parse(dr["OriginalPointID"].ToString()),
                            GradeCode = dr.IsDBNull(dr.GetOrdinal("OriginalGrade")) ? String.Empty : dr["OriginalGrade"].ToString()
                        };
                    }

                    //Add the result status if it exists in the result set.
                    if (!dr.IsDBNull(dr.GetOrdinal("OriginalResultStatusDescription")))
                    {
                        firstResultChange.ResultStatus = new ResultStatus { ResultStatusDescription = dr["OriginalResultStatusDescription"].ToString() };
                    }

                    Students student = new Students
                    {
                        StudentID = dr.IsDBNull(dr.GetOrdinal("StudentID")) ? 0 : int.Parse(dr["StudentID"].ToString()),
                        ForvusIndex = dr.IsDBNull(dr.GetOrdinal("ForvusIndex")) ? 0 : int.Parse(dr["ForvusIndex"].ToString()),

                    };

                    StudentChanges studentChange = new StudentChanges
                    {
                        Forename = dr.IsDBNull(dr.GetOrdinal("Forename")) ? "" : dr["Forename"].ToString(),
                        Surname = dr.IsDBNull(dr.GetOrdinal("Surname")) ? "" : dr["Surname"].ToString(),
                        DOB = dr.IsDBNull(dr.GetOrdinal("DOB")) ? "" : dr["DOB"].ToString(),
                        Gender = dr.IsDBNull(dr.GetOrdinal("Gender")) ? "" : dr["Gender"].ToString(),
                        Age = dr.IsDBNull(dr.GetOrdinal("Age")) ? (byte)0 : byte.Parse(dr["Age"].ToString()),
                        ENTRYDAT = dr.IsDBNull(dr.GetOrdinal("ENTRYDAT")) ? "" : dr["ENTRYDAT"].ToString()
                    };

                    if(!dr.IsDBNull(dr.GetOrdinal("EthnicityCode")))
                    {
                        studentChange.Ethnicities = new Ethnicities{EthnicityCode = dr["ENTRYDAT"].ToString()};
                    }
                    
                    if(!dr.IsDBNull(dr.GetOrdinal("FirstLanguageCode")))
                    {
                        studentChange.Languages = new Languages { LanguageCode = dr["FirstLanguageCode"].ToString() };
                    }

                    if (!dr.IsDBNull(dr.GetOrdinal("ActualYearGroup")))
                    {
                        studentChange.YearGroups = new YearGroups{ YearGroupCode = dr["ActualYearGroup"].ToString() };
                    }

                    student.StudentChanges.Add(studentChange);

                    result.Students = student;

                    rs.NationalCentreName = dr.IsDBNull(dr.GetOrdinal("NationalCentreName")) ? String.Empty : dr["NationalCentreName"].ToString();
                    rs.Results = result;
                    rs.FirstChange = firstResultChange;
                    rs.LastChange = lastResultChange;
                    rs.ResultType = resultType;
                    if(!dr.IsDBNull(dr.GetOrdinal("CurrentResultScrutinyStatusCode")))
                    {
                        rs.ScrutinyStatus = new ScrutinyStatus
                        {
                            ScrutinyStatusCode = dr["CurrentResultScrutinyStatusCode"].ToString()
                        };
                    };

                    if(!dr.IsDBNull(dr.GetOrdinal("ScrutinyEvidenceDetails")))
                    {
                        rs.ScrutinyStatusEvidenceDetails = dr["ScrutinyEvidenceDetails"].ToString();
                    }
                    
                    if(!dr.IsDBNull(dr.GetOrdinal("LateResultDateLoaded"))) rs.DateLoaded =  DateTime.Parse(dr["LateResultDateLoaded"].ToString());

                    if (!dr.IsDBNull(dr.GetOrdinal("ResultReasonText"))) rs.ResultRequestReasonText = dr["ResultReasonText"].ToString();                    

                    //include the result in actual result only if it is not a KS2/KS3 Single Level Test
                    SubLevels sl = rs.Results.QualificationTypes.SubLevels.First();
                    if (!(
                            (sl.SubLevelDescription.Contains("KS2") || sl.SubLevelDescription.Contains("KS3"))
                         && (sl.SubLevelCode.Contains("550") || sl.SubLevelCode.Contains("650"))
                        ))
                    {
                        if (!(resultType.Equals("Added") && rs.FirstChange == null))
                        {
                            returnList.Add(rs);
                        }
                    }


                }
            }

            return returnList;

        }


    }
}
