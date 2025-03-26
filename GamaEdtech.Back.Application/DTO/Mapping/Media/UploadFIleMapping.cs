using GamaEdtech.Back.Domain.DataAccess.Requests.Media;
using Microsoft.AspNetCore.Http;

namespace GamaEdtech.Back.Application.DTO.Mapping.Media;

public static class UploadFileMapping
{
    public static async Task<UploadFileRequest> MapToUploadFileRequest(this List<IFormFile> files)
    {
        var fileRequests = new List<FileRequest>();

        foreach (var file in files)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileRequest = new FileRequest
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                FileDate = memoryStream.ToArray()
            };

            fileRequests.Add(fileRequest);
        }

        return new UploadFileRequest { Files = fileRequests };
    }
}
