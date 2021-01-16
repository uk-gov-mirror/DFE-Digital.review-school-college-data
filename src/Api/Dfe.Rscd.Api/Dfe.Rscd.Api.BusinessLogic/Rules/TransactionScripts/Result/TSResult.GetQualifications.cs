using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Text.RegularExpressions;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
        public static IList<QANSubjects> GetQualifications(string QAN, string syllabus, string awardingbodyName, int? year, short keystage)
        {
            IList<QANSubjects> retList = DoGetQualifications(QAN, syllabus, awardingbodyName, year, keystage);

            if (retList == null || retList.Count == 0)
            {
                if (!string.IsNullOrEmpty(syllabus) && TSResult.LooksLikeQan(syllabus))
                {
                    QAN = syllabus;
                    syllabus = string.Empty;
                    retList = DoGetQualifications(QAN, syllabus, awardingbodyName, year, keystage);
                }
            }

            return retList;
        }

        private static IList<QANSubjects> DoGetQualifications(string QAN, string syllabus, string awardingbodyName, int? year, short keystage)
        {
            if (!string.IsNullOrEmpty(QAN))
            {
                QAN = TSResult.CleanQan(QAN);
            }

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {

                    IList<QANSubjects> retVal = new List<QANSubjects>();

                    int awardingBodyID;
                    AwardingBodies awardingBody = TSResult.GetAwardingBody(context, awardingbodyName, out awardingBodyID);

                    bool isJuneChecking = TSResult.IsJuneChecking(context);

                    #region isJuneChecking
                    if (isJuneChecking)
                    {
                        var queryJune = context.QANSubjects
                                .Include("QANs")
                                .Include("AwardingBodies")
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
                            queryJune = queryJune.Where(r => (r.o.s.KS4Main == true && r.o.s.APR15 == 1 && keystage == 4)
                                            ||
                                            (r.o.s.KS5Main == true && r.o.s.APR1618 == 1 && keystage == 5)
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

                        if (year.HasValue)
                        {
                            queryJune = queryJune.Where(r => (r.o.s.YearStart ?? 0) <= year && (r.o.s.YearDeleted ?? 9999) >= year);
                        }

                        var resultsJune = queryJune.OrderByDescending(r => r.o.s.SubjectTitle).ToList();


                        //regex for the syllabus since we have regexed the incoming text
                        if (!syllabus.Equals(string.Empty))
                        {
                            string patternSyllabus = @"[\W_]";
                            Regex rgxSyllabus = new Regex(patternSyllabus);

                            var resultsJuneWithSyllabus = resultsJune.Where(r => rgxSyllabus.Replace(r.o.s.BoardSubjectNumber, "").StartsWith(syllabus)).Distinct();

                            IList<QANSubjects> qanSubjectsJuneWithSyllabus = new List<QANSubjects>();
                            for (int i = 0; i < resultsJuneWithSyllabus.Count(); i++)
                            {
                                QANSubjects a = resultsJuneWithSyllabus.ElementAt(i).o.s;

                                if (!a.QualificationTypeCode.ToLower().Contains("unknown"))
                                {
                                    a.AwardingBodiesReference.Load();
                                    a.QANsReference.Load();
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
                    } //  if (isJuneChecking)
                    #endregion

                    var resultsSept = GetSeptemberResults(QAN, syllabus, context, awardingBodyID);

                    return resultsSept;
                }
            }
        }

        private static List<QANSubjects> GetSeptemberResults(string QAN, string syllabus, Web09_Entities context, int awardingBodyID)
        {
            IQueryable<QANSubjects> querySept = null;
            querySept = context.QANSubjects
                    .Include("QANs")
                    .Include("Subjects")
                    .Include("AwardingBodies");

            if (!QAN.Equals(string.Empty))
            {
                querySept = querySept.Where(r => r.QANs.QUID == QAN);
            }

            if (awardingBodyID != 0 && QAN.Equals(string.Empty))
            {
                querySept = querySept.Where(r => r.AwardingBodies.AwardingBodyID == awardingBodyID);
            }

            querySept = querySept.Where(r => !r.QualificationTypeCode.ToLower().Contains("unknown"));

            var resultsSept = querySept.OrderByDescending(r => r.SubjectTitle).ToList();

            if (!syllabus.Equals(string.Empty) && QAN.Equals(string.Empty))
            {
                string patternSyllabus = @"[\W_]";
                Regex rgxSyllabus = new Regex(patternSyllabus);

                Regex leadingZeros = new Regex("^[0]+");

                var resultsSeptWithSyllabus = resultsSept.Where(r => leadingZeros.Replace(rgxSyllabus.Replace(r.BoardSubjectNumber, ""), "").ToLower().StartsWith(syllabus.ToLower())).Distinct();

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
            return resultsSept;
        }

        private static AwardingBodies GetAwardingBody(Web09_Entities context, string awardingbodyName, out int awardingBodyID)
        {
            AwardingBodies awardingBody = null;
            awardingBodyID = 0;

            if (int.TryParse(awardingbodyName, out awardingBodyID))
            {
                int searchID = awardingBodyID;
                awardingBody = (from a in context.AwardingBodies where a.AwardingBodyID == searchID select a).First();
            }
            else
            {
                //it could be Awarding body code or Awarding body name
                var queryAwardingBodyCode = (from a in context.AwardingBodies where a.AwardingBodyCode.Equals(awardingbodyName) select a).ToList();

                if (queryAwardingBodyCode.Count > 0)
                {
                    awardingBody = queryAwardingBodyCode.First();
                }
                else
                {
                    var queryAwardingBody = (from a in context.AwardingBodies where a.AwardingBodyName.Equals(awardingbodyName) select a).ToList();
                    if (queryAwardingBody.Count > 0)
                    {
                        awardingBody = queryAwardingBody.First();
                    }
                }
            }

            return awardingBody;
        }

        // TFS 16340
        private static string CleanQan(string dirtyQan)
        {
            string cleanQan = dirtyQan;

            if (!string.IsNullOrEmpty(dirtyQan))
            {
                // Remove any characters before the ( final ) hyphen
                int lastIndexOfDash = cleanQan.LastIndexOf('-');
                if (lastIndexOfDash > -1)
                {
                    cleanQan = cleanQan.Substring(lastIndexOfDash + 1);
                }

                // Remove any non alphanumeric characters
                Regex rgx = new Regex("[^a-zA-Z0-9]");
                cleanQan = rgx.Replace(cleanQan, "");

                // Truncate to eight characters
                if (cleanQan.Length > 8)
                {
                    cleanQan = cleanQan.Substring(0, 8);
                }
            }

            return cleanQan;
        }

        // TFS 16340
        private static bool LooksLikeQan(string qanCandidate)
        {
            bool looksLikeQan = false;

            Regex rgx = new Regex("^[a-zA-Z0-9][0-9]{6,6}[X|(0-9)]$");
            looksLikeQan = rgx.IsMatch(qanCandidate);

            return looksLikeQan;
        }

    }

   
}
