using System.ComponentModel.DataAnnotations;

namespace CorporateTrainingManagementSystem.ViewModels.Enrollment
{
    public class CreateEnrollmentVM
    {
        [Required]
        [Display(Name = "Trainee")]
        public string TraineeId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        public IEnumerable<SelectListItem> Trainees { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> Courses { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}
