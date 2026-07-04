using CorporateTrainingManagementSystem.Services.Interfaces;
using CorporateTrainingManagementSystem.ViewModels.Department;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace CorporateTrainingManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        public async Task<IActionResult> Index()
        {
            var departments = await _departmentService.GetAllAsync();

            return View(departments);
        }
        public async Task<IActionResult> Details(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);

            if (department == null)
                return NotFound();

            return View(department);
        }
        public IActionResult Create()
        {
            return View(new CreateDepartmentVM());
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _departmentService.CreateAsync(vm);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message!);
                return View(vm);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);

            if (department == null)
                return NotFound();

            var vm = department.Adapt<EditDepartmentVM>();

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditDepartmentVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _departmentService.UpdateAsync(vm);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message!);
                return View(vm);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);

            if (department == null)
                return NotFound();

            return View(department);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _departmentService.DeleteAsync(id);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

    }
}
