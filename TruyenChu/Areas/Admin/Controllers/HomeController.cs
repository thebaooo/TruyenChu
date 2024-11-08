using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TruyenChu.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
	[Authorize(Roles = "Admin")]
	[Route("Admin/Home")]
    public class HomeController : Controller
    {
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
