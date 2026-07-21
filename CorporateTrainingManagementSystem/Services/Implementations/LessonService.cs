using Microsoft.AspNetCore.Mvc.Rendering;

namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class LessonService : ILessonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        public LessonService(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }
        public async Task<PaginatedLesson> GetAllAsync(int page = 1, int pageSize = 10, LessonFilter? filter = null,
            CancellationToken cancellationToken = default)
        {
            var lessons = await _unitOfWork.Lessons.GetAsync(
                includes: [l => l.Course],
                cancellationToken: cancellationToken);

            if (filter is not null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Title))
                {
                    lessons = lessons.Where(l =>
                        l.Title.ToLower().Contains(filter.Title.ToLower()));
                }

                if (filter.CourseId.HasValue)
                {
                    lessons = lessons.Where(l =>
                        l.CourseId == filter.CourseId.Value);
                }
            }

            var totalCount = lessons.Count();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            lessons = lessons
                .Skip((page - 1) * pageSize)
                .Take(pageSize).OrderBy(l => l.Order);

            var result = lessons.Select(l => new LessonVM
            {
                LessonId = l.LessonId,
                Title = l.Title,
                Content = l.Content,
                VideoUrl = l.VideoUrl,
                PdfPath = l.PdfPath,
                Order = l.Order,
                CourseTitle = l.Course.Title
            });

            return new PaginatedLesson
            {
                Lessons = result,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalCount = totalCount,
                Filter = filter
            };
        }

        public async Task<LessonVM?> GetByIdAsync(int id)
        {
            var lesson = await _unitOfWork.Lessons.GetOneAsync(
                expression: l => l.LessonId == id,
                includes: [l => l.Course]);

            if (lesson == null)
                return null;

            return new LessonVM
            {
                LessonId = lesson.LessonId,
                Title = lesson.Title,
                Content = lesson.Content,
                VideoUrl = lesson.VideoUrl,
                PdfPath = lesson.PdfPath,
                Order = lesson.Order,
                CourseTitle = lesson.Course.Title
            };
        }
        public async Task<ServiceResult> CreateAsync(CreateLessonVM vm,CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(vm.Title))
                return ServiceResult.Failure("Lesson title is required.");

            var course = await _unitOfWork.Courses.GetOneAsync(
                expression: c => c.CourseId == vm.CourseId,
                tracked: false,
                cancellationToken: cancellationToken);

            if (course is null)
                return ServiceResult.Failure("Selected course was not found.");

            var lesson = new Lesson
            {
                Title = vm.Title.Trim(),
                Content = vm.Content,
                VideoUrl = vm.VideoUrl,
                Order = vm.Order,
                CourseId = vm.CourseId
            };

            if (vm.PdfFile is not null)
            {
                var uploadResult = await _fileService.UploadFileAsync(
                    vm.PdfFile,
                    UploadFolders.Pdfs,
                    cancellationToken);

                if (!uploadResult.Success)
                    return ServiceResult.Failure(uploadResult.ErrorMessage!);

                lesson.PdfPath = uploadResult.FilePath;
            }

            await _unitOfWork.Lessons.CreateAsync(
                lesson,
                cancellationToken);

            var rows = await _unitOfWork.CommitAsync(cancellationToken);

            if (rows == 0)
                return ServiceResult.Failure("Failed to create lesson.");

            return ServiceResult.SuccessResult("Lesson created successfully.");
        }





        public async Task<CreateLessonVM> GetCreateVMAsync()
        {
            var vm = new CreateLessonVM();

            await LoadCoursesAsync(vm);

            return vm;
        }

        public async Task<EditLessonVM?> GetEditVMAsync(int id)
        {
            var lesson = await _unitOfWork.Lessons.GetOneAsync(
                expression: l => l.LessonId == id,
                tracked: false);

            if (lesson is null)
                return null;

            var vm = new EditLessonVM
            {
                LessonId = lesson.LessonId,
                Title = lesson.Title,
                Content = lesson.Content,
                VideoUrl = lesson.VideoUrl,
                PdfPath = lesson.PdfPath,
                Order = lesson.Order,
                CourseId = lesson.CourseId
            };

            await LoadCoursesAsync(vm);

            return vm;
        }

        public async Task<ServiceResult> UpdateAsync(EditLessonVM vm, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(vm.Title))
                return ServiceResult.Failure("Lesson title is required.");

            var lesson = await _unitOfWork.Lessons.GetOneAsync(
                expression: l => l.LessonId == vm.LessonId,
                cancellationToken: cancellationToken);

            if (lesson is null)
                return ServiceResult.Failure("Lesson not found.");

            var course = await _unitOfWork.Courses.GetOneAsync(
                expression: c => c.CourseId == vm.CourseId,
                tracked: false,
                cancellationToken: cancellationToken);

            if (course is null)
                return ServiceResult.Failure("Selected course was not found.");

            lesson.Title = vm.Title.Trim();
            lesson.Content = vm.Content;
            lesson.VideoUrl = vm.VideoUrl;
            lesson.CourseId = vm.CourseId;
            lesson.Order = vm.Order;

            // Upload new PDF if provided
            if (vm.PdfFile is not null)
            {
                var uploadResult = await _fileService.UploadFileAsync(
                    vm.PdfFile,
                    UploadFolders.Pdfs,
                    cancellationToken);

                if (!uploadResult.Success)
                    return ServiceResult.Failure(uploadResult.ErrorMessage!);

                // Delete old PDF
                _fileService.DeleteFile(lesson.PdfPath);

                // Save new PDF path
                lesson.PdfPath = uploadResult.FilePath;
            }

            _unitOfWork.Lessons.Update(lesson);

            var rows = await _unitOfWork.CommitAsync(cancellationToken);

            if (rows == 0)
                return ServiceResult.Failure("Failed to update lesson.");

            return ServiceResult.SuccessResult("Lesson updated successfully.");
        }
        public async Task<ServiceResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var lesson = await _unitOfWork.Lessons.GetOneAsync(
                expression: l => l.LessonId == id,
                cancellationToken: cancellationToken);

            if (lesson is null)
                return ServiceResult.Failure("Lesson not found.");

            // Delete PDF file if exists
            if (!string.IsNullOrWhiteSpace(lesson.PdfPath))
            {
                _fileService.DeleteFile(lesson.PdfPath);
            }

            _unitOfWork.Lessons.Delete(lesson);

            var rows = await _unitOfWork.CommitAsync(cancellationToken);

            if (rows == 0)
                return ServiceResult.Failure("Failed to delete lesson.");

            return ServiceResult.SuccessResult("Lesson deleted successfully.");
        }
        public async Task LoadCoursesAsync(CreateLessonVM vm)
        {
            var courses = await _unitOfWork.Courses.GetAsync(tracked: false);

            vm.Courses = courses
                .OrderBy(c => c.Title)
                .Select(c => new SelectListItem
                {
                    Value = c.CourseId.ToString(),
                    Text = c.Title
                });
        }
        public async Task LoadCoursesAsync(EditLessonVM vm)
        {
            var courses = await _unitOfWork.Courses.GetAsync(tracked: false);

            vm.Courses = courses
                .OrderBy(c => c.Title)
                .Select(c => new SelectListItem
                {
                    Value = c.CourseId.ToString(),
                    Text = c.Title
                });
        }
    }
    
    }
