using System;
using System.Data.EntityClient;

using Web09.Checking.DataAccess;

using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSMessage : Logic.TSBase
    {
        public static Int32 GetMessageSchoolCount(Int16 messageID)
        {
            Int32 count = 0;

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    if (!Validation.Message.IsMessageIDValid(context, messageID))
                        throw Web09Exception.GetBusinessException(Web09MessageList.MessageInvalidID);

                    // TFS 18633, replace the very slow LINQ query with a fast stored procedure

                    System.Data.Common.DbConnection connection = conn.StoreConnection;

                    System.Data.Common.DbCommand cmd = connection.CreateCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "School.GetMessageSchoolCount";

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.Int16,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@MessageID",
                        SqlValue = messageID
                    });

                    count = (Int32)cmd.ExecuteScalar();                   
                }
            }

            return count;
        }
    }
}
