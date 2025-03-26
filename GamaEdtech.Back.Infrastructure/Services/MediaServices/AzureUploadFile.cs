using Azure.Storage.Blobs;
using GamaEdtech.Back.Domain.Common.Exceptions;
using GamaEdtech.Back.Domain.Common.Utilities;
using GamaEdtech.Back.Domain.DataAccess.Requests.Media;
using GamaEdtech.Back.Domain.DataAccess.Responses.Media;
using System.Collections.Concurrent;

namespace GamaEdtech.Back.Infrastructure.Services.MediaServices
{
    public class AzureUploadFile(string connectionString, Dictionary<string, string> containers)
        : BaseFileUploader(connectionString, containers)
    {
        public override string UploaderProviderName => "Azure";


        public override ValueTask<List<FileResponse>> GetFilesUrl(string[] fileAddresses, string bucketName)
        {
            var files = new List<FileResponse>();

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
                files.Add(new FileResponse
                {
                    ContentType = string.Empty,
                    FileAddress = fileAddress,
                    Url = uri.ToString()
                });
            }
            return ValueTask.FromResult(files);
        }

        public override async Task<UploadFileResponse> UploadFile(UploadFileRequest uploadFileRequest, string bucketName, CancellationToken cancellationToken = default)
        {
            try
            {
                var containerName = FindContainer(bucketName);
                if (!containerName.HasValue())
                {
                    throw new NotFoundException();
                }

                var files = new ConcurrentBag<FileResponse>();

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
                        files.Add(new FileResponse
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

                return new UploadFileResponse { FileResults = [.. files] };
            }
            catch
            {
                return new UploadFileResponse() { FileResults = [] };
            }
        }
    }
}