using CorporateTrainingManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CorporateTrainingManagementSystem.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<Department> Departments { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Choice> Choices { get; set; }

        public DbSet<ExamAttempt> ExamAttempts { get; set; }

        public DbSet<LessonProgress> LessonProgresses { get; set; }

        public DbSet<Badge> Badges { get; set; }

        public DbSet<UserBadge> UserBadges { get; set; }

        public DbSet<Certificate> Certificates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
