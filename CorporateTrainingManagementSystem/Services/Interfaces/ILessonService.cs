

namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface ILessonService
    {
        Task<PaginatedLesson> GetAllAsync(
            int page = 1,
            int pageSize = 10,
            LessonFilter? filter = null,
            CancellationToken cancellationToken = default);

        Task<LessonVM?> GetByIdAsync(int id);

        Task<CreateLessonVM> GetCreateVMAsync();

        Task<EditLessonVM?> GetEditVMAsync(int id);

        Task<ServiceResult> CreateAsync(
            CreateLessonVM vm,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> UpdateAsync(
            EditLessonVM vm,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);
        Task LoadCoursesAsync(CreateLessonVM vm);
        Task LoadCoursesAsync(EditLessonVM vm);
    }
}
