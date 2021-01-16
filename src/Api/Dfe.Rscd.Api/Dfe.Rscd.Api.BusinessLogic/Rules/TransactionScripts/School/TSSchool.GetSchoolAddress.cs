using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSSchool : Logic.TSBase
    {

        public static SchoolAddress GetSchoolAddress(int schoolDfesNumber)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();
                
                using (Web09_Entities context = new Web09_Entities(conn))
                {

                    try
                    {
                        var add = from sa in context.SchoolAddress
                                  where sa.DFESNumber == schoolDfesNumber
                                  select sa;

                        return add.First();

                    } catch
                    {
                        throw new BusinessLevelException("The school is not valid in the database");    
                    }
                }
            }
        }
        
    }
}
