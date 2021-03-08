using System;
using System.IO;
using Dfe.Rscd.Web.Infrastructure.Models;

namespace Dfe.Rscd.Web.Infrastructure.Interfaces
{
    public interface IFileUploadService
    {
        FileUploadResult UploadFile(Stream file, string filename, string mimeType, string folderName);

        bool DeleteFile(Guid fileId);
    }
}
