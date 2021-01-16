using System;
using System.Data.EntityClient;

using Web09.Checking.DataAccess;


namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSCohortAdjustmentRequests: Logic.TSBase
    {
        public static int GetCohortAdjustmentRequestsCount(
            string requestStatus, 
            short keyStageID, 
            int dcsfNumber, 
            int forvusIndex, 
            string surname, 
            string scrutinyStatus, 
            string yearGroupCode, 
            Int16 schoolGroupID,
            bool IsJuneDecisionsRequest,
            string updatedBy, 
            DateTime? updatedAfter, 
            String currentUserName)
        {
            try
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {                        
                        String strColumn = "";
                        String strDirection = "";
                       
                        strColumn = "Surname";
                        strDirection = "ASC";

                        System.Data.Common.DbConnection connection = conn.StoreConnection;

                        System.Data.Common.DbCommand cmd = connection.CreateCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Scrutiny.CohortAdjustmentRequest_GetPage";

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                           DbType = System.Data.DbType.Int32,
                           Direction = System.Data.ParameterDirection.Input,
                           ParameterName = "@KeyStage",
                           SqlValue = keyStageID
                        });

                        if (dcsfNumber > 0 || dcsfNumber == -1)
                           cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                           {
                               DbType = System.Data.DbType.Int32,
                               Direction = System.Data.ParameterDirection.Input,
                               ParameterName = "@DCSFNumber",
                               SqlValue = dcsfNumber
                           });

                        if (forvusIndex > 0 || forvusIndex == -1)
                           cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                           {
                               DbType = System.Data.DbType.Int32,
                               Direction = System.Data.ParameterDirection.Input,
                               ParameterName = "@ForvusIndex",
                               SqlValue = forvusIndex
                           });

                        if (surname != null && surname != "")
                           cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                           {
                               DbType = System.Data.DbType.String,
                               Direction = System.Data.ParameterDirection.Input,
                               ParameterName = "@Surname",
                               SqlValue = surname
                           });

                        if (scrutinyStatus != null && scrutinyStatus != "")
                           cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                           {
                               DbType = System.Data.DbType.String,
                               Direction = System.Data.ParameterDirection.Input,
                               ParameterName = "@ScrutinyStatus",
                               SqlValue = scrutinyStatus
                           });


                        if (yearGroupCode != null && yearGroupCode != "")
                           cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                           {
                               DbType = System.Data.DbType.String,
                               Direction = System.Data.ParameterDirection.Input,
                               ParameterName = "@YearGroupCode",
                               SqlValue = yearGroupCode
                           });

                        if (requestStatus != null && requestStatus != "")
                           cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                           {
                               DbType = System.Data.DbType.Int32,
                               Direction = System.Data.ParameterDirection.Input,
                               ParameterName = "@RequestStatus",
                               SqlValue = Int32.Parse(requestStatus)
                           });

                        if (updatedBy != null && updatedBy != "")
                           cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                           {
                               DbType = System.Data.DbType.String,
                               Direction = System.Data.ParameterDirection.Input,
                               ParameterName = "@UpdatedBy",
                               SqlValue = updatedBy
                           });

                        if (updatedAfter.HasValue)
                           cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                           {
                               DbType = System.Data.DbType.DateTime,
                               Direction = System.Data.ParameterDirection.Input,
                               ParameterName = "@UpdatedAfter",
                               SqlValue = updatedAfter.Value
                           });

                        if (schoolGroupID > 0)
                           cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                           {
                               DbType = System.Data.DbType.Int32,
                               Direction = System.Data.ParameterDirection.Input,
                               ParameterName = "@SchoolGroupID",
                               SqlValue = schoolGroupID
                           });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                           DbType = System.Data.DbType.String,
                           Direction = System.Data.ParameterDirection.Input,
                           ParameterName = "@SortExpression",
                           SqlValue = strColumn
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                           DbType = System.Data.DbType.String,
                           Direction = System.Data.ParameterDirection.Input,
                           ParameterName = "@SortDirection",
                           SqlValue = strDirection
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                           DbType = System.Data.DbType.Boolean,
                           Direction = System.Data.ParameterDirection.Input,
                           ParameterName = "@IsJuneCohortRequestDecisions",
                           SqlValue = IsJuneDecisionsRequest
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@UserName",
                            SqlValue = currentUserName
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                           DbType = System.Data.DbType.Int32,
                           Direction = System.Data.ParameterDirection.Input,
                           ParameterName = "@FromRow",
                           SqlValue = 0
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                           DbType = System.Data.DbType.Int32,
                           Direction = System.Data.ParameterDirection.Input,
                           ParameterName = "@ToRow",
                           SqlValue = 0
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                           DbType = System.Data.DbType.Int32,
                           Direction = System.Data.ParameterDirection.Output,
                           ParameterName = "@RowCount"
                        });

                        cmd.ExecuteScalar();

                        if (cmd.Parameters["@RowCount"].Value != null)
                            return int.Parse(cmd.Parameters["@RowCount"].Value.ToString());
                        else
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