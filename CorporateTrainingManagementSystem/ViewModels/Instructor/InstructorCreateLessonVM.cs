using System.ComponentModel.DataAnnotations;

namespace CorporateTrainingManagementSystem.ViewModels.Instructor
{
    public class InstructorCreateLessonVM
    {
        public int CourseId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        public string? Content { get; set; }


        [Url]
        public string? VideoUrl { get; set; }

        public IFormFile? PdfFile { get; set; }
        [Range(1, 1000)]
        public int Order { get; set; }
    }
}
