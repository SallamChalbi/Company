using AutoMapper;
using Company.DAL.Models;
using Company.PL.ViewModels.Employee;
using Company.PL.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMapper _mapper;

		public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
			_userManager = userManager;
			_mapper = mapper;
		}
        public async Task<IActionResult> Index(string searchInput)
		{
			var users = new List<UserViewModel>();

            if (string.IsNullOrEmpty(searchInput))
			{
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
    }
}
