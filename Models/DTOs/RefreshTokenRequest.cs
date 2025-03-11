using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.DTOs
{
	public class RefreshTokenRequest
	{
		[Required]
		public string AccessToken { get; set; }

		[Required]
		public string RefreshToken { get; set; }
	}
}
