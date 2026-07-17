namespace CorporateTrainingManagementSystem.ViewModels.Instructor
{
    public class InstructorQuestionsVM
    {
        public int ExamId { get; set; }

        public IEnumerable<InstructorQuestionVM> Questions { get; set; }
            = Enumerable.Empty<InstructorQuestionVM>();
    }
}
