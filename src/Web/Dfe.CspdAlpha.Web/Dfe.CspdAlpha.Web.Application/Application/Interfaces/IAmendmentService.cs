using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Application.Interfaces
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
