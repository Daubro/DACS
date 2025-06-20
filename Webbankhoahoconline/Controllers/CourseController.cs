﻿using Microsoft.AspNetCore.Mvc;
using Webbankhoahoconline.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Webbankhoahoconline.Repositories;
using Webbankhoahoconline.Models.ViewModels;
using System.Security.Claims;

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
        public async Task<IActionResult> Search(string searchTerm)
        {
            var courses = await _dataContext.Courses
                .Where(c => c.Name.Contains(searchTerm) || c.Description.Contains(searchTerm))
                .ToListAsync();
            ViewBag.Keyword = searchTerm;
            return View(courses);
        }
        

        // Xem chi tiết khóa học
        public async Task<IActionResult> Details(long id)
        {
            if (id == 0) return RedirectToAction("Index");

            var courseById = await _dataContext.Courses.Include(co => co.Reviews).FirstOrDefaultAsync(co => co.Id == id);
            //related

            var relatedCourses = await _dataContext.Courses
                .Where(co => co.CategoryId == courseById.CategoryId && co.Id != courseById.Id)
                .Take(4)
                .ToListAsync();

            ViewBag.RelatedCourses = relatedCourses;

            var viewModel = new CourseDetailsViewModel
            {
                CourseDetails = courseById,
            };
            return View(viewModel);
        }

        // Hiển thị form thêm khóa học (Chỉ Admin mới có quyền)
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý thêm khóa học
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> CommentCourse(ReviewModel review)
        {
            if(ModelState.IsValid)
            {
                var reviewEntity = new ReviewModel
                {
                    CourseId = review.CourseId,
                    Comment = review.Comment,
                    Name = review.Name,
                    Email = review.Email,
                    Star = review.Star
                };
                _dataContext.Reviews.Add(review);
                await _dataContext.SaveChangesAsync();
                TempData["Success"] = "Đánh giá của bạn đã được thêm thành công!";
                return Redirect(Request.Headers["Referer"]);
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra khi gửi đánh giá";
                List<string> errors = new List<string>();
                foreach (var valua in ModelState.Values)
                {
                    foreach (var error in valua.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return RedirectToAction("Details", new { id = review.CourseId });
            }
        }
        public async Task<IActionResult> Learn()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            string userId = userIdClaim.Value;

            var courses = await _dataContext.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.Course)
                .Where(od => od.Order.UserId == userId && od.Order.Status == 2)
                .Select(od => od.Course)
                .Distinct()
                .ToListAsync();

            if (!courses.Any())
            {
                ViewBag.Message = "Bạn chưa mua khóa học nào hoặc đơn hàng chưa được xử lý.";
                return View(new List<CourseModel>());
            }

            return View(courses);
        }


        public async Task<IActionResult> Watch(int courseId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string userId = userIdClaim.Value; // giữ nguyên chuỗi GUID

            // Kiểm tra quyền xem
            var hasAccess = await _dataContext.OrderDetails
                .Include(od => od.Order)
                .AnyAsync(od => od.Order.UserId.ToString() == userId
                             && od.CourseId == courseId
                             && od.Order.Status == 2);

            if (!hasAccess)
            {
                return Forbid();
            }

            // Lấy danh sách video theo courseId
            var videos = await _dataContext.Videos
                .Include(v => v.Course)
                .Where(v => v.CourseId == courseId)
                .ToListAsync();

            if (!videos.Any())
            {
                ViewBag.Message = "Chưa có video nào cho khóa học này.";
            }

            return View(videos); // Truyền danh sách video cho View
        }



        // Xóa khóa học
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
