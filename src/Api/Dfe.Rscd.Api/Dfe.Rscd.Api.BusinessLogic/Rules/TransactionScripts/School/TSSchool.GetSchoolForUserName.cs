using System;
using System.Data.EntityClient;
using System.Web.Script.Serialization;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSSchool : Logic.TSBase
    {
        public static string GetSchoolForUserName(string userName)
        {
            Web09.Services.Common.JSONObjects.SchoolForUserName school = new Web09.Services.Common.JSONObjects.SchoolForUserName();

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
                        cmd.CommandText = "School.GetSchoolForUserName";

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@UserName",
                            SqlValue = userName
                        });
                        
                        System.Data.Common.DbDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            school.DFESNumber = Convert.ToInt32(dr["DFESNumber"]);
                            school.SchoolName = Convert.ToString(dr["SchoolName"]);
                        }


                        JavaScriptSerializer javaScriptSerialiser = new JavaScriptSerializer();
                        string jsonData = javaScriptSerialiser.Serialize(school);
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
