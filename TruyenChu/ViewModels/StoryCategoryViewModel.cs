using TruyenChu.Data;

namespace TruyenChu.ViewModels
{
    public class StoryCategoryViewModel
    {
        public Category Category { get; set; }
        public List<Story> Stories { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

}
