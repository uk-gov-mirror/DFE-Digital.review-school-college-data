using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
        public static int GetResultsForSchoolCount(int dcsfNumber)
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
                                            NationalCentreName = context.NationalCentreNumbers.Where(ncn => ncn.NationalCentreNumber == r.NationalCentreNumber).FirstOrDefault().NationalCentreName,

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
                                            LatestChangeResultChanges =
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
                                                        rr.ResultRequestChanges.Where(rrc => rrc.DateEnd == null).FirstOrDefault().ScrutinyStatus
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

                    query = query.Where(r =>
                                      !(
                                          (r.SubLevels.SubLevelDescription.Contains("KS2") || r.SubLevels.SubLevelDescription.Contains("KS3"))
                                       && (r.SubLevels.SubLevelCode.Contains("550") || r.SubLevels.SubLevelCode.Contains("650"))
                                      )
                                      &&
                                       (r.OriginalChange != null && r.ScrutinyStatus != null && r.ScrutinyStatus.ScrutinyStatusCode != "N")
                                      );                                  

                    return query.Count();
                }
            }
        }
    }
}