using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webbankhoahoconline.Models
{
    public class OrderDetailModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string OrderCode { get; set; }
        public int? OrderId { get; set; }  // thêm dòng này, phải trùng tên với ForeignKey
        public long CourseId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("OrderId")]
        public OrderModel Order { get; set; }
        [ForeignKey("CourseId")]
        public CourseModel Course { get; set; }
    }
}
