namespace GamaEdtech.Back.Domain.DataAccess.Requests.Media
{
    public class UploadFileRequest
    {
        public List<FileRequest> Files { get; init; }
    }

    public class FileRequest
    {
        public string FileName { get; init; }
        public byte[] FileDate { get; init; }
        public string ContentType { get; init; }
    }
}