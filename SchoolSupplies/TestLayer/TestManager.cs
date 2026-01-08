using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using BusinessLayer;
using DataLayer;
using NUnit.Framework;
namespace TestLayer
{
    [TestFixture]
    public class TestManager
    {
        internal static SchoolSuppliesDbContext DbContext;

        static TestManager()
        {
            var options = new DbContextOptionsBuilder<SchoolSuppliesDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            DbContext = new SchoolSuppliesDbContext(options);
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            DbContext.Dispose();
        }
    }

}

