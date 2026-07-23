using CorporateTrainingManagementSystem.ViewModels.Certificate;

namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface ICertificateService
    {
        Task<CertificateListVM> GetAllCertificatesAsync(
                CertificateFilterVM filter,
                CancellationToken cancellationToken = default);
        Task<Certificate?> GetCertificateByIdAsync(
                int id,
                CancellationToken cancellationToken = default);
    }
}
