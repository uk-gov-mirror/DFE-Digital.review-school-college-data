using System;
using System.Collections.Generic;
using Dfe.Rscd.Web.Application.Application.Interfaces;
using Dfe.Rscd.Web.Infrastructure.Interfaces;
using Dfe.Rscd.Web.Infrastructure.Models;
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

        public FileUploadResult UploadEvidence(IFormFile file)
        {
            var now = DateTime.UtcNow;
            var folderName = $"{now:yyyy-MM-dd-HH-mm-ss}-{Guid.NewGuid()}";

            return UploadEvidence(folderName, file);
        }

        public FileUploadResult UploadEvidence(string folderName, IFormFile file)
        {
            using (var fs = file.OpenReadStream())
            {
                return _fileUploadService.UploadFile(fs, file.FileName, file.ContentType, folderName);
            }
        }

        public bool DeleteEvidenceFile(Guid fileId)
        {
            return _fileUploadService.DeleteFile(fileId);
        }

        public void RelateEvidence(Guid amendmentId, string evidenceFolderName)
        {
            throw new NotImplementedException();
            //_amendmentService.RelateEvidence(amendmentId, evidenceFolderName, true);
        }
    }
}
