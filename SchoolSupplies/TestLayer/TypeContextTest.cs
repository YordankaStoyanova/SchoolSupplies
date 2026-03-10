using DataLayer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using Type = BusinessLayer.Type;

namespace TestLayer
{
    [TestFixture]
    public class TypeContextTest
    {
        private SchoolSuppliesDbContext dbContext;
        private TypeContext typeContext;

        [SetUp]
        public void Setup()
        {
            dbContext = TestManager.GetDbContext(Guid.NewGuid().ToString());
            typeContext = new TypeContext(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task Create_AddsType()
        {
            var type = new Type
            {
                Name = "Лаптоп"
            };

            int before = dbContext.Types.Count();

            await typeContext.Create(type);

            int after = dbContext.Types.Count();

            Assert.That(after, Is.EqualTo(before + 1));
        }

        [Test]
        public async Task Read_ReturnsType()
        {
            var type = new Type
            {
                Name = "Принтер"
            };

            await typeContext.Create(type);

            var result = await typeContext.Read(type.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Принтер"));
        }

        [Test]
        public async Task ReadAll_ReturnsAllTypes()
        {
            var type1 = new Type { Name = "Компютър" };
            var type2 = new Type { Name = "Операционна система" };

            await typeContext.Create(type1);
            await typeContext.Create(type2);

            var all = await typeContext.ReadAll();

            Assert.That(all.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Update_ChangesTypeName()
        {
            var type = new Type
            {
                Name = "Старо име"
            };

            await typeContext.Create(type);

            type.Name = "Ново име";

            await typeContext.Update(type);

            var updated = await typeContext.Read(type.Id);

            Assert.That(updated.Name, Is.EqualTo("Ново име"));
        }

        [Test]
        public async Task Delete_RemovesType()
        {
            var type = new Type
            {
                Name = "За изтриване"
            };

            await typeContext.Create(type);

            int before = (await typeContext.ReadAll()).Count;

            await typeContext.Delete(type.Id);

            int after = (await typeContext.ReadAll()).Count;

            Assert.That(after, Is.EqualTo(before - 1));
        }

        [Test]
        public void TypeValidation_Fails_WhenNameIsMissing()
        {
            var type = new Type
            {
                Name = null
            };

            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(type);

            bool isValid = Validator.TryValidateObject(type, context, validationResults, true);

            Assert.That(isValid, Is.False);
        }
    }
}
