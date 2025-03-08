namespace Collectors_Corner_Backend.Models.DTOs
{
	public class AuthResponse
	{
		public bool Success { get; set; }
		public string? Error { get; set; }
		public string JWToken { get; set; }
		public string RefreshToken { get; set; }
	}
}
