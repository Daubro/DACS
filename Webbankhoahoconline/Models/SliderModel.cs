using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Webbankhoahoconline.Repositories.Validation;

namespace Webbankhoahoconline.Models
{
    public class SliderModel
    {
        public int Id { get; set; }
        [Required( ErrorMessage = "Không được bỏ trống tên slider")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống mô tả slider")]
        public string Description { get; set; }
        public int? Status { get; set; }
        public string ImageUrl { get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile? ImageFile { get; set; }
    }
}
