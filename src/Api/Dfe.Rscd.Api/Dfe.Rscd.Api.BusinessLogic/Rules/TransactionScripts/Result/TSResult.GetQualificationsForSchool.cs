using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
        public static IList<Subjects> GetQualificationsForSchool(Int32 DFESNumber, Int16 Keystage)
        {

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                String strQTCode = "KS" + Keystage.ToString();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    List<Subjects> subjectList = new List<Subjects>();

                    if (Keystage == 1 || Keystage == 2 || Keystage == 3)
                    {
                        // All subjects for each keystage qualificationType
                        var query1 = (from a in context.Subjects
                                      join sl in context.SubLevels on a.SubLevels.SubLevelID equals sl.SubLevelID
                                      join qt in context.QualificationTypes on sl.QualificationTypes.QualificationTypeID equals qt.QualificationTypeID                                      
                                      where qt.QualificationTypeCollections.Any(r=> r.QualificationTypeCollectionCode.StartsWith(strQTCode))
                                      select new { a }).Distinct();

                        var res = query1.ToList();

                        for (int i = 0; i < res.Count; i++)
                        {
                            Subjects s = res[i].a;
                            subjectList.Add(s);
                        }
                    }
                    else
                    {
                        subjectList = GetSubjectListKS45(Keystage, DFESNumber);
                    }

                    return subjectList;
                }
            }
        }

        // QC 3494 
        private static List<Subjects> GetSubjectListKS45(Int16 keyStage, Int32 dfesNumber)
        {
            List<Subjects> subjectList = new List<Subjects>();

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();
                System.Data.Common.DbConnection connection = conn.StoreConnection;
                System.Data.Common.DbCommand cmd = connection.CreateCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Result.GetQualificationsForSchool";

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                {
                    DbType = System.Data.DbType.Int16,
                    Direction = System.Data.ParameterDirection.Input,
                    ParameterName = "@KeyStageID",
                    SqlValue = keyStage
                });

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                {
                    DbType = System.Data.DbType.Int32,
                    Direction = System.Data.ParameterDirection.Input,
                    ParameterName = "@DCSFNumber",
                    SqlValue = dfesNumber
                });

                using (System.Data.Common.DbDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Subjects newSubjects = new Subjects();                            
                        
                        int subLevelID        = Convert.ToInt32(dr[7]);
                        string subLevelCode   = Convert.ToString(dr[8]);
                        string subLevelDescription = Convert.ToString(dr[0]);
                        newSubjects.SubLevels = SubLevels.CreateSubLevels(subLevelID, subLevelCode);
                        newSubjects.SubLevels.SubLevelDescription = subLevelDescription;

                        newSubjects.SubjectDescription = string.Format("{0} - {1}", newSubjects.SubLevels.SubLevelDescription, Convert.ToString(dr[1]));
                        newSubjects.LongDescription    = Convert.ToString(dr[2]);
                        newSubjects.MFL                = Convert.ToBoolean(dr[3]);
                        if (!dr.IsDBNull(4))
                        {
                            newSubjects.ParentSubjectID = Convert.ToInt32(dr[4]);
                        }

                        newSubjects.SubjectCode              = Convert.ToString(dr[5]);
                        newSubjects.SubjectID                = Convert.ToInt32(dr[6]);

                        subjectList.Add(newSubjects);
                    }
                }
                
            }

            return subjectList;
        }

    }
}
