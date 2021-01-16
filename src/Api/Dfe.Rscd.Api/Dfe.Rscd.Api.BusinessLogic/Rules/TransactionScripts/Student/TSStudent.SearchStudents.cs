using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent : Logic.TSBase
    {
        public static IList<Students> SearchStudents(int dfesNumber, int keyStage, string forename, string surname, DateTime dob, string gender)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                try
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {

                        string dobStr = dob.Year.ToString() + ((dob.Month < 10) ? "0" : "") + dob.Month.ToString() + ((dob.Day < 10) ? "0" : "") + dob.Day.ToString();
                        string reversedDobStr = dob.Year.ToString() + ((dob.Day < 10) ? "0" : "") + dob.Day.ToString() + ((dob.Month < 10) ? "0" : "") + dob.Month.ToString();
                        gender = gender.ToUpper();

                        IQueryable<Web09.Checking.DataAccess.Students> query = context.Students
                            .Include("Schools")
                            .Include("PINCLs")
                            .Include("Cohorts")
                            .Include("StudentChanges")
                            .Include("StudentChanges.Ethnicities")
                            .Include("StudentChanges.Languages")
                            .Include("StudentChanges.SENStatus")
                            .Include("StudentChanges.YearGroups")
                            .Where(s => s.StudentChanges.Any(sc => (sc.DateEnd == (DateTime?)null)))
                            .Where(s => (s.Schools.DFESNumber == dfesNumber) && s.Cohorts.KeyStage == keyStage);

                        List<Web09.Checking.DataAccess.Students> filtered = new List<Web09.Checking.DataAccess.Students>();


                        //Search condition 1
                        filtered.AddRange(
                            query
                                .Where(q => q.StudentChanges.Any(sc =>
                                    sc.Forename == forename &&
                                    sc.Surname == surname &&
                                    sc.DOB == dobStr &&
                                    sc.Gender == gender))
                            );

                        //Search condition 2 (appended to the resultset, hence sorted as 2nd priority).
                        filtered.AddRange(
                            query
                                .Where(q => q.StudentChanges.Any(sc =>
                                    sc.Forename == surname &&
                                    sc.Surname == forename &&
                                    sc.DOB == dobStr &&
                                    sc.Gender == gender))
                            );

                        //Search condition 3 (appended to the resultset, hence sorted as 3rd priority).
                        filtered.AddRange(
                            query
                                .Where(q => q.StudentChanges.Any(sc =>
                                    sc.Forename == forename &&
                                    sc.Surname == surname &&
                                    sc.DOB == reversedDobStr &&
                                    sc.Gender == gender))
                            );

                        //Search condition 4 (appended to the resultset, hence sorted as 4th priority).
                        filtered.AddRange(
                            query
                                .Where(q => q.StudentChanges.Any(sc =>
                                    sc.Surname == surname &&
                                    sc.DOB == dobStr &&
                                    sc.Gender == gender))
                            );

                        //Search condition 5 (appended to the resultset, hence sorted as 5th priority).
                        filtered.AddRange(
                            query
                                .Where(q => q.StudentChanges.Any(sc =>
                                    sc.Surname == surname &&
                                    sc.DOB == reversedDobStr &&
                                    sc.Gender == gender))
                            );

                        IQueryable<Web09.Checking.DataAccess.Students> returnList = filtered.AsQueryable();

                        return returnList.Distinct().ToList();
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
