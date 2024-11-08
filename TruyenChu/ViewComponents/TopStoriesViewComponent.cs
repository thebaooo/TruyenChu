using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruyenChu.Data;
using System.Threading.Tasks;
using System.Linq;

namespace TruyenChu.ViewComponents
{
    public class TopStoriesViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public TopStoriesViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var topStories = await _context.Stories
                .Include(s => s.StoryCategories)
                .ThenInclude(sc => sc.Category)
                .OrderByDescending(s => s.ViewCount)
                .Take(10)
                .ToListAsync();

            return View(topStories);
        }

    }
}
