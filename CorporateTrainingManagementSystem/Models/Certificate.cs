namespace CorporateTrainingManagementSystem.Models
{
    public class Certificate
    {
        public int CertificateId { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public string CertificateNumber { get; set; }

        public DateTime IssueDate { get; set; } = DateTime.Now;
    }
}
