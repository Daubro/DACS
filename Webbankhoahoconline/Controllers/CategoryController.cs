using Microsoft.AspNetCore.Mvc;

namespace Webbankhoahoconline.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
