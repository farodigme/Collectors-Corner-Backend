namespace Collectors_Corner_Backend.Models.DTOs
{
	public class AuthResponse
	{
		public bool Success { get; set; }
		public string? Error { get; set; }
		public string? AccessToken { get; set; }
		public DateTime? AccessTokenExpires { get; set; }
		public string? RefreshToken { get; set; }
		public DateTime? RefreshTokenExpires { get; set; } = DateTime.UtcNow;
	}
}
