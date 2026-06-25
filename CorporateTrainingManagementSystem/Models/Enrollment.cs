namespace CorporateTrainingManagementSystem.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
    }
}
