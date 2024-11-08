using System.ComponentModel.DataAnnotations;

namespace TruyenChu.Data
{
	public class Role
	{
		[Key]
		public int RoleId { get; set; }
		[Required]
		public string RoleName { get; set; }
		public ICollection<User>? Users { get; set; } 

	}
}
