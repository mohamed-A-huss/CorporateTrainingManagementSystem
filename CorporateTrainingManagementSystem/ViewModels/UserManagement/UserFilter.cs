namespace CorporateTrainingManagementSystem.ViewModels.UserManagement
{
    public class UserFilter
    {
        public string? Search { get; set; }

        public string? Role { get; set; }

        public int? DepartmentId { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> Departments { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}
