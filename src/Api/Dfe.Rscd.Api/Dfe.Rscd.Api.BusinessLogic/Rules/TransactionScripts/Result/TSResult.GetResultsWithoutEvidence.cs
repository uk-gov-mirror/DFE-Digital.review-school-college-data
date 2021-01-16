using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
        public static IList<ResultWithScrutinyStatus> GetResultsWithoutEvidence(int dcsfNumber)
        {
            IList<ResultWithScrutinyStatus> result = new List<ResultWithScrutinyStatus>();

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    var query = context.Results
                                .Include("Students")
                                .Include("Cohorts")
                                .Include("Schools")
                                .Where(r => r.Students.Schools.DFESNumber == dcsfNumber)
                                .Select(r =>
                                    new
                                    {
                                        Result = r,
                                        StudentChanges = r.Students.StudentChanges.OrderByDescending(sc => sc.ChangeID).Take(1),
                                        AwardingBody = r.AwardingBodies,
                                        LatestChange = r.ResultChanges.Where(rc => rc.ResultStatus.ResultStatusDescription != "Cancelled").OrderByDescending(rc => rc.ChangeID).FirstOrDefault(),
                                        OriginalChange = r.ResultChanges.Where(rc => rc.ResultStatus.ResultStatusDescription != "Cancelled").OrderBy(rc => rc.ChangeID).FirstOrDefault()                                        

                                    }).Join(
                                        context.ResultRequests,
                                        o => new { o.Result.ResultID, o.LatestChange.ChangeID },
                                        i => new { i.Results.ResultID, i.Changes.ChangeID },
                                        (o, i) => new {o, i.ScrutinyStatus});

                    query = query.Where(r => r.ScrutinyStatus.ScrutinyStatusCode.Equals("N"));

                    var results = query.ToList();
                    for (int i = 0; i < results.Count; i++)
                    {
                        ResultWithScrutinyStatus rs = new ResultWithScrutinyStatus();

                        if (results[i].o.OriginalChange != null)
                        {
                            results[i].o.OriginalChange.ResultStatusReference.Load();
                            results[i].o.OriginalChange.PointsReference.Load();
                        }

                        if (results[i].o.LatestChange != null)
                        {
                            results[i].o.LatestChange.ResultStatusReference.Load();
                            results[i].o.LatestChange.PointsReference.Load();
                            results[i].o.LatestChange.ChangesReference.Load();
                        }

                        if (results[i].o.StudentChanges != null)
                        {
                            results[i].o.Result.StudentsReference.Load();
                            results[i].o.StudentChanges.OrderByDescending(sc => sc.ChangeID).Where(sc => sc.DateEnd == null).First().YearGroupsReference.Load();
                            results[i].o.Result.Students.StudentChanges.Attach(results[i].o.StudentChanges.OrderByDescending(sc => sc.ChangeID).Where(sc => sc.DateEnd == null).First());
                        }

                        results[i].o.Result.SeasonsReference.Load();
                        results[i].o.Result.RINCLsReference.Load();
                        results[i].o.Result.SubjectsReference.Load();
                        results[i].o.Result.QualificationTypesReference.Load();
                        results[i].o.Result.QANsReference.Load();
                        results[i].o.Result.QualificationTypes.QualificationTypeCollections.Load();

                        rs.Results = results[i].o.Result;                        
                        rs.Results.QualificationTypes.SubLevels.Load();
                        rs.ScrutinyStatus = results[i].ScrutinyStatus;

                        if(results[i].o.OriginalChange!=null)
                            results[i].o.OriginalChange.ChangesReference.Load();

                        rs.FirstChange = results[i].o.OriginalChange;
                        rs.LastChange = results[i].o.LatestChange;

                        results[i].o.Result.DataOriginReference.Load();
                        rs.ResultType = results[i].o.Result.DataOrigin.DataOriginDescription;

                        //add this result only if its status = 'Evidence not provided'
                        if (rs.ScrutinyStatus != null && rs.ScrutinyStatus.ScrutinyStatusDescription.Equals("Evidence not provided"))
                        {
                            result.Add(rs);
                        }


                    }
                    return result;
                }
            }

        }
    }
}
                    
        