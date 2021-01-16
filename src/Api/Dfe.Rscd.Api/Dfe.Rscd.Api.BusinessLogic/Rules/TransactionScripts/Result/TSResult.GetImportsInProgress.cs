using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Web.Script.Serialization;
using Web09.Checking.DataAccess;
using Web09.Services.Common.JSONObjects;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSResult : Logic.TSBase
    {
        public static string GetImportsInProgress()
        {
            List<ImportInProgress> importsInProgress = new List<ImportInProgress>();

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
                        cmd.CommandText = "Import.GetImportsInProgress";
                   
                        System.Data.Common.DbDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            ImportInProgress importInProgress = new ImportInProgress();

                            importInProgress.ImportProcedureName = Convert.ToString(dr[0]);
                            importsInProgress.Add(importInProgress);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            string jsonData = jsSerializer.Serialize(importsInProgress);

            return jsonData;   
        }
    }
}
