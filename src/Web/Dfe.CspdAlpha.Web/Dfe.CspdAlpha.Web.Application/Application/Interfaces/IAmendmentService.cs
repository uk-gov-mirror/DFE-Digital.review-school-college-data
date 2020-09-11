using System;
using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Microsoft.AspNetCore.Http;

namespace Dfe.CspdAlpha.Web.Application.Application.Interfaces
{
    public interface IAmendmentService
    {
        AmendmentsListViewModel GetAmendmentsListViewModel(string urn, bool lateChecking);

        AmendmentViewModel GetAddPupilAmendmentViewModel(Guid id);

        bool CancelAmendment(string id);

        string UploadEvidence(List<IFormFile> files);

        void RelateEvidence(Guid amendmentId, string evidenceFolderName);

        bool CreateAddPupilAmendment(AddPupilAmendmentViewModel addPupilAmendment, out string id);
    }
}
