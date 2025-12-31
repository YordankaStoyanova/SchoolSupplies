using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class CategoryContext : IDb<Category, int>
    {
        private readonly SchoolSuppliesDbContext dbContext;

        public CategoryContext(SchoolSuppliesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Create(Category item)
        {
            dbContext.Categories.Add(item);
            await dbContext.SaveChangesAsync();
          
           
        }
        public async Task<Category> Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Category> query = dbContext.Categories;

            if (useNavigationalProperties)
            {
                query = query.Include(c => c.Items);
            }

            if (isReadOnly)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync(c => c.Id == key);
        }

        public async Task<List<Category>> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Category> query = dbContext.Categories;

            if (useNavigationalProperties)
            {
                query = query.Include(c => c.Items);
            }

            if (isReadOnly)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task Update(Category item, bool useNavigationalProperties = false)
        {
            dbContext.Categories.Update(item);
            await dbContext.SaveChangesAsync();
        }
        public async Task Delete(int key)
        {
            var category = await dbContext.Categories.FindAsync(key);
            if (category != null)
            {
                dbContext.Categories.Remove(category);
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task<int> GetItemsCount(int categoryId)
        {
            return await dbContext.Items.CountAsync(i => i.CategoryId == categoryId);
        }
    }
}
