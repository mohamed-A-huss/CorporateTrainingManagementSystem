using CorporateTrainingManagementSystem.ViewModels.Trainee.Dashboard;

namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface ITraineeDashboardService
    {
        Task<TraineeDashboardVM> GetDashboardAsync(string traineeId,CancellationToken cancellationToken = default);
    }
}
