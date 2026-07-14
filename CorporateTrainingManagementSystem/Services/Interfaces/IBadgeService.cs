
namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface IBadgeService
    {
        Task<PaginatedBadge> GetAllAsync(BadgeFilter filter, int page, int pageSize, CancellationToken cancellationToken);

        Task<BadgeVM?> GetByIdAsync(int id);

        Task<ServiceResult> CreateAsync(CreateBadgeVM vm);

        Task<ServiceResult> UpdateAsync(EditBadgeVM vm);

        Task<ServiceResult> DeleteAsync(int id);
    }
}
