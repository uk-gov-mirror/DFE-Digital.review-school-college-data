using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSSchool : Logic.TSBase
    {

        public static void UpdateSchoolCheckingStatus(int dfesNumber, short keystage, string checkingStatus, UserContext uc)
        {           
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    //Create the change object.
                    Changes changeObj = CreateChangeObject(context, 1, uc);

                    //get the checking status object
                    var statusQuery = context.CheckingStatus
                            .Where(cs => cs.CheckingStatusDescription == checkingStatus)
                            .ToList();

                    if (statusQuery.Count > 0)
                    {
                        CheckingStatus cstatus = statusQuery.First();

                        var query = context.SchoolCheckingStatus
                                    .Include("Schools")
                                    .Include("Cohorts")
                                    .Include("CheckingStatus")
                                    .Where(scs => scs.Schools.DFESNumber == dfesNumber
                                            && scs.Cohorts.KeyStage == keystage).ToList();

                        //if there exists a row update it
                        if (query.Count > 0)
                        {
                            SchoolCheckingStatus scsPrev = query.First();

                            SchoolCheckingStatus scsNew = scsPrev;

                            scsNew.Changes = changeObj;
                            scsNew.CheckingStatus = cstatus;

                            context.Attach(scsPrev);
                            context.ApplyPropertyChanges("SchoolCheckingStatus", scsNew);
                            
                        }
                        else
                        {
                            //create a new row
                            SchoolCheckingStatus scs = new SchoolCheckingStatus();
                            scs.Changes = changeObj;
                            scs.CheckingStatus = cstatus;
                            scs.Cohorts = context.Cohorts.Where(c => c.KeyStage == keystage).First();
                            scs.Schools = context.Schools.Where(s => s.DFESNumber == dfesNumber).First();

                            context.AddToSchoolCheckingStatus(scs);
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        throw new BusinessLevelException("This checking status is not valid.");
                    }
                    
                }
            }
            
        }
    }
}
