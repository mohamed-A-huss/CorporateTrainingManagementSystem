
namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface ITraineeCourseService
    {
        Task<IEnumerable<TraineeMyCourseVM>> GetMyCoursesAsync(
            string traineeId,
            CancellationToken cancellationToken = default);
        Task<TraineeCourseDetailsVM?> GetCourseDetailsAsync(
            int courseId,
            string traineeId,
            CancellationToken cancellationToken = default);
        Task<TraineeLessonDetailsVM?> GetLessonDetailsAsync(
            int lessonId,
            string traineeId,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> MarkLessonAsReadAsync(
            int lessonId,
            string traineeId,
            CancellationToken cancellationToken = default);
        Task<TraineeExamStartVM?> GetExamStartAsync(
            int examId,
            string traineeId,
            CancellationToken cancellationToken = default);
        Task<ServiceResult<int>> BeginExamAsync(
            int examId,
            string traineeId,
            CancellationToken cancellationToken = default);
        Task<TraineeTakeExamVM?> GetTakeExamAsync(
            int attemptId,
            string traineeId,
            CancellationToken cancellationToken = default);
        Task<ServiceResult<ExamResultVM>> FinishExamAsync(
            int attemptId,
            string traineeId,
            Dictionary<int, int?> answers,
            CancellationToken cancellationToken = default);
        Task<ServiceResult<int>> StartExamAsync(
            int examId,
            string traineeId,
            CancellationToken cancellationToken = default);
        Task<List<BrowseCourseVM>> GetBrowseCoursesAsync(
            string traineeId,
            CancellationToken cancellationToken = default);

        Task<ServiceResult> EnrollAsync(
            int courseId,
            string traineeId,
            CancellationToken cancellationToken = default);
        Task<PublicCourseDetailsVM?> GetPublicCourseDetailsAsync(
            int courseId,
            string traineeId,
            CancellationToken cancellationToken = default);
        Task<List<TraineeBadgeVM>> GetMyBadgesAsync(
            string traineeId,
            CancellationToken cancellationToken = default);
            }


}
