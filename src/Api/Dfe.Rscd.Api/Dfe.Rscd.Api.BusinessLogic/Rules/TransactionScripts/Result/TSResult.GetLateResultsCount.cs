using System;
using System.Data;
using System.Data.EntityClient;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
        public static int GetLateResultsCount(int dcsfNumber, short? keyStageID)
        {

            using (var conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (var context = new Web09_Entities(conn))
                {
                    var connection = conn.StoreConnection;

                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[Result].[GetLateResultsCount]";
                        SetInputParamForCommand(cmd, "keystage", keyStageID);
                        SetInputParamForCommand(cmd, "dfesnumber", dcsfNumber);

                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
        }
    }
}
