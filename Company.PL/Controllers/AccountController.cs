using Company.DAL.Models;
using Company.PL.ViewModels.Account;
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
				var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is null)
                {
					user = await _userManager.FindByNameAsync(model.Username);
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
						ModelState.AddModelError(string.Empty, "This Username is already in Use for Another Account");
				}
				else
					ModelState.AddModelError(string.Empty, "This Account is already Exist");
			}
            return View(model);
		}
		#endregion

		#region SignIn
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{
            if (ModelState.IsValid) 
            { 
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if(flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.IsLockedOut)
							ModelState.AddModelError(string.Empty, "Your Account is locked!!");
						if (result.Succeeded)
                            return RedirectToAction(nameof(HomeController.Index), "Home");
						if (result.IsNotAllowed)
							ModelState.AddModelError(string.Empty, "Your Account is Not Confirmed Yet!!");
					}
                }
                ModelState.AddModelError(string.Empty, "Invalid Login");
            }
			return View();
		}
		#endregion

		#region SignOut
		public async new Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(SignIn));
		}
        #endregion

        #region Forget Password 
		public IActionResult ForgetPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid) 
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{

				}
				ModelState.AddModelError(string.Empty, "There is No Account with this Email!");
			}
			return View(model);
		}
		#endregion
	}
}
