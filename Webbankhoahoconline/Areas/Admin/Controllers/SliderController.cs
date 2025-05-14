using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Webbankhoahoconline.Models;
using Webbankhoahoconline.Repositories;

namespace Webbankhoahoconline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Publisher")]
    [Route("Admin/Slider")]
    public class SliderController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SliderController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Sliders.OrderByDescending(co=>co.Id).ToListAsync());
        }
        public IActionResult Create()
        {
            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(SliderModel slider)
        {
            if (ModelState.IsValid)
            {
                if (slider.ImageFile != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/sliders");
                    string imageName = Guid.NewGuid().ToString() + "_" + slider.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await slider.ImageFile.CopyToAsync(fs);
                    fs.Close();
                    slider.ImageUrl = imageName;
                }

                _dataContext.Add(slider);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm slider thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Có lỗi xảy ra trong quá trình thêm slider";
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
        }
        [Route("Edit")]
        public async Task<IActionResult> Edit(int Id)
        {
            SliderModel slider = await _dataContext.Sliders.FindAsync(Id);
            return View(slider);
        }
        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SliderModel slider)
        {
            var slider_existed = await _dataContext.Sliders.FindAsync(slider.Id);
            if (slider_existed == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                if (slider.ImageFile != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/sliders");
                    string imageName = Guid.NewGuid().ToString() + "_" + slider.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await slider.ImageFile.CopyToAsync(fs);
                    }

                    slider_existed.ImageUrl = imageName;
                }

                slider_existed.Name = slider.Name;
                slider_existed.Description = slider.Description;
                slider_existed.Status = slider.Status;

                _dataContext.Update(slider_existed);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật slider thành công";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Có lỗi xảy ra trong quá trình cập nhật slider";
            return View(slider);
        }
        [Route("Delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            SliderModel slider = await _dataContext.Sliders.FirstAsync(co => co.Id == Id);
            if (!string.Equals(slider.ImageUrl, ""))
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/sliders");
                string oldfilePathImage = Path.Combine(uploadsDir, slider.ImageUrl);
                if (System.IO.File.Exists(oldfilePathImage))
                {
                    System.IO.File.Delete(oldfilePathImage);
                }
            }
            _dataContext.Sliders.Remove(slider);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Đã xóa Slider";
            return RedirectToAction("Index");
        }
    }
}
