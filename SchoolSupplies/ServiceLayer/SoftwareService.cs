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
            var type = await _typeContext.Read(item.TypeId);
            var license = await _licenseContext.Read(item.LicenseId);
            var hardwares = new List<Hardware>();
            foreach (var id in item.HardwareIds)
            {
                var hardware = await _hardwareContext.Read(id);
                if (hardware != null) hardwares.Add(hardware);
            }
            var software = new Software(item.Name, item.SerialNumber, type, hardwares,license);
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

        public async Task Update(Software item, bool useNavigationalProperties = false)
        {

            await _softwareContext.Update(item, useNavigationalProperties);
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
            return softwares.Count(s=> s.License.ExpirationDate<=DateTime.Now);
        }
        public async Task<int> SoftwaresExpired()
        {
            List<Software> softwares = await ReadAll(true);
            return softwares.Count(s => s.License.ExpirationDate > DateTime.Now);
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
        
    }

}

