namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface IInstructorExamService
    {
        
        // Exam
        

        Task<InstructorExamDetailsVM?> GetDetailsAsync(
            int examId,
            string instructorId,
            CancellationToken cancellationToken = default);



        // Questions

        Task<ServiceResult> CreateQuestionAsync(
            InstructorCreateQuestionVM vm,
            string instructorId,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> UpdateQuestionAsync(
            InstructorEditQuestionVM vm,
            string instructorId,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> DeleteQuestionAsync(
            int questionId,
            string instructorId,
            CancellationToken cancellationToken = default);


        Task<InstructorQuestionDetailsVM?> GetQuestionDetailsAsync(int questionId,string instructorId,CancellationToken cancellationToken = default);
        // Choices

        Task<ServiceResult> CreateChoiceAsync(
            InstructorCreateChoiceVM vm,
            string instructorId,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> UpdateChoiceAsync(
            InstructorEditChoiceVM vm,
            string instructorId,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> DeleteChoiceAsync(
            int choiceId,
            string instructorId,
            CancellationToken cancellationToken = default);
    }
}
