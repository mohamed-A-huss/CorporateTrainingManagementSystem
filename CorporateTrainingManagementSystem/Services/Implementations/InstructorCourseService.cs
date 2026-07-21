using CorporateTrainingManagementSystem.ViewModels.Instructor.Student;

namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class InstructorCourseService: IInstructorCourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InstructorCourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> IsInstructorCourseAsync(int courseId,string instructorId,CancellationToken cancellationToken = default)
        {
            return (await _unitOfWork.Courses.GetAsync(
                c => c.CourseId == courseId &&
                     c.InstructorId == instructorId,
                cancellationToken: cancellationToken))
                .Any();
        }
        public async Task<IEnumerable<CourseVM>> GetInstructorCoursesAsync(string instructorId,CancellationToken cancellationToken = default)
        {
            var courses = await _unitOfWork.Courses.GetAsync(
                c => c.InstructorId == instructorId,
                includes: [c => c.Instructor],
                cancellationToken: cancellationToken);

            return courses
                .Select(c => new CourseVM
                {
                    CourseId = c.CourseId,

                    Title = c.Title,

                    Description = c.Description,

                    CreatedAt = c.CreatedAt,

                    InstructorName = c.Instructor.FullName
                })
                .ToList();
        }
        public async Task<InstructorCourseDetailsVM?> GetDetailsAsync(int courseId,string instructorId,CancellationToken cancellationToken = default)
        {
            var course = (await _unitOfWork.Courses.GetAsync(
                c => c.CourseId == courseId &&
                     c.InstructorId == instructorId,
                cancellationToken: cancellationToken))
                .FirstOrDefault();

            if (course is null)
                return null;

            var lessons = await _unitOfWork.Lessons.GetAsync(
                l => l.CourseId == courseId,
                cancellationToken: cancellationToken);

            var exams = await _unitOfWork.Exams.GetAsync(
                e => e.CourseId == courseId,
                includes: [e => e.Questions],
                cancellationToken: cancellationToken);

            var enrollments = await _unitOfWork.Enrollments.GetAsync(
                e => e.CourseId == courseId,
                includes: [e => e.User],
                cancellationToken: cancellationToken);

            return new InstructorCourseDetailsVM
            {
                CourseId = course.CourseId,

                Title = course.Title,

                Description = course.Description,

                CreatedAt = course.CreatedAt,

                StudentsCount = enrollments.Count(),

                LessonsCount = lessons.Count(),

                ExamsCount = exams.Count(),

                Lessons = lessons
                    .OrderBy(l => l.Order)
                    .Select(l => new LessonVM
                    {
                        LessonId = l.LessonId,

                        Title = l.Title,

                        Content = l.Content,

                        VideoUrl = l.VideoUrl,

                        PdfPath = l.PdfPath,
                        Order = l.Order,

                        CourseTitle = course.Title
                    })
                    .ToList(),

                Exams = exams
                    .OrderBy(e => e.ExamId)
                    .Select(e => new InstructorExamVM
                    {
                        ExamId = e.ExamId,

                        Title = e.Title,

                        DurationMinutes = e.DurationMinutes,

                        PassMark = e.PassMark,

                        TotalMarks = e.TotalMarks,
                        MaxAttempts=e.MaxAttempts,

                    })
                    .ToList(),

                Students = await GetCourseStudentsAsync(
                        course.CourseId,
                        instructorId,
                        cancellationToken)
                        is { } studentsVm
                            ? studentsVm.Students
                            : Enumerable.Empty<InstructorStudentProgressVM>(),
            };
        }
        public async Task<List<InstructorStudentProgressVM>> GetInstructorStudentsAsync(string instructorId,CancellationToken cancellationToken = default)
        {
            // 1- Courses
            var courses = (await _unitOfWork.Courses.GetAsync(
                c => c.InstructorId == instructorId,
                tracked: false,
                cancellationToken: cancellationToken))
                .ToList();

            if (!courses.Any())
                return [];

            var courseIds = courses
                .Select(c => c.CourseId)
                .ToList();

            // 2- Lessons
            var lessons = (await _unitOfWork.Lessons.GetAsync(
                l => courseIds.Contains(l.CourseId),
                tracked: false,
                cancellationToken: cancellationToken))
                .ToList();

            // Number of lessons per course
            var lessonsPerCourse = lessons
                .GroupBy(l => l.CourseId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Count());

            var lessonIds = lessons
                .Select(l => l.LessonId)
                .ToList();

            // LessonId -> CourseId
            var lessonCourseLookup = lessons
                .ToDictionary(
                    l => l.LessonId,
                    l => l.CourseId);

            // 3- Enrollments
            var enrollments = (await _unitOfWork.Enrollments.GetAsync(
                e => courseIds.Contains(e.CourseId),
                includes:
                [
                    e => e.User
                ],
                tracked: false,
                cancellationToken: cancellationToken))
                .ToList();

            // 4- Lesson Progress
            var progresses = (await _unitOfWork.LessonProgresses.GetAsync(
                lp => lp.IsCompleted &&
                      lessonIds.Contains(lp.LessonId),
                tracked: false,
                cancellationToken: cancellationToken))
                .ToList();

            var result = new List<InstructorStudentProgressVM>();

            foreach (var enrollment in enrollments)
            {
                int totalLessons = lessonsPerCourse.TryGetValue(
                    enrollment.CourseId,
                    out var count)
                    ? count
                    : 0;

                int completedLessons = progresses.Count(lp =>
                    lp.UserId == enrollment.TraineeId &&
                    lessonCourseLookup[lp.LessonId] == enrollment.CourseId);

                int progress = totalLessons == 0
                    ? 0
                    : (int)Math.Round(
                        (double)completedLessons / totalLessons * 100);

                result.Add(new InstructorStudentProgressVM
                {
                    StudentId = enrollment.TraineeId,

                    StudentName = enrollment.User.FullName,

                    CourseId = enrollment.CourseId,

                    CourseTitle = courses
                        .First(c => c.CourseId == enrollment.CourseId)
                        .Title,

                    CompletedLessons = completedLessons,

                    TotalLessons = totalLessons,

                    ProgressPercentage = progress,

                    Status = progress == 100
                        ? "Completed"
                        : "In Progress"
                });
            }

            return result
                .OrderBy(s => s.CourseTitle)
                .ThenBy(s => s.StudentName)
                .ToList();
        }
        public async Task<CourseStudentsVM?> GetCourseStudentsAsync(int courseId,string instructorId,CancellationToken cancellationToken = default)
        {
            var course = await _unitOfWork.Courses.GetOneAsync(
                c => c.CourseId == courseId &&
                     c.InstructorId == instructorId,
                tracked: false,
                cancellationToken: cancellationToken);

            if (course is null)
                return null;

            var allStudents = await GetInstructorStudentsAsync(
                instructorId,
                cancellationToken);

            return new CourseStudentsVM
            {
                CourseId = course.CourseId,

                CourseTitle = course.Title,

                Students = allStudents
                    .Where(s => s.CourseId == courseId)
                    .OrderBy(s => s.StudentName)
                    .ToList()
            };
        }
    }
}
