using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MVC.Controllers;
using ServiceLayer;

namespace MVC;

// Microsoft.VisualStudio.Web.CodeGeneration.Design v. 5.0
// Microsoft.AspNetCore.Identity.UI 5.0.17
// If you want to use or scaffold them with Visual Studio:
// dotnet tool install -g dotnet-aspnet-codegenerator --version 5.0.0
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddControllersWithViews();
        services.AddScoped<IDb<Software, int>, SoftwareContext>();
        services.AddScoped<IDb<Hardware, int>, HardwareContext>();
        services.AddScoped<IDb<BusinessLayer.Type, int>, TypeContext>();
        services.AddScoped<IDb<License, int>, LicenseContext>();
        services.AddScoped<IDb<Room, int>, RoomContext>();
        services.AddScoped<IDb<MaintenanceLog, int>, MaintenanceLogContext>();
        services.AddScoped<HardwareService, HardwareService>();
        services.AddScoped<SoftwareService, SoftwareService>();
        services.AddScoped<LicenseService, LicenseService>();
        services.AddScoped<AdministrationService,AdministrationService>();
        services.AddScoped<MaintenanceLogService, MaintenanceLogService>();
        services.AddScoped<IEmailSender, MailKitEmailService>();
        services.AddHttpContextAccessor();

        services.AddDbContext<SchoolSuppliesDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("ServerDbConnection")));

        services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<SchoolSuppliesDbContext>()
            .AddDefaultTokenProviders();
        services.AddDistributedMemoryCache();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(60);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 5;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.AllowedForNewUsers = false;

            // User settings.
            options.User.RequireUniqueEmail = true;

            // Log in required.
            options.SignIn.RequireConfirmedAccount = false; 
            options.SignIn.RequireConfirmedEmail = false; 
        });

        services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

            options.LoginPath = "/Identity/Account/Login";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.SlidingExpiration = true;
        });

        services.Configure<MailKitEmailServiceOptions>(options =>
        {
            options.HostAddress = Configuration["ExternalProviders:MailKit:SMTP:Address"];
            options.HostPort = Convert.ToInt32(Configuration["ExternalProviders:MailKit:SMTP:Port"]);
            options.HostUsername = Configuration["ExternalProviders:MailKit:SMTP:Account"];
            options.HostPassword = Configuration["ExternalProviders:MailKit:SMTP:Password"];
            options.SenderEmail = Configuration["ExternalProviders:MailKit:SMTP:SenderEmail"];
            options.SenderName = Configuration["ExternalProviders:MailKit:SMTP:SenderName"];
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSession();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
    }
}
