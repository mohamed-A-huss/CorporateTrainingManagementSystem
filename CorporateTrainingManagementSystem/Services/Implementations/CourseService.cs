
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<PaginatedCourse> GetAllAsync(int page = 1, int pageSize = 10, CourseFilter? filter = null, CancellationToken cancellationToken = default)
        {
            var courses = await _unitOfWork.Courses.GetAsync(
                includes: [c => c.Instructor],
                cancellationToken: cancellationToken);

            if (!courses.Any())
            {
                return new PaginatedCourse
                {
                    Courses = Enumerable.Empty<CourseVM>(),
                    CurrentPage = page,
                    TotalPages = 0,
                    TotalCount = 0,
                    Filter = filter
                };
            }

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Title))
                {
                    courses = courses.Where(c =>
                        c.Title.ToLower().Contains(filter.Title.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(filter.InstructorId))
                {
                    courses = courses.Where(c =>
                        c.InstructorId == filter.InstructorId);
                }

                if (filter.CreatedFrom.HasValue)
                {
                    courses = courses.Where(c =>
                        c.CreatedAt.Date >= filter.CreatedFrom.Value.Date);
                }

                if (filter.CreatedTo.HasValue)
                {
                    courses = courses.Where(c =>
                        c.CreatedAt.Date <= filter.CreatedTo.Value.Date);
                }
            }

            var totalCount = courses.Count();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            courses = courses
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return new PaginatedCourse
            {
                Courses = courses.Select(c => new CourseVM
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    CreatedAt = c.CreatedAt,
                    InstructorName = c.Instructor.FullName
                }),

                CurrentPage = page,
                TotalPages = totalPages,
                TotalCount = totalCount,
                Filter = filter
            };
        }

        public async Task<CourseVM?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var course = await _unitOfWork.Courses.GetOneAsync(
                c => c.CourseId == id,
                includes: [c => c.Instructor],
                cancellationToken: cancellationToken);

            if (course is null)
                return null;

            return new CourseVM
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                CreatedAt = course.CreatedAt,
                InstructorName = course.Instructor?.FullName ?? string.Empty
            };
        }

        public async Task<ServiceResult> CreateAsync(CreateCourseVM vm,CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(vm.Title))
                return ServiceResult.Failure("Course title is required.");

            var instructor = await _userManager.FindByIdAsync(vm.InstructorId);
            // TODO: Enable after implementing Roles & Authorization

            // bool isInstructor = await _userManager.IsInRoleAsync(instructor, "Instructor");

            // if (!isInstructor)
            //     return ServiceResult.Failure("The selected user is not an instructor.");

            if (instructor == null)
                return ServiceResult.Failure("Instructor not found.");

            var course = new Course
            {
                Title = vm.Title.Trim(),
                Description = vm.Description?.Trim(),
                InstructorId = vm.InstructorId
            };

            await _unitOfWork.Courses.CreateAsync(course, cancellationToken);

            var rows = await _unitOfWork.CommitAsync(cancellationToken);

            if (rows == 0)
                return ServiceResult.Failure("Failed to create course.");

            return ServiceResult.SuccessResult("Course created successfully.");
        }
        public async Task<CreateCourseVM> GetCreateVMAsync()
        {
            return new CreateCourseVM
            {
                Instructors = await GetInstructorSelectListAsync()
            };
        }







        public async Task<EditCourseVM?> GetEditVMAsync(int id)
        {
            var course = await _unitOfWork.Courses.GetOneAsync(c => c.CourseId == id);

            if (course == null)
                return null;

            return new EditCourseVM
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                InstructorId = course.InstructorId,
                Instructors = await GetInstructorSelectListAsync()
            };
        }

        public async Task<ServiceResult> UpdateAsync(EditCourseVM vm, CancellationToken cancellationToken = default)
        {
            var course = await _unitOfWork.Courses.GetOneAsync(
                c => c.CourseId == vm.CourseId);

            if (course == null)
                return ServiceResult.Failure("Course not found.");

            if (string.IsNullOrWhiteSpace(vm.Title))
                return ServiceResult.Failure("Course title is required.");

            var instructor = await _userManager.FindByIdAsync(vm.InstructorId);

            if (instructor == null)
                return ServiceResult.Failure("Instructor not found.");

            // TODO: Enable after implementing Roles
            /*
            bool isInstructor = await _userManager.IsInRoleAsync(instructor, "Instructor");

            if (!isInstructor)
                return ServiceResult.Failure("The selected user is not an instructor.");
            */

            course.Title = vm.Title.Trim();
            course.Description = vm.Description?.Trim();
            course.InstructorId = vm.InstructorId;

            _unitOfWork.Courses.Update(course);

            var rows = await _unitOfWork.CommitAsync(cancellationToken);

            if (rows == 0)
                return ServiceResult.Failure("Failed to update course.");

            return ServiceResult.SuccessResult("Course updated successfully.");
        }

        public async Task<ServiceResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var course = await _unitOfWork.Courses.GetOneAsync(
                c => c.CourseId == id);

            if (course == null)
                return ServiceResult.Failure("Course not found.");

            _unitOfWork.Courses.Delete(course);

            var rows = await _unitOfWork.CommitAsync(cancellationToken);

            if (rows == 0)
                return ServiceResult.Failure("Failed to delete course.");

            return ServiceResult.SuccessResult("Course deleted successfully.");
        }
        private async Task<IEnumerable<SelectListItem>> GetInstructorSelectListAsync()
        {
            var users = await _userManager.Users
                .OrderBy(u => u.FullName)
                .ToListAsync();

            // TODO: Filter by Instructor role
            // TODO: After implementing Roles
            // var instructors = new List<ApplicationUser>();
            //
            // foreach (var user in users)
            // {
            //     if (await _userManager.IsInRoleAsync(user, "Instructor"))
            //     {
            //         instructors.Add(user);
            //     }
            // }
            //
            // users = instructors;

            return users.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.FullName
            });
        }
    }
}
