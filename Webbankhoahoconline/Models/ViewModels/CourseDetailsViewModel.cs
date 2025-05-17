using System.ComponentModel.DataAnnotations;

namespace Webbankhoahoconline.Models.ViewModels
{
    public class CourseDetailsViewModel
    {
        public CourseModel CourseDetails { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đánh giá của bạn")]
        public string Comment { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên của bạn")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập email của bạn")]
        public string Email { get; set; }
        public IEnumerable<VideoModel> Videos { get; set; }
    }
}
