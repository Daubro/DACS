﻿using Microsoft.EntityFrameworkCore;
using Webbankhoahoconline.Models;

namespace Webbankhoahoconline.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<CartModel> Carts { get; set; }
        public DbSet<CartItemModel> CartItems { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderDetailModel> OrderDetails { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<InstructorModel> Instructors { get; set; }


    }
}
