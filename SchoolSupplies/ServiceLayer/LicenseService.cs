using ApplicationLayer.ViewModels;
using BusinessLayer;
using DataLayer;

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
        public async Task<List<LicenseStatsViewModel>> GetLicenseStatsByType()
        {
            var licenses = await _context.ReadAll(true, true);

            var stats = licenses
                .Where(l => l.Softwares != null)
                .SelectMany(l => l.Softwares
                    .Where(s => s != null && s.Type != null && !string.IsNullOrWhiteSpace(s.Type.Name))
                    .Select(s => new
                    {
                        Type = s.Type.Name,
                        MaxUsage = l.MaxUsage,
                        Used = s.Hardwares?.Count ?? 0
                    }))
                .GroupBy(x => x.Type)
                .Select(g => new LicenseStatsViewModel
                {
                    Title = g.Key,
                    TotalLicenses = g.Sum(x => x.MaxUsage),
                    UsedLicenses = g.Sum(x => x.Used)
                })
                .OrderBy(x => x.Title)
                .ToList();

            return stats;
        }
    }
}

