using System.ComponentModel.DataAnnotations;

namespace CorporateTrainingManagementSystem.ViewModels.UserManagement
{
    public class ChangeRoleVM
    {
        public string UserId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Role")]
        public string SelectedRole { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> Roles { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}
