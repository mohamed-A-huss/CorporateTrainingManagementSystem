using CorporateTrainingManagementSystem.ViewModels.Course;

namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface ICourseService
    {
        Task<PaginatedCourse> GetAllAsync(
            int page = 1,
            int pageSize = 10,
            CourseFilter? filter = null,
            CancellationToken cancellationToken = default);

        Task<CourseVM?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<CreateCourseVM> GetCreateVMAsync();

        Task<EditCourseVM?> GetEditVMAsync(int id);

        Task<ServiceResult> CreateAsync(
            CreateCourseVM vm,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> UpdateAsync(
            EditCourseVM vm,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
