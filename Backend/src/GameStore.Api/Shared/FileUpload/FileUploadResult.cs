namespace GameStore.Api.Shared.FileUpload;

public struct FileUploadResult
{
    public bool IsSuccess { get; set; }
    public string? FilePath { get; set; }
    public string? ErrorMessage { get; set; }
}