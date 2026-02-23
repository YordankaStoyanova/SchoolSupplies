using BusinessLayer;
using BusinessLayer.Enum;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Type = BusinessLayer.Type;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var identityOptions = new IdentityOptions
            {
                Password =
                {
                    RequireDigit = false,
                    RequireLowercase = false,
                    RequireUppercase = false,
                    RequireNonAlphanumeric = false,
                    RequiredLength = 5
                }
            };

            var options = new DbContextOptionsBuilder<SchoolSuppliesDbContext>()
                .UseSqlServer(
                    "Server=DESKTOP-VICJK85\\SQLEXPRESS;Database=SchoolSupplies;Trusted_Connection=True;TrustServerCertificate=True;"
                )
                .Options;

            using var db = new SchoolSuppliesDbContext(options);

            db.Database.Migrate();

            var userStore = new UserStore<User>(db);
            var roleStore = new RoleStore<IdentityRole>(db);

            var userManager = new UserManager<User>(
                userStore,
                Options.Create(identityOptions),
                new PasswordHasher<User>(),
                new[] { new UserValidator<User>() },
                new[] { new PasswordValidator<User>() },
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                null,
                new Logger<UserManager<User>>(new LoggerFactory())
            );

            var roleManager = new RoleManager<IdentityRole>(
                roleStore,
                new[] { new RoleValidator<IdentityRole>() },
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                new Logger<RoleManager<IdentityRole>>(new LoggerFactory())
            );

            //await SeedRoles(roleManager);
            //await SeedUsers(userManager);
            await SeedDatabase(db);

            Console.WriteLine(" FULL DATABASE SEEDED SUCCESSFULLY");
        }
        catch (Exception ex)
        {
            Console.WriteLine(" ERROR:");
            Console.WriteLine(ex.Message);
        }
    }

   
    //static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    //{
    //    string[] roles = { "Administrator", "User" };

    //    foreach (var role in roles)
    //        if (!await roleManager.RoleExistsAsync(role))
    //            await roleManager.CreateAsync(new IdentityRole(role));
    //}

    
    //static async Task SeedUsers(UserManager<User> userManager)
    //{
    //    if (!userManager.Users.Any())
    //    {
    //        var admin = new User
    //        {
    //            UserName = "admin@admin.com",
    //            Email = "admin@admin.com",
    //            PhoneNumber = "0888000000",
    //            Name = "Administrator",
    //            EmailConfirmed = true
    //        };

    //        await userManager.CreateAsync(admin, "Admin1");
    //        await userManager.AddToRoleAsync(admin, "Administrator");

    //    }
    //}


    static async Task SeedDatabase(SchoolSuppliesDbContext db)
    {
        if (db.Types.Any())
            return;

 
        var laptop = new Type("Laptop");
        var pc = new Type("PC");
        var printer = new Type("Printer");
        var softwareType = new Type("Software");

        db.Types.AddRange(laptop, pc, printer, softwareType);

      
        var room1 = new Room("Room 101", 1);
        var room2 = new Room("Room 202", 2);
        var room3 = new Room("Server Room", 0);

        db.Rooms.AddRange(room1, room2, room3);

        var officeLicense = new License("Microsoft Office", DateTime.UtcNow.AddYears(1), 100)
        {
         
        };

        var adobeLicense = new License("Adobe Photoshop", DateTime.UtcNow.AddMonths(6), 30)
        {
           
        };

        db.Licenses.AddRange(officeLicense, adobeLicense);
        await db.SaveChangesAsync();

    
        var office = new Software
        {
            Name = "Office2024",
            SerialNumber = "OFFICE12345",
            Type = softwareType,
            License = officeLicense
        };

        var photoshop = new Software
        {
            Name = "Photoshop2024",
            SerialNumber = "PS54321",
            Type = softwareType,
            License = adobeLicense
        };

        db.Softwares.AddRange(office, photoshop);

        var dell = new Hardware
        {
            Name = "Dell Latitude",
            InventoryNumber = "INV-001",
            SerialNumber = "DL12345",
            Type = laptop,
            Room = room1,
            Status = ItemStatus.Working,
            Softwares = new List<Software> { office }
        };

        var hp = new Hardware
        {
            Name = "HP EliteDesk",
            InventoryNumber = "INV-002",
            SerialNumber = "HP54321",
            Type = pc,
            Room = room2,
            Status = ItemStatus.Working,
            Softwares = new List<Software> { office, photoshop }
        };

        var canon = new Hardware
        {
            Name = "Canon Printer",
            InventoryNumber = "INV-003",
            SerialNumber = "CN77777",
            Type = printer,
            Room = room3,
            Status = ItemStatus.Repair
        };

        db.Hardwares.AddRange(dell, hp, canon);

   
        var log1 = new MaintenanceLog("Почистване на лаптоп", DateTime.UtcNow)
        {
            Hardware = dell
        };

        var log2 = new MaintenanceLog("Смяна на тонер", DateTime.UtcNow)
        {
            Hardware = canon
        };

        db.MaintenanceLogs.AddRange(log1, log2);

        await db.SaveChangesAsync();
    }
}
        
    
