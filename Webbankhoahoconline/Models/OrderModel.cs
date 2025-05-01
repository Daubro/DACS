using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Webbankhoahoconline.Models;

public class OrderModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } // sửa từ int → string

    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Required]
    public decimal TotalAmount { get; set; }

    public List<OrderDetailModel> OrderDetails { get; set; } = new List<OrderDetailModel>();
}
