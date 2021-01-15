using System;
using System.Data.EntityClient;
using System.Text;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
        public static string GetResultsForDownload(int dcsfNumber)
        {
            //declare the stringbuilder object that would serve as the placeholder for our data
            StringBuilder sb = new StringBuilder();
                
            sb.Append("DfE,");
            sb.Append("Candidate number,");
            sb.Append("Forvus index,");
            sb.Append("Surname,");
            sb.Append("Forename,");
            sb.Append("Gender,");
            sb.Append("Date of birth,");
            sb.Append("Age,");
            sb.Append("Exam,");

            sb.Append("Actual candidate number,");
            sb.Append("NCN,");
            sb.Append("Qualification code,");
            sb.Append("Awarding organisation,");
            sb.Append("Year,");
            sb.Append("Season,");
            sb.Append("Qualification type,");
            sb.Append("LEAP/LDCS code,");
            sb.Append("Subject name,");

            sb.Append("Syllabus code,");
            sb.Append("Grade,");
            sb.Append("Point score");


            sb.Append("\r\n");

            try
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();
                    System.Data.Common.DbConnection connection = conn.StoreConnection;
                    System.Data.Common.DbCommand cmd = connection.CreateCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = cmd.CommandTimeout * 10; // allow ten times longer than normal to get download file
                    cmd.CommandText = "Export.KS5SchoolData";

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@DFESNumber",
                        SqlValue = dcsfNumber
                    });

                    System.Data.Common.DbDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        sb.Append(dcsfNumber);
                        sb.Append(",");
                        sb.Append(dr.IsDBNull(dr.GetOrdinal("StudentValue")) ? "" : dr["StudentValue"].ToString());
                        sb.Append(",");
                        sb.Append(dr.IsDBNull(dr.GetOrdinal("ForvusIndex")) ? 0 : int.Parse(dr["ForvusIndex"].ToString()));
                        sb.Append(",");
                        sb.Append(dr.IsDBNull(dr.GetOrdinal("Surname")) ? "" : dr["Surname"].ToString());
                        sb.Append(",");
                        sb.Append(dr.IsDBNull(dr.GetOrdinal("ForeName")) ? "" : dr["ForeName"].ToString());
                        sb.Append(",");
                        sb.Append(dr.IsDBNull(dr.GetOrdinal("Gender")) ? "" : dr["Gender"].ToString());
                        sb.Append(",");

                        string dobOrig = dr.IsDBNull(dr.GetOrdinal("DOB")) ? "" : dr["DOB"].ToString();
                        string dobFinal = "";

                        if (dobOrig.Length == 8)
                        {
                            DateTime? dob = TSStudent.TryConvertDateTimeDBString(dobOrig);
                            if (dob.HasValue)
                            {
                                dobFinal = dob.Value.ToShortDateString();
                            }                           
                        }
                        sb.Append(dobFinal);
                        sb.Append(",");

                        sb.Append(dr.IsDBNull(dr.GetOrdinal("Age")) ? (byte)0 : byte.Parse(dr["Age"].ToString()));
                        sb.Append(",");
                        sb.Append(dr.IsDBNull(dr.GetOrdinal("Exam")) ? short.Parse("0") : short.Parse(dr["Exam"].ToString()));
                        sb.Append(",");
                        sb.Append(dr.IsDBNull(dr.GetOrdinal("ActualCandidateNumber")) ? "" : dr["ActualCandidateNumber"].ToString());
                        sb.Append(",");
                        sb.Append(dr.IsDBNull(dr.GetOrdinal("NationalCentreNumber")) ? "" : dr["NationalCentreNumber"].ToString());
                        sb.Append(",");
                        sb.Append(dr.IsDBNull(dr.GetOrdinal("QualificationCode")) ? "" : dr["QualificationCode"].ToString());
                        sb.Append(",");
                        sb.Append(dr.IsDBNull(dr.GetOrdinal("AwardingBody")) ? "" : dr["AwardingBody"].ToString());
                        sb.Append(",");
                        sb.Append(dr.IsDBNull(dr.GetOrdinal("Year")) ? "" : dr["Year"].ToString());
                        sb.Append(",");
                        sb.Append(dr.IsDBNull(dr.GetOrdinal("Season")) ? "" : dr["Season"].ToString());
                        sb.Append(",");
                        sb.Append(dr.IsDBNull(dr.GetOrdinal("QualificationType")) ? "" : dr["QualificationType"].ToString());
                        sb.Append(",");
                        sb.Append(dr.IsDBNull(dr.GetOrdinal("SubjectCode")) ? "" : dr["Subjectcode"].ToString());
                        sb.Append(",");
                        sb.Append(dr.IsDBNull(dr.GetOrdinal("SubjectName")) ? "" : dr["SubjectName"].ToString());
                        sb.Append(",");
                        sb.Append(dr.IsDBNull(dr.GetOrdinal("SyllabusCode")) ? "" : dr["SyllabusCode"].ToString());
                        sb.Append(",");
                        sb.Append(dr.IsDBNull(dr.GetOrdinal("Grade")) ? "" : dr["Grade"].ToString());
                        sb.Append(",");
                        sb.Append(dr.IsDBNull(dr.GetOrdinal("PointScore")) ? double.Parse("0") : double.Parse(dr["PointScore"].ToString()));

                        sb.Append("\r\n");
                      
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failure to retrieve results for download", ex);
            }

            return sb.ToString();
        }
    }
}
