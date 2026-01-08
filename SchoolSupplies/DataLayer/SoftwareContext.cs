using BusinessLayer;
using BusinessLayer.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = BusinessLayer.Type;

namespace DataLayer
{
    public class SoftwareContext : IDb<Software, int>
    {
        private readonly SchoolSuppliesDbContext dbContext;

        public SoftwareContext(SchoolSuppliesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task Create(Software item)
        {
            dbContext.Softwares.Add(item);
            await dbContext.SaveChangesAsync();
        }

       

        public async Task<Software> Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Software> query = dbContext.Softwares;

            if (useNavigationalProperties)
                query = query
                    .Include(s => s.Room)
                    .Include(s => s.User)
                    .Include(s => s.MaintenanceLogs)
                    .Include(s => s.Licenses)
                    .Include(s => s.Type);

            if (isReadOnly)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(s => s.Id == key);
        }

        public async Task<List<Software> > ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Software> query = dbContext.Softwares;

            if (useNavigationalProperties)
                query = query
                    .Include(s => s.Room)
                    .Include(s => s.User)
                    .Include(s => s.MaintenanceLogs)
                    .Include(s => s.Licenses)
                    .Include(s => s.Type);

            if (isReadOnly)
                query = query.AsNoTracking();

            return await query.ToListAsync();

        }

        public async Task Update(Software item, bool useNavigationalProperties = false)
        {
            Software softwareFromDb = await Read(item.Id, useNavigationalProperties = false);
            dbContext.Entry<Software>(softwareFromDb).CurrentValues.SetValues(item);
            if (useNavigationalProperties)
            {
                Type typeFromDb =  await dbContext.Types.FindAsync(item.Type.Id);
                if(typeFromDb != null)
                {
                    softwareFromDb.Type = typeFromDb;
                }
                else
                {
                    softwareFromDb.Type = item.Type;
                }
                Room roomFromDb = await dbContext.Rooms.FindAsync(item.Room.Id);
                if (roomFromDb != null)
                {
                    softwareFromDb.Room = roomFromDb; 
                }
                else
                {
                    softwareFromDb.Room = item.Room;
                }
                User userFromDb = await dbContext.Users.FindAsync(item.User.Id);
                if (userFromDb != null) {

                    softwareFromDb.User= userFromDb;
                }
                else
                {
                    softwareFromDb.User = item.User;
                }
                List<License>licenses  = new List<License>();
                for (int i = 0; i < item.Licenses.Count; i++)
                {
                    License licenseFromDb = await dbContext.Licenses.FindAsync(item.Licenses[i].Id);
                    if (licenseFromDb != null)
                    {
                        licenses.Add(licenseFromDb);
                    }
                    else
                    {
                        licenses.Add(item.Licenses[i]);
                    }
                }
                softwareFromDb.Licenses = licenses;
                List <MaintenanceLog> logs = new List <MaintenanceLog>();
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
                softwareFromDb.MaintenanceLogs = logs;
            }
            await dbContext.SaveChangesAsync();
        }


        public async Task Delete(int key)
        {
            var item = await dbContext.Softwares.FindAsync(key);
            if (item != null)
            {
                dbContext.Softwares.Remove(item);
                await dbContext.SaveChangesAsync();
            }
        }

      
    }
}
