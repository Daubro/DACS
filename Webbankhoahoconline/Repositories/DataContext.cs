using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Webbankhoahoconline.Models;

namespace Webbankhoahoconline.Repositories
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<CartModel> Carts { get; set; }
        public DbSet<CartItemModel> CartItems { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderDetailModel> OrderDetails { get; set; }
        
        public DbSet<InstructorModel> Instructors { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }
        public DbSet<SliderModel> Sliders { get; set; }
        public DbSet<VideoModel> Videos { get; set; }
        public DbSet<ContactModel> Contacts { get; set; }
        public DbSet<CourseQuantityModel> CourseQuantities { get; set; } 

        public DbSet<StatisticalModel> Statisticals { get; set; }
        public DbSet<MomoInfoModel> MomoInfos { get; set; }
    }
}
