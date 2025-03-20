using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.DTOs.Auth
{
	public class RegistrationRequest
	{
		[Required]
		[MaxLength(16)]
		public string Username { get; set; }

		[Required]
		[MaxLength(32)]
		public string Email { get; set; }

		[Required]
		[MaxLength(24)]
		public string Password { get; set; }
	}
}
