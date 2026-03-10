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
        private SchoolSuppliesDbContext dbContext;
        private HardwareContext hardwareContext;

        [SetUp]
        public void Setup()
        {
            dbContext = TestManager.GetDbContext(Guid.NewGuid().ToString());
            hardwareContext = new HardwareContext(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task Create_Hardware_AddsToDatabase()
        {
            var type = TestData.CreateType("Laptop");
            var room = TestData.CreateRoom("101", 1);

            dbContext.Types.Add(type);
            dbContext.Rooms.Add(room);
            await dbContext.SaveChangesAsync();

            var hardware = TestData.CreateHardware(type, room);

            int before = dbContext.Hardwares.Count();

            await hardwareContext.Create(hardware);

            int after = dbContext.Hardwares.Count();

            Assert.That(after, Is.EqualTo(before + 1));
        }

        [Test]
        public async Task Read_ReturnsHardware()
        {
            var type = TestData.CreateType("Printer");
            var room = TestData.CreateRoom("102", 1);

            dbContext.Types.Add(type);
            dbContext.Rooms.Add(room);
            await dbContext.SaveChangesAsync();

            var hardware = TestData.CreateHardware(type, room, "Printer HP");

            await hardwareContext.Create(hardware);

            var result = await hardwareContext.Read(hardware.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Printer HP"));
        }

        [Test]
        public async Task Update_ChangesHardwareName()
        {
            var type = TestData.CreateType("PC");
            var room = TestData.CreateRoom("103", 2);

            dbContext.Types.Add(type);
            dbContext.Rooms.Add(room);
            await dbContext.SaveChangesAsync();

            var hardware = TestData.CreateHardware(type, room, "Old PC");

            await hardwareContext.Create(hardware);

            var fromDb = await hardwareContext.Read(hardware.Id, true);
            fromDb.Name = "New PC";

            await hardwareContext.Update(fromDb);

            var updated = await hardwareContext.Read(hardware.Id);

            Assert.That(updated.Name, Is.EqualTo("New PC"));
        }

        [Test]
        public async Task Delete_RemovesHardware()
        {
            var type = TestData.CreateType("PC");
            var room = TestData.CreateRoom("104", 2);

            dbContext.Types.Add(type);
            dbContext.Rooms.Add(room);
            await dbContext.SaveChangesAsync();

            var hardware = TestData.CreateHardware(type, room);

            await hardwareContext.Create(hardware);

            int before = (await hardwareContext.ReadAll()).Count;

            await hardwareContext.Delete(hardware.Id);

            int after = (await hardwareContext.ReadAll()).Count;

            Assert.That(after, Is.EqualTo(before - 1));
        }

        [Test]
        public void HardwareValidation_Fails_WhenInventoryNumberMissing()
        {
            var hardware = new Hardware
            {
                Name = "PC",
                SerialNumber = "SERIAL-001"
            };

            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(hardware);

            bool isValid = Validator.TryValidateObject(hardware, context, validationResults, true);

            Assert.That(isValid, Is.False);
        }
    }
}

