using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IWebHostEnvironment _env;

        public DepartmentController(
            IUnitOfWork unitOfWork
            //IDepartmentRepository repository
            /*, IWebHostEnvironment env*/)
        {
            _unitOfWork = unitOfWork;
            //_repository = repository;
            //_env = env;
        }

        public async Task<IActionResult> Index(string AlertColor)
        {
            var department = await _unitOfWork.Repository<Department>().GetAllAsync();
            ViewData["AlertColor"] = AlertColor;
            //ViewBag.Message = "Hi from ViewBag";
            return View(department);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Repository<Department>().Add(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "New Department is Created";
                    return RedirectToAction(nameof(Index), new { AlertColor = "alert-primary" });
                }
            }
            return View(department);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var department = await _unitOfWork.Repository<Department>().GetAsync(id.Value);
            if (department is null)
                return NotFound();

            return View(viewName, department);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, Department department)
        {
            if (id != department.Id)
                return BadRequest();
            if (!ModelState.IsValid)
                return View(department);

            try
            {

                _unitOfWork.Repository<Department>().Update(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "One Department is Updated";
                    return RedirectToAction(nameof(Index), new { AlertColor = "alert-success" });
                }
            }
            catch (Exception ex)
            {
                // 1. Log Exception 
                // 2. Friendly Message 

                ModelState.AddModelError(string.Empty, ex.Message);

            }
            return View(department);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Department department, [FromRoute] int id)
        {
			if (id != department.Id)
				return BadRequest();

			try
            {

                _unitOfWork.Repository<Department>().Delete(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "One Department is Deleted";
                    return RedirectToAction(nameof(Index), new { AlertColor = "alert-danger" });
                }
            }
            catch (Exception ex)
            {
                // 1. Log Exception 
                // 2. Friendly Message 

                ModelState.AddModelError(string.Empty, ex.Message);

            }
            return View(department);
        }
    }
}
