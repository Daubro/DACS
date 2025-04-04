using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webbankhoahoconline.Models
{
    public class PaymentModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public UserModel User { get; set; }

        [Required]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public CourseModel Course { get; set; }

        [Required, Range(0, double.MaxValue, ErrorMessage = "Số tiền không hợp lệ")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Trạng thái thanh toán")]
        public string Status { get; set; } = "Pending"; // Pending, Completed, Failed

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}
