using System.Collections.Generic;
using System.Linq;

using Web09.Services.DataContracts;

namespace Web09.Services.Adapters.EntityTranslators
{
    public class TranslateBetweenDataContractAdjustmentReasonsAndBusinessEntityAdjustmentReasons
    {

        public static AdjustmentReasons TranslateBusinessEntityAdjustmentReasonsToDataContractAdjustmentReasons(IEnumerable<Web09.Checking.DataAccess.InclusionAdjustmentReasons> reasonsListIn)
        {
            DataContracts.AdjustmentReasons reasonsListOut = new DataContracts.AdjustmentReasons();
            reasonsListOut.AddRange(reasonsListIn.ToList<Web09.Checking.DataAccess.InclusionAdjustmentReasons>().ConvertAll(r => TranslateBusinessEntityAdjustmentReasonToDataContractAdjustmentReason(r)));
            return reasonsListOut;
        }

        public static AdjustmentReason TranslateBusinessEntityAdjustmentReasonToDataContractAdjustmentReason(Web09.Checking.DataAccess.InclusionAdjustmentReasons reasonIn)
        {
            AdjustmentReason reasonOut = new AdjustmentReason();
            reasonOut.Code = reasonIn.IncAdjReasonID;
            reasonOut.Description = reasonIn.IncAdjReasonDescription;
            return reasonOut;
        }
    }
}
