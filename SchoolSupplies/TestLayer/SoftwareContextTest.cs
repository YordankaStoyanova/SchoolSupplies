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
    public class SoftwareContextTests
    {
        private SoftwareContext softwareContext;

        [SetUp]
        public async Task Setup()
        {
            softwareContext = new SoftwareContext(TestManager.DbContext);

            // Изчистваме всички таблици преди всеки тест
            TestManager.DbContext.Softwares.RemoveRange(TestManager.DbContext.Softwares);
            TestManager.DbContext.Licenses.RemoveRange(TestManager.DbContext.Licenses);
            TestManager.DbContext.MaintenanceLogs.RemoveRange(TestManager.DbContext.MaintenanceLogs);
            TestManager.DbContext.Types.RemoveRange(TestManager.DbContext.Types);
            TestManager.DbContext.Rooms.RemoveRange(TestManager.DbContext.Rooms);
            TestManager.DbContext.Users.RemoveRange(TestManager.DbContext.Users);

            await TestManager.DbContext.SaveChangesAsync();
        }

        [Test]
        public async Task Create()
        {
            var software = new Software
            {
                Name = "Antivirus",
                SerialNumber = "SN12345",
            };

            int before = TestManager.DbContext.Softwares.Count();

            await softwareContext.Create(software);

            int after = TestManager.DbContext.Softwares.Count();
            Software last = TestManager.DbContext.Softwares.Last();

            Assert.That(before + 1 == after && last.Id == software.Id,
                "Id are not equal or the software is not created!");
        }

        [Test]
        public async Task Read()
        {
            var software = new Software
            {
                Name = "Office",
                SerialNumber = "SN67890",
            };

            await softwareContext.Create(software);

            var result = await softwareContext.Read(software.Id);

            Assert.That(result.Name == "Office", "Read() does not get Software by id!");
        }

        [Test]
        public async Task ReadAll()
        {
            var sw1 = new Software { Name = "SW1", SerialNumber = "SN1" };
            var sw2 = new Software { Name = "SW2", SerialNumber = "SN2" };

            await softwareContext.Create(sw1);
            await softwareContext.Create(sw2);

            int countBefore = 2;
            int countAfter = (await softwareContext.ReadAll()).Count;

            Assert.That(countBefore == countAfter, "ReadAll() does not return all of the Software!");
        }

        [Test]
        public async Task Update()
        {
            var software = new Software
            {
                Name = "OldSoftware",
                SerialNumber = "SN98765"
            };

            await softwareContext.Create(software);

            software.Name = "UpdatedSoftware";

            await softwareContext.Update(software);

            var updated = await softwareContext.Read(software.Id);

            Assert.That(updated.Name == "UpdatedSoftware",
                "Update() does not change the Software's name!");
        }

        [Test]
        public async Task Delete()
        {
            var software = new Software
            {
                Name = "ToDeleteSoftware",
                SerialNumber = "SN99999"
            };

            await softwareContext.Create(software);

            int before = (await softwareContext.ReadAll()).Count;

            await softwareContext.Delete(software.Id);

            int after = (await softwareContext.ReadAll()).Count;

            Assert.That(before == after + 1, "Delete() does not delete the Software!");
        }

        [Test]
        public void SoftwareValidation()
        {
            var software = new Software
            {
                Name = "A",
                SerialNumber = "SN10000"
            };

            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(software);

            bool isValid = Validator.TryValidateObject(software, context, validationResults, true);

            Assert.That(!isValid, "Validation should fail for too short Name!");
        }
    }
}
