using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Application.Cache.Redis;
using Dfe.Rscd.Web.Application.Application.Interfaces;
using Dfe.Rscd.Web.Application.Models.ViewModels;

namespace Dfe.Rscd.Web.Application.Application.Services
{
    public class CachedSchoolService : ISchoolService
    {
        private readonly IRedisCache _cache;
        private readonly ISchoolService _schoolService;

        public CachedSchoolService(IRedisCache cache, IEstablishmentService establishmentService, IClient apiClient)
        {
            _cache = cache;
            _schoolService = new SchoolService(establishmentService, apiClient);
        }

        public bool UpdateConfirmation(TaskListViewModel taskListViewModel, string userId, string urn)
        {
            return _schoolService.UpdateConfirmation(taskListViewModel, userId, urn);
        }

        public TaskListViewModel GetConfirmationRecord(string userId, string urn)
        {
            return _cache.GetOrCreate("GetConfirmationRecord" + userId + urn,
                () => _schoolService.GetConfirmationRecord(userId, urn), null, databaseId: RedisDb.General);

        }
    }
}
