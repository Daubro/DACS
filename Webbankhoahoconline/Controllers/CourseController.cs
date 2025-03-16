using Microsoft.AspNetCore.Mvc;

namespace Webbankhoahoconline.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }
    }
}
