using System.Data.Entity;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Webbankhoahoconline.Models;
using Webbankhoahoconline.Repositories;

namespace Webbankhoahoconline.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _dataContext = context;
        }

        public IActionResult Index()
        {
            var courses = _dataContext.Courses.Include("Category").Include("Instructor").ToList();
            return View(courses);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
