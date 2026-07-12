using CorporateTrainingManagementSystem.ViewModels.Identity;

namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface IAccountService
    {
        // Authentication

        Task<ServiceResult> LoginAsync(
            LoginVM vm,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> RegisterAsync(
            RegisterVM vm,
            CancellationToken cancellationToken = default);

        Task LogoutAsync();

        // Password Management

        Task<ServiceResult> ForgotPasswordAsync(
            ForgotPasswordVM vm,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> ResetPasswordAsync(
            ResetPasswordVM vm,
            CancellationToken cancellationToken = default);

        // Profile

        Task<ProfileVM?> GetProfileAsync(
            CancellationToken cancellationToken = default);

        Task<ServiceResult> UpdateProfileAsync(
            EditProfileVM vm,
            CancellationToken cancellationToken = default);

        // External Login

        Task<ServiceResult> ExternalLoginAsync(
            string provider,
            string returnUrl);

        Task<ServiceResult> ExternalLoginCallbackAsync(
            string? returnUrl,
            string? remoteError);
    }
}
