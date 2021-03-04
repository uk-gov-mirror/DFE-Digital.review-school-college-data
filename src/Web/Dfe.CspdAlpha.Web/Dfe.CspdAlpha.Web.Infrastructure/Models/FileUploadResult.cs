using System;

namespace Dfe.Rscd.Web.Infrastructure.Models
{
    public class FileUploadResult
    {
        public Guid FileId { get; set; }

        public long FileSizeInBytes { get; internal set; }

        public string FileName { get; internal set; }
        
        public string FolderName { get; internal set; }
    }
}
