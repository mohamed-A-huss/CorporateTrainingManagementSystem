namespace CorporateTrainingManagementSystem.ViewModels.Instructor.Student
{
    public class CourseStudentsVM

    {
        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public List<InstructorStudentProgressVM> Students { get; set; } = [];
    }
}
