using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{

    [Obsolete("This LINQ code generates VERY inefficent SQL. Use the GetCohortDetailsV2 operation instead.")]
    public partial class TSCohort : Logic.TSBase
    {
        
        public static List<Web09.Checking.Business.Logic.Entities.CohortDetailPupil> GetCohortDetails(int page, int rowsPerPage, int dcsfNumber, short? keyStageID, string forename, string surname, DateTime? dob, DateTime? doa, string gender, string inclusionStatus, string ethinicityCode, string firstLanguageCode, string yearGroupCode, string sortExpression, string aphabetForSurname, String strAge)
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

                                    Schools = s.Schools,

                                    KS1EXPValue = ((System.Data.Objects.ObjectQuery<StudentValues>)(from w in s.StudentValues where w.DateEnd == null && w.StudentValueTypes.ValueTypeCode == "KS1EXP" select w).AsQueryable()).Include("StudentValueTypes").FirstOrDefault(),
                                    NewMobileValue = ((System.Data.Objects.ObjectQuery<StudentValues>)(from w in s.StudentValues where w.DateEnd == null && w.StudentValueTypes.ValueTypeCode == "NEWMOBILE" select w).AsQueryable()).Include("StudentValueTypes").FirstOrDefault(),
                                    FSM6Value = ((System.Data.Objects.ObjectQuery<StudentValues>)(from w in s.StudentValues where w.DateEnd == null && w.StudentValueTypes.ValueTypeCode == "FSM6" select w).AsQueryable()).Include("StudentValueTypes").FirstOrDefault(),

                                    LatestYearGroups = ((System.Data.Objects.ObjectQuery<StudentChanges>)s.StudentChanges.Where(w => w.DateEnd == null).AsQueryable()).Include("YearGroups").FirstOrDefault().YearGroups,
                                    LatestEthnicity = s.StudentChanges.Where(w => w.DateEnd == null).FirstOrDefault().Ethnicities,
                                    LatestLanguages = s.StudentChanges.Where(w => w.DateEnd == null).FirstOrDefault().Languages,
                                    LatestSenStatus = s.StudentChanges.Where(w => w.DateEnd == null).FirstOrDefault().SENStatus,                                 

                                    PINCL = s.PINCLs,
                                    Cohort = s.Cohorts,
                                    StudentRequests = (from sr in s.StudentRequests
                                                       where !sr.StudentRequestChanges.Any(src => src.ScrutinyStatus.ScrutinyStatusCode == "C")
                                                       select new
                                                       {
                                                           SR = sr,
                                                           SRC = sr.StudentRequestChanges.OrderBy(src => src.DateEnd ?? DateTime.Now).FirstOrDefault(),
                                                           SRCS = sr.StudentRequestChanges.OrderBy(src => src.DateEnd ?? DateTime.Now).FirstOrDefault().ScrutinyStatus,
                                                       }
                                                        ).FirstOrDefault(),

                                    // Result Changes
                                    HasResultChanges = s.Results.Any
                                    (res => res.ResultChanges
                                        .Any
                                        (
                                            rchg =>
                                                rchg.DateEnd == null
                                                &&
                                            (
                                                rchg.ResultStatus.ResultStatusDescription == "Amended"
                                                ||
                                                rchg.ResultStatus.ResultStatusDescription == "Added"
                                                ||
                                                rchg.ResultStatus.ResultStatusDescription == "Withdrawn"
                                             )
                                         )
                                     )
                                  
                                };
                    if (forename != null && forename.Trim() != "")
                        query = query.Where(r => r.LatestStudentChanges.Forename.ToLower().StartsWith(forename.ToLower()));
                    if (surname != null && surname.Trim() != "")
                        query = query.Where(r => r.LatestStudentChanges.Surname.ToLower().StartsWith(surname.ToLower()));
                    else if(aphabetForSurname!=null && aphabetForSurname!="")
                        query = query.Where(r => r.LatestStudentChanges.Surname.ToLower().StartsWith(aphabetForSurname.ToLower()));

                    // DOB
                    if (strDOB != "")
                        query = query.Where(r => r.LatestStudentChanges.DOB == strDOB);

                    // Age
                    if(strAge!="" && age>0)
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

                    //**SORT BY clause

                    // without a sort order specified the skipto() will generate a runtime error and cause the web page to fall over
                    // so track sorting and ensure an order is applied
                    bool isSorted = false;

                    if (sortExpression.ToLower().Contains("forename"))
                    {
                        if (sortExpression.ToLower().Contains("asc"))
                            query = query.OrderBy(r => r.LatestStudentChanges.Forename);
                        else
                            query = query.OrderByDescending(r => r.LatestStudentChanges.Forename);
                        isSorted = true;
                    }

                    if (sortExpression.ToLower().Contains("status"))
                    {
                        if (sortExpression.ToLower().Contains("asc"))
                            query = query.OrderBy(r => r.PINCL.DisplayFlag);
                        else
                            query = query.OrderByDescending(r => r.PINCL.DisplayFlag);
                        isSorted = true;
                    }

                    if (sortExpression.ToLower().Contains("requests"))
                    {
                        if (sortExpression.ToLower().Contains("asc"))
                            query = query.OrderBy(r => r.StudentRequests.SRCS.ScrutinyStatusCode);
                        else
                            query = query.OrderByDescending(r => r.StudentRequests.SRCS.ScrutinyStatusCode);
                        isSorted = true;
                    }

                    if (sortExpression.ToLower().Contains("forvusid"))
                    {
                        if (sortExpression.ToLower().Contains("asc"))
                            query = query.OrderBy(r => r.Students.ForvusIndex);
                        else
                            query = query.OrderByDescending(r => r.Students.ForvusIndex);
                        isSorted = true;
                    }

                    if (sortExpression.ToLower().Contains("surname"))
                    {
                        if (sortExpression.ToLower().Contains("asc"))
                            query = query.OrderBy(r => r.LatestStudentChanges.Surname).ThenBy(r => r.LatestStudentChanges.Forename).ThenBy(r => r.Students.ForvusIndex);
                        else
                            query = query.OrderByDescending(r => r.LatestStudentChanges.Surname).ThenByDescending(r => r.LatestStudentChanges.Forename).ThenByDescending(r => r.Students.ForvusIndex);
                        isSorted = true;
                    }

                    if (sortExpression.ToLower().Contains("dob") || sortExpression.ToLower().Contains("age"))
                    {
                        if (sortExpression.ToLower().Contains("asc"))
                            query = query.OrderBy(r => r.LatestStudentChanges.DOB);
                        else
                            query = query.OrderByDescending(r => r.LatestStudentChanges.DOB);
                        isSorted = true;
                    }

                    if (sortExpression.ToLower().Contains("admissiondate"))
                    {
                        if (sortExpression.ToLower().Contains("asc"))
                            query = query.OrderBy(r => r.LatestStudentChanges.ENTRYDAT);
                        else
                            query = query.OrderByDescending(r => r.LatestStudentChanges.ENTRYDAT);
                        isSorted = true;
                    }

                    if (sortExpression.ToLower().Contains("gender"))
                    {
                        if (sortExpression.ToLower().Contains("asc"))
                            query = query.OrderBy(r => r.LatestStudentChanges.Gender);
                        else
                            query = query.OrderByDescending(r => r.LatestStudentChanges.Gender);
                        isSorted = true;
                    }

                    if (sortExpression.ToLower().Contains("ethnicitycode"))
                    {
                        if (sortExpression.ToLower().Contains("asc"))
                            query = query.OrderBy(r => r.LatestStudentChanges.Ethnicities.ParentEthnicityCode);
                        else
                            query = query.OrderByDescending(r => r.LatestStudentChanges.Ethnicities.ParentEthnicityCode);
                        isSorted = true;
                    }

                    if (sortExpression.ToLower().Contains("firstlanguage"))
                    {
                        if (sortExpression.ToLower().Contains("asc"))
                            query = query.OrderBy(r => r.LatestStudentChanges.Languages.LanguageCode);
                        else
                            query = query.OrderByDescending(r => r.LatestStudentChanges.Languages.LanguageCode);
                        isSorted = true;
                    }


                    if (sortExpression.ToLower().Contains("yeargroup"))
                    {
                        if (sortExpression.ToLower().Contains("asc"))
                            query = query.OrderBy(r => r.LatestStudentChanges.YearGroups.YearGroupDescription);
                        else
                            query = query.OrderByDescending(r => r.LatestStudentChanges.YearGroups.YearGroupDescription);
                        isSorted = true;
                    }

                    if (sortExpression.ToLower().Contains("fsm"))
                    {
                        if (sortExpression.ToLower().Contains("asc"))
                            query = query.OrderBy(r => r.LatestStudentChanges.FSM);
                        else
                            query = query.OrderByDescending(r => r.LatestStudentChanges.FSM);
                        isSorted = true;
                    }

                    if (sortExpression.ToLower().Contains("senstatus"))
                    {
                        if (sortExpression.ToLower().Contains("asc"))
                            query = query.OrderBy(r => r.LatestStudentChanges.SENStatus.SENStatusCode);
                        else
                            query = query.OrderByDescending(r => r.LatestStudentChanges.SENStatus.SENStatusCode);
                        isSorted = true;
                    }

                    if (sortExpression.ToLower().Contains("incare"))
                    {
                        if (sortExpression.ToLower().Contains("asc"))
                            query = query.OrderBy(r => r.LatestStudentChanges.LookedAfterEver);
                        else
                            query = query.OrderByDescending(r => r.LatestStudentChanges.LookedAfterEver);
                        isSorted = true;
                    }

                    if (sortExpression.ToLower().Contains("idaci"))
                    {
                        if (sortExpression.ToLower().Contains("asc"))
                            query = query.OrderBy(r => r.LatestStudentChanges.PostCode);
                        else
                            query = query.OrderByDescending(r => r.LatestStudentChanges.PostCode);
                        isSorted = true;
                    }

                    if (sortExpression.ToLower().Contains("ks1exp"))
                    {
                        if (sortExpression.ToLower().Contains("asc"))
                            query = query.OrderBy(r => r.KS1EXPValue.Value);
                        else
                            query = query.OrderByDescending(r => r.KS1EXPValue.Value);
                        isSorted = true;
                    }

                    if (sortExpression.ToLower().Contains("fsm6"))
                    {
                        if (sortExpression.ToLower().Contains("asc"))
                            query = query.OrderBy(r => r.FSM6Value.Value);
                        else
                            query = query.OrderByDescending(r => r.FSM6Value.Value);
                        isSorted = true;
                    }

                    if (sortExpression.ToLower().Contains("newmobile"))
                    {
                        if (sortExpression.ToLower().Contains("asc"))
                            query = query.OrderBy(r => r.NewMobileValue.Value);
                        else
                            query = query.OrderByDescending(r => r.NewMobileValue.Value);
                        isSorted = true;
                    }

                    // Default sort order to avoid runtime error when Skip() operation used
                    if (isSorted == false)
                    {
                        query = query.OrderBy(r => r.LatestStudentChanges.Surname).ThenBy(r => r.LatestStudentChanges.Forename).ThenBy(r => r.Students.ForvusIndex);
                        isSorted = true;
                    }

                    var students = query
                        .Skip((page - 1) * rowsPerPage)
                            .Take(rowsPerPage)
                        .ToList();

                    List<Web09.Checking.Business.Logic.Entities.CohortDetailPupil> studentList = new List<Web09.Checking.Business.Logic.Entities.CohortDetailPupil>();
                    
                    foreach (var resultItem in students)
                    {
                        Students student = resultItem.Students;

                        // Current/Latest student Change
                        if (resultItem.LatestStudentChanges != null)
                        {
                            if(!resultItem.LatestStudentChanges.YearGroupsReference.IsLoaded)
                                resultItem.LatestStudentChanges.YearGroupsReference.Load();

                            if (!resultItem.LatestStudentChanges.LanguagesReference.IsLoaded)
                                resultItem.LatestStudentChanges.LanguagesReference.Load();

                            if (!resultItem.LatestStudentChanges.EthnicitiesReference.IsLoaded)
                                resultItem.LatestStudentChanges.EthnicitiesReference.Load();
                            
                            if (!resultItem.LatestStudentChanges.SENStatusReference.IsLoaded)
                                resultItem.LatestStudentChanges.SENStatusReference.Load();
                            
                            student.StudentChanges.Add(resultItem.LatestStudentChanges);
                        }
                        // original unamended one which can be the latest one as well
                        if (resultItem.OriginalStudentChanges != null)
                        {
                            if(!resultItem.OriginalStudentChanges.YearGroupsReference.IsLoaded)
                                resultItem.OriginalStudentChanges.YearGroupsReference.Load();

                            if (!resultItem.OriginalStudentChanges.LanguagesReference.IsLoaded)
                                resultItem.OriginalStudentChanges.LanguagesReference.Load();

                            if (!resultItem.OriginalStudentChanges.EthnicitiesReference.IsLoaded)
                                resultItem.OriginalStudentChanges.EthnicitiesReference.Load();

                            if (!resultItem.OriginalStudentChanges.SENStatusReference.IsLoaded)
                                resultItem.OriginalStudentChanges.SENStatusReference.Load();

                            student.StudentChanges.Add(resultItem.OriginalStudentChanges);
                        }
                        student.Schools = resultItem.Schools;


                        // STUDENT HAS A REQUEST
                        if (resultItem.StudentRequests != null && resultItem.StudentRequests.SR != null)
                        {
                            student.StudentRequests.Add(resultItem.StudentRequests.SR);
                            student.StudentRequests.FirstOrDefault().StudentRequestChanges.Add(resultItem.StudentRequests.SRC);
                            student.StudentRequests.FirstOrDefault().StudentRequestChanges.FirstOrDefault().ScrutinyStatus = resultItem.StudentRequests.SRCS;
                        }

                        string _KS1EXP = string.Empty;
                        if (resultItem.KS1EXPValue != null)
                        {
                            _KS1EXP = resultItem.KS1EXPValue.Value;
                        }

                        string _FSM6 = string.Empty;
                        if (resultItem.FSM6Value != null)
                        {
                            _FSM6 = resultItem.FSM6Value.Value;
                        }

                        string _newMobile = string.Empty;
                        if (resultItem.NewMobileValue != null)
                        {
                            _newMobile = resultItem.NewMobileValue.Value;
                        }                      

                        studentList.Add(new Web09.Checking.Business.Logic.Entities.CohortDetailPupil
                            {
                                 Student=student,
                                 HasResultAmendments=resultItem.HasResultChanges,
                                 KS1EXP = _KS1EXP,
                                 FSM6 = _FSM6,
                                 NewMobile = _newMobile
                            });
                    }

                    return studentList;
                }
            }
        }

        internal static DateTime GetAnnualSchoolCensusDate(Web09_Entities context)
        {
            return Convert.ToDateTime(context.CohortConfiguration
                .FirstOrDefault(cc => cc.ConfigurationCode == Contants.ANNUAL_SCHOOL_CENSUS_DATE_LOOKUP_CODE)
                .ConfigurationValue);
        }

        internal static DateTime GetKeyStageTestStartDate(Web09_Entities context, short keyStage)
        {
            
            switch(keyStage)
            {
                case(2):
                    return Convert.ToDateTime(context.CohortConfiguration
                        .FirstOrDefault(cc => cc.Cohorts.KeyStage == 2 && cc.ConfigurationCode == Contants.KS2_TEST_START_DATE_LOOKUP_CODE)
                        .ConfigurationValue);
                case(3):
                    return Convert.ToDateTime(context.CohortConfiguration
                        .FirstOrDefault(cc => cc.Cohorts.KeyStage == 3 && cc.ConfigurationCode == Contants.KS3_TEST_START_DATE_LOOKUP_CODE)
                        .ConfigurationValue);
                default:
                    throw new Exception("Invalid key stage");
            }

            
        }
    }
}
