using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Transactions;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSSchool : Logic.TSBase
    {
        public static IList<SchoolValues> GetSchoolValuesByGroup(int dfesNumber, short keystage, string groupType)
        {
            IList<SchoolValues> list = new List<SchoolValues>();

            using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope() )
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        try
                        {

                            string groupName = "";

                            if (groupType.Equals("PupilProgress") && keystage == 2)
                            {
                                groupName = "KS2Progress";
                            }

                            if (groupType.Equals("PupilProgress") && keystage == 4)
                            {
                                groupName = "KS4Progress";
                            }

                            if (groupType.Equals("NarrowingGaps") && keystage == 2)
                            {
                                groupName = "KS2NarrowingGaps";
                            }

                            if (groupType.Equals("NarrowingGaps") && keystage == 4)
                            {
                                groupName = "KS4NarrowingGaps";
                            }

                            if (groupType.Equals("PupilAttainment") && keystage == 2)
                            {
                                groupName = "KS2Attainment";
                            }

                            if (groupType.Equals("PupilAttainment") && keystage == 4)
                            {
                                groupName = "KS4Attainment";
                            }
                           

                            var result = (from sv in context.SchoolValues
                                          join sgvt in context.SchoolGroupValueTypes on sv.SchoolValueTypes.ValueTypeID equals sgvt.SchoolValueTypes.ValueTypeID
                                          where sgvt.SchoolGroups.SchoolGroupName.Contains(groupName)
                                          && sv.DFESNumber == dfesNumber
                                          && sv.SchoolValueTypes.KeyStage == keystage
                                          orderby sgvt.ListOrder ascending
                                          select new { sv, sv.SchoolValueTypes, sgvt, sgvt.SchoolGroups }).ToList();

                            for (int i = 0; i < result.Count(); i++)
                            {
                                SchoolValues a = result[i].sv;
                                
                                if(!a.SchoolValueTypesReference.IsLoaded)
                                    a.SchoolValueTypesReference.Load();

                                if(!result[i].sgvt.SchoolGroupsReference.IsLoaded)
                                    result[i].sgvt.SchoolGroupsReference.Load();

                                a.SchoolValueTypes.SchoolGroupValueTypes.Attach(result[i].sgvt);
                                list.Add(a);
                            }

                        }
                        catch
                        {
                            throw new BusinessLevelException("Error getting values for the school.");
                        }

                    }
                }

                transaction.Complete();
            }

            return list;
        }
    }
}
