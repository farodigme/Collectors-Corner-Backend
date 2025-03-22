using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.DTOs.Auth
{
	public class RefreshTokenRequest
	{
		[Required]
		[Length(360,512)]
		public string AccessToken { get; set; }

		[Required]
		[Length(64,64)]
		public string RefreshToken { get; set; }
	}
}
