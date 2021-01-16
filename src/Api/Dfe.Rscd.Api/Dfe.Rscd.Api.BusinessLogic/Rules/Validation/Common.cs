using System.Linq;

using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.Validation
{
    public class Common : ValidationBase
    {
        public static bool IsDCSFNumberValid(Web09_Entities context, int DCSFNumber)
        {
            return context.Schools.Any(s => s.DFESNumber == DCSFNumber);
        }

        public static bool IsKeyStageValid(Web09_Entities context, short KeyStageID)
        {
            return context.Cohorts.Any(c => c.KeyStage == KeyStageID);
        }

        public static bool IsSchoolGroupValid(Web09_Entities context, short SchoolGroupID)
        {
            return context.SchoolGroups.Any(c => c.SchoolGroupID == SchoolGroupID);
        }

        public static bool IsQANValid(Web09_Entities context, string QAN)
        {
            return context.QANs.Any(c => c.QUID == QAN);
        }

        public static bool IsSyllabusValid(Web09_Entities context, string syllabus)
        {
            return context.QANSubjects.Any(c => c.BoardSubjectNumber == syllabus);
        }

        public static bool IsAwardingBodyValid(Web09_Entities context, string awardingbody)
        {
            return context.AwardingBodies.Any(c => c.AwardingBodyName == awardingbody);
        }

        public static bool IsExamYearValid(Web09_Entities context, int year)
        {
            return context.CohortSubCohortExamYears.Any(y => y.ExamYear == year);
        }

        public static bool IsExamSeasonValid(Web09_Entities context, string seasoncode)
        {
            return context.Seasons.Any(s => s.SeasonCode == seasoncode);
        }
    }
}
