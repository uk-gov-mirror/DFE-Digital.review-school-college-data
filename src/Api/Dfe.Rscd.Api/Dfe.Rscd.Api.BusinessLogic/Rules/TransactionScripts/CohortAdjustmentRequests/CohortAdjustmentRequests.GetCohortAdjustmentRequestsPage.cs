using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Globalization;
using System.Text;
using System.Threading;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSCohortAdjustmentRequests: Logic.TSBase
    {
        public static FinalCohortAdjustmentOutput GetCohortAdjustmentRequestsPage(
            int page, 
            int rowsPerPage, 
            string requestStatus, 
            short keyStageID, 
            int dcsfNumber, 
            int forvusIndex, 
            string surname, 
            string scrutinyStatus, 
            string yearGroupCode, 
            Int16 schoolGroupID, 
            string updatedBy, 
            DateTime? updatedAfter, 
            String currentUserName, 
            String sortExpression,
            bool IsJuneDecisionsRequest)
        {
            FinalCohortAdjustmentOutput output = new FinalCohortAdjustmentOutput();

            try
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        String strColumn = "";
                        String strDirection = "";
                        //RequestID
                        if (sortExpression.Contains("RequestID"))
                        {
                            strColumn = "RequestID";
                            if (sortExpression.Contains("ASC"))
                                strDirection="ASC";
                            else
                                strDirection="DESC";
                        }
                        //DCSFNo
                        if (sortExpression.Contains("DCSFNo"))
                        {
                            strColumn = "DCSFNo";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }
                        //ForvusNo
                        if (sortExpression.Contains("ForvusNo"))
                        {
                            strColumn = "ForvusNo";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }
                        //RequestType
                        if (sortExpression.Contains("RequestType") || sortExpression.Contains("RequestReason"))
                        {
                            strColumn = "RequestType";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }
                        //ForvusUpdate
                        if (sortExpression.Contains("ForvusUpdate"))
                        {
                            strColumn = "ForvusUpdate";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }
                        //ForvusInitials
                        if (sortExpression.Contains("ForvusInitials"))
                        {
                            strColumn = "ForvusInitials";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }
                        //DCSFUpdate
                        if (sortExpression.Contains("DCSFUpdate"))
                        {
                            strColumn = "DCSFUpdate";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }
                        //Decision
                        if (sortExpression.Contains("Decision") || sortExpression.Contains("CohortDecision"))
                        {
                            strColumn = "Decision";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("Surname"))
                        {
                            strColumn = "Surname";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("Forename"))
                        {
                            strColumn = "Forename";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("Forename"))
                        {
                            strColumn = "Forename";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("DOB"))
                        {
                            strColumn = "Forename";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("CohortNotes"))
                        {
                            strColumn = "CohortNotes";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        System.Data.Common.DbConnection connection = conn.StoreConnection;

                        System.Data.Common.DbCommand cmd = connection.CreateCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Scrutiny.CohortAdjustmentRequest_GetPage";

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@KeyStage",
                            SqlValue = keyStageID
                        });

                        if (dcsfNumber > 0 || dcsfNumber==-1)
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@DCSFNumber",
                            SqlValue = dcsfNumber
                        });

                        if (forvusIndex > 0 || forvusIndex==-1)
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@ForvusIndex",
                                SqlValue = forvusIndex
                            });

                        if (!string.IsNullOrEmpty(surname))
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@Surname",
                                SqlValue = surname
                            });

                        if(!string.IsNullOrEmpty(scrutinyStatus))
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@ScrutinyStatus",
                                SqlValue = scrutinyStatus
                            });


                        if (!string.IsNullOrEmpty(yearGroupCode))
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@YearGroupCode",
                                SqlValue = yearGroupCode
                            });

                        if (!string.IsNullOrEmpty(requestStatus))
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@RequestStatus",
                                SqlValue = Int32.Parse(requestStatus)
                            });

                        if (!string.IsNullOrEmpty(updatedBy))
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@UpdatedBy",
                                SqlValue = updatedBy
                            });

                        if (updatedAfter.HasValue && updatedAfter.Value > (new DateTime(1800, 1, 1)))
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.DateTime,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@UpdatedAfter",
                                SqlValue = updatedAfter.Value
                            });

                        if(schoolGroupID>0)
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@SchoolGroupID",
                                SqlValue = schoolGroupID
                            });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@SortExpression",
                                SqlValue = strColumn
                            });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@SortDirection",
                            SqlValue = strDirection
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Boolean,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@IsJuneCohortRequestDecisions",
                            SqlValue = IsJuneDecisionsRequest
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@UserName",
                            SqlValue = currentUserName
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@FromRow",
                            SqlValue = (page - 1) * rowsPerPage
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@ToRow",
                            SqlValue = (((page - 1) * rowsPerPage)+rowsPerPage)
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@RowCount"
                        });

                        System.Data.Common.DbDataReader dr = cmd.ExecuteReader();                        

                        List<CohortAdjustmentRequestResult> lst = new List<CohortAdjustmentRequestResult>();
                        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                        TextInfo textInfo = cultureInfo.TextInfo;
                        while (dr.Read())
                        {
                            
                               
                            CohortAdjustmentRequestResult obj= (new CohortAdjustmentRequestResult
                                    {
                                        DCSFNo = int.Parse(dr["DFESNumber"].ToString())
                                        ,
                                        ForvusIndex = dr.IsDBNull(dr.GetOrdinal("ForvusIndex")) ? "" : dr["ForvusIndex"].ToString()
                                        ,
                                        Surname = dr.IsDBNull(dr.GetOrdinal("Surname")) ? "" : textInfo.ToTitleCase(textInfo.ToLower(dr["Surname"].ToString()))
                                        ,
                                        RequestID = int.Parse(dr["RequestID"].ToString())
                                        ,
                                        RequestType = Int16.Parse(dr["RequestType"].ToString())
                                        ,
                                        RequestTypeToDisplay = dr["ReasonText"].ToString()
                                        ,
                                        Decision = dr.IsDBNull(dr.GetOrdinal("ScrutinyStatusDescription")) ? "" : dr["ScrutinyStatusDescription"].ToString()
                                        ,
                                        ForvusUpdate = dr.IsDBNull(dr.GetOrdinal("chgForvusForename")) ? "" : dr["chgForvusForename"].ToString() + " " + dr["chgForvusSurname"].ToString()
                                        ,
                                        ForvusUpdateDate = dr.IsDBNull(dr.GetOrdinal("chgForvusDate")) ? "" : DateTime.Parse(dr["chgForvusDate"].ToString()).ToString("dd-MM-yyyy hh:mm tt")
                                        ,
                                        DCSFUpdate = dr.IsDBNull(dr.GetOrdinal("chgDCSFDate")) ? "" : DateTime.Parse(dr["chgDCSFDate"].ToString()).ToString("dd-MM-yyyy hh:mm tt")
                                        ,
                                        DOB = dr.IsDBNull(dr.GetOrdinal("DOB")) ? "" : dr["DOB"].ToString()
                                        ,
                                        Forename = dr.IsDBNull(dr.GetOrdinal("Forename")) ? "" : textInfo.ToTitleCase(textInfo.ToLower(dr["Forename"].ToString()))
                                        ,
                                        Gender = dr.IsDBNull(dr.GetOrdinal("Gender")) ? "" : textInfo.ToTitleCase(textInfo.ToLower(dr["Gender"].ToString()))
                                        ,
                                        Notes = dr.IsDBNull(dr.GetOrdinal("DCSFNotification")) ? "" : dr["DCSFNotification"].ToString().Trim()
                                        ,
                                        RequestReason= dr["ReasonText"].ToString()
                                        ,
                                        KeyStage = Int16.Parse(dr["KeyStage"].ToString())
                                    }
                                );
                               
                            // check for automatic referrels to DCSF
                            String originalSCrutinyStatusCode= "";
                            if(!dr.IsDBNull(dr.GetOrdinal("OriginalScrutinyStatusCode")))                           
                                originalSCrutinyStatusCode= dr["OriginalScrutinyStatusCode"].ToString();

                            if (obj.ForvusUpdate == "" && originalSCrutinyStatusCode == "PD")
                            {
                                // Set auto referral
                                obj.ForvusUpdate = "Automatic referral";
                                // change date to request generation date
                                obj.ForvusUpdateDate = DateTime.Parse(dr["chgRequestDate"].ToString()).ToString("dd-MM-yyyy hh:mm tt");
                            }                            
                            lst.Add(obj);
                        }
                        
                        dr.Close();

                        int cnt = 0;

                        if (int.TryParse(cmd.Parameters["@RowCount"].Value.ToString(), out cnt))
                            output.TotalRowCount = cnt;
                        else
                            output.TotalRowCount = 0;                        

                        output.adjustmentList = lst;
                        
                        return output;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static String RemoveControlCharaters(String value)
        {
            StringBuilder sb = new StringBuilder();

            for (int index = 0; index < value.Length - 1; index++)
            {
                if( ! char.IsControl(value, index))
                    sb.Append(value.Substring(index,1));
                else if(!char.IsSymbol(value.Substring(index,1),index))
                    sb.Append(value.Substring(index, 1));
            }

            return sb.ToString();
        }

        public static FinalCohortAdjustmentOutput GetCohortDecisionPage(
            int page,
            int rowsPerPage,
            string requestStatus,
            short keyStageID,
            int dcsfNumber,
            int forvusIndex,
            string surname,
            string scrutinyStatus,
            string yearGroupCode,
            Int16 schoolGroupID,
            string updatedBy,
            DateTime? updatedAfter,
            String currentUserName,
            String sortExpression,
            bool IsJuneDecisionsRequest)
        {
            FinalCohortAdjustmentOutput output = new FinalCohortAdjustmentOutput();

            try
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        String strColumn = "";
                        String strDirection = "";
                        //RequestID
                        if (sortExpression.Contains("RequestID"))
                        {
                            strColumn = "RequestID";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }
                        //DCSFNo
                        if (sortExpression.Contains("DCSFNo"))
                        {
                            strColumn = "DCSFNo";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }
                        //ForvusNo
                        if (sortExpression.Contains("ForvusNo"))
                        {
                            strColumn = "ForvusNo";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }
                        //RequestType
                        if (sortExpression.Contains("RequestType") || sortExpression.Contains("RequestReason"))
                        {
                            strColumn = "RequestType";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }
                        //ForvusUpdate
                        if (sortExpression.Contains("ForvusUpdate"))
                        {
                            strColumn = "ForvusUpdate";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }
                        //ForvusInitials
                        if (sortExpression.Contains("ForvusInitials"))
                        {
                            strColumn = "ForvusInitials";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }
                        //DCSFUpdate
                        if (sortExpression.Contains("DCSFUpdate"))
                        {
                            strColumn = "DCSFUpdate";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }
                        //Decision
                        if (sortExpression.Contains("Decision") || sortExpression.Contains("CohortDecision"))
                        {
                            strColumn = "Decision";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("Surname"))
                        {
                            strColumn = "Surname";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("Forename"))
                        {
                            strColumn = "Forename";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("Forename"))
                        {
                            strColumn = "Forename";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("DOB"))
                        {
                            strColumn = "Forename";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("CohortNotes"))
                        {
                            strColumn = "CohortNotes";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        System.Data.Common.DbConnection connection = conn.StoreConnection;

                        System.Data.Common.DbCommand cmd = connection.CreateCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Scrutiny.CohortDecisions_GetPage";

                        if (keyStageID > 0 || keyStageID == -1)
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@KeyStage",
                            SqlValue = keyStageID
                        });

                        if (dcsfNumber > 0 || dcsfNumber == -1)
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@DCSFNumber",
                                SqlValue = dcsfNumber
                            });

                        if (forvusIndex > 0 || forvusIndex == -1)
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@ForvusIndex",
                                SqlValue = forvusIndex
                            });

                        if (surname != null && surname != "")
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@Surname",
                                SqlValue = surname
                            });

                        if (scrutinyStatus != null && scrutinyStatus != "")
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@ScrutinyStatus",
                                SqlValue = scrutinyStatus
                            });


                        if (yearGroupCode != null && yearGroupCode != "")
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@YearGroupCode",
                                SqlValue = yearGroupCode
                            });

                        if (requestStatus != null && requestStatus != "")
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@RequestStatus",
                                SqlValue = Int32.Parse(requestStatus)
                            });

                        if (updatedBy != null && updatedBy != "")
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@UpdatedBy",
                                SqlValue = updatedBy
                            });

                        if (updatedAfter.HasValue && updatedAfter.Value > (new DateTime(1800, 1, 1)))
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.DateTime,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@UpdatedAfter",
                                SqlValue = updatedAfter.Value
                            });

                        if (schoolGroupID > 0)
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@SchoolGroupID",
                                SqlValue = schoolGroupID
                            });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@SortExpression",
                            SqlValue = strColumn
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@SortDirection",
                            SqlValue = strDirection
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Boolean,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@IsJuneCohortRequestDecisions",
                            SqlValue = IsJuneDecisionsRequest
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@UserName",
                            SqlValue = currentUserName
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@FromRow",
                            SqlValue = (page - 1) * rowsPerPage
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@ToRow",
                            SqlValue = (((page - 1) * rowsPerPage) + rowsPerPage)
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@RowCount"
                        });

                        System.Data.Common.DbDataReader dr = cmd.ExecuteReader();

                        List<CohortAdjustmentRequestResult> lst = new List<CohortAdjustmentRequestResult>();
                        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                        TextInfo textInfo = cultureInfo.TextInfo;
                        while (dr.Read())
                        {


                            CohortAdjustmentRequestResult obj = (new CohortAdjustmentRequestResult
                            {
                                DCSFNo = int.Parse(dr["DFESNumber"].ToString())
                                ,
                                ForvusIndex = dr.IsDBNull(dr.GetOrdinal("ForvusIndex")) ? "" : dr["ForvusIndex"].ToString()
                                ,
                                Surname = dr.IsDBNull(dr.GetOrdinal("Surname")) ? "" : textInfo.ToTitleCase(textInfo.ToLower(dr["Surname"].ToString()))
                                ,
                                RequestID = int.Parse(dr["RequestID"].ToString())
                                ,
                                RequestType = Int16.Parse(dr["RequestType"].ToString())
                                ,
                                RequestTypeToDisplay = dr["ReasonText"].ToString()
                                ,
                                Decision = dr.IsDBNull(dr.GetOrdinal("ScrutinyStatusDescription")) ? "" : dr["ScrutinyStatusDescription"].ToString()
                                ,
                                ForvusUpdate = dr.IsDBNull(dr.GetOrdinal("chgForvusForename")) ? "" : dr["chgForvusForename"].ToString() + " " + dr["chgForvusSurname"].ToString()
                                ,
                                ForvusUpdateDate = dr.IsDBNull(dr.GetOrdinal("chgForvusDate")) ? "" : DateTime.Parse(dr["chgForvusDate"].ToString()).ToString("dd-MM-yyyy hh:mm tt")
                                ,
                                DCSFUpdate = dr.IsDBNull(dr.GetOrdinal("chgDCSFDate")) ? "" : DateTime.Parse(dr["chgDCSFDate"].ToString()).ToString("dd-MM-yyyy hh:mm tt")
                                ,
                                DOB = dr.IsDBNull(dr.GetOrdinal("DOB")) ? "" : dr["DOB"].ToString()
                                ,
                                Forename = dr.IsDBNull(dr.GetOrdinal("Forename")) ? "" : textInfo.ToTitleCase(textInfo.ToLower(dr["Forename"].ToString()))
                                ,
                                Gender = dr.IsDBNull(dr.GetOrdinal("Gender")) ? "" : textInfo.ToTitleCase(textInfo.ToLower(dr["Gender"].ToString()))
                                ,
                                Notes = dr.IsDBNull(dr.GetOrdinal("DCSFNotification")) ? "" : dr["DCSFNotification"].ToString().Trim()
                                ,
                                RequestReason = dr["ReasonText"].ToString()
                            }
                                );

                            // check for automatic referrels to DCSF
                            String originalSCrutinyStatusCode = "";
                            if (!dr.IsDBNull(dr.GetOrdinal("OriginalScrutinyStatusCode")))
                                originalSCrutinyStatusCode = dr["OriginalScrutinyStatusCode"].ToString();

                            if (obj.ForvusUpdate == "" && originalSCrutinyStatusCode == "PD")
                            {
                                // Set auto referral
                                obj.ForvusUpdate = "Automatic referral";
                                // change date to request generation date
                                obj.ForvusUpdateDate = DateTime.Parse(dr["chgRequestDate"].ToString()).ToString("dd-MM-yyyy hh:mm tt");
                            }
                            lst.Add(obj);
                        }

                        dr.Close();

                        int cnt = 0;

                        if (int.TryParse(cmd.Parameters["@RowCount"].Value.ToString(), out cnt))
                            output.TotalRowCount = cnt;
                        else
                            output.TotalRowCount = 0;

                        output.adjustmentList = lst;

                        return output;
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
