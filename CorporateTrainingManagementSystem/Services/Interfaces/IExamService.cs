using CorporateTrainingManagementSystem.ViewModels.Exam;

namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface IExamService
    {
        Task<PaginatedExam> GetAllAsync(
            int page = 1,
            int pageSize = 10,
            ExamFilter? filter = null,
            CancellationToken cancellationToken = default);

        Task<ExamVM?> GetByIdAsync(int id);

        Task<CreateExamVM> GetCreateVMAsync();

        Task LoadCoursesAsync(CreateExamVM vm);

        Task LoadCoursesAsync(EditExamVM vm);

        Task<ServiceResult> CreateAsync(
            CreateExamVM vm,
            CancellationToken cancellationToken = default);

        Task<EditExamVM?> GetEditVMAsync(int id);

        Task<ServiceResult> UpdateAsync(
            EditExamVM vm,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
