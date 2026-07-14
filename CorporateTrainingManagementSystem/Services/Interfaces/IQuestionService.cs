using CorporateTrainingManagementSystem.ViewModels.Question;

namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<PaginatedQuestion> GetAllAsync(
            int page = 1,
            int pageSize = 10,
            QuestionFilter? filter = null,
            CancellationToken cancellationToken = default);

        Task<QuestionVM?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<CreateQuestionVM> GetCreateVMAsync();

        Task<ServiceResult> CreateAsync(
            CreateQuestionVM vm,
            CancellationToken cancellationToken = default);

        Task<EditQuestionVM?> GetEditVMAsync(int id, CancellationToken cancellationToken = default);

        Task<ServiceResult> UpdateAsync(
            EditQuestionVM vm,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task LoadDropdownsAsync(CreateQuestionVM vm);

        Task LoadDropdownsAsync(EditQuestionVM vm);

        Task<IEnumerable<SelectListItem>> GetExamsByCourseAsync(int courseId);
    }
}
