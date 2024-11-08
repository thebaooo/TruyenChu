using System.ComponentModel.DataAnnotations;

namespace TruyenChu.Data
{
    public class StoryCategory
    {
        public int StoryId { get; set; }       // Khóa ngoại đến bảng Story
        public Story? Story { get; set; }       // Navigation property đến Story

        public int CategoryId { get; set; }    // Khóa ngoại đến bảng Category
        public Category? Category { get; set; } // Navigation property đến Category
    }
}
