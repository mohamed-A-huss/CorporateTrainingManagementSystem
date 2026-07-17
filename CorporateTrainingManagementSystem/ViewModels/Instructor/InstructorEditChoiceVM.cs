using System.ComponentModel.DataAnnotations;

namespace CorporateTrainingManagementSystem.ViewModels.Instructor
{
    public class InstructorEditChoiceVM
    {
        public int ChoiceId { get; set; }

        public int QuestionId { get; set; }

        [Required]

        [MaxLength(250)]

        public string ChoiceText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}
