

namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<UploadResult> UploadFileAsync(IFormFile? file,string folderName,
            CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length == 0)
                return UploadResult.Failure("No file selected.");

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (folderName == UploadFolders.Pdfs)
            {
                if (!FileValidation.PdfExtensions.Contains(extension))
                    return UploadResult.Failure("Only PDF files are allowed.");

                if (file.Length > FileValidation.MaxPdfSize)
                    return UploadResult.Failure("PDF size cannot exceed 10 MB.");
            }

            var uploadFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                folderName);

            Directory.CreateDirectory(uploadFolder);

            var fileName = $"{Guid.NewGuid()}{extension}";

            var fullPath = Path.Combine(uploadFolder, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);

            await file.CopyToAsync(stream, cancellationToken);

            return UploadResult.SuccessResult($"/uploads/{folderName}/{fileName}");
        }

        public void DeleteFile(string? filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            var fullPath = Path.Combine(
                _environment.WebRootPath,
                filePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}