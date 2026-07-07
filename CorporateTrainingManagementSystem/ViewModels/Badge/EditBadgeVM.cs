using System.ComponentModel.DataAnnotations;

namespace CorporateTrainingManagementSystem.ViewModels.Badge
{
    public class EditBadgeVM
    {
        public int BadgeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
        
        public string? Description { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Required points must be a non-negative integer.")]
        public int RequiredPoints { get; set; }
    }
}
