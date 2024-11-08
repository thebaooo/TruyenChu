using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruyenChu.Data;

namespace TruyenChu.ViewComponents
{
    public class HotStoriesViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public HotStoriesViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var hotStories = await _context.Stories
                .Where(s => s.IsHot)  // Lọc các truyện có IsHot == true
                .Include(s => s.Chapters)
                .ToListAsync();
            return View(hotStories);
        }
    }
}
