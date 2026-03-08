using BusinessLayer;
using BusinessLayer.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;

namespace MVC.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "Administrator")]
    public class AdministrationModel : PageModel
    {
        private readonly AdministrationService _administrationService;

        public AdministrationModel(AdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        public List<User> Users { get; set; } = new();

        public async Task OnGet(string? s)
        {
            Users = await _administrationService.SearchByParameter(s);
        }
    }
}
