using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Globalization;
using System.Threading;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSSchoolDocuments : Logic.TSBase
    {
        public static List<SchoolDocumentsInformation> GetPilotSchoolDocuments(int? DFESNumber)
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
                        cmd.CommandText = "School.GetDocumentListForPilotSchool";


                        if (DFESNumber.HasValue)
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@DFESNumber",
                                SqlValue = DFESNumber.Value
                            });
                        }                        

                        System.Data.Common.DbDataReader dr = cmd.ExecuteReader();
                        List<SchoolDocumentsInformation> lst = new List<SchoolDocumentsInformation>();
                        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                        TextInfo textInfo = cultureInfo.TextInfo;
                        while (dr.Read())
                        {

                            SchoolDocumentsInformation ety = new SchoolDocumentsInformation
                            {
                                DocumentID = dr.IsDBNull(dr.GetOrdinal("DocumentID")) ? 0 : Int32.Parse(dr["DocumentID"].ToString()),
                                DocumentCode = dr.IsDBNull(dr.GetOrdinal("DocumentCode")) ? "" : dr["DocumentCode"].ToString(),
                                DocumentTitle = dr.IsDBNull(dr.GetOrdinal("DocumentTitle")) ? "" : dr["DocumentTitle"].ToString(),
                                DocumentTypeName = dr.IsDBNull(dr.GetOrdinal("DocumentTypeName")) ? "" : dr["DocumentTypeName"].ToString(),
                                hasSchoolData = dr.IsDBNull(dr.GetOrdinal("HasSchoolData")) ? false : Convert.ToBoolean(dr["HasSchoolData"].ToString()),
                                isPublic = dr.IsDBNull(dr.GetOrdinal("IsPublic")) ? false : Convert.ToBoolean(dr["IsPublic"].ToString()),
                                Keystage = dr.IsDBNull(dr.GetOrdinal("KeyStage")) ? Convert.ToInt16("0") : Convert.ToInt16(dr["KeyStage"].ToString()),
                                DocumentLocation = dr.IsDBNull(dr.GetOrdinal("DocumentLocation")) ? "" : dr["DocumentLocation"].ToString(),
                                ListOrder = dr.IsDBNull(dr.GetOrdinal("ListOrder")) ? 0 : Convert.ToInt32(dr["ListOrder"].ToString())
                            };

                            lst.Add(ety);
                        }

                        dr.Close();
                        return lst;
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
