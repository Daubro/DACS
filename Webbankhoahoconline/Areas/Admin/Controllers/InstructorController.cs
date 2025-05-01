using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webbankhoahoconline.Models;
using Webbankhoahoconline.Repositories;

namespace Webbankhoahoconline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class InstructorController : Controller
    {
        private readonly DataContext _dataContext;
        public InstructorController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Instructors.OrderByDescending(co => co.Id).ToListAsync());
        }
        public async Task<IActionResult> Edit(int Id)
        {
            var instructor = await _dataContext.Instructors.FirstOrDefaultAsync(co => co.Id == Id);         
            return View(instructor);
        }
        // GET: Create Instructor
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create Instructor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InstructorModel instructor, IFormFile AvatarImage)
        {
            if (ModelState.IsValid)
            {
                if (AvatarImage != null && AvatarImage.Length > 0)
                {
                    // Tạo tên file duy nhất
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(AvatarImage.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatarurl", fileName);

                    // Lưu ảnh vào thư mục
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await AvatarImage.CopyToAsync(stream);
                    }

                    // Tạo URL cho ảnh
                    string imageUrl = "/avatarurl/" + fileName;
                    instructor.AvatarUrl = imageUrl;  // Lưu URL vào thuộc tính AvatarUrl
                }

                // Tạo Slug cho giảng viên (tùy chọn)
                instructor.Slug = instructor.Name.Replace(" ", "-").ToLower();

                // Lưu giảng viên vào cơ sở dữ liệu
                _dataContext.Add(instructor);
                await _dataContext.SaveChangesAsync();

                TempData["success"] = "Thêm giảng viên thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(instructor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(InstructorModel instructor, IFormFile AvatarImage)
        {
            if (ModelState.IsValid)
            {
                if (AvatarImage != null && AvatarImage.Length > 0)
                {
                    // Tạo tên file duy nhất
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(AvatarImage.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatarurl", fileName);

                    // Lưu ảnh vào thư mục
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await AvatarImage.CopyToAsync(stream);
                    }

                    // Tạo URL cho ảnh
                    string imageUrl = "/avatarurl/" + fileName;
                    instructor.AvatarUrl = imageUrl;  // Lưu URL vào thuộc tính AvatarUrl
                }

                // Tạo Slug cho giảng viên (tùy chọn)
                instructor.Slug = instructor.Name.Replace(" ", "-").ToLower();

                // Lưu giảng viên vào cơ sở dữ liệu
                _dataContext.Update(instructor);
                await _dataContext.SaveChangesAsync();

                TempData["success"] = "Cập nhật giảng viên thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(instructor);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            InstructorModel instructor = await _dataContext.Instructors.FirstAsync(co => co.Id == Id);


            _dataContext.Instructors.Remove(instructor);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Đã xóa Giảng viên";
            return RedirectToAction("Index");
        }
    }
}
