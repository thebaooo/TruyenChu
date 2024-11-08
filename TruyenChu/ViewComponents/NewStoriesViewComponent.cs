using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruyenChu.Data;

namespace TruyenChu.ViewComponents
{
    public class NewStoriesViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public NewStoriesViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var newStories = await _context.Stories.Where(s => s.IsNew)
                .Include(s => s.Author)
                .Include(s => s.StoryCategories) // Lấy thông tin về StoryCategories
                .ThenInclude(sc => sc.Category)
                .Include(s => s.Chapters) // Lấy thông tin thể loại
                .ToListAsync();

            return View(newStories);
        }
    }
}
