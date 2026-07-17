
namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class InstructorExamService : IInstructorExamService
    {
        private readonly IUnitOfWork _unitOfWork;
        public InstructorExamService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> CreateQuestionAsync(InstructorCreateQuestionVM vm,string instructorId,CancellationToken cancellationToken = default)
        {
            var exam = await _unitOfWork.Exams.GetOneAsync(
                e => e.ExamId == vm.ExamId,
                includes:
                [
                    e => e.Course
                ],
                cancellationToken: cancellationToken);

            if (exam is null)
                return ServiceResult.Failure("Exam not found.");

            if (exam.Course.InstructorId != instructorId)
                return ServiceResult.Failure("Unauthorized.");

            var question = new Question
            {
                ExamId = vm.ExamId,

                QuestionText = vm.QuestionText,

                QuestionType = vm.QuestionType,

                Mark = vm.Mark
            };

            await _unitOfWork.Questions.CreateAsync(
                question,
                cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return ServiceResult.SuccessResult("Question created successfully.");
        }

        public async Task<ServiceResult> DeleteQuestionAsync(int questionId,string instructorId,CancellationToken cancellationToken = default)
        {
            var question = await _unitOfWork.Questions.GetOneAsync(
                q => q.QuestionId == questionId,
                includes:
                [
                    q => q.Exam
                ],
                cancellationToken: cancellationToken);

            if (question is null)
                return ServiceResult.Failure("Question not found.");

            var exam = await _unitOfWork.Exams.GetOneAsync(
                e => e.ExamId == question.ExamId,
                includes:
                [
                    e => e.Course
                ],
                cancellationToken: cancellationToken);

            if (exam is null)
                return ServiceResult.Failure("Exam not found.");

            if (exam.Course.InstructorId != instructorId)
                return ServiceResult.Failure("Unauthorized.");

            _unitOfWork.Questions.Delete(question);

            await _unitOfWork.CommitAsync(cancellationToken);

            return ServiceResult.SuccessResult("Question deleted successfully.");
        }

        public async Task<InstructorExamDetailsVM?> GetDetailsAsync(int examId,string instructorId,CancellationToken cancellationToken = default)
        {
            var exam = await _unitOfWork.Exams.GetOneAsync(
                e => e.ExamId == examId &&
                     e.Course.InstructorId == instructorId,
                includes:
                [
                    e => e.Course,
                    e => e.Questions
                ],
                cancellationToken: cancellationToken);

            if (exam is null)
                return null;

            var questionIds = exam.Questions
                .Select(q => q.QuestionId)
                .ToList();

            var choices = await _unitOfWork.Choices.GetAsync(
                c => questionIds.Contains(c.QuestionId),
                cancellationToken: cancellationToken);

            return new InstructorExamDetailsVM
            {
                ExamId = exam.ExamId,

                CourseId = exam.CourseId,

                CourseTitle = exam.Course.Title,

                Title = exam.Title,

                DurationMinutes = exam.DurationMinutes,

                PassMark = exam.PassMark,

                TotalMarks = exam.TotalMarks,

                QuestionsCount = exam.Questions.Count,

                Questions = exam.Questions
                    .OrderBy(q => q.QuestionId)
                    .Select(q => new InstructorQuestionVM
                    {
                        QuestionId = q.QuestionId,

                        QuestionText = q.QuestionText,

                        QuestionType = q.QuestionType,

                        Mark = q.Mark,

                        ChoicesCount = choices.Count(c => c.QuestionId == q.QuestionId)
                    })
                    .ToList()
            };
        }
        public async Task<ServiceResult> UpdateQuestionAsync(InstructorEditQuestionVM vm,string instructorId,CancellationToken cancellationToken = default)
        {
            var question = await _unitOfWork.Questions.GetOneAsync(
                q => q.QuestionId == vm.QuestionId,
                includes:
                [
                    q => q.Exam
                ],
                cancellationToken: cancellationToken);

            if (question is null)
                return ServiceResult.Failure("Question not found.");

            var exam = await _unitOfWork.Exams.GetOneAsync(
                e => e.ExamId == question.ExamId,
                includes:
                [
                    e => e.Course
                ],
                cancellationToken: cancellationToken);

            if (exam is null)
                return ServiceResult.Failure("Exam not found.");

            if (exam.Course.InstructorId != instructorId)
                return ServiceResult.Failure("Unauthorized.");

            question.QuestionText = vm.QuestionText;

            question.QuestionType = vm.QuestionType;

            question.Mark = vm.Mark;

            _unitOfWork.Questions.Update(question);

            await _unitOfWork.CommitAsync(cancellationToken);

            return ServiceResult.SuccessResult("Question updated successfully.");
        }
        public async Task<InstructorQuestionDetailsVM?> GetQuestionDetailsAsync(int questionId,string instructorId,CancellationToken cancellationToken = default)
        {
            var question = await _unitOfWork.Questions.GetOneAsync(
                q => q.QuestionId == questionId,
                includes:
                [
                    q => q.Exam,
            q => q.Choices
                ],
                cancellationToken: cancellationToken);

            if (question is null)
                return null;

            var exam = await _unitOfWork.Exams.GetOneAsync(
                e => e.ExamId == question.ExamId,
                includes:
                [
                    e => e.Course
                ],
                cancellationToken: cancellationToken);

            if (exam is null)
                return null;

            if (exam.Course.InstructorId != instructorId)
                return null;

            return new InstructorQuestionDetailsVM
            {
                QuestionId = question.QuestionId,

                ExamId = exam.ExamId,

                ExamTitle = exam.Title,

                QuestionText = question.QuestionText,

                QuestionType = question.QuestionType,

                Mark = question.Mark,

                Choices = question.Choices
                    .OrderBy(c => c.ChoiceId)
                    .Select(c => new InstructorChoiceVM
                    {
                        ChoiceId = c.ChoiceId,

                        ChoiceText = c.ChoiceText,

                        IsCorrect = c.IsCorrect
                    })
                    .ToList()
            };
        }
        public async Task<ServiceResult> CreateChoiceAsync(InstructorCreateChoiceVM vm,string instructorId,CancellationToken cancellationToken = default)
        {
            var question = await GetAuthorizedQuestionAsync(
                vm.QuestionId,
                instructorId,
                cancellationToken);

            if (question is null)
                return ServiceResult.Failure("Unauthorized.");

            var validation = await ValidateChoiceRulesAsync(
                vm.QuestionId,
                vm.IsCorrect,
                null,
                cancellationToken);

            if (validation is not null)
                return validation;

            var choice = new Choice
            {
                ChoiceText = vm.ChoiceText,

                IsCorrect = vm.IsCorrect,

                QuestionId = vm.QuestionId
            };

            await _unitOfWork.Choices.CreateAsync(
                choice,
                cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return ServiceResult.SuccessResult(
                "Choice created successfully.");
        }
        public async Task<ServiceResult> UpdateChoiceAsync(InstructorEditChoiceVM vm,string instructorId,CancellationToken cancellationToken = default)
        {
            var choice = await GetAuthorizedChoiceAsync(
                vm.ChoiceId,
                instructorId,
                cancellationToken);

            if (choice is null)
                return ServiceResult.Failure("Choice not found.");

            var validation = await ValidateChoiceRulesAsync(
                choice.QuestionId,
                vm.IsCorrect,
                vm.ChoiceId,
                cancellationToken);

            if (validation is not null)
                return validation;

            choice.ChoiceText = vm.ChoiceText;

            choice.IsCorrect = vm.IsCorrect;

            _unitOfWork.Choices.Update(choice);

            await _unitOfWork.CommitAsync(cancellationToken);

            return ServiceResult.SuccessResult(
                "Choice updated successfully.");
        }
        public async Task<ServiceResult> DeleteChoiceAsync(int choiceId,string instructorId,CancellationToken cancellationToken = default)
        {
            var choice = await GetAuthorizedChoiceAsync(
                choiceId,
                instructorId,
                cancellationToken);

            if (choice is null)
                return ServiceResult.Failure("Choice not found.");

            var choices = await _unitOfWork.Choices.GetAsync(
                c => c.QuestionId == choice.QuestionId,
                cancellationToken: cancellationToken);

            if (choices.Count() <= 1)
                return ServiceResult.Failure(
                    "A question must contain at least one choice.");

            _unitOfWork.Choices.Delete(choice);

            await _unitOfWork.CommitAsync(cancellationToken);

            return ServiceResult.SuccessResult(
                "Choice deleted successfully.");
        }
        private async Task<Question?> GetAuthorizedQuestionAsync(int questionId,string instructorId,CancellationToken cancellationToken)
        {
            var question = await _unitOfWork.Questions.GetOneAsync(
                q => q.QuestionId == questionId,
                includes:
                [
                    q => q.Exam
                ],
                cancellationToken: cancellationToken);

            if (question is null)
                return null;

            var exam = await _unitOfWork.Exams.GetOneAsync(
                e => e.ExamId == question.ExamId,
                includes:
                [
                    e => e.Course
                ],
                cancellationToken: cancellationToken);

            if (exam is null)
                return null;

            if (exam.Course.InstructorId != instructorId)
                return null;

            return question;
        }
        private async Task<ServiceResult?> ValidateChoiceRulesAsync(int questionId,bool isCorrect,int? currentChoiceId,CancellationToken cancellationToken)
        {
            var question = await _unitOfWork.Questions.GetOneAsync(
                q => q.QuestionId == questionId,
                cancellationToken: cancellationToken);

            if (question is null)
                return ServiceResult.Failure("Question not found.");

            var choices = await _unitOfWork.Choices.GetAsync(
                c => c.QuestionId == questionId,
                cancellationToken: cancellationToken);

            if (question.QuestionType == QuestionType.TrueFalse)
            {
                if (choices.Count() >= 2 && currentChoiceId is null)
                    return ServiceResult.Failure(
                        "True/False questions cannot have more than two choices.");
            }

            if (isCorrect)
            {
                var anotherCorrect = choices.Any(c =>
                    c.IsCorrect &&
                    c.ChoiceId != currentChoiceId);

                if (anotherCorrect)
                    return ServiceResult.Failure(
                        "Only one correct answer is allowed.");
            }

            return null;
        }
        private async Task<Choice?> GetAuthorizedChoiceAsync(int choiceId,string instructorId,CancellationToken cancellationToken)
        {
            var choice = await _unitOfWork.Choices.GetOneAsync(
                c => c.ChoiceId == choiceId,
                includes:
                [
                    c => c.Question
                ],
                cancellationToken: cancellationToken);

            if (choice is null)
                return null;

            var question = await GetAuthorizedQuestionAsync(
                choice.QuestionId,
                instructorId,
                cancellationToken);

            if (question is null)
                return null;

            return choice;
        }
    }
}
