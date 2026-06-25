using CorporateTrainingManagementSystem.DataAccess;
using CorporateTrainingManagementSystem.Models;
using CorporateTrainingManagementSystem.Repositories.Interfaces;

namespace CorporateTrainingManagementSystem.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IRepository<Department> Departments { get; }

        public IRepository<ApplicationUser> Users { get; }

        public IRepository<Course> Courses { get; }

        public IRepository<Lesson> Lessons { get; }

        public IRepository<Enrollment> Enrollments { get; }

        public IRepository<Exam> Exams { get; }

        public IRepository<Question> Questions { get; }

        public IRepository<Choice> Choices { get; }

        public IRepository<ExamAttempt> ExamAttempts { get; }

        public IRepository<LessonProgress> LessonProgresses { get; }

        public IRepository<Badge> Badges { get; }

        public IRepository<UserBadge> UserBadges { get; }

        public IRepository<Certificate> Certificates { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Departments = new Repository<Department>(_context);

            Users = new Repository<ApplicationUser>(_context);

            Courses = new Repository<Course>(_context);

            Lessons = new Repository<Lesson>(_context);

            Enrollments = new Repository<Enrollment>(_context);

            Exams = new Repository<Exam>(_context);

            Questions = new Repository<Question>(_context);

            Choices = new Repository<Choice>(_context);

            ExamAttempts = new Repository<ExamAttempt>(_context);

            LessonProgresses = new Repository<LessonProgress>(_context);

            Badges = new Repository<Badge>(_context);

            UserBadges = new Repository<UserBadge>(_context);

            Certificates = new Repository<Certificate>(_context);
        }

        public async Task<int> CommitAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        
    }
}