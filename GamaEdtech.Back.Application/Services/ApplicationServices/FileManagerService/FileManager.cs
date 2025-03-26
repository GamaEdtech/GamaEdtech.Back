using GamaEdtech.Back.Application.DTO.FAQManager;
using GamaEdtech.Back.Application.Services.ApplicationServices.FileManagerService;
using GamaEdtech.Back.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.Domain.DataAccess.Requests.Media;
using GamaEdtech.Back.Infrastructure.Services.MediaServices;
using System.Collections.Concurrent;

namespace GamaEdtech.Back.FAQ.Application.Services.ApplicationServices.FileManagerService
{
    public class FileManager(IEnumerable<IFileUploader> fileUploaders, string[] uploaderNames) : IFileManager, IScopedDependency
    {
        public async Task<UploadFileResult> UploadFiles(UploadFileRequest uploadFileRequest, string bucketName, CancellationToken cancellationToken)
        {
            var uploadFileResponse = new UploadFileResult();
            var concurrentUploadResponsePerProvider = new ConcurrentBag<UploadResponsePerProvider>();

            var uploaderTasks = fileUploaders.Where(c => uploaderNames.Any(a => a == c.UploaderProviderName))
            .Select(s => (s.UploaderProviderName, UploadTask: s.UploadFile(uploadFileRequest, bucketName, cancellationToken)));

            foreach (var (UploaderName, UploadTask) in uploaderTasks)
            {
                var result = await UploadTask;
                concurrentUploadResponsePerProvider.Add(new UploadResponsePerProvider
                {
                    UploadFileResult = result,
                    UploaderName = UploaderName
                });
            }
            uploadFileResponse.uploadResponsePerProviders.AddRange([.. concurrentUploadResponsePerProvider]);
            return uploadFileResponse;
        }
    }
}