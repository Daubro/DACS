using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webbankhoahoconline.Models
{
    public class EnrollmentModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public CourseModel Course { get; set; }

        [Display(Name = "Ngày đăng ký")]
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    }
}
