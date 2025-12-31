using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class RoomController : Controller
    {
        private readonly RoomContext _roomContext;

        public RoomController(RoomContext roomContext)
        {
            _roomContext = roomContext;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _roomContext.ReadAll(isReadOnly: true);
            return View(rooms);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room room)
        {
            if (ModelState.IsValid)
            {
                await _roomContext.Create(room);
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var room = await _roomContext.Read(id);
            if (room == null) return NotFound();
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Room room)
        {
            if (id != room.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                await _roomContext.Update(room);
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var room = await _roomContext.Read(id);
            if (room == null) return NotFound();
            return View(room);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _roomContext.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
