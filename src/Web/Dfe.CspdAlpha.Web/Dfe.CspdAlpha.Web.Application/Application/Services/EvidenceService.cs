using System;
using System.Collections.Generic;
using Dfe.Rscd.Web.Application.Application.Interfaces;
using Dfe.Rscd.Web.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Dfe.Rscd.Web.Application.Application.Services
{
    public class EvidenceService : IEvidenceService
    {
        private IFileUploadService _fileUploadService;

        public EvidenceService(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        public string UploadEvidence(IEnumerable<IFormFile> files)
        {
            var now = DateTime.UtcNow;
            var folderName = $"{now:yyyy-MM-dd-HH-mm-ss}-{Guid.NewGuid()}";
            var validFileReceived = false;

            foreach (var file in files)
            {
                if (file.Length == 0)
                {
                    continue;
                }

                validFileReceived = true;

                using (var fs = file.OpenReadStream())
                {
                    _fileUploadService.UploadFile(fs, file.FileName, file.ContentType, folderName);
                }
            }

            return validFileReceived ? folderName : null;
        }

        public void RelateEvidence(Guid amendmentId, string evidenceFolderName)
        {
            throw new NotImplementedException();
            //_amendmentService.RelateEvidence(amendmentId, evidenceFolderName, true);
        }
    }
}
