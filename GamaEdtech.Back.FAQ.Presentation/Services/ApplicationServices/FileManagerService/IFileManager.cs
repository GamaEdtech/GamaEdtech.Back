
using GamaEdtech.Back.FAQ.Application.DTO.FAQManager;
using GamaEdtech.Back.FAQ.Domain.DataAccess.Mapper.FAQ;

namespace GamaEdtech.Back.FAQ.Application.Services.ApplicationServices.FileManagerService
{
    public interface IFileManager
    {
        Task<UploadFileResponse> UpladFiles(UploadFileRequest uploadFileRequest, string bucketName, CancellationToken cancellationToken);
    }
}