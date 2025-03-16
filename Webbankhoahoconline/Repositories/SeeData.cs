using Microsoft.EntityFrameworkCore;
using Webbankhoahoconline.Models;

namespace Webbankhoahoconline.Repositories
{
    public class SeeData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if (!_context.Courses.Any())
            {
                CategoryModel apple = new CategoryModel { Name = "Apple", Slug = "apple", Description = "2222", Status = 1 };
                CategoryModel samsung = new CategoryModel { Name = "Samsung", Slug = "samsung", Description = "3232", Status = 1 };
                CategoryModel dell = new CategoryModel { Name = "Dell", Slug = "dell", Description = "1111", Status = 1 };
                CategoryModel msi = new CategoryModel { Name = "MSI", Slug = "msi", Description = "1231", Status = 1 };
                _context.Courses.AddRange(
                    new CourseModel { Name = "Macbook", Slug = "macbook", Description = "Iphone 6", Price = 1000, Category = apple, Status = 1, ImageUrl = "default.jpg" },
                    new CourseModel { Name = "PC", Slug = "pc", Description = "Iphone 6", Price = 1000, Category = apple, Status = 1, ImageUrl = "default.jpg" }

                    );
                    _context.SaveChanges();
                }
        }
    } 
}
