namespace CorporateTrainingManagementSystem.ViewModels.UserManagement
{
    public class PaginatedUsers
    {
        public IEnumerable<UserVM> Users { get; set; }
            = Enumerable.Empty<UserVM>();

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }

        public UserFilter? Filter { get; set; }
    }
}
