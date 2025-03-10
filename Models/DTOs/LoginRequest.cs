using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.DTOs
{
	public class LoginRequest
	{
		[Required]
		[MaxLength(50)]
		public string? Username { get; set; }

		[Required]
		public string? Password { get; set; }
	}
}
