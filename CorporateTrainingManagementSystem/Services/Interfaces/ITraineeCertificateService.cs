using CorporateTrainingManagementSystem.ViewModels.Trainee.Certificate;

namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface ITraineeCertificateService
    {
        
        Task<List<CertificateVM>> GetCertificatesAsync(
            string traineeId,
            CancellationToken cancellationToken = default);

        Task<Certificate?> GetCertificateAsync(
            int certificateId,
            string traineeId,
            CancellationToken cancellationToken = default);
        Task<VerifyCertificateVM?> VerifyCertificateAsync(
            string certificateNumber,
            CancellationToken cancellationToken = default);
    }
}
