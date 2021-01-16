using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Globalization;
using System.Web.Script.Serialization;
using Web09.Checking.DataAccess;
using Web09.Services.Common.JSONObjects;


namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
        public static string GetLateResultsV2(int dcsfNumber, int keyStage, int page, int rowsPerPage, string sortOrder, int sortAscending, out int totalRows)
        {
            List<LateResultListItem> jsonResultList = new List<LateResultListItem>();

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
                        cmd.CommandText = "Result.GetLateResultsList";

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
                            ParameterName = "@KeyStage",
                            SqlValue = keyStage
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

                        totalRows = 0;
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@TotalRows",
                            SqlValue = totalRows
                        });

                        System.Data.Common.DbDataReader dr = cmd.ExecuteReader();

                        List<String> lst = new List<String>();
                        while (dr.Read())
                        {
                            LateResultListItem listItem = new LateResultListItem();
                            jsonResultList.Add(listItem);

                            listItem.ResultID = dr.IsDBNull(dr.GetOrdinal("ResultID")) ? 0 : Convert.ToInt32(dr["ResultID"]);
                            listItem.ResultCurrentChangeID = dr.IsDBNull(dr.GetOrdinal("ResultCurrentChangeID")) ? 0 : Convert.ToInt32(dr["ResultCurrentChangeID"]);
                            listItem.ResultKeyStage = dr.IsDBNull(dr.GetOrdinal("ResultKeyStage")) ? string.Empty : Convert.ToString(dr["ResultKeyStage"]);
                            listItem.CurrentPointID = dr.IsDBNull(dr.GetOrdinal("CurrentPointID")) ? 0 : Convert.ToInt32(dr["CurrentPointID"]);
                            listItem.ResultStatus = dr.IsDBNull(dr.GetOrdinal("ResultStatus")) ? string.Empty : Convert.ToString(dr["ResultStatus"]);
                            listItem.ForvusID = dr.IsDBNull(dr.GetOrdinal("ForvusID")) ? 0 : Convert.ToInt32(dr["ForvusID"]);
                            listItem.Forename = dr.IsDBNull(dr.GetOrdinal("Forename")) ? string.Empty : Convert.ToString(dr["Forename"]);
                            listItem.Surname = dr.IsDBNull(dr.GetOrdinal("Surname")) ? string.Empty : Convert.ToString(dr["Surname"]);
                            listItem.DateOfBirth = dr.IsDBNull(dr.GetOrdinal("DateOfBirth")) ? string.Empty : Convert.ToString(dr["DateOfBirth"]);
                            listItem.SeasonCode = dr.IsDBNull(dr.GetOrdinal("SeasonCode")) ? string.Empty : Convert.ToString(dr["SeasonCode"]);
                            listItem.ExamYear = dr.IsDBNull(dr.GetOrdinal("ExamYear")) ? 0 : Convert.ToInt32(dr["ExamYear"]);
                            listItem.Qualification = dr.IsDBNull(dr.GetOrdinal("Qualification")) ? string.Empty : Convert.ToString(dr["Qualification"]);
                            listItem.AwardingOrganisation = dr.IsDBNull(dr.GetOrdinal("AwardingOrganisation")) ? string.Empty : Convert.ToString(dr["AwardingOrganisation"]);
                            listItem.QAN = dr.IsDBNull(dr.GetOrdinal("QAN")) ? string.Empty : Convert.ToString(dr["QAN"]);
                            listItem.Title = dr.IsDBNull(dr.GetOrdinal("Title")) ? string.Empty : Convert.ToString(dr["Title"]);
                            listItem.CurrentGrade = dr.IsDBNull(dr.GetOrdinal("CurrentGrade")) ? string.Empty : Convert.ToString(dr["CurrentGrade"]);
                            listItem.CurrentMark = dr.IsDBNull(dr.GetOrdinal("CurrentMark")) ? string.Empty : Convert.ToString(dr["CurrentMark"]);
                            listItem.PreviousGrade = dr.IsDBNull(dr.GetOrdinal("PreviousGrade")) ? string.Empty : Convert.ToString(dr["PreviousGrade"]);
                            listItem.PreviousMark = dr.IsDBNull(dr.GetOrdinal("PreviousMark")) ? string.Empty : Convert.ToString(dr["PreviousMark"]);

                            listItem.DateLoaded = dr.IsDBNull(dr.GetOrdinal("DateLoaded")) ? string.Empty : Convert.ToString(dr["DateLoaded"]);
                            listItem.DateLastUpdated = dr.IsDBNull(dr.GetOrdinal("LastUpdated")) ? string.Empty : Convert.ToDateTime(dr["LastUpdated"]).ToString("yyyyMMdd HH:mm:ss");
                            listItem.R_INCL_DisplayFlag = dr.IsDBNull(dr.GetOrdinal("R_INCL_DisplayFlag")) ? string.Empty : Convert.ToString(dr["R_INCL_DisplayFlag"]);
                            listItem.R_INCL_Description = dr.IsDBNull(dr.GetOrdinal("R_INCL_Description")) ? string.Empty : Convert.ToString(dr["R_INCL_Description"]);

                            listItem.Exam_Date = dr.IsDBNull(dr.GetOrdinal("Exam_Date")) ? string.Empty : Convert.ToDateTime(dr["Exam_Date"]).ToString("dd/MM/yyyy");
                            if (listItem.Exam_Date.Equals("01/01/1900"))
                            {
                                listItem.Exam_Date = string.Empty;
                            }

                            // Captialise Forename and Surname
                            TextInfo myTI = new CultureInfo("en-GB", false).TextInfo;
                            listItem.Forename = myTI.ToTitleCase(myTI.ToLower(listItem.Forename));
                            listItem.Surname = myTI.ToTitleCase(myTI.ToLower(listItem.Surname));
                        }

                        dr.Close();
                        totalRows = Convert.ToInt32(cmd.Parameters["@TotalRows"].Value);
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
