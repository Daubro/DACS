using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Webbankhoahoconline.Areas.Admin.Repository;
using Webbankhoahoconline.Models;
using Webbankhoahoconline.Repositories;
using Webbankhoahoconline.Services.Momo;

namespace Webbankhoahoconline.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IEmailSender _emailSender;
        private readonly IMomoService _momoService;

        public CheckoutController(IEmailSender emailSender, DataContext context, IMomoService momoService)
        {
            _dataContext = context;
            _emailSender = emailSender;
            _momoService = momoService;
        }

        public async Task<IActionResult> Checkout(string OrderId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userEmail == null || userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var ordercode = Guid.NewGuid().ToString();
            var orderItem = new OrderModel
            {
                OrderCode = ordercode,
                UserName = userEmail,
                UserId = userId, // Gán UserId cho order
                PaymentMethod = OrderId ?? "COD",
                OrderDate = DateTime.Now,
                Status = 1
            };

            _dataContext.Add(orderItem);
            await _dataContext.SaveChangesAsync(); // Lưu để lấy Id của order

            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            foreach (var cart in cartItems)
            {
                var orderDetail = new OrderDetailModel
                {
                    UserName = userEmail,
                    OrderId = orderItem.Id,  // Gán khóa ngoại OrderId đúng
                    OrderCode = orderItem.OrderCode,  // Gán OrderCode đúng
                    CourseId = cart.CourseId,
                    Price = cart.Price,
                    Quantity = 1
                };
                _dataContext.Add(orderDetail);
            }
            await _dataContext.SaveChangesAsync();


            HttpContext.Session.Remove("Cart");

            await _emailSender.SendEmailAsync(userEmail, "Đặt hàng thành công", "Đặt hàng thành công, trải nghiệm dịch vụ của chúng tôi nhé.");
            TempData["Success"] = "Đơn hàng đã được tạo! Vui lòng đợi kiểm tra đơn hàng";

            return RedirectToAction("Index", "Cart");
        }

        [HttpGet]
        public async Task<IActionResult> PaymentCallBack(MomoInfoModel model)
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            var requestQuery = HttpContext.Request.Query;

            if (requestQuery["resultCode"] != "0")
            {
                var newMomoInsert = new MomoInfoModel
                {
                    OrderId = requestQuery["orderId"],
                    FullName = User.FindFirstValue(ClaimTypes.Email),
                    Amount = decimal.Parse(requestQuery["Amount"]),
                    OrderInfo = requestQuery["orderInfo"],
                    DatePaid = DateTime.Now
                };
                _dataContext.Add(newMomoInsert);
                await _dataContext.SaveChangesAsync();

                var checkoutResult = await Checkout(requestQuery["orderId"]);
            }
            else
            {
                TempData["Error"] = "Thanh toán thất bại!";
                return RedirectToAction("Index", "Cart");
            }

            return View(response);
        }

        public async Task<IActionResult> CheckoutCourse(long courseId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userEmail == null || userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var course = _dataContext.Courses.FirstOrDefault(co => co.Id == courseId);
            if (course == null)
            {
                TempData["Error"] = "Khóa học không tồn tại!";
                return RedirectToAction("Index", "Cart");
            }

            var ordercode = Guid.NewGuid().ToString();
            var orderItem = new OrderModel
            {
                OrderCode = ordercode,
                UserName = userEmail,
                UserId = userId, // Gán UserId cho order
                PaymentMethod = "COD",
                OrderDate = DateTime.Now,
                Status = 1
            };

            _dataContext.Add(orderItem);
            await _dataContext.SaveChangesAsync();

            var orderDetail = new OrderDetailModel
            {
                UserName = userEmail,
                OrderId = orderItem.Id, // Gán khóa ngoại OrderId đúng
                OrderCode = orderItem.OrderCode, // Gán OrderCode đúng
                CourseId = courseId,
                Price = course.Price,
                Quantity = 1
            };
            _dataContext.Add(orderDetail);
            await _dataContext.SaveChangesAsync();


            TempData["Success"] = "Đơn hàng đã được tạo! Vui lòng đợi kiểm tra đơn hàng";
            return RedirectToAction("Index", "Cart");
        }
    }
}
