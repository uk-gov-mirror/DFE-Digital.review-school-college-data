using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent : Logic.TSBase
    {
        // TODO
        public static string GetStudentV2(int studentId, string requestContext)
        {
            Web09.Services.Common.JSONObjects.Pupil pupil = new Web09.Services.Common.JSONObjects.Pupil();


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
                        cmd.CommandText = "Student.GetPupil";

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@PupilID",
                            SqlValue = studentId
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@Context",
                            SqlValue = requestContext
                        });

                        System.Data.Common.DbDataReader dr = cmd.ExecuteReader();
                                              
                        if (dr.Read())
                        {
                            pupil.PupilID                       = dr.IsDBNull(dr.GetOrdinal("StudentID")) ? 0 : Convert.ToInt32(dr["StudentID"]);

                            pupil.Forename                      = dr.IsDBNull(dr.GetOrdinal("Forename")) ? string.Empty : Convert.ToString(dr["Forename"]);
                            pupil.Surname                       = dr.IsDBNull(dr.GetOrdinal("Surname")) ? string.Empty : Convert.ToString(dr["Surname"]);
                            pupil.ForvusNumber                  = dr.IsDBNull(dr.GetOrdinal("ForvusNumber")) ? 0 : Convert.ToInt32(dr["ForvusNumber"]);
                            pupil.Gender                        = dr.IsDBNull(dr.GetOrdinal("Gender")) ? string.Empty : Convert.ToString(dr["Gender"]);
                            pupil.KeyStage                      = dr.IsDBNull(dr.GetOrdinal("KeyStage")) ? 0 : Convert.ToInt32(dr["KeyStage"]);
                            pupil.PostCode                      = dr.IsDBNull(dr.GetOrdinal("PostCode")) ? string.Empty : Convert.ToString(dr["PostCode"]);
                            pupil.SchoolName                    = dr.IsDBNull(dr.GetOrdinal("SchoolName")) ? string.Empty : Convert.ToString(dr["SchoolName"]);
                            pupil.SchoolDfesNumber              = dr.IsDBNull(dr.GetOrdinal("SchoolDfesNumber")) ? 0 : Convert.ToInt32(dr["SchoolDfesNumber"]);
                            pupil.PINCLCode                     = dr.IsDBNull(dr.GetOrdinal("PINCLCode")) ? string.Empty : Convert.ToString(dr["PINCLCode"]);
                            pupil.PINCDescription               = dr.IsDBNull(dr.GetOrdinal("PINCLDescription")) ? string.Empty : Convert.ToString(dr["PINCLDescription"]);

                            pupil.DateOfBirth                   = dr.IsDBNull(dr.GetOrdinal("DateOfBirth")) ? DateTime.MinValue : Convert.ToDateTime(dr["DateOfBirth"]);
                            pupil.EthnicityCode                 = dr.IsDBNull(dr.GetOrdinal("EthnicityCode")) ? string.Empty : Convert.ToString("EthnicityCode");
                            pupil.FirstLanguageCode             = dr.IsDBNull(dr.GetOrdinal("FirstLanguageCode")) ? string.Empty : Convert.ToString("FirstLanguageCode");
                            pupil.UPN                           = dr.IsDBNull(dr.GetOrdinal("UPN")) ? string.Empty : Convert.ToString(dr["UPN"]);
                            pupil.PINCLDisplayFlag              = dr.IsDBNull(dr.GetOrdinal("PINCLDisplayFlag")) ? string.Empty : Convert.ToString("PINCLDisplayFlag");

                            pupil.AdoptedFromCareCode           = dr.IsDBNull(dr.GetOrdinal("AdoptedFromCareCode")) ? string.Empty : Convert.ToString(dr["AdoptedFromCareCode"]);
                            pupil.AdoptedFromCareDescription    = dr.IsDBNull(dr.GetOrdinal("AdoptedFromCareDescription")) ? string.Empty : Convert.ToString(dr["AdoptedFromCareDescription"]);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            string jsonData = jsSerializer.Serialize(pupil);

            return jsonData;
        }
    }
}
