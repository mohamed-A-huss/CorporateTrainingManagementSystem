using CorporateTrainingManagementSystem.ViewModels.Trainee.Certificate;

namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class TraineeCertificateService: ITraineeCertificateService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TraineeCertificateService(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<CertificateVM>> GetCertificatesAsync(
           string traineeId,
           CancellationToken cancellationToken = default)
        {
            var certificates =
                await _unitOfWork.Certificates.GetAsync(

                    c => c.UserId == traineeId,

                    includes:
                    [
                        c => c.Course
                    ],

                    tracked: false,

                    cancellationToken: cancellationToken);

            return certificates
                .Select(c => new CertificateVM
                {
                    CertificateId = c.CertificateId,

                    CourseTitle = c.Course.Title,

                    CertificateNumber = c.CertificateNumber,

                    IssueDate = c.IssueDate
                })
                .ToList();
        }

        public async Task<Certificate?> GetCertificateAsync(
            int certificateId,
            string traineeId,
            CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Certificates.GetOneAsync(

                c =>
                    c.CertificateId == certificateId &&
                    c.UserId == traineeId,

                includes:
                [
                    c => c.Course,
                    c => c.User
                ],

                tracked: false,

                cancellationToken: cancellationToken);
        }
        public async Task<VerifyCertificateVM?> VerifyCertificateAsync(
    string certificateNumber,
    CancellationToken cancellationToken = default)
        {
            var certificate =
                await _unitOfWork.Certificates.GetOneAsync(

                    c => c.CertificateNumber == certificateNumber,

                    includes:
                    [
                        c => c.User,
                c => c.Course
                    ],

                    tracked: false,

                    cancellationToken: cancellationToken);

            if (certificate is null)
                return null;

            return new VerifyCertificateVM
            {
                IsValid = true,

                StudentName = certificate.User.FullName,

                CourseTitle = certificate.Course.Title,

                CertificateNumber = certificate.CertificateNumber,

                IssueDate = certificate.IssueDate
            };
        }

    }
}
