using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Application.Interfaces
{
    public interface IAmendmentService
    {
        AmendmentsListViewModel GetAmendmentsListViewModel(string urn, CheckingWindow checkingWindow);

        AmendmentOutcome CreateAmendment(Amendment amendment);

        Amendment GetAmendment(CheckingWindow checkingWindow, string id);

        bool CancelAmendment(CheckingWindow checkingWindow, string id);

        bool RelateEvidence(CheckingWindow checkingWindow, string amendmentId, string evidenceFolder);
    }
}
