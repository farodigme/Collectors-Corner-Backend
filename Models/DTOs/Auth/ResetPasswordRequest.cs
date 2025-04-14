using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.DTOs.Auth
{
	public class ResetPasswordRequest
	{
		[Required]
		[Length(64,64)]
		public string ResetToken { get; set; }

		[Required]
		[Length(8,16)]
		public string NewPassword { get; set; }

		[Required]
		[Length(8, 16)]
		public string ConfirmPassword { get; set; }
	}
}
