using System;
using System.Data.EntityClient;
using System.Web.Script.Serialization;
using Web09.Checking.DataAccess;
using Web09.Services.Common.JSONObjects;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {

        // TFS 21014      
        public static string ScanBarcodeV2(String barCode, string username, string forename, string surname, string rolename)
        {
            ScanBarCodeResult scanBarCodeResult = new ScanBarCodeResult();

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
                        cmd.CommandText = "Result.ScanBarCode";

                        if (!string.IsNullOrEmpty(barCode))
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@BarCode",
                                SqlValue = barCode
                            });
                        }

                        if (!string.IsNullOrEmpty(username))
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@UserName",
                                SqlValue = username
                            });
                        }

                        if (!string.IsNullOrEmpty(forename))
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@ForeName",
                                SqlValue = forename
                            });
                        }

                        if (!string.IsNullOrEmpty(surname))
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@Surname",
                                SqlValue = surname
                            });
                        }

                        if (!string.IsNullOrEmpty(rolename))
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@Rolename",
                                SqlValue = rolename
                            });
                        }
                      
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Byte,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@BarCodeExists"
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Byte,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@BarCodeRequestCancelled"
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.DateTime,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@BarCodeRequestCancelledDate"
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@BarCodeRequestCancelledBy",
                            Size = 255
                        });
                        
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@ResultID"
                        });


                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@CurrentRequestResultRequestID"
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@CurrentRequestEvidenceID"
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@CurrentRequestBarCode",
                            Size = 16
                        });

                        cmd.ExecuteNonQuery();                         

                        scanBarCodeResult.BarCodeExists           = Convert.ToBoolean(cmd.Parameters["@BarCodeExists"].Value);
                        scanBarCodeResult.BarCodeRequestCancelled = Convert.ToBoolean(cmd.Parameters["@BarCodeRequestCancelled"].Value);
                      
                        if ( scanBarCodeResult.BarCodeRequestCancelled )
                        {
                            scanBarCodeResult.BarCodeRequestCancelledDate       = Convert.ToDateTime(cmd.Parameters["@BarCodeRequestCancelledDate"].Value);
                            scanBarCodeResult.BarCodeRequestCancelledBy         = Convert.ToString(cmd.Parameters["@BarCodeRequestCancelledBy"].Value);
                            scanBarCodeResult.ResultID                          = Convert.ToInt32(cmd.Parameters["@ResultID"].Value);
                                                 
                            if (cmd.Parameters["@CurrentRequestResultRequestID"].Value is DBNull)
                            {
                                scanBarCodeResult.CurrentRequestResultRequestID = 0;
                            }
                            else
                            {
                                scanBarCodeResult.CurrentRequestResultRequestID = Convert.ToInt32(cmd.Parameters["@CurrentRequestResultRequestID"].Value);
                            }

                            if (cmd.Parameters["@CurrentRequestEvidenceID"].Value is DBNull)
                            {
                                scanBarCodeResult.CurrentRequestEvidenceID = 0;
                            }
                            else
                            {
                                scanBarCodeResult.CurrentRequestEvidenceID = Convert.ToInt32(cmd.Parameters["@CurrentRequestEvidenceID"].Value);
                            }

                            if (cmd.Parameters["@CurrentRequestBarCode"].Value is DBNull)
                            {
                                scanBarCodeResult.CurrentRequestBarCode = string.Empty;
                            }
                            else
                            {
                                scanBarCodeResult.CurrentRequestBarCode = Convert.ToString(cmd.Parameters["@CurrentRequestBarCode"].Value);
                            }                          
                                                       
                        }
                       
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            string jsonData = jsSerializer.Serialize(scanBarCodeResult);           

            return jsonData;        
        }

    }
}
