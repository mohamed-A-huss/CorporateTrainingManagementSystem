namespace CorporateTrainingManagementSystem.Common
{
    public class ServiceResult
    {
        public bool Success { get; set; }

        public string? Message { get; set; }

        public static ServiceResult SuccessResult(string? message = null)
            => new() { Success = true, Message = message };

        public static ServiceResult Failure(string message)
            => new() { Success = false, Message = message };
    }
}
