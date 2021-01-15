using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSStudent
    {

        public static IEnumerable<InclusionAdjustmentReasons> GetAddNewStudentAdjustmentReasons(int dfesNumber, int keyStage)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using(Web09_Entities context = new Web09_Entities(conn))
                {
                    if(keyStage == 2)
                    {
                        return context.InclusionAdjustmentReasons
                            .Where(iar => iar.IncAdjReasonID == 8 || iar.IncAdjReasonID == 92)
                            .Select(iar => iar)
                            .ToList()
                            .OrderBy(x => x.ListOrder);
                    }
                    else if(keyStage == 4)
                    {
                        bool isSchoolPlasc = !(TSSchool.IsSchoolNonPlasc(context, dfesNumber));

                        if(isSchoolPlasc)
                            return context.InclusionAdjustmentReasons
                                .Where(iar => iar.IncAdjReasonID == 8 || iar.IncAdjReasonID == 10 || iar.IncAdjReasonID == 94)
                                .Select(iar => iar)
                                .ToList()
                                .OrderBy(x => x.ListOrder);
                        else
                            return context.InclusionAdjustmentReasons
                                .Where(iar => iar.IncAdjReasonID == 8 || iar.IncAdjReasonID == 11 || iar.IncAdjReasonID == 94)
                                .Select(iar => iar)
                                .ToList()
                                .OrderBy(x => x.ListOrder);
                    }
                    else if(keyStage == 5)
                    {
                        return context.InclusionAdjustmentReasons
                                .Where(iar => iar.IncAdjReasonID == 95)
                                .Select(iar => iar)
                                .ToList()
                                .OrderBy(x => x.ListOrder);
                    }
                    else
                    {
                        throw Web09Exception.GetBusinessException(Web09MessageList.InvalidKeyStageForPupilAdd);
                    }
                    
                } //dispose context

            }//dispose EntityConnection conn
        }
    }
}
