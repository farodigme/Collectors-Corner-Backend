using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.DTOs
{
	public class ResetPasswordRequest
	{
		[Required]
		public string Token { get; set; }

		[Required]
		[MaxLength(16)]
		public string NewPassword { get; set; }

		[Required]
		[MaxLength(16)]
		public string ConfirmPassword { get; set; }
	}
}
