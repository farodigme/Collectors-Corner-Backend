using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.DTOs.Account
{
	public class UpdateEmailRequest
	{
		[Required]
		[MaxLength(32)]
		public string Email { get; set; }
	}
}
