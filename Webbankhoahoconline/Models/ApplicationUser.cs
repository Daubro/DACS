using Microsoft.AspNetCore.Identity;

namespace Webbankhoahoconline.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Thêm thuộc tính tùy chỉnh nếu muốn
        public string FullName { get; set; }
        public string RoleId { get; set; }
    }
}
