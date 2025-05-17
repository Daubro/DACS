using System.ComponentModel.DataAnnotations.Schema;

namespace Webbankhoahoconline.Models
{
    public class VideoModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public DateTime UploadDate { get; set; }

        // Khóa ngoại liên kết với CourseModel
        public long CourseId { get; set; }
        [ForeignKey("CourseId")]
        public CourseModel Course { get; set; }
    }
}
