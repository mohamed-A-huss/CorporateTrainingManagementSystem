using CorporateTrainingManagementSystem.DataAccess;
using CorporateTrainingManagementSystem.ViewModels.Dashboard;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<DashboardVM> GetDashboardAsync(
    CancellationToken cancellationToken = default)
        {
            var users = await _userManager.Users
                .Include(u => u.Department)
                .OrderByDescending(u => u.Id)
                .Take(5)
                .ToListAsync(cancellationToken);

            var latestUsers = new List<UserVM>();

            foreach (var user in users)
            {
                var role = (await _userManager.GetRolesAsync(user))
                    .FirstOrDefault() ?? "No Role";

                latestUsers.Add(new UserVM
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email!,
                    Department = user.Department?.Name ?? "-",
                    Role = role,
                    Points = user.Points,
                    IsLocked = user.LockoutEnd.HasValue &&
                               user.LockoutEnd > DateTimeOffset.UtcNow
                });
            }

            var latestCourses = await _context.Courses
                .Include(c => c.Instructor)
                .OrderByDescending(c => c.CourseId)
                .Take(5)
                .Select(c => new CourseVM
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    CreatedAt = c.CreatedAt,
                    InstructorName = c.Instructor.FullName
                })
                .ToListAsync(cancellationToken);

            var adminRole = await _userManager.GetUsersInRoleAsync(SD.ADMIN_ROLE);
            var instructorRole = await _userManager.GetUsersInRoleAsync(SD.INSTRUCTOR_ROLE);
            var traineeRole = await _userManager.GetUsersInRoleAsync(SD.TRAINEE_ROLE);

            return new DashboardVM
            {
                TotalUsers = await _context.Users.CountAsync(cancellationToken),

                TotalAdmins = adminRole.Count,

                TotalInstructors = instructorRole.Count,

                TotalTrainees = traineeRole.Count,

                TotalDepartments = await _context.Departments.CountAsync(cancellationToken),

                TotalCourses = await _context.Courses.CountAsync(cancellationToken),

                TotalLessons = await _context.Lessons.CountAsync(cancellationToken),

                TotalExams = await _context.Exams.CountAsync(cancellationToken),

                TotalQuestions = await _context.Questions.CountAsync(cancellationToken),

                TotalBadges = await _context.Badges.CountAsync(cancellationToken),

                TotalCertificates = await _context.Certificates.CountAsync(cancellationToken),

                LatestUsers = latestUsers,

                LatestCourses = latestCourses
            };
        }
    }
}
