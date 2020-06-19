using System.IO;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly ILogger<FileUploadController> _logger;
        private readonly IFileUploadService _fileUploadService;

        public FileUploadController(ILogger<FileUploadController> logger,
            IFileUploadService fileUploadService)
        {
            _logger = logger;
            _fileUploadService = fileUploadService;
        }

        [HttpPost]
        public Task<IActionResult> UploadFileAsync(IFormFile file)
        {
            //TODO: Usual security controls around file uploads

            var result = _fileUploadService.UploadFile(
                file.OpenReadStream(), file.FileName, file.ContentType);

            return Task.FromResult((IActionResult)Json(result));
        }
    }
}
