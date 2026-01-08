using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using License = BusinessLayer.License;


namespace DataLayer
{
    public class LicenseContext : IDb<License, int>
    {
        private readonly SchoolSuppliesDbContext dbContext;

        public LicenseContext(SchoolSuppliesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task Create(License item)
        {
            dbContext.Licenses.Add(item);
            await dbContext.SaveChangesAsync();
        }

        

        public async Task<License> Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<License> query = dbContext.Licenses;

            if (useNavigationalProperties)
                query = query.Include(l => l.Softwares);

            if (isReadOnly)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(l => l.Id == key);
        }

        public async Task<List<License>> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<License> query = dbContext.Licenses;

            if (useNavigationalProperties)
                query = query.Include(l => l.Softwares);
            if (isReadOnly)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task Update(License item, bool useNavigationalProperties = false)
        {
            License licenseFromDb = await Read(item.Id, useNavigationalProperties);
            dbContext.Entry<License>(licenseFromDb).CurrentValues.SetValues(item);
            if (useNavigationalProperties)
            {
                List<Software> softwares = new List<Software>();
                for (int i = 0; i < item.Softwares.Count; i++)
                {
                    Software softwareFromDb = dbContext.Softwares.Find(item.Softwares[i].Id);
                    if(softwareFromDb != null) softwares.Add(softwareFromDb);
                    else softwares.Add(item.Softwares[i]);
                }
                licenseFromDb.Softwares = softwares;
            }
           
            await dbContext.SaveChangesAsync();
      
        }
        public async Task Delete(int key)
        {
            var license = await dbContext.Licenses.FindAsync(key);
            if (license != null)
            {
                dbContext.Licenses.Remove(license);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
