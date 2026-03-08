using ApplicationLayer.ViewModels;
using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using ServiceLayer;
using System.ComponentModel;
using License = BusinessLayer.License;

namespace MVC.Controllers
{
    public class SoftwareController(SoftwareService softwareService,HardwareService hardwareService, IDb<BusinessLayer.Type, int> typeContext, LicenseService licenseService) : Controller
    {
        private async Task LoadSoftwareDropdownsAsync(
     List<int>? selectedHardwareIds = null,
     int? selectedTypeId = null,
     int? selectedLicenseId = null)
        {
            ViewBag.Hardwares = new SelectList(
                await hardwareService.ReadAll(true, true), "Id", "Name", selectedHardwareIds);

            ViewBag.Types = new SelectList(
                await typeContext.ReadAll(false, true), "Id", "Name", selectedTypeId);

            ViewBag.Licenses = new SelectList(
                await licenseService.ReadAll(false, true), "Id", "Name", selectedLicenseId);
        }
        // private readonly ILogger<SoftwareController> _logger;
        // GET: Softwares
        public async Task<IActionResult> Index(string? s, int? t)
        {
            var softwares = await softwareService.SearchCombined(s,t);
            return View(softwares);
        }


        // GET: Softwares/Details/5
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var software = await softwareService.Read(id.Value, useNavigationalProperties: true, isReadOnly: true);
            if (software == null) return NotFound();

            ViewBag.InstalledOn = software.Hardwares?
                .Select(h => $"{h.InventoryNumber} - {h.Name}")
                .ToList() ?? new List<string>();

            return View(software);
        }

        [Authorize(Roles = "Administrator")]
        //// GET: Softwares/Create
        //public async Task<IActionResult> Create()
        //{
        //    ViewBag.SoftwareTypes = new SelectList(await typeContext.ReadAll(false, true), "Id", "Name");
        //    ViewBag.License = new SelectList(await licenseService.ReadAll(false, true), "Id", "Name");
        //    ViewBag.HardwareIds = new SelectList(await hardwareService.ReadAll(false, true), "Id", "Name");
        //    return View();

        //}

        //[Authorize(Roles = "Administrator")]
        //// POST: Softwares/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Name,SerialNumber,TypeId,LicenseId,HardwareIds")] SoftwareViewModel model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            await softwareService.Create(model);
        //            return RedirectToPage("/Account/Manage/Softwareware", new { area = "Identity" });
        //        }
        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return NotFound();
        //    }
        //}
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create()
        {
            await LoadSoftwareDropdownsAsync();
            return View(new SoftwareViewModel());
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SoftwareViewModel model)
        {
            var all = await softwareService.ReadAll(false, true);

            if (all.Any(s => s.SerialNumber == model.SerialNumber))
                ModelState.AddModelError(nameof(model.SerialNumber), "Има софтуер със същия сериен номер.");

            if (!ModelState.IsValid)
            {
                await LoadSoftwareDropdownsAsync(model.HardwareIds, model.TypeId, model.LicenseId);
                return View(model);
            }

            try
            {
                await softwareService.Create(model);
                return RedirectToPage("/Account/Manage/Software", new { area = "Identity" });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(nameof(model.LicenseId), ex.Message);
                await LoadSoftwareDropdownsAsync(model.HardwareIds, model.TypeId, model.LicenseId);
                return View(model);
            }
        }
        [Authorize(Roles = "Administrator")]
        // GET: Softwares/License
        [Authorize(Roles = "Administrator")]
        public IActionResult License()
        {
            return View(new License
            {
                ExpirationDate = DateTime.Today.AddYears(1),
                MaxUsage = 1
            });
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> License([Bind("Name,MaxUsage,ExpirationDate")] License license)
        {
            if (!ModelState.IsValid)
                return View(license);

            await licenseService.Create(license);
            return RedirectToPage("/Account/Manage/Software", new { area = "Identity" });
        }

        //[Authorize(Roles = "Administrator")]
        //// GET: Softwares/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var software = await softwareService.Read(id.Value);
        //    if (software == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(software);
        //}
        //[Authorize(Roles = "Administrator")]
        //// POST: Softwares/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SerialNumber,Category,Type,MaintanceLogs,License")] Software software)
        //{
        //    if (id != software.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await softwareService.Update(software);
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            return NotFound();
        //        }
        //    }
        //    return View(software);
        //    // return RedirectToAction(nameof(Index)); ?
        //}
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var sw = await softwareService.Read(id, true, true);
            if (sw == null) return NotFound();

            var vm = new SoftwareViewModel
            {
                Id = sw.Id,
                Name = sw.Name,
                SerialNumber = sw.SerialNumber,
                TypeId = sw.Type.Id,
                LicenseId = sw.LicenseId,
                HardwareIds = sw.Hardwares.Select(h => h.Id).ToList()
            };

            await LoadSoftwareDropdownsAsync(vm.HardwareIds, vm.TypeId, vm.LicenseId);
            return View(vm);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SoftwareViewModel model)
        {
            if (id != model.Id) return NotFound();

            var all = await softwareService.ReadAll(false, true);

            if (all.Any(s => s.Id != model.Id && s.SerialNumber == model.SerialNumber))
                ModelState.AddModelError(nameof(model.SerialNumber), "Има софтуер със същия сериен номер.");

            if (!ModelState.IsValid)
            {
                await LoadSoftwareDropdownsAsync(model.HardwareIds, model.TypeId, model.LicenseId);
                return View(model);
            }

            try
            {
                await softwareService.Update(model);
                return RedirectToPage("/Account/Manage/Software", new { area = "Identity" });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(nameof(model.LicenseId), ex.Message);
                await LoadSoftwareDropdownsAsync(model.HardwareIds, model.TypeId, model.LicenseId);
                return View(model);
            }
        }
        [Authorize(Roles = "Administrator")]
        // GET: Softwares/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }

            var software = await softwareService.Read(id.Value,useNavigationalProperties:true,isReadOnly:true);
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
                return RedirectToPage("/Account/Manage/Software", new { area = "Identity" });
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddMaintenance(int id)
        {
            var sw = await softwareService.Read(id, useNavigationalProperties: true, isReadOnly: true);
            if (sw == null) return NotFound();

            return View(new MaintenanceViewModel
            {
                ParentId = id,
                ItemName = $"{sw.Name} ({sw.SerialNumber})",
                Date = DateTime.Now
            });
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMaintenance(MaintenanceViewModel vm)
        {
            if (!ModelState.IsValid)
            {
            
                return View(vm);
            }

            await softwareService.AddMaintenance(vm.ParentId, vm.Description, vm.Date);

            return RedirectToAction("Details", new { id = vm.ParentId });
        }
    }

}

