namespace CorporateTrainingManagementSystem.ViewModels.Course
{
    public class CourseVM
    {
        public int CourseId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public string InstructorName { get; set; } = string.Empty;
    }
}
