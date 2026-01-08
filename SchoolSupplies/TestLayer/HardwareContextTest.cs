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
    public class HardwareContextTests
    {
        private HardwareContext hardwareContext;

        [SetUp]
        public void Setup()
        {
            hardwareContext = new HardwareContext(TestManager.DbContext);

            TestManager.DbContext.Hardwares.RemoveRange(TestManager.DbContext.Hardwares);
            TestManager.DbContext.Softwares.RemoveRange(TestManager.DbContext.Softwares);
            TestManager.DbContext.MaintenanceLogs.RemoveRange(TestManager.DbContext.MaintenanceLogs);
            TestManager.DbContext.Types.RemoveRange(TestManager.DbContext.Types);
            TestManager.DbContext.Rooms.RemoveRange(TestManager.DbContext.Rooms);
            TestManager.DbContext.Users.RemoveRange(TestManager.DbContext.Users);

            TestManager.DbContext.SaveChanges();
        }

        [Test]
        public async Task Create_Hardware_AddsToDatabase()
        {
            var hardware = new Hardware { Name = "Printer" };

            int before = TestManager.DbContext.Hardwares.Count();

            await hardwareContext.Create(hardware);

            int after = TestManager.DbContext.Hardwares.Count();

            Assert.That(after == before + 1);
        }

        [Test]
        public async Task Read_ReturnsHardware()
        {
            var hardware = new Hardware { Name = "Scanner" };
            await hardwareContext.Create(hardware);

            var result = await hardwareContext.Read(hardware.Id);

            Assert.That(result.Name == "Scanner");
        }

        [Test]
        public async Task Update_ChangesHardwareName()
        {
            var hardware = new Hardware { Name = "Old" };
            await hardwareContext.Create(hardware);

            hardware.Name = "Updated";

            await hardwareContext.Update(hardware);

            var updated = await hardwareContext.Read(hardware.Id);

            Assert.That(updated.Name == "Updated");
        }

        [Test]
        public async Task Delete_RemovesHardware()
        {
            var hardware = new Hardware { Name = "DeleteMe" };
            await hardwareContext.Create(hardware);

            int before = (await hardwareContext.ReadAll()).Count;

            await hardwareContext.Delete(hardware.Id);

            int after = (await hardwareContext.ReadAll()).Count;

            Assert.That(after == before - 1);
        }
    }
}
