using System;
using System.Data.EntityClient;
using System.Linq;

using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent
    {
        public static Students FillStudentAttributes(Students studentIn)
        {
            using(EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using(Web09_Entities context = new Web09_Entities(conn))
                {

                    if (studentIn.StudentChanges != null && studentIn.StudentChanges.Count > 0 &&
                        studentIn.StudentChanges.First() != null)
                    {
                        DateTime studentDob = ConvertDateTimeDBString(studentIn.StudentChanges.First().DOB);
                        studentIn.StudentChanges.First().Age = (byte)CalculateStudentAge(studentDob);

                        //Get parent ethnicity.
                        string currentEthnicityCode = studentIn.StudentChanges.First().Ethnicities.EthnicityCode;

                        if(!String.IsNullOrEmpty(currentEthnicityCode))
                            studentIn.StudentChanges.First().Ethnicities.ParentEthnicityCode = context.Ethnicities
                                .Where(e => e.EthnicityCode == currentEthnicityCode)
                                .Select(e => e.ParentEthnicityCode)
                                .FirstOrDefault();
                    }

                    if (studentIn.Schools != null)
                        studentIn.Schools = context.Schools.Where(s => s.DFESNumber == studentIn.Schools.DFESNumber).FirstOrDefault();

                    return studentIn;
                }
            }
            
        }
    }
}
