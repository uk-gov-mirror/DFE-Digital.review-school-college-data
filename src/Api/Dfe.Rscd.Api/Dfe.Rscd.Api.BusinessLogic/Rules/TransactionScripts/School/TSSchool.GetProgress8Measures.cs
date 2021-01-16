using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Web.Script.Serialization;
using Web09.Checking.DataAccess;
using Web09.Services.Common.JSONObjects;

namespace Web09.Checking.Business.Logic.TransactionScripts
{  
    public partial class TSSchool : Logic.TSBase
    {

        public static string GetProgress8Measures(int dfesNumber)
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
                        cmd.CommandText = "School.GetProgress8Measures";
                       
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@DFESNumber",
                            SqlValue = dfesNumber
                        });                       

                        System.Data.Common.DbDataReader dr = cmd.ExecuteReader();
                        List<Progress8Measure> measuresList = new List<Progress8Measure>();                     
                        while (dr.Read())
                        {
                            Progress8Measure progress8Measure = new Progress8Measure
                            {
                                IsHeader               = Convert.ToBoolean(dr[0]),
                                MeasureTypeCode        = Convert.ToString( dr[1] ),
                                MeasureTypeDescription = Convert.ToString( dr[2] ),
                                Value                  = Convert.ToString( dr[3] )
                            };

                            measuresList.Add(progress8Measure);
                        }

                        dr.Close();

                        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                        string jsonData = jsSerializer.Serialize(measuresList);

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
