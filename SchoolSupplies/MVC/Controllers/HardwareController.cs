using BusinessLayer;
using BusinessLayer.Enum;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceLayer;
using System.Composition;

namespace MVC.Controllers
{
    public class HardwareController(HardwareService hardwareService) : Controller
    {

        // GET: Hardwares
        public async Task<IActionResult> Index(string? s,ItemStatus? t, string? r)
        {
            var hardwares = await hardwareService.SearchCombined(s,t,r);
            return View(hardwares);
        }



        [Authorize(Roles = "User")]
        [Authorize(Roles = "Administartor")]
        // GET: Hardwares/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardware = await hardwareService.Read((int)id);
            if (hardware == null)
            {
                return NotFound();
            }

            return View(hardware);
        }
        [Authorize(Roles = "Administartor")]
        // GET: Hardwares/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Administartor")]
        // POST: Hardwares/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,InventoryNumber,SerialNumber,Category,Type,Room,Status,Softwares")] Hardware hardware)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await hardwareService.Create(hardware);
                    return RedirectToAction(nameof(Index));
                }
                return View(hardware);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        [Authorize(Roles = "Administartor")]
        // GET: Hardwares/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardware = await hardwareService.Read((int)id);
            if (hardware == null)
            {
                return NotFound();
            }
            return View(hardware);
        }
        [Authorize(Roles = "Administartor")]
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
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }
            return View(hardware);
           // return RedirectToAction(nameof(Index)); ?
        }

        [Authorize(Roles = "Administartor")]
        // GET: Hardwares/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardware = await hardwareService.Read((int)id);
            if (hardware == null)
            {
                return NotFound();
            }

            return View(hardware);
        }
        [Authorize(Roles = "Administartor")]
        // POST: Hardwares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            try
            {
                await hardwareService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

    }
}

