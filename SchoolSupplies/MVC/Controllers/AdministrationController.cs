using ApplicationLayer.ViewModels;
using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using ServiceLayer;
using System.Text;

namespace MVC.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministrationController : Controller
    {
        private readonly AdministrationService _administrationService;

        public AdministrationController(AdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        public IActionResult Create()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var resetPasswordPageUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity" },
                protocol: Request.Scheme);

            var result = await _administrationService.Create(model, resetPasswordPageUrl!);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(model);
            }

            return RedirectToPage("/Account/Manage/Administration", new { area = "Identity" });
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _administrationService.Read(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _administrationService.Read(id);
            if (user == null)
                return NotFound();

            var model = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var result = await _administrationService.Update(model);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(model);
            }

            return RedirectToPage("/Account/Manage/Administration", new { area = "Identity" });
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _administrationService.Read(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var result = await _administrationService.Delete(id);

            if (!result.Succeeded)
                return BadRequest();

            return RedirectToPage("/Account/Manage/Administration", new { area = "Identity" });
        }
    }

}


