using System.Collections.Generic;
using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Application.Cache.Redis;
using Dfe.Rscd.Web.Application.Application.Interfaces;
using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;
using Dfe.Rscd.Web.Application.Models.ViewModels.RemovePupil;

namespace Dfe.Rscd.Web.Application.Application.Services
{
    public class CachedPupilService : IPupilService
    {
        private readonly IPupilService _service;
        private readonly IRedisCache _cache;

        public CachedPupilService(IClient client, IRedisCache cache)
        {
            _service = new PupilService(client);
            _cache = cache;
        }

        public List<PupilViewModel> GetPupilDetailsList(SearchQuery searchQuery)
        {
            return _cache.GetOrCreate("GetPupilDetailsList"+searchQuery.CheckingWindow+searchQuery.URN+searchQuery.Query, () => _service.GetPupilDetailsList(searchQuery), null, databaseId: RedisDb.General);
        }

        public MatchedPupilViewModel GetPupil(string id)
        {
            return _cache.GetOrCreate("GetPupil"+id, () => _service.GetPupil(id), null, databaseId: RedisDb.General);
        }

        public MatchedPupilViewModel GetMatchedPupil(string upn)
        {
            return _cache.GetOrCreate("GetMatchedPupil"+upn, () => _service.GetMatchedPupil(upn), null, databaseId: RedisDb.General);
        }

        public List<AmendmentReason> GetAmendmentReasons(AmendmentType amendmentType)
        {
            return _cache.GetOrCreate("GetAmendmentReasons"+amendmentType, () => _service.GetAmendmentReasons(amendmentType), null, databaseId: RedisDb.General);
        }
    }
}
