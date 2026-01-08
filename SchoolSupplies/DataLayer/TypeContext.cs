using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Type = BusinessLayer.Type;
namespace DataLayer
{
    public class TypeContext:IDb<Type,int>
    {
        private readonly SchoolSuppliesDbContext dbContext;

        public TypeContext(SchoolSuppliesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task Create(Type item)
        {
            dbContext.Types.Add(item);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Type> Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {

            IQueryable<Type> query = dbContext.Types;

            if (isReadOnly)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(t => t.Id == key);
        }

        public async Task<List<Type>> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {

            IQueryable<Type> query = dbContext.Types;
            if (isReadOnly)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }

        public async Task Update(Type item, bool useNavigationalProperties = false)
        {

            dbContext.Types.Update(item);
            await dbContext.SaveChangesAsync();
        }



        public async Task Delete(int key)
        {
            var type = await dbContext.Types.FindAsync(key);
            if (type != null)
            {
                dbContext.Types.Remove(type);
                await dbContext.SaveChangesAsync();
            }
        }

    }
}
