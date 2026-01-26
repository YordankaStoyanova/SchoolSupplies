using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RoomContext : IDb<Room, int>
    {
        private readonly SchoolSuppliesDbContext dbContext;

        public RoomContext(SchoolSuppliesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task Create(Room item)
        {
            dbContext.Rooms.Add(item);
            await dbContext.SaveChangesAsync();
        }

      

        public async Task<Room> Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Room> query = dbContext.Rooms;

            if (useNavigationalProperties)
                query = query.Include(r => r.Softwares).Include(r =>r.Softwares);
            if (isReadOnly)
                query = query.AsNoTrackingWithIdentityResolution();

            return await query.FirstOrDefaultAsync(r => r.Id == key);
        }

        public async Task<List<Room>> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Room> query = dbContext.Rooms;

            if (useNavigationalProperties)
                query = query.Include(r => r.Softwares).Include(r => r.Softwares);

            if (isReadOnly)
           query = query.AsNoTrackingWithIdentityResolution();
            
            return await query.ToListAsync();
        }

        public async Task Update(Room item, bool useNavigationalProperties = false)
        {
            Room roomFromDb = await Read(item.Id, useNavigationalProperties = false);
            dbContext.Entry<Room>(roomFromDb).CurrentValues.SetValues(item);
            if (useNavigationalProperties)
            {
                List<Software> softwares = new List<Software>();
                for (int i = 0; i < item.Softwares.Count; i++)
                {
                    Software softwareFromDb = await dbContext.Softwares.FindAsync(item.Softwares[i].Id);
                    if(softwareFromDb != null)
                    {
                        softwares.Add(softwareFromDb);
                    }
                    else
                    {
                        softwares.Add(item.Softwares[i]);
                    }
                }
                roomFromDb.Softwares=softwares;
                List<Hardware> hardwares = new List<Hardware>();
                for (int i = 0; i < item.Hardwares.Count; i++)
                {
                    Hardware hardwareFromDb = await dbContext.Hardwares.FindAsync(item.Hardwares[i].Id);
                    if (hardwareFromDb != null)
                    {
                        hardwares.Add(hardwareFromDb);
                    }
                    else
                    {
                        hardwares.Add(item.Hardwares[i]);
                    }
                }
                roomFromDb.Hardwares = hardwares;
            }
            await dbContext.SaveChangesAsync();
        }
        
        public async Task Delete(int key)
        {
            var room = await dbContext.Rooms.FindAsync(key);
            if (room != null)
            {
                dbContext.Rooms.Remove(room);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
