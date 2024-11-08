using System.ComponentModel.DataAnnotations;

namespace TruyenChu.Data
{
    public class Author
    {
        [Key]
        // Khóa chính
        public int AuthorId { get; set; }
        [Required]   
        public string? Name { get; set; }          // Tên tác giả
        public string? Bio { get; set; }           // Tiểu sử tác giả

        // Navigation property để truy cập các truyện của tác giả
        public ICollection<Story>? Stories { get; set; }
    }
}
