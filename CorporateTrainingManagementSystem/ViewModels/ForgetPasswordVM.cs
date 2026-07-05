using System.ComponentModel.DataAnnotations;

namespace CorporateTrainingManagementSystem.ViewModels
{
    public class ForgetPasswordVM
    {
        public int Id { get; set; }
        [Required]
        public string EmailOrUserName { get; set; } = string.Empty;
    }
}