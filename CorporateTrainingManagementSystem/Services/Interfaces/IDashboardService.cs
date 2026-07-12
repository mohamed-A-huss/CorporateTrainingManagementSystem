using CorporateTrainingManagementSystem.ViewModels.Dashboard;

namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardVM> GetDashboardAsync(
            CancellationToken cancellationToken = default);
    }
}
