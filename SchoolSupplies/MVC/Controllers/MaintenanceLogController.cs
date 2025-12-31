using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class MaintenanceLogController : Controller
    {
        private readonly MaintenanceLogContext _logContext;
        private readonly ItemContext _itemContext;

        public MaintenanceLogController(MaintenanceLogContext logContext, ItemContext itemContext)
        {
            _logContext = logContext;
            _itemContext = itemContext;
        }

        public async Task<IActionResult> Index()
        {
            var logs = await _logContext.ReadAll(useNavigationalProperties: true, isReadOnly: true);
            return View(logs);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Items = await _itemContext.ReadAll(isReadOnly: true);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaintenanceLog log)
        {
            if (ModelState.IsValid)
            {
                await _logContext.Create(log);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Items = await _itemContext.ReadAll(isReadOnly: true);
            return View(log);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var log = await _logContext.Read(id);
            if (log == null) return NotFound();
            return View(log);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _logContext.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
