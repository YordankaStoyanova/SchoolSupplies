using Microsoft.AspNetCore.Mvc;
using BusinessLayer;
using MVC.Models;
using DataLayer;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessLayer.Enum;

namespace MVC.Controllers
{
    public class ItemController : Controller
    {
        private readonly SoftwareContext _itemContext;
        private readonly CategoryContext _categoryContext;
        private readonly RoomContext _roomContext;
        private readonly IdentityContext _identityContext;

        public ItemController(SoftwareContext itemContext, CategoryContext categoryContext, RoomContext roomContext, IdentityContext identityContext)
        {
            _itemContext = itemContext;
            _categoryContext = categoryContext;
            _roomContext = roomContext;
            _identityContext = identityContext;
        }

        // GET: Items
        public async Task<IActionResult> Index(string search = "", ItemStatus? status = null)
        {
            var items = await _itemContext.ReadAll(true);

            if (!string.IsNullOrEmpty(search))
                items = items.Where(i => i.Name.Contains(search) ||
                                         i.InventoryNumber.Contains(search) ||
                                         i.SerialNumber.Contains(search))
                             .ToList();

            if (status.HasValue)
                items = items.Where(i => i.Status == status.Value).ToList();

            return View(items);
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var item = await _itemContext.Read(id, true);
            if (item == null) return NotFound();
            return View(item);
        }

        // GET: Items/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _categoryContext.ReadAll(), "Id", "Name");
            ViewBag.Rooms = new SelectList(await _roomContext.ReadAll(), "Id", "RoomNumber");
            ViewBag.Users = new SelectList(await _identityContext.GetAllUsersAsync(), "Id", "Name");
            return View();
        }

        // POST: Items/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Item item)
        {
            if (ModelState.IsValid)
            {
                await _itemContext.Create(item);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new SelectList(await _categoryContext.ReadAll(), "Id", "Name", item.CategoryId);
            ViewBag.Rooms = new SelectList(await _roomContext.ReadAll(), "Id", "RoomNumber", item.RoomId);
            ViewBag.Users = new SelectList(await _identityContext.GetAllUsersAsync(), "Id", "Name", item.UserId);
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _itemContext.Read(id);
            if (item == null) return NotFound();
            ViewBag.Categories = new SelectList(await _categoryContext.ReadAll(), "Id", "Name", item.CategoryId);
            ViewBag.Rooms = new SelectList(await _roomContext.ReadAll(), "Id", "RoomNumber", item.RoomId);
            ViewBag.Users = new SelectList(await _identityContext.GetAllUsersAsync(), "Id", "Name", item.UserId);
            return View(item);
        }

        // POST: Items/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Item item)
        {
            if (id != item.Id) return NotFound();
            if (ModelState.IsValid)
            {
                await _itemContext.Update(item);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new SelectList(await _categoryContext.ReadAll(), "Id", "Name", item.CategoryId);
            ViewBag.Rooms = new SelectList(await _roomContext.ReadAll(), "Id", "RoomNumber", item.RoomId);
            ViewBag.Users = new SelectList(await _identityContext.GetAllUsersAsync(), "Id", "Name", item.UserId);
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _itemContext.Read(id, true);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _itemContext.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Items/AssignUser
        [HttpPost]
        public async Task<IActionResult> AssignUser(int itemId, string userId)
        {
            await _itemContext.AssignUser(itemId, userId);
            return RedirectToAction(nameof(Edit), new { id = itemId });
        }

        // POST: Items/ChangeRoom
        [HttpPost]
        public async Task<IActionResult> ChangeRoom(int itemId, int roomId)
        {
            await _itemContext.ChangeRoom(itemId, roomId);
            return RedirectToAction(nameof(Edit), new { id = itemId });
        }
    }

}

