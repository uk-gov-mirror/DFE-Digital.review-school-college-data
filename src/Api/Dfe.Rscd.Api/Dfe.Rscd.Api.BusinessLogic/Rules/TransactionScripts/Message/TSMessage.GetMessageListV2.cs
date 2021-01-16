using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Web.Script.Serialization;
using Web09.Checking.DataAccess;
using Web09.Services.Common.JSONObjects;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSMessage : Logic.TSBase
    {
        public static string GetMessageListV2(int dfesNumber, int keyStage, bool isActive)
        {
            List<Message> jsonResultList = new List<Message>();

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
                        cmd.CommandText = "School.GetMessages";

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@DFESNumber",
                            SqlValue = dfesNumber
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@KeyStage",
                            SqlValue = keyStage
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@IsActive",
                            SqlValue = ( isActive == true ) ? 1 : 0
                        });

                        System.Data.Common.DbDataReader dr = cmd.ExecuteReader();

                        List<String> lst = new List<String>();
                        while (dr.Read())
                        {
                            Message message = new Message();
                            jsonResultList.Add(message);
                            
                            message.MessageTypeName = dr.IsDBNull(dr.GetOrdinal("MessageTypeName")) ? string.Empty : Convert.ToString(dr["MessageTypeName"]);
                            message.MessageID       = dr.IsDBNull(dr.GetOrdinal("MEssageID")) ? 0: Convert.ToInt32(dr["MessageID"]);
                            message.MessageText     = dr.IsDBNull(dr.GetOrdinal("MessageText")) ? string.Empty : Convert.ToString(dr["MessageText"]);
                        }

                        dr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            string jsonData = jsSerializer.Serialize(jsonResultList);

            return jsonData;
        }
    }
}
