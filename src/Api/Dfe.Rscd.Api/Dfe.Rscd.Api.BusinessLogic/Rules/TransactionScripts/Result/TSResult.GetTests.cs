using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
        public static IList<Subjects> GetTests(short keyStageID)
        {

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    List<Subjects> subjectList = new List<Subjects>();                 

                    var connection = conn.StoreConnection;

                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[Result].[GetTests]";
                        SetInputParamForCommand(cmd, "keyStage", keyStageID);

                        using (System.Data.Common.DbDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Subjects newSubject = new Subjects();

                                newSubject.SubjectID          = Convert.ToInt32(dr["SubjectID"]);
                                newSubject.SubjectCode        = Convert.ToString(dr["SubjectCode"]);
                                newSubject.MFL                = (Convert.ToInt32(dr["SubjectCode"]) == 1 );
                                newSubject.SubjectDescription = Convert.ToString(dr["SubjectDescription"]);
                                newSubject.LongDescription    = Convert.ToString(dr["LongDescription"]);

                                subjectList.Add(newSubject);
                            }
                            dr.Close();
                        }

                    }

                    return subjectList;
                }
            }
        }

    }
}
