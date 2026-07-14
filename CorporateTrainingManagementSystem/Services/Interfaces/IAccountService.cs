using CorporateTrainingManagementSystem.ViewModels;
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

        Task<ForgotPasswordResult> ForgotPasswordAsync(
        ForgotPasswordVM vm,
        CancellationToken cancellationToken = default);
        Task<ServiceResult> ChangePasswordAsync(
        string userId,
        ChangePasswordVM vm,
        CancellationToken cancellationToken = default);
        Task<ServiceResult> ResetPasswordAsync(
            ResetPasswordVM vm,
            CancellationToken cancellationToken = default);

        // Profile
        Task<EditProfileVM?> GetEditProfileAsync(string userId,CancellationToken cancellationToken = default);
        Task<ProfileVM?> GetProfileAsync(string userId, CancellationToken cancellationToken = default);
        Task<ServiceResult> UpdateProfileAsync(string userId, EditProfileVM vm, CancellationToken cancellationToken = default);





    }
}
