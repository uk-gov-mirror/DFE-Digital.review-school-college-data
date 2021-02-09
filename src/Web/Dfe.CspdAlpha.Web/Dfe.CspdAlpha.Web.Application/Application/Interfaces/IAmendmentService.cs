using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Models.ViewModels.Amendments;

namespace Dfe.Rscd.Web.Application.Application.Interfaces
{
    public interface IAmendmentService
    {
        AmendmentsListViewModel GetAmendmentsListViewModel(string urn);

        AmendmentOutcome CreateAmendment(Amendment amendment);

        Amendment GetAmendment(string id);

        bool CancelAmendment(string id);

        bool RelateEvidence(string amendmentId, string evidenceFolder);
    }
}
