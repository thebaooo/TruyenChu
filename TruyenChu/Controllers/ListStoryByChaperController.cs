using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruyenChu.Data;

namespace TruyenChu.Controllers
{
    public class ListStoryByChaperController : Controller
    {
        private readonly AppDbContext _context;

        public ListStoryByChaperController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? minChapters, int? maxChapters)
        {
            // Lấy danh sách truyện từ cơ sở dữ liệu
            var stories = _context.Stories.Include(s => s.Chapters).AsQueryable();

            // Lọc theo số lượng chương nếu các tham số được truyền vào
            if (minChapters.HasValue && maxChapters.HasValue)
            {
                stories = stories.Where(s => s.Chapters.Count >= minChapters.Value && s.Chapters.Count <= maxChapters.Value);
            }
            else if (minChapters.HasValue)
            {
                stories = stories.Where(s => s.Chapters.Count >= minChapters.Value);
            }
            else if (maxChapters.HasValue)
            {
                stories = stories.Where(s => s.Chapters.Count <= maxChapters.Value);
            }

            // Truyền các giá trị minChapters và maxChapters vào ViewBag hoặc ViewData để gửi đến view
            ViewBag.MinChapters = minChapters;
            ViewBag.MaxChapters = maxChapters;

            // Cập nhật thông tin để hiển thị "Dưới 500" hoặc "Trên 1000"
            if (maxChapters.HasValue && maxChapters.Value == 100)
            {
                ViewBag.MaxChapters = "Dưới 100";
            }
            else if (minChapters.HasValue && minChapters.Value == 1000)
            {
                ViewBag.MinChapters = "Trên 1000";
            }

            // Lấy danh sách truyện đã lọc
            var result = await stories.ToListAsync();
            return View(result);
        }

    }
}
