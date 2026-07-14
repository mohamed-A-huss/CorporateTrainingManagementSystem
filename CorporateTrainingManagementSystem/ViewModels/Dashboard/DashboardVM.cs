namespace CorporateTrainingManagementSystem.ViewModels.Dashboard
{
    public class DashboardVM
    {
        // Statistics

        public int TotalUsers { get; set; }

        public int TotalAdmins { get; set; }

        public int TotalInstructors { get; set; }

        public int TotalTrainees { get; set; }

        public int TotalDepartments { get; set; }

        public int TotalCourses { get; set; }

        public int TotalLessons { get; set; }

        public int TotalExams { get; set; }

        public int TotalQuestions { get; set; }

        public int TotalBadges { get; set; }

        public int TotalCertificates { get; set; }


        // Latest Data

        public IEnumerable<UserVM> LatestUsers { get; set; }
            = Enumerable.Empty<UserVM>();

        public IEnumerable<CourseVM> LatestCourses { get; set; }
            = Enumerable.Empty<CourseVM>();
    }
}
