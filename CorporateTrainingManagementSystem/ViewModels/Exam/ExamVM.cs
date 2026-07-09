namespace CorporateTrainingManagementSystem.ViewModels.Exam
{
    public class ExamVM
    {
        public int ExamId { get; set; }

        public string Title { get; set; } = string.Empty;

        public int DurationMinutes { get; set; }

        public int PassMark { get; set; }

        public int TotalMarks { get; set; }

        public string CourseTitle { get; set; } = string.Empty;
    }
}
