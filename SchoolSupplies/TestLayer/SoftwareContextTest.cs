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
        private SchoolSuppliesDbContext dbContext;
        private SoftwareContext softwareContext;

        [SetUp]
        public void Setup()
        {
            dbContext = TestManager.GetDbContext(Guid.NewGuid().ToString());
            softwareContext = new SoftwareContext(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task Create_AddsSoftwareToDatabase()
        {
            var type = TestData.CreateType("Office");
            var license = TestData.CreateLicense("Office License");

            dbContext.Types.Add(type);
            dbContext.Licenses.Add(license);
            await dbContext.SaveChangesAsync();

            var software = TestData.CreateSoftware(type, license);

            int before = dbContext.Softwares.Count();

            await softwareContext.Create(software);

            int after = dbContext.Softwares.Count();

            Assert.That(after, Is.EqualTo(before + 1));
        }

        [Test]
        public async Task Read_ReturnsCorrectSoftware()
        {
            var type = TestData.CreateType("Antivirus");
            var license = TestData.CreateLicense("AV License");

            dbContext.Types.Add(type);
            dbContext.Licenses.Add(license);
            await dbContext.SaveChangesAsync();

            var software = TestData.CreateSoftware(type, license, name: "Kaspersky");

            await softwareContext.Create(software);

            var result = await softwareContext.Read(software.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Kaspersky"));
        }

        [Test]
        public async Task ReadAll_ReturnsAllSoftwares()
        {
            var type = TestData.CreateType("Office");
            var license = TestData.CreateLicense();

            dbContext.Types.Add(type);
            dbContext.Licenses.Add(license);
            await dbContext.SaveChangesAsync();

            var sw1 = TestData.CreateSoftware(type, license, name: "Office 1");
            var sw2 = TestData.CreateSoftware(type, license, name: "Office 2");
            sw2.SerialNumber = "SOFT-002";

            await softwareContext.Create(sw1);
            await softwareContext.Create(sw2);

            var all = await softwareContext.ReadAll();

            Assert.That(all.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Update_ChangesSoftwareName()
        {
            var type = TestData.CreateType("Office");
            var license = TestData.CreateLicense();

            dbContext.Types.Add(type);
            dbContext.Licenses.Add(license);
            await dbContext.SaveChangesAsync();

            var software = TestData.CreateSoftware(type, license, name: "Old Name");

            await softwareContext.Create(software);

            var fromDb = await softwareContext.Read(software.Id, true);
            fromDb.Name = "New Name";

            await softwareContext.Update(fromDb);

            var updated = await softwareContext.Read(software.Id);

            Assert.That(updated.Name, Is.EqualTo("New Name"));
        }

        [Test]
        public async Task Delete_RemovesSoftware()
        {
            var type = TestData.CreateType("Office");
            var license = TestData.CreateLicense();

            dbContext.Types.Add(type);
            dbContext.Licenses.Add(license);
            await dbContext.SaveChangesAsync();

            var software = TestData.CreateSoftware(type, license);

            await softwareContext.Create(software);

            int before = (await softwareContext.ReadAll()).Count;

            await softwareContext.Delete(software.Id);

            int after = (await softwareContext.ReadAll()).Count;

            Assert.That(after, Is.EqualTo(before - 1));
        }

        [Test]
        public void SoftwareValidation_Fails_WhenNameTooShort()
        {
            var software = new Software
            {
                Name = "A",
                SerialNumber = "SOFT-001"
            };

            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(software);

            bool isValid = Validator.TryValidateObject(software, context, validationResults, true);

            Assert.That(isValid, Is.False);
        }
    }
}
