using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Globalization;
using System.Web.Script.Serialization;
using Web09.Checking.DataAccess;
using Web09.Services.Common.JSONObjects;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSCohort : Logic.TSBase
    {       
     
        public static string GetCohortDetailsV2(int page, int rowsPerPage, int dcsfNumber, short? keyStageID, string forename, string surname, DateTime? dob, DateTime? doa, string gender, string inclusionStatus, string ethinicityCode, string firstLanguageCode, string yearGroupCode, string sortExpression, string aphabetForSurname, String strAge)
        {
            List<CohortDetails> jsonResultList = new List<CohortDetails>();            

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
                        cmd.CommandText = "Student.GetCohortDetails";

                        if (dcsfNumber > 0 || dcsfNumber == -1)
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@DCSFNumber",
                                SqlValue = dcsfNumber
                            });
                        }


                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@KeyStageNumber",
                            SqlValue = keyStageID
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@Page",
                            SqlValue = page
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@RowsPerPage",
                            SqlValue = rowsPerPage
                        });

                        if (!string.IsNullOrEmpty(forename))
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@Forename",
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
                        
                        if (!string.IsNullOrEmpty(aphabetForSurname))
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@SurnameFirstLetter",
                                SqlValue = aphabetForSurname
                            });
                        }

                        if (!string.IsNullOrEmpty(inclusionStatus))
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@InclusionStatus",
                                SqlValue = inclusionStatus
                            });
                        }


                        if (dob.HasValue)
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.DateTime,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@DateOfBirth",
                                SqlValue = dob.Value
                            });
                        }

                        if (doa.HasValue)
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.DateTime,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@AdmissionDate",
                                SqlValue = doa.Value
                            });
                        }

                        if (!string.IsNullOrEmpty(gender))
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@Gender",
                                SqlValue = gender
                            });
                        }

                        if (!string.IsNullOrEmpty(ethinicityCode))
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@EthnicityCode",
                                SqlValue = ethinicityCode
                            });
                        }

                        if (!string.IsNullOrEmpty(firstLanguageCode))
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@FirstLanguageCode",
                                SqlValue = firstLanguageCode
                            });
                        }

                        if (!string.IsNullOrEmpty(yearGroupCode))
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@YearGroupCode",
                                SqlValue = yearGroupCode
                            });
                        }

                        if (!string.IsNullOrEmpty(strAge))
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@Age",
                                SqlValue = strAge
                            });
                        }

                        int totalRows = 0;

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@TotalRows",
                            SqlValue = totalRows
                        });

                       
                        // Set the sortOrder
                        string sortOrder = string.Empty;
                        int sortAscending = 1;
                        if (!string.IsNullOrEmpty(sortExpression))
                        {
                            string[] sortExpressionElements = sortExpression.Split(' ');

                            if ( (sortExpressionElements.Length == 1) || (sortExpressionElements.Length == 2))
                            {
                                sortOrder = sortExpressionElements[0];
                            }

                            if (sortExpressionElements.Length == 2)
                            {
                                if (sortExpressionElements[1].Equals("desc", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    sortAscending = 0;
                                }
                            }

                            if (!string.IsNullOrEmpty(sortOrder))
                            {
                                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                                {
                                    DbType = System.Data.DbType.String,
                                    Direction = System.Data.ParameterDirection.Input,
                                    ParameterName = "@SortOrder",
                                    SqlValue = sortOrder
                                });

                                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                                {
                                    DbType = System.Data.DbType.Int32,
                                    Direction = System.Data.ParameterDirection.Input,
                                    ParameterName = "@SortAscending",
                                    SqlValue = sortAscending
                                });                               
                            }

                           
                        }                        
                    
                        System.Data.Common.DbDataReader dr = cmd.ExecuteReader();

                        List<String> lst = new List<String>();                      
                        while (dr.Read())
                        {                    
      

                            CohortDetails cohortDetails = new CohortDetails();
                            jsonResultList.Add(cohortDetails);

                            cohortDetails.TotalRecords                  = totalRows;
                           
                            cohortDetails.StudentID = Convert.ToInt32(dr["StudentID"]);
                            cohortDetails.ForvusNumber = dr.IsDBNull(dr.GetOrdinal("ForvusNumber")) ? 0 : Convert.ToInt32(dr["ForvusNumber"]);
                            cohortDetails.PINCLDisplayFlag = dr.IsDBNull(dr.GetOrdinal("PINCLDisplayFlag")) ? string.Empty : dr["PINCLDisplayFlag"].ToString();
                            cohortDetails.PINCLDescription = dr.IsDBNull(dr.GetOrdinal("PINCLDescription")) ? string.Empty : dr["PINCLDescription"].ToString();
                            cohortDetails.ScrutinyStatusCode = dr.IsDBNull(dr.GetOrdinal("ScrutinyStatusCode")) ? string.Empty : dr["ScrutinyStatusCode"].ToString();

                            cohortDetails.Surname = dr.IsDBNull(dr.GetOrdinal("Surname")) ? string.Empty : dr["Surname"].ToString();
                            cohortDetails.Forename = dr.IsDBNull(dr.GetOrdinal("Forename")) ? string.Empty : dr["Forename"].ToString();
                            cohortDetails.DOBDisplayString = dr.IsDBNull(dr.GetOrdinal("DOBDisplayString")) ? string.Empty : dr["DOBDisplayString"].ToString();
                            cohortDetails.Age = dr.IsDBNull(dr.GetOrdinal("Age")) ? 0 : Convert.ToInt32(dr["Age"]);
                            cohortDetails.AdmissionDateDisplayString = dr.IsDBNull(dr.GetOrdinal("AdmissionDateDisplayString")) ? string.Empty : dr["AdmissionDateDisplayString"].ToString();
                            cohortDetails.Gender = dr.IsDBNull(dr.GetOrdinal("Gender")) ? string.Empty : dr["Gender"].ToString();
                            cohortDetails.EthnicityCode = dr.IsDBNull(dr.GetOrdinal("EthnicityCode")) ? string.Empty : dr["EthnicityCode"].ToString();
                            cohortDetails.FirstLanguageCode = dr.IsDBNull(dr.GetOrdinal("FirstLanguageCode")) ? string.Empty : dr["FirstLanguageCode"].ToString();
                            cohortDetails.YearGroup = dr.IsDBNull(dr.GetOrdinal("YearGroup")) ? string.Empty : dr["YearGroup"].ToString();
                            cohortDetails.FreeSchoolMeals = dr.IsDBNull(dr.GetOrdinal("FreeSchoolMeals")) ? string.Empty : dr["FreeSchoolMeals"].ToString();
                            cohortDetails.InCare = dr.IsDBNull(dr.GetOrdinal("InCare")) ? string.Empty : dr["InCare"].ToString();
                            cohortDetails.SENStatusCode = dr.IsDBNull(dr.GetOrdinal("SENStatusCode")) ? string.Empty : dr["SENStatusCode"].ToString();
                            cohortDetails.KS1EXP = dr.IsDBNull(dr.GetOrdinal("KS1EXP")) ? string.Empty : dr["KS1EXP"].ToString();
                            cohortDetails.NewMobile = dr.IsDBNull(dr.GetOrdinal("NewMobile")) ? string.Empty : dr["NewMobile"].ToString();
                            cohortDetails.AdoptedFromCareCode = dr.IsDBNull(dr.GetOrdinal("AdoptedFromCareCode")) ? string.Empty : dr["AdoptedFromCareCode"].ToString();
                            cohortDetails.AdoptedFromCareDescription = dr.IsDBNull(dr.GetOrdinal("AdoptedFromCareDescription")) ? string.Empty : dr["AdoptedFromCareDescription"].ToString();

                            cohortDetails.OldSurname = dr.IsDBNull(dr.GetOrdinal("OldSurname")) ? string.Empty : dr["OldSurname"].ToString();
                            cohortDetails.OldForename = dr.IsDBNull(dr.GetOrdinal("OldForename")) ? string.Empty : dr["OldForename"].ToString();
                            cohortDetails.OldDOBDisplayString = dr.IsDBNull(dr.GetOrdinal("OldDOBDisplayString")) ? "" : dr["OldDOBDisplayString"].ToString();
                            cohortDetails.OldAge = dr.IsDBNull(dr.GetOrdinal("OldAge")) ? 0 : Convert.ToInt32(dr["OldAge"]);
                            cohortDetails.OldAdmissionDateDisplayString = dr.IsDBNull(dr.GetOrdinal("OldAdmissionDateDisplayString")) ? "" : dr["OldAdmissionDateDisplayString"].ToString();
                            cohortDetails.OldGender = dr.IsDBNull(dr.GetOrdinal("OldGender")) ? string.Empty : dr["OldGender"].ToString();
                            cohortDetails.OldEthnicityCode = dr.IsDBNull(dr.GetOrdinal("OldEthnicityCode")) ? string.Empty : dr["OldEthnicityCode"].ToString();
                            cohortDetails.OldFirstLanguageCode = dr.IsDBNull(dr.GetOrdinal("OldFirstLanguageCode")) ? string.Empty : dr["OldFirstLanguageCode"].ToString();
                            cohortDetails.OldYearGroup = dr.IsDBNull(dr.GetOrdinal("OldYearGroup")) ? string.Empty : dr["OldYearGroup"].ToString();
                            cohortDetails.OldFreeSchoolMeals = dr.IsDBNull(dr.GetOrdinal("OldFreeSchoolMeals")) ? string.Empty : dr["OldFreeSchoolMeals"].ToString();
                            cohortDetails.OldInCare = dr.IsDBNull(dr.GetOrdinal("OldInCare")) ? string.Empty : dr["OldInCare"].ToString();
                            cohortDetails.OldSENStatusCode = dr.IsDBNull(dr.GetOrdinal("OldSENStatusCode")) ? string.Empty : dr["OldSENStatusCode"].ToString();
                            cohortDetails.OldKS1EXP = dr.IsDBNull(dr.GetOrdinal("OldKS1EXP")) ? string.Empty : dr["OldKS1EXP"].ToString();
                            cohortDetails.OldNewMobile = dr.IsDBNull(dr.GetOrdinal("OldNewMobile")) ? string.Empty : dr["OldNewMobile"].ToString();
                            cohortDetails.OldAdoptedFromCareCode = dr.IsDBNull(dr.GetOrdinal("OldAdoptedFromCareCode")) ? string.Empty : dr["OldAdoptedFromCareCode"].ToString();
                            cohortDetails.OldAdoptedFromCareDescription = dr.IsDBNull(dr.GetOrdinal("OldAdoptedFromCareDescription")) ? string.Empty : dr["OldAdoptedFromCareDescription"].ToString();

                            cohortDetails.HasOldValues = dr.IsDBNull(dr.GetOrdinal("HasOldValues")) ? false : Convert.ToBoolean(dr["HasOldValues"]);
                            cohortDetails.HasResultAmendments = dr.IsDBNull(dr.GetOrdinal("HasResultAmendments")) ? false : Convert.ToBoolean(dr["HasResultAmendments"]);

                            // Transform date strings to dd/mm/yyyy format
                            if (!string.IsNullOrEmpty(cohortDetails.DOBDisplayString))
                            {
                                DateTime dobDate;
                                if (DateTime.TryParseExact(cohortDetails.DOBDisplayString, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dobDate))
                                {
                                    cohortDetails.DOBDisplayString = dobDate.Day.ToString("00") + "/" + dobDate.Month.ToString("00") + "/" + dobDate.Year.ToString();
                                }
                                else
                                {
                                    cohortDetails.DOBDisplayString = string.Empty;
                                }                                
                            }
                            if (!string.IsNullOrEmpty(cohortDetails.AdmissionDateDisplayString))
                            {
                                DateTime admDate;
                                if (DateTime.TryParseExact(cohortDetails.AdmissionDateDisplayString, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out admDate))
                                {
                                    cohortDetails.AdmissionDateDisplayString = admDate.Day.ToString("00") + "/" + admDate.Month.ToString("00") + "/" + admDate.Year.ToString();
                                }
                                else
                                {
                                    cohortDetails.AdmissionDateDisplayString = string.Empty;
                                }                                                 
                            }
                            if (!string.IsNullOrEmpty(cohortDetails.DOBDisplayString))
                            {
                                DateTime dobDate;
                                if (DateTime.TryParseExact(cohortDetails.OldDOBDisplayString, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dobDate))
                                {
                                    cohortDetails.OldDOBDisplayString = dobDate.Day.ToString("00") + "/" + dobDate.Month.ToString("00") + "/" + dobDate.Year.ToString();
                                }
                                else
                                {
                                    cohortDetails.OldDOBDisplayString = string.Empty;
                                }
                            }
                            if (!string.IsNullOrEmpty(cohortDetails.AdmissionDateDisplayString))
                            {
                                DateTime admDate;
                                if (DateTime.TryParseExact(cohortDetails.OldAdmissionDateDisplayString, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out admDate))
                                {
                                    cohortDetails.OldAdmissionDateDisplayString = admDate.Day.ToString("00") + "/" + admDate.Month.ToString("00") + "/" + admDate.Year.ToString();
                                }
                                else
                                {
                                    cohortDetails.OldAdmissionDateDisplayString = string.Empty;
                                }
                            }
                            

                            // Captialise Forename and Surname
                            TextInfo myTI = new CultureInfo("en-GB", false).TextInfo;
                            cohortDetails.Forename = myTI.ToTitleCase(myTI.ToLower(cohortDetails.Forename));
                            cohortDetails.Surname = myTI.ToTitleCase(myTI.ToLower(cohortDetails.Surname));
                            cohortDetails.OldForename = myTI.ToTitleCase(myTI.ToLower(cohortDetails.OldForename));
                            cohortDetails.OldSurname = myTI.ToTitleCase(myTI.ToLower(cohortDetails.OldSurname));                          
                        }

                        dr.Close();

                       
                        totalRows = Convert.ToInt32(cmd.Parameters["@TotalRows"].Value);
                        foreach(CohortDetails detail in jsonResultList)
                        {
                            detail.TotalRecords = totalRows;
                        }
                        
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
