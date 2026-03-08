using ApplicationLayer.ViewModels;
using BusinessLayer;
using BusinessLayer.Enum;
using DataLayer;
using Type = BusinessLayer.Type;

namespace ServiceLayer
{
    public class SoftwareService
    {

        private readonly IDb<Software, int> _softwareContext;
        private readonly IDb<Hardware, int> _hardwareContext;
        private readonly IDb<License, int> _licenseContext;
        private readonly IDb<Type, int> _typeContext;

        public SoftwareService(IDb<Software, int> context,IDb<Hardware,int> hardwareContext,IDb<License,int> licenseContext,IDb<Type,int> typeContext)
        {
            _softwareContext = context;
            _hardwareContext = hardwareContext; 
            _licenseContext = licenseContext;
            _typeContext = typeContext;
        }

       
        public async Task Create(SoftwareViewModel item)
        {
            if (await ExceedsLicenseUsage(item.LicenseId, item.HardwareIds))
                throw new InvalidOperationException("Избраният лиценз не позволява толкова инсталации.");

            var type = await _typeContext.Read(item.TypeId);
            var license = await _licenseContext.Read(item.LicenseId);

            var hardwares = new List<Hardware>();
            foreach (var id in item.HardwareIds.Distinct())
            {
                var hardware = await _hardwareContext.Read(id);
                if (hardware != null) hardwares.Add(hardware);
            }

            var software = new Software(item.Name, item.SerialNumber, type, hardwares, license);
            await _softwareContext.Create(software);
        }

        public async Task<Software> Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            return await _softwareContext.Read(key, useNavigationalProperties,isReadOnly);
        }

        public async Task<List<Software>> ReadAll(bool useNavigationalProperties = false,bool isReadOnly = false)
        {
            return await _softwareContext.ReadAll(useNavigationalProperties,isReadOnly);
        }

        //public async Task Update(Software item, bool useNavigationalProperties = false)
        //{

        //    await _softwareContext.Update(item, useNavigationalProperties);
        //}
        public async Task Update(SoftwareViewModel item)
        {
            if (await ExceedsLicenseUsage(item.LicenseId, item.HardwareIds, item.Id))
                throw new InvalidOperationException("Избраният лиценз не позволява толкова инсталации.");

            var softwareFromDb = await _softwareContext.Read(item.Id, useNavigationalProperties: true, isReadOnly: false);
            if (softwareFromDb == null)
                throw new ArgumentException("Software not found");

            softwareFromDb.Name = item.Name;
            softwareFromDb.SerialNumber = item.SerialNumber;

            softwareFromDb.Type = await _typeContext.Read(item.TypeId);
            softwareFromDb.License = await _licenseContext.Read(item.LicenseId);

            var hardwares = new List<Hardware>();
            foreach (var hwId in item.HardwareIds.Distinct())
            {
                var hw = await _hardwareContext.Read(hwId);
                if (hw != null) hardwares.Add(hw);
            }

            softwareFromDb.Hardwares = hardwares;

            await _softwareContext.Update(softwareFromDb, useNavigationalProperties: true);
        }
        public async Task Delete(int key)
        {
            await _softwareContext.Delete(key);
        }
        private List<Software> SearchByType(List<Software> softwares,int? typeId)
        {
            if(typeId is null) return softwares;
            var softwaresByStatus = softwares.Where(s => s.Type.Id == typeId.Value).ToList();
            return softwaresByStatus;
        }
        public async Task<int> SoftwareActive()
        {
            List<Software> softwares = await ReadAll(true);
            return softwares.Count(s=> s.License.ExpirationDate>DateTime.Now);
        }
        public async Task<int> SoftwaresExpired()
        {
            List<Software> softwares = await ReadAll(true);
            return softwares.Count(s => s.License.ExpirationDate <= DateTime.Now);
        }

        private List<Software> SearchByParameter(List<Software> softwares, string parameter)
        {
            if(string.IsNullOrWhiteSpace(parameter)) return softwares;
            parameter= parameter.Trim().ToLower();
            var filteredSoftwares = softwares.Where(h => h.Name.ToLower().Contains(parameter) || h.SerialNumber.ToLower().Contains(parameter)).ToList();
            return filteredSoftwares;
        }
        public async Task<List<Software>> SearchCombined(string parameter, int? typeId)
        {
            List<Software> softwares = await ReadAll(true, true);
            List<Software> softwaresByType = SearchByType(softwares,typeId);
            List<Software> result = SearchByParameter(softwaresByType, parameter);
            return result;
        }
        public async Task AddMaintenance(int softwareId, string description, DateTime date)
        {
            var software = await _softwareContext.Read(softwareId, useNavigationalProperties: true, isReadOnly: false);
            if (software == null) throw new ArgumentException("Software not found");

            software.MaintenanceLogs.Add(new MaintenanceLog(description, date)
            {
                Software = software,
                Hardware = null
            });

            await _softwareContext.Update(software, useNavigationalProperties: true);
        }
        private async Task<bool> ExceedsLicenseUsage(int licenseId, List<int> newHardwareIds, int? softwareIdToExclude = null)
        {
            var softwares = await _softwareContext.ReadAll(useNavigationalProperties: true, isReadOnly: true);
            var license = await _licenseContext.Read(licenseId, useNavigationalProperties: false, isReadOnly: true);

            if (license == null) return false;

            int currentUsage = softwares
                .Where(s => s.LicenseId == licenseId && (!softwareIdToExclude.HasValue || s.Id != softwareIdToExclude.Value))
                .Sum(s => s.Hardwares?.Count ?? 0);

            int newUsage = newHardwareIds?.Distinct().Count() ?? 0;

            return currentUsage + newUsage > license.MaxUsage;
        }
    }

}

