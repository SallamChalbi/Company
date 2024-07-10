﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels.User
{
	public class UserViewModel
	{
		public string Id { get; set; }
		public string FName { get; set; }
		public string LName { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }
		public IEnumerable<string> Roles { get; set; }
	}
}
