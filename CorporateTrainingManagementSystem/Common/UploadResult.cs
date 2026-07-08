namespace CorporateTrainingManagementSystem.Common
{
    public class UploadResult
    {
        public bool Success { get; set; }

        public string? FilePath { get; set; }

        public string? ErrorMessage { get; set; }

        public static UploadResult SuccessResult(string filePath)
            => new()
            {
                Success = true,
                FilePath = filePath
            };

        public static UploadResult Failure(string message)
            => new()
            {
                Success = false,
                ErrorMessage = message
            };
    }
}
