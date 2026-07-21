
using CorporateTrainingManagementSystem.ViewModels.Instructor.Student;

namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface IInstructorCourseService
    {
        Task<IEnumerable<CourseVM>> GetInstructorCoursesAsync(
       string instructorId,
       CancellationToken cancellationToken = default);

        Task<InstructorCourseDetailsVM?> GetDetailsAsync(
            int courseId,
            string instructorId,
            CancellationToken cancellationToken = default);
        Task<bool> IsInstructorCourseAsync(
            int courseId,
            string instructorId,
            CancellationToken cancellationToken = default);
        Task<List<InstructorStudentProgressVM>> GetInstructorStudentsAsync(
            string instructorId,
            CancellationToken cancellationToken = default);

        Task<CourseStudentsVM?> GetCourseStudentsAsync(
            int courseId,
            string instructorId,
            CancellationToken cancellationToken = default);

    }
}
