using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.DTOs.Auth
{
	public class LoginRequest
	{
		[Required]
		[MinLength(8)]
		[MaxLength(16)]
		public string Username { get; set; }

		[Required]
		[MinLength(8)]
		[MaxLength(24)]
		public string Password { get; set; }
	}
}
