using Microsoft.EntityFrameworkCore;
using Webbankhoahoconline.Models;


namespace Webbankhoahoconline.Repositories
{
    public class SeeData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();

            if (!_context.Instructors.Any())
            {
                _context.Instructors.AddRange(
                    new InstructorModel
                    {
                        Name = "Nguyễn Văn Anh",
                        Slug = "nguyen-van-anh", 
                        Email = "nguyenvana@example.com",
                        Bio = "Chuyên gia lập trình Web với hơn 10 năm kinh nghiệm.",
                        AvatarUrl = "nguyenvana.jpg",
                        CreatedAt = DateTime.UtcNow
                    },
                    new InstructorModel
                    {
                        Name = "Trần Thị Bin",
                        Slug = "tran-thi-bin", 
                        Email = "tranthib@example.com",
                        Bio = "Chuyên gia khoa học dữ liệu, giảng viên tại các trường đại học.",
                        AvatarUrl = "tranthib.jpg",
                        CreatedAt = DateTime.UtcNow
                    }
                );
                _context.SaveChanges();
            }


            // Lấy danh sách Instructors đã có trong database
            var instructorList = _context.Instructors.ToList();

            // Kiểm tra và thêm Category nếu chưa có
            if (!_context.Categories.Any())
            {
                _context.Categories.AddRange(
                    new CategoryModel
                    {
                        Name = "Web Development",
                        Slug = "web-development",
                        Description = "Học lập trình web",
                        Status = 1
                    },
                    new CategoryModel
                    {
                        Name = "Data Science",
                        Slug = "data-science",
                        Description = "Học khoa học dữ liệu",
                        Status = 1
                    }
                );
                _context.SaveChanges();
            }

            // Lấy danh sách Category đã có
            var webDevelopment = _context.Categories.FirstOrDefault(c => c.Slug == "web-development");
            var dataScience = _context.Categories.FirstOrDefault(c => c.Slug == "data-science");

            // Kiểm tra nếu chưa có Course nào thì thêm mới
            if (!_context.Courses.Any() && webDevelopment != null && dataScience != null)
            {
                _context.Courses.AddRange(
                    new CourseModel
                    {
                        Name = "Lập trình ASP.NET Core",
                        Slug = "asp-net-core",
                        Description = "Khóa học lập trình ASP.NET Core từ cơ bản đến nâng cao",
                        Price = 1500,
                        Category = webDevelopment,
                        Status = 1,
                        ImageUrl = "aspnetcore.jpg",
                        InstructorId = instructorList[0].Id // Instructor đầu tiên
                    },
                    new CourseModel
                    {
                        Name = "Python cho Data Science",
                        Slug = "python-data-science",
                        Description = "Học Python ứng dụng trong khoa học dữ liệu",
                        Price = 1200,
                        Category = dataScience,
                        Status = 1,
                        ImageUrl = "python.jpg",
                        InstructorId = instructorList[1].Id // Instructor thứ hai
                    }
                );
                _context.SaveChanges();
            }
        }
    }
}
