using System.Data.EntityClient;
using System.Linq;
using System.Transactions;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
        public static int AddQualifications(string QAN, string syllabus, string awardingbody, int year, short keystage, string subjectTitle)
        {
            int newQANSubjectId = -1;

            using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope())
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        //get the awarding body
                        int awaId;
                        AwardingBodies awa = null;
                        if (int.TryParse(awardingbody, out awaId))
                        {
                            int awardingBodyId = awaId;
                            awa = (from a in context.AwardingBodies
                                   where a.AwardingBodyID == awardingBodyId
                                   select a).First();
                        }
                        else
                        {
                            //it could be awacode or awa name
                            var queryA = (from a in context.AwardingBodies
                                          where a.AwardingBodyCode.Equals(awardingbody)
                                          select a).ToList();

                            if (queryA.Count > 0)
                            {
                                awa = queryA.First();
                            }
                            else
                            {
                                var queryA1 = (from a in context.AwardingBodies
                                               where a.AwardingBodyName.Equals(awardingbody)
                                               select a).ToList();

                                if (queryA1.Count > 0)
                                {
                                    awa = queryA1.First();
                                }
                            }
                        }

                        QualificationTypes qtype = null;
                        SubLevels slevel = null;                        
                        QANs qan = null;

                        QualificationTypeCollections qtc = null;

                        if(keystage == 4)
                        {
                            qtc = (from qtc1 in context.QualificationTypeCollections
                                   where qtc1.QualificationTypeCollectionCode == "KS4_Equivalences"
                                   select qtc1).First();

                            //get unknown sublevel
                            var levelQuery = (from l in context.SubLevels
                                              where l.SubLevelDescription == "UNKNOWN KS4 SUBLEVEL"
                                              select l).ToList();

                            if (levelQuery.Count <= 0)
                                throw new BusinessLevelException("Unrecognized qualification could not be added to the database.");
                            else
                                slevel = levelQuery.First();

                            //select the unknown QT that belongs to the keystage
                            var qtypeQuery = (from qt in context.QualificationTypes
                                              where qt.QualificationTypeDescription == "Unknown KS4 Qualification"
                                              select qt).ToList();

                            if (qtypeQuery.Count <= 0)
                                throw new BusinessLevelException("Unrecognized qualification could not be added to the database.");
                            else
                                qtype = qtypeQuery.First();
                         }

                         if(keystage == 5)
                         {
                             qtc = (from qtc1 in context.QualificationTypeCollections
                                    where qtc1.QualificationTypeCollectionCode == "KS5_Equivalences"
                                    select qtc1).First();

                             //get unknown sublevel
                             var levelQuery = (from l in context.SubLevels
                                               where l.SubLevelDescription == "UNKNOWN KS5 SUBLEVEL"
                                               select l).ToList();

                             if (levelQuery.Count <= 0)
                                 throw new BusinessLevelException("Unrecognized qualification could not be added to the database.");
                             else
                                 slevel = levelQuery.First();

                             //select the unknown QT that belongs to the keystage
                             var qtypeQuery = (from qt in context.QualificationTypes
                                               where qt.QualificationTypeDescription == "Unknown KS5 Qualification"
                                               select qt).ToList();

                             if (qtypeQuery.Count <= 0)
                                 throw new BusinessLevelException("Unrecognized qualification could not be added to the database.");
                             else
                                 qtype = qtypeQuery.First();
                         }                        
                       

                        //is syllabus is specified use it alone, since UI does the same, emptyout the QAN
                        //use dummy 'unknown' QAN, only if QAN isnt provided
                         if (!string.IsNullOrEmpty(syllabus) && string.IsNullOrEmpty(QAN))
                        {
                            //get the dummy QAN, Sublevel, QualificationType
                            var qanQuery = (from q in context.QANs
                                            where q.QUID == "UNKNOWN"
                                                    && q.QualificationTypes.QualificationTypeID == qtype.QualificationTypeID
                                            select q).ToList();

                            if (qanQuery.Count <= 0)
                                throw new BusinessLevelException("Unrecognized qualification could not be added to the database.");
                            else
                            {
                                qan = qanQuery.First();
                                qan.QualificationTypesReference.Load();                                                                                                                                                              
                            }
                        }
                        else
                        {
                            //add a new row in QANs table
                            QANs newQan = new QANs();

                            newQan.QUID = QAN;
                            newQan.HasPoints = false;
                            newQan.QualificationTypes = qtype;
                            context.AddToQANs(newQan);

                            qan = newQan;
                        }

                        //now create the new QANSubjects and subjects entry
                        Subjects subject = new Subjects();
                        subject.SubjectCode = "0000";
                        subject.SubjectDescription = subjectTitle;
                        subject.LongDescription = subjectTitle;
                        context.AddToSubjects(subject);

                        QANSubjects qanSubject = new QANSubjects();
                        if (keystage == 4)
                        {
                            qanSubject.KS4Main = true;
                            qanSubject.APR15 = 1;
                        }
                        else if (keystage == 5)
                        {
                            qanSubject.KS5Main = true;
                            qanSubject.APR1618 = 1;
                        }
                        if (awa != null)
                            qanSubject.AwardingBodies = awa;

                        qanSubject.BoardSubjectNumber = syllabus;
                        qanSubject.LevelDescription = slevel.SubLevelDescription;
                        qanSubject.MappTitle = subjectTitle;
                        qanSubject.QANs = qan;
                        qanSubject.QualificationTypeCode = qtype.QualificationTypeCode;
                        qanSubject.Subjects = subject;
                        qanSubject.SubjectTitle = subjectTitle;
                        qanSubject.YearStart = year;
                        qanSubject.YearDeleted = year + 1;

                        context.AddToQANSubjects(qanSubject);
                        context.SaveChanges();
                        newQANSubjectId = qanSubject.QANSubjectID;
                    }
                }

                transaction.Complete();
            }
            return newQANSubjectId;
        }
    }
}
