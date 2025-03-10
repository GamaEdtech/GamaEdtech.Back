﻿using GamaEdtech.Back.FAQ.Domain.DataAccess.Mapper.FAQ;

namespace GamaEdtech.Back.FAQ.Application.DTO.Mapping
{
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
}
