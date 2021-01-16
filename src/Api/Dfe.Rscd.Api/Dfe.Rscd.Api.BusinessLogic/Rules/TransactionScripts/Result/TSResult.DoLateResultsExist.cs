using System;
using System.Data;
using System.Data.EntityClient;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
        public static int DoLateResultsExist(int dcsfNumber, short? keyStageID)
        {
            int hasLateResults = 0;

            using (var conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (var context = new Web09_Entities(conn))
                {
                    var connection = conn.StoreConnection;

                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[Result].[DoLateResultsExist]";

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@DCSFNumber",
                            SqlValue = dcsfNumber
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@KeyStage",
                            SqlValue = keyStageID
                        });

                       
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@HasLateResults",
                            SqlValue = hasLateResults
                        });

                        cmd.ExecuteNonQuery();
                        hasLateResults = Convert.ToInt32(cmd.Parameters["@HasLateResults"].Value);
                    }
                }

                return hasLateResults;
            }
        }
    }
}
