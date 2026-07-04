using CorporateTrainingManagementSystem.Common;
using CorporateTrainingManagementSystem.Models;
using CorporateTrainingManagementSystem.ViewModels.Department;

namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentVM>> GetAllAsync();

        Task<DepartmentVM?> GetByIdAsync(int id);

        Task<ServiceResult> CreateAsync(CreateDepartmentVM vm);

        Task<ServiceResult> UpdateAsync(EditDepartmentVM vm);

        Task<ServiceResult> DeleteAsync(int id);
    }
}
