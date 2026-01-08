using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using Type = BusinessLayer.Type;

namespace DataLayer
{
    public class HardwareContext : IDb<Hardware, int>
    {
        private readonly SchoolSuppliesDbContext dbContext;

        public HardwareContext(SchoolSuppliesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task Create(Hardware item)
        {
            dbContext.Hardwares.Add(item);
            await dbContext.SaveChangesAsync();
        }

        

        public async Task<Hardware> Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Hardware> query = dbContext.Hardwares;

            if (useNavigationalProperties)
                query = query
                    .Include(h => h.Room)
                    .Include(h => h.User)
                    .Include(h => h.MaintenanceLogs)
                    .Include(h => h.Softwares)
                    .Include(h => h.Type);

            if (isReadOnly)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(h => h.Id == key);
        }

        public async Task<List<Hardware>> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Hardware> query = dbContext.Hardwares;

            if (useNavigationalProperties)
                query = query
                   .Include(h => h.Room)
                    .Include(h => h.User)
                    .Include(h => h.MaintenanceLogs)
                    .Include(h => h.Softwares)
                    .Include(h => h.Type);

            if (isReadOnly)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }

        public async Task Update(Hardware item, bool useNavigationalProperties = false)
        {
            Hardware hardwareFromDb = await Read(item.Id, useNavigationalProperties = false);
            dbContext.Entry<Hardware>(hardwareFromDb).CurrentValues.SetValues(item);
            if (useNavigationalProperties)
            {
                Type typeFromDb = await dbContext.Types.FindAsync(item.Type.Id);
                if (typeFromDb != null)
                {
                    hardwareFromDb.Type = typeFromDb;
                }
                else
                {
                    hardwareFromDb.Type = item.Type;
                }
                Room roomFromDb = await dbContext.Rooms.FindAsync(item.Room.Id);
                if (roomFromDb != null)
                {
                    hardwareFromDb.Room = roomFromDb;
                }
                else
                {
                    hardwareFromDb.Room = item.Room;
                }
                User userFromDb = await dbContext.Users.FindAsync(item.User.Id);
                if (userFromDb != null)
                {

                    hardwareFromDb.User = userFromDb;
                }
                else
                {
                    hardwareFromDb.User = item.User;
                }
                List<Software> softwares = new List<Software>();
                for (int i = 0; i < item.Softwares.Count; i++)
                {
                    Software softwareFromDb = await dbContext.Softwares.FindAsync(item.Softwares[i].Id);
                    if (softwareFromDb != null)
                    {
                        softwares.Add(softwareFromDb);
                    }
                    else
                    {
                        softwares.Add(item.Softwares[i]);
                    }
                }
                hardwareFromDb.Softwares = softwares;
                List<MaintenanceLog> logs = new List<MaintenanceLog>();
                for (int i = 0; i < item.MaintenanceLogs.Count; i++)
                {
                    MaintenanceLog logFromDb = dbContext.MaintenanceLogs.Find(item.MaintenanceLogs[i].Id);
                    if (logFromDb != null)
                    {
                        logs.Add(logFromDb);
                    }
                    else
                    {
                        logs.Add(item.MaintenanceLogs[i]);
                    }
                }
                hardwareFromDb.MaintenanceLogs = logs;
            }
            await dbContext.SaveChangesAsync();
        }
        public async Task Delete(int key)
        {
            var item = await dbContext.Hardwares.FindAsync(key);
            if (item != null)
            {
                dbContext.Hardwares.Remove(item);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
