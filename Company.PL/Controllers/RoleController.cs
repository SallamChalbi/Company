using AutoMapper;
using Company.DAL.Models;
using Company.PL.ViewModels.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<ApplicationRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
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
            
            var mappedRole = _mapper.Map<List<ApplicationRole>, List<RoleViewModel>>(roles);
            return View(mappedRole);
        }
    }
}
