using Azure.Storage.Blobs;
using GamaEdtech.Back.FAQ.Domain.Common.Exceptions;
using GamaEdtech.Back.FAQ.Domain.Common.Utilities;
using GamaEdtech.Back.FAQ.Domain.DataAccess.Mapper.FAQ;
using System.Collections.Concurrent;

namespace GamaEdtech.Back.FAQ.Infrastructure.Services.MediaServices
{
    public class AzureUploadFile(string connectionString, Dictionary<string, string> containers) : IFileUploader
    {
        public string UploaderProviderName => "Azure";

        public ValueTask<List<FileResult>> GetFilesUrl(string[] fileAddresses, string bucketName)
        {
            var files = new List<FileResult>();

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(bucketName);

            var fileUrlTasks = fileAddresses.Select(fileAddress =>
            {
                return (fileAddress, UrlFunc: containerClient.GenerateSasUri
                (Azure.Storage.Sas.BlobContainerSasPermissions.Read,
                DateTimeOffset.UtcNow.AddMinutes(10)));
            });

            foreach ((string fileAddress, Uri UrlFunc) in fileUrlTasks)
            {
                Uri uri = UrlFunc;
                files.Add(new FileResult
                {
                    ContentType = string.Empty,
                    FileAddress = fileAddress, 
                    Url = uri.ToString()
                });
            }
            return ValueTask.FromResult(files);
        }

        public async Task<UploadFileResult> UploadFile(UploadFileRequest uploadFileRequest, string bucketName, CancellationToken cancellationToken = default)
        {
            try
            {
                var containerName = FindContainer(bucketName);
                if (!containerName.HasValue())
                {
                    throw new NotFoundException();
                }

                var files = new ConcurrentBag<FileResult>();

                var blobServiceClient = new BlobServiceClient(connectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

                var fileUploadTasks = uploadFileRequest.Files.Select(file =>
                {
                    var blobClient = containerClient.GetBlobClient(file.FileName);
                    return (file.ContentType, file.FileName, UploadTask: 
                    blobClient.UploadAsync(new MemoryStream(file.FileDate), overwrite: true, cancellationToken));
                });

                foreach (var (ContentType, FileName, UploadTask) in fileUploadTasks)
                {
                    try
                    {
                        var uploadResult = await UploadTask;
                        files.Add(new FileResult
                        {
                            ContentType = ContentType,
                            FileName = FileName,
                        });
                    }
                    catch 
                    {
                        //continue;
                        throw;
                    }
                }

                return new UploadFileResult { FileResults = [.. files] };
            }
            catch
            {
                return new UploadFileResult() { FileResults = [] };
            }
        }

        private string FindContainer(string bucketName)
        {
            if (containers.TryGetValue(bucketName, out var container))
            {
                return container;
            }
            return string.Empty;
        }
    }
}