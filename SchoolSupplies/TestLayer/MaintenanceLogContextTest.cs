using BusinessLayer;
using DataLayer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLayer
{
    [TestFixture]
    public class MaintenanceLogContextTests
    {
        private MaintenanceLogContext logContext;

        [SetUp]
        public async Task Setup()
        {
            logContext = new MaintenanceLogContext(TestManager.DbContext);

            // Изчистваме всички таблици преди всеки тест
            TestManager.DbContext.MaintenanceLogs.RemoveRange(TestManager.DbContext.MaintenanceLogs);
            TestManager.DbContext.Hardwares.RemoveRange(TestManager.DbContext.Hardwares);
            TestManager.DbContext.Softwares.RemoveRange(TestManager.DbContext.Softwares);
            TestManager.DbContext.Users.RemoveRange(TestManager.DbContext.Users);
            TestManager.DbContext.Rooms.RemoveRange(TestManager.DbContext.Rooms);
            TestManager.DbContext.Types.RemoveRange(TestManager.DbContext.Types);
            TestManager.DbContext.Licenses.RemoveRange(TestManager.DbContext.Licenses);

            await TestManager.DbContext.SaveChangesAsync();
        }

        [Test]
        public async Task Create()
        {
            var user = new User { Name = "Test User" };
            TestManager.DbContext.Users.Add(user);
            await TestManager.DbContext.SaveChangesAsync();

            var log = new MaintenanceLog
            {
                Description = "Routine check",
                Date = DateTime.UtcNow,
                User = user
            };

            int before = TestManager.DbContext.MaintenanceLogs.Count();

            await logContext.Create(log);

            int after = TestManager.DbContext.MaintenanceLogs.Count();
            MaintenanceLog last = TestManager.DbContext.MaintenanceLogs.Last();

            Assert.That(before + 1 == after && last.Id == log.Id,
                "Id are not equal or the MaintenanceLog is not created!");
        }

        [Test]
        public async Task Read()
        {
            var user = new User { Name = "Test User" };
            TestManager.DbContext.Users.Add(user);
            await TestManager.DbContext.SaveChangesAsync();

            var log = new MaintenanceLog
            {
                Description = "Fix issue",
                Date = DateTime.UtcNow,
                User = user
            };

            await logContext.Create(log);

            var result = await logContext.Read(log.Id);

            Assert.That(result.Description == "Fix issue", "Read() does not get MaintenanceLog by id!");
        }

        [Test]
        public async Task ReadAll()
        {
            var user = new User { Name = "User1" };
            TestManager.DbContext.Users.Add(user);
            await TestManager.DbContext.SaveChangesAsync();

            var log1 = new MaintenanceLog { Description = "Log1", Date = DateTime.UtcNow, User = user };
            var log2 = new MaintenanceLog { Description = "Log2", Date = DateTime.UtcNow, User = user };

            await logContext.Create(log1);
            await logContext.Create(log2);

            int countBefore = 2;
            int countAfter = (await logContext.ReadAll()).Count;

            Assert.That(countBefore == countAfter, "ReadAll() does not return all MaintenanceLogs!");
        }

        [Test]
        public async Task Update()
        {
            var user = new User { Name = "User1" };
            TestManager.DbContext.Users.Add(user);
            await TestManager.DbContext.SaveChangesAsync();

            var log = new MaintenanceLog { Description = "Old description", Date = DateTime.UtcNow, User = user };
            await logContext.Create(log);

            log.Description = "Updated description";

            await logContext.Update(log);

            var updated = await logContext.Read(log.Id);

            Assert.That(updated.Description == "Updated description",
                "Update() does not change the MaintenanceLog's description!");
        }

        [Test]
        public async Task Delete()
        {
            var user = new User { Name = "User1" };
            TestManager.DbContext.Users.Add(user);
            await TestManager.DbContext.SaveChangesAsync();

            var log = new MaintenanceLog { Description = "To delete", Date = DateTime.UtcNow, User = user };
            await logContext.Create(log);

            int before = (await logContext.ReadAll()).Count;

            await logContext.Delete(log.Id);

            int after = (await logContext.ReadAll()).Count;

            Assert.That(before == after + 1, "Delete() does not delete the MaintenanceLog!");
        }

        [Test]
        public void MaintenanceLogValidation()
        {
            var log = new MaintenanceLog
            {
                Description = "", // празно описание
                Date = DateTime.UtcNow,
                User = new User { Name = "User1" }
            };

            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(log);

            bool isValid = Validator.TryValidateObject(log, context, validationResults, true);

            Assert.That(!isValid, "Validation should fail when Description is empty!");
        }
    }
}
