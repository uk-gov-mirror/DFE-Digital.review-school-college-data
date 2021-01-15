using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
	public partial class TSSchool: Logic.TSBase
	{

        public static List<SmallSchoolClass> GetAllSchoolsForLA( int LANumber )
        {
            try
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {

                        System.Data.Common.DbConnection connection = conn.StoreConnection;

                        System.Data.Common.DbCommand cmd = connection.CreateCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "School.GetAllSchoolsForLA";

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@LANumber",
                            SqlValue = LANumber
                        });

                        System.Data.Common.DbDataReader dr = cmd.ExecuteReader();
                        List<SmallSchoolClass> lst = new List<SmallSchoolClass>();
                        while (dr.Read())
                        {
                            SmallSchoolClass sc = new SmallSchoolClass();
                            sc.DFESNumber = Convert.ToInt32(dr["DFESNumber"]);
                            sc.SchoolName = dr["SchoolName"].ToString();

                            lst.Add(sc);
                        }
                        return lst;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
	}
}
