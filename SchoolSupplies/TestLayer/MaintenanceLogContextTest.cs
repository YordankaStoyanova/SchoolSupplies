using BusinessLayer;
using DataLayer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLayer
{
    [TestFixture]
    public class MaintenanceLogContextTests
    {
        private MaintenanceLogContext maintenanceLogContext;

        [SetUp]
        public void Setup()
        {
            maintenanceLogContext = new MaintenanceLogContext(TestManager.DbContext);

            TestManager.DbContext.MaintenanceLogs.RemoveRange(TestManager.DbContext.MaintenanceLogs);
            TestManager.DbContext.Softwares.RemoveRange(TestManager.DbContext.Softwares);
            TestManager.DbContext.Hardwares.RemoveRange(TestManager.DbContext.Hardwares);
            TestManager.DbContext.Users.RemoveRange(TestManager.DbContext.Users);

            TestManager.DbContext.SaveChanges();
        }

        [Test]
        public async Task Create_MaintenanceLog_AddsToDatabase()
        {
            var log = new MaintenanceLog { Description = "Test log" };

            int before = TestManager.DbContext.MaintenanceLogs.Count();

            await maintenanceLogContext.Create(log);

            int after = TestManager.DbContext.MaintenanceLogs.Count();

            Assert.That(after == before + 1);
        }

        [Test]
        public async Task Read_ReturnsMaintenanceLog()
        {
            var log = new MaintenanceLog { Description = "Read test" };
            await maintenanceLogContext.Create(log);

            var result = await maintenanceLogContext.Read(log.Id);

            Assert.That(result.Description == "Read test");
        }

        [Test]
        public async Task Update_ChangesDescription()
        {
            var log = new MaintenanceLog { Description = "Old" };
            await maintenanceLogContext.Create(log);

            log.Description = "Updated";

            await maintenanceLogContext.Update(log);

            var updated = await maintenanceLogContext.Read(log.Id);

            Assert.That(updated.Description == "Updated");
        }

        [Test]
        public async Task Delete_RemovesMaintenanceLog()
        {
            var log = new MaintenanceLog { Description = "Delete" };
            await maintenanceLogContext.Create(log);

            int before = (await maintenanceLogContext.ReadAll()).Count;

            await maintenanceLogContext.Delete(log.Id);

            int after = (await maintenanceLogContext.ReadAll()).Count;

            Assert.That(after == before - 1);
        }
    }
}
