using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models
{
	public class LoginModel
	{
		[Required]
		public string? Email { get; set; }

		[Required]
		public string? Password { get; set; }
	}
}
