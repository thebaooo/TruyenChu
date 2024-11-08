using TruyenChu.Data;

namespace TruyenChu.ViewModels
{
    public class StoryViewModel
    {
        public Story Story { get; set; }
        public List<Chapter> Chapters { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool IsFull { get; set; }
        public int ViewCount { get; set; } = 0;

    }
}
