using BusinessLayer;
using BusinessLayer.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;

namespace MVC.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "Administrator")]
    public class AdministrationModel : PageModel
    {
        private readonly HardwareService _hardwareService;
        public AdministrationModel(HardwareService hardwareService)
        {
            _hardwareService = hardwareService;
        }
        public List<User> Users { get; set; }
        public async Task OnGet(string? s, ItemStatus? t, string? r)
        {
            Users = new List<User>();
        }
    }
}
