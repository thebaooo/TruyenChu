using System.ComponentModel.DataAnnotations;

namespace TruyenChu.Data
{
    public class Story
    {
        [Key]
        public int StoryId { get; set; }
        [Required]
        public string? StoryTitle { get; set; }
        [Required]  
        public int AuthorId { get; set; } // Khóa ngoại đến Author
        public Author? Author { get; set; }    // Navigation property đến Author
        public string? Description { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public int ViewCount { get; set; } = 0;
        public string? ImagePath { get; set; }
        public bool IsHot { get; set; } = true;    // Đánh dấu truyện "Hot"
        public bool IsNew { get; set; } = true;     // Đánh dấu truyện "Mới"
        public bool IsFull { get; set; } = true;   // Đánh dấu truyện "Đầy đủ"

        public ICollection<StoryCategory>?  StoryCategories { get; set; }
        public ICollection<Chapter>? Chapters { get; set; } // Thêm thuộc tính điều hướng đến Chapter
    }
}
