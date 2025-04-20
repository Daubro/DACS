 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webbankhoahoconline.Repositories; 
using Webbankhoahoconline.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Webbankhoahoconline.Controllers
{
    public class CourseController : Controller
    {
        private readonly DataContext _dataContext;

        public CourseController(DataContext context)
        {
            _dataContext = context;
        }


        // Danh sách khóa học
        public async Task<IActionResult> Index()
        {
            var courses = await _dataContext.Courses.ToListAsync();
            return View(courses);
        }

        // Xem chi tiết khóa học
        public async Task<IActionResult> Details(int Id )
        {
            if (Id == null) return RedirectToAction("Index");

            var coursesById = _dataContext.Courses.Where(co => co.Id == Id).FirstOrDefault();
            return View(coursesById);
        }

        // Hiển thị form thêm khóa học (Chỉ Admin mới có quyền)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý thêm khóa học
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CourseModel course)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Courses.Add(course);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // Hiển thị form chỉnh sửa khóa học
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _dataContext.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // Xử lý chỉnh sửa khóa học
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, CourseModel course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _dataContext.Courses.Update(course);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // Xóa khóa học
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _dataContext.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _dataContext.Courses.Remove(course);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
