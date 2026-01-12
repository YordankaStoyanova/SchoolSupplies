using BusinessLayer;
using BusinessLayer.Enum;
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
    public class HardwareContextTests
    {
        private static HardwareContext hardwareContext;

        static HardwareContextTests()
        {
            hardwareContext = new HardwareContext(TestManager.DbContext);
        }

        [SetUp]
        public async Task Setup()
        {
            // Изчистваме таблиците преди всеки тест
            TestManager.DbContext.Hardwares.RemoveRange(TestManager.DbContext.Hardwares);
            TestManager.DbContext.Softwares.RemoveRange(TestManager.DbContext.Softwares);
            TestManager.DbContext.MaintenanceLogs.RemoveRange(TestManager.DbContext.MaintenanceLogs);
            TestManager.DbContext.Types.RemoveRange(TestManager.DbContext.Types);
            TestManager.DbContext.Rooms.RemoveRange(TestManager.DbContext.Rooms);
            TestManager.DbContext.Users.RemoveRange(TestManager.DbContext.Users);

            await TestManager.DbContext.SaveChangesAsync();
        }

        [Test]
        public async Task Create()
        {
            var hardware = new Hardware
            {
                Name = "Printer",
                InventoryNumber = "INV123",
                SerialNumber = "SN12345",
                Status = ItemStatus.Working
            };

            int before = TestManager.DbContext.Hardwares.Count();

            await hardwareContext.Create(hardware);

            int after = TestManager.DbContext.Hardwares.Count();
            Hardware last = TestManager.DbContext.Hardwares.Last();

            Assert.That(after == before + 1 && last.Id == hardware.Id,
                "Hardware was not created correctly!");
        }

        [Test]
        public async Task Read()
        {
            var hardware = new Hardware
            {
                Name = "Scanner",
                InventoryNumber = "INV456",
                SerialNumber = "SN67890",
                Status = ItemStatus.Working
            };
            await hardwareContext.Create(hardware);

            var result = await hardwareContext.Read(hardware.Id);

            Assert.That(result.Name == "Scanner", "Read() did not return the correct Hardware!");
        }

        [Test]
        public async Task ReadAll()
        {
            var hw1 = new Hardware { Name = "HW1", InventoryNumber = "INV1", SerialNumber = "SN1", Status = ItemStatus.Working};
            var hw2 = new Hardware { Name = "HW2", InventoryNumber = "INV2", SerialNumber = "SN2", Status = ItemStatus.Working};

            await hardwareContext.Create(hw1);
            await hardwareContext.Create(hw2);

            var all = await hardwareContext.ReadAll();

            Assert.That(all.Count == 2, "ReadAll() did not return all hardware items!");
        }

        [Test]
        public async Task Updatе()
        {
            var hardware = new Hardware
            {
                Name = "OldName",
                InventoryNumber = "INV789",
                SerialNumber = "SN98765",
                Status = ItemStatus.Working
            };
            await hardwareContext.Create(hardware);

            hardware.Name = "NewName";

            await hardwareContext.Update(hardware);

            var updated = await hardwareContext.Read(hardware.Id);

            Assert.That(updated.Name == "NewName", "Update() did not change the Hardware's name!");
        }

        [Test]
        public async Task Delete()
        {
            var hardware = new Hardware
            {
                Name = "ToDelete",
                InventoryNumber = "INV999",
                SerialNumber = "SN99999",
                Status = ItemStatus.Working
            };
            await hardwareContext.Create(hardware);

            int before = (await hardwareContext.ReadAll()).Count;

            await hardwareContext.Delete(hardware.Id);

            int after = (await hardwareContext.ReadAll()).Count;

            Assert.That(after == before - 1, "Delete() did not remove the Hardware!");
        }

        [Test]
        public void HardwareValidation()
        {
            var hardware = new Hardware
            {
                Name = "A",
                InventoryNumber = "INV100",
                SerialNumber = "SN10000",
                Status = ItemStatus.Working
            };

            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(hardware);

            bool isValid = Validator.TryValidateObject(hardware, context, validationResults, true);

            Assert.That(!isValid, "Validation should fail for too short Name!");
        }
    }
}
