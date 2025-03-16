using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.DTOs
{
	public class ResetPasswordRequest
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
