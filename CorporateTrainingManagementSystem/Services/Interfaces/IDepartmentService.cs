
namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<PaginatedDepartment> GetAllAsync(int page , int pageSize, string? query , CancellationToken cancellationToken );

        Task<IEnumerable<DepartmentVM>> GetDropdownAsync(CancellationToken cancellationToken = default);
        Task<DepartmentVM?> GetByIdAsync(int id);

        Task<ServiceResult> CreateAsync(CreateDepartmentVM vm);

        Task<ServiceResult> UpdateAsync(EditDepartmentVM vm);

        Task<ServiceResult> DeleteAsync(int id);
    }
}
