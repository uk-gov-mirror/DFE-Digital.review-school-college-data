using System;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSCohort : Logic.TSBase
    {
        public static int GetCohortDetailsCount(int dcsfNumber, short? keyStageID, string forename, string surname, DateTime? dob, DateTime? doa, string gender, string inclusionStatus, string ethinicityCode, string firstLanguageCode, string yearGroupCode, string aphabetForSurname, String strAge)
        {
            try
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        if (!Validation.Common.IsDCSFNumberValid(context, dcsfNumber))
                            throw Web09Exception.GetBusinessException(Web09MessageList.DCSFNumberInvalid);                        
                        else if (keyStageID.HasValue && !Validation.Common.IsKeyStageValid(context, keyStageID.Value))
                            throw Web09Exception.GetBusinessException(Web09MessageList.KeystageInvalid);

                        String strDOB = "";
                        if (dob.HasValue)
                            strDOB = dob.Value.Year.ToString() + dob.Value.Month.ToString("00") + dob.Value.Day.ToString("00");

                        String strDOA = "";
                        if (doa.HasValue)
                            strDOA = doa.Value.Year.ToString() + doa.Value.Month.ToString("00") + doa.Value.Day.ToString("00");

                        int age = 0;
                        if (!int.TryParse(strAge, out age))
                            age = 0;

                        var query = from s in context.Students
                                    where s.Schools.DFESNumber == dcsfNumber
                                    && s.Cohorts.KeyStage == keyStageID
                                    && s.PINCLs != null
                                    && s.StudentChanges.Any()
                                    select new
                                    {
                                        Students = s,
                                        LatestStudentChanges = ((System.Data.Objects.ObjectQuery<StudentChanges>)(from w in s.StudentChanges where w.DateEnd == null select w).AsQueryable()).Include("YearGroups").Include("Ethnicities").Include("Languages").Include("SENStatus").FirstOrDefault(),
                                        OriginalStudentChanges = ((System.Data.Objects.ObjectQuery<StudentChanges>)s.StudentChanges.OrderBy(ob => ob.Changes.ChangeID).AsQueryable()).Include("YearGroups").Include("Ethnicities").Include("Languages").Include("SENStatus").FirstOrDefault(),
                                        PINCL = s.PINCLs,
                                        Cohort = s.Cohorts
                                    };
                        if (forename != null && forename.Trim() != "")
                            query = query.Where(r => r.LatestStudentChanges.Forename.ToLower().StartsWith(forename.ToLower()));
                        if (surname != null && surname.Trim() != "")
                            query = query.Where(r => r.LatestStudentChanges.Surname.ToLower().StartsWith(surname.ToLower()));
                        else if (aphabetForSurname != null && aphabetForSurname != "")
                            query = query.Where(r => r.LatestStudentChanges.Surname.ToLower().StartsWith(aphabetForSurname.ToLower()));

                        // DOB
                        if (strDOB != "")
                            query = query.Where(r => r.LatestStudentChanges.DOB == strDOB);

                        // Age
                        if (strAge != "" && age > 0)
                            query = query.Where(r => r.LatestStudentChanges.Age == age);

                        // DOA
                        if (strDOA != "")
                            query = query.Where(r => r.LatestStudentChanges.ENTRYDAT == strDOA);
                        if (gender != null && gender.Trim() != "")
                            query = query.Where(r => r.LatestStudentChanges.Gender.ToLower().Contains(gender.ToLower()));

                        // Inclusion status
                        if (inclusionStatus != null && inclusionStatus.Trim() != "")
                        {
                            string tickFlag = "√";

                            if (inclusionStatus.Trim() == "P")
                                query = query.Where(r => r.PINCL.DisplayFlag.Contains(inclusionStatus));
                            else if (inclusionStatus.Trim() == "X")
                                query = query.Where(r => r.PINCL.DisplayFlag.Contains(inclusionStatus));
                            else
                                query = query.Where(r => r.PINCL.DisplayFlag.Contains(tickFlag));
                        }

                        if (ethinicityCode != null && ethinicityCode.Trim() != "")
                            query = query.Where(r => r.LatestStudentChanges.Ethnicities.ParentEthnicityCode == ethinicityCode);

                        if (firstLanguageCode != null && firstLanguageCode.Trim() != "")
                            query = query.Where(r => r.LatestStudentChanges.Languages.LanguageCode == firstLanguageCode);

                        if (yearGroupCode != null && yearGroupCode.Trim() != "")
                            query = query.Where(r => r.LatestStudentChanges.YearGroups.YearGroupCode == yearGroupCode);

                        return query.Count();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }        
    }
}
