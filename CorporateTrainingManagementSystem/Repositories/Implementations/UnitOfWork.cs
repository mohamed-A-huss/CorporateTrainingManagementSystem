using CorporateTrainingManagementSystem.DataAccess;
using CorporateTrainingManagementSystem.Models;
using CorporateTrainingManagementSystem.Repositories.Interfaces;

namespace CorporateTrainingManagementSystem.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private IRepository<Department>? _departments;
        private IRepository<Course>? _courses;
        private IRepository<Lesson>? _lessons;
        private IRepository<Enrollment>? _enrollments;
        private IRepository<Exam>? _exams;
        private IRepository<Question>? _questions;
        private IRepository<Choice>? _choices;
        private IRepository<ExamAttempt>? _examAttempts;
        private IRepository<LessonProgress>? _lessonProgresses;
        private IRepository<Badge>? _badges;
        private IRepository<UserBadge>? _userBadges;
        private IRepository<Certificate>? _certificates;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<Department> Departments
            => _departments ??= new Repository<Department>(_context);

        public IRepository<Course> Courses
            => _courses ??= new Repository<Course>(_context);

        public IRepository<Lesson> Lessons
            => _lessons ??= new Repository<Lesson>(_context);

        public IRepository<Enrollment> Enrollments
            => _enrollments ??= new Repository<Enrollment>(_context);

        public IRepository<Exam> Exams
            => _exams ??= new Repository<Exam>(_context);

        public IRepository<Question> Questions
            => _questions ??= new Repository<Question>(_context);

        public IRepository<Choice> Choices
            => _choices ??= new Repository<Choice>(_context);

        public IRepository<ExamAttempt> ExamAttempts
            => _examAttempts ??= new Repository<ExamAttempt>(_context);

        public IRepository<LessonProgress> LessonProgresses
            => _lessonProgresses ??= new Repository<LessonProgress>(_context);

        public IRepository<Badge> Badges
            => _badges ??= new Repository<Badge>(_context);

        public IRepository<UserBadge> UserBadges
            => _userBadges ??= new Repository<UserBadge>(_context);

        public IRepository<Certificate> Certificates
            => _certificates ??= new Repository<Certificate>(_context);

        public Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}