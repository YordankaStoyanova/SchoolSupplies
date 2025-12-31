using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class MaintenanceLogContext : IDb<MaintenanceLog,int>
    {
        private readonly SchoolSuppliesDbContext dbContext;

        public MaintenanceLogContext(SchoolSuppliesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Create(MaintenanceLog item)
        {
           dbContext.MaintenanceLogs.Add(item);
            await dbContext.SaveChangesAsync();
        }

       
        public  async Task<MaintenanceLog> Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<MaintenanceLog> query = dbContext.MaintenanceLogs;

            if (useNavigationalProperties)
            {
                query = query.Include(m => m.Item);
            }
            if (isReadOnly)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync(m => m.Id == key);
        }

        public async Task<List<MaintenanceLog>> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<MaintenanceLog> query = dbContext.MaintenanceLogs;

            if (useNavigationalProperties)
            {
                query = query.Include(m => m.Item);
            }
            if (isReadOnly)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public async Task Update(MaintenanceLog item, bool useNavigationalProperties = false)
        {
            dbContext.MaintenanceLogs.Update(item);
            await dbContext.SaveChangesAsync();
        }

        public  async Task Delete(int key)
        {
            var log = await dbContext.MaintenanceLogs.FindAsync(key);
            if (log != null)
            {
                dbContext.MaintenanceLogs.Remove(log);
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task<List<MaintenanceLog>> GetLogsForItem(int itemId)
        {
            return await dbContext.MaintenanceLogs
                .Where(m => m.ItemId == itemId)
                .OrderByDescending(m => m.Date)
                .ToListAsync();
        }

    }
}
