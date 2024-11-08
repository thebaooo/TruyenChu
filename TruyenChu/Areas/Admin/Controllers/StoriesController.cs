using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TruyenChu.Data;

namespace TruyenChu.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("Admin/Stories")]
    public class StoriesController : Controller
    {
        private readonly AppDbContext _context;

        public StoriesController(AppDbContext context)
        {
            _context = context;
        }

        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var stories = await _context.Stories
                .Include(s => s.Author)
                .Include(s => s.StoryCategories) // Lấy thông tin về StoryCategories
                .ThenInclude(sc => sc.Category)
                .Include(s => s.Chapters) // Lấy thông tin thể loại
                .ToListAsync();
            return View(stories);
        }

        // GET: Stories/Details/5
        [Route("Index/{id?}")]
        public async Task<IActionResult> Details(int id)
        {
            var story = await _context.Stories
                .Include(s => s.Author)
                .Include(s => s.StoryCategories)
                    .ThenInclude(sc => sc.Category)
                .Include(s => s.Chapters) // Lấy thông tin các chương
                .FirstOrDefaultAsync(m => m.StoryId == id);

            if (story == null)
            {
                return NotFound();
            }

            return View(story);
        }

        // GET: Stories/Create
        [Route("Create")]
        public IActionResult Create()
        {
            // Lấy danh sách các tác giả và thể loại để hiển thị trong dropdown
            ViewBag.Authors = new SelectList(_context.Authors, "AuthorId", "Name");
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Stories/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Story story, int[]? selectedCategories, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                // Gán trạng thái từ form
                story.IsHot = Request.Form["IsHot"] == "true"; // Nếu checkbox được chọn, giá trị sẽ là "true"
                story.IsNew = Request.Form["IsNew"] == "true";
                story.IsFull = Request.Form["IsFull"] == "true";
                // Lưu thông tin về truyện
                story.CreateDate = DateTime.Now;
                story.ViewCount = 0; // Khởi tạo số lượt xem là 0

                // Xử lý việc tải lên hình ảnh
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Tạo tên ngẫu nhiên cho hình ảnh
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    var filePath = Path.Combine(uploadPath, uniqueFileName);

                    // Tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    story.ImagePath = $"/images/{uniqueFileName}"; // Lưu đường dẫn hình ảnh
                }

                

                // Thêm truyện vào cơ sở dữ liệu
                _context.Add(story);
                await _context.SaveChangesAsync();

                // Thêm mối quan hệ giữa truyện và thể loại
                if (selectedCategories != null)
                {
                    foreach (var categoryId in selectedCategories)
                    {
                        var storyCategory = new StoryCategory
                        {
                            StoryId = story.StoryId,
                            CategoryId = categoryId
                        };
                        _context.StoryCategories.Add(storyCategory);
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            // Nếu không hợp lệ, lấy lại danh sách tác giả và thể loại
            ViewBag.Authors = new SelectList(_context.Authors, "AuthorId", "Name", story.AuthorId);
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(story);
        }

        // GET: Stories/Edit/5
        [Route("Edit/{id?}")]
        public async Task<IActionResult> Edit(int id)
        {
            var story = await _context.Stories
                .Include(s => s.StoryCategories)
                    .ThenInclude(sc => sc.Category)
                .FirstOrDefaultAsync(m => m.StoryId == id);

            if (story == null)
            {
                return NotFound();
            }

            // Lấy danh sách các tác giả và thể loại để hiển thị trong dropdown
            ViewBag.Authors = new SelectList(_context.Authors, "AuthorId", "Name", story.AuthorId);
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");

            // Tạo danh sách để lưu trữ các CategoryId đã chọn
            ViewBag.SelectedCategories = story.StoryCategories.Select(sc => sc.CategoryId).ToArray();

            return View(story);
        }

        // POST: Stories/Edit/5
        [HttpPost]
        [Route("Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Story story, int[]? selectedCategories, IFormFile? imageFile)
        {
            if (id != story.StoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Gán trạng thái từ form
                    story.IsHot = Request.Form["IsHot"] == "true"; // Nếu checkbox được chọn, giá trị sẽ là "true"
                    story.IsNew = Request.Form["IsNew"] == "true";
                    story.IsFull = Request.Form["IsFull"] == "true";
                    // Lấy thông tin câu chuyện cũ từ database
                    var existingStory = await _context.Stories.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.StoryId == id);

                    if (existingStory == null)
                    {
                        return NotFound();
                    }

                    // Kiểm tra ảnh mới
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Xóa ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(existingStory.ImagePath))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingStory.ImagePath.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Tạo tên ngẫu nhiên cho hình ảnh mới
                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                        var filePath = Path.Combine(uploadPath, uniqueFileName);

                        // Tạo thư mục nếu chưa tồn tại
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        story.ImagePath = $"/images/{uniqueFileName}"; // Cập nhật đường dẫn hình ảnh mới
                    }
                    else
                    {
                        // Giữ nguyên đường dẫn ảnh nếu không có ảnh mới
                        story.ImagePath = existingStory.ImagePath;
                    }

                    // Cập nhật các trường khác của câu chuyện
                    _context.Update(story);

                    // Xóa và cập nhật mối quan hệ giữa truyện và thể loại
                    var existingCategories = await _context.StoryCategories
                        .Where(sc => sc.StoryId == story.StoryId)
                        .ToListAsync();
                    _context.StoryCategories.RemoveRange(existingCategories);

                    if (selectedCategories != null)
                    {
                        foreach (var categoryId in selectedCategories)
                        {
                            var storyCategory = new StoryCategory
                            {
                                StoryId = story.StoryId,
                                CategoryId = categoryId
                            };
                            _context.StoryCategories.Add(storyCategory);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoryExists(story.StoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            // Nếu không hợp lệ, lấy lại danh sách tác giả và thể loại
            ViewBag.Authors = new SelectList(_context.Authors, "AuthorId", "Name", story.AuthorId);
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(story);
        }

        // GET: Categories/Delete/5
        [Route("Delete/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            var story = await _context.Stories
                 .Include(s => s.Author)
                 .FirstOrDefaultAsync(m => m.StoryId == id);

            if (story == null)
            {
                return NotFound();
            }

            return View(story);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("Delete/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var story = await _context.Stories.FindAsync(id);
            if (story != null)
            {
                // Xóa ảnh trong thư mục nếu có
                if (!string.IsNullOrEmpty(story.ImagePath))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", story.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                // Xóa mối quan hệ trong bảng StoryCategory trước
                var storyCategories = _context.StoryCategories.Where(sc => sc.StoryId == id);
                _context.StoryCategories.RemoveRange(storyCategories);

                // Xóa truyện
                _context.Stories.Remove(story);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        // Phương thức kiểm tra tồn tại câu chuyện
        private bool StoryExists(int id)
        {
            return _context.Stories.Any(e => e.StoryId == id);
        }
    }
}
