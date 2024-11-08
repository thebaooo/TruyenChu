using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TruyenChu.Data
{
	public class User
	{
		[Key]
		public int UserId { get; set; }
		[Required]
		public string UserName { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		public string Email { get; set; }
		public int RoleId { get; set; }
		public Role? Role {  get; set; } 
	}
}
