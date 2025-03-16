using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webbankhoahoconline.Models
{
    public class CourseModel
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(4, ErrorMessage = "Yêu cầu nhập tên khóa học")]
        public string Name { get; set; }

        [Required, MinLength(10, ErrorMessage = "Yêu cầu nhập mô tả khóa học")]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là số dương")]
        public decimal Price { get; set; }

        [Required, Url(ErrorMessage = "Yêu cầu nhập đúng định dạng URL")]
        public string? ImageUrl { get; set; }

        [Required]
        public string Slug { get; set; }

        public int Status { get; set; }

        // Khóa ngoại liên kết với CategoryModel
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public CategoryModel Category { get; set; }
    }
}
