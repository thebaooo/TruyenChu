using System.ComponentModel.DataAnnotations;

namespace TruyenChu.ViewModels
{
    public class RegisterViewModels
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
    }
}
