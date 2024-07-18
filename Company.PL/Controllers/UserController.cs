using AutoMapper;
using Company.DAL.Models;
using Company.PL.Authorizations;
using Company.PL.ViewModels.Employee;
using Company.PL.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    [Authorize(Roles = AppPermessions.Supervisor)]
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(
            UserManager<ApplicationUser> userManager, 
            IMapper mapper,
            ILogger<UserController> logger)
        {
			_userManager = userManager;
			_mapper = mapper;
            _logger = logger;
        }
        public async Task<IActionResult> Index(string AlertColor, string searchInput)
		{
			var users = new List<UserViewModel>();

            if (string.IsNullOrEmpty(searchInput))
			{
                ViewData["AlertColor"] = AlertColor;
                users = await _userManager.Users.Select(U => new UserViewModel()
				{
					Id = U.Id,
					FName = U.FName,
					LName = U.LName,
					Email = U.Email,
					PhoneNumber = U.PhoneNumber,
					Username = U.UserName,
					Roles = _userManager.GetRolesAsync(U).Result

				}).ToListAsync();
            }
			else
			{
				users = await _userManager.Users.Where(u => u.NormalizedEmail.Trim().Contains(searchInput.Trim().ToUpper())
                    || u.UserName.Trim().ToLower().Contains(searchInput.ToLower()))
					.Select (u => new UserViewModel()
                    {
                        Id = u.Id,
                        FName = u.FName,
                        LName = u.LName,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        Username = u.UserName,
                        Roles = _userManager.GetRolesAsync(u).Result

                    }).ToListAsync();
            }
            return View(users);
        }

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();

            var userVM = _mapper.Map<ApplicationUser, UserViewModel>(user);
            return View(viewName, userVM);
        }

        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel userVM)
        {
            if (id != userVM.Id)
                return BadRequest();
            if (!ModelState.IsValid)
                return View(userVM);

            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is null)
                    return NotFound();

                user.FName = userVM.FName;
                user.LName = userVM.LName;
                user.PhoneNumber = userVM.PhoneNumber;
                
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["Message"] = "One User is Updated";
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
            return View(userVM);
        }

		[Authorize(Roles = AppPermessions.Admin)]
		public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

		[Authorize(Roles = AppPermessions.Admin)]
		[HttpPost]
        public async Task<IActionResult> Delete(UserViewModel userVM)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userVM.Id);
                if (user is null)
                    return NotFound();

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["Message"] = "One User is Deleted";
                    return RedirectToAction(nameof(Index), new { AlertColor = "alert-danger" });
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
            return View(userVM);
        }
    }
}
