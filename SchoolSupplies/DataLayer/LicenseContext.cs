using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License = BusinessLayer.License;

namespace DataLayer
{
    public class LicenseContext : IDb<License,int>
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
            { 
            query = query.Include(l => l.Item);
            }
            if (isReadOnly)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync(l => l.Id == key);
        }

        public async  Task<List<License>> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<License> query = dbContext.Licenses;

            if (useNavigationalProperties)
            {
                query = query.Include(l => l.Item);
            }
            if (isReadOnly)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public  async Task Update(License item, bool useNavigationalProperties = false)
        {
            dbContext.Licenses.Update(item);
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
        public async Task<List<License>> GetExpiringLicenses(int days)
        {
            var date = DateTime.Now.AddDays(days);

            return await dbContext.Licenses
                .Include(l => l.Item)
                .Where(l => l.ExpirationDate <= date)
                .ToListAsync();
        }
    }
}
