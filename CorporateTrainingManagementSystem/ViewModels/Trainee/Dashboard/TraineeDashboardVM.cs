namespace CorporateTrainingManagementSystem.ViewModels.Trainee.Dashboard
{
    public class TraineeDashboardVM
    {
        public string FullName { get; set; } = string.Empty;

        public int Points { get; set; }

        public int ActiveCourses { get; set; }

        public int CompletedCourses { get; set; }

        public int BadgesCount { get; set; }

        public int CertificatesCount { get; set; }
    }
}
