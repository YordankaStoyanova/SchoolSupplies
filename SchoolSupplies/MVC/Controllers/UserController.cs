using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace MVC.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {

        private readonly IdentityContext _identityContext;

        public UserController(IdentityContext identityContext)
        {
            _identityContext = identityContext;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var users = await _identityContext.GetAllUsersAsync();
            return View(users);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var user = await _identityContext.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(UserRole)));
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string email, string name, string password, UserRole role)
        {
            if (ModelState.IsValid)
            {
                await _identityContext.RegisterAsync(email, name, password, role);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(UserRole)), role);
            return View();
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var user = await _identityContext.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(UserRole)));
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string name, UserRole role)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var user = await _identityContext.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            await _identityContext.UpdateUserAsync(id, name);
            await _identityContext.ChangeUserRoleAsync(id, role);

            return RedirectToAction(nameof(Index));
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var user = await _identityContext.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _identityContext.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
