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
    public class LicenseContextTest
    {
        private SchoolSuppliesDbContext dbContext;
        private LicenseContext licenseContext;

        [SetUp]
        public void Setup()
        {
            dbContext = TestManager.GetDbContext(Guid.NewGuid().ToString());
            licenseContext = new LicenseContext(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task Create_AddsLicense()
        {
            var license = TestData.CreateLicense();

            await licenseContext.Create(license);

            Assert.That(dbContext.Licenses.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Read_ReturnsLicense()
        {
            var license = TestData.CreateLicense("Windows License", 20);

            await licenseContext.Create(license);

            var result = await licenseContext.Read(license.Id);

            Assert.That(result.Name, Is.EqualTo("Windows License"));
        }

        [Test]
        public async Task Update_ChangesLicenseName()
        {
            var license = TestData.CreateLicense("Old License", 10);

            await licenseContext.Create(license);

            license.Name = "New License";
            await licenseContext.Update(license);

            var updated = await licenseContext.Read(license.Id);

            Assert.That(updated.Name, Is.EqualTo("New License"));
        }

        [Test]
        public async Task Delete_RemovesLicense()
        {
            var license = TestData.CreateLicense();

            await licenseContext.Create(license);

            int before = (await licenseContext.ReadAll()).Count;
            await licenseContext.Delete(license.Id);
            int after = (await licenseContext.ReadAll()).Count;

            Assert.That(after, Is.EqualTo(before - 1));
        }
    }
}

