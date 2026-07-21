namespace CorporateTrainingManagementSystem.ViewModels.Trainee.Certificate
{
    public class CertificateVM
    {
        public int CertificateId { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public string CertificateNumber { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; }
    }
}

