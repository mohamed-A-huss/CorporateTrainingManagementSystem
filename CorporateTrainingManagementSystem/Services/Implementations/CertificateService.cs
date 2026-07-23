using CorporateTrainingManagementSystem.Repositories.Implementations;
using CorporateTrainingManagementSystem.ViewModels.Certificate;

namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class CertificateService : ICertificateService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CertificateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CertificateListVM> GetAllCertificatesAsync(
    CertificateFilterVM filter,
    CancellationToken cancellationToken = default)
        {
            var certificates = await _unitOfWork.Certificates.GetAsync(
                includes:
                [
                    x => x.User,
            x => x.Course
                ],
                tracked: false,
                cancellationToken: cancellationToken);

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                certificates = certificates.Where(x =>

                    x.User.FullName.Contains(filter.Search,
                        StringComparison.OrdinalIgnoreCase)

                    ||

                    x.Course.Title.Contains(filter.Search,
                        StringComparison.OrdinalIgnoreCase)

                    ||

                    x.CertificateNumber.Contains(filter.Search,
                        StringComparison.OrdinalIgnoreCase));
            }

            var count = certificates.Count();

            var pages = (int)Math.Ceiling(
                count / (double)filter.PageSize);

            certificates = certificates

                .OrderByDescending(x => x.IssueDate)

                .Skip((filter.Page - 1) * filter.PageSize)

                .Take(filter.PageSize);

            return new CertificateListVM
            {
                Certificates = certificates.Select(x => new AdminCertificateVM
                {
                    CertificateId = x.CertificateId,
                    CertificateNumber = x.CertificateNumber,
                    StudentName = x.User.FullName,
                    StudentEmail = x.User.Email!,
                    CourseTitle = x.Course.Title,
                    IssueDate = x.IssueDate
                }),

                Filter = filter,

                TotalPages = pages,

                TotalCount = count
            };
        }
        public async Task<Certificate?> GetCertificateByIdAsync(
    int id,
    CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Certificates.GetOneAsync(
                x => x.CertificateId == id,
                includes:
                [
                    x => x.User,
            x => x.Course
                ],
                tracked: false,
                cancellationToken: cancellationToken);
        }
    }
}
