using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels.Account
{
	public class SignUpViewModel
	{

		[Required(ErrorMessage = "First Name is Required")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last Name is Required")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }
		
		[Required(ErrorMessage = "Email is Required")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

		[Required(ErrorMessage = "Password is Required")]
		[MinLength(6, ErrorMessage = "Minimum Password Lenght is 6 Digits")]
		[DataType(DataType.Password)]
        public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is Required")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Confirm Password Doesn't match with Password")]
		[Display (Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
		public bool IsAgree { get; set; }
    }
}
