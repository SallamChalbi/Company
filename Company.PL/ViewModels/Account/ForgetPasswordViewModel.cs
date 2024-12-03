using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels.Account
{
	public class ForgetPasswordViewModel
	{
		[Required(ErrorMessage = "Email is Required")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }
	}
}
