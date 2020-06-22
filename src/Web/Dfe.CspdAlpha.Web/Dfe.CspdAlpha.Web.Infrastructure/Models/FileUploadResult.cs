using System;

namespace Dfe.CspdAlpha.Web.Infrastructure.Models
{
    public class FileUploadResult
    {
        public Guid FileId { get; set; }

        public long FileSizeInBytes { get; internal set; }

        public string FileName { get; internal set; }
    }
}
