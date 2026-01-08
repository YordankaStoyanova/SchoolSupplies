using DataLayer;
using BusinessLayer;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class LicenseController : Controller
    {
        private readonly LicenseContext _licenseContext;
        private readonly SoftwareContext _itemContext;

        public LicenseController(LicenseContext licenseContext, SoftwareContext itemContext)
        {
            _licenseContext = licenseContext;
            _itemContext = itemContext;
        }

        public async Task<IActionResult> Index()
        {
            var licenses = await _licenseContext.ReadAll(useNavigationalProperties: true, isReadOnly: true);
            return View(licenses);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Items = await _itemContext.ReadAll(isReadOnly: true);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(License license)
        {
            if (ModelState.IsValid)
            {
                await _licenseContext.Create(license);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Items = await _itemContext.ReadAll(isReadOnly: true);
            return View(license);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var license = await _licenseContext.Read(id);
            if (license == null) return NotFound();
            ViewBag.Items = await _itemContext.ReadAll(isReadOnly: true);
            return View(license);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, License license)
        {
            if (id != license.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                await _licenseContext.Update(license);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Items = await _itemContext.ReadAll(isReadOnly: true);
            return View(license);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var license = await _licenseContext.Read(id);
            if (license == null) return NotFound();
            return View(license);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _licenseContext.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
