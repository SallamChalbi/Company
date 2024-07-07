using Company.DAL.Models;
using Company.PL.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;          
			_signInManager = signInManager;      
		}                                        
        #region SignUp                           
        public IActionResult SignUp()            
        {                                        
            return View();                       
        }                                        
                                                 
        [HttpPost]                               
		public async Task<IActionResult> SignUp(SignUpViewModel model)
		{
            if (ModelState.IsValid) 
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user is null)
                {
				    user = new ApplicationUser()
                    {
                        FName = model.FirstName,
                        LName = model.LastName,
                        UserName = model.Username,
                        Email = model.Email,
                        IsAgree = model.IsAgree
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(SignIn));

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
				    ModelState.AddModelError(string.Empty, "Ths Username is already in Use for Another Account");
			}
            return View(model);
		}
		#endregion

		#region SignIn
        public IActionResult SignIn()
        {
            return View();
        }
		#endregion
	}
}
