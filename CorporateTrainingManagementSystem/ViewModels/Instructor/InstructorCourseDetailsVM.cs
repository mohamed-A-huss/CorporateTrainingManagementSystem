namespace CorporateTrainingManagementSystem.ViewModels.Instructor
{
    public class InstructorCourseDetailsVM
    {
        public int CourseId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public int StudentsCount { get; set; }

        public int LessonsCount { get; set; }

        public int ExamsCount { get; set; }

        public IEnumerable<LessonVM> Lessons { get; set; }
            = Enumerable.Empty<LessonVM>();

        public IEnumerable<InstructorExamVM> Exams { get; set; }
            = Enumerable.Empty<InstructorExamVM>();

        public IEnumerable<InstructorStudentProgressVM> Students { get; set; }
            = Enumerable.Empty<InstructorStudentProgressVM>();
    }
}
