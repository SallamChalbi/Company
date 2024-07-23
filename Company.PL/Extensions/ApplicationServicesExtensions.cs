using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Data;
using Company.DAL.Models;
using Company.PL.Helpers;
using Company.PL.MapperProfiles;
using Company.PL.Services.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Company.PL.Settings;

namespace Company.PL.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, WebApplicationBuilder builder)
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

            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.LoginPath = new PathString("/Account/SignIn");
            //    options.ExpireTimeSpan = TimeSpan.FromDays(1);
            //    options.AccessDeniedPath = new PathString("/Home/Error");
            //});
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            //}).AddGoogle(op =>
            //{
            //    op.ClientId = builder.Configuration.GetSection("Authentication:Google")["ClientId"];
            //    op.ClientSecret = builder.Configuration.GetSection("Authentication:Google")["ClientSecret"];
            //});

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

            services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSettings"));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = new PathString("/Account/Login");
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.AccessDeniedPath = new PathString("/Home/Error");
            });

            //// Add Authentication services
            //services.AddAuthentication(options =>
            //{
            //    //    // Specify the default authentication scheme
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            //})
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            //{
            //    options.LoginPath = "/Account/Login";
            //    options.AccessDeniedPath = "/Account/AccessDenied";
            //    options.ExpireTimeSpan = TimeSpan.FromDays(1);
            //})
            //.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            //{
            //    options.ClientId = builder.Configuration.GetSection("Authentication:Google")["ClientId"];
            //    options.ClientSecret = builder.Configuration.GetSection("Authentication:Google")["ClientSecret"];
            //});

            return services;
        }
    }
}
