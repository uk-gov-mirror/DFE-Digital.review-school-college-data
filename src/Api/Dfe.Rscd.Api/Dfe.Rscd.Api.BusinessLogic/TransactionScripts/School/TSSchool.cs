using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Checking.Infrastructure.CosmosDb.Core;
using Web09.Checking.Infrastructure.CosmosDb.Interfaces;
using Web09.Checking.Infrastructure.CosmosDb.Services;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSSchool : TSBase
    {
        internal static bool IsSchoolNonPlasc(Web09_Entities context, int dfesNumber)
        {
            // MDP: Return true for now as no concept of this in Cosmos Data
            bool isSchoolNonPlasc = false;

            return isSchoolNonPlasc;
        }

        public static bool IsSchoolNonPlasc(int dfesNumber)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return IsSchoolNonPlasc(context, dfesNumber);
                }
            }
        }

        internal static bool IsSchoolIndependant(Web09_Entities context, int dfesNumber)
        {
            return HasSchoolGroup(context, dfesNumber, "Independent");
        }

        internal static bool IsSchoolMaintained(Web09_Entities context, int dfesNumber)
        {
            return HasSchoolGroup(context, dfesNumber, "Maintained");
        }

        internal static bool IsSchoolFE(Web09_Entities context, int dfesNumber)
        {
            return HasSchoolGroup(context, dfesNumber, "FE");
        }

        private static bool HasSchoolGroup(Web09_Entities context, int dfesNumber, string schoolGroup)
        {
            Schools school = context.Schools
                .Include("SchoolGroups")
                .FirstOrDefault(s => s.DFESNumber == dfesNumber);

            bool hasSchoolGroup = false;

            foreach (SchoolGroups sg in school.SchoolGroups)
            {
                if (sg.SchoolGroupName == schoolGroup)
                {
                    hasSchoolGroup = true;
                    break;
                }
            }
            return hasSchoolGroup;
        }

        internal static bool ParticipatesInKeyStage(Web09_Entities context, int dfesNumber, short keyStage)
        {
            Schools school = context.Schools
                .Include("SchoolCheckingStatus")
                .FirstOrDefault(s => s.DFESNumber == dfesNumber);

            foreach(SchoolCheckingStatus scs in school.SchoolCheckingStatus)
            {                
                if ((keyStage == 4 && scs.Cohorts.KeyStage == 4 && scs.Schools.LowestAge >= 16)) return false;
                if (scs.Cohorts.KeyStage == keyStage) return true;
            }

            //If we reach this point we've iterated through all key stages and
            //the provided key stage does not exist.
            return false;
        }

        

        internal static bool IsSchoolRecognisedAndMaintained(int dfesNumber)
        {
            using(EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using(Web09_Entities context = new Web09_Entities(conn))
                {
                    Schools school = context.Schools
                        .Include("SchoolGroups")
                        .Where(s => s.DFESNumber == dfesNumber)
                        .FirstOrDefault();

                    if (school == null)
                        return false;

                    foreach (SchoolGroups sg in school.SchoolGroups)
                    {
                        if (sg.SchoolGroupName.ToLower() == "maintained")
                            return true;
                    }

                    //If this point is reached, the school is not maintained.
                    return false;

                }
            }

        }

        internal static bool IsRecognizedDCSF(int dfesNumber)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return IsRecognizedDCSF(context, dfesNumber);
                }
            }
        }

        internal static bool IsRecognizedDCSF(Web09_Entities context, int dfesNumber)
        {
            int schoolCount = context.Schools
                .Where(s => s.DFESNumber == dfesNumber)
                .Count();

            if (schoolCount == 0)
                return false;
            else
                return true;
        }

        #region School Requests Validations

        /// <summary>
        /// Validate that an existing school request does not already exist for a
        /// given school
        /// </summary>
        /// <param name="context">Web09 EF context object.</param>
        /// <param name="studentId">The dfesNumber of the school for whom the check is to be carried out for.</param>
        /// <returns>Boolean to indicate whether the validation is successful or not.</returns>
        public static bool ValidateSchoolHasNoPriorSchoolRequests(Web09_Entities context, int dfesNumber)
        {
            var existingSchoolRequest = context.SchoolRequests
                .Where(sr => sr.Schools.DFESNumber == dfesNumber)
                .OrderByDescending(sr => sr.SchoolRequestID)
                .Select(sr => sr).ToList();

            if (existingSchoolRequest.Count > 0)
            {
                //if a request exists and if it has been cancelled, accepted or rejected
                //then no open/pending request exists. so return true
                SchoolRequests sr = existingSchoolRequest.First();
                var existingSchoolRequestChanges = context.SchoolRequestChanges
                .Where(src => src.SchoolRequests.SchoolRequestID == sr.SchoolRequestID
                        && src.DateEnd == null
                        && (src.ScrutinyStatus.ScrutinyStatusCode == "C"
                                || src.ScrutinyStatus.ScrutinyStatusCode == "A"
                                || src.ScrutinyStatus.ScrutinyStatusCode == "AA"
                                || src.ScrutinyStatus.ScrutinyStatusCode == "R"))                
                .Select(src => src).ToList();

                if (existingSchoolRequestChanges.Count > 0)
                    return true;
                else
                    return false;

            }                
            else
                return true;

        }

        #endregion

        #region School Status Confirmation
        public static bool RequireSchoolStatusConfirmation(string username)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    if(context.SchoolStatusConfirmation.Where(obj=>obj.Username == username).FirstOrDefault()!=null)
                        return false;
                    else
                        return true;
                }
            }
        }

        public static string SaveSchoolStatusConfirmation(UserContext userContext, string action, string  consumerURL, string  newDCSFNumber, string  newEmail, string  oldUserName, string  username)
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

                    // create SchoolStatusConfirmation object
                    byte val = byte.Parse(action);
                    SchoolStatusConfirmation obj = new SchoolStatusConfirmation
                        {
                            Action = val,
                            ChangeID=newChange.ChangeID,
                            NewDCSFNumber=newDCSFNumber,
                            NewEmail=newEmail,
                            OldUsername=oldUserName,
                            Username=username
                        };

                    context.AddToSchoolStatusConfirmation(obj);

                    if (action == "2")
                    {
                        UserPasswordLetters letter = new UserPasswordLetters
                        {
                            CreatedChangeID = newChange.ChangeID,
                            ConsumerURL = consumerURL,
                            Username = username
                        };

                        context.AddToUserPasswordLetters(letter);
                    }

                    context.SaveChanges();

                    return "saved";
                }
            }
        }
        #endregion
    }
}
