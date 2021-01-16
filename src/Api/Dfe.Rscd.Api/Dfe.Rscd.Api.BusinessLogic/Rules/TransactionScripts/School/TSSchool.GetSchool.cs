using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSSchool : Logic.TSBase
    {

        public static Schools GetSchool(int schoolDfesNumber)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();
                
                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    var Query = context.SchoolCheckingStatus
                               .Where(scs => scs.Schools.DFESNumber == schoolDfesNumber)
                               .Select(scs => new
                               {
                                   School = scs.Schools,
                                   SchoolGroups = scs.Schools.SchoolGroups,
                                   SchoolCheckingStatus = scs,
                                   KeyStages = scs.Cohorts
                               });

                    Query = Query.Where(q1 => (q1.School.LowestAge < 16)
                                                           || (q1.SchoolCheckingStatus.Cohorts.KeyStage == 5 && q1.School.LowestAge >= 16));
                    var schoolQuery = Query.ToList();

                    if (schoolQuery.Count > 0)
                    {
                        Schools school = schoolQuery[0].School;
                        school.SchoolGroups.Attach(schoolQuery[0].SchoolGroups);


                        foreach (SchoolGroups sg in school.SchoolGroups)
                        {
                            var schoolGroupTypesQuery = context.SchoolGroups
                                .Where(sgType => sgType.SchoolGroupID == sg.SchoolGroupID)
                                .Select(sgType => new { SchoolGroupType = sgType.SchoolGroupTypes })
                                .ToList();

                            sg.SchoolGroupTypes = schoolGroupTypesQuery[0].SchoolGroupType;
                        }

                        if (school.LowestAge == 16)
                        {
                            if (schoolQuery[0].SchoolCheckingStatus.Cohorts.KeyStage != 4)
                            {
                                school.SchoolCheckingStatus.Attach(schoolQuery[0].SchoolCheckingStatus);
                            }
                        }
                        else
                        {
                            school.SchoolCheckingStatus.Attach(schoolQuery[0].SchoolCheckingStatus);
                        }
                        school.SchoolStatusReference.Load();
                        return school;
                    }
                    else
                        throw new BusinessLevelException("The school is not valid in the database");
                }
            }
        }

        internal static short? GetSchoolMinimumAge(Web09_Entities context, int schoolDfesNumber)
        {
            return context.Schools.Where(s => s.DFESNumber == schoolDfesNumber).Select(s => s.LowestAge).First();
        }
       
        internal static short? GetSchoolMaximumAge(Web09_Entities context, int schoolDfesNumber)
        {
            return context.Schools.Where(s => s.DFESNumber == schoolDfesNumber).Select(s => s.HighestAge).First();
        }






    }
}
