using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruyenChu.Data;
using TruyenChu.ViewModels;

namespace TruyenChu.Controllers
{
    public class StoryController : Controller
    {
        private readonly AppDbContext _context;

        public StoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int id, int page = 1)
        {
            // Lấy thông tin truyện dựa trên ID
            var story = await _context.Stories
                .Include(s => s.Chapters)      // Tải kèm các chương
                .Include(s => s.Author)
                .Include(s => s.StoryCategories) // Lấy thông tin về StoryCategories
                .ThenInclude(sc => sc.Category) // Tải kèm thông tin tác giả
                .FirstOrDefaultAsync(s => s.StoryId == id); // Tìm truyện theo ID

            if (story == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy truyện
            }

            int pageSize = 2;

            // Lấy số lượng chương của truyện
            var totalChapters = story.Chapters.Count();
            var totalPages = (int)Math.Ceiling(totalChapters / (double)pageSize);

            // Phân trang chương
            var chapters = story.Chapters
                .OrderBy(c => c.ChapterNumber)  // Sắp xếp theo ChapterNumber
                .Skip((page - 1) * pageSize)   // Bỏ qua các chương trước đó
                .Take(pageSize)                // Lấy số chương theo trang
                .ToList();

            


            // Tạo ViewModel để truyền dữ liệu
            var viewModel = new StoryViewModel
            {
                Story = story,
                Chapters = chapters,
                CurrentPage = page,
                TotalPages = totalPages,
                ViewCount = story.ViewCount,
                IsFull = story.IsFull,
            };

            return View(viewModel); // Trả về view với ViewModel đã phân trang
        }

        

    }
}
