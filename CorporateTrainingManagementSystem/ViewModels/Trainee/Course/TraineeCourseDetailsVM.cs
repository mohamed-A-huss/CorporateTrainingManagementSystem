namespace CorporateTrainingManagementSystem.ViewModels.Trainee.Course
{
    public class TraineeCourseDetailsVM
    {
        public int CourseId { get; set; }

        public int ExamId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int TotalLessons { get; set; }

        public int CompletedLessons { get; set; }

        public double ProgressPercentage { get; set; }

        public bool CanTakeExam { get; set; }

        public bool HasRemainingAttempts { get; set; }

        public int AttemptsUsed { get; set; }

        public int MaxAttempts { get; set; }

        public List<TraineeLessonVM> Lessons { get; set; } = [];
    }
}
