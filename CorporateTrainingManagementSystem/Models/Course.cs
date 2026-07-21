namespace CorporateTrainingManagementSystem.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int RewardPoints { get; set; } = 100;
        public string InstructorId { get; set; }

        public ApplicationUser Instructor { get; set; }

        public ICollection<Lesson> Lessons { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }

        public ICollection<Exam> Exams { get; set; }

        public ICollection<Certificate> Certificates { get; set; }
    }
}
