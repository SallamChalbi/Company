using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Data;
using Company.DAL.Models;
using Company.PL.MapperProfiles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Company.PL.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            ////services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            //services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            ////services.AddSingleton<IDepartmentRepository, DepartmentRepository>();
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            
            services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));

            //services.AddScoped<UserManager<ApplicationUser>>();
            //services.AddScoped<SignInManager<ApplicationUser>>();
            //services.AddScoped<RoleManager<IdentityRole>>();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 2;
                //options.Password.RequireDigit = true;
                //options.Password.RequireNonAlphanumeric = true;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireLowercase = true;
                //options.Password.RequiredLength = 6;

                //options.Lockout.AllowedForNewUsers = true;
                //options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(5);

                //options.User.AllowedUserNameCharacters = "qwerty234$78";
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            return services;
        }
    }
}
