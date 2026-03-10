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
    public class RoomContextTest
    {
        private SchoolSuppliesDbContext dbContext;
        private RoomContext roomContext;

        [SetUp]
        public void Setup()
        {
            dbContext = TestManager.GetDbContext(Guid.NewGuid().ToString());
            roomContext = new RoomContext(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task Create_AddsRoom()
        {
            var room = new Room("Lab 1", 1);

            await roomContext.Create(room);

            Assert.That(dbContext.Rooms.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Read_ReturnsRoom()
        {
            var room = new Room("Lab 2", 2);
            await roomContext.Create(room);

            var result = await roomContext.Read(room.Id);

            Assert.That(result.Name, Is.EqualTo("Lab 2"));
        }

        [Test]
        public async Task Update_ChangesRoomName()
        {
            var room = new Room("Old Room", 1);
            await roomContext.Create(room);

            room.Name = "New Room";
            await roomContext.Update(room);

            var updated = await roomContext.Read(room.Id);

            Assert.That(updated.Name, Is.EqualTo("New Room"));
        }

        [Test]
        public async Task Delete_RemovesRoom()
        {
            var room = new Room("Delete Room", 1);
            await roomContext.Create(room);

            int before = (await roomContext.ReadAll()).Count;
            await roomContext.Delete(room.Id);
            int after = (await roomContext.ReadAll()).Count;

            Assert.That(after, Is.EqualTo(before - 1));
        }
    }
}

