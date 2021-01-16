using System;
using System.Data.EntityClient;
using System.Web.Script.Serialization;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSSchool : Logic.TSBase
    {

        public static string GetOptinSchool(int DFESNumber)
        {
            Web09.Services.Common.JSONObjects.SchoolOptin optin = new Web09.Services.Common.JSONObjects.SchoolOptin();

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
                        cmd.CommandText = "Optin.GetSchool";
                        
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@DFESNumber",
                            SqlValue = DFESNumber
                        });


                        System.Data.Common.DbDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {                        
                            optin.DFESNumber           = Convert.ToInt32(dr["DFESNumber"]);
                            optin.SchoolName           = Convert.ToString(dr["SchoolName"]);
                            optin.CreatedDate          = dr.IsDBNull(dr.GetOrdinal("CreatedDate")) ? DateTime.MinValue : Convert.ToDateTime(dr["CreatedDate"]);
                            optin.DeletedDate          = dr.IsDBNull(dr.GetOrdinal("DeletedDate")) ? DateTime.MinValue : Convert.ToDateTime(dr["DeletedDate"]);
                            optin.OptinDate            = dr.IsDBNull(dr.GetOrdinal("OptinDate")) ? DateTime.MinValue : Convert.ToDateTime(dr["OptinDate"]);
                            optin.OptinClientIPAddress = Convert.ToString(dr["OptinClientIPAddress"]);
                            optin.OptinUserID          = Convert.ToString(dr["OptinUserID"]);
                            optin.OptinUserName        = Convert.ToString(dr["OptInUserName"]);
                            optin.OptinRoleName        = Convert.ToString(dr["OptinRoleName"]);
                            optin.OptinFirstName       = Convert.ToString(dr["OptinFirstName"]);
                            optin.OptinSurname         = Convert.ToString(dr["OptinSurname"]);
                            optin.LastLoginDate        = dr.IsDBNull(dr.GetOrdinal("LastLoginDate")) ? DateTime.MinValue : Convert.ToDateTime(dr["LastLoginDate"]);
                        }

                        JavaScriptSerializer javaScriptSerialiser = new JavaScriptSerializer();
                        string jsonData = javaScriptSerialiser.Serialize(optin);
                        return jsonData;                        
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
