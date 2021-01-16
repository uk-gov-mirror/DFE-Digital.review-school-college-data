using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Text.RegularExpressions;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
        public static IList<QANSubjects> GetQualificationsAll(string QAN, string syllabus, Int16 awardingBodyID, Int16 keystage, Int16 year)
        {

            //reg exp to remove all non-alphanumeric characters 
            Regex regNonAlphaNum = new Regex(@"[\W_]");
            //reg exp to remove all leading zeros
            Regex regLeadingZeros = new Regex("^[0]+");
            string matchingSyllabus = syllabus;

            // prepare for "fuzzy" logic search on syllabus, if it is provided
            if (!String.IsNullOrEmpty(syllabus))
            {
                matchingSyllabus = regLeadingZeros.Replace(regNonAlphaNum.Replace(matchingSyllabus, ""), "");
            }


            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();
                
                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    bool isJuneChecking = TSResult.IsJuneChecking(context);

                    if (isJuneChecking)
                    {                       
                        var queryJune = context.QANSubjects.Include("QANs").Include("AwardingBodies")
                                .Join(
                                        context.QualificationTypes,
                                        s => s.QualificationTypeCode,
                                        qt => qt.QualificationTypeCode,
                                        (s, qt) => new { qt, s }).Join(context.SubLevels.Where(sl => sl.SubLevelCode.CompareTo("831") == -1 && sl.SubLevelCode.CompareTo("800") == 1),
                                                                      o => o.qt.QualificationTypeID,
                                                                      sl => sl.QualificationTypes.QualificationTypeID,
                                                                      (o, sl) => new { sl, o });

                        if (!keystage.Equals(string.Empty))
                        {
                            //Defect #2447: (3) The qualification is "unrecognized" if search fails to find at least one reference 
                            //data qualification (excluding any unrecognized qualifications in the database) 
                            //where the validity flags for KS4 or KS5 (KS4Main or KS5Main) are set 
                            //and the year entered is in the approved range (YearStart to YearDeleted). 
                            //(The approval flags APR15 and APR1618 are not to be considered) .

                            queryJune = queryJune.Where(r => (r.o.s.KS4Main == true /*&& r.o.s.APR15 == 1*/ && keystage == 4)
                                            ||
                                            (r.o.s.KS5Main == true /*&& r.o.s.APR1618 == 1*/ && keystage == 5)
                                        );
                        }

                        if (!QAN.Equals(string.Empty))
                        {
                            queryJune = queryJune.Where(r => r.o.s.QANs.QUID == QAN);
                        }

                        if (awardingBodyID != 0)
                        {
                            queryJune = queryJune.Where(r => r.o.s.AwardingBodies.AwardingBodyID == awardingBodyID);
                        }

                        var resultsJune = queryJune.OrderByDescending(r => r.o.s.SubjectTitle).ToList();


                        
                        if (!syllabus.Equals(string.Empty))
                        {
                            //use regex for the BoardSubjectNumber to perform fuzzy logic search on syllabus 
                            var resultsJuneWithSyllabus = resultsJune.Where(r => regLeadingZeros.Replace(regNonAlphaNum.Replace(r.o.s.BoardSubjectNumber, ""), "").ToLower().StartsWith(matchingSyllabus.ToLower())).Distinct();

                            IList<QANSubjects> qanSubjectsJuneWithSyllabus = new List<QANSubjects>();
                            for (int i = 0; i < resultsJuneWithSyllabus.Count(); i++)
                            {
                                QANSubjects a = resultsJuneWithSyllabus.ElementAt(i).o.s;

                                if (!a.QualificationTypeCode.ToLower().Contains("unknown"))
                                {
                                    a.AwardingBodiesReference.Load();
                                    a.QANsReference.Load();
                                    a.SubjectsReference.Load();
                                    qanSubjectsJuneWithSyllabus.Add(a);
                                }
                            }
                            return qanSubjectsJuneWithSyllabus;
                        }

                        //if syllabus is not in the picture
                        IList<QANSubjects> qanSubjectsJune = new List<QANSubjects>();
                        for (int i = 0; i < resultsJune.Count; i++)
                        {
                            QANSubjects a = resultsJune[i].o.s;
                            if (!a.QualificationTypeCode.ToLower().Contains("unknown"))
                            {
                                a.AwardingBodiesReference.Load();
                                a.QANsReference.Load();
                                a.SubjectsReference.Load();
                                qanSubjectsJune.Add(a);
                            }
                        }

                        return qanSubjectsJune;
                    }

                    //var query2 = (from qsub in context.QANSubjects
                    //              join qt in context.QualificationTypes on qsub.QualificationTypeCode equals qt.QualificationTypeCode
                    //              where (syllabus=="" || qsub.BoardSubjectNumber == syllabus) &&
                    //              qsub.YearStart <= year &&
                    //              qsub.YearDeleted >= year &&
                    //              (
                    //                 (qsub.KS4Main == true && qsub.APR15 == 1 && keystage == 4)
                    //                 ||
                    //                 (qsub.KS5Main == true && qsub.APR1618 == 1 && keystage == 5)
                    //              ) &&
                    //              (QAN=="" || qsub.QANs.QUID == QAN) &&
                    //              (awardingBodyID==0 || qsub.AwardingBodies.AwardingBodyID == awardingBodyID)
                    //              select new { qsub , qsub.AwardingBodies, qsub.QANs, qsub.Subjects}).OrderBy(r => r.qsub.SubjectTitle);

                    //List<QANSubjects> results = new List<QANSubjects>();

                    //foreach (var res in query2.ToList())
                    //{
                    //    QANSubjects qsub = res.qsub;
                    //    qsub.AwardingBodies = res.AwardingBodies;
                    //    qsub.QANs = res.QANs;
                    //    qsub.Subjects = res.Subjects;
                    //    results.Add(qsub);
                    //}

                    //return results;

                    IQueryable<QANSubjects> querySept = null;
                    querySept = context.QANSubjects.Include("QANs").Include("AwardingBodies");

                    if (!QAN.Equals(string.Empty))
                    {
                        querySept = querySept.Where(r => r.QANs.QUID == QAN);
                    }

                    if (awardingBodyID != 0)
                    {
                        querySept = querySept.Where(r => r.AwardingBodies.AwardingBodyID == awardingBodyID);
                    }
                    
                    querySept = querySept.Where(r => (r.YearStart ?? 0) <= year && (r.YearDeleted ?? 9999) >= year);

                    if (!keystage.Equals(string.Empty))
                    {
                        querySept = querySept.Where(r => (r.KS4Main == true && r.APR15 == 1 && keystage == 4)
                                        ||
                                        (r.KS5Main == true && r.APR1618 == 1 && keystage == 5)
                                    );
                    }

                    var resultsSept = querySept.OrderByDescending(r => r.SubjectTitle).ToList();

                    if (!syllabus.Equals(string.Empty))
                    {
                        //use regex for the BoardSubjectNumber to perform fuzzy logic search on syllabus 
                        var resultsSeptWithSyllabus = resultsSept.Where(r => regLeadingZeros.Replace(regNonAlphaNum.Replace(r.BoardSubjectNumber, ""), "").ToLower().StartsWith(matchingSyllabus.ToLower())).Distinct();

                        IList<QANSubjects> qanSubjectsSeptWithSyllabus = new List<QANSubjects>();
                        for (int i = 0; i < resultsSeptWithSyllabus.Count(); i++)
                        {
                            QANSubjects a = resultsSeptWithSyllabus.ElementAt(i);
                            if (!a.QualificationTypeCode.ToLower().Contains("unknown"))
                            {
                                a.AwardingBodiesReference.Load();
                                a.QANsReference.Load();
                                a.SubjectsReference.Load();
                                qanSubjectsSeptWithSyllabus.Add(a);
                            }
                        }

                        resultsSept = qanSubjectsSeptWithSyllabus.ToList();
                    }
                    else
                    {
                        IList<QANSubjects> qanSubjectsSept = new List<QANSubjects>();
                        for (int i = 0; i < resultsSept.Count(); i++)
                        {
                            QANSubjects a = resultsSept.ElementAt(i);
                            if (!a.QualificationTypeCode.ToLower().Contains("unknown"))
                            {
                                a.AwardingBodiesReference.Load();
                                a.QANsReference.Load();
                                a.SubjectsReference.Load();
                                qanSubjectsSept.Add(a);
                            }
                        }

                        resultsSept = qanSubjectsSept.ToList();
                    }

                    return resultsSept;

                }
            }
        }

    }
}
