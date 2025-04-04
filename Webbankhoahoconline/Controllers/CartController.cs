using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webbankhoahoconline.Repositories;

namespace Webbankhoahoconline.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext _context;

        public CartController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var carts = await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Course).ToListAsync();
            return View(carts);
        }

        public async Task<IActionResult> Details(int id)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Course)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }
        public IActionResult Checkout()
        {
            return View("~/Views/Checkout/Index.cshtml");
        }
    }
}

