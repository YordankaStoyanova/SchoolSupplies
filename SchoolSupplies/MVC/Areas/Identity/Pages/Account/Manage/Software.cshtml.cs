using BusinessLayer;
using BusinessLayer.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;
using ApplicationLayer.ViewModels;
using Type = BusinessLayer.Type;
using DataLayer;

namespace MVC.Areas.Identity.Pages.Account.Manage
{
    public class SoftwareModel : PageModel
    {
        private readonly SoftwareService _softwareService;
        private readonly LicenseService _licenseService;
        private readonly IDb<Type, int> _typeContext;

        public SoftwareModel(
            SoftwareService softwareService,
            LicenseService licenseService,
           IDb<Type, int> typeContext)
        {
            _softwareService = softwareService;
            _licenseService = licenseService;
            _typeContext = typeContext;
        }

        public List<Software> Softwares { get; set; } = new();
        public List<LicenseStatsViewModel> LicenseStats { get; set; } = new();
        public List<Type> Types { get; set; } = new();

        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }

        public async Task OnGet(string? s, int? t, int pageNumber = 1)
        {
            int pageSize = 5;

            var result = await _softwareService.ReadPaged(pageNumber, pageSize, s, t);

            Softwares = result.Items;
            PageNumber = result.CurrentPage;
            TotalPages = result.TotalPages;
            TotalItems = result.TotalItems;

            Types = await _typeContext.ReadAll(false, true);
            LicenseStats = await _licenseService.GetLicenseStatsByType();
        }
    }
}

