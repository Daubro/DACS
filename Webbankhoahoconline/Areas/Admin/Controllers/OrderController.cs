using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webbankhoahoconline.Models;
using Webbankhoahoconline.Repositories;

namespace Webbankhoahoconline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly DataContext _dataContext;
        public OrderController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Orders.OrderByDescending(co => co.Id).ToListAsync());
        }
        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var DetailOrder = await _dataContext.OrderDetails.Include(od => od.Course).Where(od => od.OrderCode == ordercode).ToListAsync();
            var Order = _dataContext.Orders.Where(o => o.OrderCode == ordercode).First();
            ViewBag.Status = Order.Status;
            return View(DetailOrder);
        }
        [HttpPost]
        [Route("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(string ordercode, int status)
        {
            var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);
            if (order == null)
            {
                return NotFound();
            }
            order.Status = status;
            try
            {
                await _dataContext.SaveChangesAsync();
                return Ok(new { success = true, message = "Order status updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the order status: " );
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string ordercode)
        {
            if (string.IsNullOrEmpty(ordercode))
            {
                TempData["error"] = "Không tìm thấy mã đơn hàng";
                return RedirectToAction("Index");
            }

            var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);
            if (order == null)
            {
                TempData["error"] = "Đơn hàng không tồn tại";
                return RedirectToAction("Index");
            }

            // Xóa chi tiết đơn hàng trước (nếu có)
            var orderDetails = await _dataContext.OrderDetails
                .Where(od => od.OrderCode == ordercode)
                .ToListAsync();

            if (orderDetails.Any())
            {
                _dataContext.OrderDetails.RemoveRange(orderDetails);
            }

            _dataContext.Orders.Remove(order);

            try
            {
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Đã xóa đơn hàng thành công";
            }
            catch (Exception)
            {
                TempData["error"] = "Có lỗi xảy ra khi xóa đơn hàng";
            }

            return RedirectToAction("Index");
        }


    }
}