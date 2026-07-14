using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CorporateTrainingManagementSystem.ViewModels.Course
{
    public class EditCourseVM
    {
        public int CourseId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public string InstructorId { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> Instructors { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}
