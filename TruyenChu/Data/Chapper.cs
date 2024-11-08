using System.ComponentModel.DataAnnotations;

namespace TruyenChu.Data
{
    public class Chapter
    {
        [Key]
        public int ChapterId { get; set; } // Khóa chính
        public int StoryId { get; set; }    // Khóa ngoại đến Story
        public Story? Story { get; set; }     // Navigation property đến Story

        [Required]
        public string? Title { get; set; }    // Tiêu đề chương
        public string? Content { get; set; }   // Nội dung chương
        public int ChapterNumber { get; set; } // Số thứ tự chương
        public DateTime CreateDate { get; set; } = DateTime.Now; // Ngày tạo chương
    }
}
