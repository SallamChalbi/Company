using AutoMapper;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Company.PL.Helpers;
using Company.PL.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
		private readonly IDocumentSettings _settings;

		//private readonly IEmployeeRepository _employeeRepository;
		private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(
            IDocumentSettings settings,
            IUnitOfWork unitOfWork,
            //IEmployeeRepository employeeRepository, 
            IMapper mapper
            /*, IDepartmentRepository departmentRepository*/)
        {
			_settings = settings;
			//_employeeRepository = employeeRepository;
			_unitOfWork = unitOfWork;
            _mapper = mapper;
            //_departmentRepository = departmentRepository;
        }

        public async Task<IActionResult> Index(string AlertColor, string searchInput)
        {
            var employees = Enumerable.Empty<Employee>();
            var employeeRepository = _unitOfWork.Repository<Employee>() as EmployeeRepository;
            if (string.IsNullOrEmpty(searchInput))
            {
                ViewData["AlertColor"] = AlertColor;
                employees = await employeeRepository.GetAllAsync();
            }
            else
                employees = employeeRepository.SearchByName(searchInput.ToLower());

            var employeeVM = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(employeeVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                if (employeeVM.Image is not null)
                    employeeVM.ImageName = await _settings.UploadFile(employeeVM.Image, "images");
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.Repository<Employee>().Add(mappedEmployee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {

                    TempData["Message"] = "New Employee is Created";
                    return RedirectToAction(nameof(Index), new { AlertColor = "alert-primary" });
                }
            }
            return View(employeeVM);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null )
                return BadRequest();

            var employee = await _unitOfWork.Repository<Employee>().GetAsync(id.Value);
            if (employee is null)
                return NotFound();

            if(viewName.Equals("Delete", StringComparison.OrdinalIgnoreCase) ||
                viewName.Equals("Edit", StringComparison.OrdinalIgnoreCase))
                TempData["ImageName"] = employee.ImageName;

            var employeeVM = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(viewName, employeeVM);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (!ModelState.IsValid)
                return View(employeeVM);

            try
            {
                var employeeDB = await _unitOfWork.Repository<Employee>().GetAsync(employeeVM.Id);
                if (employeeDB is null)
                    return NotFound();

                var ImgName = TempData["ImageName"] as string;
                if (ImgName is not null)
                {
                    if (employeeVM.Image is not null)
                        _settings.DeleteFile(ImgName, "images");
                    else
                        employeeVM.ImageName = ImgName;
                }
                if (employeeVM.Image is not null)
                    employeeVM.ImageName = await _settings.UploadFile(employeeVM.Image, "images");

                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.Repository<Employee>().Update(mappedEmployee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "One Employee is Updated";
                    return RedirectToAction(nameof(Index), new { AlertColor = "alert-success" });
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

        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM)
        {
            try
            {
                var employeeDB = await _unitOfWork.Repository<Employee>().GetAsync(employeeVM.Id);
                if (employeeDB is null)
                    return NotFound();

                employeeVM.ImageName = TempData["ImageName"] as string;
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.Repository<Employee>().Delete(mappedEmployee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    if(employeeVM.ImageName is not null)
                        _settings.DeleteFile(employeeVM.ImageName, "images");
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
            return View(employeeVM);
        }
    }
}
