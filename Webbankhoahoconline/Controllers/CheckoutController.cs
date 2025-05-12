using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Webbankhoahoconline.Areas.Admin.Repository;
using Webbankhoahoconline.Models;
using Webbankhoahoconline.Repositories;

namespace Webbankhoahoconline.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IEmailSender _emailSender;

        public CheckoutController(IEmailSender emailSender, DataContext context)
        {
            _dataContext = context;
            _emailSender = emailSender;
        }
        public async Task<IActionResult> Checkout()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if(userEmail == null)
            {
            return RedirectToAction("Login", "Account");
            }
            else
            {
            var ordercode = Guid.NewGuid().ToString();
            var orderItem = new OrderModel();
                orderItem.OrderCode = ordercode;
                orderItem.UserName = userEmail;
                orderItem.OrderDate = DateTime.Now;
                orderItem.Status = 1;
                _dataContext.Add(orderItem);
                _dataContext.SaveChanges();
                List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
                foreach (var cart in cartItems)
                {
                    var orderDetail = new OrderDetailModel();
                    orderDetail.UserName = userEmail;
                    orderDetail.OrderCode = ordercode;
                    orderDetail.CourseId = cart.CourseId;
                    orderDetail.Price = cart.Price;
                    orderDetail.Quantity = 1;
                    _dataContext.Add(orderDetail);
                    _dataContext.SaveChanges();
                }
                HttpContext.Session.Remove("Cart");
                //Send email
                var receiver = userEmail;
                var subject = "Đặt hàng thành công";
                var message = "Đặt hàng thành công, trải nghiệm dịch vụ của chúng tôi nhé.";

                await _emailSender.SendEmailAsync(receiver, subject, message);

                TempData["Success"] = "Đơn hàng đã được tạo ! Vui lòng đợi kiểm tra đơn hàng";
                return RedirectToAction("Index", "Cart");
            }
            return View();
        }

    }

}
