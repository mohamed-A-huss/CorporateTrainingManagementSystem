using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CorporateTrainingManagementSystem.ViewModels.Lesson
{
    public class CreateLessonVM
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        public string? Content { get; set; }

        [Display(Name = "Video URL")]
        [Url]
        public string? VideoUrl { get; set; }

        public IFormFile? PdfFile { get; set; }
        [Range(1, 1000)]
        public int Order { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        public IEnumerable<SelectListItem> Courses { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}
