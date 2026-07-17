namespace CorporateTrainingManagementSystem.ViewModels.Instructor
{
    public class InstructorExamDetailsVM
    {
        public int ExamId { get; set; }

        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public int DurationMinutes { get; set; }

        public int PassMark { get; set; }

        public int TotalMarks { get; set; }

        public int QuestionsCount { get; set; }

        public IEnumerable<InstructorQuestionVM> Questions { get; set; }
            = Enumerable.Empty<InstructorQuestionVM>();
    }
}
