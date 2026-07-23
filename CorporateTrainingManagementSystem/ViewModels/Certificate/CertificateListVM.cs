namespace CorporateTrainingManagementSystem.ViewModels.Certificate
{
    public class CertificateListVM
    {
        public IEnumerable<AdminCertificateVM> Certificates { get; set; }
        = Enumerable.Empty<AdminCertificateVM>();

        public CertificateFilterVM Filter { get; set; }
            = new();

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }
    }
}
