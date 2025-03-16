using System.ComponentModel.DataAnnotations;

namespace Webbankhoahoconline.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(4, ErrorMessage = "Tên đăng nhập phải có ít nhất 4 ký tự")]
        public string Username { get; set; }

        [Required, EmailAddress(ErrorMessage = "Yêu cầu nhập đúng định dạng email")]
        public string Email { get; set; }

        [Required, MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; } = "Customer"; // "Admin" hoặc "Customer"
    }
}
