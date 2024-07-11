using AutoMapper;
using Company.DAL.Models;
using Company.PL.ViewModels.Role;

namespace Company.PL.MapperProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<ApplicationRole, RoleViewModel>().ReverseMap();
        }
    }
}
