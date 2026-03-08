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
            //await SeedAdmin(userManager);
            //  await SeedUser(userManager);
            // await SeedDatabase(db);
            await SeedTypesAsync(db);

            // 2) Seed Rooms
            await SeedRoomsAsync(db);

            // 3) Seed Licenses (Вариант A: 1 License -> много Softwares)
            await SeedLicensesAsync(db);

            // 4) Seed Softwares + връзки към License/Type
            await SeedSoftwaresAsync(db);

            // 5) Seed Hardwares + връзки към Room/Type + инсталиран софтуер (many-to-many)
            await SeedHardwaresAsync(db);

            // 6) Seed Maintenance logs
            await SeedMaintenanceLogsAsync(db);
            Console.WriteLine(" FULL DATABASE SEEDED SUCCESSFULLY");
        }
        catch (Exception ex)
        {
            Console.WriteLine(" ERROR:");
            Console.WriteLine(ex.Message);
        }
    }


    static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "Administrator", "User" };

        foreach (var role in roles)
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
    }


    static async Task SeedAdmin(UserManager<User> userManager)
    {
        if (!userManager.Users.Any())
        {
            var admin = new User
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                PhoneNumber = "0888000000",
                Name = "Administrator",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, "Admin1");
            await userManager.AddToRoleAsync(admin, "Administrator");

        }
    }
    static async Task SeedUser(UserManager<User> userManager)
    {
         var user = new User
            {
                UserName = "user@user.com",
                Email = "user@user.com",
                PhoneNumber = "0888000000",
                Name = "User",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(user, "User1");
            await userManager.AddToRoleAsync(user, "User");

        
    }
 private static async Task SeedTypesAsync(SchoolSuppliesDbContext db)
    {
        if (await db.Types.AnyAsync()) return;

        var names = new[]
        {
                "Лаптоп",
                "Компютър",
                "Мултимедия",
                "Принтер",
                "Антивирусна",
                "Офис пакет",
                "Графичен софтуер",
                "Операционна система"
            };

        db.Types.AddRange(names.Select(n => new BusinessLayer.Type { Name = n }));
        await db.SaveChangesAsync();
    }

    private static async Task SeedRoomsAsync(SchoolSuppliesDbContext db)
    {
        if (await db.Rooms.AnyAsync()) return;

        db.Rooms.AddRange(new[]
        {
                new Room { Name = "КАБИНЕТ 101" },
                new Room { Name = "КАБИНЕТ 102" },
                new Room { Name = "КАБИНЕТ 103" },
                new Room { Name = "ЛАБОРАТОРИЯ 201" },
                new Room { Name = "ДИРЕКТОРСКИ КАБИНЕТ" }
            });

        await db.SaveChangesAsync();
    }

    private static async Task SeedLicensesAsync(SchoolSuppliesDbContext db)
    {
        if (await db.Licenses.AnyAsync()) return;

        db.Licenses.AddRange(new[]
        {
                new License { Name = "Windows 11 Education", ExpirationDate = DateTime.UtcNow.AddYears(2), MaxUsage = 40 },
                new License { Name = "Microsoft 365 A3", ExpirationDate = DateTime.UtcNow.AddYears(1), MaxUsage = 200 },
                new License { Name = "ESET Endpoint", ExpirationDate = DateTime.UtcNow.AddMonths(10), MaxUsage = 120 },
                new License { Name = "Adobe CC (Education)", ExpirationDate = DateTime.UtcNow.AddMonths(6), MaxUsage = 25 }
            });

        await db.SaveChangesAsync();
    }

    private static async Task SeedSoftwaresAsync(SchoolSuppliesDbContext db)
    {
        if (await db.Softwares.AnyAsync()) return;

        var tOS = await db.Types.FirstAsync(t => t.Name == "Операционна система");
        var tOffice = await db.Types.FirstAsync(t => t.Name == "Офис пакет");
        var tAV = await db.Types.FirstAsync(t => t.Name == "Антивирусна");
        var tGraphics = await db.Types.FirstAsync(t => t.Name == "Графичен софтуер");

        var licWindows = await db.Licenses.FirstAsync(l => l.Name.Contains("Windows"));
        var licM365 = await db.Licenses.FirstAsync(l => l.Name.Contains("365"));
        var licEset = await db.Licenses.FirstAsync(l => l.Name.Contains("ESET"));
        var licAdobe = await db.Licenses.FirstAsync(l => l.Name.Contains("Adobe"));

        var softwares = new List<Software>
            {
                new Software
                {
                    Name = "Windows 11",
                    SerialNumber = "WIN11EDU12345",
                    Type = tOS,
                    LicenseId = licWindows.Id,
                    License = licWindows
                },
                new Software
                {
                    Name = "Microsoft 365",
                    SerialNumber = "M365A3XYZ999",
                    Type = tOffice,
                    LicenseId = licM365.Id,
                    License = licM365
                },
                new Software
                {
                    Name = "ESET Endpoint Antivirus",
                    SerialNumber = "ESETENDP77777",
                    Type = tAV,
                    LicenseId = licEset.Id,
                    License = licEset
                },
                new Software
                {
                    Name = "Adobe Photoshop",
                    SerialNumber = "ADOBEPS22222",
                    Type = tGraphics,
                    LicenseId = licAdobe.Id,
                    License = licAdobe
                },
            };

        db.Softwares.AddRange(softwares);
        await db.SaveChangesAsync();
    }

    private static async Task SeedHardwaresAsync(SchoolSuppliesDbContext db)
    {
        if (await db.Hardwares.AnyAsync()) return;

        var tLaptop = await db.Types.FirstAsync(t => t.Name == "Лаптоп");
        var tPC = await db.Types.FirstAsync(t => t.Name == "Компютър");
        var tPrinter = await db.Types.FirstAsync(t => t.Name == "Принтер");
        var tMultimedia = await db.Types.FirstAsync(t => t.Name == "Мултимедия");

        var r101 = await db.Rooms.FirstAsync(r => r.Name == "КАБИНЕТ 101");
        var r102 = await db.Rooms.FirstAsync(r => r.Name == "КАБИНЕТ 102");
        var r201 = await db.Rooms.FirstAsync(r => r.Name.Contains("201"));

        var win = await db.Softwares.FirstAsync(s => s.Name.Contains("Windows"));
        var m365 = await db.Softwares.FirstAsync(s => s.Name.Contains("365"));
        var eset = await db.Softwares.FirstAsync(s => s.Name.Contains("ESET"));
        var ps = await db.Softwares.FirstAsync(s => s.Name.Contains("Photoshop"));

        var hw = new List<Hardware>
            {
                new Hardware
                {
                    Name = "Lenovo ThinkPad L14",
                    InventoryNumber = "INV-LAP-0001",
                    SerialNumber = "SN-LEN-11111",
                    Type = tLaptop,
                    Room = r101,
                    Status = ItemStatus.Working,
                    Softwares = new List<Software> { win, m365, eset }
                },
                new Hardware
                {
                    Name = "Dell OptiPlex 7090",
                    InventoryNumber = "INV-PC-0001",
                    SerialNumber = "SN-DEL-22222",
                    Type = tPC,
                    Room = r101,
                    Status = ItemStatus.Working,
                    Softwares = new List<Software> { win, m365, eset, ps }
                },
                new Hardware
                {
                    Name = "HP ProDesk 400",
                    InventoryNumber = "INV-PC-0002",
                    SerialNumber = "SN-HP-33333",
                    Type = tPC,
                    Room = r102,
                    Status = ItemStatus.Repair,
                    Softwares = new List<Software> { win, eset }
                },
                new Hardware
                {
                    Name = "Epson EcoTank L3250",
                    InventoryNumber = "INV-PRN-0001",
                    SerialNumber = "SN-EPS-44444",
                    Type = tPrinter,
                    Room = r102,
                    Status = ItemStatus.Working,
                    Softwares = new List<Software>() // принтерът няма нужда
                },
                new Hardware
                {
                    Name = "BenQ Projector",
                    InventoryNumber = "INV-MUL-0001",
                    SerialNumber = "SN-BEN-55555",
                    Type = tMultimedia,
                    Room = r201,
                    Status = ItemStatus.Working,
                    Softwares = new List<Software>()
                },
            };

        db.Hardwares.AddRange(hw);
        await db.SaveChangesAsync();
    }

    private static async Task SeedMaintenanceLogsAsync(SchoolSuppliesDbContext db)
    {
        if (await db.MaintenanceLogs.AnyAsync()) return;

        var pc1 = await db.Hardwares.FirstAsync(h => h.InventoryNumber == "INV-PC-0001");
        var pc2 = await db.Hardwares.FirstAsync(h => h.InventoryNumber == "INV-PC-0002");
        var laptop1 = await db.Hardwares.FirstAsync(h => h.InventoryNumber == "INV-LAP-0001");

        var eset = await db.Softwares.FirstAsync(s => s.Name.Contains("ESET"));
        var m365 = await db.Softwares.FirstAsync(s => s.Name.Contains("365"));

        // ✅ Вариант A (препоръчано): MaintenanceLog има HardwareId? / SoftwareId? (nullable)
        // Ако още НЕ си го променил, виж коментара "Вариант B" по-долу.
        db.MaintenanceLogs.AddRange(new[]
        {
                new MaintenanceLog("Почистване на прах + проверка на вентилатори", DateTime.UtcNow.AddDays(-20))
                {
                    Hardware = pc1,
                    Software = null
                },
                new MaintenanceLog("Смяна на RAM модул", DateTime.UtcNow.AddDays(-10))
                {
                    Hardware = pc2,
                    Software = null
                },
                new MaintenanceLog("Преинсталация на Windows и драйвери", DateTime.UtcNow.AddDays(-5))
                {
                    Hardware = laptop1,
                    Software = null
                },
                new MaintenanceLog("Обновяване на дефиниции и пълно сканиране", DateTime.UtcNow.AddDays(-7))
                {
                    Hardware = null,
                    Software = eset
                },
                new MaintenanceLog("Рестарт на активация и обновяване на приложения", DateTime.UtcNow.AddDays(-3))
                {
                    Hardware = null,
                    Software = m365
                }
            });

        // ❗ Вариант B (ако при теб логът изисква И Hardware, И Software NOT NULL):
        // тогава трябва да зададеш и двете, например:
        //
        // db.MaintenanceLogs.Add(new MaintenanceLog("ESET сканиране на PC1", DateTime.UtcNow.AddDays(-2))
        // {
        //     Hardware = pc1,
        //     Software = eset
        // });
        //
        // Но това е по-нечист модел — по-добре премини към nullable FK-та.

        await db.SaveChangesAsync();
    }

}

        
    
