using GamaEdtech.Back.FAQ.Domain.DataAccess.Mapper.FAQ;

namespace GamaEdtech.Back.FAQ.Infrastructure.Services.MediaServices
{
    public interface IFileUploader
    {
        string UploaderProviderName { get; }
        ValueTask<List<FileResult>> GetFilesUrl(string[] fileAddresses, string bucketName);
        Task<UploadFileResult> UploadFile(UploadFileRequest uploadFileRequest, string bucketName, CancellationToken cancellationToken);
    }
}
