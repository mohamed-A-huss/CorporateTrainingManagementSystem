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
                    .OrderBy(l => l.LessonId)
                    .Select(l => new LessonVM
                    {
                        LessonId = l.LessonId,

                        Title = l.Title,

                        Content = l.Content,

                        VideoUrl = l.VideoUrl,

                        PdfPath = l.PdfPath,

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

                    })
                    .ToList(),

                Students = enrollments
                    .Select(e => new InstructorStudentVM
                    {
                        Id = e.User.Id,

                        FullName = e.User.FullName,

                        Email = e.User.Email!
                    })
                    .ToList()
            };
        }
    }
}
