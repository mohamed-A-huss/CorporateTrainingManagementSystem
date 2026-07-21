namespace CorporateTrainingManagementSystem.Models
{
    public class Exam
    {
        public int ExamId { get; set; }

        public string Title { get; set; }

        public int DurationMinutes { get; set; }

        public int PassMark { get; set; }

        public int TotalMarks { get; set; }
        public int MaxAttempts { get; set; } = 3;

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public ICollection<Question> Questions { get; set; }

        public ICollection<ExamAttempt> ExamAttempts { get; set; }
    }
}
