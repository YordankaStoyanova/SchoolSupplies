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

        // GET: Hardwares
        public async Task<IActionResult> Index(string? s,ItemStatus? t, int? r)
        {
            var hardwares = await hardwareService.SearchCombined(s,t,r);
            return View(hardwares);
        }



        [Authorize(Roles = "Administrator,User")]
        // GET: Hardwares/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardware = await hardwareService.Read(id.Value);
            if (hardware == null)
            {
                return NotFound();
            }

            return View(hardware);
        }
        [Authorize(Roles = "Administrator")]
        // GET: Hardwares/Create
        public async  Task<IActionResult> Create()
        {
            ViewBag.SoftwareList = new SelectList(await softwareService.ReadAll(false,true), "Id", "Name");
            ViewBag.Types = new SelectList(await typeContext.ReadAll(false,true), "Id", "Name");
            ViewBag.Rooms = new SelectList(await roomContext.ReadAll(false,true), "Id", "Name");
            return View();
        }
        [Authorize(Roles = "Administrator")]
        // POST: Hardwares/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,InventoryNumber,SerialNumber,TypeId,RoomId,Status,SoftwareIds")] HardwareViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await hardwareService.Create(model);
                    return RedirectToPage("/Account/Manage/Hardware", new { area = "Identity" });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        [Authorize(Roles = "Administrator")]
        // GET: Hardwares/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardware = await hardwareService.Read(id.Value);
            if (hardware == null)
            {
                return NotFound();
            }
            return View(hardware);
        }
        [Authorize(Roles = "Administrator")]
        // POST: Hardwares/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,InventoryNumber,SerialNumber,Category,Type,Room,Status,Softwares")] Hardware hardware)
        {
            if (id != hardware.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await hardwareService.Update(hardware);
                    return RedirectToPage("/Account/Manage/Hardware", new { area = "Identity" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }
            return View(hardware);
           // return RedirectToAction(nameof(Index)); ?
        }

        [Authorize(Roles = "Administrator")]
        // GET: Hardwares/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardware = await hardwareService.Read(id.Value);
            if (hardware == null)
            {
                return NotFound();
            }

            return View(hardware);
        }
        [Authorize(Roles = "Administrator")]
        // POST: Hardwares/Delete/5
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

    }
}

