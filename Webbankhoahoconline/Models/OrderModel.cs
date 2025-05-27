using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Webbankhoahoconline.Models;

public class OrderModel
{
    [Key]
    public int Id { get; set; }
    public string OrderCode { get; set; }
    public string UserName { get; set; }
    public string UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public int Status { get; set; } 
    public string? PaymentMethod { get; set; } 
    public List<OrderDetailModel> OrderDetails { get; set; } = new List<OrderDetailModel>();
}
