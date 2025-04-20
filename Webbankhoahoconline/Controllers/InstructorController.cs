using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webbankhoahoconline.Models;
using Webbankhoahoconline.Repositories;

namespace Webbankhoahoconline.Controllers
{
    public class InstructorController : Controller  
    {
        private readonly DataContext _dataContext;

        public InstructorController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index(string Slug = "")
        {
            InstructorModel instructor = _dataContext.Instructors.Where(c => c.Slug == Slug).FirstOrDefault();

            if (instructor == null) return RedirectToAction("Index");

            var coursesByInstructor = _dataContext.Courses.Where(co => co.InstructorId == instructor.Id);

            return View(await coursesByInstructor.OrderByDescending(co => co.Id).ToListAsync());
        }
    }
}
