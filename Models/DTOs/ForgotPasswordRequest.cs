using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.DTOs
{
	public class ForgotPasswordRequest
	{
		[Required]
		[EmailAddress]
		[MaxLength(32)]
		public string Email { get; set; }
	}
}
