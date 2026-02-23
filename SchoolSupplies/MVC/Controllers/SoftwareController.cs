using ApplicationLayer.ViewModels;
using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using ServiceLayer;

namespace MVC.Controllers
{
    public class SoftwareController(SoftwareService softwareService,HardwareService hardwareService, IDb<BusinessLayer.Type, int> typeContext, LicenseService licenseService) : Controller
    {
       // private readonly ILogger<SoftwareController> _logger;
        // GET: Softwares
        public async Task<IActionResult> Index(string? s, int? t)
        {
            var softwares = await softwareService.SearchCombined(s,t);
            return View(softwares);
        }

        [Authorize(Roles = "Administrator,User")]
        // GET: Softwares/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var software = await softwareService.Read(id.Value);
            if (software == null)
            {
                return NotFound();
            }

            return View(software);
        }
  
        [Authorize(Roles = "Administrator")]
        // GET: Softwares/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.SoftwareTypes = new SelectList(await typeContext.ReadAll(false, true), "Id", "Name");
            ViewBag.License = new SelectList(await licenseService.ReadAll(false, true), "Id", "Name");
            ViewBag.HardwareIds = new SelectList(await hardwareService.ReadAll(false, true), "Id", "Name");
            return View();
            
        }
      
        [Authorize(Roles = "Administrator")]
        // POST: Softwares/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,SerialNumber,TypeId,LicenseId,HardwareIds")] SoftwareViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await softwareService.Create(model);
                    return RedirectToPage("/Account/Manage/Softwareware", new { area = "Identity" });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        [Authorize(Roles = "Administrator")]
        // GET: Hardwares/Create
        public IActionResult License()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> License([Bind("Name,Usage,MaxUsage,ExparationDate,Status")] License license)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await licenseService.Create(license);
                    return RedirectToPage("/Account/Manage/Software", new { area = "Identity" });
                }
                return View(license);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Administrator")]
        // GET: Softwares/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var software = await softwareService.Read(id.Value);
            if (software == null)
            {
                return NotFound();
            }
            return View(software);
        }
        [Authorize(Roles = "Administrator")]
        // POST: Softwares/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SerialNumber,Category,Type,MaintanceLogs,License")] Software software)
        {
            if (id != software.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await softwareService.Update(software);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }
            return View(software);
            // return RedirectToAction(nameof(Index)); ?
        }

        [Authorize(Roles = "Administrator")]
        // GET: Softwares/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var software = await softwareService.Read(id.Value);
            if (software == null)
            {
                return NotFound();
            }

            return View(software);
        }
        [Authorize(Roles = "Administrator")]
        // POST: Softwares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            try
            {
                await softwareService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

    }
}
