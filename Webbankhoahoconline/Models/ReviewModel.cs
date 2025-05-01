using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webbankhoahoconline.Models
{
    public class ReviewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public CourseModel Course { get; set; }

        [Required, Range(1, 5, ErrorMessage = "Điểm đánh giá phải từ 1 đến 5")]
        public int Rating { get; set; }

        [Required, MinLength(5, ErrorMessage = "Nội dung đánh giá ít nhất 5 ký tự")]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
