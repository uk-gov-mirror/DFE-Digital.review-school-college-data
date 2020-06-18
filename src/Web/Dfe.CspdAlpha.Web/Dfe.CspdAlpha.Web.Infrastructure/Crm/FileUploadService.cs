using Dfe.CspdAlpha.Web.Infrastructure.Interfaces;
using Dfe.CspdAlpha.Web.Infrastructure.Models;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Dfe.CspdAlpha.Web.Infrastructure.Crm
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IOrganizationService _organizationService;

        public FileUploadService(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        public FileUploadResult UploadFile(Stream file, string filename, string mimeType)
        {
            // TODO: Change this method to use the Web API to make it async
            // https://docs.microsoft.com/en-us/powerapps/developer/common-data-service/file-attributes#upload-file-data
            var upload = new cr3d5_Fileupload();

            upload.cr3d5_Filename = filename;

            using (var context = new CrmServiceContext(_organizationService))
            {
                context.AddObject(upload);
                context.SaveChanges();
            }

            const int bufferSize = 4 * 1024 * 1024; // 4 MB

            try
            {
                var initUploadRequest = new InitializeFileBlocksUploadRequest
                {
                    FileName = filename,
                    Target = new EntityReference("cr3d5_fileupload", upload.Id),
                    FileAttributeName = "cr3d5_file"
                };

                var initUploadResponse = (InitializeFileBlocksUploadResponse)_organizationService.Execute(initUploadRequest);
                var blockIds = new List<string>();

                using (file)
                {
                    var buffer = new byte[bufferSize];

                    for (var offset = 0; offset < file.Length; offset += bufferSize)
                    {
                        file.Read(buffer, 0, bufferSize);
                        var blockId = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

                        var uploadRequest = new UploadBlockRequest
                        {
                            BlockData = buffer,
                            BlockId = HttpUtility.UrlEncode(blockId),
                            FileContinuationToken = initUploadResponse.FileContinuationToken
                        };

                        var uploadResponse = (UploadBlockResponse)_organizationService.Execute(uploadRequest);

                        blockIds.Add(blockId);
                    }
                }

                var commitUploadRequest = new CommitFileBlocksUploadRequest
                {
                    BlockList = blockIds.ToArray(),
                    FileContinuationToken = initUploadResponse.FileContinuationToken,
                    FileName = filename,
                    MimeType = mimeType
                };

                var commitUploadResponse = (CommitFileBlocksUploadResponse)_organizationService.Execute(commitUploadRequest);
            }
            catch (Exception)
            {
                using (var context = new CrmServiceContext(_organizationService))
                {
                    context.DeleteObject(upload);
                    context.SaveChanges();
                }

                throw;
            }

            return new FileUploadResult
            {
                FileId = upload.Id
            };
        }
    }
}