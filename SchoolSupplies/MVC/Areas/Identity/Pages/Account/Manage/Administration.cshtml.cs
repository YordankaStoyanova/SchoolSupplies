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

        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalUsers { get; set; }

        public async Task OnGet(string? s, int pageNumber = 1)
        {
            int pageSize = 2;

            var result = await _administrationService.ReadPaged(pageNumber, pageSize, s);

            Users = result.Items;
            PageNumber = result.CurrentPage;
            TotalPages = result.TotalPages;
            TotalUsers = result.TotalItems;
            TotalUsers = await _administrationService.TotalUsersCountAsync();
        }
    }
}

