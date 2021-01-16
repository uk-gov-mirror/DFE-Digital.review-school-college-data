using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Globalization;
using System.Threading;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSCohortAdjustmentRequests : Logic.TSBase
    {
        public static FinalCohortAdjustmentOutput GetResultAdjustmentRequestsPage(int page, int rowsPerPage, string scrutinyStatus, short keyStageID, int dcsfNumber, int forvusIndex, int requestID, string surname, Int16 schoolGroupID, string updatedBy, DateTime? updatedAfter, String currentUserName, String sortExpression, int reasonID, String supportedBy)
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

                        if (sortExpression.Contains("SubjectTitle"))
                        {
                            strColumn = "SubjectTitle";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("GradeCode"))
                        {
                            strColumn = "GradeCode";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("EvidenceDate"))
                        {
                            strColumn = "EvidenceDate";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }     

                        System.Data.Common.DbConnection connection = conn.StoreConnection;

                        System.Data.Common.DbCommand cmd = connection.CreateCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Scrutiny.ResultAdjustmentRequest_GetPage";                    

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

                        if (requestID > 0 || requestID==-1)
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@RequestID",
                                SqlValue = requestID
                            });

                        if (updatedBy != null && updatedBy != "")
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@UpdatedBy",
                                SqlValue = updatedBy
                            });

                        if (updatedAfter.HasValue && updatedAfter.Value>(new DateTime(1800,1,1)))
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.DateTime,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@UpdatedAfter",
                                SqlValue = updatedAfter.Value
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

                        if (reasonID > 0)
                        {
                            String strRequestType="";
                            switch (reasonID)
                            {
                                case 0:
                                    strRequestType = "";
                                    break;
                                case 1:
                                    strRequestType = "Added";
                                    break;
                                case 2:
                                    strRequestType = "Amended";
                                    break;
                                default:
                                    strRequestType = "";
                                    break;
                            }

                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@RequestType",
                                SqlValue = strRequestType
                            });
                        }

                        if (supportedBy != "")
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@SupportedBy",
                                SqlValue = supportedBy
                            });
                        }

                        if (schoolGroupID > 0)
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@SchoolGroupID",
                                SqlValue = schoolGroupID
                            });
                        }

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
                            lst.Add
                                (new CohortAdjustmentRequestResult
                                {
                                    DCSFNo = dr.IsDBNull(dr.GetOrdinal("DFESNumber")) ?  -1 : int.Parse(dr["DFESNumber"].ToString())
                                    ,
                                    ForvusIndex = dr.IsDBNull(dr.GetOrdinal("ForvusIndex")) ? "" : dr["ForvusIndex"].ToString()
                                    ,                                 
                                    Surname = dr.IsDBNull(dr.GetOrdinal("Surname")) ? "" : textInfo.ToTitleCase(textInfo.ToLower(dr["Surname"].ToString()))
                                    ,
                                    RequestID = dr.IsDBNull(dr.GetOrdinal("RequestID")) ? -1 : int.Parse(dr["RequestID"].ToString())
                                    ,
                                    RequestType = Int16.Parse(dr["RequestType"].ToString())
                                    ,
                                    ResultType = dr.IsDBNull(dr.GetOrdinal("ResultType")) ? "" : dr["ResultType"].ToString()
                                    ,
                                    RequestTypeToDisplay = dr.IsDBNull(dr.GetOrdinal("ResultReasonText")) ? "" : dr["ResultReasonText"].ToString()
                                    ,
                                    Decision = dr.IsDBNull(dr.GetOrdinal("ScrutinyStatusDescription")) ? "" : dr["ScrutinyStatusDescription"].ToString()
                                    ,
                                    ForvusUpdate = dr.IsDBNull(dr.GetOrdinal("chgForvusForename")) ? "" : dr["chgForvusForename"].ToString() + " " + dr["chgForvusSurname"].ToString()
                                    ,
                                    ForvusUpdateDate = dr.IsDBNull(dr.GetOrdinal("chgForvusDate")) ? "" : DateTime.Parse(dr["chgForvusDate"].ToString()).ToString("dd-MM-yyyy hh:mm tt")
                                    ,
                                    EvidenceID = int.Parse((dr["EvidenceID"] == DBNull.Value ? "-1" : dr["EvidenceID"].ToString()))
                                    ,                              
                                    Forename = dr.IsDBNull(dr.GetOrdinal("Forename")) ? "" : textInfo.ToTitleCase(textInfo.ToLower(dr["Forename"].ToString()))       
                                    ,                                    
                                    RequestReason = dr.IsDBNull(dr.GetOrdinal("ResultReasonText")) ? "" : dr["ResultReasonText"].ToString()
                                    ,
                                    SubjectTitle = dr.IsDBNull(dr.GetOrdinal("SubjectTitle")) ? "" : dr["SubjectTitle"].ToString()                                                                        
                                    ,
                                   RequestCount = int.Parse(dr["RequestCount"].ToString())
                                   ,
                                   ScrutinyStatusCode= dr["ScrutinyStatusCode"].ToString()
                                   ,
                                   GradeCode = dr["GradeCode"].ToString()
                                   ,
                                   EvidenceDate = dr.IsDBNull(dr.GetOrdinal("EvidenceDate")) ? "" : DateTime.Parse(dr["EvidenceDate"].ToString()).ToString("dd-MM-yyyy")
                                }
                                );
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

        public static List<ResultAdjustmentRequestListPage> GetResultAdjustmentRequestsPageList(int page, int rowsPerPage, string scrutinyStatus, short keyStageID, int dcsfNumber, int forvusIndex, int requestID, string surname, Int16 schoolGroupID, string updatedBy, DateTime? updatedAfter, String currentUserName, String sortExpression, int reasonID, String supportedBy, out int totalRowCount)
        {
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

                        if (sortExpression.Contains("SubjectTitle"))
                        {
                            strColumn = "SubjectTitle";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        System.Data.Common.DbConnection connection = conn.StoreConnection;

                        System.Data.Common.DbCommand cmd = connection.CreateCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Scrutiny.ResultAdjustmentRequest_GetPage";

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

                        if (requestID > 0 || requestID == -1)
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@RequestID",
                                SqlValue = requestID
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

                        if (reasonID > 0)
                        {
                            String strRequestType = "";
                            switch (reasonID)
                            {
                                case 0:
                                    strRequestType = "";
                                    break;
                                case 1:
                                    strRequestType = "Added";
                                    break;
                                case 2:
                                    strRequestType = "Amended";
                                    break;
                                default:
                                    strRequestType = "";
                                    break;
                            }

                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@RequestType",
                                SqlValue = strRequestType
                            });
                        }

                        if (supportedBy != "")
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@SupportedBy",
                                SqlValue = supportedBy
                            });
                        }

                        if (schoolGroupID > 0)
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@SchoolGroupID",
                                SqlValue = schoolGroupID
                            });
                        }

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
                        List<ResultAdjustmentRequestListPage> lst = new List<ResultAdjustmentRequestListPage>();
                        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                        TextInfo textInfo = cultureInfo.TextInfo;
                        while (dr.Read())
                        {
                            lst.Add
                                (new ResultAdjustmentRequestListPage
                                {
                                    DCSFNo = dr.IsDBNull(dr.GetOrdinal("DFESNumber")) ? -1 : int.Parse(dr["DFESNumber"].ToString())
                                    ,
                                    RequestID = dr.IsDBNull(dr.GetOrdinal("RequestID")) ? -1 : int.Parse(dr["RequestID"].ToString())
                                    ,
                                    SubjectTitle = dr.IsDBNull(dr.GetOrdinal("SubjectTitle")) ? "" : dr["SubjectTitle"].ToString()
                                    ,
                                    EvidenceID = Int16.Parse(dr["EvidenceID"].ToString())
                                    ,
                                    ScrutinyStatusCode = dr["ScrutinyStatusCode"].ToString()
                                }
                                );
                        }
                        dr.Close();

                        int cnt = 0;

                        if (int.TryParse(cmd.Parameters["@RowCount"].Value.ToString(), out cnt))
                            totalRowCount = cnt;
                        else
                            totalRowCount = 0;

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
