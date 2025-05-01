using System.ComponentModel.DataAnnotations;

namespace Webbankhoahoconline.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        public string Email { get; set; }
        [DataType(DataType.Password),Required(ErrorMessage ="Mật khẩu không được để trống")]
        public string Password { get; set; }     
    }
}
