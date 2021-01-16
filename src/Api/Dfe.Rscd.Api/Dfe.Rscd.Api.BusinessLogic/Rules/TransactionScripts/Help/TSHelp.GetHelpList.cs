using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSHelp : Logic.TSBase
    {
        public static List<Help> GetHelpList(Int16? pageID, string pageName, Int16? cohortID, Int16? schoolGroupID, String userLevelID, bool? june, bool? sept, bool? active, bool includeDetails)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    // Validation
                    if (cohortID.HasValue && cohortID .Value>0 && !Validation.Common.IsKeyStageValid(context, cohortID.Value))
                        throw Web09Exception.GetBusinessException(Web09MessageList.KeystageInvalid);
                    if (schoolGroupID.HasValue && schoolGroupID .Value>0 && !Validation.Common.IsSchoolGroupValid(context, schoolGroupID.Value))
                        throw Web09Exception.GetBusinessException(Web09MessageList.SchoolGroupInvalid);
                    if (pageID.HasValue && !Validation.Help.IsPageIDValid(context, pageID.Value))
                        throw Web09Exception.GetBusinessException(Web09MessageList.HelpInvalidPageID);

                    int vpageID=0, vcohortID = 0, vschoolgroupID = 0;                    

                    if (cohortID.HasValue)
                        vcohortID = cohortID.Value;
                    if(schoolGroupID.HasValue)                    
                        vschoolgroupID = schoolGroupID.Value;
                    
                    if (pageID.HasValue)
                        vpageID = pageID.Value;

                    var query = (from f in context.Help.Include("Pages").Include("UserLevels").Include("SchoolGroups").Include("Cohorts")
                                 where (f.Cohorts.Any(c => c.KeyStage == vcohortID) || vcohortID == 0)
                                 && (f.SchoolGroups.Any(s => s.SchoolGroupID == vschoolgroupID) || vschoolgroupID ==0)
                                 && (f.HelpUserLevels.Any(s => s.UserLevel == userLevelID) || userLevelID == null)
                                 && (f.Pages.PageID == vpageID || vpageID == 0)    
                                 && (f.Pages.PageName == pageName || String.IsNullOrEmpty(pageName))
                                       select new {HELP=f, PAGE=f.Pages, USERLEVEL=f.HelpUserLevels, SCHGROUPS=f.SchoolGroups, COHORTS=f.Cohorts}
                                       ).OrderBy(o=>o.HELP.ListOrder).ToList();                    

                    List<Help> list = new List<Help>();
                    foreach (var item in query)
                    {
                        bool allow = true;
                        Help help = item.HELP;

                        help.Pages = new Pages { PageID = item.PAGE.PageID };

                        if (sept.HasValue)
                            if (sept != help.IsSept)
                            allow = false;

                        if (june.HasValue)
                            if (june != help.IsJune)
                            allow = false;

                        if (active.HasValue)
                            if (active != help.IsActive)
                            allow = false;                        

                        if (includeDetails==false)
                        {   
                            if(help.SchoolGroups!=null) help.SchoolGroups.Clear();
                            if(help.Cohorts!=null) help.Cohorts.Clear();
                        }

                        if(allow)
                            list.Add(help);
                    }
                    return list;
                }
            }
        }     
      

        public static List<Help> GetHelpListForPage(string pageName, List<Int16> cohortIDList, List<Int16> schoolGroupIDList, String userLevelID, bool? june, bool? sept)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    if (userLevelID == null)
                        userLevelID = "";

                    var prelist = context.Help
                        .Include("Pages")
                        .Include("HelpUserLevels")
                        .Include("Cohorts")
                        .Include("SchoolGroups")
                        .Where(
                            h =>
                            h.Pages.PageName == pageName
                            && (userLevelID == "" || h.HelpUserLevels.Any(hul => hul.UserLevel == userLevelID))
                            && h.IsActive == true
                        );

                    if(sept ?? false)
                    {
                        prelist = prelist.Where(x => x.IsSept);
                    }
                    if(june ?? false)
                    {
                        prelist = prelist.Where(x => x.IsJune);
                    }

                    var list = prelist.ToList();

                    var resultList = new List<Help>();

                    // filter by cohort
                    if (cohortIDList != null && schoolGroupIDList!=null && schoolGroupIDList.Count>0 && cohortIDList.Count > 0)
                    {   
                        foreach (Help h in list)
                        {
                            bool cohortExist = cohortIDList.Any(t => h.Cohorts.Any(c => c.KeyStage == t));

                            bool schoolGroupExists = schoolGroupIDList.Any(t => h.SchoolGroups.Any(c => c.SchoolGroupID == t));

                            if(cohortExist && schoolGroupExists)
                                resultList.Add
                                (
                                    h
                                );
                        }                         
                    }
                    else
                    {
                        resultList.AddRange(list);
                    }

                    return resultList.OrderBy(h=>h.ListOrder).ToList();                                        
                }
            }
        }
    }
}
