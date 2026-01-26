using DataLayer;
using BusinessLayer;

namespace ServiceLayer
{
    public class LicenseService
    {
        private readonly IDb<License, int> _context;

        public LicenseService(IDb<License, int> context)
        {
            _context = context;
        }

        public async Task Create(License item)
        {
            await _context.Create(item);
        }

        public async Task<License> Read(int key, bool useNavigationalProperties = false,bool isReadOnly = false )
        {
            return await _context.Read(key, useNavigationalProperties,isReadOnly);
        }

        public async Task<List<License>> ReadAll(bool useNavigationalProperties = false,bool isReadOnly = false)
        {
            return await _context.ReadAll(useNavigationalProperties,isReadOnly);
        }

        public async Task Update(License item, bool useNavigationalProperties = false)
        {

            await _context.Update(item, useNavigationalProperties);
        }

        public async Task Delete(int key)
        {
            await _context.Delete(key);
        }
    }
}

