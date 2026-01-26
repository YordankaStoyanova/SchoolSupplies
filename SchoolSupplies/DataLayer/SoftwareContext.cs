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
            Type typeFromDb =await dbContext.Types.FindAsync(item.Type.Id);
            if (typeFromDb != null) item.Type = typeFromDb;
            License licenseFromDb = await dbContext.Licenses.FindAsync(item.License.Id);
            if (licenseFromDb != null) item.License = licenseFromDb;
            dbContext.Softwares.Add(item);
            await dbContext.SaveChangesAsync();
        }

       

        public async Task<Software> Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Software> query = dbContext.Softwares;

            if (useNavigationalProperties)
                query = query
                    .Include(s => s.MaintenanceLogs)
                    .Include(s => s.License)
                    .Include(s => s.Type);

            if (isReadOnly)
                query = query.AsNoTrackingWithIdentityResolution();

            return await query.FirstOrDefaultAsync(s => s.Id == key);
        }

        public async Task<List<Software> > ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Software> query = dbContext.Softwares;

            if (useNavigationalProperties)
                query = query
                    .Include(s => s.MaintenanceLogs)
                    .Include(s => s.License)
                    .Include(s => s.Type);

            if (isReadOnly)
                query = query.AsNoTrackingWithIdentityResolution();

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
                
                License licenseFromDb  = await dbContext.Licenses.FindAsync(item.License.Id);
                if (licenseFromDb != null)
                {
                    softwareFromDb.License = licenseFromDb;
                }
                else
                {
                    softwareFromDb.License = item.License;
                }

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
