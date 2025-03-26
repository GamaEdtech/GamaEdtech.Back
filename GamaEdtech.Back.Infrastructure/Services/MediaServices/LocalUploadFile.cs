using GamaEdtech.Back.Domain.DataAccess.Requests.Media;
using GamaEdtech.Back.Domain.DataAccess.Responses.Media;
using System.Collections.Concurrent;

namespace GamaEdtech.Back.Infrastructure.Services.MediaServices
{
    public class LocalUploadFile(string rootDirectory) : BaseFileUploader(rootDirectory, null)
    {
        public override string UploaderProviderName => "Local";

        public override ValueTask<List<FileResponse>> GetFilesUrl(string[] fileAddresses, string bucketName)
        {
            var files = new List<FileResponse>();

            foreach (var fileAddress in fileAddresses)
            {
                var filePath = Path.Combine(rootDirectory, bucketName, fileAddress);
                if (File.Exists(filePath))
                {
                    files.Add(new FileResponse
                    {
                        ContentType = string.Empty,
                        FileAddress = fileAddress,
                        Url = $"/{bucketName}/{fileAddress}"
                    });
                }
            }

            return ValueTask.FromResult(files);
        }

        public async override Task<UploadFileResponse> UploadFile(UploadFileRequest uploadFileRequest, string bucketName, CancellationToken cancellationToken)
        {
            try
            {
                var targetDirectory = Path.Combine(rootDirectory, bucketName);

                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                var files = new ConcurrentBag<FileResponse>();

                var fileUploadTasks = uploadFileRequest.Files.Select(file =>
                {
                    var filePath = Path.Combine(targetDirectory, file.FileName);
                    return (file.ContentType, file.FileName, UploadTask: File.WriteAllBytesAsync(filePath, file.FileDate, cancellationToken));
                });

                foreach (var (ContentType, FileName, UploadTask) in fileUploadTasks)
                {
                    try
                    {
                        await UploadTask;
                        files.Add(new FileResponse
                        {
                            ContentType = ContentType,
                            FileName = FileName,
                            Url = $"/{bucketName}/{FileName}"
                        });
                    }
                    catch
                    {
                        throw;
                    }
                }

                return new UploadFileResponse { FileResults = files.ToList() };
            }
            catch
            {
                return new UploadFileResponse { FileResults = new List<FileResponse>() };
            }
        }
    }
}