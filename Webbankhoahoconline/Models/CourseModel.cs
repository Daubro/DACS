using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Webbankhoahoconline.Repositories.Validation;

namespace Webbankhoahoconline.Models
{
    public class CourseModel
    {
        [Key]
        public long Id { get; set; }

        [Required, MinLength(4, ErrorMessage = "Tên khóa học phải có ít nhất 4 ký tự")]
        public string Name { get; set; }

        [Required, MinLength(10, ErrorMessage = "Mô tả khóa học phải có ít nhất 10 ký tự")]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là số dương")]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; } 
        [NotMapped]
        [FileExtension]
        public IFormFile? ImageFile { get; set; } 
        public string Slug { get; set; }

        public int Status { get; set; } 

        public ReviewModel Reviews { get; set; } 

        // Liên kết với danh mục khóa học
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public CategoryModel Category { get; set; }
        public int InstructorId { get; set; }
        [ForeignKey("InstructorId")]
        public InstructorModel Instructor { get; set; }

    }
}
