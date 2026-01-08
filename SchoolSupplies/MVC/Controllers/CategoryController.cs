using BusinessLayer.Enum;
using DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryContext _categoryContext;

        public CategoryController(CategoryContext categoryContext)
        {
            _categoryContext = categoryContext;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryContext.ReadAll(isReadOnly: true);
            return View(categories);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryContext.Create(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryContext.Read(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                await _categoryContext.Update(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryContext.Read(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryContext.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
