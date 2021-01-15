using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSSchool : Logic.TSBase
    {

        internal static String GenerateSchoolListQuery(String schoolDfesNumber, List<Cohorts> cohorts, List<SchoolGroups> schoolGroups, Boolean? submissionPosted, Int16? messageID)
        {
            String strQuery = "SELECT VALUE s FROM Web09_Entities.Schools AS s WHERE 1=1 ";
            // DFESNUmber param                
            if (schoolDfesNumber.Length > 0)
            {
                String vstrDfesNumberDivider = "1";
                Int32 vdfesNumberDivider = 1;
                Int32 vintdfesNumber = 1;

                vdfesNumberDivider = Int32.Parse(vstrDfesNumberDivider.PadRight((7 - schoolDfesNumber.Length) + 1, '0'));
                vintdfesNumber = Int32.Parse(schoolDfesNumber);
                strQuery += " && s.DFESNumber / " + vdfesNumberDivider.ToString() + " == " + vintdfesNumber + "";
            }

            // Keystage List Param                    
            if (cohorts.Count > 0)
            {
                string keyStageIDs = String.Join(",", cohorts.ConvertAll<String>(k => k.KeyStage.ToString()).ToArray());
                if (keyStageIDs.Trim() != "")
                    strQuery += " && EXISTS(SELECT VALUE SC FROM s.SchoolCheckingStatus AS SC WHERE SC.Cohorts.KeyStage IN{" + keyStageIDs + "})";
            }

            // SchoolGroups
            if (schoolGroups.Count > 0)
            {
                string schoolGroupIDs = String.Join(",", schoolGroups.ConvertAll<string>(s => s.SchoolGroupID.ToString()).ToArray());
                strQuery += " && EXISTS(SELECT VALUE SG FROM s.SchoolGroups AS SG WHERE SG.SchoolGroupID IN {" + schoolGroupIDs + "})";
            }

            //Schools school;
            //school.SchoolCheckingStatus.ToList()[0].CheckingStatus.CheckingStatusDescription = "Not Checked";
            if (submissionPosted.HasValue)
            {
                if (submissionPosted.Value)
                    strQuery += " && !EXISTS(SELECT VALUE SCS FROM s.SchoolCheckingStatus AS SCS WHERE SCS.CheckingStatus.CheckingStatusDescription == 'Not Checked')";
                else
                    strQuery += " &&  EXISTS(SELECT VALUE SCS FROM s.SchoolCheckingStatus AS SCS WHERE SCS.CheckingStatus.CheckingStatusDescription == 'Not Checked')";
            }

            // Schools which are already assigned to messageID, should not be part of search again.
            // As assigned schools are accessible from Message.GetMessageByID method
            if (messageID.HasValue)
                strQuery += " && !EXISTS(SELECT VALUE MSG FROM s.Messages AS MSG WHERE MSG.MessageID == " + messageID.ToString() + ")";                    

            return strQuery;
        }

        public static List<Schools> GetSchoolList(Int32 page, Int32 rowsPerPage, String schoolDfesNumber, List<Cohorts> cohorts, List<SchoolGroups> schoolGroups, Boolean? submissionPosted, Int16? messageID)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();                

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    ObjectQuery<Schools> query = context.CreateQuery<Schools>(GenerateSchoolListQuery(schoolDfesNumber, cohorts, schoolGroups, submissionPosted, messageID));

                    List<Schools> list = query.OrderBy(s=>s.DFESNumber).Skip((page - 1) * rowsPerPage)
                            .Take(rowsPerPage)
                            .ToList();
                    return list;
                }
            }
        }

        public static Int32 GetSchoolListCount(String schoolDfesNumber, List<Cohorts> cohorts, List<SchoolGroups> schoolGroups, Boolean? submissionPosted, Int16? messageID)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    ObjectQuery<Schools> query = context.CreateQuery<Schools>(GenerateSchoolListQuery(schoolDfesNumber, cohorts, schoolGroups, submissionPosted, messageID));
                    return query.Count();
                }
            }
        }

        public static List<Schools> GetSchoolList(String schoolDfesNumber, List<Cohorts> cohorts, List<SchoolGroups> schoolGroups, Boolean? submissionPosted, Int16? messageID)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    ObjectQuery<Schools> query = context.CreateQuery<Schools>(GenerateSchoolListQuery(schoolDfesNumber, cohorts, schoolGroups, submissionPosted, messageID));

                    List<Schools> list = query.OrderBy(s => s.DFESNumber).ToList();                            
                    return list;
                }
            }
        }

        public static List<Schools> GetSchoolList(Int32 page, Int32 rowsPerPage, Int16 messageID)
        {
            List<Schools> schoolList = new List<Schools>();

            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {

                    System.Data.Common.DbConnection connection = conn.StoreConnection;

                    System.Data.Common.DbCommand cmd = connection.CreateCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "School.GetSchoolListForMessage";

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@PageNumber",
                        SqlValue = page
                    });

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@RowsPerPage",
                        SqlValue = rowsPerPage
                    });

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Direction = System.Data.ParameterDirection.Input,
                        ParameterName = "@MessageID",
                        SqlValue = messageID
                    });

                    using (System.Data.Common.DbDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Schools school = new Schools();
                            school.DFESNumber = Convert.ToInt32(dr[0]);
                            school.SchoolName = Convert.ToString(dr[1]);

                            schoolList.Add(school);
                        }         
                    }
                                     
                }
            }

            return schoolList;
        }
    }
}
