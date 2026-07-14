namespace CorporateTrainingManagementSystem.ViewModels.Identity
{
    public class ForgotPasswordResult
    {
        public ServiceResult Result { get; set; } = ServiceResult.SuccessResult();

        public string? Token { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
