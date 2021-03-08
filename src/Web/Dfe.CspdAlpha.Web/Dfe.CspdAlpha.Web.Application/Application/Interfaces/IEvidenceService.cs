using System;
using System.Collections.Generic;
using Dfe.Rscd.Web.Infrastructure.Models;
using Microsoft.AspNetCore.Http;

namespace Dfe.Rscd.Web.Application.Application.Interfaces
{
    public interface IEvidenceService
    {
        FileUploadResult UploadEvidence(IFormFile file);

        FileUploadResult UploadEvidence(string folderName, IFormFile file);

        bool DeleteEvidenceFile(Guid fileId);

        void RelateEvidence(Guid amendmentId, string evidenceFolderName);
    }
}
