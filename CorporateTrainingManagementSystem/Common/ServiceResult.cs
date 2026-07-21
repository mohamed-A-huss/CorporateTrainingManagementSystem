namespace CorporateTrainingManagementSystem.Common
{
    public class ServiceResult
    {
        public bool Success { get; set; }

        public string? Message { get; set; }

        public static ServiceResult SuccessResult(string? message = null)
            => new()
            {
                Success = true,
                Message = message
            };

        public static ServiceResult Failure(string message)
            => new()
            {
                Success = false,
                Message = message
            };
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }

        public static ServiceResult<T> SuccessResult(
            T data,
            string? message = null)
            => new()
            {
                Success = true,
                Message = message,
                Data = data
            };

        public new static ServiceResult<T> Failure(string message)
            => new()
            {
                Success = false,
                Message = message
            };
    }
}