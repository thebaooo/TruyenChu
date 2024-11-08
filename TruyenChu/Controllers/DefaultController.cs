using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruyenChu.Data;

namespace TruyenChu.Controllers
{
    public class DefaultController : Controller
    {
        private readonly AppDbContext _context;

        public DefaultController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var stories = await _context.Stories.ToListAsync();
            return View(stories);
        }

        // Action xử lý tìm kiếm
        public async Task<IActionResult> Search(string key_word)
        {
            if (string.IsNullOrEmpty(key_word))
            {
                return View(); // Trả về view với danh sách rỗng nếu không có từ khóa
            }

            var searchResults = await _context.Stories
                .Where(s => s.StoryTitle.Contains(key_word) || s.Description.Contains(key_word))
                .ToListAsync();
            ViewBag.KeyWord = key_word;
            return View(searchResults);
        }
    }
}
