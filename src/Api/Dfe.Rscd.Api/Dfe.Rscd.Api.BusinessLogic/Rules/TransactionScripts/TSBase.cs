using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;

namespace Web09.Checking.Business.Logic
{
    public class TSBase
    {
        public static Changes CreateChangeObject(Web09_Entities context, int changeTypeId, UserContext userContext)
        {

            var changeObj = new Changes();

            //Get the Change Type
            var changeTypeQry = context.ChangeType.Where(ct => ct.ChangeTypeID == changeTypeId).Select(ct => ct).ToList();

            if (changeTypeQry.Count == 0)
            {
                throw new ApplicationException(String.Format("No change type exists with change type id of {0}",
                                                             changeTypeId));
            }

            changeObj.ChangeTypeID = changeTypeQry[0].ChangeTypeID;

            changeObj.ChangeDate = DateTime.Now;
            changeObj.UserName = userContext.UserName;
            changeObj.Forename = userContext.Forename;
            changeObj.Surname = userContext.Surname;
            changeObj.RoleName = userContext.RoleName;
            return changeObj;
        }


        protected static void SetInputParamForCommand(DbCommand cmd, string paramName, object paramValue)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.Value = paramValue;
            param.Direction = ParameterDirection.Input;
            param.DbType = DbType.AnsiString;
            cmd.Parameters.Add(param);
        }

        protected static void SetFixedLengthInputParamForCommand(DbCommand cmd, string paramName, object paramValue)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.Value = paramValue;
            param.Direction = ParameterDirection.Input;
            param.DbType = DbType.StringFixedLength;
            cmd.Parameters.Add(param);
        }
    }
}
