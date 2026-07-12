namespace CorporateTrainingManagementSystem.ViewModels.Enrollment
{
    public class PaginatedEnrollment
    {
        public IEnumerable<EnrollmentVM> Enrollments { get; set; }
            = Enumerable.Empty<EnrollmentVM>();

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }

        public EnrollmentFilter? Filter { get; set; }
    }
}
