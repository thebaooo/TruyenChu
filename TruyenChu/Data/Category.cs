using System.ComponentModel.DataAnnotations;

namespace TruyenChu.Data
{
    public class Category
    {
        // Khóa chính
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string? CategoryName { get; set; }         // Tên thể loại
        public string? CategoryDescription { get; set; }

        // Navigation property để truy cập các truyện thuộc thể loại này
        public ICollection<StoryCategory>? StoryCategories { get; set; }
    }
}
