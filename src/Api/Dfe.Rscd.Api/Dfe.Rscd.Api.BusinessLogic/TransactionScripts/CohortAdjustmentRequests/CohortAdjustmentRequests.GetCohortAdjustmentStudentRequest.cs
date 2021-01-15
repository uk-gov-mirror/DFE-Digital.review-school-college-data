using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Linq;
using System.Text;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSCohortAdjustmentRequests: Logic.TSBase
    {

        public enum StudentRequestFetchType
        {
            LatestStatus = 1,
            OriginalStatus = 2
        }

        private class StudentRequestInfoFromDB
        {
            public DateTime DCSFChangeDate { get; set; }
            public string DCSFChangeForename { get; set; }
            public string DCSFChangeSurname { get; set; }
            public DateTime ForvusChangeDate { get; set; }
            public string ForvusChangeForename { get; set; }
            public string ForvusChangeSurname { get; set; }
            public string ScrutinyCode { get; set; }
            public string CommentsHistory { get; set; }
        }      

        private static StudentRequestInfoFromDB GetStudentRequestFromDB( int requestID, StudentRequestFetchType studentRequestFetchType )
        {
            StudentRequestInfoFromDB info = new StudentRequestInfoFromDB();

            info.DCSFChangeDate       = DateTime.MinValue;
            info.DCSFChangeForename   = string.Empty;
            info.DCSFChangeSurname    = string.Empty;

            info.ForvusChangeDate     = DateTime.MinValue;
            info.ForvusChangeForename = string.Empty;
            info.ForvusChangeSurname  = string.Empty;

            using (var conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (var context2 = new Web09_Entities(conn))
                {
                    var connection = conn.StoreConnection;

                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[Scrutiny].[GetCohortAdjustmentStudentRequest]";

                        int fetchLatest = studentRequestFetchType == StudentRequestFetchType.LatestStatus ? 1 : 0;

                        SetInputParamForCommand(cmd, "StudentRequestID", requestID);
                        SetInputParamForCommand(cmd, "FetchLatest", fetchLatest);

                        using (System.Data.Common.DbDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                if (!Convert.IsDBNull(dr["DCSFChangeDate"]))
                                {
                                  info.DCSFChangeDate       = Convert.ToDateTime(dr["DCSFChangeDate"]);
                                  info.DCSFChangeForename   = Convert.ToString(dr["DCSFChangeForename"]);
                                  info.DCSFChangeSurname    = Convert.ToString(dr["DCSFChangeSurname"]);
                                }

                                if (!Convert.IsDBNull(dr["ForvusChangeDate"]))
                                {
                                    info.ForvusChangeDate     = Convert.ToDateTime(dr["ForvusChangeDate"]);
                                    info.ForvusChangeForename = Convert.ToString(dr["ForvusChangeForename"]);
                                    info.ForvusChangeSurname  = Convert.ToString(dr["ForvusChangeSurname"]);
                                }

                                info.ScrutinyCode = Convert.ToString(dr["ScrutinyCode"]);
                            }

                            // now read comments history
                            dr.NextResult();
                            StringBuilder commentsHistoryBuilder = new StringBuilder();
                            while (dr.Read())
                            {
                                string userName     = Convert.ToString(dr["username"]);
                                DateTime changeDate = Convert.ToDateTime(dr["ChangeDate"]);
                                string comments     = Convert.ToString(dr["Comments"]);

                                commentsHistoryBuilder.AppendLine( string.Format("{0} : {1}", userName, changeDate.ToShortDateString()) );
                                commentsHistoryBuilder.AppendLine(comments);
                                commentsHistoryBuilder.AppendLine();
                            }

                            info.CommentsHistory = commentsHistoryBuilder.ToString();

                            dr.Close();
                        }
                    }
                }
            }

            return info;
        }
        
        public static CohortAdjustmentRequestEntity GetCohortAdjustmentStudentRequest(Web09_Entities context, Int32 requestID, StudentRequestFetchType studentRequestFetchType)
        {
            var request = (
                                           from r in context.StudentRequests
                                           where r.StudentRequestID == requestID
                                           select new
                                           {
                                               Request = r,
                                               Student = r.Students,
                                               IAR = r.InclusionAdjustmentReasons,
                                               RequestTypeToDisplay =
                                               r.InclusionAdjustmentReasons.IncAdjReasonID==30?"Edit pupil":
                                               ((from inc in context.InclusionAdjustmentReasons where inc.IncAdjReasonID == r.InclusionAdjustmentReasons.IncAdjReasonID select inc.IsInclusion).FirstOrDefault() ? "Include" : "Remove"),
                                               // Original request for User and creation Date
                                               OriginalRequest =r.Changes
                                           }
                                       ).FirstOrDefault();

            if (request == null)
                throw new BusinessLevelException("Student Request not found.");

            StudentRequestInfoFromDB info = GetStudentRequestFromDB(requestID, studentRequestFetchType);
           
            if (studentRequestFetchType == StudentRequestFetchType.LatestStatus)
            {
            request.Request.StudentRequestChanges.Add
                (
                    context.StudentRequestChanges
                    .Include("AmendCodes")
                    .Include("ScrutinyStatus")
                    .Include("Changes")
                    .Where
                    (
                        src =>
                            src.StudentRequestID == request.Request.StudentRequestID
                            && src.DateEnd == null
                    )
                    .FirstOrDefault()
                );
            }
            else if (studentRequestFetchType == StudentRequestFetchType.OriginalStatus)
            {
                request.Request.StudentRequestChanges.Add
                   (
                       context.StudentRequestChanges
                       .Include("AmendCodes")
                       .Include("ScrutinyStatus")
                       .Include("Changes")
                       .Where
                       (
                           src =>
                               src.StudentRequestID == request.Request.StudentRequestID
                       )
                       .OrderBy(src => src.DateEnd ?? DateTime.Now)
                       .FirstOrDefault()
                   );
            }

            request.Request.StudentRequestChanges.FirstOrDefault().AmendCodesReference.Load();
            request.Request.ChangesReference.Load();
            request.Request.ReasonsReference.Load();
            request.Request.InclusionAdjustmentReasonsReference.Load();
            request.Request.StudentRequestChanges.FirstOrDefault().ReasonsReference.Load();
            request.Request.StudentRequestChanges.FirstOrDefault().ScrutinyStatusReference.Load();

            // TODO spin this off into SQL
            // Original Amend Code
            AmendCodes amd=context.StudentRequestChanges.Include("AmendCodes").Where(src=>src.StudentRequestID==request.Request.StudentRequestID).OrderBy(o=>o.ChangeID).FirstOrDefault().AmendCodes;

            CohortAdjustmentRequestEntity obj = new CohortAdjustmentRequestEntity
            {
                StudentRequest = request.Request,
                RequestType = request.RequestTypeToDisplay,
                DCSFUpdateDate = info.DCSFChangeDate != DateTime.MinValue ? info.DCSFChangeDate.ToString("dd/MM/yyyy HH:mm:ss") + " " + info.DCSFChangeForename + " " + info.DCSFChangeSurname : "",
                ForvusUpdateDate = info.ForvusChangeDate != DateTime.MinValue ? info.ForvusChangeDate.ToString("dd/MM/yyyy HH:mm:ss") + " " + info.ForvusChangeForename + " " + info.ForvusChangeSurname : "",
                RequestDate = request.OriginalRequest.ChangeDate.ToShortDateString(),
                RequestBy = request.OriginalRequest.Forename+" "+request.OriginalRequest.Surname,
                SuggestedDecision = (info.ScrutinyCode == "A" || info.ScrutinyCode == "R" || info.ScrutinyCode == "AA") ? info.ScrutinyCode : "",
                CommentHistory = info.CommentsHistory,
                OriginalAmendCode=amd==null? "" : amd.AmendCode
            };

            if (
                info.ScrutinyCode == "PD" // originally a pending DCSF
                &&
                obj.ForvusUpdateDate=="" // no forvus update yet
                )
            {
                //replace with automatic referral
                obj.ForvusUpdateDate = request.OriginalRequest.ChangeDate.ToString("dd/MM/yyyy HH:mm:ss") + " Automatic referral";
            }

            return obj;
        }


        private class SchoolRequestInfoFromDB
        {
            public DateTime DCSFChangeDate { get; set; }
            public string DCSFChangeForename { get; set; }
            public string DCSFChangeSurname { get; set; }
            public DateTime ForvusChangeDate { get; set; }
            public string ForvusChangeForename { get; set; }
            public string ForvusChangeSurname { get; set; }
            public string ScrutinyCode { get; set; }
            public string CommentsHistory { get; set; }
            public string DocumentLocation { get; set; }
            public string DocumentType { get; set; }
        }

        private class aComment
        {
            public String UserName { get; set; }
            public DateTime ChangeDate { get; set; }
            public string Comments { get; set; }
        }

        private static SchoolRequestInfoFromDB GetSchoolRequestFromDB(int requestID)
        {
            SchoolRequestInfoFromDB info = new SchoolRequestInfoFromDB();

            info.DCSFChangeDate = DateTime.MinValue;
            info.DCSFChangeForename = string.Empty;
            info.DCSFChangeSurname = string.Empty;

            info.ForvusChangeDate = DateTime.MinValue;
            info.ForvusChangeForename = string.Empty;
            info.ForvusChangeSurname = string.Empty;

            using (var conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (var context2 = new Web09_Entities(conn))
                {
                    var connection = conn.StoreConnection;

                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[Scrutiny].[GetCohortAdjustmentSchoolRequest]";

                        SetInputParamForCommand(cmd, "SchoolRequestID", requestID);

                        using (System.Data.Common.DbDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                if (!Convert.IsDBNull(dr["DCSFChangeDate"]))
                                {
                                    info.DCSFChangeDate = Convert.ToDateTime(dr["DCSFChangeDate"]);
                                    info.DCSFChangeForename = Convert.ToString(dr["DCSFChangeForename"]);
                                    info.DCSFChangeSurname = Convert.ToString(dr["DCSFChangeSurname"]);
                                }

                                if (!Convert.IsDBNull(dr["ForvusChangeDate"]))
                                {
                                    info.ForvusChangeDate = Convert.ToDateTime(dr["ForvusChangeDate"]);
                                    info.ForvusChangeForename = Convert.ToString(dr["ForvusChangeForename"]);
                                    info.ForvusChangeSurname = Convert.ToString(dr["ForvusChangeSurname"]);
                                }

                                info.ScrutinyCode = Convert.ToString(dr["ScrutinyCode"]);

                                info.DocumentLocation = Convert.ToString(dr["DocumentLocation"]);
                                info.DocumentType = Convert.ToString(dr["DocumentType"]);
                            }

                            // now read comments history
                            dr.NextResult();
                            List<aComment> commentsList = new List<aComment>();
                            StringBuilder commentsHistoryBuilder = new StringBuilder();
                            while (dr.Read())
                            {
                                commentsList.Add( new aComment{  UserName =  Convert.ToString(dr["username"])
                                    , ChangeDate = Convert.ToDateTime(dr["ChangeDate"])
                                    , Comments =  Convert.ToString(dr["Comments"])} );
                            }                         

                            dr.Close();

                            for (int commentLoopIndex = 0; commentLoopIndex < commentsList.Count-1; commentLoopIndex++)
                            {
                                string formattedNameAndDate = string.Format("{0} : {1}", commentsList[commentLoopIndex].UserName, commentsList[commentLoopIndex].ChangeDate.ToShortDateString());
                                commentsHistoryBuilder.AppendLine(formattedNameAndDate);

                                commentsHistoryBuilder.AppendLine(commentsList[commentLoopIndex].Comments);
                                commentsHistoryBuilder.AppendLine();
                            }

                            if (commentsList.Count > 0)
                            {
                                commentsHistoryBuilder.AppendLine("|NOR|");
                                commentsHistoryBuilder.Append(commentsList[commentsList.Count - 1].Comments);
                            }

                            info.CommentsHistory = commentsHistoryBuilder.ToString();
                        }
                    }
                }
            }

            return info;
        }

        public static CohortAdjustmentRequestEntity GetCohortAdjustmentSchoolRequest(Web09_Entities context, Int32 requestID)
        {
            var request = (
                                           from r in context.SchoolRequests
                                           where r.SchoolRequestID == requestID
                                           select new
                                           {
                                               Request = r,
                                               School = r.Schools,
                                               IAR = "",
                                               RequestTypeToDisplay ="NOR Update",                                               
                                               // Original request for User and creation Date
                                               OriginalRequest = r.Changes
                                           }
                                       ).FirstOrDefault();

            if (request == null)
                throw new BusinessLevelException("School Request not found.");

            SchoolRequestInfoFromDB info = GetSchoolRequestFromDB(requestID);
          
            request.Request.SchoolRequestChanges.Add
                (
                    context.SchoolRequestChanges                   
                    .Include("ScrutinyStatus")
                    .Include("Changes")
                    .Include("SchoolReasons")
                    .Where
                    (
                        src =>
                            src.SchoolRequestID == request.Request.SchoolRequestID
                            && src.DateEnd == null
                    )
                    .FirstOrDefault()
                );
            request.Request.ChangesReference.Load();
            request.Request.SchoolRequestChanges.FirstOrDefault().ScrutinyStatusReference.Load();

            var ssr = request.Request.SchoolRequestChanges.Last();
            var schoolReason = 0;
            if(ssr.SchoolReasons != null)
            {
                schoolReason = ssr.SchoolReasons.SchoolReasonId;
            }

            var obj = new CohortAdjustmentRequestEntity
            {
                SchoolRequest = request.Request,
                RequestType = request.RequestTypeToDisplay,
                DCSFUpdateDate = info.DCSFChangeDate != DateTime.MinValue ? info.DCSFChangeDate.ToString("dd/MM/yyyy HH:mm:ss") + " : " + info.DCSFChangeForename + " " + info.DCSFChangeSurname : "",
                ForvusUpdateDate = info.ForvusChangeDate != DateTime.MinValue ? info.ForvusChangeDate.ToString("dd/MM/yyyy HH:mm:ss") + " : " + info.ForvusChangeForename + " " + info.ForvusChangeSurname: "",
                RequestDate = request.OriginalRequest.ChangeDate.ToShortDateString(),
                RequestBy = request.OriginalRequest.Forename+ " " + request.OriginalRequest.Surname,
                SuggestedDecision = (info.ScrutinyCode == "A" || info.ScrutinyCode == "R" || info.ScrutinyCode == "AA") ? info.ScrutinyCode : "",
                CommentHistory = info.CommentsHistory,
                OriginalAmendCode = "",
                DocumentLocation = info.DocumentLocation,
                DocumentType = info.DocumentType,
                SchoolReasonId = schoolReason
            };

            if (
                    info.ScrutinyCode == "PD" // originally a pending DCSF
                    &&
                    obj.ForvusUpdateDate == "" // no forvus update yet
                    )
            {
                //replace with automatic referral
                obj.ForvusUpdateDate = request.OriginalRequest.ChangeDate.ToString("dd/MM/yyyy HH:mm:ss") + " Automatic referral";
            }

            return obj;
        }

        public static CohortAdjustmentRequestEntity GetCohortAdjustmentStudentRequest(Int32 requestID)
        {
            try
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        return GetCohortAdjustmentStudentRequest(context, requestID, StudentRequestFetchType.LatestStatus);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CohortAdjustmentRequestEntity GetCohortAdjustmentSchoolRequest(Int32 requestID)
        {
            try
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        return GetCohortAdjustmentSchoolRequest(context, requestID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CohortAdjustmentRequestEntity GetCohortAdjustmentStudentRequestWithOriginalStatus(int studentId)
        {
            int? studentRequestID;

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    //Get the outstanding student request (ie where status is not cancelled!!)
                    studentRequestID = context.StudentRequestChanges
                            .Where(src => 
                                src.StudentRequests.Students.StudentID == studentId && 
                                src.ScrutinyStatus.ScrutinyStatusCode != Contants.SCRUTINY_STATUS_CANCELLED && 
                                src.DateEnd == null)
                            .Select(src => src.StudentRequests.StudentRequestID)
                            .FirstOrDefault();

                    if (studentRequestID.HasValue)
                        return GetCohortAdjustmentStudentRequest(context, studentRequestID.Value, StudentRequestFetchType.OriginalStatus);
                    else
                        throw new BusinessLevelException("Student request not found");
                }
            }
        }       
    }
}
