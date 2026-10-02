using System.ComponentModel.DataAnnotations;

namespace MinimalAPIArtemio.Models
{
	public class UserModel
	{
		public int UserId { get; set; }

		[Required]
		public string User { get; set; }

		public string Password { get; set; }

		[EmailAddress]
		public string Email { get; set; }

		public string Type { get; set; }
	}
}
