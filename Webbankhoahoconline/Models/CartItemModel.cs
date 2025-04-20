using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webbankhoahoconline.Models
{
    public class CartItemModel
    {
        [Key]
        public int Id { get; set; }

        public long CourseId { get; set; }
        public string CourseName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public decimal TotalPrice => Price * Quantity;

        public string ImageUrl { get; set; }

        public CartItemModel() { }

        public CartItemModel(CourseModel course)
        {
            CourseId = course.Id;
            CourseName = course.Name;
            Price = course.Price;
            ImageUrl = course.ImageUrl;
            Quantity = 1;
        }
    }
}
