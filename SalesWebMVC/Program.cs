using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMVC.Data;
using System.Configuration;
namespace SalesWebMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetConnectionString("SalesWebMVCContext");
            builder.Services.AddDbContext<SalesWebMVCContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), builder =>
                    builder.MigrationsAssembly("SalesWebMVC")));

            builder.Services.AddScoped<SeedingService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<SalesWebMVCContext>();
            context.GetService<SeedingService>().Seed();

            app.Run();
        }
    }
}
