namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface IFileService
    {
        Task<UploadResult> UploadFileAsync(
            IFormFile? file,
            string folderName,
            CancellationToken cancellationToken = default);

        void DeleteFile(string? filePath);
    }
}
