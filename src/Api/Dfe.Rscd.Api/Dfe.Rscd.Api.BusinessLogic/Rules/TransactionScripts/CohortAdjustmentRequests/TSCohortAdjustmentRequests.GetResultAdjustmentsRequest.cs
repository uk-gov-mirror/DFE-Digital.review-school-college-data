using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.EntityClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSCohortAdjustmentRequests : Logic.TSBase
	{
        public static List<ResultAdjustmentRequestEntity> GetResultAdjustmentRequests(int? evidenceID, string scrutinyStatusCode, int? resultRequestID)
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
                    cmd.CommandText = "Scrutiny.ResultAdjustmentRequest_GetRequestDetailsCollection";

                    if ((evidenceID ?? 0) > 0)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                                               {
                                                   DbType = System.Data.DbType.Int32,
                                                   Direction = System.Data.ParameterDirection.Input,
                                                   ParameterName = "@EvidenceID",
                                                   SqlValue = evidenceID.Value
                                               });
                    }

                    if (!string.IsNullOrEmpty(scrutinyStatusCode))
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                                               {
                                                   DbType = System.Data.DbType.String,
                                                   Direction = System.Data.ParameterDirection.Input,
                                                   ParameterName = "@ScrutinyStatusCode",
                                                   SqlValue = scrutinyStatusCode
                                               });
                    }

                    if ((resultRequestID ?? 0) > 0)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                                               {
                                                   DbType = System.Data.DbType.Int32,
                                                   Direction = System.Data.ParameterDirection.Input,
                                                   ParameterName = "@ResultRequestID",
                                                   SqlValue = resultRequestID.Value
                                               });
                    }

                    System.Data.Common.DbDataReader dr = cmd.ExecuteReader();
                    List<ResultAdjustmentRequestEntity> lst = new List<ResultAdjustmentRequestEntity>();
                    CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                    TextInfo textInfo = cultureInfo.TextInfo;
                    while (dr.Read())
                    {

                        ResultAdjustmentRequestEntity ety = new ResultAdjustmentRequestEntity
                                                                {
                                                                    requestID = dr.IsDBNull(dr.GetOrdinal("RequestID")) ? 0 : Int32.Parse(dr["RequestID"].ToString()),
                                                                    pINCLFlag = dr.IsDBNull(dr.GetOrdinal("PINCLFlag")) ? "" : dr["PINCLFlag"].ToString(),
                                                                    pINCLDescription = dr.IsDBNull(dr.GetOrdinal("PINCLDescription")) ? "" : dr["PINCLDescription"].ToString(),
                                                                    cohortRequestStatusCode = dr.IsDBNull(dr.GetOrdinal("CohortRequestStatusCode")) ? "" : dr["CohortRequestStatusCode"].ToString(),
                                                                    cohortRequestStatusDescription = dr.IsDBNull(dr.GetOrdinal("CohortRequestStatusDescription")) ? "" : dr["CohortRequestStatusDescription"].ToString(),
                                                                    forvusIndex = dr.IsDBNull(dr.GetOrdinal("ForvusIndex")) ? "" : dr["ForvusIndex"].ToString(),
                                                                    forename = dr.IsDBNull(dr.GetOrdinal("Forename")) ? "" : dr["Forename"].ToString(),
                                                                    surname = dr.IsDBNull(dr.GetOrdinal("Surname")) ? "" : dr["Surname"].ToString(),
                                                                    dOB = dr.IsDBNull(dr.GetOrdinal("DOB")) ? "" : new DateTime(int.Parse(dr["DOB"].ToString().Substring(0, 4)),int.Parse(dr["DOB"].ToString().Substring(4, 2)),int.Parse(dr["DOB"].ToString().Substring(6, 2))).ToShortDateString(),
                                                                    age = dr.IsDBNull(dr.GetOrdinal("Age")) ? "" : dr["Age"].ToString(),
                                                                    gender = dr.IsDBNull(dr.GetOrdinal("Gender")) ? "" : dr["Gender"].ToString(),
                                                                    yearGroupCode = dr.IsDBNull(dr.GetOrdinal("YearGroupCode")) ? "" : dr["YearGroupCode"].ToString(),
                                                                    yearGroupDescription = dr.IsDBNull(dr.GetOrdinal("YearGroupDescription")) ? "" : dr["YearGroupDescription"].ToString(),
                                                                    awardingBodyCode = dr.IsDBNull(dr.GetOrdinal("AwardingBodyCode")) ? "" : dr["AwardingBodyCode"].ToString(),
                                                                    awardingBodyName = dr.IsDBNull(dr.GetOrdinal("AwardingBodyName")) ? "" : dr["AwardingBodyName"].ToString(),
                                                                    session = dr.IsDBNull(dr.GetOrdinal("Session")) ? "" : dr["Session"].ToString(),
                                                                    year = dr.IsDBNull(dr.GetOrdinal("Year")) ? "" : dr["Year"].ToString(),
                                                                    Exam_Date = dr["Exam_Date"] == DBNull.Value ? "n/a" : GetExamDateForDisplay(Convert.ToString(dr["Exam_Date"])),
                                                                    qualification = dr.IsDBNull(dr.GetOrdinal("Qualification")) ? "" : dr["Qualification"].ToString(),
                                                                    test = dr.IsDBNull(dr.GetOrdinal("Test")) ? "" : dr["Test"].ToString(),
                                                                    syllabus = dr.IsDBNull(dr.GetOrdinal("Syllabus")) ? "" : dr["Syllabus"].ToString(),
                                                                    qAN = dr.IsDBNull(dr.GetOrdinal("QAN")) ? "" : dr["QAN"].ToString(),
                                                                    subjectTitle = dr.IsDBNull(dr.GetOrdinal("SubjectTitle")) ? "" : dr["SubjectTitle"].ToString(),
                                                                    nCN = dr.IsDBNull(dr.GetOrdinal("NCN")) ? "" : dr["NCN"].ToString(),
                                                                    grade = dr.IsDBNull(dr.GetOrdinal("Grade")) ? "" : dr["Grade"].ToString(),
                                                                    level = dr.IsDBNull(dr.GetOrdinal("Level")) ? "" : dr["Level"].ToString(),
                                                                    mark = dr.IsDBNull(dr.GetOrdinal("Mark")) ? "" : dr["Mark"].ToString(),
                                                                    originalGrade = dr.IsDBNull(dr.GetOrdinal("OriginalGrade")) ? "" : dr["OriginalGrade"].ToString(),
                                                                    originalLevel = dr.IsDBNull(dr.GetOrdinal("OriginalLevel")) ? "" : dr["OriginalLevel"].ToString(),
                                                                    originalMark = dr.IsDBNull(dr.GetOrdinal("OriginalMark")) ? "" : dr["OriginalMark"].ToString(),
                                                                    fineGrade = dr.IsDBNull(dr.GetOrdinal("FineGrade")) ? "" : dr["FineGrade"].ToString(),
                                                                    resultID = dr.IsDBNull(dr.GetOrdinal("ResultID")) ? 0 : Int32.Parse(dr["ResultID"].ToString()),
                                                                    keystage = dr.IsDBNull(dr.GetOrdinal("Keystage")) ? Int16.Parse("0") : Int16.Parse(dr["Keystage"].ToString()),
                                                                    changeID = dr.IsDBNull(dr.GetOrdinal("ChangeID")) ? 0 : Int32.Parse(dr["ChangeID"].ToString()),
                                                                    resultAdjustmentScrutinyCode = dr.IsDBNull(dr.GetOrdinal("ResultAdjustmentScrutinyCode")) ? "" : dr["ResultAdjustmentScrutinyCode"].ToString(),
                                                                    resultAdjustmentReasonID = dr.IsDBNull(dr.GetOrdinal("ResultAdjustmentReasonId")) ? "" : dr["ResultAdjustmentReasonId"].ToString(),
                                                                    commentHistory = "",
                                                                    lastForvusUpdate = dr.IsDBNull(dr.GetOrdinal("LastForvusUpdate")) ? "" : dr["LastForvusUpdate"].ToString(),
                                                                    evidenceID = dr.IsDBNull(dr.GetOrdinal("EvidenceID")) ? 0 : Int32.Parse(dr["EvidenceID"].ToString()),
                                                                    RequestDate = dr.IsDBNull(dr.GetOrdinal("RequestDate")) ? "" : dr["RequestDate"].ToString(),
                                                                    RequestedBy = dr.IsDBNull(dr.GetOrdinal("RequestedBy")) ? "" : dr["RequestedBy"].ToString(),
                                                                    ResultReasonText = dr.IsDBNull(dr.GetOrdinal("ResultReasonText")) ? "" : dr["ResultReasonText"].ToString(),
                                                                    ResultType = dr.IsDBNull(dr.GetOrdinal("ResultType")) ? "" : dr["ResultType"].ToString(),
                                                                    DocumentLocation = dr.IsDBNull(dr.GetOrdinal("DocumentLocation")) ? "" : dr["DocumentLocation"].ToString(),
                                                                    DocumentType = dr.IsDBNull(dr.GetOrdinal("DocumentType")) ? "" : dr["DocumentType"].ToString(),
                                                                    DCSFNumber = dr.IsDBNull(dr.GetOrdinal("DFESNumber")) ? 0 : Int32.Parse(dr["DFESNumber"].ToString()),
                                                                    EvidenceDate = dr.IsDBNull(dr.GetOrdinal("EvidenceDate")) ? "" : dr["EvidenceDate"].ToString(),
                                                                    ScrutinyMessage = GetScrutinyMessage(dr)
                                                                };
                        String commentHistory = "";
                        var list =
                            (
                                from rrc in context.ResultRequestChanges
                                where rrc.ResultRequestID == ety.requestID
                                      &&
                                      rrc.Comments != ""
                                orderby rrc.ChangeID descending
                                select new
                                           {
                                               UserName = rrc.Changes.UserName,
                                               CommentDate = rrc.Changes.ChangeDate,
                                               Comment = rrc.Comments
                                           }
                            ).ToList();

                        foreach (var itm in list)
                            commentHistory += itm.UserName + ": " + itm.CommentDate.ToShortDateString() + "\r\n" + itm.Comment + "\r\n\r\n";

                        ety.commentHistory = commentHistory;

                        lst.Add(ety);
                    }
                    dr.Close();
                    return lst;
                }
            }
        }

        public static string GetExamDateForDisplay(string ddmmyyyyy)
        {
            string exam_Date = "n/a";

            if (String.IsNullOrEmpty(ddmmyyyyy))
            {
                return exam_Date;
            }

            if (ddmmyyyyy == "00000000")
            {
                exam_Date = "n/a";
                return exam_Date;
            }

            if (ddmmyyyyy == "88888888")
            {
                exam_Date = "not avail";
                return exam_Date;
            }

            if (ddmmyyyyy == "99999999")
            {
                exam_Date = "multiple";
                return exam_Date;
            }

            try
            {
                exam_Date = DateTime.ParseExact(ddmmyyyyy, "ddMMyyyy", CultureInfo.InvariantCulture).ToShortDateString();
            }
            catch (Exception)
            {
                exam_Date = ddmmyyyyy; // should never happen
            }

            return exam_Date;
        }

        private static string GetScrutinyMessage(DbDataReader currentReader)
        {
            if(currentReader["YearStart"] == DBNull.Value 
                || currentReader["YearDeleted"] == DBNull.Value
                || (currentReader["KS4Main"] == DBNull.Value && currentReader["KS5Main"] == DBNull.Value)
                || (currentReader["APR15"] == DBNull.Value && currentReader["APR1618"] == DBNull.Value)
                || currentReader["Year"] == DBNull.Value)
            {
                return string.Empty;
            }
            var yearStart = Convert.ToInt32(currentReader["YearStart"]);
            var yearDeleted = Convert.ToInt32(currentReader["YearDeleted"]);
            var year = Convert.ToInt32(currentReader["Year"]);
            
            bool ks4Main;
            bool ks5Main;
            int Apr15;
            int Apr1618;

            bool.TryParse(currentReader["KS4Main"].ToString(), out ks4Main);
            bool.TryParse(currentReader["KS5Main"].ToString(), out ks5Main);
            int.TryParse(currentReader["APR1618"].ToString(), out Apr1618);
            int.TryParse(currentReader["APR15"].ToString(), out Apr15);

            var inYear = (yearStart <= year && year <= yearDeleted);

            if (Convert.ToInt32(currentReader["KeyStage"]) == 4)
            {
                if (Apr15 == 1 && inYear & !ks4Main)
                {
                    return "NOT COUNTED";
                }
                if (Apr15 == 2 && inYear)
                {
                    return "NOT APPROVED";
                }
                if (Apr15 == 1 & !inYear)
                {
                    return "YEAR OUT OF RANGE";
                }
            } else
            {
                if (Apr1618 == 1 && inYear & !ks5Main)
                {
                    return "NOT COUNTED";
                }
                if (Apr1618 == 2 && inYear)
                {
                    return "NOT APPROVED";
                }
                if (Apr1618 == 1 & !inYear)
                {
                    return "YEAR OUT OF RANGE";
                }
            }

            return string.Empty;
        }
	}
}
