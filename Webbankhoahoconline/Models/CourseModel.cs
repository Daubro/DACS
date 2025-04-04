﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webbankhoahoconline.Models
{
    public class CourseModel
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(4, ErrorMessage = "Tên khóa học phải có ít nhất 4 ký tự")]
        public string Name { get; set; }

        [Required, MinLength(10, ErrorMessage = "Mô tả khóa học phải có ít nhất 10 ký tự")]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là số dương")]
        public decimal Price { get; set; }

        [Required, Url(ErrorMessage = "Yêu cầu nhập đúng định dạng URL")]
        public string ImageUrl { get; set; }

        [Required]
        public string Slug { get; set; }

        public int Status { get; set; } // 0: Ẩn, 1: Hiển thị

        // Liên kết với danh mục khóa học
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public CategoryModel Category { get; set; }
        public int InstructorId { get; set; }
        [ForeignKey("InstructorId")]
        public InstructorModel Instructor { get; set; }

    }
}
