using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSFAQCategory : Logic.TSBase
    {
        public static List<FAQCategories> SaveCategory(FAQCategories category, bool returnList)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    FAQCategories saveCategory = null;

                    // Update
                    if (category.CategoryID > 0)
                    {
                        if (!Validation.FAQ.IsFAQCategoryIDValid(context, category.CategoryID))
                            throw Web09Exception.GetBusinessException(Web09MessageList.FAQInvalidCategoryID);

                        saveCategory = context.FAQCategories.First(r => r.CategoryID == category.CategoryID);
                    }
                    else
                        saveCategory = new FAQCategories();
                    
                    saveCategory.CategoryName = category.CategoryName;
                    saveCategory.ListOrder = category.ListOrder;

                    if (category.CategoryID > 0)
                        context.ApplyPropertyChanges("FAQCategories", saveCategory);
                    else
                        context.AddToFAQCategories(saveCategory);
                    
                    context.SaveChanges();

                    if (returnList)
                        return GetCategoryList();
                }
            }
            return null;
        }

        public static List<FAQCategories> SaveCategories(List<FAQCategories> categories)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    FAQCategories saveCategory = null;

                    foreach (FAQCategories category in categories)
                    {
                        // Update
                        if (category.CategoryID > 0)
                        {
                            if (!Validation.FAQ.IsFAQCategoryIDValid(context, category.CategoryID))
                                throw Web09Exception.GetBusinessException(Web09MessageList.FAQInvalidCategoryID);

                            saveCategory = context.FAQCategories.First(r => r.CategoryID == category.CategoryID);
                        }
                        else
                            saveCategory = new FAQCategories();

                        saveCategory.CategoryName = category.CategoryName;
                        saveCategory.ListOrder = category.ListOrder;

                        if (category.CategoryID > 0)
                            context.ApplyPropertyChanges("FAQCategories", saveCategory);
                        else
                            context.AddToFAQCategories(saveCategory);
                    }
                    context.SaveChanges();

                    return GetCategoryList();
                }
            }            
        }
    }
}
