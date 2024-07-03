using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Company.PL.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //services.AddSingleton<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            return services;
        }
    }
}
