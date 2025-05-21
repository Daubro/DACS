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

        public async Task<IActionResult> Index(string Slug= "",string sort_by="",string startprice = "", string endprice = "")
        {
            CategoryModel category = _dataContext.Categories.Where(c => c.Slug == Slug).FirstOrDefault();
            if (category == null) 
            {
                return RedirectToAction("Index"); 
            }
            ViewBag.Slug = Slug;
            IQueryable<CourseModel> coursesByCategory = _dataContext.Courses.Where(co => co.CategoryId == category.Id);
            var count = await coursesByCategory.CountAsync();
            if (count > 0)
            {
                // Áp dụng sắp xếp dựa trên tham số sort_by
                if (sort_by == "price_increase")
                {
                    coursesByCategory = coursesByCategory.OrderBy(co => co.Price);
                }
                else if (sort_by == "price_decrease")
                {
                    coursesByCategory = coursesByCategory.OrderByDescending(co => co.Price);
                }
                else if (sort_by == "price_newest")
                {
                    coursesByCategory = coursesByCategory.OrderByDescending(co => co.Id);
                }
                else if (sort_by == "price_oldest")
                {
                    coursesByCategory = coursesByCategory.OrderBy(co => co.Id);
                }
                else if (startprice != "" && endprice !="")
                {
                    decimal startPriceValue;
                    decimal endPriceValue;

                    if(decimal.TryParse(startprice, out startPriceValue) && decimal.TryParse(endprice, out endPriceValue))
                    {
                        coursesByCategory = coursesByCategory.Where(co => co.Price >= startPriceValue && co.Price <= endPriceValue);
                    }
                    else
                    {
                        coursesByCategory = coursesByCategory.OrderByDescending(co => co.Id);
                    }
                }
                else
                {
                    coursesByCategory = coursesByCategory.OrderByDescending(co => co.Id);
                }
            }

            return View(await coursesByCategory.ToListAsync());
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
