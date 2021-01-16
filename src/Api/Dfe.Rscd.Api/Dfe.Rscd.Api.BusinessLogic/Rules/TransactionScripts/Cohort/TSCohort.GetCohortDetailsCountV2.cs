using System;
using System.Data.EntityClient;

using Web09.Checking.DataAccess;


namespace Web09.Checking.Business.Logic.TransactionScripts
{
	 public partial class TSCohort : Logic.TSBase
	 {
                 
	   public static int GetCohortDetailsCountV2(int dcsfNumber, short? keyStageID, string forename, string surname, DateTime? dob, DateTime? doa, string gender, string inclusionStatus, string ethinicityCode, string firstLanguageCode, string yearGroupCode, string aphabetForSurname, String strAge)
        {
            int rowCount = 0;

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
                            ParameterName = "@RowsPerPage",
                            SqlValue = 0
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

                       cmd.ExecuteNonQuery();
                       rowCount = Convert.ToInt32(cmd.Parameters["@TotalRows"].Value);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rowCount;
        }
	 }
}
