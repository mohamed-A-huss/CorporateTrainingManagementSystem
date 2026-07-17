using System.ComponentModel.DataAnnotations;

namespace CorporateTrainingManagementSystem.ViewModels.Instructor
{
    public class InstructorEditLessonVM
    {
        public int LessonId { get; set; }

        public int CourseId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        public string? Content { get; set; }

        [Url]
        public string? VideoUrl { get; set; }

        public string? ExistingPdf { get; set; }

        public IFormFile? PdfFile { get; set; }
    }
}
