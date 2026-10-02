using System.ComponentModel.DataAnnotations;

namespace MinimalAPIArtemio.Models
{
	public class LoginModel
	{
        public string UserId { get; set; }

        [Required]
        public string User { get; set; }

        [Required]
		public string Password { get; set; }

        public int SeasonId { get; set; }
	}
}
