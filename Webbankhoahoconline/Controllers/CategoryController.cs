using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webbankhoahoconline.Repositories;
using Webbankhoahoconline.Models;

namespace Webbankhoahoconline.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;

        public CategoryController(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<IActionResult> Index(string Slug= "")
        {
            CategoryModel category = _dataContext.Categories.Where(c => c.Slug == Slug).FirstOrDefault();
            if (category == null) return RedirectToAction("Index");

            var coursesByCategory = _dataContext.Courses.Where(co => co.CategoryId == category.Id);
         
            return View(await coursesByCategory.OrderByDescending(co => co.Id).ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _dataContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
    }
}
