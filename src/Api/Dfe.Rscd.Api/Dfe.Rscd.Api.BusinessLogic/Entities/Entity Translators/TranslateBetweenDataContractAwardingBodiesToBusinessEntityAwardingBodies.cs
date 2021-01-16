using System.Collections.Generic;

using Web09.Checking.DataAccess;

namespace Web09.Services.Adapters.EntityTranslators
{
    public class TranslateBetweenDataContractAwardingBodiesToBusinessEntityAwardingBodies
    {
        public static Web09.Services.DataContracts.AwardingBodyCollection TranslateBusinessEntityAwardingBodyCollectionToDataContractAwardingbodyCollection(List<AwardingBodies> awardingBodies)
        {
            var awardingBodyCollection = new Web09.Services.DataContracts.AwardingBodyCollection();
            awardingBodyCollection.AddRange(awardingBodies.ConvertAll(r => TranslateBusinessEntityAwardingBodyToDataContractAwardingBody(r)));
            return awardingBodyCollection;
        }

        public static Web09.Services.DataContracts.AwardingBody TranslateBusinessEntityAwardingBodyToDataContractAwardingBody(AwardingBodies awardingbody)
        {
            return new Web09.Services.DataContracts.AwardingBody
            {
                AwardingBodyID = awardingbody.AwardingBodyID,
                AwardingBodyName = awardingbody.AwardingBodyCode+": "+ awardingbody.AwardingBodyName,
            };
        }

    }
}
