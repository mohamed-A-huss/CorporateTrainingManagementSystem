using System.ComponentModel.DataAnnotations;

namespace CorporateTrainingManagementSystem.ViewModels.Instructor
{
    public class InstructorCreateChoiceVM
    {
        public int QuestionId { get; set; }

        [Required]

        [MaxLength(250)]

        public string ChoiceText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}
