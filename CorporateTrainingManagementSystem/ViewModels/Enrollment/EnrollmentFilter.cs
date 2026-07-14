namespace CorporateTrainingManagementSystem.ViewModels.Enrollment
{
    public class EnrollmentFilter
    {
        public string? TraineeId { get; set; }

        public int? CourseId { get; set; }

        public EnrollmentStatus? Status { get; set; }

        public IEnumerable<SelectListItem> Trainees { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> Courses { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}
