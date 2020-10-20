using System;
using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;

namespace Dfe.CspdAlpha.Web.Application.Application.Interfaces
{
    public interface IAmendmentService
    {
        AmendmentsListViewModel GetAmendmentsListViewModel(string urn, CheckingWindow checkingWindow);

        Dictionary<string, string> GetRemoveReasons(string reason = null);

        string CreateAmendment(Amendment amendment);

        Amendment GetAmendment(CheckingWindow checkingWindow, string id);

        bool CancelAmendment(CheckingWindow checkingWindow, string id);

        bool RelateEvidence(CheckingWindow checkingWindow, string amendmentid, string evidencefolder);
    }
}
