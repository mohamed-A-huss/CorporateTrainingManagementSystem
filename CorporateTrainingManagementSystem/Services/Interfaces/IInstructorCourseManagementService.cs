namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface IInstructorCourseManagementService
    {
        // Lessons

        Task<ServiceResult> CreateLessonAsync(
            CreateLessonVM vm,
            string instructorId,
            CancellationToken cancellationToken = default);

        Task<InstructorEditLessonVM?> GetLessonForEditAsync(
            int lessonId,
            string instructorId,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> UpdateLessonAsync(
            InstructorEditLessonVM vm,
            string instructorId,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> DeleteLessonAsync(
            int lessonId,
            string instructorId, 
            CancellationToken cancellationToken = default);
        // Exams
        Task<ServiceResult> CreateExamAsync(
            InstructorCreateExamVM vm,
            string instructorId,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> UpdateExamAsync(
            InstructorEditExamVM vm,
            string instructorId,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> DeleteExamAsync(
            int examId,
            string instructorId,
            CancellationToken cancellationToken = default);


    }
}
