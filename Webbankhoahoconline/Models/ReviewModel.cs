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
        public long CourseId { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập đánh giá")]
        public string Comment { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tên người đánh giá")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập email người đánh giá")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        public string Star { get; set; } 
        [ForeignKey("CourseId")]
        public CourseModel Course { get; set; }
    }
}
