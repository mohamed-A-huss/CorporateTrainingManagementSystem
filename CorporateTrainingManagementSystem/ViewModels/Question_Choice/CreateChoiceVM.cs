using System.ComponentModel.DataAnnotations;

namespace CorporateTrainingManagementSystem.ViewModels.Question_Choice
{
    public class CreateChoiceVM
    {
        [Required]
        [StringLength(300)]
        public string ChoiceText { get; set; } = string.Empty;

    }
}
