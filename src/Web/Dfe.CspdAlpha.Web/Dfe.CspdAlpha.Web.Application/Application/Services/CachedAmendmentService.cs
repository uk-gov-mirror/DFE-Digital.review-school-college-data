using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Application.Cache.Redis;
using Dfe.Rscd.Web.Application.Application.Interfaces;
using Dfe.Rscd.Web.Application.Models.ViewModels.Amendments;

namespace Dfe.Rscd.Web.Application.Application.Services
{
    public class CachedAmendmentService : ContextAwareService, IAmendmentService
    {
        private const string GetAmendmentKeyFormat = "GetAmendmentsListViewModel{0}{1}";
        private readonly IRedisCache _cache;
        private readonly IAmendmentService _amendmentService;

        public CachedAmendmentService(IRedisCache cache, IClient client)
        {
            _cache = cache;
            _amendmentService = new AmendmentService(client);
        }

        public AmendmentsListViewModel GetAmendmentsListViewModel(string urn)
        {
            return _cache.GetOrCreate(string.Format(GetAmendmentKeyFormat, CheckingWindow, urn), () => _amendmentService.GetAmendmentsListViewModel(urn),
                null, databaseId: RedisDb.General);
        }

        public AmendmentOutcome CreateAmendment(Amendment amendment)
        {
            _cache.Remove(string.Format(GetAmendmentKeyFormat, CheckingWindow, amendment.Urn));
            return _amendmentService.CreateAmendment(amendment);
        }

        public Amendment GetAmendment(string id)
        {
            return _amendmentService.GetAmendment(id);
        }

        public bool CancelAmendment(string id)
        {
            var amendment = GetAmendment(id);
            _cache.Remove(string.Format(GetAmendmentKeyFormat, CheckingWindow, amendment.Urn));
            return _amendmentService.CancelAmendment(id);
        }

        public bool RelateEvidence(string amendmentId, string evidenceFolder)
        {
            var amendment = GetAmendment(amendmentId);
            _cache.Remove(string.Format(GetAmendmentKeyFormat, CheckingWindow, amendment.Urn));
            return _amendmentService.RelateEvidence(amendmentId, evidenceFolder);
        }
    }
}
