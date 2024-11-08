using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TruyenChu.Data;
using TruyenChu.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TruyenChu.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AccountController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModels model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _mapper.Map<User>(model);
                    user.RoleId = 1;
                    _context.Add(user);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Default");
                }
            } catch (Exception ex) { }
            
            return View();
        }

        [HttpGet]
        public IActionResult Login(string? ReturnUrl) 
        { 
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl) 
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var user = _context.Users
                    .Include(r => r.Role)
                    .SingleOrDefault(u =>
                    u.UserName == model.UserName && u.Password == model.Password);
                if (user == null) {
                    ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
                } 
                else
                {
                    var claims = new List<Claim> {
                                new Claim(ClaimTypes.Email, user.Email),
								new Claim(ClaimTypes.Name, "Admin"),

								//claim - role động
								new Claim(ClaimTypes.Role, user.Role.RoleName)
                            };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(claimsPrincipal);

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return Redirect("/");
                    }
                }
            }
            return View();
        }   

		[Authorize]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return Redirect("/");
		}
	}
}
