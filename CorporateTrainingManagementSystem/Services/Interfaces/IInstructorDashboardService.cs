using CorporateTrainingManagementSystem.ViewModels.Instructor;

namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface IInstructorDashboardService
    {
        Task<InstructorDashboardVM> GetDashboardAsync(string instructorId,CancellationToken cancellationToken = default);
    }
}
