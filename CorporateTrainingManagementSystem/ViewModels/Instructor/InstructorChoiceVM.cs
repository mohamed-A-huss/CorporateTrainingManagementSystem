namespace CorporateTrainingManagementSystem.ViewModels.Instructor
{
    public class InstructorChoiceVM
    {
        public int ChoiceId { get; set; }

        public string ChoiceText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}
