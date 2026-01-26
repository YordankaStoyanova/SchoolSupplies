using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLayer;

namespace MVC.Controllers
{
    public class SoftwareController(SoftwareService softwareService) : Controller
    {
       // private readonly ILogger<SoftwareController> _logger;
        // GET: Softwares
        public async Task<IActionResult> Index(string? s, int? t)
        {
            var softwares = await softwareService.SearchCombined(s,t);
            return View(softwares);
        }

        [Authorize(Roles = "User")]
        [Authorize(Roles = "Administartor")]
        // GET: Softwares/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var software = await softwareService.Read((int)id);
            if (software == null)
            {
                return NotFound();
            }

            return View(software);
        }
  
        [Authorize(Roles = "Administartor")]
        // GET: Softwares/Create
        public IActionResult Create()
        {
            return View();
        }
      
        [Authorize(Roles = "Administartor")]
        // POST: Softwares/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,SerialNumber,Category,Type,MaintanceLogs,License")] Software software)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await softwareService.Create(software);
                    return RedirectToAction(nameof(Index));
                }
                return View(software);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
     
        [Authorize(Roles = "Administartor")]
        // GET: Softwares/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var software = await softwareService.Read((int)id);
            if (software == null)
            {
                return NotFound();
            }
            return View(software);
        }
        [Authorize(Roles = "Administartor")]
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

        [Authorize(Roles = "Administartor")]
        // GET: Softwares/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var software = await softwareService.Read((int)id);
            if (software == null)
            {
                return NotFound();
            }

            return View(software);
        }
        [Authorize(Roles = "Administartor")]
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
