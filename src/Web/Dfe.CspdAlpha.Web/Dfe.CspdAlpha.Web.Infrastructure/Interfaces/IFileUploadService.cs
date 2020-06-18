using Dfe.CspdAlpha.Web.Infrastructure.Models;
using System.IO;

namespace Dfe.CspdAlpha.Web.Infrastructure.Interfaces
{
    public interface IFileUploadService
    {
        FileUploadResult UploadFile(Stream file, string filename, string mimeType);
    }
}
