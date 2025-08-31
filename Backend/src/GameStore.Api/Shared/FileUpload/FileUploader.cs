namespace GameStore.Api.Shared.FileUpload;

public class FileUploader(
    IWebHostEnvironment webHostEnvironment,
    IHttpContextAccessor httpContextAccessor)
{
    readonly string[] permittedFileExtensions = [".jpg", ".jpeg", ".png", ".gif"];
    public async Task<FileUploadResult> UploadFileAsync(
        IFormFile file,
        string folderName,
        CancellationToken ct = default)
    {
        if (file is null || file.Length == 0)
        {
            return new FileUploadResult
            {
                IsSuccess = false,
                ErrorMessage = "File is empty."
            };
        }

        if (file.Length > 10 * 1024 * 1024)
        {
            return new FileUploadResult
            {
                IsSuccess = false,
                ErrorMessage = "File size exceeds the limit of 10MB."
            };
        }

        string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!permittedFileExtensions.Contains(fileExtension))
        {
            return new FileUploadResult
            {
                IsSuccess = false,
                ErrorMessage = "Invalid file type."
            };
        }

        try
        {
            string uploadFolderPath = Path.Combine(
                webHostEnvironment.WebRootPath,
                folderName);

            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            string safeFileName = $"{Guid.NewGuid()}{fileExtension}";
            string fullPath = Path.Combine(uploadFolderPath, safeFileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream, ct);

            HttpContext? httpCtx = httpContextAccessor.HttpContext;
            if (httpCtx is null)
            {
                return new FileUploadResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Unable to determine file URL."
                };
            }

            Uri fileUri = new(
                $"{httpCtx.Request.Scheme}://{httpCtx.Request.Host}/{folderName}/{safeFileName}");

            return new FileUploadResult
            {
                IsSuccess = true,
                FilePath = fileUri
            };
        }
        catch (Exception ex)
        {

            return new FileUploadResult
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }
}
