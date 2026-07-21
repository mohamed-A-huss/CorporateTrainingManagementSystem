using CorporateTrainingManagementSystem.Repositories.Implementations;
using CorporateTrainingManagementSystem.ViewModels.Exam;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class ExamService : IExamService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExamService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PaginatedExam> GetAllAsync(int page = 1, int pageSize = 10, ExamFilter? filter = null, CancellationToken cancellationToken = default)
        {
            var exams = await _unitOfWork.Exams.GetAsync(
                includes: [e => e.Course],
                tracked: false,
                cancellationToken: cancellationToken);

            if (!exams.Any())
            {
                return new PaginatedExam
                {
                    Exams = Enumerable.Empty<ExamVM>(),
                    CurrentPage = page,
                    TotalPages = 0,
                    TotalCount = 0,
                    Filter = filter
                };
            }

            if (filter is not null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Title))
                {
                    exams = exams.Where(e =>
                        e.Title.Contains(filter.Title, StringComparison.OrdinalIgnoreCase));
                }

                if (filter.CourseId.HasValue)
                {
                    exams = exams.Where(e =>
                        e.CourseId == filter.CourseId.Value);
                }

                if (filter.MinPassMark.HasValue)
                {
                    exams = exams.Where(e =>
                        e.PassMark >= filter.MinPassMark.Value);
                }

                if (filter.MaxPassMark.HasValue)
                {
                    exams = exams.Where(e =>
                        e.PassMark <= filter.MaxPassMark.Value);
                }
            }

            var totalCount = exams.Count();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            exams = exams
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return new PaginatedExam
            {
                Exams = exams.Select(e => new ExamVM
                {
                    ExamId = e.ExamId,
                    Title = e.Title,
                    DurationMinutes = e.DurationMinutes,
                    PassMark = e.PassMark,
                    MaxAttempts = e.MaxAttempts,
                    TotalMarks = e.TotalMarks,
                    CourseTitle = e.Course.Title
                }),

                CurrentPage = page,
                TotalPages = totalPages,
                TotalCount = totalCount,
                Filter = filter
            };
        }

        public async Task<ExamVM?> GetByIdAsync(int id)
        {
            var exam = await _unitOfWork.Exams.GetOneAsync(
                expression: e => e.ExamId == id,
                includes: [e => e.Course],
                tracked: false);

            if (exam is null)
                return null;

            return new ExamVM
            {
                ExamId = exam.ExamId,
                Title = exam.Title,
                DurationMinutes = exam.DurationMinutes,
                PassMark = exam.PassMark,
                MaxAttempts = exam.MaxAttempts,
                TotalMarks = exam.TotalMarks,
                CourseTitle = exam.Course.Title
            };
        }
        public async Task<ServiceResult> CreateAsync(CreateExamVM vm, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(vm.Title))
                return ServiceResult.Failure("Exam title is required.");

            if (vm.PassMark > vm.TotalMarks)
                return ServiceResult.Failure("Pass mark cannot be greater than total marks.");
            if (vm.DurationMinutes <= 0)
                return ServiceResult.Failure("Duration must be greater than zero.");

            if (vm.TotalMarks <= 0)
                return ServiceResult.Failure("Total marks must be greater than zero.");

            var course = await _unitOfWork.Courses.GetOneAsync(
                expression: c => c.CourseId == vm.CourseId,
                tracked: false,
                cancellationToken: cancellationToken);

            if (course is null)
                return ServiceResult.Failure("Selected course was not found.");

            var exam = new Exam
            {
                Title = vm.Title.Trim(),
                DurationMinutes = vm.DurationMinutes,
                PassMark = vm.PassMark,
                MaxAttempts = vm.MaxAttempts,
                TotalMarks = vm.TotalMarks,
                CourseId = vm.CourseId
            };

            await _unitOfWork.Exams.CreateAsync(
                exam,
                cancellationToken);

            var rows = await _unitOfWork.CommitAsync(cancellationToken);

            if (rows == 0)
                return ServiceResult.Failure("Failed to create exam.");

            return ServiceResult.SuccessResult("Exam created successfully.");
        }
        public async Task<CreateExamVM> GetCreateVMAsync()
        {
            var vm = new CreateExamVM();

            await LoadCoursesAsync(vm);

            return vm;
        }



        public async Task<ServiceResult> UpdateAsync(EditExamVM vm, CancellationToken cancellationToken = default)
        {
            var exam = await _unitOfWork.Exams.GetOneAsync(
                expression: e => e.ExamId == vm.ExamId,
                cancellationToken: cancellationToken);

            if (exam is null)
                return ServiceResult.Failure("Exam not found.");

            if (string.IsNullOrWhiteSpace(vm.Title))
                return ServiceResult.Failure("Exam title is required.");

            if (vm.PassMark > vm.TotalMarks)
                return ServiceResult.Failure("Pass mark cannot be greater than total marks.");

            var course = await _unitOfWork.Courses.GetOneAsync(
                expression: c => c.CourseId == vm.CourseId,
                tracked: false,
                cancellationToken: cancellationToken);

            if (course is null)
                return ServiceResult.Failure("Selected course was not found.");

            exam.Title = vm.Title.Trim();
            exam.DurationMinutes = vm.DurationMinutes;
            exam.PassMark = vm.PassMark;
            exam.MaxAttempts = vm.MaxAttempts;
            exam.TotalMarks = vm.TotalMarks;
            exam.CourseId = vm.CourseId;

            _unitOfWork.Exams.Update(exam);

            var rows = await _unitOfWork.CommitAsync(cancellationToken);

            if (rows == 0)
                return ServiceResult.Failure("Failed to update exam.");

            return ServiceResult.SuccessResult("Exam updated successfully.");
        }


        public async Task<EditExamVM?> GetEditVMAsync(int id)
        {
            var exam = await _unitOfWork.Exams.GetOneAsync(
                expression: e => e.ExamId == id,
                tracked: false);

            if (exam is null)
                return null;

            var vm = new EditExamVM
            {
                ExamId = exam.ExamId,
                Title = exam.Title,
                DurationMinutes = exam.DurationMinutes,
                PassMark = exam.PassMark,
                MaxAttempts = exam.MaxAttempts,
                TotalMarks = exam.TotalMarks,
                CourseId = exam.CourseId
            };

            await LoadCoursesAsync(vm);

            return vm;
        }

        public async Task LoadCoursesAsync(CreateExamVM vm)
        {
            var courses = await _unitOfWork.Courses.GetAsync(
                tracked: false);

            vm.Courses = courses
                .OrderBy(c => c.Title)
                .Select(c => new SelectListItem
                {
                    Value = c.CourseId.ToString(),
                    Text = c.Title
                });
        }

        public async Task LoadCoursesAsync(EditExamVM vm)
        {
            var courses = await _unitOfWork.Courses.GetAsync(
                tracked: false);

            vm.Courses = courses
                .OrderBy(c => c.Title)
                .Select(c => new SelectListItem
                {
                    Value = c.CourseId.ToString(),
                    Text = c.Title
                });
        }


        public async Task<ServiceResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var exam = await _unitOfWork.Exams.GetOneAsync(
                expression: e => e.ExamId == id,
                cancellationToken: cancellationToken);

            if (exam is null)
                return ServiceResult.Failure("Exam not found.");

            _unitOfWork.Exams.Delete(exam);

            var rows = await _unitOfWork.CommitAsync(cancellationToken);

            if (rows == 0)
                return ServiceResult.Failure("Failed to delete exam.");

            return ServiceResult.SuccessResult("Exam deleted successfully.");
        }
    }
}
