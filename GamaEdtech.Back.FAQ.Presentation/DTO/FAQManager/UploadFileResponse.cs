using GamaEdtech.Back.FAQ.Domain.DataAccess.Mapper.FAQ;

namespace GamaEdtech.Back.FAQ.Application.DTO.FAQManager
{
    public class UploadFileResponse
    {
        public List<UploadResponsePerProvider> uploadResponsePerProviders { get; set; } = [];

        public UploadResponsePerProvider GetUploaderFileResultsAllUploaded()
        {
            return uploadResponsePerProviders.Where(c =>
            c.UploadFileResult.FileResults.All(a => a.FileUploadStatus)).FirstOrDefault();
        }
    }

    public class UploadResponsePerProvider
    {
        public string UploaderName { get; init; }
        public UploadFileResult UploadFileResult { get; init; }
    }
}