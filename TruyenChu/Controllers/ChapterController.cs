using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruyenChu.Data;
using TruyenChu.ViewModels;

namespace TruyenChu.Controllers
{
    public class ChapterController : Controller
    {
        private readonly AppDbContext _context;

        public ChapterController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int id)
        {
            var currentChapter = await _context.Chapters.FindAsync(id);
            if (currentChapter == null) return NotFound();
            // Tìm truyện của chương này
            var story = await _context.Stories
                .FirstOrDefaultAsync(s => s.StoryId == currentChapter.StoryId);

            if (story != null)
            {
                // Tăng lượt xem của truyện
                story.ViewCount++;
                _context.Update(story);
                await _context.SaveChangesAsync();
            }

            var previousChapter = await _context.Chapters
                .Where(c => c.StoryId == currentChapter.StoryId && c.ChapterNumber < currentChapter.ChapterNumber)
                .OrderByDescending(c => c.ChapterNumber)
                .FirstOrDefaultAsync();

            var nextChapter = await _context.Chapters
                .Where(c => c.StoryId == currentChapter.StoryId && c.ChapterNumber > currentChapter.ChapterNumber)
                .OrderBy(c => c.ChapterNumber)
                .FirstOrDefaultAsync();

            // Lấy tất cả chương của truyện
            var allChapters = await _context.Chapters
                .Where(c => c.StoryId == currentChapter.StoryId)
                .OrderBy(c => c.ChapterNumber)
                .ToListAsync();

            var viewModel = new ChapterViewModel
            {
                CurrentChapterId = currentChapter.ChapterId,
                CurrentChapterNumber = currentChapter.ChapterNumber,
                CurrentTitle = currentChapter.Title,
                Content = currentChapter.Content,
                PreviousChapterId = previousChapter?.ChapterId,
                PreviousTitle = previousChapter?.Title,
                NextChapterId = nextChapter?.ChapterId,
                NextTitle = nextChapter?.Title,
                AllChapters = allChapters,
            };

            return View(viewModel);
        }


    }
}
