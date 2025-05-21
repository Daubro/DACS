using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Webbankhoahoconline.Repositories.Validation;

namespace Webbankhoahoconline.Models
{
    public class ContactModel
    {
        [Key]
        [Required(ErrorMessage="Yêu cầu nhập tiêu đề website")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập địa chỉ website")]
        public string Map { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập Email website")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập phone")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập thông tin liên hệ ")]
        public string Description { get; set; }
        public string LogoImg { get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile? ImageFile { get; set; }
    }
}
