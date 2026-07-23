namespace CorporateTrainingManagementSystem.ViewModels.Certificate
{
    public class AdminCertificateVM
    {
        public int CertificateId { get; set; }

        public string CertificateNumber { get; set; } = string.Empty;

        public string StudentName { get; set; } = string.Empty;

        public string StudentEmail { get; set; } = string.Empty;

        public string CourseTitle { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; }
    }
}
