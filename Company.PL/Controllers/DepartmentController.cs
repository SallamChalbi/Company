using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;

namespace Company.PL.Controllers
{
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

        public IActionResult Index(string AlertColor)
        {
            var department = _unitOfWork.Repository<Department>().GetAll();
            ViewData["AlertColor"] = AlertColor;
            //ViewBag.Message = "Hi from ViewBag";
            return View(department);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Repository<Department>().Add(department);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    TempData["Message"] = "New Department is Created";
                    return RedirectToAction(nameof(Index),new { AlertColor = "alert-primary" });
                }
            }
            return View(department);
        }

        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var department = _unitOfWork.Repository<Department>().Get(id.Value);
            if(department is null)
                return NotFound();

            return View(viewName, department);
        }

        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id, Department department)
        {
            if(id != department.Id)
                return BadRequest();
            if (!ModelState.IsValid)
                return View(department);

            try
            {
                _unitOfWork.Repository<Department>().Update(department);
                var count = _unitOfWork.Complete();
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

        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        public IActionResult Delete(Department department)
        {
            try
            {
                _unitOfWork.Repository<Department>().Delete(department);
                var count = _unitOfWork.Complete();
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
