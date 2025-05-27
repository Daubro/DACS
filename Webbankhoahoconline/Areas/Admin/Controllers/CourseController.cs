
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Webbankhoahoconline.Models;
using Webbankhoahoconline.Models.ViewModels;
using Webbankhoahoconline.Repositories;

namespace Webbankhoahoconline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Publisher,Author")]
    public class CourseController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HttpClient _httpClient;

        public CourseController(DataContext context, IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
            _httpClient = httpClientFactory.CreateClient();
        }
        // Danh sách khóa học
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Courses.OrderByDescending(co => co.Id).Include(co => co.Category).Include(co => co.Instructor).ToListAsync());
        }
        // Xem chi tiết khóa học
        public IActionResult Details(long id)
        {
            var course = _dataContext.Courses.FirstOrDefault(co => co.Id == id);
            if (course == null)
                return NotFound();

            var videos = _dataContext.Videos.Where(v => v.CourseId == id).ToList();

            var model = new CourseDetailsViewModel
            {
                CourseDetails = course,
                Videos = videos,
                Comment = "",
                Name = "",
                Email = ""
            };

            return View(model);
        }


        // Hiển thị form thêm khóa học (Chỉ Admin mới có quyền)
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id","Name");
            ViewBag.Instructors = new SelectList(_dataContext.Instructors, "Id", "Name");
            return View();
        }
        // Xử lý thêm khóa học
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Create(CourseModel course)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", course.CategoryId);
            ViewBag.Instructors = new SelectList(_dataContext.Instructors, "Id", "Name", course.InstructorId);
    
            if(ModelState.IsValid)
            {
                course.Slug = course.Name.Replace(" ", "-");
                var slug = await _dataContext.Courses.FirstOrDefaultAsync(co => co.Slug == course.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Khóa học đã tồn tại");
                    return View(course);
                }
                else
                {
                    if (course.ImageFile != null)
                    {
                        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        string imageName = Guid.NewGuid().ToString() + "_" + course.ImageFile.FileName;
                        string filePath = Path.Combine(uploadsDir, imageName);

                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        await course.ImageFile.CopyToAsync(fs);
                        fs.Close();
                        course.ImageUrl = imageName;
                    }
                }
                _dataContext.Add(course);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm khóa học thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"]="Có lỗi xảy ra trong quá trình thêm khóa học";
                List<string> errors = new List<string>();
                foreach (var error in ModelState.Values)
                {
                    foreach (var err in error.Errors)
                    {
                        errors.Add(err.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }

            return View(course);
        }
        public async Task<IActionResult> Edit(long Id)
        {
            CourseModel course = await _dataContext.Courses.FirstAsync(co => co.Id == Id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", course.CategoryId);
            ViewBag.Instructors = new SelectList(_dataContext.Instructors, "Id", "Name", course.InstructorId);

            var videos = _dataContext.Videos.Where(v => v.CourseId == Id).ToList();
            ViewBag.Videos = videos;

            return View(course);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(CourseModel course)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", course.CategoryId);
            ViewBag.Instructors = new SelectList(_dataContext.Instructors, "Id", "Name", course.InstructorId);

            var existed_course = _dataContext.Courses.Find(course.Id); 

            if (ModelState.IsValid)
            {
                course.Slug = course.Name.Replace(" ", "-");
                { 
                    if (course.ImageFile != null)
                    {
                        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        string imageName = Guid.NewGuid().ToString() + "_" + course.ImageFile.FileName;
                        string filePath = Path.Combine(uploadsDir, imageName);

                        string oldfilePathImage = Path.Combine(uploadsDir, existed_course.ImageUrl);

                        try
                        {
                            if (System.IO.File.Exists(oldfilePathImage))
                            {
                                System.IO.File.Delete(oldfilePathImage);
                            }
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Có lỗi xảy ra trong quá trình xóa ảnh");
                        }

                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        await course.ImageFile.CopyToAsync(fs);
                        fs.Close();
                        existed_course.ImageUrl = imageName;
                    }
                    existed_course.Name = course.Name;
                    existed_course.Description = course.Description;
                    existed_course.Price = course.Price;
                    existed_course.CategoryId = course.CategoryId;
                    existed_course.InstructorId = course.InstructorId;
                }
                _dataContext.Update(existed_course);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật khóa học thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Có lỗi xảy ra trong quá trình thêm khóa học";
                List<string> errors = new List<string>();
                foreach (var error in ModelState.Values)
                {
                    foreach (var err in error.Errors)
                    {
                        errors.Add(err.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }

            return View(course);
        }
        public async Task<IActionResult> Delete(long Id)
        {
            CourseModel course = await _dataContext.Courses.FirstAsync(co => co.Id == Id);
            if (!string.Equals(course.ImageUrl, ""))
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string oldfilePathImage = Path.Combine(uploadsDir, course.ImageUrl);
                if (System.IO.File.Exists(oldfilePathImage))
                {
                    System.IO.File.Delete(oldfilePathImage);
                }
            }
            _dataContext.Courses.Remove(course);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Đã xóa Khóa học";
            return RedirectToAction("Index");
        }
        [Route("AddQuantity")]
        [HttpGet]
        public async Task<IActionResult> AddQuantity(int Id)
        {
            var coursebyquantity = await _dataContext.CourseQuantities.Where(cq => cq.CourseId == Id).ToListAsync();
            ViewBag.CourseByQuantity = coursebyquantity;
            ViewBag.Id = Id;    
            return View();
        }
        [Route("CourseQuantity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult CourseQuantity(CourseQuantityModel courseQuantityModel)
        {
            
                var course =  _dataContext.Courses.Find(courseQuantityModel.CourseId);
                if (course == null)
                {
                    return NotFound();
                }
                course.Quantity += courseQuantityModel.Quantity;

            courseQuantityModel.Quantity = courseQuantityModel.Quantity;
            courseQuantityModel.CourseId = courseQuantityModel.CourseId;
            courseQuantityModel.CreateDate = DateTime.Now;

            _dataContext.Add(courseQuantityModel);
            _dataContext.SaveChanges();
            TempData["success"] = "Cập nhật số lượng khóa học thành công";
            return RedirectToAction("AddQuantity", "Course", new {Id= courseQuantityModel.CourseId});
        }
    }
    
}
