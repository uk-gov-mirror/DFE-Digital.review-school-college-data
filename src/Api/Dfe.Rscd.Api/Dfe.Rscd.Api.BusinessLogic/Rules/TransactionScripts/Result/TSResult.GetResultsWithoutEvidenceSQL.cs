using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Web.Script.Serialization;
using Web09.Services.Common.JSONObjects;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {

        internal class GetResultsWithoutEvidenceResult
        {
            public int StudentID { get; set; }
        }       

        // TFS 19452
        public static string GetResultsWithoutEvidenceSQL(int dcsfNumber)
        {
            IList<ResultWithoutEvidence> jsonResultList = new List<ResultWithoutEvidence>();
      
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                System.Data.Common.DbConnection connection = conn.StoreConnection;
                System.Data.Common.DbCommand cmd = connection.CreateCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Scrutiny.GetResultRequestsWithoutEvidence";

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                {
                    DbType = System.Data.DbType.Int32,
                    Direction = System.Data.ParameterDirection.Input,
                    ParameterName = "@DFESNumber",
                    SqlValue = dcsfNumber
                });

                using (System.Data.Common.DbDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ResultWithoutEvidence newJSONResult = new ResultWithoutEvidence();
                       
                        newJSONResult.KeyStage                 = Convert.ToInt32(dr["KeyStage"]);
                        newJSONResult.ForvusIndex              = Convert.ToInt32(dr["ForvusIndex"]);
                        newJSONResult.StudentID                = Convert.ToInt32(dr["StudentID"]);
                        newJSONResult.Surname                  = Convert.ToString(dr["Surname"]);
                        newJSONResult.Forename                 = Convert.ToString(dr["Forename"]);
                        newJSONResult.DOBDisplayString         = Convert.ToString(dr["DOBDisplayString"]);
                        newJSONResult.DOB                      = Convert.ToString(dr["DOB"]);
                        newJSONResult.Gender                   = Convert.ToString(dr["Gender"]);
                        newJSONResult.AmendmentType            = Convert.ToString(dr["AmendmentType"]);
                        newJSONResult.Subject                  = Convert.ToString(dr["Subject"]);
                        newJSONResult.ResultID                 = Convert.ToInt32(dr["ResultID"]);
                        newJSONResult.ResultStatusDescription  = Convert.ToString(dr["ResultStatusDescription"]);
                        newJSONResult.ResultCurrentChangeID    = Convert.ToInt32(dr["ResultCurrentChangeID"]);
                        newJSONResult.NationalCentreNumber     = Convert.ToString(dr["NationalCentreNumber"]);
                        newJSONResult.YearGroup                = Convert.ToInt32(dr["YearGroup"]);
                        newJSONResult.AwardingBodyName         = Convert.ToString(dr["AwardingBodyName"]);
                        newJSONResult.QAN                      = Convert.ToString(dr["QAN"]);
                        newJSONResult.BoardSubjectNumber       = Convert.ToString(dr["BoardSubjectNumber"]);
                        newJSONResult.NewGrade                 = Convert.ToString(dr["NewGrade"]);
                        newJSONResult.NewMarks                 = Convert.ToString(dr["NewMarks"]);
                        newJSONResult.FineGrade                = Convert.ToString(dr["FineGrade"]);
                        newJSONResult.NewGradeResultStatus     = Convert.ToString(dr["NewGradeResultStatus"]);
                        newJSONResult.Qualification            = Convert.ToString(dr["Qualification"]);
                        newJSONResult.Title                    = Convert.ToString(dr["Title"]);
                        newJSONResult.Session                  = Convert.ToString(dr["Session"]);
                        newJSONResult.ResultType               = Convert.ToString(dr["ResultType"]);

                        jsonResultList.Add(newJSONResult);                                     
                    }

                    dr.Close();
                }
            }

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            string jsonData = jsSerializer.Serialize(jsonResultList);

            return jsonData;
        }
    }
}
                    
        