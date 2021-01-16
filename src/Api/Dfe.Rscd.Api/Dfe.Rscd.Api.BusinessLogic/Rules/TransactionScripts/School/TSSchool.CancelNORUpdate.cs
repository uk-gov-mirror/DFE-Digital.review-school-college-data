using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Transactions;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSSchool : Logic.TSBase
    {
        public static void AddCancelledSchoolValue(SchoolRequests schoolRequest, Changes oldChange, string type, Changes changeObj, Schools school, Web09_Entities context)
        {
            try
            {
                IEnumerable<SchoolValues> schoolValueList = context.SchoolValues
                    .Include("Schools")
                    .Include("Schools.SchoolRequests")
                    .Include("Schools.SchoolRequests.SchoolRequestChanges")
                    .Where(sv => sv.Schools.DFESNumber == school.DFESNumber &&
                        sv.SchoolValueTypes.ValueTypeCode == type &&
                        sv.SchoolValueTypes.KeyStage == 4);
                        
                        
                //if there is an existing value set its old value back as current
                if (schoolValueList.Count() > 0)
                {
                    // Get old value for this type and school
                    SchoolValues schoolValue = schoolValueList
                        .Where(sv => sv.ChangeID < schoolRequest.Changes.ChangeID)
                        .OrderByDescending(ob => ob.ChangeID)
                        .FirstOrDefault();

                    if (schoolValue != null)
                        CreateSchoolValuesObject(type, schoolValue.Value, changeObj, school, context);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CancelNORUpdate(int requestID, UserContext uc)
        {
            using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope() )
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {

                        // create change object
                        Changes newChange = CreateChangeObject(context, 1, uc);
                        context.AddToChanges(newChange);
                        context.SaveChanges();

                        SchoolRequests schoolRequest = context.SchoolRequests
                                   .Include("Schools")
                                   .Include("Changes")
                                   .Where(src => src.SchoolRequestID == requestID)
                                   .FirstOrDefault();

                        if (schoolRequest != null)
                        {
                            AddCancelledSchoolValue(schoolRequest, schoolRequest.Changes, "S2SENSP", newChange, schoolRequest.Schools, context);
                            AddCancelledSchoolValue(schoolRequest, schoolRequest.Changes, "S2SENA", newChange, schoolRequest.Schools, context);
                            AddCancelledSchoolValue(schoolRequest, schoolRequest.Changes, "BOYSKS4", newChange, schoolRequest.Schools, context);
                            AddCancelledSchoolValue(schoolRequest, schoolRequest.Changes, "GIRLSKS4", newChange, schoolRequest.Schools, context);                            

                            SchoolRequestChanges srcCurrent = schoolRequest.SchoolRequestChanges.Where(src => src.DateEnd == null).First();
                            srcCurrent.DateEnd = DateTime.Now;
                            context.ApplyPropertyChanges("SchoolRequestChanges", srcCurrent);

                            // create new request
                            SchoolRequestChanges srNew = new SchoolRequestChanges
                            {
                                Changes = newChange,
                                Comments = srcCurrent.Comments,
                                DCSFNotification = srcCurrent.DCSFNotification,
                                ScrutinyStatus = context.ScrutinyStatus.First(s => s.ScrutinyStatusDescription.Equals("Cancelled")),
                                SchoolRequests = schoolRequest
                            };

                            // new request status
                            context.AddToSchoolRequestChanges(srNew);
                            context.SaveChanges();
                        }
                    }
                }

                transaction.Complete();
            }
        }
    }
    }
