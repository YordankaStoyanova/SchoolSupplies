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
    public class SoftwareContextTests
    {
        private SoftwareContext softwareContext;

        [SetUp]
        public void Setup()
        {
            softwareContext = new SoftwareContext(TestManager.DbContext);

            TestManager.DbContext.Softwares.RemoveRange(TestManager.DbContext.Softwares);
            TestManager.DbContext.Hardwares.RemoveRange(TestManager.DbContext.Hardwares);
            TestManager.DbContext.MaintenanceLogs.RemoveRange(TestManager.DbContext.MaintenanceLogs);
            TestManager.DbContext.Licenses.RemoveRange(TestManager.DbContext.Licenses);
            TestManager.DbContext.Types.RemoveRange(TestManager.DbContext.Types);
            TestManager.DbContext.Rooms.RemoveRange(TestManager.DbContext.Rooms);
            TestManager.DbContext.Users.RemoveRange(TestManager.DbContext.Users);

            TestManager.DbContext.SaveChanges();
        }

        [Test]
        public async Task Create_Software_AddsToDatabase()
        {
            var software = new Software { Name = "MS Office" };

            int before = TestManager.DbContext.Softwares.Count();

            await softwareContext.Create(software);

            int after = TestManager.DbContext.Softwares.Count();

            Assert.That(after == before + 1);
        }

        [Test]
        public async Task Read_ReturnsSoftware()
        {
            var software = new Software { Name = "Photoshop" };
            await softwareContext.Create(software);

            var result = await softwareContext.Read(software.Id);

            Assert.That(result.Name == "Photoshop");
        }

        [Test]
        public async Task Update_ChangesSoftwareName()
        {
            var software = new Software { Name = "Old" };
            await softwareContext.Create(software);

            software.Name = "Updated";

            await softwareContext.Update(software);

            var updated = await softwareContext.Read(software.Id);

            Assert.That(updated.Name == "Updated");
        }

        [Test]
        public async Task Delete_RemovesSoftware()
        {
            var software = new Software { Name = "DeleteMe" };
            await softwareContext.Create(software);

            int before = (await softwareContext.ReadAll()).Count;

            await softwareContext.Delete(software.Id);

            int after = (await softwareContext.ReadAll()).Count;

            Assert.That(after == before - 1);
        }
    }
}
