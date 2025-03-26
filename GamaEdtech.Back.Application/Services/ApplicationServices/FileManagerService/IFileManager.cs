using GamaEdtech.Back.Application.DTO.FAQManager;
using GamaEdtech.Back.Domain.DataAccess.Requests.Media;

namespace GamaEdtech.Back.Application.Services.ApplicationServices.FileManagerService
{
    public interface IFileManager
    {
        Task<UploadFileResult> UploadFiles(UploadFileRequest uploadFileRequest, string bucketName, CancellationToken cancellationToken);
    }
}