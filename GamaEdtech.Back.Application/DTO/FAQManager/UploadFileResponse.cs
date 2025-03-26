using GamaEdtech.Back.Domain.DataAccess.Requests.Media;
using GamaEdtech.Back.Domain.DataAccess.Responses.Media;

namespace GamaEdtech.Back.Application.DTO.FAQManager;

public class UploadFileResult
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
    public Domain.DataAccess.Responses.Media.UploadFileResponse UploadFileResult { get; init; }
}