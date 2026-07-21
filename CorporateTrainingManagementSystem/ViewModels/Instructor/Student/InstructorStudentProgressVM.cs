namespace CorporateTrainingManagementSystem.ViewModels.Instructor.Student
{
    public class InstructorStudentProgressVM
    {
        public string StudentId { get; set; } = string.Empty;

        public string StudentName { get; set; } = string.Empty;

        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public int CompletedLessons { get; set; }

        public int TotalLessons { get; set; }

        public int ProgressPercentage { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
