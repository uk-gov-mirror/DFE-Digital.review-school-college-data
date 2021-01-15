using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
        public static IList<Points> GetGrades(int qualificationId, short keyStageID, short studentKeyStageID)
        {

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    if (!Validation.Common.IsKeyStageValid(context, keyStageID))
                        throw Web09Exception.GetBusinessException(Web09MessageList.KeystageInvalid);                                                         

                    IQueryable<Points> query = null;

                    if (keyStageID >= 4)
                    {
                        bool isJune = TSResult.IsJuneChecking(context);

                        if (isJune)
                        {
                            var query2 = (from qt in context.QualificationTypes
                                          join p in context.Points on qt.QualificationTypeID equals p.QualificationTypes.QualificationTypeID
                                          join a in context.QANSubjects on p.QualificationTypes.QualificationTypeCode equals a.QualificationTypeCode
                                          join sl in context.SubLevels on qt.QualificationTypeID equals sl.QualificationTypes.QualificationTypeID
                                          where sl.SubLevelCode.CompareTo("831") == -1
                                             && sl.SubLevelCode.CompareTo("800") == 1
                                             && a.QANSubjectID == qualificationId
                                          select new { p });
                            query2 = query2.Distinct();

                            var res = query2.ToList();

                            IList<Points> points = new List<Points>();
                            for (int i = 0; i < res.Count; i++)
                            {
                                Points a = res[i].p;
                                points.Add(a);
                            }

                            return points;
                        }

                        IQueryable<QANSubjects> query1 = context.QANSubjects
                                                                     .Include("QANs")                                                                     
                                                                     .Where(r =>
                                                                         r.QANSubjectID == qualificationId);

                        QANSubjects qualification = query1.First();

                        IQueryable<QualificationTypes> q = context.QualificationTypes
                                                .Where(qt => qt.QualificationTypeCode == qualification.QualificationTypeCode);
                        QualificationTypes qtype = q.First();
                        if (qtype.Points_Table == 3)
                        {
                            query = context.Points
                                    .Include("QANs")
                                    .Where(r => r.QANs.QANID == qualification.QANs.QANID);
                        }
                        else if (qtype.Points_Table == 2)
                        {
                            query = context.Points
                                     .Include("QANs")
                                     .Where(r => r.QualificationTypes.QualificationTypeID == qtype.QualificationTypeID
                                     && r.QANs == null);
                        }
                        else
                        {
                            query = context.Points
                                     .Include("QANs")
                                     .Where(r => r.QualificationTypes.QualificationTypeID == qtype.QualificationTypeID);
                        }
                    }
                    else
                    {
                        string keystageString = "KS" + keyStageID.ToString();

                        IQueryable<SubLevels> q = context.SubLevels
                                                .Include("QualificationTypes")  
                                                .Where( s => s.SubLevelDescription.Contains(keystageString));
                        SubLevels sublevel = q.First();
                       
                        query = context.Points
                                .Include("QualificationTypes")
                                .Where(r => r.QualificationTypes.QualificationTypeID == sublevel.QualificationTypes.QualificationTypeID);
                            
                         //exclude grades D, Y and Z if the result is for KS2 or KS3 and it is the students
                        //current keystage
                        if(studentKeyStageID == keyStageID && (studentKeyStageID == 2 || studentKeyStageID == 3))
                        {
                            query = query.Where(r => r.GradeCode != "D" && r.GradeCode != "Y" && r.GradeCode != "Z");
                        }
                                    
                    }

                    var results = query.OrderByDescending(r => r.GradeCode)
                                            .ToList();

                    return results;
                }
            }
        }
               

        //another version used in EditTesults page
        public static IList<Points> GetGrades(int QANID, int qualificationTypeID, short studentKeyStage)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {

                    IQueryable<QualificationTypes> q = context.QualificationTypes
                                                .Include("SubLevels")
                                                .Where(qt => qt.QualificationTypeID == qualificationTypeID);
                    QualificationTypes qtype = q.First(); 
                    qtype.SubLevels.Load();
                    SubLevels sl = qtype.SubLevels.First();

                    IQueryable<Points> query = null;

                    query = context.Points
                            .Include("QANs")
                            .Where(r => r.QualificationTypes.QualificationTypeID == qualificationTypeID);

                    query = query.Distinct();

                    if (QANID != -1 && qtype.Points_Table == 3)
                    {
                        query = query.Where(r => r.QANs.QANID == QANID);
                    }
                                        
                    query = query.Distinct();
                
                    var results = query.OrderByDescending(r => r.GradeCode)
                                            .ToList();                    

                    return results;
                }
            }
        }
    }
}
