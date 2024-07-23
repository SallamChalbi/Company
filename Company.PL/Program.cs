using Company.DAL.Data;
using Company.PL.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder();

			#region Services 
			webApplicationBuilder.Services.AddControllersWithViews();
			webApplicationBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			});

			webApplicationBuilder.Services.AddApplicationServices(webApplicationBuilder);
			#endregion

			var app = webApplicationBuilder.Build();

			#region Pipeline 
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			#endregion

			app.Run();
		}


	}
}
