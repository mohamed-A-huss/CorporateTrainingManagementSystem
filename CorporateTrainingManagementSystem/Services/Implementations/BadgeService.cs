using CorporateTrainingManagementSystem.ViewModels.Badge;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class BadgeService : IBadgeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BadgeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> CreateAsync(CreateBadgeVM vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Name))
                return ServiceResult.Failure("Badge name is required.");

            var badge = vm.Adapt<Badge>();

            badge.Name = badge.Name.Trim();

            await _unitOfWork.Badges.CreateAsync(badge);

            var rows = await _unitOfWork.CommitAsync();

            if (rows == 0)
                return ServiceResult.Failure("Failed to create badge.");

            return ServiceResult.SuccessResult("Badge created successfully.");
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var badge = await _unitOfWork.Badges.GetOneAsync(b => b.BadgeId == id);

            if (badge == null)
                return ServiceResult.Failure("Badge not found.");

            _unitOfWork.Badges.Delete(badge);

            var rows = await _unitOfWork.CommitAsync();

            if (rows == 0)
                return ServiceResult.Failure("Failed to delete badge.");

            return ServiceResult.SuccessResult("Badge deleted successfully.");
        }

        public async Task<PaginatedBadge> GetAllAsync(BadgeFilter filter, int page=1, int pageSize=10, CancellationToken cancellationToken=default)
        {
            var badges = await _unitOfWork.Badges.GetAsync(cancellationToken: cancellationToken);
            if (badges.Count() == 0)
            {
                return new PaginatedBadge
                {
                    Badges = Enumerable.Empty<BadgeVM>(),
                    CurrentPage = page,
                    TotalPages = 0,
                    TotalCount = 0
                };
            }
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                badges = badges.Where(
                    e => e.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.MinRequiredPoints.HasValue)
            {
                badges = badges.Where(e => e.RequiredPoints >= filter.MinRequiredPoints.Value);
            }

            if (filter.MaxRequiredPoints.HasValue)
            {
                badges = badges.Where(e => e.RequiredPoints <= filter.MaxRequiredPoints.Value);
            }
            var totalCount = badges.Count();

            double totalPages = Math.Ceiling(totalCount / (double)pageSize);

            badges = badges.Skip((page - 1) * pageSize).Take(pageSize);
            var result = badges.ToList();
            return new PaginatedBadge
            {
                Badges = result.Adapt<IEnumerable<BadgeVM>>(),
                CurrentPage = page,
                TotalPages = (int)totalPages,
                TotalCount = totalCount,
                Query = filter.Name
            };
        }

        public async Task<BadgeVM?> GetByIdAsync(int id)
        {
            var badge = await _unitOfWork.Badges.GetOneAsync(b => b.BadgeId == id);
            return badge == null ? null : badge.Adapt<BadgeVM>();
        }

        public async Task<ServiceResult> UpdateAsync(EditBadgeVM vm)
        {
            var badge = await _unitOfWork.Badges.GetOneAsync(b => b.BadgeId == vm.BadgeId);

            if (badge == null)
                return ServiceResult.Failure("Badge not found.");

            if (string.IsNullOrWhiteSpace(vm.Name))
                return ServiceResult.Failure("Badge name is required.");

            vm.Adapt(badge);

            badge.Name = badge.Name.Trim();

            badge.Description = string.IsNullOrWhiteSpace(badge.Description) ? null : badge.Description.Trim();

            _unitOfWork.Badges.Update(badge);

            var rows = await _unitOfWork.CommitAsync();

            if (rows == 0)
                return ServiceResult.Failure("Failed to update badge.");

            return ServiceResult.SuccessResult("Badge updated successfully.");
        }
    }
}
