namespace CorporateTrainingManagementSystem.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }

        public string Title { get; set; }

        public string? Content { get; set; }

        public string? VideoUrl { get; set; }

        public string? PdfPath { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public ICollection<LessonProgress> LessonProgresses { get; set; }
    }
}
