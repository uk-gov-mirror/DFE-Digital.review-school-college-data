using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Linq;
using System.Web.Script.Serialization;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Services.Common;
using Web09.Services.Common.JSONObjects;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSCommonLists : Logic.TSBase
    {

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public TSCommonLists()
        {
        }

        /// <summary>
        /// Retrieve all ethnicities for use in a selection control.
        /// </summary>
        /// <returns>A list of Ethnicities</returns>
        public static IList<Ethnicities> GetEthnicityList()
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {                
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return context.Ethnicities
                        //.Where(e=>e.ParentEthnicityCode==e.EthnicityCode)
                        .OrderBy(e => e.EthnicityDescription).ToList();
                }
            }
        }       

        /// <summary>
        /// Retrieve a list of languages
        /// </summary>
        /// <returns>A list of type Languages.</returns>
        public static IList<Languages> GetLanguageList()
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return context.Languages.OrderBy(l => l.LanguageCode).ToList();
                }
            }
        }

        /// <summary>
        /// Retrieve a list of Special Education Needs options.
        /// </summary>
        /// <returns>A list of type SENStatus.</returns>
        public static IList<SENStatus> GetSENList()
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return context.SENStatus.OrderBy(s => s.SENStatusDescription).ToList();
                }
            }
        }

        public static IList<CohortConfiguration> GetKeyStageConfiguration(int keyStage)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return GetKeyStageConfiguration(context, keyStage);
                }
            }
        }

        public static IList<CohortConfiguration> GetKeyStageConfiguration(Web09_Entities context, int keyStage)
        {
            if (keyStage == -1)
            {
                return context.CohortConfiguration.Select(cc => cc).ToList();
            }
            else
            {
                return context.CohortConfiguration.Where(cc => cc.KeyStage == keyStage).Select(cc => cc).ToList();
            }
        }

        /// <summary>
        /// Retrieve a list of year groups
        /// </summary>
        /// <returns>A list of year groups.</returns>
        public static IList<YearGroups> GetYearGroups()
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return context.YearGroups.OrderBy(l => l.YearGroupDescription).ToList();
                }
            }
        }

        /// <summary>
        /// Retrieve a list of scrutiny status
        /// </summary>
        /// <returns>A list of scrutiny status.</returns>
        public static IList<ScrutinyStatus> GetScrutinyStatusList()
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return context.ScrutinyStatus.OrderBy(l => l.ScrutinyStatusDescription).ToList();
                }
            }
        }

        public static IList<SchoolReasons> GetScrutinySchoolNORUpdateReasonList(int keyStage)
        {
            using (var conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (var context = new Web09_Entities(conn))
                {
                    return context.SchoolReasons
                        .Where(r => r.IsRejection && (
                            r.KeyStage == keyStage || keyStage == 0 || keyStage == 9
                         ))
                        .OrderBy(l => l.SchoolReasonText)
                        .ToList();
                }
            }
        } 

        /// <summary>
        /// Retrieve a list of Rejection reasons
        /// </summary>
        /// <returns>A list of scrutiny status.</returns>
        public static IList<Reasons> GetScrutinyRejectionReasonList(Int16 keystage, bool isPLASC)
        {
            using (var conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (var context = new Web09_Entities(conn))
                {


                    if (keystage != 4)
                    {
                        return context.Reasons
                            .Where(r => r.IsRejection == true && r.ReasonCohorts
                            .Any(rc => rc.KeyStage == keystage))
                            .OrderBy(l => l.ReasonText).ToList();
                    }

                    return context.Reasons.Where(r => r.IsRejection == true && r.ReasonCohorts.Any(rc => rc.KeyStage == keystage && rc.IsPLASC == isPLASC)).OrderBy(l => l.ReasonText).ToList();
                }
            }
        }

        /// <summary>
        /// Retrieve a list of Rejection reasons
        /// </summary>
        /// <returns>A list of scrutiny status.</returns>
        public static IList<Reasons> GetScrutinyAcceptanceReasonList(Int16 keystage, bool isPLASC)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    if (keystage != 4)
                        return context.Reasons.Where(r => r.IsRejection == false && r.ReasonCohorts.Any(rc => rc.KeyStage == keystage)).OrderBy(l => l.ReasonText).ToList();
                    else
                        return context.Reasons.Where(r => r.IsRejection == false && r.ReasonCohorts.Any(rc => rc.KeyStage == keystage && rc.IsPLASC == isPLASC)).OrderBy(l => l.ReasonText).ToList();
                }
            }
        }

        /// <summary>
        /// Retrieve a list of Acceptence reason and Amend Codes
        /// </summary>
        /// <returns>A list of scrutiny status.</returns>
        public static IList<Reasons> GetScrutinyAcceptanceReasonAmendCodesList(Int16 keystage, bool isPLASC)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    if(keystage!=4)
                        return context.Reasons.Include("AmendCodes").Where(r => r.IsRejection == false && r.ReasonCohorts.Any(rc => rc.KeyStage == keystage)).OrderBy(l => l.ReasonText).ToList();
                    else
                        return context.Reasons.Include("AmendCodes").Where(r => r.IsRejection == false && r.ReasonCohorts.Any(rc => rc.KeyStage == keystage && rc.IsPLASC == isPLASC)).OrderBy(l => l.ReasonText).ToList();
                }
            }
        }

        /// <summary>
        /// Retrieve a list of AmendCodes
        /// </summary>
        /// <returns>A list of AmendCodes.</returns>
        public static IList<AmendCodes> GetScrutinyAmendCodeList()
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return context.AmendCodes.ToList();
                }
            }
        }

        /// <summary>
        /// Retrieve a list of valid ExamYears
        /// </summary>
        /// <returns>A list of Exam Years.</returns>
        public static IList<CohortSubCohortExamYears> GetValidExamYearsList(short keystage, short subkeystage)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return context.CohortSubCohortExamYears
                            .Where(c => c.KeyStage == keystage
                                    && c.SubKeyStage == subkeystage)
                            .OrderByDescending(c => c.IsDefaultYear)
                            .OrderByDescending(c => c.ExamYear)
                            .ToList();

                }
            }
                
        }


        /// <summary>
        /// Retrieve a list of Ethinicities for a school
        /// </summary>
        /// <param name="DCSFNumber">School DFESNumber</param>
        /// <returns></returns>
        public static IList<Ethnicities> GetSchoolEthinicityList(Int32 DCSFNumber)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return GetSchoolEthinicityList(DCSFNumber, context);
                }
            }
        }

        public static IList<Ethnicities> GetSchoolEthinicityList(Int32 DCSFNumber, Web09_Entities context)
        {            
            //return context.Ethnicities.Where(e =>e.EthnicityCode==e.ParentEthnicityCode && e.StudentChanges.Any(SC => SC.Students.PINCLs != null && SC.Students.Schools.DFESNumber == DCSFNumber && SC.DateEnd == null)).OrderBy(o => o.EthnicityDescription).ToList();            
            return context.Ethnicities.Where
               (
               l => l.StudentChanges.Any
                   (
                   sc => sc.Students.Schools.DFESNumber == DCSFNumber
                       && sc.DateEnd == null
                        && sc.Students.PINCLs != null
                       )
                   )
                   .OrderBy(l => l.EthnicityDescription).ToList();
        }

        /// <summary>
        /// Retrieve a list of languages
        /// </summary>
        /// <param name="DCSFNumber">School DFESNumber</param>
        /// <returns>A list of type Languages.</returns>
        public static IList<Languages> GetSchoolLanguageList(Int32 DCSFNumber)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return GetSchoolLanguageList(DCSFNumber, context);
                }
            }
        }

        public static IList<Languages> GetSchoolLanguageList(Int32 DCSFNumber, Web09_Entities context)
        {
            return context.Languages.Where
                (
                l => l.StudentChanges.Any
                    (
                    sc => sc.Students.Schools.DFESNumber == DCSFNumber
                        && sc.DateEnd == null
                        && sc.Students.PINCLs != null
                        )
                    )
                    .OrderBy(l => l.LanguageCode).ToList();             
        }

        /// <summary>
        /// Retrieve a list of School year groups
        /// </summary>
        /// <param name="DCSFNumber">School DFESNumber</param>
        /// <returns>A list of School year groups.</returns>
        public static IList<YearGroups> GetSchoolYearGroups(Int32 DCSFNumber)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return GetSchoolYearGroups(DCSFNumber, context);
                }
            }
        }

        public static IList<YearGroups> GetSchoolYearGroups(Int32 DCSFNumber, Web09_Entities context)
        {   
            return context.YearGroups.Where
                (
                l => l.StudentChanges.Any
                    (
                    sc => sc.Students.Schools.DFESNumber == DCSFNumber
                        && sc.DateEnd == null
                        && sc.Students.PINCLs != null
                        )
                    )
                .OrderBy(l => l.YearGroupDescription).ToList();             
        }


        /// <summary>
        /// Retrieve a list of School age
        /// </summary>
        /// <param name="DCSFNumber">School DFESNumber</param>
        /// <returns>A list of School age.</returns>
        public static IList<int> GetSchoolAgeList(Int32 DCSFNumber)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return GetSchoolAgeList(DCSFNumber, context);
                }
            }
        }

        public static IList<int> GetSchoolAgeList(Int32 DCSFNumber, Web09_Entities context)
        {
            int currentAcademicYear = int.Parse(TSCommonLists.GetKeyStageConfiguration(context, 9)
                .Where(r => r.ConfigurationCode == Web09.Checking.Business.Logic.Common.Contants.COHORT_CONFIG_LOOKUP_CODE_TABLE_YEAR).FirstOrDefault().ConfigurationValue) -1;
            int lastAcademicYearDate = int.Parse(currentAcademicYear.ToString() + "0901");

            var query = from sc in context.StudentChanges
                        where sc.Students.Schools.DFESNumber == DCSFNumber
                        && sc.Students.PINCLs != null
                        && sc.DateEnd == null
                        select new
                        {
                            Age = sc.Age
                        };

            List<int> list = new List<int>();
            foreach (var age in query.Distinct().OrderBy(o => o.Age))
                list.Add(age.Age);

            return list;                
        }

        /// <summary>
        /// Retrieve a list of scrutiny reasons
        /// </summary>
        /// <returns>A list of AmendCodes.</returns>
        public static IList<ResultReasons> GetResultScrutinyReasons()
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return context.ResultReasons.OrderBy(r => r.ResultReasonText).ToList();
                }
            }
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

        /// <summary>
        /// Updates Common.CohortConfiguration with values from CheckingExerciseParameters
        /// </summary>
        /// <param name="configurationList"></param>
        public static void SaveCohortConfigurations(List<CohortConfiguration> configurationList)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    foreach (CohortConfiguration c in configurationList)
                    {
                        CohortConfiguration obj = context.CohortConfiguration.Where(cc => cc.KeyStage == c.KeyStage && cc.ConfigurationCode == c.ConfigurationCode)
                            .FirstOrDefault();
                        obj.ConfigurationValue = c.ConfigurationValue;
                        context.ApplyPropertyChanges("CohortConfiguration", obj);
                    }
                    context.SaveChanges();
                }
            }
        }

        public static PupilFilterLists GetPupilFilterLists(int dcsfNumber)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    PupilFilterLists list = new PupilFilterLists
                    {
                        ageList = TSCommonLists.GetSchoolAgeList(dcsfNumber,context),
                        awardingBodies = TSResult.GetAwardingBodies(context),
                        cohortConfigList = TSCommonLists.GetKeyStageConfiguration(context,9),
                        cohortlist = TSCohort.GetList(context),
                        ethnicityList = TSCommonLists.GetSchoolEthinicityList(dcsfNumber, context),
                        languageList = TSCommonLists.GetSchoolLanguageList(dcsfNumber,context),
                        yearGroupList = TSCommonLists.GetSchoolYearGroups(dcsfNumber,context)
                    };
                    return list;
                }
            }
        }

        public static string GetPupilFilterListsV2(int dcsfNumber)
        {
            Web09.Services.Common.JSONObjects.PupilFilters pupilFilters = new Web09.Services.Common.JSONObjects.PupilFilters();

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();
                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    System.Data.Common.DbConnection connection = conn.StoreConnection;

                    System.Data.Common.DbCommand cmd = connection.CreateCommand();
                    cmd.Connection = connection;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[Student].[GetPupilFilters]";

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@DCSFNumber",
                        SqlValue = dcsfNumber
                    });

                    using (System.Data.Common.DbDataReader dr = cmd.ExecuteReader())
                    {

                        // 1st resultset returned is AgeList
                        while (dr.Read())
                        {
                            pupilFilters.Ages.Add(new Web09.Services.Common.JSONObjects.PupilFilters.PupilFilterAge
                            {
                                Age      = Convert.ToString(dr["Age"]),
                                AgeLabel = Convert.ToString(dr["Age"])
                            });
                        }

                        // 2nd resultset is AwardingBodyList
                        dr.NextResult();
                        while (dr.Read())
                        {
                            pupilFilters.AwardingBodyCodes.Add(new Web09.Services.Common.JSONObjects.PupilFilters.PupilFilterAwardingBody
                                {
                                    AwardingBodyID = Convert.ToString(dr["AwardingBodyID"]),
                                    AwardingBodyCode = Convert.ToString(dr["AwardingBodyLabel"])
                                });
                        }

                        // 3rd resultset is CohortList
                        dr.NextResult();
                        while (dr.Read())
                        {
                            pupilFilters.Cohorts.Add(new Web09.Services.Common.JSONObjects.PupilFilters.PupilFilterCohort
                            {
                                KeyStage = Convert.ToInt32(dr["KeyStage"]),
                                KeyStageName = Convert.ToString(dr["KeyStageName"])
                            });
                        }

                        // 4th resultset is ConfigurationList
                        dr.NextResult();
                        while (dr.Read())
                        {
                          pupilFilters.ConfigurationItems.Add(new Web09.Services.Common.JSONObjects.PupilFilters.PupilFilterConfigurationValue
                          {
                            ConfigurationCode = Convert.ToString(dr["ConfigurationCode"]),
                            ConfigurationValue = Convert.ToString(dr["ConfigurationValue"])
                          });
                        }

                        // 5th resultset is Ethnicity list
                        dr.NextResult();
                        while (dr.Read())
                        {
                          pupilFilters.Ethnicitys.Add(new Web09.Services.Common.JSONObjects.PupilFilters.PupilFilterEthnicity
                          {
                            EthnicityCode        = Convert.ToString(dr["EthnicityCode"]),
                            EthnicityDescription = Convert.ToString(dr["EthnicityDescription"]),
                            ParentEthnicityCode  = Convert.ToString(dr["ParentEthnicityCode"])
                          });
                        }

                        // 6th resultset is LanguageList
                        dr.NextResult();
                        while (dr.Read())
                        {
                            pupilFilters.Languages.Add(new Web09.Services.Common.JSONObjects.PupilFilters.PupilFilterLanguage
                            {
                                LanguageCode = Convert.ToString(dr["LanguageCode"]),
                                LanguageDescription = Convert.ToString(dr["LanguageDescription"])
                            });
                        }

                        // 7th Resultset is YearGroupList
                        dr.NextResult();
                        while (dr.Read())
                        {
                          pupilFilters.YearGroups.Add(new Web09.Services.Common.JSONObjects.PupilFilters.PupilFilterYearGroup
                          {
                            YearGroupCode        = Convert.ToString(dr["YearGroupCode"]),
                            YearGroupDescription = Convert.ToString(dr["YearGroupDescription"])
                          });
                        }
                    }


                }

                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                string jsonData                   = jsSerializer.Serialize(pupilFilters);

                return jsonData;
            }
        }

        public static void AddUserToPasswordLetterQueue(UserContext userContext, string consumerURL, string username)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    // create change object
                    Changes newChange = CreateChangeObject(context, 1, userContext);
                    context.AddToChanges(newChange);
                    context.SaveChanges();

                    UserPasswordLetters letter = new UserPasswordLetters
                    {
                        CreatedChangeID = newChange.ChangeID,
                        ConsumerURL = consumerURL,
                        Username = username
                    };

                    context.AddToUserPasswordLetters(letter);

                    context.SaveChanges();
                }
            }

        }

        public static int GetPasswordLetterCount(DateTime minDate, bool letterSent)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {

                    var query = (from u in context.UserPasswordLetters
                                 join chg in context.Changes on u.CreatedChangeID equals chg.ChangeID
                                 where chg.ChangeDate > minDate && u.PrintedChangeID == null != letterSent
                                 select u);

                    return query.Count();

                }
            }
        }

        public static List<string> GetPasswordLettersToSend()
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {

                    var query = (from u in context.UserPasswordLetters
                                 join chg in context.Changes on u.CreatedChangeID equals chg.ChangeID
                                 where u.PrintedChangeID == null
                                 select u.Username);

                    return query.ToList();
                }
            }
        }

        public static List<PrintQueueItem> GetPasswordPostQueueData(DateTime minDate, bool letterSent)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    if (letterSent)
                    {
                        return (from u in context.UserPasswordLetters
                                join chg in context.Changes on u.CreatedChangeID equals chg.ChangeID
                                join printed in context.Changes on u.PrintedChangeID equals printed.ChangeID
                                where chg.ChangeDate > minDate
                                select new PrintQueueItem
                                {
                                    Username = u.Username,
                                    DateQueued = chg.ChangeDate,
                                    QueuedByUser = chg.UserName,
                                    PrintedByUser = printed.UserName,
                                    DatePrinted = printed.ChangeDate
                                }).ToList();
                    }
                    return (from u in context.UserPasswordLetters
                            join chg in context.Changes on u.CreatedChangeID equals chg.ChangeID
                            where chg.ChangeDate > minDate
                                  && u.PrintedChangeID == null
                            select new PrintQueueItem
                            {
                                Username = u.Username,
                                DateQueued = chg.ChangeDate,
                                QueuedByUser = chg.UserName
                            }).ToList();
                }
            }
        }

      
        public static string GetUserStatus(string username)
        {
            UserStatus userStatus = null;

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();
                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    System.Data.Common.DbConnection connection = conn.StoreConnection;

                    System.Data.Common.DbCommand cmd = connection.CreateCommand();
                    cmd.Connection = connection;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[FSG].[GetUserStatus]";

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.String,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@UserName",
                        SqlValue = username
                    });

                    using (System.Data.Common.DbDataReader dr = cmd.ExecuteReader())
                    {                       
                        if (dr.Read())
                        {
                            userStatus = new UserStatus
                            {
                                IsLockedOut = Convert.ToBoolean(dr["IsLockedOut"]),
                                CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                                LastLockedOutDate = Convert.ToDateTime(dr["LastLockOutDate"]),
                                LastLoginDate = Convert.ToDateTime(dr["LastLoginDate"])
                            };
                        }                       
                    }
                    
                }

                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                string jsonData = string.Empty;
                if (userStatus != null)
                {
                    jsonData = jsSerializer.Serialize(userStatus);
                }

                return jsonData;
            }
        }
        
       
        public static string GetContent( string contentURI, string contentRequestContext )
        {
            string content         = string.Empty;
            Dictionary<string, string> contextDictionary = new Dictionary<string, string>();
          
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();
                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    System.Data.Common.DbConnection connection = conn.StoreConnection;

                    System.Data.Common.DbCommand cmd = connection.CreateCommand();
                    cmd.Connection = connection;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[ContentManagement].[GetContent]";

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.String,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@ContentURI",
                        SqlValue = contentURI
                    });

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.String,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@Context",
                        SqlValue = contentRequestContext
                    });                                    
                   
                    System.Data.Common.DbDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        content = Convert.ToString(dr[0]);
                    }

                    dr.NextResult();
                    while (dr.Read())
                    {
                        contextDictionary.Add( Convert.ToString(dr[0]), 
                            Convert.ToString(dr[1]) );
                    }
                                    
                }
            }

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            ContentHolder contentHolder = new ContentHolder { Content = content, Context = contextDictionary };
            string jsonData = jsSerializer.Serialize(contentHolder);

            return jsonData;
        }

        public static void SetPasswordPrintedDate(string userName, UserContext userContext)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {

                    Changes newChange = CreateChangeObject(context, 1, userContext);
                    context.AddToChanges(newChange);
                    context.SaveChanges();

                    var userRecord = (from u in context.UserPasswordLetters
                                      where u.PrintedChangeID == null && u.Username == userName
                                      select u).FirstOrDefault();

                    userRecord.PrintedChangeID = newChange.ChangeID;

                    context.SaveChanges();
                }
            }
        }


        public static bool AddToUserPasswordLetter(string userName, bool checkQueueStatus, string consumerURL, Web09.Checking.Business.Logic.Entities.UserContext userContext, out DateTime? lastPrintDate, out DateTime? lastQueuedDate)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {

                    UserPasswordLetters lastLetterStatus = context.UserPasswordLetters.Where(upl => upl.Username == userName).ToList().OrderByDescending(upl => upl.CreatedChangeID).ThenByDescending(upl => upl.PrintedChangeID).FirstOrDefault();

                    bool queueUser = false;
                    lastPrintDate = null;
                    lastQueuedDate = null;

                    // should check queue status or not
                    if (checkQueueStatus)
                    {
                        if (lastLetterStatus != null)
                        {
                            // Get Change Information                            
                            Changes lastQueuedDateChange = context.Changes.Where(c => c.ChangeID == lastLetterStatus.CreatedChangeID).FirstOrDefault();
                            if (lastQueuedDateChange != null)
                            {
                                lastQueuedDate = lastQueuedDateChange.ChangeDate;
                            }                            

                            if (lastLetterStatus.PrintedChangeID != null)
                            {
                                Changes lastPrintedDateChange = context.Changes.Where(c => c.ChangeID == lastLetterStatus.PrintedChangeID).FirstOrDefault();
                                if (lastPrintedDateChange != null)
                                {
                                    lastPrintDate = lastPrintedDateChange.ChangeDate;
                                }
                            }
                        }
                        else
                        {
                            queueUser = true;
                        }
                        // return information only
                        return true;
                    }
                    else
                    {
                        queueUser = true;
                    }

                    if (queueUser)
                    {
                        if (checkQueueStatus == false)
                        {
                            // another check before saving for active queue request.
                            UserPasswordLetters alreadyQueued = context.UserPasswordLetters.Where(upl => upl.Username == userName && upl.PrintedChangeID == null).FirstOrDefault();
                            if (alreadyQueued != null)
                            {
                                Changes lastQueuedDateChange = context.Changes.Where(c => c.ChangeID == alreadyQueued.CreatedChangeID).FirstOrDefault();
                                if (lastQueuedDateChange != null)
                                {
                                    lastQueuedDate = lastQueuedDateChange.ChangeDate;
                                }
                                
                                return false;
                            }
                        }                        

                        // create change object
                        Changes newChange = CreateChangeObject(context, 1, userContext);
                        context.AddToChanges(newChange);
                        context.SaveChanges();

                        UserPasswordLetters letter = new UserPasswordLetters
                        {
                            CreatedChangeID = newChange.ChangeID,
                            ConsumerURL = consumerURL,
                            Username = userName
                        };

                        context.AddToUserPasswordLetters(letter);

                        context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }            
        }

    }
}
