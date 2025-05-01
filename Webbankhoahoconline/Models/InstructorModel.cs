using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webbankhoahoconline.Models
{
    public class InstructorModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên giảng viên phải có ít nhất 4 ký tự")]
        [Display(Name = "Tên giảng viên")]
        public string Name { get; set; }

        [Required, EmailAddress(ErrorMessage = "Yêu cầu nhập đúng định dạng email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required( ErrorMessage = "Bắt buộc nhập mô tả")]
        [Display(Name = "Mô tả")]
        public string Bio { get; set; }

        [Required, Url(ErrorMessage = "Yêu cầu nhập đúng định dạng URL")]
        [Display(Name = "Ảnh đại diện")]
        public string AvatarUrl { get; set; }
        public string Slug { get; set; }

        [Display(Name = "Ngày tham gia")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Liên kết với khóa học
        public ICollection<CourseModel> Courses { get; set; } = new List<CourseModel>();
    }
}
