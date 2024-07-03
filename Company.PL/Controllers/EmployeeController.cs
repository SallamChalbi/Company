using AutoMapper;
using Company.BLL.Interfaces;
using Company.DAL.Models;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;

namespace Company.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        //private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IMapper mapper/*, IDepartmentRepository departmentRepository*/)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            //_departmentRepository = departmentRepository;
        }

        public IActionResult Index(string Message)
        {
            ViewData["Message"] = Message;
            var employee = _employeeRepository.GetAll();
            var employeeVM = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employee);
            return View(employeeVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                var count = _employeeRepository.Add(mappedEmployee);
                if (count > 0)
                {
                    TempData["Message"] = "New Employee is Created";
                    return RedirectToAction(nameof(Index), new { Message = "alert-success" });
                }
            }
            return View(employeeVM);
        }

        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var employee = _employeeRepository.Get(id.Value);
            if (employee is null)
                return NotFound();

            var employeeVM = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(viewName, employeeVM);
        }

        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (!ModelState.IsValid)
                return View(employeeVM);

            try
            {
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                var count = _employeeRepository.Update(mappedEmployee);
                if (count > 0)
                {
                    TempData["Message"] = "One Employee is Updated";
                    return RedirectToAction(nameof(Index), new { Message = "alert-info" });
                }
            }
            catch (Exception ex)
            {
                // 1. Log Exception 
                // 2. Friendly Message 

                ModelState.AddModelError(string.Empty, ex.Message);

            }
            return View(employeeVM);
        }

        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        public IActionResult Delete(EmployeeViewModel employeeVM)
        {
            try
            {
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                var count = _employeeRepository.Delete(mappedEmployee);
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

                ModelState.AddModelError(string.Empty, ex.Message);
                
            }
            return View(employeeVM);
        }
    }
}
