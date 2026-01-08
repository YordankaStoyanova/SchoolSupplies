using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<SchoolSuppliesDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            

            builder.Services.AddIdentity<User, IdentityRole>()
               .AddEntityFrameworkStores<SchoolSuppliesDbContext>()
               .AddDefaultTokenProviders();
            // Добавяне на твоя Data Layer (CRUD контексти)
            builder.Services.AddScoped<SoftwareContext>();
           builder.Services.AddScoped<HardwareContext>();
            builder.Services.AddScoped<TypeContext>();
            builder.Services.AddScoped<LicenseContext>();
            builder.Services.AddScoped<RoomContext>();
            builder.Services.AddScoped<MaintenanceLogContext>();
            builder.Services.AddScoped<IdentityContext>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();



        }
    }
}
