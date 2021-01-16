using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSSchool : Logic.TSBase
    {

        public static List<SchoolValues> GetNORAdjustmentRequestDetails(SchoolRequestChanges src)
        {
            List<SchoolValues> result = new List<SchoolValues>();

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();                

                using (Web09_Entities context = new Web09_Entities(conn))
                {                   
                    // compare with change id of the request rather request change
                    var querySchoolValues = context.SchoolValues
                                .Where(sv => sv.Changes.ChangeID == src.SchoolRequests.Changes.ChangeID
                                            && sv.DateEnd == null && sv.DFESNumber == src.SchoolRequests.Schools.DFESNumber);

                    result = querySchoolValues.ToList();

                    if(result != null)
                    {
                        foreach (SchoolValues v in result)
                        {
                            v.SchoolValueTypesReference.Load();
                            v.SchoolsReference.Load();
                            v.ChangesReference.Load();
                            

                        }
                    }

                    // Load NOR Adjustment's comment section (if available)
                    var NORcomment = (
                        from sc in context.SchoolRequestChanges
                        where sc.ChangeID == src.SchoolRequests.Changes.ChangeID
                        orderby sc.ChangeID descending
                        select sc.Comments
                        ).FirstOrDefault();

                    if (!String.IsNullOrEmpty(NORcomment.ToString()))
                    {
                        SchoolValues svNORComment = new SchoolValues();
                        svNORComment.Value = NORcomment.ToString();
                        
                        result.Add(svNORComment);
                    }
                    
                }
            }

             return result;
        }

        public static List<SchoolValues> GetNORAdjustmentRequestDetailsOriginal(SchoolRequestChanges src)
        {
            List<SchoolValues> result = new List<SchoolValues>();
            List<SchoolValues> results = new List<SchoolValues>();

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {                    
                    // compare with change id of the request rather request change
                    SchoolValues schoolValueSENA = context.SchoolValues
                                .Where
                                (
                                sv =>
                                    sv.DateEnd != null 
                                    && sv.DFESNumber == src.SchoolRequests.Schools.DFESNumber
                                    && sv.SchoolValueTypes.ValueTypeDescription == "Number pupils at end of KS4 with SEN on School Action"
                                )
                                .OrderBy(ob=>ob.ChangeID).FirstOrDefault();

                    if (schoolValueSENA != null)
                    {
                        schoolValueSENA.SchoolValueTypesReference.Load();
                        schoolValueSENA.SchoolsReference.Load();
                        schoolValueSENA.ChangesReference.Load();
                        results.Add(schoolValueSENA);
                    }

                    SchoolValues schoolValueSENSP = context.SchoolValues
                                .Where
                                (
                                sv =>
                                    sv.DateEnd != null
                                    && sv.DFESNumber == src.SchoolRequests.Schools.DFESNumber
                                    && sv.SchoolValueTypes.ValueTypeDescription == "Number pupils at end of KS4 with SEN statements or on School Action Plus"
                                )
                                .OrderBy(ob => ob.ChangeID).FirstOrDefault();

                    if (schoolValueSENSP != null)
                    {
                        schoolValueSENSP.SchoolValueTypesReference.Load();
                        schoolValueSENSP.SchoolsReference.Load();
                        schoolValueSENSP.ChangesReference.Load();
                        results.Add(schoolValueSENSP);
                    }

                    SchoolValues schoolValueBoys = context.SchoolValues
                                .Where
                                (
                                sv =>
                                    sv.DateEnd != null
                                    && sv.DFESNumber == src.SchoolRequests.Schools.DFESNumber
                                    && sv.SchoolValueTypes.ValueTypeDescription == "Number of boys in final year of KS4"
                                )
                                .OrderBy(ob => ob.ChangeID).FirstOrDefault();

                    if (schoolValueBoys != null)
                    {
                        schoolValueBoys.SchoolValueTypesReference.Load();
                        schoolValueBoys.SchoolsReference.Load();
                        schoolValueBoys.ChangesReference.Load();
                        results.Add(schoolValueBoys);
                    }

                    SchoolValues schoolValueGirls = context.SchoolValues
                                .Where
                                (
                                sv =>
                                    sv.DateEnd != null
                                    && sv.DFESNumber == src.SchoolRequests.Schools.DFESNumber
                                    && sv.SchoolValueTypes.ValueTypeDescription == "Number of girls in final year of KS4"
                                )
                                .OrderBy(ob => ob.ChangeID).FirstOrDefault();

                    if (schoolValueGirls != null)
                    {
                        schoolValueGirls.SchoolValueTypesReference.Load();
                        schoolValueGirls.SchoolsReference.Load();
                        schoolValueGirls.ChangesReference.Load();
                        results.Add(schoolValueGirls);
                    }
                }
            }

            return results;
        }
    }

}
