using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Transactions;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSHelp : Logic.TSBase
    {
        public static void SaveHelp(Help help)
        {
            using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope())
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();
                    
                    if (help.HelpID > 0)
                    {
                        // reset help user levels
                        System.Data.Common.DbConnection con=conn.StoreConnection;
                        System.Data.Common.DbCommand cmd = con.CreateCommand();
                        cmd.Connection = con;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "DELETE FROM School.HelpUserLevels where HelpID=" + help.HelpID.ToString() + "";
                        cmd.ExecuteNonQuery();
                    }

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        Help saveHelp = null;

                        if (!Validation.Help.IsHelpRequiredInfoValid(context, help))
                            throw Web09Exception.GetBusinessException(Web09MessageList.HelpIncompleteInformation);

                        if (help.HelpID == 0) // new
                        {
                            saveHelp = new Help();
                            saveHelp.Cohorts = new System.Data.Objects.DataClasses.EntityCollection<Cohorts>();
                            saveHelp.SchoolGroups = new System.Data.Objects.DataClasses.EntityCollection<SchoolGroups>();
                            saveHelp.HelpUserLevels = new System.Data.Objects.DataClasses.EntityCollection<HelpUserLevels>();

                            Int16? maxListOrder = null;

                            try
                            {
                                maxListOrder = context.Help.Where(h => h.Pages.PageID == help.Pages.PageID).Max(h => h.ListOrder);
                            }
                            catch
                            {
                                maxListOrder = 0;
                            }

                            if (maxListOrder.HasValue)
                                saveHelp.ListOrder = maxListOrder.Value;
                            else
                                saveHelp.ListOrder = 0;

                            saveHelp.ListOrder++;
                        }
                        else
                        {
                            if (!Validation.Help.IsHelpIDValid(context, help.HelpID))
                                throw Web09Exception.GetBusinessException(Web09MessageList.HelpInvalidID);

                            saveHelp = context.Help.Include("HelpUserLevels").Include("Pages").Include("Cohorts").Include("SchoolGroups").Where(h => h.HelpID == help.HelpID).First();
                            
                            if (saveHelp.Pages.PageID != help.Pages.PageID)
                            {   
                                Int16? maxListOrder =null;

                                try
                                {
                                    maxListOrder = context.Help.Where(h => h.Pages.PageID == help.Pages.PageID).Max(h => h.ListOrder);
                                }
                                catch
                                {
                                    maxListOrder = 0;
                                }
                                saveHelp.ListOrder = maxListOrder.Value;
                                saveHelp.ListOrder++;
                            }
                        }

                        saveHelp.Pages = context.Pages.Where(p => p.PageID == help.Pages.PageID).ToList().First();

                        saveHelp.IsActive = help.IsActive;
                        saveHelp.IsJune = help.IsJune;
                        saveHelp.IsSept = help.IsSept;

                        saveHelp.HelpTitle = help.HelpTitle.Trim();
                        saveHelp.HelpText = help.HelpText.Trim();

                        List<Cohorts> cohortList = context.Cohorts.ToList();
                        List<SchoolGroups> schoolGroupList = TSSchoolGroup.GetFAQSchoolGroups(context);                        

                        saveHelp.Cohorts.Clear();
                        if (help.Cohorts != null && help.Cohorts.Count > 0)
                            for (int counter = 0; counter < help.Cohorts.ToList().Count; counter++)
                                saveHelp.Cohorts.Add((from c in cohortList where c.KeyStage == help.Cohorts.ToList()[counter].KeyStage select c).First());

                        saveHelp.SchoolGroups.Clear();
                        // remove old cohort add new cohort
                        if (help.SchoolGroups != null && help.SchoolGroups.Count > 0)
                            for (int counter = 0; counter < help.SchoolGroups.ToList().Count; counter++)
                                saveHelp.SchoolGroups.Add((from c in schoolGroupList where c.SchoolGroupID == help.SchoolGroups.ToList()[counter].SchoolGroupID select c).First());

                        //saveHelp.HelpUserLevels.Clear();                       

                        if (saveHelp.HelpID == 0) // new faq
                            context.AddToHelp(saveHelp);
                        else
                            context.ApplyPropertyChanges("Help", saveHelp);

                        context.SaveChanges();

                        if (help.HelpUserLevels != null && help.HelpUserLevels.Count > 0)
                        {
                            System.Data.Common.DbConnection con = conn.StoreConnection;
                            System.Data.Common.DbCommand cmd = con.CreateCommand();
                            cmd.Connection = con;
                            cmd.CommandType = System.Data.CommandType.Text;

                            for (int counter = 0; counter < help.HelpUserLevels.ToList().Count; counter++)
                            {                                
                                cmd.CommandText = "INSERT INTO School.HelpUserLevels(HelpID, UserLevel) VALUES (" + saveHelp.HelpID.ToString() + ",'" + help.HelpUserLevels.ElementAt(counter).UserLevel + "')";
                                cmd.ExecuteNonQuery();
                                //saveHelp.HelpUserLevels.Add(new HelpUserLevels { UserLevel = help.HelpUserLevels.ElementAt(counter).UserLevel, Help = saveHelp });
                            }
                        }
                    }
                }
                transaction.Complete();
            }
        }
    }
}
