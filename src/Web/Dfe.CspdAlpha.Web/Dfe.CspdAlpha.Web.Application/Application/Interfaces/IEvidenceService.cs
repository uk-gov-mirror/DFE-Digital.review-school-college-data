using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Dfe.Rscd.Web.Application.Application.Interfaces
{
    public interface IEvidenceService
    {
        string UploadEvidence(IEnumerable<IFormFile> files);

        void RelateEvidence(Guid amendmentId, string evidenceFolderName);
    }
}
