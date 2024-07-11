using AutoMapper;
using Company.DAL.Models;
using Company.PL.ViewModels.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleController> _logger;

        public RoleController(
            RoleManager<ApplicationRole> roleManager, 
            IMapper mapper,
            ILogger<RoleController> logger)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string AlertColor, string searchInput)
        {
            var roles = new List<ApplicationRole>();
            if (string.IsNullOrEmpty(searchInput))
            {
                ViewData["AlertColor"] = AlertColor;
                roles = await _roleManager.Roles.ToListAsync();
            }
            else
                roles = await _roleManager.Roles.Where(r => r.NormalizedName.Trim().Contains(searchInput.Trim().ToUpper())).ToListAsync();
            
            var roleVM = _mapper.Map<List<ApplicationRole>, List<RoleViewModel>>(roles);
            return View(roleVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                var mappedRole = _mapper.Map<RoleViewModel, ApplicationRole>(role);
                var result = await _roleManager.CreateAsync(mappedRole);
                if (result.Succeeded) 
                    return RedirectToAction(nameof(Index));

                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(role);
        }

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();

            var roleVM = _mapper.Map<ApplicationRole, RoleViewModel>(role);
            return View(viewName, roleVM);
        }

        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel roleVM)
        {
            if (id != roleVM.Id)
                return BadRequest();
            if (!ModelState.IsValid)
                return View(roleVM);

            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null)
                    return NotFound();

                role.Name = roleVM.Name;
                role.NormalizedName =roleVM.Name.ToUpper();

                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    TempData["Message"] = "One Role is Updated";
                    return RedirectToAction(nameof(Index), new { AlertColor = "alert-success" });
                }

                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            catch (Exception ex)
            {
                // 1. Log Exception 
                // 2. Friendly Message 
                _logger.LogError(ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);

            }
            return View(roleVM);
        }

        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RoleViewModel roleVM)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(roleVM.Id);
                if (role is null)
                    return NotFound();

                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    TempData["Message"] = "One Role is Deleted";
                    return RedirectToAction(nameof(Index), new { AlertColor = "alert-success" });
                }

                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            catch (Exception ex)
            {
                // 1. Log Exception 
                // 2. Friendly Message 
                _logger.LogError(ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);

            }
            return View(roleVM);
        }
    }
}
