using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent : Logic.TSBase
    {
        public static List<StudentWithResult> GetStudentsWithoutResults(int dcsfNumber, short keyStageID)
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
                        else if (!Validation.Common.IsKeyStageValid(context, keyStageID))
                            throw Web09Exception.GetBusinessException(Web09MessageList.KeystageInvalid);                        

                        var query = from s in context.Students
                                    join sc in context.StudentChanges on s.StudentID equals sc.StudentID
                                    where sc.DateEnd == null
                                    && (s.Schools.DFESNumber == dcsfNumber)
                                    && (s.Cohorts.KeyStage == keyStageID)
                                    && (s.Results.Count == 0)
                                    select new
                                    {
                                        s.Schools.DFESNumber,
                                        s.Cohorts.KeyStage,
                                        s.StudentID,
                                        PINCLDisplayFlag = s.PINCLs.DisplayFlag,
                                        s.ForvusIndex,
                                        sc.Forename,
                                        sc.Surname,
                                        sc.DOB,
                                        sc.Gender,
                                        sc.Ethnicities.EthnicityCode,
                                        sc.Languages.LanguageCode,
                                        sc.YearGroups.YearGroupCode,
                                        objResult = (Results)null,
                                        ColumnSort = 4,
                                        DataOriginDescription = ""
                                    };

                        var results = query.Distinct();
                        List<StudentWithResult> result = new List<StudentWithResult>();

                        foreach (var info in results)
                        {
                            StudentWithResult obj = new StudentWithResult();

                            obj.DFESNumber = info.DFESNumber;
                            obj.StudentID = info.StudentID;
                            obj.DOB = info.DOB;
                            obj.EthnicityCode = info.EthnicityCode;
                            obj.Forename = info.Forename;
                            if (info.ForvusIndex.HasValue) obj.ForvusIndex = info.ForvusIndex.Value;
                            obj.Gender = info.Gender;
                            obj.KeyStage = info.KeyStage;
                            obj.LanguageCode = info.LanguageCode;
                            obj.Surname = info.Surname;
                            obj.YearGroupCode = info.YearGroupCode;
                            obj.PINCLDisplayFlag = info.PINCLDisplayFlag;
                            result.Add(obj);
                        }

                        return result;
                    }
                }
                        
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }
}
