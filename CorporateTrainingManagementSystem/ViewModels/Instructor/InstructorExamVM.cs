namespace CorporateTrainingManagementSystem.ViewModels.Instructor
{
    public class InstructorExamVM
    {
        public int ExamId { get; set; }

        public string Title { get; set; } = string.Empty;

        public int DurationMinutes { get; set; }

        public int PassMark { get; set; }

        public int TotalMarks { get; set; }

        public int QuestionsCount { get; set; }
    }
}
