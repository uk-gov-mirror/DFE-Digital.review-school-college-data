using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent : Logic.TSBase
    {
        public static Int32 GetStudentsWithResultsCount(int dcsfNumber, short keyStageID, string forename, string surname, DateTime? dob, DateTime? doa, string gender, string inclusionStatus, string ethinicityCode, string firstLanguageCode, string yearGroupCode, Int32? subjectID, String qan, String syllabus, Int32? awardingBodyID, Int32? qanSubjectID, int? subLevelId, String alphabetForSurname, String sortExpression, String strAge)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    if (!Validation.Common.IsDCSFNumberValid(context, dcsfNumber))
                        throw Web09Exception.GetBusinessException(Web09MessageList.DCSFNumberInvalid);
                    else if (!Validation.Common.IsKeyStageValid(context, keyStageID))
                        throw Web09Exception.GetBusinessException(Web09MessageList.KeystageInvalid);          

                    System.Data.Common.DbConnection connection = conn.StoreConnection;

                    System.Data.Common.DbCommand cmd = connection.CreateCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Student.StudentResults_GetPage";

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@keyStageID",
                        SqlValue = keyStageID
                    });

                    if (dcsfNumber > 0 || dcsfNumber == -1)
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@dcsfNumber",
                            SqlValue = dcsfNumber
                        });

                    if (forename != "")
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@forename",
                            SqlValue = forename
                        });

                    if (surname != "")
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@surname",
                            SqlValue = surname
                        });

                    String strDOB = "";
                    if (dob.HasValue)
                        strDOB = dob.Value.Year.ToString() + dob.Value.Month.ToString("00") + dob.Value.Day.ToString("00");

                    String strDOA = "";
                    if (doa.HasValue)
                        strDOA = doa.Value.Year.ToString() + doa.Value.Month.ToString("00") + doa.Value.Day.ToString("00");

                    if (strDOB != "")
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@dob",
                            SqlValue = strDOB
                        });

                    if (strDOA != "")
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@doa",
                            SqlValue = strDOA
                        });

                    if (gender != "")
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@gender",
                            SqlValue = gender
                        });

                    if (inclusionStatus != null && inclusionStatus.Trim() != "")
                    {
                        string tickFlag = "√";

                        if (!(inclusionStatus.Trim() == "P" || inclusionStatus.Trim() == "X"))
                            inclusionStatus = tickFlag;

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@inclusionStatus",
                            SqlValue = inclusionStatus
                        });
                    }

                    if (ethinicityCode != null && ethinicityCode != "")
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@ethinicityCode",
                            SqlValue = ethinicityCode
                        });

                    if (firstLanguageCode != null && firstLanguageCode != "")
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@firstLanguageCode",
                            SqlValue = firstLanguageCode
                        });

                    if (yearGroupCode != null && yearGroupCode != "")
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@yearGroupCode",
                            SqlValue = yearGroupCode
                        });

                    if (subjectID.HasValue && subjectID.Value > 0)
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@subjectID",
                            SqlValue = subjectID
                        });

                    if (qan != "")
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@qan",
                            SqlValue = qan
                        });

                    if (syllabus != "")
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@syllabus",
                            SqlValue = syllabus
                        });
                    if (awardingBodyID.HasValue && awardingBodyID.Value > 0)
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@awardingBodyID",
                            SqlValue = awardingBodyID
                        });
                    if (qanSubjectID.HasValue && qanSubjectID.Value > 0)
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@qanSubjectID",
                            SqlValue = qanSubjectID
                        });

                    if ((subLevelId ?? 0) > 0)
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@subLevelId",
                            SqlValue = subLevelId
                        });

                    if (alphabetForSurname != null && alphabetForSurname != "")
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@alphabetForSurname",
                            SqlValue = alphabetForSurname
                        });

                    int age = 0;
                    if (! int.TryParse(strAge, out age))
                        age = 0;
                    if (age > 0)
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.String,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@strAge",
                            SqlValue = age
                        });

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@FromRow",
                        SqlValue = 0
                    });

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@ToRow",
                        SqlValue = 0
                    });

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Direction = System.Data.ParameterDirection.Output,
                        ParameterName = "@RowCount"
                    });

                    cmd.ExecuteScalar();

                    if (cmd.Parameters["@RowCount"].Value != null)
                        return int.Parse(cmd.Parameters["@RowCount"].Value.ToString());
                    else
                        return 0;
                }
            }
        }

        public static List<StudentWithResult> GetStudentsWithResultsPage(int page, int rowsPerPage, int dcsfNumber, short keyStageID, string forename, string surname, DateTime? dob, DateTime? doa, string gender, string inclusionStatus, string ethinicityCode, string firstLanguageCode, string yearGroupCode, Int32? subjectID, String qan, String syllabus, Int32? awardingBodyID, Int32? qanSubjectID, int? subLevelId, String alphabetForSurname, String sortExpression, String strAge, out int count)
        {
            try
            {
                List<StudentWithResult> result = new List<StudentWithResult>();

                count = 0;

                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        if (!Validation.Common.IsDCSFNumberValid(context, dcsfNumber))
                            throw Web09Exception.GetBusinessException(Web09MessageList.DCSFNumberInvalid);
                        else if (!Validation.Common.IsKeyStageValid(context, keyStageID))
                            throw Web09Exception.GetBusinessException(Web09MessageList.KeystageInvalid);              

                        String strColumn = "";
                        String strDirection = "";

                        if (sortExpression.Contains("SPECIALFORSUBJECTFILTER"))
                        {
                            strColumn = "SPECIALFORSUBJECTFILTER";
                            strDirection = "ASC";
                        }

                        if (sortExpression.Contains("Forename"))
                        {
                            strColumn = "Forename";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("Status"))
                        {
                            strColumn = "Status";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("Session"))
                        {
                            strColumn = "Session";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("ForvusID"))
                        {
                            strColumn = "ForvusID";
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

                        if (sortExpression.Contains("DOB"))
                        {
                            strColumn = "DOB";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("AwardingBody"))
                        {
                            strColumn = "AwardingBody";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("Gender"))
                        {
                            strColumn = "Gender";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("Qualification"))
                        {
                            strColumn = "Qualification";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("Grade"))
                        {
                            strColumn = "Grade";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("QAN"))
                        {
                            strColumn = "QAN";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("NCN"))
                        {
                            strColumn = "NCN";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("Syllabus"))
                        {
                            strColumn = "Syllabus";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("Title"))
                        {
                            strColumn = "Title";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("FineG"))
                        {
                            strColumn = "FineG";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }


                        if (sortExpression.Contains("Mark"))
                        {
                            strColumn = "Mark";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        if (sortExpression.Contains("Age"))
                        {
                            strColumn = "AGE";
                            if (sortExpression.Contains("ASC"))
                                strDirection = "ASC";
                            else
                                strDirection = "DESC";
                        }

                        System.Data.Common.DbConnection connection = conn.StoreConnection;

                        System.Data.Common.DbCommand cmd = connection.CreateCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Student.StudentResults_GetPage";

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int16,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@keyStageID",
                            SqlValue = keyStageID
                        });

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@dcsfNumber",
                            SqlValue = dcsfNumber
                        });

                        if (forename != "")
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@forename",
                                SqlValue = forename
                            });

                        if (surname != "")
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@surname",
                                SqlValue = surname
                            });

                        String strDOB = "";
                        if (dob.HasValue)
                            strDOB = dob.Value.Year.ToString() + dob.Value.Month.ToString("00") + dob.Value.Day.ToString("00");

                        String strDOA = "";
                        if (doa.HasValue)
                            strDOA = doa.Value.Year.ToString() + doa.Value.Month.ToString("00") + doa.Value.Day.ToString("00");

                        if (strDOB != "")
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@dob",
                                SqlValue = strDOB
                            });

                        if (strDOA != "")
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@doa",
                                SqlValue = strDOA
                            });

                        if (gender != "")
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@gender",
                                SqlValue = gender
                            });

                        if (inclusionStatus != null && inclusionStatus.Trim() != "")
                        {
                            string tickFlag = "√";

                            if (!(inclusionStatus.Trim() == "P" || inclusionStatus.Trim() == "X"))
                                inclusionStatus = tickFlag;

                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@inclusionStatus",
                                SqlValue = inclusionStatus
                            });
                        }

                        if (ethinicityCode != null && ethinicityCode != "")
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@ethinicityCode",
                                SqlValue = ethinicityCode
                            });

                        if (firstLanguageCode != null && firstLanguageCode != "")
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@firstLanguageCode",
                                SqlValue = firstLanguageCode
                            });

                        if (yearGroupCode != null && yearGroupCode != "")
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@yearGroupCode",
                                SqlValue = yearGroupCode
                            });

                        if (subjectID.HasValue && subjectID.Value > 0)
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@subjectID",
                                SqlValue = subjectID
                            });

                        if (qan != "")
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@qan",
                                SqlValue = qan
                            });

                        if (syllabus != "")
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@syllabus",
                                SqlValue = syllabus
                            });
                        if (awardingBodyID.HasValue && awardingBodyID.Value > 0)
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@awardingBodyID",
                                SqlValue = awardingBodyID
                            });
                        String QANSubjectTitleToDisplay = "";
                        if (qanSubjectID.HasValue && qanSubjectID.Value > 0)
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@qanSubjectID",
                                SqlValue = qanSubjectID
                            });

                            QANSubjectTitleToDisplay = context.QANSubjects.Where(qs => qs.QANSubjectID == qanSubjectID.Value).FirstOrDefault().SubjectTitle;
                        }

                        if ((subLevelId ?? 0) > 0)
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.Int32,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@subLevelId",
                                SqlValue = subLevelId
                            });

                        if (alphabetForSurname != null && alphabetForSurname != "")
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@alphabetForSurname",
                                SqlValue = alphabetForSurname
                            });

                        int age = 0;
                        if (! int.TryParse(strAge, out age))
                            age = 0;
                        if (age > 0)
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                            {
                                DbType = System.Data.DbType.String,
                                Direction = System.Data.ParameterDirection.Input,
                                ParameterName = "@strAge",
                                SqlValue = age
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
                        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                        TextInfo textInfo = cultureInfo.TextInfo;

                        while (dr.Read())
                        {
                            StudentWithResult obj = new StudentWithResult
                            {
                                DFESNumber = dr.IsDBNull(dr.GetOrdinal("DFESNumber")) ? -1 : int.Parse(dr["DFESNumber"].ToString()),
                                KeyStage = Int16.Parse(dr["KeyStage"].ToString()),
                                StudentID = int.Parse(dr["StudentID"].ToString()),
                                PINCLDisplayFlag = dr.IsDBNull(dr.GetOrdinal("DisplayFlag")) ? "" : dr["DisplayFlag"].ToString(),
                                PINCLDescription = dr.IsDBNull(dr.GetOrdinal("R_INCLDescription")) ? "" : dr["R_INCLDescription"].ToString(),
                                ForvusIndex = dr.IsDBNull(dr.GetOrdinal("ForvusIndex")) ? -1 : int.Parse(dr["ForvusIndex"].ToString()),
                                Forename = dr.IsDBNull(dr.GetOrdinal("Forename")) ? "" : dr["Forename"].ToString(),
                                Surname = dr.IsDBNull(dr.GetOrdinal("Surname")) ? "" : dr["Surname"].ToString(),
                                DOB = dr.IsDBNull(dr.GetOrdinal("DOB")) ? "" : dr["DOB"].ToString(),
                                Gender = dr.IsDBNull(dr.GetOrdinal("Gender")) ? "" : dr["Gender"].ToString(),
                                EthnicityCode = dr.IsDBNull(dr.GetOrdinal("EthnicityCode")) ? "" : dr["EthnicityCode"].ToString(),
                                LanguageCode = dr.IsDBNull(dr.GetOrdinal("FirstLanguageCode")) ? "" : dr["FirstLanguageCode"].ToString(),
                                YearGroupCode = dr.IsDBNull(dr.GetOrdinal("ActualYearGroup")) ? "" : dr["ActualYearGroup"].ToString(),
                                AdmissionDate = dr.IsDBNull(dr.GetOrdinal("ENTRYDAT")) ? "" : dr["ENTRYDAT"].ToString(),
                                ResultID = dr.IsDBNull(dr.GetOrdinal("ResultID")) ? -1 : int.Parse(dr["ResultID"].ToString()),
                                RINCLDisplayFlag = dr.IsDBNull(dr.GetOrdinal("DisplayFlag")) ? "" : dr["DisplayFlag"].ToString(),
                                RINCLDescription = dr.IsDBNull(dr.GetOrdinal("R_INCLDescription")) ? "" : dr["R_INCLDescription"].ToString(),
                                SeasonCode = dr.IsDBNull(dr.GetOrdinal("SeasonCode")) ? "" : dr["SeasonCode"].ToString(),
                                ExamYear = dr.IsDBNull(dr.GetOrdinal("ExamYear")) ? (int?)null : int.Parse(dr["ExamYear"].ToString()),
                                Exam_Date = dr.IsDBNull(dr.GetOrdinal("Exam_Date")) ? null : Convert.ToString(dr["Exam_Date"]),
                                BoardSubjectNumber = dr.IsDBNull(dr.GetOrdinal("BoardSubjectNumber")) ? "" : dr["BoardSubjectNumber"].ToString(),
                                AwardingBodyID = dr.IsDBNull(dr.GetOrdinal("AwardingBodyID")) ? (int?)null : int.Parse(dr["AwardingBodyID"].ToString()),
                                AwardingBodyName = dr.IsDBNull(dr.GetOrdinal("AwardingBodyCode")) ? "" : dr["AwardingBodyCode"].ToString(),
                                QUID = dr.IsDBNull(dr.GetOrdinal("QUID")) ? "" : dr["QUID"].ToString(),
                                QANID = dr.IsDBNull(dr.GetOrdinal("QANID")) ? -1 : int.Parse(dr["QANID"].ToString()),
                                QualificationTypeTitle = dr.IsDBNull(dr.GetOrdinal("QualificationTypeTitle")) ? "" : dr["QualificationTypeTitle"].ToString(),
                                NationalCentreNumber = dr.IsDBNull(dr.GetOrdinal("NationalCentreNumber")) ? null : dr["NationalCentreNumber"].ToString(),
                                SubjectID = dr.IsDBNull(dr.GetOrdinal("SubjectID")) ? (int?)null : int.Parse(dr["SubjectID"].ToString()),
                                SubjectCode = dr.IsDBNull(dr.GetOrdinal("SubjectCode")) ? "" : dr["SubjectCode"].ToString(),
                                SubjectDescription = dr.IsDBNull(dr.GetOrdinal("SubjectDescription")) ? "" : dr["SubjectDescription"].ToString(),
                                CurrentGrade = dr.IsDBNull(dr.GetOrdinal("CurrentGrade")) ? "" : dr["CurrentGrade"].ToString(),
                                OriginalGrade = dr.IsDBNull(dr.GetOrdinal("OriginalGrade")) ? "" : dr["OriginalGrade"].ToString(),
                                ScrutinyStatus = "",
                                Age = dr.IsDBNull(dr.GetOrdinal("Age")) ? "" : dr["Age"].ToString(),
                                FineGrade = dr.IsDBNull(dr.GetOrdinal("FineGrade")) ? "" : dr["FineGrade"].ToString(),
                                CurrentTestMark = dr.IsDBNull(dr.GetOrdinal("CurrentMark")) ? "" : dr["CurrentMark"].ToString(),
                                OriginalTestMark = dr.IsDBNull(dr.GetOrdinal("OriginalMark")) ? "" : dr["OriginalMark"].ToString(),
                                PreviousAge = dr.IsDBNull(dr.GetOrdinal("PreviousAge")) ? null : (int?)Convert.ToInt32(dr["PreviousAge"]),
                                PreviousSurname = dr.IsDBNull(dr.GetOrdinal("PreviousSurname")) ? "" : dr["PreviousSurname"].ToString(),
                                PreviousForename = dr.IsDBNull(dr.GetOrdinal("PreviousForename")) ? "" : dr["PreviousForename"].ToString(),
                                PreviousYearGroup = dr.IsDBNull(dr.GetOrdinal("PreviousYearGroup")) ? "" : dr["PreviousYearGroup"].ToString(),
                                PreviousDOB = dr.IsDBNull(dr.GetOrdinal("PreviousDOB")) ? "" : dr["PreviousDOB"].ToString(),
                                DataOriginDescription = dr.IsDBNull(dr.GetOrdinal("DataOriginDescription")) ? "" : dr["DataOriginDescription"].ToString(),
                                CurrentResultStatusDescription = dr.IsDBNull(dr.GetOrdinal("CurrentResultStatusDescription")) ? "" : dr["CurrentResultStatusDescription"].ToString(),
                                OriginalResultStatusDescription = dr.IsDBNull(dr.GetOrdinal("OriginalResultStatusDescription")) ? "" : dr["OriginalResultStatusDescription"].ToString(),
                                ChangeStatus = dr.IsDBNull(dr.GetOrdinal("ChangeStatus")) ? "" : dr["ChangeStatus"].ToString()
                            };

                            if (obj.RINCLDisplayFlag == "v")
                                obj.RINCLDisplayFlag = "√";

                            if (QANSubjectTitleToDisplay != "")
                            {
                                obj.SubjectCode = "";
                                obj.SubjectDescription = QANSubjectTitleToDisplay;
                                obj.SubjectID = null;
                            }

                            result.Add(obj);
                        }
                        dr.Close();
                        count = Convert.ToInt32(cmd.Parameters["@RowCount"].Value);
                        return result;
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