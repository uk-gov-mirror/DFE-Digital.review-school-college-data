using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Microsoft.AspNetCore.Http;

namespace Dfe.CspdAlpha.Web.Application.Application.Interfaces
{
    public interface ISchoolService
    {
        SchoolViewModel GetSchoolViewModel(string urn);
        PupilListViewModel GetPupilListViewModel(string urn);
        List<string> UploadEvidence(List<IFormFile> files);

        bool CreateAddPupilAmendment(AddPupilAmendmentViewModel addPupilAmendment, out string id);
    }
}
