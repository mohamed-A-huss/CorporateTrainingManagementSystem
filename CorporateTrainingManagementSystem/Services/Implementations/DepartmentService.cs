
namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedDepartment> GetAllAsync(int page = 1, int pageSize = 10, string? query = null, CancellationToken cancellationToken = default)
        {
            var departments = await _unitOfWork.Departments.GetAsync(cancellationToken: cancellationToken);
            if (departments.Count() == 0)
            {
                return new PaginatedDepartment
                {
                    Departments = Enumerable.Empty<DepartmentVM>(),
                    CurrentPage = page,
                    TotalPages = 0,
                    TotalCount = 0
                };
            }
            if (!string.IsNullOrWhiteSpace(query))
            {
                departments = departments.Where(
                    e => e.Name.Contains(query, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount =  departments.Count();

            double totalPages = Math.Ceiling(totalCount / (double)pageSize);

            departments = departments.Skip((page - 1) * pageSize).Take(pageSize);
            var result =  departments.ToList();
            return new PaginatedDepartment
            {
                Departments = result.Adapt<IEnumerable<DepartmentVM>>(),
                CurrentPage = page,
                TotalPages = (int)totalPages,
                TotalCount = totalCount,
                Query = query
            };
        }

        public async Task<IEnumerable<DepartmentVM>> GetDropdownAsync(CancellationToken cancellationToken = default)
        {
            var departments = await _unitOfWork.Departments.GetAsync(
                cancellationToken: cancellationToken);

            return departments
                .OrderBy(d => d.Name)
                .Adapt<IEnumerable<DepartmentVM>>();
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

            var department = vm.Adapt<Department>();

            department.Name = department.Name.Trim();

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

            vm.Adapt(department);

            department.Name = department.Name.Trim();

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