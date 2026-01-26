using BusinessLayer;
using BusinessLayer.Enum;
using DataLayer;

namespace ServiceLayer
{
    public class SoftwareService
    {

        private readonly IDb<Software, int> _context;

        public SoftwareService(IDb<Software, int> context)
        {
            _context = context;
        }

        public async Task Create(Software item)
        {
            await _context.Create(item);
        }

        public async Task<Software> Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            return await _context.Read(key, useNavigationalProperties,isReadOnly);
        }

        public async Task<List<Software>> ReadAll(bool useNavigationalProperties = false,bool isReadOnly = false)
        {
            return await _context.ReadAll(useNavigationalProperties,isReadOnly);
        }

        public async Task Update(Software item, bool useNavigationalProperties = false)
        {

            await _context.Update(item, useNavigationalProperties);
        }

        public async Task Delete(int key)
        {
            await _context.Delete(key);
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
            return softwares.Count(s=> s.License.Status == LicenseStatus.Active);
        }
        public async Task<int> SoftwaresExpired()
        {
            List<Software> softwares = await ReadAll(true);
            return softwares.Count(s => s.License.Status == LicenseStatus.Expired);
        }

        private List<Software> SearchByParameter(List<Software> softwares, string parameter)
        {
            if(string.IsNullOrWhiteSpace(parameter)) return softwares;
            var filteredSoftwares = softwares.Where(h => h.Name == parameter || h.SerialNumber ==parameter).ToList();
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

