using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSFAQ : Logic.TSBase
    {
        public static List<FAQ> GetFAQList(Int16? cohortID, Int16? schoolGroupID, Int16? faqCategoryID, bool? june, bool? sept, bool? active)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    // Validation
                    if(cohortID.HasValue && !Validation.Common.IsKeyStageValid(context,cohortID.Value))
                        throw Web09Exception.GetBusinessException(Web09MessageList.KeystageInvalid);

                    if (schoolGroupID.HasValue && !Validation.Common.IsSchoolGroupValid(context, schoolGroupID.Value))
                        throw Web09Exception.GetBusinessException(Web09MessageList.SchoolGroupInvalid);

                    if (faqCategoryID.HasValue && !Validation.FAQ.IsFAQCategoryIDValid(context, faqCategoryID.Value))
                        throw Web09Exception.GetBusinessException(Web09MessageList.FAQInvalidCategoryID);

                    // Fetch
                    int vcohortID = 0, vschoolgroupID=0, vfaqCategoryID=0;
                    if (cohortID.HasValue)
                        vcohortID = cohortID.Value;
                    if(schoolGroupID.HasValue)                    
                        vschoolgroupID = schoolGroupID.Value;
                    if (faqCategoryID.HasValue)
                        vfaqCategoryID = faqCategoryID.Value;
                    
                    var query = (from f in context.FAQ.Include("FAQCategories")
                                 where (f.Cohorts.Any(c => c.KeyStage == vcohortID) || vcohortID == 0)
                                 && (f.SchoolGroups.Any(s => s.SchoolGroupID == vschoolgroupID) || vschoolgroupID == 0)
                                 && (f.FAQCategories.CategoryID == vfaqCategoryID || vfaqCategoryID==0)
                                       select new {FAQ=f, CAT=f.FAQCategories}
                                       ).OrderBy(o => o.FAQ.FAQCategories.ListOrder).ThenBy(o => o.FAQ.ListOrder).ToList();
                    var list = new List<FAQ>();
                    foreach (var item in query)
                    {
                        
                        var faq=item.FAQ;
                        faq.FAQCategories = item.CAT;

                        var allow = faq.IsActive;

                        if (!(active ?? false) && !faq.IsActive)
                            allow = true;

                        if (allow)
                        {
                            var validMonth = false;

                            if(sept == null && june == null)
                            {
                                //admin
                                validMonth = true;
                            }
                            if (!validMonth)
                            {
                                if (sept == (faq.IsSept ?? false))
                                    validMonth = true;
                            }
                            if (!validMonth)
                            {
                                if (june == (faq.IsJune ?? false))
                                    validMonth = true;
                            }

                            allow = validMonth;
                        }


                        if (allow)
                            list.Add(faq);

                    }
                    return list;
                }
            }
        }
    }
}
