using AutoMapper;
using Company.DAL.Models;
using Company.PL.ViewModels.User;

namespace Company.PL.MapperProfiles
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
		}
	}
}
