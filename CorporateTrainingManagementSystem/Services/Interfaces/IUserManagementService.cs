using CorporateTrainingManagementSystem.ViewModels.UserManagement;

namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface IUserManagementService
    {
        Task<PaginatedUsers> GetAllAsync(
            int page = 1,
            int pageSize = 10,
            UserFilter? filter = null,
            CancellationToken cancellationToken = default);

        Task<UserDetailsVM?> GetByIdAsync(
            string id,
            CancellationToken cancellationToken = default);
        Task<ServiceResult> CreateUserAsync(CreateUserVM vm);

        Task<ChangeRoleVM?> GetChangeRoleVMAsync(
            string id,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> ChangeRoleAsync(
            ChangeRoleVM vm,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> LockAsync(
            string id,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> UnlockAsync(
            string id,
            CancellationToken cancellationToken = default);

        Task LoadDropdownsAsync(
            UserFilter filter,
            CancellationToken cancellationToken = default);

        Task LoadRolesAsync(
            ChangeRoleVM vm);
    }
}
