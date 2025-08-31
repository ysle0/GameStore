namespace GameStore.Api.Shared.FileUpload;

public struct FileUploadResult
{
    public bool IsSuccess { get; set; }
    public Uri? FilePath { get; set; }
    public string? ErrorMessage { get; set; }
}