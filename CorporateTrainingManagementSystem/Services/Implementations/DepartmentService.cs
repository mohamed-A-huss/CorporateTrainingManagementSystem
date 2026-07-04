using CorporateTrainingManagementSystem.Common;
using CorporateTrainingManagementSystem.Models;
using CorporateTrainingManagementSystem.Repositories.Interfaces;
using CorporateTrainingManagementSystem.Services.Interfaces;
using CorporateTrainingManagementSystem.ViewModels.Department;
using Mapster;

namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DepartmentVM>> GetAllAsync()
        {
            var departments = await _unitOfWork.Departments.GetAsync();
            return departments.Adapt<IEnumerable<DepartmentVM>>();
        }

        public async Task<DepartmentVM?> GetByIdAsync(int id)
        {
            var department = await _unitOfWork.Departments.GetOneAsync(d => d.DepartmentId == id);
            return department == null ? null : department.Adapt<DepartmentVM>();
        }

        public async Task<ServiceResult> CreateAsync(CreateDepartmentVM vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Name))
                return ServiceResult.Failure("Department name is required.");

            var department = new Department
            {
                Name = vm.Name.Trim()
            };

            await _unitOfWork.Departments.CreateAsync(department);

            var rows = await _unitOfWork.CommitAsync();

            if (rows == 0)
                return ServiceResult.Failure("Failed to create department.");

            return ServiceResult.SuccessResult("Department created successfully.");
        }

        public async Task<ServiceResult> UpdateAsync(EditDepartmentVM vm)
        {
            var department = await _unitOfWork.Departments.GetOneAsync(d => d.DepartmentId == vm.DepartmentId);

            if (department == null)
                return ServiceResult.Failure("Department not found.");

            if (string.IsNullOrWhiteSpace(vm.Name))
                return ServiceResult.Failure("Department name is required.");

            department.Name = vm.Name.Trim();

            _unitOfWork.Departments.Update(department);

            var rows = await _unitOfWork.CommitAsync();

            if (rows == 0)
                return ServiceResult.Failure("Failed to update department.");

            return ServiceResult.SuccessResult("Department updated successfully.");
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var department = await _unitOfWork.Departments.GetOneAsync(d => d.DepartmentId == id);

            if (department == null)
                return ServiceResult.Failure("Department not found.");

            _unitOfWork.Departments.Delete(department);

            var rows = await _unitOfWork.CommitAsync();

            if (rows == 0)
                return ServiceResult.Failure("Failed to delete department.");

            return ServiceResult.SuccessResult("Department deleted successfully.");
        }
    }

}