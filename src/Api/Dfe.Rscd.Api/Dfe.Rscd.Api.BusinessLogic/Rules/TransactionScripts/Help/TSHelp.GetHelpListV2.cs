using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Web.Script.Serialization;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSHelp : Logic.TSBase
    {
        public static string GetHelpListForPageV2(string pageName, string keyStageList, string schoolGroupIDList, String userLevel, bool? active)
        {
          List<Web09.Services.Common.JSONObjects.HelpItem> helpItems = new List<Web09.Services.Common.JSONObjects.HelpItem>();

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
                      cmd.CommandText = "School.GetHelpListForPage";

                      cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                      {
                          DbType = System.Data.DbType.String,
                          Direction = System.Data.ParameterDirection.Input,
                          ParameterName = "@PageName",
                          SqlValue = pageName
                      });

                      if (!string.IsNullOrEmpty(keyStageList))
                      {
                          cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                          {
                              DbType = System.Data.DbType.String,
                              Direction = System.Data.ParameterDirection.Input,
                              ParameterName = "@KeyStageList",
                              SqlValue = keyStageList
                          });
                      }

                      if (!string.IsNullOrEmpty(schoolGroupIDList))
                      {
                          cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                          {
                              DbType = System.Data.DbType.String,
                              Direction = System.Data.ParameterDirection.Input,
                              ParameterName = "@SchoolGroupIDList",
                              SqlValue = schoolGroupIDList
                          });
                      }

                      if (!string.IsNullOrEmpty(userLevel))
                      {
                          cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter
                          {
                              DbType = System.Data.DbType.String,
                              Direction = System.Data.ParameterDirection.Input,
                              ParameterName = "@UserLevel",
                              SqlValue = userLevel
                          });
                      }
                    
                      System.Data.Common.DbDataReader dr = cmd.ExecuteReader();
                      while (dr.Read())
                      {
                          Web09.Services.Common.JSONObjects.HelpItem helpItem = new Web09.Services.Common.JSONObjects.HelpItem();

                          helpItem.HelpID    = Convert.ToInt32(dr["HelpID"]);
                          helpItem.HelpTitle = dr.IsDBNull(dr.GetOrdinal("HelpTitle")) ? string.Empty : dr["HelpTitle"].ToString();
                          helpItem.HelpText  = dr.IsDBNull(dr.GetOrdinal("HelpText")) ? string.Empty : dr["HelpText"].ToString();
                          helpItem.HelpCohorts = dr.IsDBNull(dr.GetOrdinal("HelpCohorts")) ? string.Empty : dr["HelpCohorts"].ToString();

                          helpItems.Add(helpItem);
                      }

                  }
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }

          JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
          string jsonData = jsSerializer.Serialize(helpItems);

          return jsonData;           
        }
    }
}
