using Microsoft.AspNetCore.Identity;

namespace CorporateTrainingManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public int Points { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public ICollection<Course> CoursesAsInstructor { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }

        public ICollection<ExamAttempt> ExamAttempts { get; set; }

        public ICollection<LessonProgress> LessonProgresses { get; set; }

        public ICollection<UserBadge> UserBadges { get; set; }

        public ICollection<Certificate> Certificates { get; set; }
    }
}
