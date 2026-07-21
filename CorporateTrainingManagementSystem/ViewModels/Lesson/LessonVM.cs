using System.ComponentModel.DataAnnotations;

namespace CorporateTrainingManagementSystem.ViewModels.Lesson
{
    public class LessonVM
    {
        public int LessonId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Content { get; set; }

        public string? VideoUrl { get; set; }

        public string? PdfPath { get; set; }

        public string CourseTitle { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
