using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruyenChu.Data;

namespace TruyenChu.ViewComponents
{
    public class CategoriesDropdownViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public CategoriesDropdownViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName)
        {
            var categories = await _context.Categories.ToListAsync();
            return View(viewName, categories); // Trả về view với danh sách các thể loại
        }
    }
}
