
namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class InstructorDashboardService : IInstructorDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InstructorDashboardService(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<InstructorDashboardVM> GetDashboardAsync(string instructorId,CancellationToken cancellationToken = default)
        {
            var courses = await _unitOfWork.Courses.GetAsync(
                c => c.InstructorId == instructorId,
                cancellationToken: cancellationToken);

            var courseIds = courses
                .Select(c => c.CourseId)
                .ToList();

            var lessons = await _unitOfWork.Lessons.GetAsync(
                l => courseIds.Contains(l.CourseId),
                cancellationToken: cancellationToken);

            var exams = await _unitOfWork.Exams.GetAsync(
                e => courseIds.Contains(e.CourseId),
                cancellationToken: cancellationToken);

            var enrollments = await _unitOfWork.Enrollments.GetAsync(
                e => courseIds.Contains(e.CourseId),
                cancellationToken: cancellationToken);
            var allCourses = courses
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new InstructorCourseSummaryVM
                {
                    CourseId = c.CourseId,

                    Title = c.Title,

                    StudentsCount = enrollments
                        .Count(e => e.CourseId == c.CourseId),

                    LessonsCount = lessons
                        .Count(l => l.CourseId == c.CourseId),

                    ExamsCount = exams
                        .Count(e => e.CourseId == c.CourseId)
                })
                .ToList();

            return new InstructorDashboardVM
            {
                CoursesCount = courses.Count(),

                LessonsCount = lessons.Count(),

                ExamsCount = exams.Count(),

                StudentsCount = enrollments
                    .Select(e => e.TraineeId)
                    .Distinct()
                    .Count(),

                AllCourses = allCourses
            };
        }
    }
}