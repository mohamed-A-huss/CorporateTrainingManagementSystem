namespace CorporateTrainingManagementSystem.ViewModels.Trainee.Exam
{
    public class TraineeExamStartVM
    {
        public int ExamId { get; set; }

        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public string ExamTitle { get; set; } = string.Empty;

        public int DurationMinutes { get; set; }

        public int PassMark { get; set; }

        public int TotalMarks { get; set; }

        public int QuestionsCount { get; set; }

        public int MaxAttempts { get; set; }

        public int AttemptsUsed { get; set; }

        public int AttemptsRemaining => MaxAttempts - AttemptsUsed;

        public bool CanStart { get; set; }
    }
}
