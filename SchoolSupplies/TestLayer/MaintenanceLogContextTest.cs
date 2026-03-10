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
        private SchoolSuppliesDbContext dbContext;
        private MaintenanceLogContext logContext;

        [SetUp]
        public void Setup()
        {
            dbContext = TestManager.GetDbContext(Guid.NewGuid().ToString());
            logContext = new MaintenanceLogContext(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task Create_AddsMaintenanceLog()
        {
            var log = TestData.CreateMaintenanceLog("Initial log");

            await logContext.Create(log);

            Assert.That(dbContext.MaintenanceLogs.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Read_ReturnsMaintenanceLog()
        {
            var log = TestData.CreateMaintenanceLog("Read log");

            await logContext.Create(log);

            var result = await logContext.Read(log.Id);

            Assert.That(result.Description, Is.EqualTo("Read log"));
        }

        [Test]
        public async Task Update_ChangesDescription()
        {
            var log = TestData.CreateMaintenanceLog("Old description");

            await logContext.Create(log);

            log.Description = "New description";
            await logContext.Update(log);

            var updated = await logContext.Read(log.Id);

            Assert.That(updated.Description, Is.EqualTo("New description"));
        }

        [Test]
        public async Task Delete_RemovesMaintenanceLog()
        {
            var log = TestData.CreateMaintenanceLog("Delete log");

            await logContext.Create(log);

            int before = (await logContext.ReadAll()).Count;
            await logContext.Delete(log.Id);
            int after = (await logContext.ReadAll()).Count;

            Assert.That(after, Is.EqualTo(before - 1));
        }
    }
}

