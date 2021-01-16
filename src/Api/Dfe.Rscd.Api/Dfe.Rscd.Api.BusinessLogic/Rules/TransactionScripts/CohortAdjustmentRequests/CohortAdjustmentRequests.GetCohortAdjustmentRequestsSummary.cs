using System;
using System.Data.EntityClient;

using Web09.Checking.DataAccess;


namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSCohortAdjustmentRequests: Logic.TSBase
    {
        public static int GetCohortAdjustmentRequestsSummary(Int16 keystage, out Int32 RequestsAccepted, out Int32 RequestsRejected, out Int32 RequestsUndecided, out Int32 SchoolAllAccepted, out Int32 SchoolAllRejected, out Int32 SchoolMixedDecisions)
        {
            try
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        System.Data.Common.DbConnection connection = conn.StoreConnection;

                        System.Data.Common.DbCommand cmd = connection.CreateCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Scrutiny.CohortAdjustmentRequest_GetSummary";

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@KeyStage",
                                SqlValue = keystage
                            });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@AcceptedRequests"
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@RejectedRequests"
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@UndecidedRequests"
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@SchoolAllAcceptedRequests"
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@SchoolAllRejectedRequests"
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@SchoolMixedRequests"
                        });

                        cmd.ExecuteScalar();

                        RequestsAccepted = Int32.Parse(cmd.Parameters["@AcceptedRequests"].Value.ToString());
                        RequestsRejected = Int32.Parse(cmd.Parameters["@RejectedRequests"].Value.ToString());
                        RequestsUndecided = Int32.Parse(cmd.Parameters["@UndecidedRequests"].Value.ToString());
                        SchoolAllAccepted = Int32.Parse(cmd.Parameters["@SchoolAllAcceptedRequests"].Value.ToString());
                        SchoolAllRejected = Int32.Parse(cmd.Parameters["@SchoolAllRejectedRequests"].Value.ToString());
                        SchoolMixedDecisions = Int32.Parse(cmd.Parameters["@SchoolMixedRequests"].Value.ToString());

                        cmd.Dispose();                       

                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
