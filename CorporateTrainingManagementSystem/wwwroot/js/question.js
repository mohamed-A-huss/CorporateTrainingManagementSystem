document.addEventListener("DOMContentLoaded", function () {

    const courseSelect = document.getElementById("CourseId");
    const examSelect = document.getElementById("ExamId");
    const typeSelect = document.getElementById("QuestionType");

    const mcqSection = document.getElementById("mcq-section");
    const tfSection = document.getElementById("truefalse-section");

    function toggleQuestionType() {

        if (typeSelect.value === "0") {

            mcqSection.style.display = "block";
            tfSection.style.display = "none";
        }
        else {

            mcqSection.style.display = "none";
            tfSection.style.display = "block";
        }
    }

    typeSelect.addEventListener("change", toggleQuestionType);

    toggleQuestionType();

    courseSelect.addEventListener("change", async function () {

        examSelect.innerHTML =
            '<option value="">Select Exam</option>';

        if (!courseSelect.value)
            return;

        const response =
            await fetch(`/Admin/Question/GetExams?courseId=${courseSelect.value}`);

        if (!response.ok)
            return;

        const exams = await response.json();

        exams.forEach(exam => {

            const option =
                document.createElement("option");

            option.value = exam.value;
            option.text = exam.text;

            examSelect.appendChild(option);

        });

    });

});