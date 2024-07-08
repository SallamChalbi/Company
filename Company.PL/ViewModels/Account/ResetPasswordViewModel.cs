using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels.Account
{
    public class ResetPasswordViewModel
    {

        [Required(ErrorMessage = "New Password is Required")]
        [MinLength(6, ErrorMessage = "Minimum Password Lenght is 6 Digits")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password Doesn't match with New Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
