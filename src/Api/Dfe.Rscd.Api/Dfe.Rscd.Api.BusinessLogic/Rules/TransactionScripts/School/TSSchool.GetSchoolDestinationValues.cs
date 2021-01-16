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

        // TODO
        public static string GetSchoolDestinationValues(int DFESNumber, int KeyStage)
        {
            List<SchoolDestinationValue> jsonResultDestinationsList = new List<SchoolDestinationValue>();
            List<SchoolQSRValue> jsonResultQSRList = new List<SchoolQSRValue>();
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            string jsonData = null;

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    System.Data.Common.DbConnection connection = conn.StoreConnection;
                    if (KeyStage == 4)
                    {
                        //Getting Destinations Data from Database
                        System.Data.Common.DbCommand cmd = connection.CreateCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "School.GetSchoolDestinationValues";

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@DFESNumber",
                            SqlValue = DFESNumber
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@KeyStage",
                            SqlValue = KeyStage
                        });

                        System.Data.Common.DbDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            SchoolDestinationValue schoolDestinationValue = new SchoolDestinationValue();
                            jsonResultDestinationsList.Add(schoolDestinationValue);

                            schoolDestinationValue.IsHeader = dr.IsDBNull(dr.GetOrdinal("IsHeader")) ? 0 : Convert.ToInt32(dr["IsHeader"]); ;
                            schoolDestinationValue.DestinationTypeCode = dr.IsDBNull(dr.GetOrdinal("DestinationTypeCode")) ? string.Empty : dr["DestinationTypeCode"].ToString();                            
                            schoolDestinationValue.DestinationTypeDescription = dr.IsDBNull(dr.GetOrdinal("DestinationTypeDescription")) ? string.Empty : dr["DestinationTypeDescription"].ToString();                            
                            schoolDestinationValue.DestinationTypeFootNote = dr.IsDBNull(dr.GetOrdinal("DestinationTypeFootNote")) ? string.Empty : dr["DestinationTypeFootNote"].ToString();
                            schoolDestinationValue.Value = dr.IsDBNull(dr.GetOrdinal("Value")) ? string.Empty : dr["Value"].ToString();
                            schoolDestinationValue.Percentage = dr.IsDBNull(dr.GetOrdinal("Percentage")) ? string.Empty : dr["Percentage"].ToString();
                        }
                        jsonData = jsSerializer.Serialize(jsonResultDestinationsList);
                    }
                    else if (KeyStage == 5)
                    {
                        //Getting QSR Data from Database
                        System.Data.Common.DbCommand cmd = connection.CreateCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "School.GetSchoolQSRValues";

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@DFESNumber",
                            SqlValue = DFESNumber
                        });

                        System.Data.Common.DbDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            SchoolQSRValue schoolQSRValue = new SchoolQSRValue();
                            jsonResultQSRList.Add(schoolQSRValue);

                            schoolQSRValue.RecordType = dr.IsDBNull(dr.GetOrdinal("RECTYPE")) ? 0 : Convert.ToInt32(dr["RECTYPE"]);
                            schoolQSRValue.DFESNumber = dr.IsDBNull(dr.GetOrdinal("LAESTAB")) ? 0 : Convert.ToInt32(dr["LAESTAB"]);
                            schoolQSRValue.URN = dr.IsDBNull(dr.GetOrdinal("URN")) ? 0 : Convert.ToInt32(dr["URN"]);
                            schoolQSRValue.Level1LearningAim = dr.IsDBNull(dr.GetOrdinal("L1LearningAim")) ? string.Empty : dr["L1LearningAim"].ToString();
                            schoolQSRValue.Level1SuccessRate = dr.IsDBNull(dr.GetOrdinal("L1SuccessRate")) ? string.Empty : dr["L1SuccessRate"].ToString();
                            schoolQSRValue.Level2LearningAim = dr.IsDBNull(dr.GetOrdinal("L2LearningAim")) ? string.Empty : dr["L2LearningAim"].ToString();
                            schoolQSRValue.Level2SuccessRate = dr.IsDBNull(dr.GetOrdinal("L2SuccessRate")) ? string.Empty : dr["L2SuccessRate"].ToString();
                            schoolQSRValue.Level3LearningAim = dr.IsDBNull(dr.GetOrdinal("L3LearningAim")) ? string.Empty : dr["L3LearningAim"].ToString();
                            schoolQSRValue.Level3SuccessRate = dr.IsDBNull(dr.GetOrdinal("L3SuccessRate")) ? string.Empty : dr["L3SuccessRate"].ToString();
                            schoolQSRValue.LevelUnKnownLearningAim = dr.IsDBNull(dr.GetOrdinal("LUnkLearningAim")) ? string.Empty : dr["LUnkLearningAim"].ToString();
                            schoolQSRValue.LevelUnKnownSuccessRate = dr.IsDBNull(dr.GetOrdinal("LUnkSuccessRate")) ? string.Empty : dr["LUnkSuccessRate"].ToString();
                            schoolQSRValue.TotalLearningAim = dr.IsDBNull(dr.GetOrdinal("TotalLearningAim")) ? string.Empty : dr["TotalLearningAim"].ToString();
                            schoolQSRValue.TotalSuccessRate = dr.IsDBNull(dr.GetOrdinal("TotalSuccessRate")) ? string.Empty : dr["TotalSuccessRate"].ToString();
                            schoolQSRValue.Level1SuccessRateNational = dr.IsDBNull(dr.GetOrdinal("L1SuccessRate_Nat")) ? string.Empty : dr["L1SuccessRate_Nat"].ToString();
                            schoolQSRValue.Level2SuccessRateNational = dr.IsDBNull(dr.GetOrdinal("L2SuccessRate_Nat")) ? string.Empty : dr["L2SuccessRate_Nat"].ToString();
                            schoolQSRValue.Level3SuccessRateNational = dr.IsDBNull(dr.GetOrdinal("L3SuccessRate_Nat")) ? string.Empty : dr["L3SuccessRate_Nat"].ToString();
                            schoolQSRValue.LevelUnKnownSuccessRateNational = dr.IsDBNull(dr.GetOrdinal("LUnkSuccessRate_Nat")) ? string.Empty : dr["LUnkSuccessRate_Nat"].ToString();
                            schoolQSRValue.TotalSuccessRateNational = dr.IsDBNull(dr.GetOrdinal("TotalSuccessRate_Nat")) ? string.Empty : dr["TotalSuccessRate_Nat"].ToString();
                        }

                        jsonData = jsSerializer.Serialize(jsonResultQSRList);
                    }
                }
            }

            return jsonData;
        }

    }
}
