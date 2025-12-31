using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ItemContext : IDb<Item, int>
    {
        private readonly SchoolSuppliesDbContext dbContext;

        public ItemContext(SchoolSuppliesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task Create(Item item)
        {
            dbContext.Items.Add(item);
            await dbContext.SaveChangesAsync();

        }


        public async Task<Item> Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Item> query = dbContext.Items;

            if (useNavigationalProperties)
            {
                query = query.Include(i => i.Category)
                             .Include(i => i.Room)
                             .Include(i => i.User)
                             .Include(i => i.Licenses)
                             .Include(i => i.MaintenanceLogs);
            }

            if (isReadOnly)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(i => i.Id == key);

        }

        public async Task<List<Item>> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Item> query = dbContext.Items;

            if (useNavigationalProperties)
            {
                query = query.Include(i => i.Category)
                             .Include(i => i.Room)
                             .Include(i => i.User)
                             .Include(i => i.Licenses)
                             .Include(i => i.MaintenanceLogs);
            }

            if (isReadOnly)
                query = query.AsNoTracking();

            return await query.ToListAsync();

        }

        public async Task Update(Item item, bool useNavigationalProperties = false)
        {
            dbContext.Items.Update(item);
            await dbContext.SaveChangesAsync();
        }


        public async Task Delete(int key)
        {
            var item = await dbContext.Items.FindAsync(key);
            if (item != null)
            {
                dbContext.Items.Remove(item);
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task<bool> InventoryNumberExists(string inventoryNumber)
        {
            return await dbContext.Items
                .AnyAsync(i => i.InventoryNumber == inventoryNumber);
        }
        public async Task<List<Item>> GetByStatus(ItemStatus status)
        {
            return await dbContext.Items
                .Include(i => i.Room)
                .Include(i => i.User)
                .Where(i => i.Status == status)
                .ToListAsync();
        }
        public async Task AssignUser(int itemId, string userId)
        {
            var item = await dbContext.Items.FindAsync(itemId);
            if (item == null)
                throw new ArgumentException("Item not found");

            item.UserId = userId;
            await dbContext.SaveChangesAsync();
        }
        public async Task ChangeRoom(int itemId, int roomId)
        {
            var item = await dbContext.Items.FindAsync(itemId);
            if (item == null)
                throw new ArgumentException("Item not found");

            item.RoomId = roomId;
            await dbContext.SaveChangesAsync();
        }
       
        public async Task<List<Item>> SearchItems(string search)
        {
            return await dbContext.Items
                .Where(i =>
                    i.InventoryNumber.Contains(search) ||
                    i.SerialNumber.Contains(search) ||
                    i.Name.Contains(search))
                .ToListAsync();
        }
    }
}
