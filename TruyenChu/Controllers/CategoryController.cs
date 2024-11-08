using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruyenChu.Data;
using TruyenChu.ViewModels;

namespace TruyenChu.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int id, int page = 1)
        {
            var category = await _context.Categories
                .Include(s => s.StoryCategories)
                .ThenInclude(sc => sc.Story)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound(); // Nếu không tìm thấy category
            }
            // Lấy các truyện thuộc category, với phân trang
            int pageSize = 1;  // Số lượng truyện mỗi trang
            int totalStories = category.StoryCategories.Count();
            int totalPages = (int)Math.Ceiling(totalStories / (double)pageSize);

            var stories = category.StoryCategories
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(sc => sc.Story)
                .ToList();

            var viewModel = new StoryCategoryViewModel
            {
                Category = category,
                Stories = stories,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }
    }
}
