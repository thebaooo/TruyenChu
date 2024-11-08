using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruyenChu.Data;

namespace TruyenChu.ViewComponents
{
    public class FullStoriesViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public FullStoriesViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Lấy các truyện có trạng thái 'IsFull' là true
            var fullStories = await _context.Stories
                .Where(s => s.IsFull)  // Lọc các truyện có IsFull == true
                .ToListAsync();

            return View(fullStories);
        }
    }
}
