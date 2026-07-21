namespace CorporateTrainingManagementSystem.ViewModels.Trainee.Certificate
{
    public class VerifyCertificateVM
    {
        public bool IsValid { get; set; }

        public string StudentName { get; set; } = string.Empty;

        public string CourseTitle { get; set; } = string.Empty;

        public string CertificateNumber { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; }
    }
}
