using Dfe.CspdAlpha.Web.Infrastructure.Interfaces;
using Dfe.CspdAlpha.Web.Infrastructure.Models;
using Dfe.CspdAlpha.Web.Shared.Config;
using Microsoft.Extensions.Options;
using Microsoft.SharePoint.Client;
using System;
using System.IO;
using System.Linq;

namespace Dfe.CspdAlpha.Web.Infrastructure.SharePoint
{
    public class SharePointFileUploadService : IFileUploadService
    {
        private readonly SharePointOptions _options;

        public SharePointFileUploadService(IOptions<SharePointOptions> options)
        {
            _options = options.Value;
        }

        public FileUploadResult UploadFile(Stream file, string filename, string mimeType, string folderName)
        {
            // TODO: Consider updating this to use PnP Core when it supports .NET Standard (https://github.com/pnp/PnP-Sites-Core)
            Uri site = new Uri(_options.SiteUrl);
            const int fileChunkSizeInMB = 3;
            Microsoft.SharePoint.Client.File uploadFile = null;

            using (var authenticationManager = new AuthenticationManager(_options.TenantId))
            {
                using (var ctx = authenticationManager.GetContext(site, _options.ClientId, _options.ClientSecret))
                {
                    // https://docs.microsoft.com/en-us/sharepoint/dev/solution-guidance/upload-large-files-sample-app-for-sharepoint
                    
                    // Each sliced upload requires a unique id
                    Guid uploadId = Guid.NewGuid();

                    List uploadLibrary = ctx.Web.Lists.GetByTitle("File upload");

                    // create dedicated folder if one has not already been created for this amendment
                    FolderCollection folders = uploadLibrary.RootFolder.Folders;
                    ctx.Load(folders);
                    ctx.ExecuteQuery();

                    folderName = folderName.Trim();

                    var targetFolder = folders.FirstOrDefault(x => x.Name == folderName);

                    if (targetFolder == null)
                    {
                        // create folder
                        targetFolder = folders.Add(folderName);
                        ctx.Load(targetFolder);
                        ctx.ExecuteQuery();
                    }

                    // Calculate block size in bytes
                    int blockSize = fileChunkSizeInMB * 1024 * 1024;

                    // Get the size of the file
                    long fileSize = file.Length;

                    if (fileSize <= blockSize)
                    {
                        // Use regular approach
                        FileCreationInformation fileInfo = new FileCreationInformation();
                        fileInfo.ContentStream = file;
                        fileInfo.Url = filename;
                        fileInfo.Overwrite = true;
                        uploadFile = targetFolder.Files.Add(fileInfo);
                        ctx.Load(uploadFile);
                        ctx.ExecuteQuery();
                    }
                    else
                    {
                        // Use large file upload approach
                        ClientResult<long> bytesUploaded = null;

                        using (BinaryReader br = new BinaryReader(file))
                        {
                            byte[] buffer = new byte[blockSize];
                            long fileoffset = 0;
                            long totalBytesRead = 0;
                            int bytesRead;
                            bool first = true;

                            // Read data from filesystem in blocks 
                            while ((bytesRead = br.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                totalBytesRead = totalBytesRead + bytesRead;

                                if (first)
                                {
                                    using (MemoryStream contentStream = new MemoryStream())
                                    {
                                        // Add an empty file.
                                        FileCreationInformation fileInfo = new FileCreationInformation();
                                        fileInfo.ContentStream = contentStream;
                                        fileInfo.Url = filename;
                                        fileInfo.Overwrite = true;
                                        uploadFile = targetFolder.Files.Add(fileInfo);

                                        // Start upload by uploading the first slice. 
                                        using (MemoryStream s = new MemoryStream(buffer))
                                        {
                                            // Call the start upload method on the first slice
                                            bytesUploaded = uploadFile.StartUpload(uploadId, s);
                                            ctx.ExecuteQuery();
                                            // fileoffset is the pointer where the next slice will be added
                                            fileoffset = bytesUploaded.Value;
                                        }

                                        // we can only start the upload once
                                        first = false;
                                    }
                                }
                                else
                                {
                                    // Get a reference to our file
                                    uploadFile = ctx.Web.GetFileByServerRelativeUrl(
                                        targetFolder.ServerRelativeUrl + System.IO.Path.AltDirectorySeparatorChar + filename);

                                    if (totalBytesRead == fileSize)
                                    {
                                        // We've reached the end of the file
                                        using (MemoryStream s = new MemoryStream(buffer, 0, bytesRead))
                                        {
                                            // End sliced upload by calling FinishUpload
                                            uploadFile = uploadFile.FinishUpload(uploadId, fileoffset, s);
                                            ctx.ExecuteQuery();

                                            break;
                                        }
                                    }
                                    else
                                    {
                                        using (MemoryStream s = new MemoryStream(buffer))
                                        {
                                            // Continue sliced upload
                                            bytesUploaded = uploadFile.ContinueUpload(uploadId, fileoffset, s);
                                            ctx.ExecuteQuery();
                                            // update fileoffset for the next slice
                                            fileoffset = bytesUploaded.Value;
                                        }
                                    }
                                }

                            } // while ((bytesRead = br.Read(buffer, 0, buffer.Length)) > 0)
                        }
                    }
                }
            }

            return new FileUploadResult
            {
                FileId = uploadFile.UniqueId,
                FileName = filename,
                FileSizeInBytes = uploadFile.Length
            };
        }
    }
}