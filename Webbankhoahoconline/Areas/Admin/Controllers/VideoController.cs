using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webbankhoahoconline.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Webbankhoahoconline.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Webbankhoahoconline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Publisher,Author")]
    public class VideoController : Controller
    {
        private readonly DataContext _dataContext;

        public VideoController(DataContext context)
        {
            _dataContext = context;
        }
        public IActionResult Index()
        {
            var videosWithCourses = _dataContext.Videos
                .Include(v => v.Course)
                .OrderBy(v => v.Course.Name)      // Sắp xếp video theo tên khóa học
                .ThenBy(v => v.UploadDate)       // Sắp xếp video theo ngày tải lên trong mỗi khóa học
                .ToList();

            return View(videosWithCourses);
        }




        // GET: Admin/Video/Create?courseId=123
        public IActionResult Create(long courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }

        // POST: Admin/Video/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(long courseId, IFormFile videoFile)
        {
            if (videoFile == null || videoFile.Length == 0)
            {
                ModelState.AddModelError("", "Vui lòng chọn file video.");
                ViewBag.CourseId = courseId;
                return View();
            }

            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    using (var stream = videoFile.OpenReadStream())
                    {
                        var fileContent = new StreamContent(stream);
                        content.Add(fileContent, "file", videoFile.FileName);

                        var backendUploadUrl = "http://localhost:5035/api/VideoUpload/upload";

                        var response = await client.PostAsync(backendUploadUrl, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var uploadResult = System.Text.Json.JsonSerializer.Deserialize<UploadResult>(json);

                            if (uploadResult != null && !string.IsNullOrEmpty(uploadResult.url))
                            {
                                var video = new VideoModel
                                {
                                    FileName = videoFile.FileName,
                                    Url = uploadResult.url,
                                    UploadDate = DateTime.Now,
                                    CourseId = courseId
                                };

                                _dataContext.Videos.Add(video);
                                await _dataContext.SaveChangesAsync();

                                TempData["success"] = "Thêm video thành công!";
                                return RedirectToAction("Edit", "Course", new { Id = courseId, area = "Admin" });
                            }
                            else
                            {
                                ModelState.AddModelError("", "Backend trả về URL video không hợp lệ.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Lỗi khi upload video lên backend.");
                        }
                    }
                }
            }

            ViewBag.CourseId = courseId;
            return View();
        }

        private class UploadResult
        {
            public string url { get; set; }
        }
        public IActionResult Delete(int id)
        {
            var video = _dataContext.Videos.FirstOrDefault(v => v.Id == id);
            if (video == null)
            {
                return NotFound();
            }

            // Lấy courseId để redirect về Edit Course sau khi xóa
            var courseId = video.CourseId;

            // Xóa video
            _dataContext.Videos.Remove(video);
            _dataContext.SaveChanges();
            TempData["success"] = "Đã xóa Video";

            // Redirect về trang Edit Course để thấy danh sách video cập nhật
            return RedirectToAction("Edit", "Course", new { id = courseId });
        }
    }
}
