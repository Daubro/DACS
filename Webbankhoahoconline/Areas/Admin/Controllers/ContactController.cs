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
    [Authorize(Roles = "Admin")]
    public class ContactController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ContactController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var contacts = _dataContext.Contacts.ToList();
            return View(contacts);
        }
        public async Task<IActionResult> Edit()
        {
            ContactModel contact = await _dataContext.Contacts.FirstOrDefaultAsync();
            return View(contact);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(ContactModel contact)
        {
            var existed_contact = _dataContext.Contacts.FirstOrDefault();

            if (ModelState.IsValid)
            {
                
                    if (contact.ImageFile != null)
                    {
                        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/logo");
                        string imageName = Guid.NewGuid().ToString() + "_" + contact.ImageFile.FileName;
                        string filePath = Path.Combine(uploadsDir, imageName);

                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        await contact.ImageFile.CopyToAsync(fs);
                        fs.Close();
                        existed_contact.LogoImg = imageName;
                    }
                existed_contact.Name = contact.Name;
                existed_contact.Email = contact.Email;
                existed_contact.Description = contact.Description;
                existed_contact.Phone = contact.Phone;
                existed_contact.Map = contact.Map;
                
                _dataContext.Update(existed_contact);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật thông tin web thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Có lỗi xảy ra trong quá trình update thông tin";
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

            return View(contact);
        }
    }

}
