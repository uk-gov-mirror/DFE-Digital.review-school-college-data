using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Application.Cache.Redis;
using Dfe.Rscd.Web.Application.Application.Interfaces;
using Dfe.Rscd.Web.Application.Models.School;
using Dfe.Rscd.Web.Application.Models.ViewModels;

namespace Dfe.Rscd.Web.Application.Application.Services
{
    public class CachedEstablishmentService : IEstablishmentService
    {
        private readonly IRedisCache _cache;
        private readonly IEstablishmentService _establishmentService;

        public CachedEstablishmentService(IRedisCache cache, IClient client)
        {
            _cache = cache;
            _establishmentService = new EstablishmentService(client);
        }

        public string GetSchoolName(string laestab)
        {
            return _cache.GetOrCreate("GetSchoolName" + laestab, () => _establishmentService.GetSchoolName(laestab),
                null, databaseId: RedisDb.General);
        }

        public SchoolDetails GetSchoolDetails(string urn)
        {
            return _cache.GetOrCreate("GetSchoolDetails" + urn, () => _establishmentService.GetSchoolDetails(urn),
                null, databaseId: RedisDb.General);
        }

        public SchoolViewModel GetSchoolViewModel(string urn)
        {
            return _cache.GetOrCreate("GetSchoolViewModel" + urn, () => _establishmentService.GetSchoolViewModel(urn),
                null, databaseId: RedisDb.General);
        }
    }
}
