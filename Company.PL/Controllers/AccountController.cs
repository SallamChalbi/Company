using Company.DAL.Models;
using Company.PL.Services.EmailSender;
using Company.PL.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly IEmailSender _emailSender;
		private readonly IConfiguration _configuration;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private static int _counter = 0;

		public AccountController(
			IEmailSender emailSender,
			IConfiguration configuration,
			UserManager<ApplicationUser> userManager, 
			SignInManager<ApplicationUser> signInManager)
        {
			_emailSender = emailSender;
			_configuration = configuration;
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
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> Login(SignInViewModel model)
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
						//if (result.IsNotAllowed)
						//	ModelState.AddModelError(string.Empty, "Your Account is Not Confirmed Yet!!");
						if (result.Succeeded)
                            return RedirectToAction(nameof(HomeController.Index), "Home");
					}
                }
                ModelState.AddModelError(string.Empty, "Invalid Email or Password!");
            }
			return View(model);
		}
		#endregion

		#region SignOut
		public async new Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}
        #endregion

        #region Forget Password 
		public IActionResult ForgetPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPasswordEmail(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid) 
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
					var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, token = resetPasswordToken }, Request.Scheme);

					await _emailSender.SendAsync(
						from: _configuration["EmailSettings:SenderEmail"],
						recipients: model.Email,
						subject: "Reset Your Password",
						body: resetPasswordUrl);

					TempData["senderEmail"] = model.Email;
					TempData["senderToken"] = resetPasswordToken;
					return RedirectToAction(nameof(CheckYourInbox));
				}
				ModelState.AddModelError(string.Empty, "There is No Account with this Email!");
			}
			return View(model);
		}

		public IActionResult CheckYourInbox()
		{
			TempData["senderEmail"] = TempData["senderEmail"];
			TempData["senderToken"] = TempData["senderToken"];
			return View();
		}
		#endregion

		#region Reset Password 
		public IActionResult ResetPassword(string email, string token)
		{
			TempData["Email"] = email;
			TempData["Token"] = token;

			if (!TempData["senderEmail"].Equals(TempData["Email"]) || !TempData["senderToken"].Equals(TempData["Token"]))
				return RedirectToAction(nameof(InvaledEmailOrToken));

			TempData["senderEmail"] = TempData["senderEmail"];
			TempData["senderToken"] = TempData["senderToken"];
			TempData["Email"] = email;
			TempData["Token"] = token;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var email = TempData["Email"] as string;
				var token = TempData["Token"] as string;

				var user = await _userManager.FindByEmailAsync(email);
				if (user is not null)
				{
					var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
					if (result.Succeeded)
						return RedirectToAction(nameof(SignIn));

					foreach (var error in result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);
				}
				else
					ModelState.AddModelError(string.Empty, "User is Not Valid");

				if (++_counter >= 3)
				{
					_counter = 0;
					return RedirectToAction(nameof(MoreThanThreeAttemps));
				}

				TempData["email"] = email;
				TempData["token"] = token;
			}
			return View(model);
		}

		public IActionResult InvaledEmailOrToken()
		{
			return View();
		}
		public IActionResult MoreThanThreeAttemps()
		{
			return View();
		}
		#endregion

		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}
