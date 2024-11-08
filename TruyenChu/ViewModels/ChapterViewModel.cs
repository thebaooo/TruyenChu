using TruyenChu.Data;

namespace TruyenChu.ViewModels
{
    public class ChapterViewModel
    {
        public int CurrentChapterId { get; set; }
        public int CurrentChapterNumber { get; set; }

        public string CurrentTitle { get; set; }
        public string Content { get; set; }

        public int? PreviousChapterId { get; set; }
        public string PreviousTitle { get; set; }

        public int? NextChapterId { get; set; }
        public string NextTitle { get; set; }
        public List<Chapter> AllChapters { get; set; } = new List<Chapter>();
    }
}
