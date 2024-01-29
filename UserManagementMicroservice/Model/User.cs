

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagementMicroservice.Model
{
	[Table("usersData")]
	public class User
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }

		[Required]
		[Column("name")]
		public string Username { get; set; } = string.Empty;

		[Required]
		[Column("password")]
		public string Password { get; set; } = string.Empty;

		[Required]
		[Column("role")]
		public string Role { get; set; } = string.Empty;

	}
}
