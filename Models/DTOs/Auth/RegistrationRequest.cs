using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.DTOs.Auth
{
	public class RegistrationRequest
	{
		[Required]
		[MinLength(8)]
		[MaxLength(16)]
		public string Username { get; set; }

		[Required]
		[EmailAddress]
		[MaxLength(32)]
		public string Email { get; set; }

		[Required]
		[MinLength(8)]
		[MaxLength(24)]
		public string Password { get; set; }
	}
}
