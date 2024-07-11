﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels.User
{
	public class UserViewModel
	{
		public string Id { get; set; }
        [Required(ErrorMessage = "First Name is Required")]
        [Display(Name = "First Name")]
        public string FName { get; set; }
        [Required(ErrorMessage = "Last Name is Required")]
        [Display(Name = "Last Name")]
        public string LName { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }
		public IEnumerable<string> Roles { get; set; }
	}
}