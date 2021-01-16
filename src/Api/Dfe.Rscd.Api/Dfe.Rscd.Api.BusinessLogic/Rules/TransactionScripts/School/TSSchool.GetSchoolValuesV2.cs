using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Script.Serialization;
using Web09.Services.Common.JSONObjects;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSSchool : Logic.TSBase
    {
        public static string GetSchoolValuesSummaryV2(string dfesNumber, string keystage, string context)
        {
            List<SchoolValue> schoolValues = new List<SchoolValue>();

            string schoolGroupName = string.Empty;
            string[] contextNameValuePairs = context.Split(',');
            foreach (string contextNamePair in contextNameValuePairs)
            {
                string[] nameValuePair = contextNamePair.Split('=');

                if ( nameValuePair.Length == 2 )
                {
                    if ( nameValuePair[0].Equals("SchoolGroupName", StringComparison.CurrentCultureIgnoreCase) )
                    {
                        schoolGroupName = nameValuePair[1];
                    }
                }
            }


            using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope())
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();
                    System.Data.Common.DbConnection connection = conn.StoreConnection;
                    System.Data.Common.DbCommand cmd = connection.CreateCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "School.GetSchoolValuesV2";


                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@DFESNumber",
                        SqlValue = dfesNumber
                    });

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.Int16,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@KeyStage",
                        SqlValue = keystage
                    });

                    if ( !string.IsNullOrEmpty(schoolGroupName) )
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@SchoolGroupName",
                            SqlValue = schoolGroupName
                        });
                    }
                    

                    System.Data.Common.DbDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        if ( dr.IsDBNull(dr.GetOrdinal("DateEnd") ) )
                        {
                            SchoolValue schoolValue = new SchoolValue();

                            if (!dr.IsDBNull(dr.GetOrdinal("Value")))
                            {
                                schoolValue.Value = dr["Value"].ToString();
                            }


                            if (!dr.IsDBNull(dr.GetOrdinal("IsHeader")))
                            {
                                schoolValue.IsHeader = bool.Parse(dr["IsHeader"].ToString());
                            }

                            if (!dr.IsDBNull(dr.GetOrdinal("ValueTypeCode")))
                            {
                                schoolValue.ValueCode = dr["ValueTypeCode"].ToString();
                            }

                            if (!dr.IsDBNull(dr.GetOrdinal("ValueTypeDescription")))
                            {
                                schoolValue.ValueDescription = dr["ValueTypeDescription"].ToString();
                            }

                            if (!dr.IsDBNull(dr.GetOrdinal("HeaderCode")))
                            {
                                schoolValue.HeaderCode = dr["HeaderCode"].ToString();
                            }

                            if (!dr.IsDBNull(dr.GetOrdinal("HeaderDescription")))
                            {
                                schoolValue.HeaderDescription = dr["HeaderDescription"].ToString();
                            }

                            schoolValues.Add(schoolValue);
                        }

                      
                    }
                }

                transaction.Complete();
            }

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            string json = jsSerializer.Serialize(schoolValues);

            return json;
        }
    }
}
