using System.IO;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using Dfe.CspdAlpha.Web.Infrastructure.Interfaces;
using Dfe.CspdAlpha.Web.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly ILogger<FileUploadController> _logger;
        private readonly IAmendmentService _amendmentService;
        private readonly IFileUploadService _fileUploadService;

        public FileUploadController(ILogger<FileUploadController> logger,
            IAmendmentService amendmentService,
            IFileUploadService fileUploadService)
        {
            _logger = logger;
            _amendmentService = amendmentService;
            _fileUploadService = fileUploadService;
        }

        public IActionResult UploadFile()
        {
            //TODO: Usual security controls around file uploads

            //result = _fileUploadService.UploadFile(
            //    file.OpenReadStream(), file.FileName, file.ContentType);

            FileUploadResult result;

            using (FileStream stream = System.IO.File.Open("filepath", FileMode.Open))
            {
                result = _fileUploadService.UploadFile(
                stream, "filename", "application/vnd.oasis.opendocument.text");
            }

            return Json(result);
        }
    }
}
