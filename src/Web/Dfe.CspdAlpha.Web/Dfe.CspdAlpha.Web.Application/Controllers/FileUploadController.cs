using System.Threading.Tasks;
using Dfe.Rscd.Web.Application.Security;
using Dfe.Rscd.Web.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dfe.Rscd.Web.Application.Controllers
{
    public class FileUploadController : SessionController
    {
        private readonly ILogger<FileUploadController> _logger;
        private readonly IFileUploadService _fileUploadService;

        public FileUploadController(
            ILogger<FileUploadController> logger,
            IFileUploadService fileUploadService,
            UserInfo userInfo)
            : base(userInfo)
        {
            _logger = logger;
            _fileUploadService = fileUploadService;
        }

        [HttpPost]
        public Task<IActionResult> UploadFileAsync(IFormFile file)
        {
            //TODO: Usual security controls around file uploads

            var result = _fileUploadService.UploadFile(
                file.OpenReadStream(), file.FileName, file.ContentType, null);

            return Task.FromResult((IActionResult)Json(result));
        }
    }
}
