using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Web.Script.Serialization;
using Web09.Checking.DataAccess;
using Web09.Services.Common.JSONObjects;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {

        public static string GetQualificationsV2(string QAN, string syllabus, string awardingbodyName, int? year, short keystage)
        {
            string qualificationsListJSON = DoGetQualificationsV2(QAN, syllabus, awardingbodyName, year, keystage);

            return qualificationsListJSON;
        }

        private static string DoGetQualificationsV2(string QAN, string syllabus, string awardingbodyName, int? year, short keystage)
        {
            List<Qualification> jsonResultList = new List<Web09.Services.Common.JSONObjects.Qualification>();

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
                        cmd.CommandText = "Result.GetQualifications";

                        if (!string.IsNullOrEmpty(QAN))
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@QAN",
                                SqlValue = QAN
                            });
                        }

                        if (!string.IsNullOrEmpty(syllabus))
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@Syllabus",
                                SqlValue = syllabus
                            });
                        }

                        if (!string.IsNullOrEmpty(awardingbodyName))
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@AwardingBodyName",
                                SqlValue = awardingbodyName
                            });
                        }

                        if (year.HasValue)
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@Year",
                                SqlValue = year.Value
                            });
                        }

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@KeyStage",
                            SqlValue = keystage
                        });

                        System.Data.Common.DbDataReader dr = cmd.ExecuteReader();

                        List<String> lst = new List<String>();
                        while (dr.Read())
                        {
                            Qualification qualificationItem = new Web09.Services.Common.JSONObjects.Qualification();
                            jsonResultList.Add(qualificationItem);

                            qualificationItem.QualificationID   = dr.IsDBNull(dr.GetOrdinal("QualificationID")) ? 0 : Convert.ToInt32(dr["QualificationID"]);
                            qualificationItem.QualificationCode = dr.IsDBNull(dr.GetOrdinal("QualificationCode")) ? string.Empty : Convert.ToString(dr["QualificationCode"]);
                            qualificationItem.QualificationName = dr.IsDBNull(dr.GetOrdinal("QualificationName")) ? string.Empty : Convert.ToString(dr["QualificationName"]);
                            qualificationItem.APR15             = dr.IsDBNull(dr.GetOrdinal("APR15")) ? 0 : Convert.ToInt32(dr["APR15"]);
                            qualificationItem.APR1618           = dr.IsDBNull(dr.GetOrdinal("APR1618")) ? 0 : Convert.ToInt32(dr["APR1618"]);
                            qualificationItem.KS4Main           = dr.IsDBNull(dr.GetOrdinal("KS4Main")) ? false : Convert.ToBoolean(dr["KS4Main"]);
                            qualificationItem.KS5Main           = dr.IsDBNull(dr.GetOrdinal("KS5Main")) ? false : Convert.ToBoolean(dr["KS5Main"]);
                            qualificationItem.YearStart         = dr.IsDBNull(dr.GetOrdinal("YearStart")) ? 0 : Convert.ToInt32(dr["YearStart"]);
                            qualificationItem.YearDeleted       = dr.IsDBNull(dr.GetOrdinal("YearDeleted")) ? 0 : Convert.ToInt32(dr["YearDeleted"]);
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
