namespace GamaEdtech.Back.Domain.DataAccess.Responses.Media;

public class UploadFileResponse
{
    public List<FileResponse> FileResults { get; init; }
}
public class FileResponse
{
    public bool FileUploadStatus { get; init; }
    public string FileName { get; init; }
    public string FileAddress { get; init; }
    public string ContentType { get; init; }
    public string Url { get; init; }
}
