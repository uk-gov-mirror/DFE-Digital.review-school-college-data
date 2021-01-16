using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;
using Web09.Services.Common;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
        public static IList<AwardingBodies> GetAwardingBodies()
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return GetAwardingBodies(context);
                }
            }
        }

        public static IList<AwardingBodies> GetAwardingBodies(Web09_Entities context)
        {
            var query = (from a in context.AwardingBodies
                         join s in context.QANSubjects on a.AwardingBodyID equals s.AwardingBodies.AwardingBodyID
                         where (s.KS4Main == true)
                         select new { a });
            query = query.Distinct();

            bool isJune = IsJuneChecking(context);

            if (isJune)
            {
                query = query.Where(a => a.a.DoesGradedExams == true);
            }

            var results = query.OrderBy(b => b.a.AwardingBodyCode).ToList();

            IList<AwardingBodies> awardingBodies = new List<AwardingBodies>();
            for (int i = 0; i < results.Count; i++)
            {
                AwardingBodies a = results[i].a;
                awardingBodies.Add(a);
            }

            return awardingBodies;
        }

        /// <summary>
        /// Method used to indicate whether the site is configured for June mode or not
        /// </summary>
        /// <param name="context">The Web09 Entity Framework object context</param>
        /// <returns>Boolean where true indicates that the site is indeed in June checking mode.</returns>
        internal static bool IsJuneChecking(Web09_Entities context)
        {
            bool isJuneChecking;

            string _webSiteMode = context.CohortConfiguration
                 .Where(cc => cc.ConfigurationCode == "WebsiteMode")
                 .Select(cc => cc.ConfigurationValue)
                 .FirstOrDefault();

            if (string.IsNullOrEmpty(_webSiteMode))
            {
                isJuneChecking = false;
            }
            else
            {
                isJuneChecking = WebSiteModeHelper.IsJuneCheckingExercise(_webSiteMode);
            }

            return isJuneChecking;
        }
    }
}

