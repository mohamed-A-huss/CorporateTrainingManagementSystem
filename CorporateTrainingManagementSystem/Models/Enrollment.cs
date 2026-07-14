using CorporateTrainingManagementSystem.ViewModels.Enrollment;

namespace CorporateTrainingManagementSystem.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        public string TraineeId { get; set; }

        public ApplicationUser User { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;

    }
}
