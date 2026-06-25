using CorporateTrainingManagementSystem.Models;

namespace CorporateTrainingManagementSystem.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        public interface IUnitOfWork 
        {
            IRepository<Department> Departments { get; }

            IRepository<ApplicationUser> Users { get; }

            IRepository<Course> Courses { get; }

            IRepository<Lesson> Lessons { get; }

            IRepository<Enrollment> Enrollments { get; }

            IRepository<Exam> Exams { get; }

            IRepository<Question> Questions { get; }

            IRepository<Choice> Choices { get; }

            IRepository<ExamAttempt> ExamAttempts { get; }

            IRepository<LessonProgress> LessonProgresses { get; }

            IRepository<Badge> Badges { get; }

            IRepository<UserBadge> UserBadges { get; }

            IRepository<Certificate> Certificates { get; }

            Task<int> CommitAsync(CancellationToken cancellationToken = default);
        }
    }
}
