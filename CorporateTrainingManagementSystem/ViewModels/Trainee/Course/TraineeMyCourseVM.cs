namespace CorporateTrainingManagementSystem.ViewModels.Trainee.Course
{
    public class TraineeMyCourseVM
    {
        public int EnrollmentId { get; set; }

        public int CourseId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public EnrollmentStatus Status { get; set; }

        public int LessonsCount { get; set; }

        public int CompletedLessons { get; set; }

        public double ProgressPercentage { get; set; }
    }
}
