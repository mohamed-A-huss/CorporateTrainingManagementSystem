namespace CorporateTrainingManagementSystem.ViewModels.Enrollment
{
    public enum EnrollmentStatus
    {
        Active = 1,
        Completed = 2,
        Cancelled = 3
    }
    public class EnrollmentVM
    {
        public int EnrollmentId { get; set; }

        public string TraineeId { get; set; } = string.Empty;

        public string TraineeName { get; set; } = string.Empty;

        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public DateTime EnrollmentDate { get; set; }

        public EnrollmentStatus Status { get; set; }
    }
}
