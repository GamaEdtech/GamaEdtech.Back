using GamaEdtech.Back.FAQ.Application.DTO.FAQManager;
using GamaEdtech.Back.FAQ.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.FAQ.Domain.DataAccess.Mapper.FAQ;
using GamaEdtech.Back.FAQ.Infrastructure.Services.MediaServices;

namespace GamaEdtech.Back.FAQ.Application.Services.ApplicationServices.FileManagerService
{
    public class FileManager(IEnumerable<IFileUploader> fileUploaders, string[] uploaderNames) : IFileManager, IScopedDependency
    {
        public async Task<UploadFileResponse> UpladFiles(UploadFileRequest uploadFileRequest, string bucketName, CancellationToken cancellationToken)
        {
            var uploadFileResponse = new UploadFileResponse();

            var uploaderTasks = fileUploaders.Where(c => uploaderNames.Any(a => a == c.UploaderProviderName))
            .Select(s => (s.UploaderProviderName, UploadTask: s.UploadFile(uploadFileRequest, bucketName, cancellationToken)));

            foreach (var (UploaderName, UploadTask) in uploaderTasks)
            {
                var result = await UploadTask;
                uploadFileResponse.uploadResponsePerProviders.Add(new UploadResponsePerProvider
                {
                    UploadFileResult = result,
                    UploaderName = UploaderName

                });
            }
            return uploadFileResponse;
        }
    }
}