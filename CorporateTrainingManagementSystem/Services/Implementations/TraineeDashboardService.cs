using CorporateTrainingManagementSystem.ViewModels.Trainee.Dashboard;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class TraineeDashboardService : ITraineeDashboardService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IUnitOfWork _unitOfWork;
        public TraineeDashboardService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<TraineeDashboardVM?> GetDashboardAsync(
            string traineeId,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users
                    .Include(u => u.Enrollments)
                    .Include(u => u.UserBadges)
                    .Include(u => u.Certificates)
                    .FirstOrDefaultAsync(u => u.Id == traineeId, cancellationToken);

            if (user is null)
                return null;

            var completedCourses = user.Enrollments.Count(e =>
                e.Status == EnrollmentStatus.Completed);

            var activeCourses = user.Enrollments.Count(e =>
                e.Status == EnrollmentStatus.Active);

            return new TraineeDashboardVM
            {
                FullName = user.FullName,
                Points = user.Points,

                ActiveCourses = activeCourses,
                CompletedCourses = completedCourses,

                BadgesCount = user.UserBadges.Count,
                CertificatesCount = user.Certificates.Count
            };
        }
    }
}
