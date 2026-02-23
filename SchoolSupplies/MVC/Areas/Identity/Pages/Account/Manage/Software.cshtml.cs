using BusinessLayer;
using BusinessLayer.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;

namespace MVC.Areas.Identity.Pages.Account.Manage
{
    public class SoftwareModel : PageModel
    {
        private readonly SoftwareService _softwareService;
        public SoftwareModel(SoftwareService softwareService)
        {
            _softwareService = softwareService;
        }
        public List<Software> Softwares { get; set; }
        public async Task OnGet(string? s, int? t)
        {
            var softwares = await _softwareService.SearchCombined(s, t);
            Softwares = (softwares is null)?new List<Software>():softwares;
        }
    }
}
