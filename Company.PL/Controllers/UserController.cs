using AutoMapper;
using Company.DAL.Models;
using Company.PL.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
			if (string.IsNullOrEmpty(searchInput))
			{
				var users = await _userManager.Users.Select(U => new UserViewModel()
				{
					Id = U.Id,
					FName = U.FName,
					LName = U.LName,
					Email = U.Email,
					PhoneNumber = U.PhoneNumber,
					Username = U.UserName,
					Roles = _userManager.GetRolesAsync(U).Result

				}).ToListAsync();
				return View(users);
            }
			else
			{
                var users = await _userManager.Users
                    .Where(u => u.NormalizedEmail.Trim().Contains(searchInput.Trim().ToUpper())
                    || u.UserName.Trim().ToLower().Contains(searchInput.ToLower())).ToListAsync();

                var mappedUser = new List<UserViewModel>();
				foreach (var user in users)
				{
					mappedUser.Add(new UserViewModel()
					{
						Id= user.Id,
						FName= user.FName,
						LName= user.LName,
						Username= user.UserName,
						Email= user.Email,
						PhoneNumber= user.PhoneNumber,
						Roles = _userManager.GetRolesAsync(user).Result
					});
				}
                return View(mappedUser);
            }
		}
	}
}
