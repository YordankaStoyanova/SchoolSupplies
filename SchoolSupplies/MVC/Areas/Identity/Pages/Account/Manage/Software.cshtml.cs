using BusinessLayer;
using BusinessLayer.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;
using ApplicationLayer.ViewModels;

namespace MVC.Areas.Identity.Pages.Account.Manage
{
    public class SoftwareModel : PageModel
    {
        private readonly SoftwareService _softwareService;
        private readonly LicenseService _licenseService;
        public SoftwareModel(SoftwareService softwareService, LicenseService licenseService )
        {
            _softwareService = softwareService;
            _licenseService = licenseService;
        }
        public List<Software> Softwares { get; set; } = new();

        public List<LicenseStatsViewModel> LicenseStats { get; set; } = new();

        public async Task OnGet(string? s, int? t)
        {
            var softwares = await _softwareService.SearchCombined(s, t);
            Softwares = softwares ?? new List<Software>();

            LicenseStats = await _licenseService.GetLicenseStatsByType();
        }
    }
}
