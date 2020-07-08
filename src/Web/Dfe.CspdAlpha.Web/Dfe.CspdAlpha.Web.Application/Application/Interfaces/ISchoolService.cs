using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Microsoft.AspNetCore.Http;

namespace Dfe.CspdAlpha.Web.Application.Application.Interfaces
{
    public interface ISchoolService
    {
        SchoolViewModel GetSchoolViewModel(string urn);
        PupilListViewModel GetPupilListViewModel(string urn);
        AmendmentsListViewModel GetAmendmentsListViewModel(string urn);

        bool CancelAmendment(string id);
        List<EvidenceFile> UploadEvidence(List<IFormFile> files);

        bool CreateAddPupilAmendment(AddPupilAmendmentViewModel addPupilAmendment, out string id);

        bool UpdateConfirmation(TaskListViewModel taskListViewModel, string userId, string urn);
        TaskListViewModel GetConfirmationRecord(string userId, string urn);
    }
}
