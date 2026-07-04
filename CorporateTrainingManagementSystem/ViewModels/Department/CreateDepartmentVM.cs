using System.ComponentModel.DataAnnotations;

namespace CorporateTrainingManagementSystem.ViewModels.Department
{
    public class CreateDepartmentVM
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
    }
}
