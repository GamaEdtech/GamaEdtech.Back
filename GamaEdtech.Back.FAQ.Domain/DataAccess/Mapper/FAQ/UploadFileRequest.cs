namespace GamaEdtech.Back.FAQ.Domain.DataAccess.Mapper.FAQ
{
    public class UploadFileRequest
    {
        public List<FileRequest> Files { get; init; }
    }

    public class UploadFileResult
    {
        public List<FileResult> FileResults { get; init; }
    }

    public class FileRequest
    {
        public string FileName { get; init; }
        public byte[] FileDate { get; init; }
        public string ContentType { get; init; }
    }

    public class FileResult
    {
        public bool FileUploadStatus { get; init; }
        public string FileName { get; init; }
        public string FileAddress { get; init; }
        public string ContentType { get; init; }
        public string Url { get; init; }
    }
}
