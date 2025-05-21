using System.ComponentModel.DataAnnotations;

namespace Webbankhoahoconline.Models
{
    public class CourseQuantityModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu không được bỏ trống số lượng sản phẩm")]
        public int Quantity { get; set; }
        public long CourseId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
