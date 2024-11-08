using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruyenChu.Data;

namespace TruyenChu.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("Admin/Chapters")]
    public class ChaptersController : Controller
    {
        private readonly AppDbContext _context;

        public ChaptersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Chapters/Index/5
        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index(int storyId)
        {
            var story = await _context.Stories
                .Include(s => s.Chapters) // Bao gồm các chương
                .FirstOrDefaultAsync(s => s.StoryId == storyId);

            if (story == null)
            {
                return NotFound();
            }

            return View(story); // Trả về view với model là story
        }

        // GET: Chapters/Create
        [Route("Create/{storyId}")]
        public async Task<IActionResult> Create(int storyId)
        {
            // Lấy truyện tương ứng từ cơ sở dữ liệu
            var story = await _context.Stories.FindAsync(storyId);

            // Kiểm tra nếu truyện không tồn tại
            if (story == null)
            {
                return NotFound();
            }

            // Gán tiêu đề truyện vào ViewBag
            ViewBag.StoryTitle = story.StoryTitle;

            var chapter = new Chapter
            {
                StoryId = storyId // Gán ID truyện
            };

            return View(chapter);
        }

        // POST: Chapters/Create
        [HttpPost("Create/{storyId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int storyId, Chapter chapter)
        {
            // Kiểm tra nếu truyện tồn tại
            var storyExists = await _context.Stories.AnyAsync(s => s.StoryId == storyId);
            if (!storyExists)
            {
                ModelState.AddModelError("", "Truyện không tồn tại.");
            }

            if (ModelState.IsValid)
            {
                chapter.StoryId = storyId; // Gán lại StoryId
                chapter.CreateDate = DateTime.Now; // Gán ngày tạo
                _context.Chapters.Add(chapter);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Chapters", new { storyId });
            }

            // Nếu ModelState không hợp lệ, gán lại ViewBag.StoryTitle
            ViewBag.StoryTitle = await _context.Stories.Where(s => s.StoryId == storyId).Select(s => s.StoryTitle).FirstOrDefaultAsync();

            return View(chapter);
        }

        // GET: Chapters/Edit/5
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter == null)
            {
                return NotFound();
            }
            return View(chapter);
        }

        // POST: Chapters/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Chapter chapter)
        {
            if (id != chapter.ChapterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chapter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChapterExists(chapter.ChapterId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction("Index", "Chapters", new { storyId = chapter.StoryId });

            }
            return View(chapter);
        }





        // GET: Chapters/Details/5
        [Route("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var chapter = await _context.Chapters
                .Include(c => c.Story) // Bao gồm thông tin truyện
                .FirstOrDefaultAsync(c => c.ChapterId == id);

            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter); // Trả về view với model là chapter
        }

        // GET: Chapters/Delete/5
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var chapter = await _context.Chapters
                .Include(c => c.Story)
                .FirstOrDefaultAsync(m => m.ChapterId == id);
            if (chapter == null)
            {
                return NotFound();
            }
            return View(chapter);
        }

        // POST: Chapters/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter != null)
            {
                _context.Chapters.Remove(chapter);
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound(); // Nếu chương không tìm thấy, trả về lỗi NotFound
            }

            return RedirectToAction("Index", "Chapters", new { storyId = chapter.StoryId });
        }


        private bool ChapterExists(int id)
        {
            return _context.Chapters.Any(e => e.ChapterId == id);
        }
    }
}
