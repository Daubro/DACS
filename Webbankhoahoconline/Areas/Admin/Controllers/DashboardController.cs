using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webbankhoahoconline.Migrations;
using Webbankhoahoconline.Repositories;

namespace Webbankhoahoconline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DashboardController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var count_course = _dataContext.Courses.Count();
            var count_category = _dataContext.Categories.Count();
            var count_order = _dataContext.Orders.Count();
            var count_user = _dataContext.Users.Count();
            ViewBag.CountCourse = count_course;
            ViewBag.CountCategory = count_category;
            ViewBag.CountOrder = count_order;
            ViewBag.CountUser = count_user;
            return View();
        }
        [HttpPost]
        [Route("GetChartData")]
        public IActionResult GetChartData()
        {
            var data = _dataContext.Statisticals.Select(s => new
            {
                date = s.CreateDate.ToString("yyyy-MM-dd"),
                sold = s.Sold,
                quantity = s.Quantity,
                revenue = s.Revenue,
                profit = s.Profit
            }).ToList();
            return Json(data);
        }
        [HttpPost]
        [Route("GetChartDataBySelect")]
        public IActionResult GetChartDataBySelect(DateTime startDate,DateTime endDate)
        {
            var data = _dataContext.Statisticals
            .Where(s => s.CreateDate >= startDate && s.CreateDate <= endDate)
            .Select(s => new
            {
                date = s.CreateDate.ToString("yyyy-MM-dd"),
                sold = s.Sold,
                quantity = s.Quantity,
                revenue = s.Revenue,
                profit = s.Profit
            }).ToList();
            return Json(data);
        }
        [HttpPost]
        public IActionResult FilterData(DateTime? fromDate, DateTime? toDate)
        {
            var query = _dataContext.Statisticals.AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(s => s.CreateDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(s => s.CreateDate <= toDate.Value);
            }
            var data = query
                .Select(s => new
                {
                    date = s.CreateDate.ToString("yyyy-MM-dd"),
                    sold = s.Sold,
                    quantity = s.Quantity,
                    revenue = s.Revenue,
                    profit = s.Profit
                }).ToList();
            return Json(data);
        }
    }
}
