using System;
using System.Data.EntityClient;
using System.Linq;
using System.Transactions;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSUserSurvey : Logic.TSBase
    {
        public static void SaveUserSurvey(UserSurveyInfo userSurveyInfo)
        {
            using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope() )
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        UserSurvey saveObj= null;

                        if (userSurveyInfo.UserSurveyID == 0) // new Object                        
                            saveObj = new UserSurvey
                                {
                                    Answer1 = userSurveyInfo.Answer1,
                                    Answer2 = userSurveyInfo.Answer2,
                                    Answer3Clarity = userSurveyInfo.Answer3Clarity,
                                    Answer3Comments = userSurveyInfo.Answer3Comments,
                                    Answer3Completeness = userSurveyInfo.Answer3Completeness,
                                    Answer4aClarity = userSurveyInfo.Answer4aClarity,
                                    Answer4aCompleteness = userSurveyInfo.Answer4aCompleteness,
                                    Answer4bComments = userSurveyInfo.Answer4bComments,
                                    Answer5 = userSurveyInfo.Answer5,
                                    Answer6Comments = userSurveyInfo.Answer6Comments,
                                    Forename = userSurveyInfo.UserContext.Forename,
                                    Username = userSurveyInfo.UserContext.UserName,
                                    Rolename = userSurveyInfo.UserContext.RoleName,
                                    Surname = userSurveyInfo.UserContext.Surname,
                                    SurveyDate = DateTime.Now,
                                    Schools=context.Schools.Where(s=>s.DFESNumber==userSurveyInfo.DCSFNumber).FirstOrDefault()
                                };
                        else
                            throw new BusinessLevelException("Survey edit not implemented yet");
                        
                        if (userSurveyInfo.UserSurveyID== 0) // new faq
                            context.AddToUserSurvey(saveObj);
                        else
                            context.ApplyPropertyChanges("UserSurvey", saveObj);

                        context.SaveChanges();
                    }
                }
                transaction.Complete();
            }
        }
    }
}
