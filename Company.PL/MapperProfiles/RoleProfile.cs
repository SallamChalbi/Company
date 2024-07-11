using AutoMapper;
using Company.PL.ViewModels.Role;
using Microsoft.AspNetCore.Identity;

namespace Company.PL.MapperProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleViewModel>();
        }
    }
}
