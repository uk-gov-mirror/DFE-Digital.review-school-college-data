using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Transactions;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSFAQ : Logic.TSBase
    {
        public static void SaveFAQ(FAQ faq)
        {
            using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope() )
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        FAQ saveFAQ = null;

                        if (!Validation.FAQ.IsFAQRequiredInfoValid(context, faq))
                            throw Web09Exception.GetBusinessException(Web09MessageList.FAQIncompleteInformation);

                        if (faq.FAQID == 0) // new faq
                        {
                            saveFAQ = new FAQ();
                            saveFAQ.Cohorts = new System.Data.Objects.DataClasses.EntityCollection<Cohorts>();
                            saveFAQ.SchoolGroups = new System.Data.Objects.DataClasses.EntityCollection<SchoolGroups>();

                            Int16? maxListOrder = null;

                            try
                            {
                                maxListOrder = context.FAQ.Where(cat => cat.FAQCategories.CategoryID == faq.FAQCategories.CategoryID).Max(F => F.ListOrder);
                            }
                            catch
                            {
                                maxListOrder = 0;
                            }

                            if (maxListOrder.HasValue)
                                saveFAQ.ListOrder = maxListOrder.Value;
                            else
                                saveFAQ.ListOrder = 0;                           

                            saveFAQ.ListOrder++;
                        }
                        else
                        {
                            if(!Validation.FAQ.IsFAQIDValid(context, faq.FAQID))
                                throw Web09Exception.GetBusinessException(Web09MessageList.FAQInvalidID);

                            saveFAQ = context.FAQ.Include("FAQCategories").Include("Cohorts").Include("SchoolGroups").Where(f => f.FAQID == faq.FAQID).First();
                            if (saveFAQ.FAQCategories.CategoryID != faq.FAQCategories.CategoryID)
                            {
                                Int16? maxListOrder =null;

                                try
                                {
                                    maxListOrder = context.FAQ.Where(cat => cat.FAQCategories.CategoryID == faq.FAQCategories.CategoryID).Max(F => F.ListOrder);
                                }
                                catch
                                {
                                    maxListOrder = 0;
                                }

                                saveFAQ.ListOrder = maxListOrder.Value;

                                saveFAQ.ListOrder++;
                            }
                        }
                        
                        saveFAQ.FAQCategories = context.FAQCategories.Where(cat => cat.CategoryID == faq.FAQCategories.CategoryID).ToList().First();

                        saveFAQ.IsActive = faq.IsActive;
                        saveFAQ.IsJune = faq.IsJune;
                        saveFAQ.IsSept = faq.IsSept;

                        saveFAQ.QuestionText = faq.QuestionText.Trim();
                        saveFAQ.AnswerText = faq.AnswerText.Trim();

                        List<Cohorts> cohortList = context.Cohorts.ToList();
                        List<SchoolGroups> schoolGroupList = TSSchoolGroup.GetFAQSchoolGroups(context);

                        saveFAQ.Cohorts.Clear();
                        if (faq.Cohorts != null && faq.Cohorts.Count > 0)
                            for (int counter = 0; counter < faq.Cohorts.ToList().Count; counter++)
                                saveFAQ.Cohorts.Add((from c in cohortList where c.KeyStage == faq.Cohorts.ToList()[counter].KeyStage select c).First());

                        saveFAQ.SchoolGroups.Clear();
                        // remove old cohort add new cohort
                        if (faq.SchoolGroups != null && faq.SchoolGroups.Count > 0)
                            for (int counter = 0; counter < faq.SchoolGroups.ToList().Count; counter++)
                                saveFAQ.SchoolGroups.Add((from c in schoolGroupList where c.SchoolGroupID == faq.SchoolGroups.ToList()[counter].SchoolGroupID select c).First());

                        if (faq.FAQID == 0) // new faq
                            context.AddToFAQ(saveFAQ);
                        else
                            context.ApplyPropertyChanges("FAQ", saveFAQ);

                        context.SaveChanges();
                    }
                }
                transaction.Complete();
            }
        }
    }
}
