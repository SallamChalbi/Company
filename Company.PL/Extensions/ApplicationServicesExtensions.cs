using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Data;
using Company.DAL.Models;
using Company.PL.Helpers;
using Company.PL.MapperProfiles;
using Company.PL.Services.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Company.PL.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IDocumentSettings, DocumentSettings>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            ////services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            //services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            ////services.AddSingleton<IDepartmentRepository, DepartmentRepository>();
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            
            services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            services.AddAutoMapper(M => M.AddProfile(new UserProfile()));
            services.AddAutoMapper(M => M.AddProfile(new RoleProfile()));

            //services.AddScoped<UserManager<ApplicationUser>>();
            //services.AddScoped<SignInManager<ApplicationUser>>();
            //services.AddScoped<RoleManager<IdentityRole>>();
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredUniqueChars = 2;
                //options.Password.RequireDigit = true;
                //options.Password.RequireNonAlphanumeric = true;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireLowercase = true;
                //options.Password.RequiredLength = 6;

                //options.Lockout.AllowedForNewUsers = true;
                //options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(3);

                //options.User.AllowedUserNameCharacters = "qwerty234$78";
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders(); // Generate default token for resetPasswordToken in ResetPasswordEmail in AccountController 

			services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/SignIn";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.AccessDeniedPath = "/Home/Error";
            });

            services.AddAuthentication().AddCookie("MyScheme", options =>
            {
				options.LoginPath = "/Account/SignIn";
				options.ExpireTimeSpan = TimeSpan.FromDays(1);
				options.AccessDeniedPath = "/Home/Error";
			});
            return services;
        }
    }
}
