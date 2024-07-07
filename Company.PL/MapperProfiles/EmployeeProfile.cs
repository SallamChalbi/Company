﻿using AutoMapper;
using Company.DAL.Models;
using Company.PL.ViewModels.Employee;

namespace Company.PL.MapperProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }
    }
}
