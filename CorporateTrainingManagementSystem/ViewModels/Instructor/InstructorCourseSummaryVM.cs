namespace CorporateTrainingManagementSystem.ViewModels.Instructor
{
    public class InstructorCourseSummaryVM
    {
        public int CourseId { get; set; }

        public string Title { get; set; } = string.Empty;

        public int StudentsCount { get; set; }

        public int LessonsCount { get; set; }

        public int ExamsCount { get; set; }
    }
}
