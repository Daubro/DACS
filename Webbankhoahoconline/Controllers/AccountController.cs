using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Webbankhoahoconline.Areas.Admin.Repository;
using Webbankhoahoconline.Models;
using Webbankhoahoconline.Models.ViewModels;
using Webbankhoahoconline.Repositories;

namespace Webbankhoahoconline.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly DataContext _dataContext;
        public AccountController(IEmailSender emailSender, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, DataContext dataContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _dataContext = dataContext;
        }
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl});
        }
        public async Task<IActionResult> UpdateAccount()
        {
            if ((bool)!User.Identity?.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if(user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateInfoAccount(ApplicationUser user)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var userById = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userById == null)
            {
                return NotFound();
            }else
            {
                string newtoken = Guid.NewGuid().ToString();

                var passwordHasher = new PasswordHasher<ApplicationUser>();
                var passwordHash = passwordHasher.HashPassword(userById, user.PasswordHash);

                userById.PasswordHash = passwordHash;
                _dataContext.Update(userById);
                await _dataContext.SaveChangesAsync();
                TempData["Success"] = "Cập nhật thông tin thành công";
            }
            return RedirectToAction("UpdateAccount", "Account");
        }
        public async Task<IActionResult> NewPass(ApplicationUser user, string token)
        {
            var checkuser = await _userManager.Users.Where(u => u.Email == user.Email && u.Token == token).FirstOrDefaultAsync();
            if(checkuser !=null)
            {
                ViewBag.Email = checkuser.Email;
                ViewBag.Token = token;
            }
            else
            {
                TempData["Error"] = "Email không tồn tại hoặc token không hợp lệ";
                return RedirectToAction("ForgetPass", "Account");
            }
            return View();
        }
        public async Task<IActionResult> UpdateNewPassword (ApplicationUser user, string token)
        {
            var checkuser = await _userManager.Users.Where(u => u.Email == user.Email && u.Token == token).FirstOrDefaultAsync();

            if(checkuser != null)
            {
                string newtoken = Guid.NewGuid().ToString();

                var passwordHasher = new PasswordHasher<ApplicationUser>();
                var passwordHash = passwordHasher.HashPassword(checkuser, user.PasswordHash);

                checkuser.PasswordHash = passwordHash;
                checkuser.Token = newtoken;

                await _userManager.UpdateAsync(checkuser);
                TempData["Success"] = "Đổi mật khẩu thành công";
                return RedirectToAction("Login", "Account");
            }
            else
            {
                TempData["Error"] = "Email không tồn tại hoặc token không hợp lệ";
                return RedirectToAction("ForgetPass", "Account");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMailForgotPass(ApplicationUser user)
        {
            var checkMail = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (checkMail == null)
            {
                TempData["Error"] = "Email không tồn tại";
                return RedirectToAction("ForgetPass", "Account");
            }
            else
            {
                string token = Guid.NewGuid().ToString();
                //update token to user
                checkMail.Token = token;
                _dataContext.Update(checkMail);
                await _dataContext.SaveChangesAsync();
                var receiver = checkMail.Email;
                var subject = "Change password for user " + checkMail.Email;
                var message = "Click on link to change password " + "<a href='" + $"{Request.Scheme}://{Request.Host}/Account/NewPass?email=" + checkMail.Email + "&token=" + token + "'>";

                await _emailSender.SendEmailAsync(receiver, subject, message);
            }
            TempData["Success"] = "Vui lòng kiểm tra email để đổi mật khẩu";
            return RedirectToAction("ForgetPass", "Account");
        }
        public async Task<IActionResult> ForgetPass(string returnUrl)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, false);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Đăng nhập thành công";
                    var receiver = "dxk1708@gmail.com";
                    var subject = "Đăng nhập trên thiết bị thành công";
                    var message = "Đăng nhập thành công, trải nghiệm dịch vụ của chúng tôi nhé.";

                    await _emailSender.SendEmailAsync(receiver, subject, message);
                    return Redirect(loginVM.ReturnUrl ?? "/");
                }
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            }
            return View(loginVM);
        }

        public IActionResult Create()
        {
            return View();
        }
        
        public async Task<IActionResult> History()
        {
            if ((bool)!User.Identity?.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _dataContext.Orders
                .Where(o => o.UserName == userEmail).OrderByDescending(o => o.Id)
                .ToListAsync();
            ViewBag.UserEmail = userEmail;
            return View(Orders);
        }
        public async Task<IActionResult> CancelOrder(string ordercode)
        {
            if ((bool)!User.Identity?.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var order = await _dataContext.Orders.Where(o=>o.OrderCode == ordercode).FirstAsync();
                
                    order.Status = 3;
                    _dataContext.Update(order);
                    await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("An Error occurred while canceling the order.");
            }
            return RedirectToAction("History", "Account");
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser newUser = new ApplicationUser { UserName = user.UserName, Email = user.Email};
                IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Tạo tài khoản thành công";
                    return Redirect("/account/login");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                
            }
            return View(user);
        }
        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}
