using System.ComponentModel.DataAnnotations;

namespace Webbankhoahoconline.Models.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string UserName { get; set; }
        
        [DataType(DataType.Password), Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }

    }
}
