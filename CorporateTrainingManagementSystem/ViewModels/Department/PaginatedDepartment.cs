namespace CorporateTrainingManagementSystem.ViewModels.Department
{
    public class PaginatedDepartment
    {
        public IEnumerable<DepartmentVM> Departments { get; set; } = [];

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }
        public string? Query { get; set; } 
    }
}
