namespace CorporateTrainingManagementSystem.ViewModels.Trainee.Course
{
    public class TraineeLessonDetailsVM
    {
        public int LessonId { get; set; }

        public int CourseId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Content { get; set; }

        public string? VideoUrl { get; set; }

        public string? PdfPath { get; set; }

        public bool IsCompleted { get; set; }

        public int PreviousLessonId { get; set; }

        public int NextLessonId { get; set; }

        public bool HasPreviousLesson { get; set; }

        public bool HasNextLesson { get; set; }
    }
}
