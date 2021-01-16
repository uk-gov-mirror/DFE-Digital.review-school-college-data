using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSUserManagement : TSBase
    {
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

    }
}
