using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Dfe.CspdAlpha.Web.Application.Application.Interfaces
{
    public interface IEvidenceService
    {
        string UploadEvidence(IEnumerable<IFormFile> files);

        void RelateEvidence(Guid amendmentId, string evidenceFolderName);
    }
}
