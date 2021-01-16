using System;
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
        public static IList<SchoolValues> GetSchoolValues(int dfesNumber, short keystage, string valueType, bool latest)
        {
            IList<SchoolValues> list = new List<SchoolValues>();

            using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope())
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        try
                        {
                            //when returning a value for a particular type, return the latest
                            if (!string.IsNullOrEmpty(valueType))
                            {
                                var query = context.SchoolValues
                                                   .Include("SchoolValueTypes")
                                                  .Where(sv => sv.DFESNumber == dfesNumber
                                                            && sv.SchoolValueTypes.KeyStage == keystage
                                                            && sv.SchoolValueTypes.ValueTypeCode.Equals(valueType))
                                                  .ToList();

                                if (query.Count > 0)
                                {
                                    int maxChangeID = 0;
                                    if(latest)
                                        maxChangeID=  query.Max(sv => sv.ChangeID);
                                    else
                                        maxChangeID = query.Min(sv => sv.ChangeID);

                                    list = context.SchoolValues
                                                .Include("SchoolValueTypes")
                                                 .Where(sv => sv.DFESNumber == dfesNumber
                                                            && sv.SchoolValueTypes.KeyStage == keystage
                                                            && sv.SchoolValueTypes.ValueTypeCode.Equals(valueType)
                                                        && sv.ChangeID == maxChangeID)
                                                 .ToList();
                                }
                            }
                            else //return all school values
                            {
                                string keystagestring = "KS" + keystage.ToString() + "AATFigures";

                                //get the school group according to the keystage
                                var q1 = context.SchoolGroups
                                                   .Where(sg1 => sg1.SchoolGroupName.Contains(keystagestring))
                                                   .ToList();

                                if (q1.Count > 0)
                                {
                                    SchoolGroups sg = q1.First();

                                    var result = (from sv in context.SchoolValues
                                                  join sgvt in context.SchoolGroupValueTypes on sv.SchoolValueTypes.ValueTypeID equals sgvt.SchoolValueTypes.ValueTypeID
                                                  where sgvt.SchoolGroups.SchoolGroupID == sg.SchoolGroupID
                                                  && sv.DFESNumber == dfesNumber
                                                  && sv.SchoolValueTypes.KeyStage == keystage                                                  
                                                  orderby sgvt.ListOrder ascending
                                                  select new { sv}).ToList();

                                    for (int i = 0; i < result.Count(); i++)
                                    {
                                        SchoolValues a = result[i].sv;
                                        a.SchoolValueTypesReference.Load();
                                        list.Add(a);
                                    }


                                }
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
    
        public static string GetSchoolValue(int dfesNumber, string schoolValueTypeCode, int keyStage)
        {
            using (var conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();
                using (var context = new Web09_Entities(conn))
                {
                    var results = (from sv in context.SchoolValues
                                  where sv.DFESNumber == dfesNumber
                                        && sv.SchoolValueTypes.ValueTypeCode == schoolValueTypeCode
                                        && sv.SchoolValueTypes.KeyStage == keyStage
                                  select sv.Value);
                    if (results.Count() == 0) return "0";
                    return results.First();
                }
            }   
        }

        public static IList<SchoolValues> GetSchoolValuesSummary(string dfesNumber, string keystage)
        {
            IList<SchoolValues> list = new List<SchoolValues>();

            using (TransactionScope transaction = TransactionScopeFactory.CreateTransactionScope() )
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();
                    System.Data.Common.DbConnection connection = conn.StoreConnection;
                    System.Data.Common.DbCommand cmd = connection.CreateCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "School.GetSchoolValues";


                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@DFESNumber",
                        SqlValue = dfesNumber
                    });

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.Int16,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@KeyStage",
                        SqlValue = keystage
                    });

                    System.Data.Common.DbDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        SchoolValues aValue = new SchoolValues();
                        aValue.SchoolValueTypes = new SchoolValueTypes();
                        bool isHeader = false;

                        if (!dr.IsDBNull(dr.GetOrdinal("DFESNumber")))
                        {
                            aValue.DFESNumber = int.Parse(dr["DFESNumber"].ToString());
                        }
                        if (!dr.IsDBNull(dr.GetOrdinal("ValueTypeID")))
                        {
                            aValue.ValueTypeID = short.Parse(dr["ValueTypeID"].ToString());
                            aValue.SchoolValueTypes.ValueTypeID = short.Parse(dr["ValueTypeID"].ToString());
                        }
                        if (!dr.IsDBNull(dr.GetOrdinal("ChangeID")))
                        {
                            aValue.ChangeID = int.Parse(dr["ChangeID"].ToString());
                        }
                        if (!dr.IsDBNull(dr.GetOrdinal("Value")))
                        {
                            aValue.Value = dr["Value"].ToString();
                        }

                        if (!dr.IsDBNull(dr.GetOrdinal("DateEnd")))
                        {
                            aValue.DateEnd = DateTime.Parse(dr["DateEnd"].ToString());
                        }
                        if (!dr.IsDBNull(dr.GetOrdinal("IsHeader")))
                        {
                            isHeader = bool.Parse(dr["IsHeader"].ToString());
                        }

                        if (!dr.IsDBNull(dr.GetOrdinal("ValueTypeCode")))
                        {
                            aValue.SchoolValueTypes.ValueTypeCode = dr["ValueTypeCode"].ToString();
                        }

                        if (!dr.IsDBNull(dr.GetOrdinal("ValueTypeDescription")))
                        {
                            if (!isHeader)
                                aValue.SchoolValueTypes.ValueTypeDescription = dr["ValueTypeDescription"].ToString();
                            else
                                aValue.SchoolValueTypes.ValueTypeDescription = "HEADER:" + dr["ValueTypeDescription"].ToString();
                        }
                        list.Add(aValue);
                    }
                }

                transaction.Complete();
            }

            return list;
        }

    }
}
