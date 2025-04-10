﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webbankhoahoconline.Repositories;
using Webbankhoahoconline.Models;

namespace Webbankhoahoconline.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataContext _context;

        public CategoryController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
    }
}
