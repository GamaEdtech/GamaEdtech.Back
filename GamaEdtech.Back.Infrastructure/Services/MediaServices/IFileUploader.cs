using GamaEdtech.Back.Domain.DataAccess.Requests.Media;
using GamaEdtech.Back.Domain.DataAccess.Responses.Media;

namespace GamaEdtech.Back.Infrastructure.Services.MediaServices
{
    public interface IFileUploader
    {
        string UploaderProviderName { get; }
        ValueTask<List<FileResponse>> GetFilesUrl(string[] fileAddresses, string bucketName);
        Task<UploadFileResponse> UploadFile(UploadFileRequest uploadFileRequest, string bucketName, CancellationToken cancellationToken);
    }

    public abstract class BaseFileUploader(string connectionString, Dictionary<string, string> containers) : IFileUploader
    {
        public abstract string UploaderProviderName { get; }

        public abstract ValueTask<List<FileResponse>> GetFilesUrl(string[] fileAddresses, string bucketName);

        public abstract Task<UploadFileResponse> UploadFile(UploadFileRequest uploadFileRequest, string bucketName, CancellationToken cancellationToken);

        public string FindContainer(string bucketName)
        {
            if (containers.TryGetValue(bucketName, out var container))
            {
                return container;
            }
            return string.Empty;
        }
    }
}
