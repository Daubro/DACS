using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webbankhoahoconline.Models;
using Webbankhoahoconline.Models.ViewModels;
using Webbankhoahoconline.Repositories;

namespace Webbankhoahoconline.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext _dataContext;

        public CartController(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<IActionResult> Index()
        {
           List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemViewModel cartVM = new()
            {
                CartItems = cartItems,
                TotalPrice = cartItems.Sum(x => x.Price * x.Quantity)
            };
            return View(cartVM);
        }

        public async Task<IActionResult> Details(int id)
        {
            
            return View();
        }
        public IActionResult Checkout()
        {
            return View("~/Views/Checkout/Index.cshtml");
        }
        public async Task<IActionResult> Add(long Id)
        {
            CourseModel course = await _dataContext.Courses.FindAsync((long)Id);
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItems = cart.Where(c=>c.CourseId==Id).FirstOrDefault();
            if (cartItems == null)
            {
                cart.Add(new CartItemModel(course));
            }
            else
            {
                cartItems.Quantity +=1;
            }
            HttpContext.Session.SetJson("Cart", cart);

            TempData["Success"] = "Thêm vào giỏ hàng thành công";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> Remove(int Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItems = cart.Where(c => c.CourseId == Id).FirstOrDefault();
            if (cartItems != null)
            {
                cart.Remove(cartItems);
            }
            HttpContext.Session.SetJson("Cart", cart);
            TempData["Success"] = "Xóa khóa học khỏi giỏ hàng thành công";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> Clear()
        {
            HttpContext.Session.Remove("Cart");
            TempData["Success"] = "Xóa giỏ hàng thành công";
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}

