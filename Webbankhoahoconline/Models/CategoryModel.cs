﻿using System.ComponentModel.DataAnnotations;

namespace Webbankhoahoconline.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        [Required,MinLength(4,ErrorMessage ="Yêu cầu nhập tên Danh mục")]
        public string Name { get; set; }
        [Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả Danh mục")]
        public string Description { get; set; }
        
        public string Slug { get; set; }
        public int Status { get; set; }
        public List<CourseModel>? Courses { get; set; }
    }
}
