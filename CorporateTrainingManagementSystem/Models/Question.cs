namespace CorporateTrainingManagementSystem.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        public string QuestionText { get; set; }

        public string QuestionType { get; set; }

        public int ExamId { get; set; }

        public Exam Exam { get; set; }

        public ICollection<Choice> Choices { get; set; }
    }
}
