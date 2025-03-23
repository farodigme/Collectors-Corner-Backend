using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.DTOs.Token
{
	public class JWToken
	{
		[Required]
		public string Token { get; set; }

		[Required]
		public DateTime ExpiresAt { get; set; }
	}
}
