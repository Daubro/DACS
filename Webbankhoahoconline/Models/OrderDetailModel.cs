using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webbankhoahoconline.Models
{
    public class OrderDetailModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public OrderModel Order { get; set; }

        [Required]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public CourseModel Course { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
