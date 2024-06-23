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
        private readonly IDepartmentRepository _repository;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(IDepartmentRepository repository, IWebHostEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        public IActionResult Index(string Message)
        {
            var department = _repository.GetAll();
            ViewData["Message"] = Message;
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
                var count = _repository.Add(department);
                if (count > 0)
                {
                    TempData["Message"] = "New Department is Created";
                    return RedirectToAction(nameof(Index),new { Message = "alert-success" });
                }
            }
            return View(department);
        }

        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var department = _repository.Get(id.Value);
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
                var count = _repository.Update(department);
                if (count > 0)
                {
                    TempData["Message"] = "One Department is Updated";
                    return RedirectToAction(nameof(Index), new { Message = "alert-info" });
                }
            }
            catch (Exception ex)
            {
                // 1. Log Exception 
                // 2. Friendly Message 

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred During Updating the Department");

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
                var count = _repository.Delete(department);
                if (count > 0)
                {
                    TempData["Message"] = "One Department is Deleted";
                    return RedirectToAction(nameof(Index), new { Message = "alert-danger" });
                }
            }
            catch (Exception ex)
            {
                // 1. Log Exception 
                // 2. Friendly Message 

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred During Updating the Department");

            }
            return View(department);
        }
    }
}
