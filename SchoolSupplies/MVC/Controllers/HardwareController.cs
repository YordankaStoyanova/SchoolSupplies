using ApplicationLayer.ViewModels;
using BusinessLayer;
using BusinessLayer.Enum;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using ServiceLayer;
using System.Composition;

namespace MVC.Controllers
{
    public class HardwareController(HardwareService hardwareService, SoftwareService softwareService, IDb<BusinessLayer.Type,int> typeContext, IDb<Room,int> roomContext) : Controller
    {
        private async Task LoadHardwareDropdownsAsync(
        List<int>? selectedSoftwareIds = null,
        int? selectedTypeId = null,
        int? selectedRoomId = null)
        {
            ViewBag.SoftwareList = new SelectList(
                await softwareService.ReadAll(true, true), "Id", "Name", selectedSoftwareIds);

            ViewBag.Types = new SelectList(
                await typeContext.ReadAll(false, true), "Id", "Name", selectedTypeId);

            ViewBag.Rooms = new SelectList(
                await roomContext.ReadAll(false, true), "Id", "Name", selectedRoomId);
        }

        // GET: Hardwares
        public async Task<IActionResult> Index(string? s, ItemStatus? t, int? r, int pageNumber = 1)
        {
            int pageSize = 5;

            var result = await hardwareService.ReadPaged(pageNumber, pageSize, s, t, r);

            ViewBag.PageNumber = result.CurrentPage;
            ViewBag.TotalPages = result.TotalPages;

            return View(result.Items);
        }



        [Authorize(Roles = "Administrator,User")]
        // GET: Hardwares/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardware = await hardwareService.Read((int)id, useNavigationalProperties: true, isReadOnly: true);
            if (hardware == null)
            {
                return NotFound();
            }
            ViewBag.InstalledSoftware = hardware.Softwares?
            .Select(s => s.Name)
            .ToList() ?? new List<string>();

            return View(hardware);
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create()
        {
            await LoadHardwareDropdownsAsync();
            return View(new HardwareViewModel());
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HardwareViewModel model)
        {
            var all = await hardwareService.ReadAll(false, true);

            if (all.Any(h => h.InventoryNumber == model.InventoryNumber))
                ModelState.AddModelError(nameof(model.InventoryNumber), "Има устройство с този инвентарен номер.");

            if (all.Any(h => h.SerialNumber == model.SerialNumber))
                ModelState.AddModelError(nameof(model.SerialNumber), "Има устройство с този сериен номер.");

            if (!ModelState.IsValid)
            {
                await LoadHardwareDropdownsAsync(model.SoftwareIds);
                return View(model);
            }

            await hardwareService.Create(model);
            return RedirectToPage("/Account/Manage/Hardware", new { area = "Identity" });
        }
   
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var hardware = await hardwareService.Read(id, true, true);
            if (hardware == null) return NotFound();

            var vm = new HardwareViewModel
            {
                Id = hardware.Id,
                Name = hardware.Name,
                InventoryNumber = hardware.InventoryNumber,
                SerialNumber = hardware.SerialNumber,
                Status = hardware.Status,
                TypeId = hardware.Type.Id,
                RoomId = hardware.Room.Id,
                SoftwareIds = hardware.Softwares.Select(s => s.Id).ToList()
            };
            await LoadHardwareDropdownsAsync(vm.SoftwareIds, vm.TypeId, vm.RoomId);
            return View(vm);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HardwareViewModel model)
        {
            if (id != model.Id) return NotFound();

            var all = await hardwareService.ReadAll(false, true);

            if (all.Any(h => h.Id != model.Id && h.InventoryNumber == model.InventoryNumber))
                ModelState.AddModelError(nameof(model.InventoryNumber), "Има устройство с този инвентарен номер.");

            if (all.Any(h => h.Id != model.Id && h.SerialNumber == model.SerialNumber))
                ModelState.AddModelError(nameof(model.SerialNumber), "Има устройство с този сериен номер.");

            if (!ModelState.IsValid)
            {
                await LoadHardwareDropdownsAsync(model.SoftwareIds, model.TypeId, model.RoomId);
                return View(model);
            }

            await hardwareService.Update(model);
            return RedirectToPage("/Account/Manage/Hardware", new { area = "Identity" });
        }
        [Authorize(Roles = "Administrator")]
        // GET: Hardwares/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardware = await hardwareService.Read((int)id, useNavigationalProperties: true, isReadOnly: true);
            if (hardware == null)
            {
                return NotFound();
            }

            return View(hardware);
        }
        // POST: Hardwares/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            try
            {
                await hardwareService.Delete(id);
                return RedirectToPage("/Account/Manage/Hardware", new { area = "Identity" });
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddMaintenance(int id)
        {
            var hw = await hardwareService.Read(id, true, true);
            if (hw == null) return NotFound();

            return View(new MaintenanceViewModel
            {
                ParentId = id,
                ItemName = $"{hw.InventoryNumber} - {hw.Name}",
                Date = DateTime.Now
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddMaintenance(MaintenanceViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await hardwareService.AddMaintenance(vm.ParentId, vm.Description, vm.Date);
            return RedirectToAction("Details", new { id = vm.ParentId });
        }

    }
}

