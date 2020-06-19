using System;
using System.Collections.Generic;
using System.Text;

namespace Dfe.CspdAlpha.Web.Infrastructure.Mock.Services
{
    public class EvidenceService : IEvidenceService
    {
        public string UploadEvidenceFile(string file)
        {
            return $"file-{Guid.NewGuid().ToString().ToLower()}";
        }
    }
}
