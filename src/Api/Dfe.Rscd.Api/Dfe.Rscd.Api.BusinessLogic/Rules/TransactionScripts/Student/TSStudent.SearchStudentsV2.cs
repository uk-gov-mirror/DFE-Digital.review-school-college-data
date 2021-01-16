using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Globalization;
using System.Web.Script.Serialization;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent : Logic.TSBase
    {

        public static string SearchStudentsV2(int dfesNumber, int keyStage, string forename, string surname, DateTime dob, string gender)
        {
            List<Web09.Services.Common.JSONObjects.PupilSearchResult> searchResults = new List<Web09.Services.Common.JSONObjects.PupilSearchResult>();

            // TODO call proc and load results

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
                        cmd.CommandText = "Student.SearchStudents";

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@DfesNumber",
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
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@Forename",
                            SqlValue = forename
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@Surname",
                            SqlValue = surname
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.DateTime,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@DateOfBirth",
                            SqlValue = dob
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@Gender",
                            SqlValue = gender
                        });
                      
                        System.Data.Common.DbDataReader dr = cmd.ExecuteReader();
                       
                        while (dr.Read())
                        {
                            Web09.Services.Common.JSONObjects.PupilSearchResult searchResult = new Web09.Services.Common.JSONObjects.PupilSearchResult();

                            searchResult.DFESNumber            = Convert.ToInt32(dr["DFESNumber"]);
                            searchResult.KeyStage              = Convert.ToInt32(dr["KeyStage"]);
                            searchResult.SchoolName            = dr.IsDBNull(dr.GetOrdinal("SchoolName")) ? string.Empty : Convert.ToString(dr["SchoolName"]);
                            searchResult.StudentID             = Convert.ToInt32(dr["StudentID"]);
                            searchResult.ChangeID              = Convert.ToInt32(dr["ChangeID"]);
                            searchResult.PINCLCode             = dr.IsDBNull(dr.GetOrdinal("PINCLCode")) ? string.Empty : Convert.ToString(dr["PINCLCode"]);
                            searchResult.PINCLDisplayFlag      = dr.IsDBNull(dr.GetOrdinal("PINCLDisplayFlag")) ? string.Empty : Convert.ToString(dr["PINCLDisplayFlag"]);
                            searchResult.PINCLDescription      = dr.IsDBNull(dr.GetOrdinal("PINCLDescription")) ? string.Empty : Convert.ToString(dr["PINCLDescription"]);
                            searchResult.ScrutinyRequestStatus = dr.IsDBNull(dr.GetOrdinal("ScrutinyRequestStatus")) ? string.Empty : Convert.ToString(dr["ScrutinyRequestStatus"]);
                            searchResult.Surname               = dr.IsDBNull(dr.GetOrdinal("Surname")) ? string.Empty : Convert.ToString(dr["Surname"]);
                            searchResult.Forename              = dr.IsDBNull(dr.GetOrdinal("Forename")) ? string.Empty : Convert.ToString(dr["Forename"]);
                            searchResult.DOB                   = dr.IsDBNull(dr.GetOrdinal("DOB")) ? string.Empty : Convert.ToString(dr["DOB"]);
                            searchResult.Age                   = dr.IsDBNull(dr.GetOrdinal("Age")) ? 0 : Convert.ToInt32(dr["Age"]);
                            searchResult.AdmissionDate         = dr.IsDBNull(dr.GetOrdinal("ENTRYDAT")) ? string.Empty : Convert.ToString(dr["ENTRYDAT"]);
                            searchResult.Gender                = dr.IsDBNull(dr.GetOrdinal("Gender")) ? string.Empty : Convert.ToString(dr["Gender"]);
                            searchResult.EthnicityCode         = dr.IsDBNull(dr.GetOrdinal("EthnicityCode")) ? string.Empty : Convert.ToString(dr["EthnicityCode"]);
                            searchResult.FirstLanguageCode     = dr.IsDBNull(dr.GetOrdinal("FirstLanguageCode")) ? string.Empty : Convert.ToString(dr["FirstLanguageCode"]);
                            searchResult.YearGroup             = dr.IsDBNull(dr.GetOrdinal("ActualYearGroup")) ? 0 : Convert.ToInt32(dr["ActualYearGroup"]);

                            // Transform date strings to dd/mm/yyyy format
                            if (!string.IsNullOrEmpty(searchResult.DOB))
                            {
                                DateTime dobDate;
                                if (DateTime.TryParseExact(searchResult.DOB, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dobDate))
                                {
                                    searchResult.DOB = dobDate.Day.ToString("00") + "/" + dobDate.Month.ToString("00") + "/" + dobDate.Year.ToString();
                                }
                                else
                                {
                                    searchResult.DOB = string.Empty;
                                }
                            }
                            if (!string.IsNullOrEmpty(searchResult.AdmissionDate))
                            {
                                DateTime admissionDate;
                                if (DateTime.TryParseExact(searchResult.AdmissionDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out admissionDate))
                                {
                                    searchResult.AdmissionDate = admissionDate.Day.ToString("00") + "/" + admissionDate.Month.ToString("00") + "/" + admissionDate.Year.ToString();
                                }
                                else
                                {
                                    searchResult.DOB = string.Empty;
                                }
                            }

                            searchResults.Add(searchResult);
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
            string jsonData = jsSerializer.Serialize(searchResults);

            return jsonData;
        }

    }
}
