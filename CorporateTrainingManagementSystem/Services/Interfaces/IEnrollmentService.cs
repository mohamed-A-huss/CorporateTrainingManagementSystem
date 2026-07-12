using CorporateTrainingManagementSystem.ViewModels.Enrollment;

namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<PaginatedEnrollment> GetAllAsync(
            int page = 1,
            int pageSize = 10,
            EnrollmentFilter? filter = null,
            CancellationToken cancellationToken = default);

        Task<EnrollmentVM?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);
        Task<ServiceResult> ChangeStatusAsync(
            int enrollmentId,
            EnrollmentStatus status,
            CancellationToken cancellationToken = default);

        Task LoadDropdownsAsync(
            CreateEnrollmentVM vm,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> CreateAsync(
            CreateEnrollmentVM vm,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
