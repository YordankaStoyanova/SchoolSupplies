using DataLayer;
using BusinessLayer;

namespace ServiceLayer
{
    public class LicenseService
    {
        private readonly IDb<License, int> licenseDb;

        public LicenseService(IDb<License, int> licenseDb)
        {
            this.licenseDb = licenseDb;
        }

        public async Task<List<License>> GetAllAsync()
            => await licenseDb.ReadAll(true);

        public async Task<License?> GetByIdAsync(int id)
            => await licenseDb.Read(id, true);

        public async Task<bool> IsExpiredAsync(int id)
        {
            var license = await licenseDb.Read(id);
            return license != null &&
                   license.ExpirationDate < DateTime.UtcNow;
        }

        public async Task CreateAsync(License license)
            => await licenseDb.Create(license);

        public async Task UpdateAsync(License license)
            => await licenseDb.Update(license);

        public async Task DeleteAsync(int id)
            => await licenseDb.Delete(id);
    }
}
